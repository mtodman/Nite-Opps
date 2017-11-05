using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Nite_Opps
{
    
    public class imageInfo
    {
        public Bitmap bmp;
        public string RA, Dec, Alt, Az;
        public string frameType;
        public double arcSecsPerPixX, arcSecsPerPixY;
        public double dRA, dDec;
        public int BinX, BinY;
        public int MaxADU;
        public string Description;
        public double LastExposureDuration;
        public string LastExposureStartTime;
        public double PixelSizeX, PixelSizeY;
        public int StartX, StartY;
        public decimal Min, Max;
        public double CCDTemperature;
        public bool CanSetCCDTemperature;
        public double SetCCDTemperature;
        public string objectName;
    }
}

