using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nite_Opps
{
    public partial class frmImageRun : Form
    {
        public clsSharedData sd;
        clsForms form;
        User_Profile Prof = new User_Profile();

        public frmImageRun()
        {
            InitializeComponent();
        }

        public frmImageRun(ref clsSharedData d, ref clsForms f)
        {
            InitializeComponent();
            sd = d;
            form = f;
            chkArrayFileDirExists();
        }

        int intTaskNumber;
        ComboBox[] cmbTask_;
        TextBox[] txtObject_;
        ComboBox[] cmbFilter_;
        ComboBox[] cmbBinning_;
        TextBox[] txtDuration_;
        TextBox[] txtCount_;
        string arrayfiledir = System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "data";
        public string arrayfile = System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar + "arrayfile.txt";
        string[] data_array;
        int counter = 0;
        //int intArraySize = 15;
        //string[,] ImagingArray = new string[15, 6];

        string strFileLocation;

        private void chkArrayFileDirExists()
        {
            if(!Directory.Exists(arrayfiledir))
            {
                Directory.CreateDirectory(arrayfiledir);
            }
        }

        private void LoadImageRunFields()
        {
            cmbTask_ = new ComboBox[] {
		    cmbTask1,
		    cmbTask2,
		    cmbTask3,
		    cmbTask4,
		    cmbTask5,
		    cmbTask6,
		    cmbTask7,
		    cmbTask8,
		    cmbTask9,
		    cmbTask10,
		    cmbTask11,
		    cmbTask12,
		    cmbTask13,
		    cmbTask14,
		    cmbTask15
	    };

            txtObject_ = new TextBox[] {
		    txtObject1,
		    txtObject2,
		    txtObject3,
		    txtObject4,
		    txtObject5,
		    txtObject6,
		    txtObject7,
		    txtObject8,
		    txtObject9,
		    txtObject10,
		    txtObject11,
		    txtObject12,
		    txtObject13,
		    txtObject14,
		    txtObject15
	    };

            cmbFilter_ = new ComboBox[] {
		    cmbFilter1,
		    cmbFilter2,
		    cmbFilter3,
		    cmbFilter4,
		    cmbFilter5,
		    cmbFilter6,
		    cmbFilter7,
		    cmbFilter8,
		    cmbFilter9,
		    cmbFilter10,
		    cmbFilter11,
		    cmbFilter12,
		    cmbFilter13,
		    cmbFilter14,
		    cmbFilter15
	    };

            cmbBinning_ = new ComboBox[] {
		    cmbBinning1,
		    cmbBinning2,
		    cmbBinning3,
		    cmbBinning4,
		    cmbBinning5,
		    cmbBinning6,
		    cmbBinning7,
		    cmbBinning8,
		    cmbBinning9,
		    cmbBinning10,
		    cmbBinning11,
		    cmbBinning12,
		    cmbBinning13,
		    cmbBinning14,
		    cmbBinning15
	    };

            txtDuration_ = new TextBox[] {
		    txtDuration1,
		    txtDuration2,
		    txtDuration3,
		    txtDuration4,
		    txtDuration5,
		    txtDuration6,
		    txtDuration7,
		    txtDuration8,
		    txtDuration9,
		    txtDuration10,
		    txtDuration11,
		    txtDuration12,
		    txtDuration13,
		    txtDuration14,
		    txtDuration15
	    };

            txtCount_ = new TextBox[] {
		    txtCount1,
		    txtCount2,
		    txtCount3,
		    txtCount4,
		    txtCount5,
		    txtCount6,
		    txtCount7,
		    txtCount8,
		    txtCount9,
		    txtCount10,
		    txtCount11,
		    txtCount12,
		    txtCount13,
		    txtCount14,
		    txtCount15
	    };


            //populate the the combo boxes with the correct values

            for (int i = 0; i <= sd.intArraySize - 1; i++)
            {
                cmbTask_[i].Items.Add("Slew to Object");
                cmbTask_[i].Items.Add("Light Frame");
                cmbTask_[i].Items.Add("Dark Frame");
                cmbTask_[i].Items.Add("Flat Frame");
                cmbTask_[i].Items.Add("Bias Frame");
                cmbTask_[i].Items.Add("Pause");
                cmbTask_[i].Items.Add("Empty");
                cmbTask_[i].Text = "Empty";
                cmbTask_[i].Tag = i + 1;
                cmbTask_[i].SelectedIndexChanged += new System.EventHandler(cmbTask_SelectedIndexChanged);

                cmbBinning_[i].Items.Add("1");
                cmbBinning_[i].Items.Add("2");
                cmbBinning_[i].Items.Add("3");
                cmbBinning_[i].Items.Add("4");
                cmbBinning_[i].Text = "1";

                cmbFilter_[i].Items.Add(Properties.Settings.Default.filter1);
                cmbFilter_[i].Items.Add(Properties.Settings.Default.filter2);
                cmbFilter_[i].Items.Add(Properties.Settings.Default.filter3);
                cmbFilter_[i].Items.Add(Properties.Settings.Default.filter4);
                cmbFilter_[i].Items.Add(Properties.Settings.Default.filter5);

            }

            txtFileLocation.Text = strFileLocation;


            initcontrols();
            FromFileToArray();
            LoadFromArray();


        }

        private void initcontrols()
        {
            for (int i = 0; i <= sd.intArraySize - 1; i++)
            {
                for (int count = 0; count <= 5; count++)
                {
                    sd.ImagingArray[i, count] = "Empty";
                }
                txtObject_[i].Enabled = false;
                cmbFilter_[i].Enabled = false;
                cmbBinning_[i].Enabled = false;
                txtDuration_[i].Enabled = false;
                txtCount_[i].Enabled = false;

            }
        }

        private void cmbTask_SelectedIndexChanged(System.Object sender, EventArgs e)
        {
            ComboBox curcmbbox = (System.Windows.Forms.ComboBox)sender;
	            int position = (int)curcmbbox.Tag - 1;
	            string action = curcmbbox.Text;
	            switch (action) {
		            case "Slew to Object":
			            txtObject_[position].Enabled = true;
			            cmbFilter_[position].Enabled = false;
			            cmbBinning_[position].Enabled = false;
			            txtDuration_[position].Enabled = false;
			            txtCount_[position].Enabled = false;

			            break;

		            case "Light Frame":
			            txtObject_[position].Enabled = false;
			            cmbFilter_[position].Enabled = true;
			            cmbBinning_[position].Enabled = true;
			            txtDuration_[position].Enabled = true;
			            txtCount_[position].Enabled = true;

			            break;

		            case "Dark Frame":
			            txtObject_[position].Enabled = false;
			            cmbFilter_[position].Enabled = false;
			            cmbBinning_[position].Enabled = true;
			            txtDuration_[position].Enabled = true;
			            txtCount_[position].Enabled = true;

			            break;
		            case "Flat Frame":
			            txtObject_[position].Enabled = false;
			            cmbFilter_[position].Enabled = true;
			            cmbBinning_[position].Enabled = true;
			            txtDuration_[position].Enabled = true;
			            txtCount_[position].Enabled = true;

			            break;
		            case "Bias Frame":
			            txtObject_[position].Enabled = false;
			            cmbFilter_[position].Enabled = false;
			            cmbBinning_[position].Enabled = true;
			            txtDuration_[position].Enabled = false;
			            txtCount_[position].Enabled = true;

			            break;
		            case "Pause":
			            txtObject_[position].Enabled = false;
			            cmbFilter_[position].Enabled = false;
			            cmbBinning_[position].Enabled = false;
			            txtDuration_[position].Enabled = true;
			            txtCount_[position].Enabled = false;

			            break;
		            case "Empty":
			            txtObject_[position].Enabled = false;
			            cmbFilter_[position].Enabled = false;
			            cmbBinning_[position].Enabled = false;
			            txtDuration_[position].Enabled = false;
			            txtCount_[position].Enabled = false;

			            break;
	            }



        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Write the following information to the registry
            //image file location
            strFileLocation = txtFileLocation.Text;
            //Prof.strImgRunFileLocation = strFileLocation;
            Properties.Settings.Default.strImgRunFileLocation = strFileLocation;
            //Prof.WriteToReg("Image Run File Location", strFileLocation);

            //Should we include date subdirectory in image file path?
            if (chkInclDateSubdirectory.Checked == true)
            {
                //Prof.inclDateSubdirectoryChecked = "true";
                Properties.Settings.Default.inclDateSubdirectoryChecked = true;
                //Prof.WriteToReg("Include Date Subdirectory", "true");
            }
            else
            {
                //Prof.inclDateSubdirectoryChecked = "false";
                Properties.Settings.Default.inclDateSubdirectoryChecked = false;
                //Prof.WriteToReg("Include Date Subdirectory", "false");
            }

            if (SaveToArray())
            {
                FromArrayToFile();
                MessageBox.Show("saved");
            }
        }

        public bool SaveToArray()
        {
            //Take the values from the table of textboxes and comboboxes 
            //and use them to populate an array (ImagingArray()).
            for (int i = 0; i <= sd.intArraySize - 1; i++)
            {
                sd.ImagingArray[i, 0] = cmbTask_[i].Text;

                //The below switch case ensures that numeric values have been entered into 
                //the duration and count fields
                switch (sd.ImagingArray[i, 0])
                {
                    case "Empty":
                        break;
                    case "Pause":

                        if (!ValidNumber(txtDuration_[i].Text))
                        {
                            MessageBox.Show("Invalid Input. Please ener a numeric value only");
                            txtDuration_[i].Focus();
                            return false;
                        }
                        break;
                    case "Light Frame":
                    case "Dark Frame":
                    case "Flat Frame":
                        if (!ValidNumber(txtDuration_[i].Text))
                        {
                            MessageBox.Show("Invalid Input. Please ener a numeric value only");
                            txtDuration_[i].Focus();
                            return false;
                        }
                        if (!ValidNumber(txtCount_[i].Text))
                        {
                            MessageBox.Show("Invalid Input. Please ener a numeric value only");
                            txtCount_[i].Focus();
                            return false;
                        }
                        break;
                    case "Bias Frame":
                        if (!ValidNumber(txtCount_[i].Text))
                        {
                            MessageBox.Show("Invalid Input. Please ener a numeric value only");
                            txtCount_[i].Focus();
                            return false;
                        }
                        break;
                }

                if (txtObject_[i].Enabled == true)
                {
                    sd.ImagingArray[i, 1] = txtObject_[i].Text;
                }
                else
                {
                    sd.ImagingArray[i, 1] = "";
                }

                if (cmbFilter_[i].Enabled == true)
                {
                    sd.ImagingArray[i, 2] = cmbFilter_[i].Text;
                }
                else
                {
                    sd.ImagingArray[i, 2] = "";
                }

                if (cmbBinning_[i].Enabled == true)
                {
                    sd.ImagingArray[i, 3] = cmbBinning_[i].Text;
                }
                else
                {
                    sd.ImagingArray[i, 3] = "";
                }

                if (txtDuration_[i].Enabled == true)
                {
                    sd.ImagingArray[i, 4] = txtDuration_[i].Text;
                }
                else
                {
                    sd.ImagingArray[i, 4] = "";
                }

                if (txtCount_[i].Enabled == true)
                {
                    sd.ImagingArray[i, 5] = txtCount_[i].Text;
                }
                else
                {
                    sd.ImagingArray[i, 5] = "";
                }
            }
            return true;
        }

        public void LoadFromArray()
        {
            //This subroutine takes the values from the array (ImagingArray) and uses the to
            //populate the various comboboxes and textboxes on the form.
            for (int i = 1; i <= sd.intArraySize; i++)
            {
                cmbTask_[i - 1].Text = sd.ImagingArray[i - 1, 0];
                txtObject_[i - 1].Text = sd.ImagingArray[i - 1, 1];
                cmbFilter_[i - 1].Text = sd.ImagingArray[i - 1, 2];
                cmbBinning_[i - 1].Text = sd.ImagingArray[i - 1, 3];
                txtDuration_[i - 1].Text = sd.ImagingArray[i - 1, 4];
                txtCount_[i - 1].Text = sd.ImagingArray[i - 1, 5];
            }
        }

        public void FromArrayToFile()
        {
            // This subroutine reads all values from the array (ImagingArray)
            // and writes it to the file (arrayfile.txt) in a csv format.
            // The File.Delete command zero's the file so that it's not appended to.
            if (File.Exists(arrayfile))
            {
                File.Delete(arrayfile);
            }

            using (StreamWriter objWriter = File.AppendText(arrayfile))
            {
                //Auto creates the file

                for (int i = 1; i <= sd.intArraySize; i++)
                {
                    objWriter.Write(sd.ImagingArray[i - 1, 0]);
                    objWriter.Write(",");
                    objWriter.Write(sd.ImagingArray[i - 1, 1]);
                    objWriter.Write(",");
                    objWriter.Write(sd.ImagingArray[i - 1, 2]);
                    objWriter.Write(",");
                    objWriter.Write(sd.ImagingArray[i - 1, 3]);
                    objWriter.Write(",");
                    objWriter.Write(sd.ImagingArray[i - 1, 4]);
                    objWriter.Write(",");
                    objWriter.Write(sd.ImagingArray[i - 1, 5]);
                    objWriter.WriteLine();
                }
                objWriter.Close();
            }

        }

        public void FromFileToArray()
        {
            // This subroutine reads all values from the file (arrayfile.txt) in a csv format
            // and writes it to the array (ImagingArray).
            if (File.Exists(arrayfile))
            {
                int linecount = 0;
                StreamReader oRead = new StreamReader(arrayfile);
                while (!oRead.EndOfStream)
                {
                    string LineIn = oRead.ReadLine();
                    data_array = LineIn.Split(new Char[] { ',' });
                    for (int i = 0; i <= data_array.Length - 1; i++)
                    {
                        sd.ImagingArray[linecount, i] = data_array[i];
                    }
                    linecount += 1;
                }
                oRead.Close();
            }
        }

        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            SaveToArray();
            FromArrayToUserFile();
        }

        private void btnLoadFromFile_Click(object sender, EventArgs e)
        {
            FromUserFileToArray();
            LoadFromArray();
        }

        public void FromUserFileToArray()
        {
            // Read all values from the file selected from the openfiledialog control in a csv format
            // and writes it to the array (ImagingArray).
            string filename = "";

            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = OpenFileDialog1.FileName;
            }

            if (File.Exists(filename))
            {
                int linecount = 0;
                StreamReader oRead = new StreamReader(filename);
                while (!oRead.EndOfStream)
                {
                    string LineIn = oRead.ReadLine();

                    data_array = LineIn.Split(new Char[] { ',' });
                    for (int i = 0; i <= data_array.Length - 1; i++)
                    {
                        sd.ImagingArray[linecount, i] = data_array[i];
                    }
                    linecount += 1;
                }
                oRead.Close();
            }
        }

        public void FromArrayToUserFile()
        {
            // This subroutine reads all values from the array (ImagingArray)
            // and writes it to the user selected file in a csv format.
            // The File.Delete command zero's the file so that it's not appended to.
            string filename = "";

            if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = SaveFileDialog1.FileName;
            }

            if (filename == "") return;

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (StreamWriter objWriter = File.AppendText(filename))
            {
                //Auto creates the file

                for (int i = 1; i <= sd.intArraySize; i++)
                {
                    objWriter.Write(sd.ImagingArray[i - 1, 0]);
                    objWriter.Write(",");
                    objWriter.Write(sd.ImagingArray[i - 1, 1]);
                    objWriter.Write(",");
                    objWriter.Write(sd.ImagingArray[i - 1, 2]);
                    objWriter.Write(",");
                    objWriter.Write(sd.ImagingArray[i - 1, 3]);
                    objWriter.Write(",");
                    objWriter.Write(sd.ImagingArray[i - 1, 4]);
                    objWriter.Write(",");
                    objWriter.Write(sd.ImagingArray[i - 1, 5]);
                    objWriter.WriteLine();

                }
                objWriter.Close();
            }
        }

        private void btnSetFileLocation_Click(object sender, EventArgs e)
        {
            //Set the folder browser dialogue properties
            {
                FolderBrowserDialog1.Description = "Select the location for saving image files";
                //FolderBrowserDialog1.ShowNewFolderButton = false;
            }
            if (FolderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                strFileLocation = FolderBrowserDialog1.SelectedPath;
                txtFileLocation.Text = strFileLocation;
                Properties.Settings.Default.strImgRunFileLocation = strFileLocation;
            }
        }

        private void frmImageRun_Load(object sender, EventArgs e)
        {
            LoadImageRunFields();
            if (Properties.Settings.Default.inclDateSubdirectoryChecked == true)
            { chkInclDateSubdirectory.Checked = true; }
            else
            {chkInclDateSubdirectory.Checked = false; }
            txtFileLocation.Text = Properties.Settings.Default.strImgRunFileLocation;
            Properties.Settings.Default.strImgRunFileLocation = Properties.Settings.Default.strImgRunFileLocation;
            Properties.Settings.Default.inclDateSubdirectoryChecked = Properties.Settings.Default.inclDateSubdirectoryChecked;
        }

        private bool ValidNumber(string s)
        {
            int i;
            return int.TryParse(s, out i);
        }

    }
}
