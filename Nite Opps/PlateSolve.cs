using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PinPoint;

namespace Nite_Opps
{
    
    public class PlateSolve
    {
        PinPoint.Plate P;
        string PP_app;
        string response;
        clsSharedData sd;


        #region Constructor

        public PlateSolve(string app, ref clsSharedData d)
        {
            sd = d;
            P = new PinPoint.Plate();
            PP_app = "PinPoint";
        }
        #endregion

        AstroCalculations.solvedCoordinates coords;

        #region public properties
        double rightascension;
        public double RightAscension
        {
            get
            {
                return P.RightAscension;
            }
        }


        double declination;
        public double Declination
        {
            get
            {
                return P.Declination;
            }
        }

        #endregion

        /// <summary>
        /// Check to see if Pinpoint is connected
        /// </summary>
        /// <returns></returns>
        public bool checkStatus()
        {
            response = P.MaxSolveTime.ToString();
            if (response != "") return true;
            else return false;
        }


        public bool solve(string filePath, imageInfo bm, string strGSClocation)
        {
            try
            {
                P.AttachFITS(filePath);
                P.ArcsecPerPixelHoriz = bm.arcSecsPerPixX;
                P.ArcsecPerPixelVert = bm.arcSecsPerPixY;
                P.RightAscension = bm.dRA;
                P.Declination = bm.dDec;
                P.Catalog = (PinPoint.CatalogType)3;  // Corrected GSC
                P.CatalogPath = strGSClocation;
                bool pRresult = P.Solve();
                sd.solvedRA = P.RightAscension;
                sd.solvedDec = P.Declination;
                P.DetachFITS();
                return pRresult;
            }
            catch (System.Exception msg)
            {
                sd.plate_solve_error_msg = msg.Message;
                P.DetachFITS();
                return false;
                // Need to add code here to cater for specific exceptions (timeout, etc).
            }
            
            return false; //assume that there was a problem with either of the above solves
        }

        public double calcPointingError(double solvedRA, double solvedDec, double targetObjectRA, double targetObjectDec)
        {
            double ra_error = Math.Abs(solvedRA - targetObjectRA);
            double dec_error = Math.Abs(solvedDec - targetObjectDec);
            double error = Math.Sqrt((ra_error * ra_error) + (dec_error * dec_error)); //Woohoo. I finally get to Use Pythagoras.
            return error;
        }

    }
}

