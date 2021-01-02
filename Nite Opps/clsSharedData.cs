using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Nite_Opps
{
    public class clsSharedData
    {
        public string APPLICATION_NAME = "Nite_Ops";
        public bool isScopeConnected = false;
        public bool isImagingCamConnected = false;
        public bool isGuidingCamConnected = false;
        public bool isObjectDbConnected = false;
        public bool isAstrometryEngineConnected = false;
        public bool isFilterWheelConnected = false;
        public bool isPHDConnected = false;
        public bool parkStatus;
        public double targetObjectAlt, targetObjectAz, targetObjectRA, targetObjectDec;
        public double solvedRA, solvedDec;
        public string tempSolveFileDir = System.Windows.Forms.Application.StartupPath + "\\data";
        public string tempSolveFile = System.Windows.Forms.Application.StartupPath + "\\data\\solvefile.fits";
        public int imagingCamExposureDurationMins, imagingCamExposureDurationSecs, imagingCamExposureDurationMs, imagingCamBinning;
        public bool imagingCamAutoRepeat, imagingCamCoolerOn;
        public string imagingCamFrameType;
        public int imagingCamCoolerTargetTemp;
        public string plate_solve_error_msg = null; //Used to temporarily hold the failure reason for a failed palte solve
        public bool slewcomplete = true; //used to advise if the mount is currently slewing.
        public bool imagecomplete = false; //used to advise when the image has been written to the picturebox
        public bool solvecomplete = false; //used to advise when the solve has been completed
        public int sscount; //used to count the number of plate solves in a "slew & solve" command.
        public double solveError; //contains the solve error (arc secs)


        public string[,] ImagingArray = new string[15, 6];
        public int intArraySize = 15;

        public Hashtable filtertable = new Hashtable();
        public int filternum;

        public objectDb O;
        public ASCOMCamera ImagingCam, GuidingCam;
        public ASCOMFilterWheel FW;
        public imageInfo theImage = new imageInfo();
        public fitsHeader F;
        //public phd G;
        public clsPhd G;
        public Array imgArray;


        public void readCameraValues()
        {
            frmImaging i = new frmImaging();

            imagingCamExposureDurationMins = Convert.ToInt16(i.numMins.Value);
            imagingCamExposureDurationSecs = Convert.ToInt16(i.numSecs.Value);
            imagingCamExposureDurationMs = Convert.ToInt16(i.numMs.Value);
            imagingCamBinning = Convert.ToInt16(i.numBinning.Value);
            imagingCamAutoRepeat = i.chkAutoRepeat.Checked;
            imagingCamCoolerOn = i.chkCoolerOn.Checked;
            imagingCamCoolerTargetTemp = Convert.ToInt16(i.numTargetTemp.Value);

            if (i.rbFlatFrame.Checked) imagingCamFrameType = "Flat";
            else if (i.rbDarkFrame.Checked) imagingCamFrameType = "Dark";
            else if (i.rbBiasFrame.Checked) imagingCamFrameType = "Bias";
            else imagingCamFrameType = "Light";
        }

        public void setCameraValues()
        {
            frmImaging i = new frmImaging();

            ImagingCam.BinX = ImagingCam.BinY = (short)imagingCamBinning;
            ImagingCam.NumX = ImagingCam.CameraXSize;
            ImagingCam.NumY = ImagingCam.CameraYSize;
            if (imagingCamCoolerOn)
            {
                ImagingCam.CoolerOn = true;
                ImagingCam.SetCCDTemperature = (int)i.numTargetTemp.Value;
            }
        }
    }
}
