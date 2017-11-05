using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;


namespace Nite_Opps
{
    class IntArrayToBitmap
    {
        double average;
        double stddev;
        Bitmap b;
        int width, height;
        int min, max;


        #region Constructor
        public IntArrayToBitmap(int[,] a)
        {
            width = a.GetUpperBound(0) + 1;
            height = a.GetUpperBound(1) + 1;
            GetMinMax(a);
            Array2Bmp(a, width, height);
        }
        #endregion


        #region Properties
        // The BitMap
        public Bitmap bmp
        {
            get { return b; }
        }

        // Raw Image Width
        public int Width
        {
            get { return width; }
        }

        // Raw Image Height
        public int Height
        {
            get { return height; }
        }

        #endregion


        #region Methods
        void Array2Bmp(int[,] a, int width, int height)
        {
            int resultValue;
            int bitmapvalue;
            int numPixels = width * height;
            b = new Bitmap(width, height);
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int divider = (int)Math.Ceiling((double)(max - min) / 256);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            long totalPix = 0;
            int numpixels = width * height;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - width * 3;
                //short[] bb = null;
                for (int y = 0; y < height; y++)
                {
                    //bb = (short[])a[y];
                    for (int x = 0; x < width; x++)
                    {

                        // Begin testing algorithms
                        //resultValue = (int)(bb[x] + bZero);
                        resultValue = a[x, y];
                        totalPix = totalPix + resultValue;
                        if (resultValue < min)
                            resultValue = 0;
                        else if (resultValue > max)
                            resultValue = max;
                        bitmapvalue = (int)Math.Floor((double)resultValue / divider);
                        //bitmapvalue = (resultValue-minLimit)/div;
                        if (bitmapvalue > 255)
                            bitmapvalue = 255;
                        p[0] = p[1] = p[2] = (byte)bitmapvalue;
                        // End testing algorithms

                        p += 3;
                    }
                    p += nOffset;
                }
                average = totalPix / numpixels;

                // Calculate the Standard Deviation
                double diff = 0;
                double i = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // Begin algorithms
                        i = Math.Pow((a[x, y]) - average, 2.0);
                        diff += i;
                        // End algorithms
                    }
                }
                stddev = Math.Sqrt(diff / numPixels);

            }

            b.UnlockBits(bmData);
        }

        void GetMinMax(int[,] a)
        {
            min = 65535;
            max = 0;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {

                    // Begin testing algorithms
                    min = Math.Min(min, a[x, y]);
                    max = Math.Max(max, a[x, y]);
                    // End testing algorithms

                }
            }
        }
        //}
        #endregion
    }
}

