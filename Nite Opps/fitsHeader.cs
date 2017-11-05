using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using nom.tam.fits;
using nom.tam.util;

namespace Nite_Opps
{
    public class fitsHeader
    {

        public void saveImageToFits(string path, clsSharedData sd)
        {
            var imageData = (Array)ArrayFuncs.Flatten(sd.imgArray);
            const double bZero = 0;
            const double bScale = 1.0;
            if (sd.theImage.MaxADU <= 65535)
            {
                //bZero = 32768;
                //imageData = (ushort[])ArrayFuncs.ConvertArray(imageData, typeof(ushort));
            }
            int[] dims = ArrayFuncs.GetDimensions(sd.imgArray);
            //Array image = ArrayFuncs.Curl(imageData, dims);

            // put the image data in a basic HDU of the fits 
            BasicHDU imageHdu = FitsFactory.HDUFactory(ArrayFuncs.Curl(imageData, dims));

            // put the other data in the HDU
            imageHdu.AddValue("BZERO", bZero, "");
            imageHdu.AddValue("BSCALE", bScale, "");
            imageHdu.AddValue("DATAMIN", 0.0, "");      // should this reflect the actual data values
            imageHdu.AddValue("DATAMAX", sd.theImage.MaxADU, "pixel values above this level are considered saturated.");
            imageHdu.AddValue("INSTRUME", sd.theImage.Description, "");
            imageHdu.AddValue("EXPTIME", sd.theImage.LastExposureDuration, "duration of exposure in seconds.");
            imageHdu.AddValue("DATE-OBS", sd.theImage.LastExposureStartTime, "");
            imageHdu.AddValue("XPIXSZ", sd.theImage.PixelSizeX * sd.theImage.BinX, "physical X dimension of the sensor's pixels in microns"); //  (present only if the information is provided by the camera driver). Includes binning.
            imageHdu.AddValue("YPIXSZ", sd.theImage.PixelSizeY * sd.theImage.BinY, "physical Y dimension of the sensor's pixels in microns"); //  (present only if the information is provided by the camera driver). Includes binning.
            imageHdu.AddValue("XBINNING", sd.theImage.BinX, "");
            imageHdu.AddValue("YBINNING", sd.theImage.BinY, "");
            imageHdu.AddValue("XORGSUBF", sd.theImage.StartX, "subframe origin on X axis in binned pixels");
            imageHdu.AddValue("YORGSUBF", sd.theImage.StartY, "subframe origin on Y axis in binned pixels");
            imageHdu.AddValue("CBLACK", (double)sd.theImage.Min, "");
            imageHdu.AddValue("CWHITE", (double)sd.theImage.Max, "");
            imageHdu.AddValue("SWCREATE", "Nite Ops", "string indicating the software used to create the file");
            imageHdu.AddValue("OBJCTRA", sd.theImage.RA, "Approximate Right Ascension of image centre");
            imageHdu.AddValue("OBJCTDEC", sd.theImage.Dec, "Approximate Declination of image centre");
            imageHdu.AddValue("OBJCTALT", sd.theImage.Alt, "Approximate Altitude of image centre");
            imageHdu.AddValue("OBJCTAZ", sd.theImage.Az, "Approximate Azimuth of image centre");


            // extensions as specified by SBIG
            try
                {
                    imageHdu.AddValue("CCD_TEMP", sd.theImage.CCDTemperature, "sensor temperature in degrees C");  // TODO sate this at the start of exposure . Absent if temperature is not available.
                }
            catch (Exception)
                {
                    imageHdu.Info();
                }
            if (sd.theImage.CanSetCCDTemperature) imageHdu.AddValue("SET-TEMP", sd.theImage.SetCCDTemperature, "CCD temperature setpoint in degrees C");
            if (sd.theImage.objectName != "")
                {
                    imageHdu.AddValue("OBJECT", sd.theImage.objectName, "The name of the object");
                }
            else
                {
                    imageHdu.AddValue("OBJECT", "Unknown", "The name of the object");
                }
            imageHdu.AddValue("TELESCOP", Properties.Settings.Default.imaging_telescope, "Telescope used to acquire this image");        // user-entered information about the telescope used.
            imageHdu.AddValue("OBSERVER", Properties.Settings.Default.your_name, "Name of the observer");        // user-entered information; the observer’s name.

            //DARKTIME – dark current integration time, if recorded. May be longer than exposure time.
            imageHdu.AddValue("IMAGETYP", sd.theImage.frameType + " Frame", "Type of image");
            //ISOSPEED – ISO camera setting, if camera uses ISO speeds.
            //JD_GEO – records the geocentric Julian Day of the start of exposure.
            //JD_HELIO – records the Heliocentric Julian Date at the exposure midpoint.
            //NOTES – user-entered information; free-form notes.
            //READOUTM – records the selected Readout Mode (if any) for the camera.

            //imageHdu.AddValue("SBSTDVER", "SBFITSEXT Version 1.0", "version of the SBIG FITS extensions supported");

            // save it
            var fitsImage = new Fits();
            fitsImage.AddHDU(imageHdu); //Adds the actual image data (the header info already exists in imageHDU)
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Create);
                using (var bds = new BufferedDataStream(fs))
                {
                    fs = null;
                    fitsImage.Write(bds);
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }


        public void saveImageToFitsForSolveOnly(string path, imageInfo image, Array imgArray)
        {
            var imageData = (Array)ArrayFuncs.Flatten(imgArray);
            const double bZero = 0;
            const double bScale = 1.0;
            if (image.MaxADU <= 65535)
            {
                //bZero = 32768;
                //imageData = (ushort[])ArrayFuncs.ConvertArray(imageData, typeof(ushort));
            }
            int[] dims = ArrayFuncs.GetDimensions(imgArray);

            // put the image data in a basic HDU of the fits 
            BasicHDU imageHdu = FitsFactory.HDUFactory(ArrayFuncs.Curl(imageData, dims));
            // Add the other fits fields to the HDU
            imageHdu.AddValue("BZERO", bZero, "");
            imageHdu.AddValue("BSCALE", bScale, "");
            imageHdu.AddValue("DATAMIN", 0.0, "");      // should this reflect the actual data values
            imageHdu.AddValue("DATAMAX", image.MaxADU, "pixel values above this level are considered saturated.");
            imageHdu.AddValue("DATE-OBS", image.LastExposureStartTime, "");
            imageHdu.AddValue("XPIXSZ", image.PixelSizeX * image.BinX, "physical X dimension of the sensor's pixels in microns"); //  (present only if the information is provided by the camera driver). Includes binning.
            imageHdu.AddValue("YPIXSZ", image.PixelSizeY * image.BinY, "physical Y dimension of the sensor's pixels in microns"); //  (present only if the information is provided by the camera driver). Includes binning.
            imageHdu.AddValue("XBINNING", image.BinX, "");
            imageHdu.AddValue("YBINNING", image.BinY, "");
            imageHdu.AddValue("OBJCTRA", image.RA, "Approximate Right Ascension of image centre");
            imageHdu.AddValue("OBJCTDEC", image.Dec, "Approximate Declination of image centre");

            // save it
            var fitsImage = new Fits();
            fitsImage.AddHDU(imageHdu); //Adds the actual image data (the header info already exists in imageHDU)
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Create);
                using (var bds = new BufferedDataStream(fs))
                {
                    fs = null;
                    fitsImage.Write(bds);
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }


    }
}

