using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner.Utilities; // Locationwp function
using MissionPlanner.GCSViews;  // FlightPlanner.cs function
using GMap.NET;   // PointLatLng function
using System.Threading;

namespace MissionPlanner.Auto_Guide
{
    public partial class Auto_Guide : Form
    {
        FlightData FlightData = new FlightData();
        Locationwp gotohere = new Locationwp();
        FlightPlanner FlightPlanner = new FlightPlanner();
        PointLatLngAlt WP = new PointLatLngAlt();
        public delegate void mydalegate();
        public mydalegate change_text;
        static bool threadrun;
        int wpnumber = 0;
        int Awpnumber = 0;
        int Bwpnumber = 0;
        int Cwpnumber = 0;
        int Aendwpnum = 0;
        int Bendwpnum = 0;
        int Cendwpnum = 0;
        int wpcount = 0;
        bool firstwp = false;
        bool Afirstwp = false;
        bool Bfirstwp = false;
        bool Cfirstwp = false;
        static bool newwp = false;
        static bool Anewwp = false;
        static bool Bnewwp = false;
        static bool Cnewwp = false;
        internal MAVLinkInterface Acopter = null;
        internal MAVLinkInterface Bcopter = null;
        internal MAVLinkInterface Ccopter = null;
        public Auto_Guide(FlightPlanner flightplannerform)
        {
            InitializeComponent();
            this.Tag = flightplannerform;
            bindingSource1.DataSource = MainV2.Comports;

            Connection_Select.DataSource = bindingSource1;
            change_text = new mydalegate(Button_start_end);
        }

        public void Button_start_end()
        {
            Button_start.Text = "Start";
        }
        public void getwpdata(double Lat, double Lng, double Alt, int RowCount) //取得從Flight Planner 回傳的經緯度資料
        {
            WP.Lat = Lat;
            WP.Lng = Lng;
            WP.Alt = Alt;
            wpcount = RowCount;
        }

        private void Connection_Select_SelectedIndexChanged(object sender, EventArgs e) //已連線之連結選單
        {
            foreach (var port in MainV2.Comports)
            {
                if (port.ToString() == Connection_Select.Text)
                {
                    MainV2.comPort = port;
                }
            }
        }

        private void SetA_Click(object sender, EventArgs e)
        {
            Acopter = MainV2.comPort;
        }

        private void SetB_Click(object sender, EventArgs e)
        {
            Bcopter = MainV2.comPort;
        }

        private void SetC_Click(object sender, EventArgs e)
        {
            Ccopter = MainV2.comPort;
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            wpnumber = 0;
            wpcount = 0;
            firstwp = false;
            newwp = false;

            Awpnumber = 0;
            Bwpnumber = 0;
            Cwpnumber = 0;
            Aendwpnum = 0;
            Bendwpnum = 0;
            Cendwpnum = 0;
            wpcount = 0;
            Afirstwp = false;
            Bfirstwp = false;
            Cfirstwp = false;
            Anewwp = false;
            Bnewwp = false;
            Cnewwp = false;
        }

        private void Button_start_Click(object sender, EventArgs e)
        {
            if (threadrun == true)
            {
                threadrun = false;
                Button_start.Text = Strings.Start;
                return;
            }

            //if (SwarmInterface != null)
            {
                new System.Threading.Thread(Mainthread) { IsBackground = true }.Start();
                Button_start.Text = Strings.Stop;

                ((FlightPlanner)this.Tag).Getwpdata(Awpnumber);
                Bwpnumber = wpcount / 3;
                Cwpnumber = (wpcount / 3) * 2;
                Aendwpnum = Bwpnumber;
                Bendwpnum = Cwpnumber;
                Cendwpnum = wpcount;
            }
        }

        private void Mainthread()
        {
            threadrun = true;

            while (threadrun)
            {
                new System.Threading.Thread(ACopter) { IsBackground = true }.Start();
                new System.Threading.Thread(BCopter) { IsBackground = true }.Start();
                new System.Threading.Thread(CCopter) { IsBackground = true }.Start();
                //ACopter();
                //BCopter();
                //CCopter();
                System.Threading.Thread.Sleep(450);
                if (Awpnumber == Aendwpnum && Bwpnumber == Bendwpnum && Cwpnumber == Cendwpnum)
                {
                    threadrun = false;
                    Invoke(change_text);
                }
            }
        }
        private void ACopter()
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                CustomMessageBox.Show(Strings.PleaseConnect, Strings.ERROR);
                return;
            }
            while (Afirstwp == false && Awpnumber == 0)
            {
                ((FlightPlanner)this.Tag).Getwpdata(Awpnumber);
                gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                //gotohere.alt = MainV2.comPort.MAV.GuidedMode.z; // back to m
                gotohere.alt = (float)WP.Alt;
                gotohere.lat = WP.Lat;
                gotohere.lng = WP.Lng;
                try
                {
                    Acopter.setGuidedModeWP(gotohere);
                    Afirstwp = true;
                }
                catch (Exception ex)
                {
                    Acopter.giveComport = false;
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
                return;
            }

            while (Awpnumber != 0 && Awpnumber < Aendwpnum && Anewwp == true)
            {
                ((FlightPlanner)this.Tag).Getwpdata(Awpnumber);
                gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                //gotohere.alt = MainV2.comPort.MAV.GuidedMode.z; // back to m
                gotohere.alt = (float)WP.Alt;
                gotohere.lat = WP.Lat;
                gotohere.lng = WP.Lng;

                try
                {
                    Acopter.setGuidedModeWP(gotohere);
                    Anewwp = false;

                }
                catch (Exception ex)
                {
                    Acopter.giveComport = false;
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
                return;
            }
            float wpdistance = Acopter.MAV.cs.wp_dist;
            while (Afirstwp == true && wpdistance <= 2 && wpdistance > 0 && Awpnumber < Aendwpnum)
            {
                Awpnumber++;
                Anewwp = true;
                break;
            }
            while (Awpnumber == Aendwpnum)
            {
                Acopter.setMode("RTL");
                break;
            }
        }
        private void BCopter()
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                CustomMessageBox.Show(Strings.PleaseConnect, Strings.ERROR);
                return;
            }
            while (Bfirstwp == false && Bwpnumber != 0)
            {
                ((FlightPlanner)this.Tag).Getwpdata(Bwpnumber);
                gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                //gotohere.alt = MainV2.comPort.MAV.GuidedMode.z; // back to m
                gotohere.alt = (float)WP.Alt;
                gotohere.lat = WP.Lat;
                gotohere.lng = WP.Lng;
                try
                {
                    Bcopter.setGuidedModeWP(gotohere);
                    Bfirstwp = true;
                }
                catch (Exception ex)
                {
                    Bcopter.giveComport = false;
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
                return;
            }

            while (Bwpnumber != 0 && Bwpnumber < Bendwpnum && Bnewwp == true)
            {
                ((FlightPlanner)this.Tag).Getwpdata(Bwpnumber);
                gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                //gotohere.alt = MainV2.comPort.MAV.GuidedMode.z; // back to m
                gotohere.alt = (float)WP.Alt;
                gotohere.lat = WP.Lat;
                gotohere.lng = WP.Lng;

                try
                {
                    Bcopter.setGuidedModeWP(gotohere);
                    Bnewwp = false;

                }
                catch (Exception ex)
                {
                    Bcopter.giveComport = false;
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
                return;
            }
            float wpdistance = Bcopter.MAV.cs.wp_dist;
            while (Bfirstwp == true && wpdistance <= 2 && wpdistance > 0 && Bwpnumber < Bendwpnum)
            {
                Bwpnumber++;
                Bnewwp = true;
                break;
            }
            while (Bwpnumber == Bendwpnum)
            {
                Bcopter.setMode("RTL");
                break;
            }
        }
        private void CCopter()
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                CustomMessageBox.Show(Strings.PleaseConnect, Strings.ERROR);
                return;
            }
            while (Cfirstwp == false && Cwpnumber != 0)
            {
                ((FlightPlanner)this.Tag).Getwpdata(Cwpnumber);
                gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                //gotohere.alt = MainV2.comPort.MAV.GuidedMode.z; // back to m
                gotohere.alt = (float)WP.Alt;
                gotohere.lat = WP.Lat;
                gotohere.lng = WP.Lng;
                try
                {
                    Ccopter.setGuidedModeWP(gotohere);
                    Cfirstwp = true;
                }
                catch (Exception ex)
                {
                    Ccopter.giveComport = false;
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
                return;
            }

            while (Cwpnumber != 0 && Cwpnumber < Cendwpnum && Cnewwp == true)
            {
                ((FlightPlanner)this.Tag).Getwpdata(Cwpnumber);
                gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                //gotohere.alt = MainV2.comPort.MAV.GuidedMode.z; // back to m
                gotohere.alt = (float)WP.Alt;
                gotohere.lat = WP.Lat;
                gotohere.lng = WP.Lng;

                try
                {
                    Ccopter.setGuidedModeWP(gotohere);
                    Cnewwp = false;

                }
                catch (Exception ex)
                {
                    Ccopter.giveComport = false;
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
                return;
            }
            float wpdistance = Ccopter.MAV.cs.wp_dist;
            while (Cfirstwp == true && wpdistance <= 2 && wpdistance > 0 && Cwpnumber < Cendwpnum)
            {
                Cwpnumber++;
                Cnewwp = true;
                break;
            }
            while (Cwpnumber == Cendwpnum)
            {
                Ccopter.setMode("RTL");
                break;
            }
        }

        private void Armed_All_Click(object sender, EventArgs e)
        {
            try
            {
                if (Acopter.MAV.cs.armed)
                    if (CustomMessageBox.Show("Are you sure you want to Disarm?", "Disarm?", MessageBoxButtons.YesNo) !=
                        DialogResult.Yes)
                        return;
                if (Bcopter.MAV.cs.armed)
                    if (CustomMessageBox.Show("Are you sure you want to Disarm?", "Disarm?", MessageBoxButtons.YesNo) !=
                        DialogResult.Yes)
                        return;
                if (Ccopter.MAV.cs.armed)
                    if (CustomMessageBox.Show("Are you sure you want to Disarm?", "Disarm?", MessageBoxButtons.YesNo) !=
                        DialogResult.Yes)
                        return;
                bool Aans = Acopter.doARM(!Acopter.MAV.cs.armed);
                bool Bans = Bcopter.doARM(!Acopter.MAV.cs.armed);
                bool Cans = Ccopter.doARM(!Acopter.MAV.cs.armed);
                if (Aans == false)
                    CustomMessageBox.Show(Strings.ErrorRejectedByMAV, Strings.ERROR);
                if (Bans == false)
                    CustomMessageBox.Show(Strings.ErrorRejectedByMAV, Strings.ERROR);
                if (Cans == false)
                    CustomMessageBox.Show(Strings.ErrorRejectedByMAV, Strings.ERROR);
            }
            catch
            {
                CustomMessageBox.Show(Strings.ErrorNoResponce, Strings.ERROR);
            }
        }

        private void Takeoff_All_Click(object sender, EventArgs e)
        {
            Acopter.setMode("GUIDED");
            Bcopter.setMode("GUIDED");
            Ccopter.setMode("GUIDED");
            try
            {
                Acopter.doCommand(MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 5);  //A機起飛高度5米
                Bcopter.doCommand(MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 5);  //B機起飛高度5米
                Ccopter.doCommand(MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 5);  //C機起飛高度5米
            }
            catch
            {
                CustomMessageBox.Show(Strings.CommandFailed, Strings.ERROR);
            }
        }

        private void Armed_and_Takeoff_All_Click(object sender, EventArgs e)
        {
            try
            {
                Acopter.setMode("Stabilize");
                Bcopter.setMode("Stabilize");
                Ccopter.setMode("Stabilize");
                if (Acopter.doARM(true))
                {
                    Acopter.setMode("GUIDED");

                    //Thread.Sleep(300);

                    Acopter.doCommand(MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 5);
                }
                if (Bcopter.doARM(true))
                {
                    Bcopter.setMode("GUIDED");

                    //Thread.Sleep(300);

                    Bcopter.doCommand(MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 5);
                }
                if (Ccopter.doARM(true))
                {
                    Ccopter.setMode("GUIDED");

                    //Thread.Sleep(300);

                    Ccopter.doCommand(MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 10);
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.ToString());
            }
        }

        private void RTL_All_Click(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).Enabled = false;
                Acopter.setMode("RTL");
                Bcopter.setMode("RTL");
                Ccopter.setMode("RTL");
            }
            catch
            {
                CustomMessageBox.Show(Strings.CommandFailed, Strings.ERROR);
            }
            ((Button)sender).Enabled = true;
        }
    }
}
