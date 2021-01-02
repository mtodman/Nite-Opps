using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Nite_Opps
{
    public partial class frmImaging : Form
    {
        fitsHeader F;
        clsSharedData sd;
        clsTargetObject to;
        AstroUtils Util = new AstroUtils();
        clsForms form;

        // imaging camera variables
        int imagingCamExposureDurationMins, imagingCamExposureDurationSecs, imagingCamExposureDurationMs, imagingCamBinning;
        bool imagingCamAutoRepeat, imagingCamCoolerOn;
        string imagingCamFrameType;
        int imagingCamCoolerTargetTemp;
        bool ImageFromCam = false;
        bool abortProgressBar = false;
        bool isAutoRepeat = false;
        bool autoRepeatImageTaken = false;

        // Variables for the progressbar
        private int maxTime, remainingTime;
        private double elapsedTime;
        private double maxTimeMS, total_duration_secs;
        private DateTime startTime, endTime;
        private TimeSpan elapsedtime;

        public struct maxtime
        {
            public int MAXTIME;
        }




        public frmImaging()
        {
            InitializeComponent();
        }

        public frmImaging(ref clsSharedData d, ref clsTargetObject t, ref clsForms f)
        {
            InitializeComponent();
            sd = d;
            to = t;
            form = f;

        }

        delegate void displayImageCallback(clsSharedData sharedData, clsForms form);


        public void displayImage(clsSharedData sharedData, clsForms form)
        {
            sharedData.F = new fitsHeader();
            Graphics g = form.ImagingForm.pictureBox1.CreateGraphics();
            if (form.ImagingForm.InvokeRequired)
            {
                form.ImagingForm.Invoke(new Action<clsSharedData, clsForms>(displayImage), new object[] { sharedData, form });
                return;
            }

            if (!sharedData.ImagingCam.ImageReady) System.Threading.Thread.Sleep(500);
            System.Threading.Thread.Sleep(500);
            try
            {
                sharedData.theImage.LastExposureDuration = sharedData.ImagingCam.LastExposureDuration;
            }
            catch
            {
                //Used to handle exception raised if there is no LastExposureDuration (ie, this is the first image)
            }
            
            sharedData.theImage.LastExposureStartTime = sharedData.ImagingCam.LastExposureStartTime;
            form.ImagingForm.pictureBox1.Size = new System.Drawing.Size(sharedData.ImagingCam.ExposureWidth, sharedData.ImagingCam.ExposureHeight);


            g.DrawImage(sharedData.theImage.bmp, new Rectangle(0, 0, sharedData.ImagingCam.ExposureWidth, sharedData.ImagingCam.ExposureHeight));
            form.ImagingForm.ImageFromCam = true;


            form.ImagingForm.btnStop.Enabled = false;
            form.ImagingForm.btnAbort.Enabled = false;
            form.ImagingForm.btnStart.Enabled = true;
            form.ImagingForm.chkAutoRepeat.Enabled = true;


            if (form.ImagingForm.isAutoRepeat) form.ImagingForm.autoRepeatImageTaken = true;
            sharedData.imagecomplete = true;
            clsStatics.imageComplete = true;

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            takeAnImage(true);
        }

        public void takeAnImage(bool arg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<bool>(takeAnImage), new object[] { arg });
                return;
            }

            if (!sd.isImagingCamConnected)
            {
                MessageBox.Show("Can't begin exposure. The Imaging camera is not connected");
                return;
            }
            form.ImagingForm.btnStart.Enabled = false;
            form.ImagingForm.btnAbort.Enabled = true;
            form.ImagingForm.btnStop.Enabled = true;
            form.ImagingForm.chkAutoRepeat.Enabled = false;

            readCameraValues();
            readImageInfo();
            Int32 total_duration_msecs = (imagingCamExposureDurationMs + (imagingCamExposureDurationSecs * 1000) + (imagingCamExposureDurationMins * 1000 * 60));
            if (total_duration_msecs == 0) total_duration_msecs = 1; //1ms is assumed to be a 0 length exposure
            total_duration_secs = total_duration_msecs / 1000.0;
            maxTime = (int)total_duration_secs;
            maxTimeMS = total_duration_msecs;
            var d = new maxtime
            {
                MAXTIME = maxTime,
            };
            Thread progressBarThread = new System.Threading.Thread(t_progressbar);
            progressBarThread.Start(d);

            sd.ImagingCam.takeTheImage(imagingCamBinning, imagingCamFrameType, total_duration_secs);


            if (chkAutoRepeat.Checked)
            {
                if (!isAutoRepeat)
                {
                    isAutoRepeat = true;
                    Thread autoRepeatThread = new System.Threading.Thread(t_autoRepeat);
                    autoRepeatThread.Start();
                }
            }
        }

        private void readCameraValues()
        {
            imagingCamExposureDurationMins = Convert.ToInt16(numMins.Value);
            imagingCamExposureDurationSecs = Convert.ToInt16(numSecs.Value);
            imagingCamExposureDurationMs = Convert.ToInt16(numMs.Value);
            imagingCamBinning = Convert.ToInt16(numBinning.Value);
            imagingCamAutoRepeat = chkAutoRepeat.Checked;
            imagingCamCoolerOn = chkCoolerOn.Checked;
            imagingCamCoolerTargetTemp = Convert.ToInt16(numTargetTemp.Value);

            if (rbFlatFrame.Checked) sd.imagingCamFrameType = "Flat";
            else if (rbDarkFrame.Checked) sd.imagingCamFrameType = "Dark";
            else if (rbBiasFrame.Checked)
            {
                sd.imagingCamFrameType = "Bias";
                imagingCamExposureDurationMs = 0;
                imagingCamExposureDurationSecs = 0;
                imagingCamExposureDurationMins = 0;
            }
            else sd.imagingCamFrameType = "Light";
            imagingCamFrameType = sd.theImage.frameType = sd.imagingCamFrameType;
        }

        private void readImageInfo()
        {
            sd.theImage.dRA = to.targetObjectRA;
            sd.theImage.dDec = to.targetObjectDec;
            sd.theImage.RA = Util.HoursToHMS(to.targetObjectRA, ":", ":");
            sd.theImage.Dec = Util.HoursToHMS(to.targetObjectDec, ":", ":");
            sd.theImage.Alt = Util.HoursToHMS(to.targetObjectAlt, " ", " ");
            sd.theImage.Az = Util.HoursToHMS(to.targetObjectAz, " ", " ");
            sd.theImage.arcSecsPerPixX = (sd.ImagingCam.PixelSizeX / Properties.Settings.Default.imaging_telescope_focal_length) * 0.206 * 1000;
            sd.theImage.arcSecsPerPixY = (sd.ImagingCam.PixelSizeY / Properties.Settings.Default.imaging_telescope_focal_length) * 0.206 * 1000;
            sd.theImage.BinX = sd.ImagingCam.BinX;
            sd.theImage.BinY = sd.ImagingCam.BinY;
            sd.theImage.MaxADU = sd.ImagingCam.MaxADU;
            sd.theImage.Description = sd.ImagingCam.Description;
            //sd.theImage.LastExposureDuration = sd.ImagingCam.LastExposureDuration;
            //sd.theImage.LastExposureStartTime = sd.ImagingCam.LastExposureStartTime;
            sd.theImage.PixelSizeX = sd.ImagingCam.PixelSizeX;
            sd.theImage.PixelSizeY = sd.ImagingCam.PixelSizeY;
            sd.theImage.StartX = sd.ImagingCam.StartX;
            sd.theImage.StartY = sd.ImagingCam.StartY;
            sd.theImage.Min = sd.ImagingCam.Min;
            sd.theImage.Max = sd.ImagingCam.Max;
            if (sd.ImagingCam.CanSetCCDTemperature)
            {
                sd.theImage.CCDTemperature = sd.ImagingCam.CCDTemperature;
                sd.theImage.CanSetCCDTemperature = sd.ImagingCam.CanSetCCDTemperature;
                sd.theImage.SetCCDTemperature = sd.ImagingCam.SetCCDTemperature;
            }
            sd.theImage.objectName = to.targetObjectName;

        }

        private void t_progressbar(object x)
        {
            maxtime d = (maxtime)x;
            if (InvokeRequired)
            {
                this.Invoke(new Action<object>(t_progressbar), new object[] { x });
                return;
            }

            startTime = DateTime.Now;
            endTime = startTime.AddMilliseconds(maxTimeMS);
            progressBar1.Maximum = (int)maxTimeMS;

            progressBar1.Minimum = 0;
            progressbarTimer.Start();
        }

        private void progressbarTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now < endTime)
            {
                elapsedtime = DateTime.Now.Subtract(startTime);
                elapsedTime = elapsedtime.Milliseconds + (elapsedtime.Seconds * 1000) + (elapsedtime.Minutes * 60000);
                progressBar1.Value = Convert.ToInt32(elapsedTime);
                remainingTime = Convert.ToInt32((maxTimeMS - elapsedTime) / 1000);
                this.lblimageTimeRemaining.Text = "Time Remaining: " + remainingTime + " Secs";
            }
            else
            {
                progressBar1.Value = 0;
                this.lblimageTimeRemaining.Text = "";
                progressbarTimer.Stop();

            }

            if (abortProgressBar)
            {
                progressBar1.Value = 0;
                this.lblimageTimeRemaining.Text = "";
                progressbarTimer.Stop();
                abortProgressBar = false;
                btnStop.Enabled = false;
                btnAbort.Enabled = false;
                btnStart.Enabled = true;
                chkAutoRepeat.Enabled = true;
                writetolog("Exposure Arorted by user\r\n", true);
                if (chkAutoRepeat.Checked)
                    chkAutoRepeat.Checked = false;
            }

        }

        private void t_autoRepeat()
        {
            while (isAutoRepeat)
            {
                if (autoRepeatImageTaken)
                {
                    autoRepeatImageTaken = false;
                    takeAnImage(true);
                }
                System.Threading.Thread.Sleep(250);
            }
        }

        // Code to create the event which sends a string message to 
        // the Main form's Status Box (txtStatusBox).
        public delegate void LogHandler(string message, bool displaytime);
        public event LogHandler Log;
        public void writetolog(string message, bool displaytime)
        {
            Log(message, displaytime);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (form.ImagingForm.ImageFromCam)
            {
                Graphics g = e.Graphics;
                form.ImagingForm.pictureBox1.Size = new System.Drawing.Size(sd.ImagingCam.ExposureWidth, sd.ImagingCam.ExposureHeight);
                g.DrawImage(sd.theImage.bmp, new Rectangle(0, 0, sd.ImagingCam.ExposureWidth, sd.ImagingCam.ExposureHeight));

            }

        }

        private void rbLightFrame_CheckedChanged(object sender, EventArgs e)
        {
            form.ImagingForm.numMins.Enabled = true;
            form.ImagingForm.numSecs.Enabled = true;
            form.ImagingForm.numMs.Enabled = true;
        }

        private void rbBiasFrame_CheckedChanged(object sender, EventArgs e)
        {
            form.ImagingForm.numMins.Enabled = false;
            form.ImagingForm.numSecs.Enabled = false;
            form.ImagingForm.numMs.Enabled = false;
        }

        private void rbDarkFrame_CheckedChanged(object sender, EventArgs e)
        {
            form.ImagingForm.numMins.Enabled = true;
            form.ImagingForm.numSecs.Enabled = true;
            form.ImagingForm.numMs.Enabled = true;
        }

        private void rbFlatFrame_CheckedChanged(object sender, EventArgs e)
        {
            form.ImagingForm.numMins.Enabled = true;
            form.ImagingForm.numSecs.Enabled = true;
            form.ImagingForm.numMs.Enabled = true;
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            //Todo
            // Write "Aborted" to the status window
            if (!sd.isImagingCamConnected) return;
            try
            {
                sd.ImagingCam.AbortExposure();
                abortProgressBar = true;

                if (isAutoRepeat)
                {
                    chkAutoRepeat.Checked = false;
                    isAutoRepeat = false;
                    autoRepeatImageTaken = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException != null
                                    ? string.Format("Inner {0}", ex.InnerException.Message)
                                    : string.Format("Error {0}", ex.Message));
            }
              

        }

        private void timerUpdateCamState_Tick(object sender, EventArgs e)
        {
            if (sd.isImagingCamConnected)
            {
                lblCameraStatus.Text = sd.ImagingCam.CameraState.ToString();
                if (sd.ImagingCam.CanSetCCDTemperature)
                {
                    lblCameraTemp.Text = sd.ImagingCam.CCDTemperature.ToString("F2") + "°C"; //Add support for cameras that don't have cooling
                    lblCoolerPower.Text = sd.ImagingCam.CoolerPower.ToString("F2") + "%";
                    if (imagingCamCoolerOn) sd.ImagingCam.SetCCDTemperature = (int)numTargetTemp.Value;
                }
            }
            else
            {
                lblCameraStatus.Text = "Disconnected";
                lblCameraTemp.Text = "---";
                lblCoolerPower.Text = "---";
            }

        }

        private void chkCoolerOn_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCoolerOn.Checked)
            {
                imagingCamCoolerOn = true;
                sd.ImagingCam.CoolerOn = true;
            }
            else
            {
                imagingCamCoolerOn = false;
                sd.ImagingCam.CoolerOn = false;
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            string filename;
            F = new fitsHeader();
            saveFileDialog1.Title = "Save the .FITS file";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
            }
            else { filename = ""; }

            F.saveImageToFits(filename, sd);
        }


    }
}
