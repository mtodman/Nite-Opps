using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;

namespace Nite_Opps
{
    public class ASCOMCamera : ASCOM.DriverAccess.Camera
    {
        #region Constructor
        clsSharedData sd;
        clsForms form;

        public ASCOMCamera(string CamID, ref clsSharedData d, ref clsForms f)
            : base(CamID)
        {
            this.ExposureTimer.Elapsed += new ElapsedEventHandler(this.ExposureTimerTick);
            ExposureTimer.Interval = 100;
            ExposureTimer.Enabled = false;
            sd = d;
            form = f;
        }

        #endregion

        #region Private Properties

        //private Array imgArray;
        private bool isLightFrame;
        //private Bitmap b_map;
        Timer ExposureTimer = new System.Timers.Timer();
        decimal min = 65535;
        decimal max = 0;
        int width, height;


        #endregion


        #region Public Properties

        // Width of the exposure just taken
        public int ExposureWidth
        {
            get
            {
                return width;
            }
        }

        // Height of the exposure just taken
        public int ExposureHeight
        {
            get
            {
                return height;
            }
        }


        // minimum pixel value of the exposure just taken
        public decimal Min
        {
            get
            {
                return min;
            }
        }


        // Maximum pixel value of the exposure just taken
        public decimal Max
        {
            get
            {
                return max;
            }
        }



        #endregion


        #region Private Methods

        private void ExposureTimerTick(object sender, EventArgs e)
        {
            if (ImageReady)
            {
                writetolog("Downloading & Processing Exposure\r\n", true);
                sd.imgArray = (Array)ImageArray;
                ExposureTimer.Enabled = false;
                createBitMap();
                write(sd, form);
                writetolog("Exposure Complete\r\n", true);
            }
        }

        
        private void createBitMap()
        {
            unsafe
            {
                try
                {
                    int resultValue;
                    int bitmapvalue;
                    width = sd.imgArray.GetLength(0);
                    height = sd.imgArray.GetLength(1);
                    int numPixels = width * height;

                    sd.theImage.bmp = new Bitmap(width, height);
                    BitmapData bmData = sd.theImage.bmp.LockBits(new Rectangle(0, 0, width, height),
                            ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    int stride = bmData.Stride;
                    System.IntPtr Scan0 = bmData.Scan0;
                    long totalPix = 0;
                    int numpixels = width * height;

                    byte* p = (byte*)(void*)Scan0;
                    int nOffset = stride - width * 3;


                    //Get the minimum and maximum pixel values from the image array
                    fixed (int* pArr = (int[,])sd.imgArray)
                    {
                        var pA = pArr;
                        min = 65535;
                        max = 0;
                        for (int i = 0; i < sd.imgArray.GetLength(0) * sd.imgArray.GetLength(1); i++)
                        {
                            int v = *pA;
                            min = Math.Min(min, v);
                            max = Math.Max(max, v);
                            pA++;
                        }
                    }
                    int divider = (int)Math.Ceiling((double)(max - min) / 256);

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            resultValue = (int)sd.imgArray.GetValue(x, y);
                            totalPix = totalPix + resultValue;
                            if (resultValue < min)
                                resultValue = 0;
                            else if (resultValue > max)
                                resultValue = (int)max;
                            bitmapvalue = (int)Math.Floor((double)resultValue / divider);
                            if (bitmapvalue > 255)
                                bitmapvalue = 255;
                            p[0] = p[1] = p[2] = (byte)bitmapvalue;
                            p += 3;
                        }
                        p += nOffset;
                    }


                    sd.theImage.bmp.UnlockBits(bmData);
                }
                catch
                {
                    
                }
            }
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Takes an image with the Imanging Camera using the provided binning, imagetype (Light, Bias, Flat or Dark) at the specified duration
        /// </summary>
        /// <param name="binning"></param>
        /// <param name="imageType"></param>
        /// <param name="duration"></param>
        public void takeImage(int binning, string imageType, double duration)
        {
            switch (imageType)
            {
                case "Light":
                case "Bias":
                case "Flat":
                isLightFrame = true;
                break;
                case "Dark":
                isLightFrame = false;
                break;
            }

                StartExposure(duration, isLightFrame);
                ExposureTimer.Enabled = true;
        }

        public void takeTheImage(int binning, string imageType, double duration)
        {
            var d = new imageData
            {
                BINNING = binning, IMAGETYPE = imageType, DURATION = duration
            };
            var t1 = new System.Threading.Thread(t_takeImage);
            t1.Start(d);
        }

        private void t_takeImage(object o)
        {
            imageData d = (imageData)o;

            switch (d.IMAGETYPE)
            {
                case "Light":
                case "Bias":
                case "Flat":
                    isLightFrame = true;
                    break;
                case "Dark":
                    isLightFrame = false;
                    break;
            }

            StartExposure(d.DURATION, isLightFrame);
            writetolog("Taking exposure\r\n", true);
            ExposureTimer.Enabled = true;

        }

        #endregion

        // Code to create the event which sends a bitmap to 
        // the Main form's subscribed client.
        //public delegate void PicReady(Bitmap b_map, clsSharedData d, clsForms f);
        public delegate void PicReady(clsSharedData d, clsForms f);
        public event PicReady write;


        // Code to create the event which sends a string message to 
        // the Main form's Status Box (txtStatusBox).
        public delegate void LogHandler(string message, bool displaytime);
        public event LogHandler Log;
        public void writetolog(string message, bool displaytime)
        {
            Log(message, displaytime);
        }


        public struct imageData
        {
            public int BINNING;
            public string IMAGETYPE;
            public double DURATION;
        }
    }
}

