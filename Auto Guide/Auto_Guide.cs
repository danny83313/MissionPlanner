using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner.Utilities; // Locationwp function
using MissionPlanner.GCSViews;  // FlightPlanner.cs FlightData.cs function
using GMap.NET;   // PointLatLng function
using System.Threading;

namespace MissionPlanner.Auto_Guide
{
    public partial class Auto_Guide : Form
    {
        FlightData FlightData = new FlightData();
        Locationwp gotohere = new Locationwp();
        FlightPlanner FlightPlanner = new FlightPlanner();
        public List<PointLatLngAlt> Apointlist = new List<PointLatLngAlt>(); //宣告Apointlist，接收FlightPlanner的A群List
        public List<PointLatLngAlt> Bpointlist = new List<PointLatLngAlt>(); //宣告Bpointlist，接收FlightPlanner的B群List
        public List<PointLatLngAlt> Cpointlist = new List<PointLatLngAlt>(); //宣告Cpointlist，接收FlightPlanner的C群List
        public List<PointLatLngAlt> Dpointlist = new List<PointLatLngAlt>(); //宣告Dpointlist，接收FlightPlanner的D群List
        public List<PointLatLngAlt> Epointlist = new List<PointLatLngAlt>(); //宣告Epointlist，接收FlightPlanner的E群List
        PointLatLngAlt WP = new PointLatLngAlt();
        public delegate void mydalegate();
        public mydalegate change_text;
        static bool threadrun;
        static bool Athread;
        static bool Bthread;
        static bool Cthread;
        static bool Dthread;
        static bool Ethread;
        static bool Aend;
        static bool Bend;
        static bool Cend;
        static bool Dend;
        static bool Eend;

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
        internal MAVLinkInterface Dcopter = null;
        internal MAVLinkInterface Ecopter = null;
        public Auto_Guide()
        //public Auto_Guide(FlightPlanner flightplannerform)
        {
            InitializeComponent();
            //this.Tag = flightplannerform;
            
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

        private void SetD_Click(object sender, EventArgs e)
        {
            Dcopter = MainV2.comPort;
        }

        private void SetE_Click(object sender, EventArgs e)
        {
            Ecopter = MainV2.comPort;
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            /*wpnumber = 0;
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
            Cnewwp = false;*/
        }

        private void Button_start_Click(object sender, EventArgs e)
        {
            if (threadrun == true)
            {
                threadrun = false;
                Athread = false;
                Bthread = false;
                Cthread = false;
                Dthread = false;
                Ethread = false;
                Button_start.Text = Strings.Start;
                return;
            }

            //if (SwarmInterface != null)
            {
                new System.Threading.Thread(Mainthread) { IsBackground = true }.Start();
                Button_start.Text = Strings.Stop;
                Athread = false;
                Bthread = false;
                Cthread = false;
                Dthread = false;
                Ethread = false;
                FlightPlanner.Receivelist(ref Apointlist, ref Bpointlist, ref Cpointlist, ref Dpointlist, ref Epointlist);
                /*((FlightPlanner)this.Tag).Getwpdata(Awpnumber);
                Bwpnumber = wpcount / 3;
                Cwpnumber = (wpcount / 3) * 2;
                Aendwpnum = Bwpnumber;
                Bendwpnum = Cwpnumber;
                Cendwpnum = wpcount;*/
            }
        }

        private void Mainthread()
        {
            threadrun = true;

            while (threadrun)
            {
                if (Athread == false && Acopter != null && Acopter.MAV.cs.mode != "Brake")
                {
                    Athread = true;
                    new System.Threading.Thread(ACopter) { IsBackground = true }.Start(); 
                }
                if (Bthread == false && Bcopter != null && Bcopter.MAV.cs.mode != "Brake")
                {
                    Bthread = true;
                    new System.Threading.Thread(BCopter) { IsBackground = true }.Start();
                }
                if (Cthread == false && Ccopter != null && Ccopter.MAV.cs.mode != "Brake")
                {
                    Cthread = true;
                    new System.Threading.Thread(CCopter) { IsBackground = true }.Start();
                }
                if (Dthread == false && Dcopter != null && Dcopter.MAV.cs.mode != "Brake")
                {
                    Dthread = true;
                    new System.Threading.Thread(DCopter) { IsBackground = true }.Start();
                }
                if (Ethread == false && Ecopter != null && Ecopter.MAV.cs.mode != "Brake")
                {
                    Ethread = true;
                    new System.Threading.Thread(ECopter) { IsBackground = true }.Start();
                }
                System.Threading.Thread.Sleep(450);
                /*if (Awpnumber == Aendwpnum && Bwpnumber == Bendwpnum && Cwpnumber == Cendwpnum)
                {
                    threadrun = false;
                    Invoke(change_text);
                }*/
                if(Aend==true && Bend==true && Cend==true && Dend==true && Eend==true)
                {
                    threadrun = false;
                    Athread = false;
                    Bthread = false;
                    Cthread = false;
                    Dthread = false;
                    Ethread = false;
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
            for (int i = 0; i < Apointlist.Count - 2; i++)
            {
                gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                gotohere.alt = (float)Apointlist[i + 1].Alt;  //List第0點是Home點
                gotohere.lat = Apointlist[i + 1].Lat;
                gotohere.lng = Apointlist[i + 1].Lng;
                try
                {
                    Acopter.setGuidedModeWP(gotohere);
                    Thread.Sleep(550);
                }
                catch (Exception ex)
                {
                    Acopter.giveComport = false;
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
                float wpdistance = Acopter.MAV.cs.wp_dist;
                do
                {
                    wpdistance = Acopter.MAV.cs.wp_dist;
                    Thread.Sleep(450);
                } while (wpdistance >= 2);
                if (Acopter.MAV.cs.mode == "Brake")
                    Athread = false;
                if (Athread == false)
                    break;
            }
            if (Athread == true)
            {
                Acopter.setMode("RTL");
                Aend = true;
            }
        }
        private void BCopter()
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                CustomMessageBox.Show(Strings.PleaseConnect, Strings.ERROR);
                return;
            }

            for (int i = 0; i < Bpointlist.Count - 2; i++)   
            {
                gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                gotohere.alt = (float)Bpointlist[i + 1].Alt;  //List第0點是Home點
                gotohere.lat = Bpointlist[i + 1].Lat;
                gotohere.lng = Bpointlist[i + 1].Lng;
                try
                {
                    Bcopter.setGuidedModeWP(gotohere);
                    Thread.Sleep(550);
                }
                catch (Exception ex)
                {
                    Bcopter.giveComport = false;
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
                float wpdistance = Bcopter.MAV.cs.wp_dist;
                do
                {
                    wpdistance = Bcopter.MAV.cs.wp_dist;
                    Thread.Sleep(450);
                } while (wpdistance >= 2);
                if (Bcopter.MAV.cs.mode == "Brake")
                    Bthread = false;
                if (Bthread == false)
                    break;
            }
            if (Bthread == true)
            {
                Bcopter.setMode("RTL");
                Bend = true;
            }
           
        }
        private void CCopter()
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                CustomMessageBox.Show(Strings.PleaseConnect, Strings.ERROR);
                return;
            }

            for (int i = 0; i < Cpointlist.Count - 2; i++)
            {
                gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                gotohere.alt = (float)Cpointlist[i + 1].Alt;  //List第0點是Home點
                gotohere.lat = Cpointlist[i + 1].Lat;
                gotohere.lng = Cpointlist[i + 1].Lng;
                try
                {
                    Ccopter.setGuidedModeWP(gotohere);
                    Thread.Sleep(550);
                }
                catch (Exception ex)
                {
                    Ccopter.giveComport = false;
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
                float wpdistance = Ccopter.MAV.cs.wp_dist;
                do
                {
                    wpdistance = Ccopter.MAV.cs.wp_dist;
                    Thread.Sleep(450);
                } while (wpdistance >= 2);
                if (Ccopter.MAV.cs.mode == "Brake")
                    Cthread = false;
                if (Cthread == false)
                    break;
            }
            if (Cthread == true)
            {
                Ccopter.setMode("RTL");
                Cend = true;
            }
          
        }
        private void DCopter()
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                CustomMessageBox.Show(Strings.PleaseConnect, Strings.ERROR);
                return;
            }

            for (int i = 0; i < Dpointlist.Count - 2; i++)
            {
                gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                gotohere.alt = (float)Dpointlist[i + 1].Alt;  //List第0點是Home點
                gotohere.lat = Dpointlist[i + 1].Lat;
                gotohere.lng = Dpointlist[i + 1].Lng;
                try
                {
                    Dcopter.setGuidedModeWP(gotohere);
                    Thread.Sleep(550);
                }
                catch (Exception ex)
                {
                    Dcopter.giveComport = false;
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
                float wpdistance = Dcopter.MAV.cs.wp_dist;
                do
                {
                    wpdistance = Dcopter.MAV.cs.wp_dist;
                    Thread.Sleep(450);
                } while (wpdistance >= 2);
                if (Dcopter.MAV.cs.mode == "Brake")
                    Dthread = false;
                if (Dthread == false)
                    break;
            }
            if (Dthread == true)
            {
                Dcopter.setMode("RTL");
                Dend = true;
            }
        }
        private void ECopter()
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                CustomMessageBox.Show(Strings.PleaseConnect, Strings.ERROR);
                return;
            }

            for (int i = 0; i < Epointlist.Count - 2; i++)
            {
                gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                gotohere.alt = (float)Epointlist[i + 1].Alt;  //List第0點是Home點
                gotohere.lat = Epointlist[i + 1].Lat;
                gotohere.lng = Epointlist[i + 1].Lng;
                try
                {
                    Ecopter.setGuidedModeWP(gotohere);
                    Thread.Sleep(550);
                }
                catch (Exception ex)
                {
                    Ecopter.giveComport = false;
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
                float wpdistance = Ecopter.MAV.cs.wp_dist;
                do
                {
                    wpdistance = Ecopter.MAV.cs.wp_dist;
                    Thread.Sleep(450);
                } while (wpdistance >= 2);
                if (Ecopter.MAV.cs.mode == "Brake")
                    Ethread = false;
                if (Ethread == false)
                    break;
            }
            if (Ethread == true)
            {
                Ecopter.setMode("RTL");
                Eend = true;
            }
        }
        private void Armed_All_Click(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var MAV in port.MAVlist)
                {
                    MAV.parent.doARM(MAV.sysid, MAV.compid, true);
                }
            }
          
        }

        private void Takeoff_All_Click(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var MAV in port.MAVlist)
                {
                    MAV.parent.setMode(MAV.sysid, MAV.compid, "GUIDED");

                    MAV.parent.doCommand(MAV.sysid, MAV.compid, MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 5);
                }
            }
            /*Acopter.setMode("GUIDED");
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
            }*/
        }

        private void Armed_and_Takeoff_All_Click(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var MAV in port.MAVlist)
                {
                    MAV.parent.doARM(MAV.sysid, MAV.compid, true);
                    MAV.parent.setMode(MAV.sysid, MAV.compid, "GUIDED");

                    MAV.parent.doCommand(MAV.sysid, MAV.compid, MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 5);
                }
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
