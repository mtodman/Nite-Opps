using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nite_Opps
{
    public partial class frmConfig : Form
    {
        clsSharedData sd;
        ASCOM.Utilities.Chooser chooser;
        clsForms form;
        User_Profile Prof = new User_Profile();


        public frmConfig()
        {
            InitializeComponent();
            //loadSharedData();
            writeConfigValuesToScreen();
        }

        public frmConfig(ref clsSharedData d, ref clsForms f)
        {
            InitializeComponent();
            sd = d;
            form = f;
            //loadSharedData();
            writeConfigValuesToScreen();
        }


        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            updateFromScreen();
            //Prof.writeValuesToReg();
            //Prof.writeValuesToSettings();
            //loadSharedData();
            MessageBox.Show("Config Saved");
            
        }

        private void updateFromScreen()
        {
            try
            {
                Properties.Settings.Default.GSC_Location = this.txtCSGLocation.Text;
                Properties.Settings.Default.your_name = this.txtYourName.Text;
                Properties.Settings.Default.location_name = this.txtLocationName.Text;
                Properties.Settings.Default.site_lat_deg = Convert.ToInt16(this.txtLatDeg.Text);
                Properties.Settings.Default.site_lat_min = Convert.ToInt16(this.txtLatMin.Text);
                Properties.Settings.Default.site_lat_sec = Convert.ToInt16(this.txtLatSec.Text);
                Properties.Settings.Default.site_lat_ns = this.cmbLat.Text;
                Properties.Settings.Default.site_long_deg = Convert.ToInt16(this.txtLongDeg.Text);
                Properties.Settings.Default.site_long_min = Convert.ToInt16(this.txtLongMin.Text);
                Properties.Settings.Default.site_long_sec = Convert.ToInt16(this.txtLongSec.Text);
                Properties.Settings.Default.site_long_ew = this.cmbLong.Text;
                Properties.Settings.Default.site_altitude = Convert.ToInt16(this.txtSiteAltitude.Text);
                Properties.Settings.Default.site_pressure = Convert.ToInt16(this.txtSitePressure.Text);
                Properties.Settings.Default.site_temperature = Convert.ToInt16(this.txtSiteTemperature.Text);
                Properties.Settings.Default.imaging_telescope = txtImagingTelescopeName.Text;
                Properties.Settings.Default.imaging_telescope_ota_dia = Convert.ToInt16(txtImagingAperture.Text);
                Properties.Settings.Default.imaging_telescope_focal_length = Convert.ToInt16(txtImagingFL.Text);
                Properties.Settings.Default.guiding_telescope = txtGuidingTelescopeName.Text;
                Properties.Settings.Default.guiding_telescope_ota_dia = Convert.ToInt16(txtGuidingAperture.Text);
                Properties.Settings.Default.guiding_telescope_focal_length = Convert.ToInt16(txtGuidingFL.Text);
                Properties.Settings.Default.filter1 = txtFilter1.Text;
                Properties.Settings.Default.filter2 = txtFilter2.Text;
                Properties.Settings.Default.filter3 = txtFilter3.Text;
                Properties.Settings.Default.filter4 = txtFilter4.Text;
                Properties.Settings.Default.filter5 = txtFilter5.Text;
                Properties.Settings.Default.exposure_delay = Convert.ToInt16(txtImagingExposureDelay.Text);
                Properties.Settings.Default.imaging_camera_download_time = Convert.ToInt16(txtImagingCamDownloadTime.Text);
                //Properties.Settings.Default.strImgRunFileLocation = txtFileLocation.Text;
                Properties.Settings.Default.mount_control = "ASCOM";
                

                if (rbUsePHDGuiding.Checked)
                { Properties.Settings.Default.guiding_program = "PHD"; }
                else { Properties.Settings.Default.guiding_program = "MaximDL"; }

                if (rbUseFocusmaxFocus.Checked)
                { Properties.Settings.Default.autofocus_program = "FocusMax"; }
                else { Properties.Settings.Default.autofocus_program = "MaximDL"; }

                if (rbUseMiniSAC.Checked)
                { Properties.Settings.Default.object_database = "MiniSAC"; }
                else { Properties.Settings.Default.object_database = "Generic"; }

                if (rbUsePinpointSolve.Checked)
                { Properties.Settings.Default.platesolve_program = "PinPoint"; }
                else { Properties.Settings.Default.platesolve_program = "Elbrus"; }

                if (chkFormOnTop.Checked)
                { Properties.Settings.Default.keep_window_on_top = true; }
                else { Properties.Settings.Default.keep_window_on_top = false; }

                //if (chkInclDateSubdirectory.Checked)
                //{ Properties.Settings.Default.inclDateSubdirectoryChecked = "true"; }
                //else { Properties.Settings.Default.inclDateSubdirectoryChecked = "false"; }


                Properties.Settings.Default.platesolve_exposure_duration = Convert.ToInt16(txtDuration.Text);
                Properties.Settings.Default.platesolve_binning = Convert.ToInt16(cmbBinning.Text);
                Properties.Settings.Default.platesolve_max_pointing_error = Convert.ToInt16(txtMaxError.Text);
                Properties.Settings.Default.max_num_platesolves = Convert.ToInt16(txtNumSolves.Text);

                Properties.Settings.Default.Save();
            }
            catch
            {
            }

        }



        private void writeConfigValuesToScreen()
        {
            txtCSGLocation.Text = Properties.Settings.Default.GSC_Location;
            txtYourName.Text = Properties.Settings.Default.your_name;
            txtLocationName.Text = Properties.Settings.Default.location_name;
            txtLatDeg.Text = Properties.Settings.Default.site_lat_deg.ToString();
            txtLatMin.Text = Properties.Settings.Default.site_lat_min.ToString();
            txtLatSec.Text = Properties.Settings.Default.site_lat_sec.ToString();
            cmbLat.Text = Properties.Settings.Default.site_lat_ns;
            txtLongDeg.Text = Properties.Settings.Default.site_long_deg.ToString();
            txtLongMin.Text = Properties.Settings.Default.site_long_min.ToString();
            txtLongSec.Text = Properties.Settings.Default.site_long_sec.ToString();
            cmbLong.Text = Properties.Settings.Default.site_long_ew;
            txtSiteAltitude.Text = Properties.Settings.Default.site_altitude.ToString();
            txtSitePressure.Text = Properties.Settings.Default.site_pressure.ToString();
            txtSiteTemperature.Text = Properties.Settings.Default.site_temperature.ToString();
            txtImagingTelescopeName.Text = Properties.Settings.Default.imaging_telescope;
            txtImagingAperture.Text = Properties.Settings.Default.imaging_telescope_ota_dia.ToString();
            txtImagingFL.Text = Properties.Settings.Default.imaging_telescope_focal_length.ToString();
            txtGuidingTelescopeName.Text = Properties.Settings.Default.guiding_telescope;
            txtGuidingAperture.Text = Properties.Settings.Default.guiding_telescope_ota_dia.ToString();
            txtGuidingFL.Text = Properties.Settings.Default.guiding_telescope_focal_length.ToString();
            txtFilter1.Text = Properties.Settings.Default.filter1;
            txtFilter2.Text = Properties.Settings.Default.filter2;
            txtFilter3.Text = Properties.Settings.Default.filter3;
            txtFilter4.Text = Properties.Settings.Default.filter4;
            txtFilter5.Text = Properties.Settings.Default.filter5;
            txtImagingExposureDelay.Text = Properties.Settings.Default.exposure_delay.ToString();
            txtImagingCamDownloadTime.Text = Properties.Settings.Default.imaging_camera_download_time.ToString();
            //txtFileLocation.Text = Properties.Settings.Default.strImgRunFileLocation;
            
            if (Properties.Settings.Default.guiding_program == "PHD")
            { rbUsePHDGuiding.Checked = true; }
            else { rbUseMaximGuiding.Checked = true; }

            if (Properties.Settings.Default.autofocus_program == "FocusMax")
            { rbUseFocusmaxFocus.Checked = true; }
            else { rbUseMaximdlFocus.Checked = true; }

            if (Properties.Settings.Default.object_database == "MiniSAC")
            { rbUseMiniSAC.Checked = true; Properties.Settings.Default.object_database = "MiniSAC"; }
            else { rbUseGenericDB.Checked = true; Properties.Settings.Default.object_database = "Generic"; }

            if (Properties.Settings.Default.platesolve_program == "PinPoint")
            { rbUsePinpointSolve.Checked = true; }
            else { rbUseElbrusSolve.Checked = true; }

            if (Properties.Settings.Default.keep_window_on_top == true)
            { chkFormOnTop.Checked = true; }
            else { chkFormOnTop.Checked = false; }

            //if (Properties.Settings.Default.inclDateSubdirectoryChecked == "true")
            //{ chkInclDateSubdirectory.Checked = true; }
            //else { chkInclDateSubdirectory.Checked = false; }


            txtDuration.Text = Properties.Settings.Default.platesolve_exposure_duration.ToString();
            cmbBinning.Text = Properties.Settings.Default.platesolve_binning.ToString();
            txtMaxError.Text = Properties.Settings.Default.platesolve_max_pointing_error.ToString();
            txtNumSolves.Text = Properties.Settings.Default.max_num_platesolves.ToString();
        }


        private void btnSelectScope_Click(object sender, EventArgs e)
        {
            chooser = new ASCOM.Utilities.Chooser();
            chooser.DeviceType = "Telescope";
            if (Properties.Settings.Default.ScopeProdID != null)
            {
                Properties.Settings.Default.ScopeProdID = chooser.Choose(Properties.Settings.Default.ScopeProdID);
                //sd.conf.ScopeProdID = Properties.Settings.Default.ScopeProdID;
            }
            else
            {
                Properties.Settings.Default.ScopeProdID = chooser.Choose();
                //sd.conf.ScopeProdID = Properties.Settings.Default.ScopeProdID;
            }
        }

        private void btnChooseFilterWheel_Click(object sender, EventArgs e)
        {
            chooser = new ASCOM.Utilities.Chooser();
            chooser.DeviceType = "FilterWheel";
            if (Properties.Settings.Default.FilterWheelProdID != null)
            {
                Properties.Settings.Default.FilterWheelProdID = chooser.Choose(Properties.Settings.Default.FilterWheelProdID);
            }
            else
            {
                Properties.Settings.Default.FilterWheelProdID = chooser.Choose();
            }
        }

        private void btnSelectGuidingCamera_Click(object sender, EventArgs e)
        {
            chooser = new ASCOM.Utilities.Chooser();
            chooser.DeviceType = "Camera";
            if (Properties.Settings.Default.GuidingCamProdID != null)
            {
                Properties.Settings.Default.GuidingCamProdID = chooser.Choose(Properties.Settings.Default.GuidingCamProdID);
            }
            else
            {
                Properties.Settings.Default.GuidingCamProdID = chooser.Choose();
            }
        }

        private void btnSelectImagingCamera_Click(object sender, EventArgs e)
        {
            chooser = new ASCOM.Utilities.Chooser();
            chooser.DeviceType = "Camera";
            if (Properties.Settings.Default.ImagingCamProdID != null)
            {
                Properties.Settings.Default.ImagingCamProdID = chooser.Choose(Properties.Settings.Default.ImagingCamProdID);
            }
            else
            {
                Properties.Settings.Default.ImagingCamProdID = chooser.Choose();
            }

        }

        private void btnSetGSCLocation_Click(object sender, EventArgs e)
        {
            //Set the folder browser dialogue properties
            {
                FolderBrowserDialog1.Description = "Select the base directory of the GSC catalogue";
                FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
                FolderBrowserDialog1.ShowNewFolderButton = false;
            }
            if (FolderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.GSC_Location = FolderBrowserDialog1.SelectedPath;
                txtCSGLocation.Text = Properties.Settings.Default.GSC_Location;
            }
        }



    }
}
