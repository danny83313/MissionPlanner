using System;

namespace PathProgram
{
    public class PathProgramming
    {
        /*public void math(double inlat, double inlng, int inalt, out double lat, out double lng, out int alt)
        {
           
                lat = 0.0005 + inlat;
                lng = 0.0005 + inlng;
                alt = 10;
            
        }*/
        public void math(double[] inlat, double[] inlng, int[] inalt, out double[] lat, out double[] lng, out int[] alt)
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
        }
    }
}
