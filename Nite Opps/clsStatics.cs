using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nite_Opps
{
    public static class clsStatics
    {
        public static bool imageComplete = false;
        public static bool[] taskNumComplete;
        public static bool isImagingRun = false;

        public static int imaging_camera_num_pixels_width, imaging_camera_num_pixels_height;
        public static double imaging_camera_pixel_width, imaging_camera_pixel_height;
        public static int guiding_camera_num_pixels_width, guiding_camera_num_pixels_height;
        public static double guiding_camera_pixel_width, guiding_camera_pixel_height;

        public static string phd_host = "localhost";
        public static int phd_port = 4400;



        /// <summary>
        /// Runs code in UI thread synchronously with BeginInvoke when required.
        /// </summary>
        /// <param name="code">the code, like "delegate { this.Text = "new text"; }"
        /// </param>
        static public void UIThread(this Control control, Action code)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(code);
                return;
            }
            code.Invoke();
        }

        public struct coordinates
        {
            public double ra;
            public double dec;
        };


        //Need to keep the MiniSAC object as static because it doesn't seem to close 
        // properly when disconnecting from and reconnecting to the objectDb.
        // Therefore, by keeping it static, the cat object doesn't nullify when disconnecting the 
        // MiniSAC objectdb.
        //public static Catalog cat;
        public static bool MiniSAC_OPENED = false;

        public enum eAlarmTypes
        {
            eStarLost,
            eExcursion
        }

        public struct AlarmInfo
        {
            public eAlarmTypes eType;
            public double Info1;
            public double Info2;
        }
    }
}
