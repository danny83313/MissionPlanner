using System;
using System.Collections.Generic;
using MissionPlanner.Utilities;

namespace PathProgram
{
    public class PathProgramming
    {
        public List<PointLatLngAlt> Allpointlist = new List<PointLatLngAlt>();
        public List<PointLatLngAlt> Outpointlist = new List<PointLatLngAlt>();
        /*public void math(double inlat, double inlng, int inalt, out double lat, out double lng, out int alt)
        {
           
                lat = 0.0005 + inlat;
                lng = 0.0005 + inlng;
                alt = 10;
            
        }*/
        /*public void math(double homelat ,double homelng,double[] inlat, double[] inlng, int[] inalt, out double[] lat, out double[] lng, out int[] alt)
        {
            lat = new double[inlat.Length];
            lng = new double[inlat.Length];
            alt = new int[inlat.Length];
            for (int i = 0; i < inlat.Length; i++)
            {
                lat[i] = 0.0005 + inlat[i];
                lng[i] = 0.0005 + inlng[i];
                alt[i] = 10;
            }
           
        }*/
        public void math(List<PointLatLngAlt> Allpointlist, ref List<PointLatLngAlt> Outpointlist)
        {

            for (int i = 0; i < Allpointlist.Count; i++)
            {
                Outpointlist[i].Lat = Allpointlist[i].Lat + 0.005;
                Outpointlist[i].Lng = Allpointlist[i].Lng + 0.005;
                Outpointlist[i].Alt = 0;
            }
        }

        
    }
}
