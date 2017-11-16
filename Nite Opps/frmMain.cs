using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;


namespace Nite_Opps
{
    public partial class frmMain : Form
    {

        string tempSolveFileDir = System.Windows.Forms.Application.StartupPath + "\\data";
        string tempSolveFile = System.Windows.Forms.Application.StartupPath + "\\data\\solvefile.fits";
        AstroUtils Util = new AstroUtils();
        Telescope T;
        public ASCOM.Utilities.Profile prof = new ASCOM.Utilities.Profile();
        clsSharedData sd;
        clsTargetObject to;
        PlateSolve ps;
        double solvedRA, solvedDec, pointingErrorDegs, pointingErrorSecs;
        clsForms form;
        User_Profile Prof = new User_Profile();
        //phd G;
        public bool bLogging = true;
        public float flMaxGuideDelta = 10.0F;

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


        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(ref clsSharedData d, ref clsTargetObject t, ref clsForms f)
        {
            InitializeComponent();
            sd = d;
            to = t;
            form = f;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName + " " + Application.ProductVersion;
            timer1.Enabled = true;
            loadAutoConnects();
        }

        
        private void writeChangableValuesToScreen()
        {

            lblSIteLat.Text = Properties.Settings.Default.site_lat_deg.ToString() + "° " + Properties.Settings.Default.site_lat_min.ToString() + "' " + Properties.Settings.Default.site_lat_sec.ToString() + "\" " + Properties.Settings.Default.site_lat_ns;
            lblSiteLong.Text = Properties.Settings.Default.site_long_deg.ToString() + "° " + Properties.Settings.Default.site_long_min.ToString() + "' " + Properties.Settings.Default.site_long_sec.ToString() + "\" " + Properties.Settings.Default.site_long_ew;
            lblUT.Text = DateTime.UtcNow.Hour.ToString("00") + ":" + DateTime.UtcNow.Minute.ToString("00") + ":" + DateTime.UtcNow.Second.ToString("00");
            lblPCT.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");

            if (sd.isScopeConnected)
            {
                lblScopeRA.Text = Util.HoursToHMS(T.RightAscension);
                lblScopeDEC.Text = Util.HoursToHMS(T.Declination, "° ", "' ", "\" ");
                lblScopeAlt.Text = Util.HoursToHMS(T.Altitude, "° ", "' ", "\" ");
                lblScopeAz.Text = Util.HoursToHMS(T.Azimuth, "° ", "' ", "\" ");
                if(T.AtPark)
                {
                    lblTelescopeParkStatus.Text = "Parked";
                    sd.parkStatus = true;
                }
                else
                {
                    lblTelescopeParkStatus.Text = "Not Parked";
                    sd.parkStatus = false;
                }
                if (T.Tracking)
                {
                    lblTelescopeTrackingStatus.Text = "Tracking";
                }
                else
                {
                    lblTelescopeTrackingStatus.Text = "Not Tracking";
                }

                updateButtons();
            }
            else
            {
                lblScopeRA.Text = "Not Connected";
                lblScopeDEC.Text = "Not Connected";
                lblScopeAlt.Text = "Not Connected";
                lblScopeAz.Text = "Not Connected";
                lblTelescopeParkStatus.Text = "Not Connected";
                lblTelescopeTrackingStatus.Text = "Not Connected";
            }

            if(sd.isPHDConnected)
            {
                //lblGuidingStatus.Text = sd.G.getStatus.ToString();
                lblGuidingStatus.Text = sd.G.bConnected.ToString();
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            writeChangableValuesToScreen();
        }

        private void btnObjectDatabase_Click(object sender, EventArgs e)
        {
            objectDatabaseConnectDisconnect();
            updateButtons();
        }

        void objectDatabaseConnectDisconnect()
        {
            switch (Properties.Settings.Default.object_database)
            {
                case "Generic":
                    switch (sd.isObjectDbConnected)
                    {
                        case true:
                            disconnectObjectDb();
                            sd.O = null;
                            break;
                        case false:
                            sd.O = new objectDb("Generic");
                            sd.isObjectDbConnected = true;
                            updateStatusBox("Generic Object Db is connected\r\n", true);

                            break;
                    }
                    break;
                case "MiniSAC":
                    switch (sd.isObjectDbConnected)
                    {
                        case true:
                            disconnectObjectDb();
                            sd.O = null;
                            break;
                        case false:
                            sd.O = new objectDb("MiniSAC");
                            sd.isObjectDbConnected = true;
                            updateStatusBox("MiniSAC Object Db is connected\r\n", true);

                            break;
                    }
                    break;
            }
        }

        private void btnImagingCam_Click(object sender, EventArgs e)
        {
            imagingCameraConnectDisconnect();
            updateButtons();
        }

        void imagingCameraConnectDisconnect()
        {
            switch (sd.isImagingCamConnected)
            {
                case true:
                    form.ImagingForm.timerUpdateCamState.Enabled = false;
                    sd.ImagingCam.Connected = false;
                    sd.isImagingCamConnected = false;
                    sd.ImagingCam.Dispose();
                    updateStatusBox("Imaging Camera is disconnected\r\n", true);
                    break;

                case false:
                    if (Properties.Settings.Default.ImagingCamProdID != "")
                    {
                        sd.ImagingCam = new ASCOMCamera(Properties.Settings.Default.ImagingCamProdID, ref sd, ref form);
                        sd.ImagingCam.Log += new ASCOMCamera.LogHandler(updateStatusBox);
                        form.ImagingForm.Log += new frmImaging.LogHandler(updateStatusBox);
                        sd.ImagingCam.Connected = true;
                        //WriteToReg("ImagingCamProdID", Properties.Settings.Default.ImagingCamProdID);
                        sd.isImagingCamConnected = true;
                        updateStatusBox("Imaging Camera is connected\r\n", true);
                        sd.ImagingCam.write += new ASCOMCamera.PicReady(form.ImagingForm.displayImage); // Subscribe to the PicReady event & execute displayImage when that event fires
                        form.ImagingForm.timerUpdateCamState.Enabled = true;
                        sd.readCameraValues();
                        sd.setCameraValues();
                        clsStatics.imaging_camera_num_pixels_height = sd.ImagingCam.CameraXSize;
                        clsStatics.imaging_camera_num_pixels_width = sd.ImagingCam.CameraYSize;
                        clsStatics.imaging_camera_pixel_height = sd.ImagingCam.PixelSizeX;
                        clsStatics.imaging_camera_pixel_width = sd.ImagingCam.PixelSizeY;
                        
                    }
                    else { MessageBox.Show("No Imaging Camera has been selected in the configuration tab"); }
                    break;
            }

        }

        private void btnGuidingCam_Click(object sender, EventArgs e)
        {
            guidingCameraConnectDisconnect();
            updateButtons();
        }

        void guidingCameraConnectDisconnect()
        {
            switch (sd.isGuidingCamConnected)
            {
                case true:
                    form.ImagingForm.timerUpdateCamState.Enabled = false;
                    sd.GuidingCam.Connected = false;
                    sd.isGuidingCamConnected = false;
                    sd.GuidingCam.Dispose();
                    updateStatusBox("Guiding Camera is disconnected\r\n", true);
                    break;

                case false:
                    if (Properties.Settings.Default.GuidingCamProdID != "")
                    {
                        sd.GuidingCam = new ASCOMCamera(Properties.Settings.Default.GuidingCamProdID, ref sd, ref form);
                        sd.GuidingCam.Log += new ASCOMCamera.LogHandler(updateStatusBox);
                        sd.GuidingCam.Connected = true;
                        //WriteToReg("GuidingCamProdID", Properties.Settings.Default.GuidingCamProdID);
                        sd.isGuidingCamConnected = true;
                        updateStatusBox("Guiding Camera is connected\r\n", true);
                        clsStatics.guiding_camera_num_pixels_height = sd.GuidingCam.CameraXSize;
                        clsStatics.guiding_camera_num_pixels_width = sd.GuidingCam.CameraYSize;
                        clsStatics.guiding_camera_pixel_height = sd.GuidingCam.PixelSizeX;
                        clsStatics.guiding_camera_pixel_width = sd.GuidingCam.PixelSizeY;
                    }
                    break;
            }

        }

        private void btnFilterWheel_Click(object sender, EventArgs e)
        {
            filterWheelConnectDisconnect();
            updateButtons();
        }

        void filterWheelConnectDisconnect()
        {
            switch (sd.isFilterWheelConnected)
            {
                case true:
                    sd.FW.Connected = false;
                    sd.isFilterWheelConnected = false;
                    sd.FW.Dispose();
                    updateStatusBox("Filter Wheel is disconnected\r\n", true);
                    break;

                case false:
                    if (Properties.Settings.Default.FilterWheelProdID != "")
                    {
                        sd.FW = new ASCOMFilterWheel(Properties.Settings.Default.FilterWheelProdID, ref sd, ref form);
                        sd.FW.Log += new ASCOMFilterWheel.LogHandler(updateStatusBox);
                        sd.FW.Connected = true;
                        //WriteToReg("ImagingCamProdID", Properties.Settings.Default.ImagingCamProdID);
                        sd.isFilterWheelConnected = true;
                        updateStatusBox("Filter Wheel is connected\r\n", true);
                    }
                    break;
            }

        }


        void ImagingForm_Log(string message, bool displaytime)
        {
            throw new NotImplementedException();
        }


        private void btnASCOMScopeConnect_Click(object sender, EventArgs e)
        {
            mountConnectDisconnect();
            updateButtons();
        }

        void mountConnectDisconnect()
        {
            switch (sd.isScopeConnected)
            {
                case true:
                    disconnectScope();
                    break;

                case false:
                    if (Properties.Settings.Default.ScopeProdID != "")
                    {
                        T = new Telescope(Properties.Settings.Default.ScopeProdID, ref sd);
                        T.Log += new Telescope.LogHandler(updateStatusBox); // Subscribe to the Loghandler event & execute updateStatusBox when that event fires
                        T.Connected = true;
                        //WriteToReg("scopeProgID", Properties.Settings.Default.ScopeProdID);
                        sd.isScopeConnected = true;
                        updateStatusBox("Mount is connected\r\n", true);

                        if (T.CanPark)
                        {
                            btnParkStatus.Enabled = true;
                            sd.parkStatus = T.AtPark;
                        }
                    }
                    break;
            }

        }


        private void btnPlateSolve_Click(object sender, EventArgs e)
        {
            astrometryEngineConnectDisconnect();
            updateButtons();
        }

        void astrometryEngineConnectDisconnect()
        {
            switch (Properties.Settings.Default.platesolve_program)
            {
                case "PinPoint":
                    switch (sd.isAstrometryEngineConnected)
                    {
                        case true:
                            disconnectAstrometryEngine();
                            break;
                        case false:
                            ps = new PlateSolve(Properties.Settings.Default.platesolve_program, ref sd);
                            if (ps.checkStatus())
                            {
                                sd.isAstrometryEngineConnected = true;
                                updateStatusBox("PinPoint Plate Solve Engine is connected\r\n", true);
                            }
                            else
                            {
                                MessageBox.Show("Problem encountered connecting to PinPoint");
                                sd.isAstrometryEngineConnected = false;
                            }
                            break;
                    }
                    break;

                case "Elbrus":
                    switch (sd.isAstrometryEngineConnected)
                    {
                        case true:
                            disconnectAstrometryEngine();
                            break;
                        case false:
                            ps = new PlateSolve(Properties.Settings.Default.platesolve_program, ref sd);
                            if (ps.checkStatus())
                            {
                                sd.isAstrometryEngineConnected = true;
                                updateStatusBox("Elbrus Plate Solve Engine is connected\r\n", true);

                            }
                            else
                            {
                                MessageBox.Show("Problem encountered connecting to Elbrus");
                                sd.isAstrometryEngineConnected = false;
                            }
                            break;
                    }

                    break;
            }

        }

        private void btnPHD_Click(object sender, EventArgs e)
        {
            phdConnectDisconnect();
            updateButtons();
        }

        void phdConnectDisconnect()
        {
            switch (sd.isPHDConnected)
            {
                case true:
                    disconnectPHD();
                    break;
                case false:
                    sd.G = new clsPhd(ref sd, this);
                    sd.G.Log += new clsPhd.LogHandler(updateStatusBox);
                    sd.G.evPHDAlarm += new clsPhd.PHDAlarmHandler(alarmReceived);
                    //sd.G.bConnected = true;
                    sd.G.Listening = true;
                   
                    if (sd.G.bConnected)
                    {
                        sd.isPHDConnected = true;
                        
                        updateStatusBox("PHD is connected\r\n", true);
                    }
                    else
                    {
                        MessageBox.Show("Problem encountered connecting to PHD");
                        sd.isPHDConnected = false;
                    }
                    break;
            }
        }


        private void btnDisconnectAll_Click(object sender, EventArgs e)
        {
            disconnectAll();
            updateButtons();
        }



        public void disconnectObjectDb()
        {
            switch (Properties.Settings.Default.object_database)
            {
                case "MiniSAC":
                    sd.O = null;
                    sd.isObjectDbConnected = false;
                    updateStatusBox("MiniSAC Object Db is disconnected\r\n", true);
                    break;
                case "Generic":
                    sd.O = null;
                    sd.isObjectDbConnected = false;
                    updateStatusBox("Generic Object Db is disconnected\r\n", true);
                    break;
            }
        }

        public void disconnectScope()
        {
            T.Connected = false;
            sd.isScopeConnected = false;
            updateStatusBox("Mount is disconnected\r\n", true);
            T.Dispose();
        }

        public void disconnectImagingCamera()
        {
            sd.ImagingCam.Connected = false;
            sd.isImagingCamConnected = false;
            updateStatusBox("Imaging Camera is disconnected\r\n", true);
            sd.ImagingCam.Dispose();
        }

        public void disconnectGuidingCamera()
        {
            sd.GuidingCam.Connected = false;
            sd.isGuidingCamConnected = false;
            updateStatusBox("Guiding Camera is disconnected\r\n", true);
            sd.GuidingCam.Dispose();
        }

        public void disconnectFilterWheel()
        {
            sd.FW.Connected = false;
            sd.isFilterWheelConnected = false;
            updateStatusBox("Filter Wheel is disconnected\r\n", true);
            sd.FW.Dispose();
        }


        public void disconnectAstrometryEngine()
        {
            sd.isAstrometryEngineConnected = false;
            updateStatusBox("Astrometry Engine is disconnected\r\n", true);
            ps = null;
        }

        public void disconnectPHD()
        {
            sd.isPHDConnected = false;
            updateStatusBox("PHD is disconnected\r\n", true);
            sd.G = null;
        }

        private void btnConnectAll_Click(object sender, EventArgs e)
        {
            if (chkObjectDatabase.Checked)
            {
                if (!sd.isObjectDbConnected)
                {
                    objectDatabaseConnectDisconnect();
                }
            }
            if (chkImagingCamera.Checked)
            {
                if (!sd.isImagingCamConnected)
                {
                    imagingCameraConnectDisconnect();
                }
            }
            if (chkGuidingCamera.Checked)
            {
                if (!sd.isGuidingCamConnected)
                {
                    guidingCameraConnectDisconnect();
                }
            }
            if (chkMount.Checked)
            {
                if (!sd.isScopeConnected)
                {
                    mountConnectDisconnect();
                }
            }
            if (chkFilterWheel.Checked)
            {
                if (!sd.isFilterWheelConnected)
                {
                    filterWheelConnectDisconnect();
                }
            }
            if (chkAstrometryEngine.Checked)
            {
                if (!sd.isAstrometryEngineConnected)
                {
                    astrometryEngineConnectDisconnect();
                }
            }
            updateButtons();
        }


        public void disconnectAll()
        {
            if (sd.isScopeConnected)
            {
                disconnectScope();
            }

            if (sd.isObjectDbConnected)
            {
                disconnectObjectDb();
            }

            if (sd.isImagingCamConnected)
            {
                disconnectImagingCamera();
            }

            if (sd.isGuidingCamConnected)
            {
                disconnectGuidingCamera();
            }

            if (sd.isAstrometryEngineConnected)
            {
                disconnectAstrometryEngine();
            }

            if (sd.isFilterWheelConnected)
            {
                disconnectFilterWheel();
            }
        }

        

        private void updateStatusBox(string message, bool displayTimeStamp)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string, bool>(updateStatusBox), new object[] { message, displayTimeStamp });
                return;
            }

            if (displayTimeStamp) this.txtStatusBox.Text += DateTime.Now.ToLongTimeString() + ": ";

            this.txtStatusBox.Text += message;

            // The following 2 lines force the textbox to scroll down automatically as new lines added.
            this.txtStatusBox.Select(txtStatusBox.Text.Length, 0);
            this.txtStatusBox.ScrollToCaret();

        }

        private void alarmReceived(clsStatics.AlarmInfo e)
        {
            MessageBox.Show("Alarm Received");
        }

        private void updateStatusBoxNoInvoke(string message, bool displayTimeStamp)
        {

            if (displayTimeStamp) this.txtStatusBox.Text += DateTime.Now.ToLongTimeString() + ": ";

            this.txtStatusBox.Text += message;

            // The following 2 lines force the textbox to scroll down automatically as new lines added.
            this.txtStatusBox.Select(txtStatusBox.Text.Length, 0);
            this.txtStatusBox.ScrollToCaret();
        }

        private void SetTextUIThreadPatternParams(string text, bool timestamp)
        {
            this.UIThread(delegate
            {
                updateStatusBoxNoInvoke(text, timestamp);
            });
        }


        private void updateButtons()
        {
            switch (sd.isScopeConnected)
            {
                case true:
                    btnASCOMScopeConnect.BackColor = Color.Green;
                    btnASCOMScopeConnect.Text = "ASCOM Mount Connected";
                    break;
                case false:
                    btnASCOMScopeConnect.BackColor = Color.Red;
                    btnASCOMScopeConnect.Text = "ASCOM Mount Disconnected";
                    break;
            }

            switch (sd.isImagingCamConnected)
            {
                case true:
                    btnImagingCam.BackColor = Color.Green;
                    btnImagingCam.Text = "Imaging Camera Connected";
                    form.ImagingForm.lblCameraStatus.Text = "Conected";

                    break;
                case false:
                    btnImagingCam.BackColor = Color.Red;
                    btnImagingCam.Text = "Imaging Camera Disconnected";
                    form.ImagingForm.lblCameraStatus.Text = "Disconected";

                    break;
            }

            switch (sd.isGuidingCamConnected)
            {
                case true:
                    btnGuidingCam.BackColor = Color.Green;
                    btnGuidingCam.Text = "Guiding Camera Connected";
                    break;
                case false:
                    btnGuidingCam.BackColor = Color.Red;
                    btnGuidingCam.Text = "Guiding Camera Disconnected";
                    break;
            }

            switch (sd.isObjectDbConnected)
            {
                case true:
                    btnObjectDatabase.BackColor = Color.Green;
                    btnObjectDatabase.Text = "Object Database Connected";
                    break;
                case false:
                    btnObjectDatabase.BackColor = Color.Red;
                    btnObjectDatabase.Text = "Object Database Disconnected";
                    break;
            }

            switch (sd.isAstrometryEngineConnected)
            {
                case true:
                    btnPlateSolve.BackColor = Color.Green;
                    btnPlateSolve.Text = "Astrometry Engine Connected";
                    break;
                case false:
                    btnPlateSolve.BackColor = Color.Red;
                    btnPlateSolve.Text = "Astrometry Engine Disconnected";
                    break;
            }

            switch (sd.isScopeConnected)
            {
                case true:
                    btnParkStatus.Enabled = true;
                    switch (sd.parkStatus)
                    {
                        case false:
                            btnParkStatus.BackColor = Color.Green;
                            btnParkStatus.Text = "Park";
                            break;
                        case true:
                            btnParkStatus.BackColor = Color.Red;
                            btnParkStatus.Text = "Unpark";
                            break;
                    }
                    break;
                case false:
                    btnParkStatus.BackColor = Color.Gray;
                    btnParkStatus.Enabled = false;
                    break;
            }


            switch (sd.isFilterWheelConnected)
            {
                case true:
                    btnFilterWheel.BackColor = Color.Green;
                    btnFilterWheel.Text = "Filter Wheel Connected";

                    break;
                case false:
                    btnFilterWheel.BackColor = Color.Red;
                    btnFilterWheel.Text = "Filter Wheel Disconnected";

                    break;
            }

            switch (sd.isPHDConnected)
            {
                case true:
                    btnPHD.BackColor = Color.Green;
                    btnPHD.Text = "PHD Connected";

                    break;
                case false:
                    btnPHD.BackColor = Color.Red;
                    btnPHD.Text = "PHD Disconnected";

                    break;
            }
        }


        //public void WriteToReg(string parameter, string value)
        //{
        //    prof.WriteValue(sd.APPLICATION_NAME, parameter, value);
        //}

        //public void WriteToReg(string parameter, int value)
        //{
        //    prof.WriteValue(sd.APPLICATION_NAME, parameter, value.ToString());
        //}

        private void btnGetCoordinates_Click(object sender, EventArgs e)
        {
            getCoords(txtObject.Text);
        }

        private void btnManualCoordinates_Click(object sender, EventArgs e)
        {
            // Convert Manually entered coordinates into decimal and store in to.targetObjectRA & DEC variables
            to.targetObjectRA = Util.DMSToDegrees(txtRAhh.Text + ":" + txtRAmm.Text + ":" + txtRAss.Text);
            to.targetObjectDec = Util.DMSToDegrees(txtDecDeg.Text + ":" + txtDecMin.Text + ":" + txtDecSec.Text);
            AstroCalculations.structAltAz AltAz;
            AstroCalculations.structAltAz AltAz2;
            double siteLat = Util.DMSToDegrees(lblSIteLat.Text);
            double siteLon = Util.DMSToDegrees(lblSiteLong.Text);
            if (Properties.Settings.Default.site_lat_ns == "S") siteLat = -siteLat;
            AltAz = AstroCalculations.GetAltAz(siteLat, siteLon, to.targetObjectRA, to.targetObjectDec, sd, Util.JulianDate);
            AltAz2 = AstroCalculations.Calculate(to.targetObjectRA, to.targetObjectDec, siteLat, siteLon, DateTime.UtcNow);
            //AltAz2 = AstroCalculations.GetAltAz(to.targetObjectRA, to.targetObjectDec, siteLat, siteLon, Util.JulianDate);
            to.targetObjectAlt = AltAz.Alt;
            to.targetObjectAz = AltAz.Az;
            lblRAASCOM.Text = Util.HoursToHMS(to.targetObjectRA);
            lblDECASCOM.Text = Util.HoursToHMS(to.targetObjectDec, "° ", "' ", "\" ");
            lblAltASCOM.Text = Util.HoursToHMS(to.targetObjectAlt, "° ", "' ", "\" ");
            lblAzASCOM.Text = Util.HoursToHMS(to.targetObjectAz, "° ", "' ", "\" ");
            to.targetObjectAlt = AltAz2.Alt;
            to.targetObjectAz = AltAz2.Az;
            lblALT.Text = Util.HoursToHMS(to.targetObjectAlt, "° ", "' ", "\" ");
            lblAZ.Text = Util.HoursToHMS(to.targetObjectAz, "° ", "' ", "\" ");
        }


        void getCoords(string coords)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(getCoords), new object[] { coords });
                return;
            }
            if (coords != "" & !sd.isObjectDbConnected) //Assume that coordinates will be based on the entered object name
            {
                MessageBox.Show("Object Database is not connected");
                return;
            }

            if (coords != "")
            {

                updateStatusBox("Getting Co-ordinates \r\n", true);

                if(Properties.Settings.Default.object_database == "MiniSAC")
                {
                    clsStatics.coordinates c;
                    c = sd.O.getMiniSACCoords(txtObject.Text);
                    to.targetObjectRA = c.ra;
                    to.targetObjectDec = c.dec;
                }
                else
                {
                    to.targetObjectRA = sd.O.GetObjectRA(txtObject.Text);
                    to.targetObjectDec = sd.O.GetObjectDEC(txtObject.Text);
                }

                //to.targetObjectRA = sd.O.GetObjectRA(txtObject.Text);
                //to.targetObjectDec = sd.O.GetObjectDEC(txtObject.Text);
                AstroCalculations.structAltAz AltAz;
                AstroCalculations.structAltAz AltAz2;
                double siteLat = Util.DMSToDegrees(lblSIteLat.Text);
                double siteLon = Util.DMSToDegrees(lblSiteLong.Text);
                if (Properties.Settings.Default.site_lat_ns == "S") siteLat = -siteLat;
                AltAz = AstroCalculations.GetAltAz(siteLat, siteLon, to.targetObjectRA, to.targetObjectDec, sd, Util.JulianDate);
                //AltAz2 = AstroCalculations.Calculate(to.targetObjectRA, to.targetObjectDec, siteLat, siteLon, DateTime.UtcNow);
                AltAz2 = AstroCalculations.GetAltAz(to.targetObjectRA, to.targetObjectDec, siteLat, siteLon, Util.JulianDate);
                to.targetObjectAlt = AltAz.Alt;
                to.targetObjectAz = AltAz.Az;

                AstroCalculations.RAandDEC2hhmmss RAandDEC;
                RAandDEC = AstroCalculations.Dec2hhmmss(to.targetObjectRA, to.targetObjectDec);
                txtRAhh.Text = RAandDEC.RAhh.ToString();
                txtRAmm.Text = RAandDEC.RAmm.ToString();
                txtRAss.Text = RAandDEC.RAss.ToString();
                txtDecDeg.Text = RAandDEC.DECdeg.ToString();
                txtDecMin.Text = RAandDEC.DECmm.ToString();
                txtDecSec.Text = RAandDEC.DECss.ToString();

                lblRAASCOM.Text = Util.HoursToHMS(to.targetObjectRA);
                lblDECASCOM.Text = Util.HoursToHMS(to.targetObjectDec, "° ", "' ", "\" ");
                lblAltASCOM.Text = Util.HoursToHMS(to.targetObjectAlt, "° ", "' ", "\" ");
                lblAzASCOM.Text = Util.HoursToHMS(to.targetObjectAz, "° ", "' ", "\" ");

                to.targetObjectAlt = AltAz2.Alt;
                to.targetObjectAz = AltAz2.Az;
                lblALT.Text = Util.HoursToHMS(to.targetObjectAlt, "° ", "' ", "\" ");
                lblAZ.Text = Util.HoursToHMS(to.targetObjectAz, "° ", "' ", "\" ");

                updateStatusBox("Co-ordinates successfully obtained \r\n", true);
                to.targetObjectName = coords;
                return;

            }

            if (txtRAhh.Text == "" | txtDecDeg.Text == "" | txtRAmm.Text == "" | txtDecMin.Text == ""
                | txtRAss.Text == "" | txtDecSec.Text == "")
            {
                MessageBox.Show("No object found. Please enter an object name or object coordinates");
                to.targetObjectName = null;
                return;
            }
            else
            {
                to.targetObjectRA = Util.DMSToDegrees(txtRAhh.Text + " " + txtRAmm.Text + " " + txtRAss.Text);
                to.targetObjectDec = Util.DMSToDegrees(txtDecDeg.Text + " " + txtDecMin.Text + " " + txtDecSec.Text);

                AstroCalculations.structAltAz AltAz, AltAz2;
                double siteLat = Util.DMSToDegrees(lblSIteLat.Text);
                double siteLon = Util.DMSToDegrees(lblSiteLong.Text);
                if (Properties.Settings.Default.site_lat_ns == "S") siteLat = -siteLat;
                AltAz = AstroCalculations.GetAltAz(siteLat, siteLon, to.targetObjectRA, to.targetObjectDec, sd, Util.JulianDate);
                AltAz2 = AstroCalculations.Calculate(to.targetObjectRA, to.targetObjectDec, siteLat, siteLon, DateTime.UtcNow);
                to.targetObjectAlt = AltAz.Alt;
                to.targetObjectAz = AltAz.Az;

                lblRAASCOM.Text = Util.HoursToHMS(to.targetObjectRA);
                lblDECASCOM.Text = Util.HoursToHMS(to.targetObjectDec, "° ", "' ", "\" ");
                lblAltASCOM.Text = Util.HoursToHMS(to.targetObjectAlt, "° ", "' ", "\" ");
                lblAzASCOM.Text = Util.HoursToHMS(to.targetObjectAz, "° ", "' ", "\" ");

                to.targetObjectAlt = AltAz2.Alt;
                to.targetObjectAz = AltAz2.Az;
                lblALT.Text = Util.HoursToHMS(to.targetObjectAlt, "° ", "' ", "\" ");
                lblAZ.Text = Util.HoursToHMS(to.targetObjectAz, "° ", "' ", "\" ");

                updateStatusBox("Co-ordinates successfully obtained \r\n", true);
                to.targetObjectName = coords;

            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            disconnectAll();
            MdiParent.Close();
        }

        private void btnGoto_Click(object sender, EventArgs e)
        {
            slew();

        }

        private void btnParkStatus_Click(object sender, EventArgs e)
        {
            switch (sd.parkStatus)
            {
                case true: // The mount is currently parked
                    T.Unpark();
                    if (!T.AtPark) sd.parkStatus = false;
                    updateButtons();
                    break;

                case false:
                    T.parkScope();
                    if (T.AtPark) sd.parkStatus = true;
                    updateButtons();
                    break;
            }
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            solve();
        }

        void solve()
        {
            if (form.MainForm.InvokeRequired)
            {
                form.MainForm.Invoke(new Action(solve), new object[] {});
                return;
            }
            if (!sd.isAstrometryEngineConnected)
            {
                MessageBox.Show("Can't solve the image. Astrometry Engine is not connected");
                return;
            }
            else if (sd.theImage.LastExposureDuration == 0.0)
            {
                MessageBox.Show("Can't solve the image. No image in buffer");
                return;
            }
            updateStatusBox("Solve Started\r\n", true);
            var xx = (Array)sd.ImagingCam.ImageArray;
            sd.F.saveImageToFitsForSolveOnly(tempSolveFile, sd.theImage, xx);

            if (ps.solve(tempSolveFile, sd.theImage, Properties.Settings.Default.GSC_Location))
            {
                solvedRA = ps.RightAscension;
                form.MainForm.txtSolvedRA.Text = Util.DegreesToDMS(solvedRA);
                solvedDec = ps.Declination;
                form.MainForm.txtSolvedDEC.Text = Util.DegreesToDMS(solvedDec);
                pointingErrorDegs = ps.calcPointingError(solvedRA, solvedDec, to.targetObjectRA, to.targetObjectDec);
                pointingErrorSecs = AstroCalculations.Dec2secs(pointingErrorDegs);
                sd.solveError = pointingErrorSecs;
                form.MainForm.txtPointingError.Text = Util.DegreesToDMS(pointingErrorDegs);
                sd.sscount += 1;
                form.MainForm.txtSolveCount.Text = sd.sscount.ToString();
                updateStatusBox("Solve Completed Successfully\r\n", true);
            }
            else
            {
                updateStatusBox("Solve Failed\r\n", true);
                if (sd.plate_solve_error_msg != null)
                {
                    MessageBox.Show("Solve Failed: " + sd.plate_solve_error_msg);
                    sd.plate_solve_error_msg = null;
                }
                else
                {
                    MessageBox.Show("Solve Failed");
                }
            }
            sd.solvecomplete = true;

        }



        private void btnSlewAndSolve_Click(object sender, EventArgs e)
        {
            form.ImagingForm.chkAutoRepeat.Checked = false;
            form.ImagingForm.rbLightFrame.Checked = true;
            form.ImagingForm.rbDarkFrame.Checked = false;
            form.ImagingForm.rbBiasFrame.Checked = false;
            form.ImagingForm.rbFlatFrame.Checked = false;
            form.ImagingForm.numBinning.Value = Properties.Settings.Default.platesolve_binning;
            form.ImagingForm.numSecs.Value = Properties.Settings.Default.platesolve_exposure_duration;
            form.ImagingForm.numMins.Value = 0;
            form.ImagingForm.numMs.Value = 0;
            slewAndSolve(0);
        }

        public delegate void slewAndSolveDelegate(object irc);
        void slewAndSolve(int imageRunCount)
        {
            if (to.targetObjectAlt < 0) //Check that the target object is above the horizon
            {
                MessageBox.Show("Target Object is currently below the Horizon");
                return;
            }
            else if (!sd.isScopeConnected)
            {
                MessageBox.Show("Can't Slew. ASCOM Scope not connected");
                return;
            }
            else if (sd.parkStatus)
            {
                MessageBox.Show("Can't Slew. The Mount is current in a parked state");
                return;
            }

            slewAndSolveDelegate sas1 = t_slew;
            IAsyncResult ar = sas1.BeginInvoke(imageRunCount, null, null);
            while (!ar.IsCompleted)
            { System.Threading.Thread.Sleep(1000); }

        }

        void t_slew(object imageRunCount)
        {
            sd.sscount = 0;

            while (sd.sscount < Properties.Settings.Default.max_num_platesolves)
            {
                updateStatusBox("Initiating Slew & Solve attempt " + (sd.sscount + 1).ToString() + "\r\n", true);

                sd.slewcomplete = false;
                sd.imagecomplete = false;
                sd.solvecomplete = false;
                T.slewScope(to.targetObjectRA, to.targetObjectDec);
                while (!sd.slewcomplete)
                {
                    System.Threading.Thread.Sleep(500);
                }

                form.ImagingForm.takeAnImage(true);

                while (!sd.imagecomplete)
                {
                    System.Threading.Thread.Sleep(500);
                }

                solve();

                while (!sd.solvecomplete)
                {
                    System.Threading.Thread.Sleep(500);
                }

                System.Threading.Thread.Sleep(500); //Just a little pause between slew & solves to make it look better.
                updateStatusBox("Pointing error = " + Convert.ToInt16(sd.solveError).ToString() + " arc secs \r\n", true);
                updateMountPos();
                if (Convert.ToInt16(sd.solveError) <= Properties.Settings.Default.platesolve_max_pointing_error)
                {
                    if (sd.sscount == 1) { updateStatusBox("Mount pointing accuracyn achieved after " + sd.sscount.ToString() + " plate solve \r\n", true); }
                    else { updateStatusBox("Mount pointing accuracy achieved after " + sd.sscount.ToString() + " plate solves \r\n", true); }
                    break;
                }

            }
            if (Convert.ToInt16(sd.solveError) > Properties.Settings.Default.platesolve_max_pointing_error)
            {
                updateStatusBox("Failed to achieve required pointing accuracy after " + Properties.Settings.Default.max_num_platesolves.ToString() + " plate solves \r\n", true);
            }
            if (clsStatics.isImagingRun) { clsStatics.taskNumComplete[(int)imageRunCount - 1] = true; }
            if ((int)imageRunCount == sd.intArraySize) { clsStatics.isImagingRun = false; }
        }

        void slew()
        {
            if (to.targetObjectAlt < 0) //Check that the target object is above the horizon
            {
                MessageBox.Show("Target Object is currently below the Horizon");
            }
            else if (!sd.isScopeConnected)
            {
                MessageBox.Show("Can't Slew. ASCOM Scope not connected");
            }
            else if (sd.parkStatus)
            {
                MessageBox.Show("Can't Slew. The Mount is current in a parked state");
            }
            else
            {
                T.slewScope(to.targetObjectRA, to.targetObjectDec);
            }
        }

        private void btnSyncWithSolve_Click(object sender, EventArgs e)
        {
            updateMountPos();
        }
        
        void updateMountPos()
        {
            if (!sd.isScopeConnected)
            {
                MessageBox.Show("Can't Sync. Mount not connected");
                return;
            }
            T.SyncToCoordinates(solvedRA, solvedDec);
        }

        #region Auto Load Checkboxes
        private void loadAutoConnects()
        {
            if (Properties.Settings.Default.objectDBChecked == true) { chkObjectDatabase.Checked = true; }
            else { chkObjectDatabase.Checked = false; }

            if (Properties.Settings.Default.imagingCameraChecked == true) { chkImagingCamera.Checked = true; }
            else { chkImagingCamera.Checked = false; }

            if (Properties.Settings.Default.guidingCameraChecked == true) { chkGuidingCamera.Checked = true; }
            else { chkGuidingCamera.Checked = false; }

            if (Properties.Settings.Default.mountChecked == true) { chkMount.Checked = true; }
            else { chkMount.Checked = false; }

            if (Properties.Settings.Default.filterWheelChecked == true) { chkFilterWheel.Checked = true; }
            else { chkFilterWheel.Checked = false; }

            if (Properties.Settings.Default.astrometryEngineChecked == true) { chkAstrometryEngine.Checked = true; }
            else { chkAstrometryEngine.Checked = false; }

        }

        private void chkObjectDatabase_CheckedChanged(object sender, EventArgs e)
        {
            if (chkObjectDatabase.Checked) { Properties.Settings.Default.objectDBChecked = true; }
            else { Properties.Settings.Default.objectDBChecked = false; }
        }

        private void chkImagingCamera_CheckedChanged(object sender, EventArgs e)
        {
            if (chkImagingCamera.Checked) { Properties.Settings.Default.imagingCameraChecked = true; }
            else { Properties.Settings.Default.imagingCameraChecked = false; }
        }

        private void chkGuidingCamera_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGuidingCamera.Checked) { Properties.Settings.Default.guidingAppChecked = true; }
            else { Properties.Settings.Default.guidingAppChecked = false; }
        }

        private void chkMount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMount.Checked) { Properties.Settings.Default.mountChecked = true; }
            else { Properties.Settings.Default.mountChecked = false; }
        }

        private void chkFilterWheel_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFilterWheel.Checked) { Properties.Settings.Default.filterWheelChecked = true; }
            else { Properties.Settings.Default.filterWheelChecked = false; }
        }

        private void chkAstrometryEngine_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAstrometryEngine.Checked) { Properties.Settings.Default.astrometryEngineChecked = true; }
            else { Properties.Settings.Default.astrometryEngineChecked = false; }
        }
        #endregion
        public string filename = null;
        public delegate void masterDelegate();

        private void btnStartImagingRun_Click(object sender, EventArgs e)
        {
            clsStatics.isImagingRun = true;
            if (!sd.isImagingCamConnected)
            { 
            MessageBox.Show("Unable to image. Camera not connected");
            return;
            }

            if (Properties.Settings.Default.inclDateSubdirectoryChecked == true)
            {
                filename = Properties.Settings.Default.strImgRunFileLocation + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
            } else {
                filename = Properties.Settings.Default.strImgRunFileLocation;
            }


            if (!Directory.Exists(filename) && filename != "")
            {
                Directory.CreateDirectory(filename);
            }
            if (filename == "") filename = "c:\\";


            System.Threading.Thread thread0 = new System.Threading.Thread(t_sas);
            thread0.IsBackground = true;
            thread0.Start();
            
        }

        public delegate void slewDelegate(slewAndSolveInfo ss_1);
        public delegate void lightDelegate(slewAndSolveInfo ss_2);
        public delegate void darkDelegate(slewAndSolveInfo ss_3);
        public delegate void flatDelegate(slewAndSolveInfo ss_4);
        public delegate void biasDelegate(slewAndSolveInfo ss_5);
        public delegate void pauseDelegate(slewAndSolveInfo ss_6);
        void t_sas()
        {
            clsStatics.taskNumComplete = new bool[sd.intArraySize];

            for (int i = 1; i <= sd.intArraySize; i++)
            {
                string Task = sd.ImagingArray[i - 1, 0];
                string Obj = sd.ImagingArray[i - 1, 1];
                string Filter = sd.ImagingArray[i - 1, 2];
                string Bin = sd.ImagingArray[i - 1, 3];
                string Duration = sd.ImagingArray[i - 1, 4];
                string count = sd.ImagingArray[i - 1, 5];

                switch (Task)
                {

                    case "Empty":
                        break;

                    case "Slew to Object":
                        slewAndSolveInfo ss1;
                        this.UIThread(() => this.txtObject.Text = Obj);
                        getCoords(Obj);
                        this.UIThread(() => form.ImagingForm.chkAutoRepeat.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbLightFrame.Checked = true);
                        this.UIThread(() => form.ImagingForm.rbDarkFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbBiasFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbFlatFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.numBinning.Value = Properties.Settings.Default.platesolve_binning);
                        this.UIThread(() => form.ImagingForm.numSecs.Value = Properties.Settings.Default.platesolve_exposure_duration);
                        form.ImagingForm.numMins.Value = 0;
                        form.ImagingForm.numMs.Value = 0;
                        ss1.bin = 1;
                        ss1.count = 1;
                        ss1.duration = 1;
                        ss1.loopNum = i;
                        ss1.task = Task;
                        slewDelegate d2 = t_slewAndSolve;
                        IAsyncResult ar2 = d2.BeginInvoke(ss1, null, null);
                        while (!ar2.IsCompleted)
                        { System.Threading.Thread.Sleep(1000); }
                        break;

                    case "Light Frame":
                        //sd.filternum = (int)sd.filtertable[Filter];
                        slewAndSolveInfo ss2;
                        this.UIThread(() => form.ImagingForm.chkAutoRepeat.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbLightFrame.Checked = true);
                        this.UIThread(() => form.ImagingForm.rbDarkFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbBiasFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbFlatFrame.Checked = false);
                        this.UIThread(() =>form.ImagingForm.numBinning.Value = Convert.ToDecimal(Bin));
                        this.UIThread(() => form.ImagingForm.numSecs.Value = Convert.ToDecimal(Duration));
                        form.ImagingForm.numMins.Value = 0;
                        form.ImagingForm.numMs.Value = 0;
                        ss2.bin = Convert.ToInt16(Bin);
                        ss2.count = Convert.ToInt16(count);
                        ss2.duration = Convert.ToInt16(Duration);
                        ss2.loopNum = i;
                        ss2.task = Task;
                        slewDelegate d3 = t_image;
                        IAsyncResult ar3 = d3.BeginInvoke(ss2, null, null);
                        while (!ar3.IsCompleted)
                        { System.Threading.Thread.Sleep(1000); }
                        break;

                    case "Dark Frame":
                        slewAndSolveInfo ss3;
                        //sd.filternum = (int)sd.filtertable[Filter];
                        this.UIThread(() => form.ImagingForm.chkAutoRepeat.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbLightFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbDarkFrame.Checked = true);
                        this.UIThread(() => form.ImagingForm.rbBiasFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbFlatFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.numBinning.Value = Convert.ToDecimal(Bin));
                        this.UIThread(() => form.ImagingForm.numSecs.Value = Convert.ToDecimal(Duration));
                        form.ImagingForm.numMins.Value = 0;
                        form.ImagingForm.numMs.Value = 0;
                        ss3.bin = Convert.ToInt16(Bin);
                        ss3.count = Convert.ToInt16(count);
                        ss3.duration = Convert.ToInt16(Duration);
                        ss3.loopNum = i;
                        ss3.task = Task;
                        slewDelegate d4 = t_image;
                        IAsyncResult ar4 = d4.BeginInvoke(ss3, null, null);
                        while (!ar4.IsCompleted)
                        { System.Threading.Thread.Sleep(1000); }
                        break;

                    case "Flat Frame":
                        //sd.filternum = (int)sd.filtertable[Filter];
                        slewAndSolveInfo ss4;
                        this.UIThread(() => form.ImagingForm.chkAutoRepeat.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbLightFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbDarkFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbBiasFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbFlatFrame.Checked = true);
                        form.ImagingForm.numBinning.Value = Convert.ToDecimal(Bin);
                        form.ImagingForm.numSecs.Value = Convert.ToDecimal(Duration);
                        form.ImagingForm.numMins.Value = 0;
                        form.ImagingForm.numMs.Value = 0;
                        ss4.bin = Convert.ToInt16(Bin);
                        ss4.count = Convert.ToInt16(count);
                        ss4.duration = Convert.ToInt16(Duration);
                        ss4.loopNum = i;
                        ss4.task = Task;
                        slewDelegate d5 = t_image;
                        IAsyncResult ar5 = d5.BeginInvoke(ss4, null, null);
                        while (!ar5.IsCompleted)
                        { System.Threading.Thread.Sleep(1000); }
                        break;

                    case "Bias Frame":
                        //sd.filternum = (int)sd.filtertable[Filter];
                        slewAndSolveInfo ss5;
                        this.UIThread(() => form.ImagingForm.chkAutoRepeat.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbLightFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbDarkFrame.Checked = false);
                        this.UIThread(() => form.ImagingForm.rbBiasFrame.Checked = true);
                        this.UIThread(() => form.ImagingForm.rbFlatFrame.Checked = false);
                        form.ImagingForm.numBinning.Value = Convert.ToDecimal(Bin);
                        this.UIThread(() => form.ImagingForm.numSecs.Value = 0);
                        form.ImagingForm.numMins.Value = 0;
                        form.ImagingForm.numMs.Value = 0;
                        ss5.bin = Convert.ToInt16(Bin);
                        ss5.count = Convert.ToInt16(count);
                        ss5.duration = 0;
                        ss5.loopNum = i;
                        ss5.task = Task;
                        slewDelegate d6 = t_image;
                        IAsyncResult ar6 = d6.BeginInvoke(ss5, null, null);
                        while (!ar6.IsCompleted)
                        { System.Threading.Thread.Sleep(1000); }
                        break;

                    case "Pause":
                        slewAndSolveInfo ss6;
                        ss6.bin = 1;
                        ss6.count = 1;
                        ss6.duration = Convert.ToInt16(Duration);
                        ss6.loopNum = i;
                        ss6.task = Task;
                        slewDelegate d7 = t_pause;
                        IAsyncResult ar7 = d7.BeginInvoke(ss6, null, null);
                        while (!ar7.IsCompleted)
                        { System.Threading.Thread.Sleep(1000); }
                        break;

                }

            }
            clsStatics.isImagingRun = false;

        }


        void t_empty(slewAndSolveInfo _ss)
        {
        }

        void t_image(slewAndSolveInfo _ss)
        {
            
            slewAndSolveInfo ss = (slewAndSolveInfo)_ss;
            (new System.Threading.Thread(delegate() { SetTextUIThreadPatternParams("Starting " + ss.task + " task from task list\r\n", true); })).Start();

            for (int a = 1; a <= ss.count; a++)
            {
                int filecount = a;
                string filename="";
                string filepath;
                fitsHeader F = new fitsHeader();
                clsStatics.imageComplete = false;
                form.ImagingForm.takeAnImage(true);
                while (!clsStatics.imageComplete)
                {
                    System.Threading.Thread.Sleep(500);
                }
                clsStatics.imageComplete = false;

                filepath = Properties.Settings.Default.strImgRunFileLocation;
                if (filepath == "") filepath = "c:\\";
                if (Properties.Settings.Default.inclDateSubdirectoryChecked == true)
                {
                    filepath += "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                }
                if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);

                switch (ss.task)
                {
                    case "Light Frame":
                    case "Flat Frame":
                        filename = filepath + "\\" + ss.task + "-bin=" + ss.bin + "-Duration=" + ss.duration + "-Filter=TBA" + "-count=" + filecount + ".fit";
                        while (File.Exists(filename))
                        {
                            filecount++;
                            filename = filepath + "\\" + ss.task + "-bin=" + ss.bin + "-Duration=" + ss.duration + "-Filter=TBA" + "-count=" + filecount + ".fit";
                        }
                        break;
                    case "Dark Frame":
                        filename = filepath + "\\" + ss.task + "-bin=" + ss.bin + "-Duration=" + ss.duration + "-count=" + filecount + ".fit";
                        while (File.Exists(filename))
                        {
                            filecount++;
                            filename = filepath + "\\" + ss.task + "-bin=" + ss.bin + "-Duration=" + ss.duration + "-count=" + filecount + ".fit";
                        }
                        break;
                    case "Bias Frame":
                        filename = filepath + "\\" + ss.task + "-bin=" + ss.bin + "-count=" + filecount + ".fit";
                        while (File.Exists(filename))
                        {
                            filecount++;
                            filename = filepath + "\\" + ss.task + "-bin=" + ss.bin + "-count=" + filecount + ".fit";
                        }
                        break;
                }
                F.saveImageToFits(filename, sd);
                F = null;
            }
            (new System.Threading.Thread(delegate() { SetTextUIThreadPatternParams("Finished " + ss.task + " task from task list\r\n", true); })).Start();
        }

        void t_slewAndSolve(slewAndSolveInfo _ss)
        {
            slewAndSolveInfo ss = (slewAndSolveInfo)_ss;

            slewAndSolve(ss.loopNum);
        }

        void t_pause(slewAndSolveInfo _ss)
        {
            slewAndSolveInfo ss = (slewAndSolveInfo)_ss;
            string str = "Pausing operation for " + ss.duration + " seconds\r\n";
            updateStatusBox(str, true);
            System.Threading.Thread.Sleep(ss.duration * 1000);
            updateStatusBox("Pause Ended \r\n", true);
        }

        public struct slewAndSolveInfo
        {
            public string task;
            public int count;
            public int bin;
            public int duration;
            public int loopNum;
        }

        

    }
}
