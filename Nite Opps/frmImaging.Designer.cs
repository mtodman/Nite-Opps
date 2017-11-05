namespace Nite_Opps
{
    partial class frmImaging
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerUpdateCamState = new System.Windows.Forms.Timer(this.components);
            this.pnlImage = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.rbFlatFrame = new System.Windows.Forms.RadioButton();
            this.numBinning = new System.Windows.Forms.NumericUpDown();
            this.label75 = new System.Windows.Forms.Label();
            this.rbBiasFrame = new System.Windows.Forms.RadioButton();
            this.rbDarkFrame = new System.Windows.Forms.RadioButton();
            this.rbLightFrame = new System.Windows.Forms.RadioButton();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.lblimageTimeRemaining = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.numMs = new System.Windows.Forms.NumericUpDown();
            this.numSecs = new System.Windows.Forms.NumericUpDown();
            this.numMins = new System.Windows.Forms.NumericUpDown();
            this.label85 = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.label83 = new System.Windows.Forms.Label();
            this.chkAutoRepeat = new System.Windows.Forms.CheckBox();
            this.lblCameraStatus = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.numTargetTemp = new System.Windows.Forms.NumericUpDown();
            this.lblCoolerPower = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.lblCameraTemp = new System.Windows.Forms.Label();
            this.chkCoolerOn = new System.Windows.Forms.CheckBox();
            this.label79 = new System.Windows.Forms.Label();
            this.progressbarTimer = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pnlImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBinning)).BeginInit();
            this.groupBox18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSecs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMins)).BeginInit();
            this.groupBox19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetTemp)).BeginInit();
            this.SuspendLayout();
            // 
            // timerUpdateCamState
            // 
            this.timerUpdateCamState.Interval = 250;
            this.timerUpdateCamState.Tick += new System.EventHandler(this.timerUpdateCamState_Tick);
            // 
            // pnlImage
            // 
            this.pnlImage.AutoScroll = true;
            this.pnlImage.Controls.Add(this.pictureBox1);
            this.pnlImage.Location = new System.Drawing.Point(218, 14);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(798, 628);
            this.pnlImage.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(772, 677);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.rbFlatFrame);
            this.groupBox20.Controls.Add(this.numBinning);
            this.groupBox20.Controls.Add(this.label75);
            this.groupBox20.Controls.Add(this.rbBiasFrame);
            this.groupBox20.Controls.Add(this.rbDarkFrame);
            this.groupBox20.Controls.Add(this.rbLightFrame);
            this.groupBox20.Location = new System.Drawing.Point(12, 276);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(200, 100);
            this.groupBox20.TabIndex = 6;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Frame Type";
            // 
            // rbFlatFrame
            // 
            this.rbFlatFrame.AutoSize = true;
            this.rbFlatFrame.Location = new System.Drawing.Point(100, 42);
            this.rbFlatFrame.Name = "rbFlatFrame";
            this.rbFlatFrame.Size = new System.Drawing.Size(68, 17);
            this.rbFlatFrame.TabIndex = 141;
            this.rbFlatFrame.TabStop = true;
            this.rbFlatFrame.Text = "flat frame";
            this.rbFlatFrame.UseVisualStyleBackColor = true;
            this.rbFlatFrame.CheckedChanged += new System.EventHandler(this.rbFlatFrame_CheckedChanged);
            // 
            // numBinning
            // 
            this.numBinning.Location = new System.Drawing.Point(38, 72);
            this.numBinning.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numBinning.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBinning.Name = "numBinning";
            this.numBinning.Size = new System.Drawing.Size(31, 20);
            this.numBinning.TabIndex = 140;
            this.numBinning.Tag = "1";
            this.numBinning.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Location = new System.Drawing.Point(10, 74);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(22, 13);
            this.label75.TabIndex = 140;
            this.label75.Text = "Bin";
            // 
            // rbBiasFrame
            // 
            this.rbBiasFrame.AutoSize = true;
            this.rbBiasFrame.Location = new System.Drawing.Point(100, 19);
            this.rbBiasFrame.Name = "rbBiasFrame";
            this.rbBiasFrame.Size = new System.Drawing.Size(73, 17);
            this.rbBiasFrame.TabIndex = 2;
            this.rbBiasFrame.TabStop = true;
            this.rbBiasFrame.Text = "bias frame";
            this.rbBiasFrame.UseVisualStyleBackColor = true;
            this.rbBiasFrame.CheckedChanged += new System.EventHandler(this.rbBiasFrame_CheckedChanged);
            // 
            // rbDarkFrame
            // 
            this.rbDarkFrame.AutoSize = true;
            this.rbDarkFrame.Location = new System.Drawing.Point(12, 42);
            this.rbDarkFrame.Name = "rbDarkFrame";
            this.rbDarkFrame.Size = new System.Drawing.Size(75, 17);
            this.rbDarkFrame.TabIndex = 1;
            this.rbDarkFrame.TabStop = true;
            this.rbDarkFrame.Text = "dark frame";
            this.rbDarkFrame.UseVisualStyleBackColor = true;
            this.rbDarkFrame.CheckedChanged += new System.EventHandler(this.rbDarkFrame_CheckedChanged);
            // 
            // rbLightFrame
            // 
            this.rbLightFrame.AutoSize = true;
            this.rbLightFrame.Checked = true;
            this.rbLightFrame.Location = new System.Drawing.Point(12, 19);
            this.rbLightFrame.Name = "rbLightFrame";
            this.rbLightFrame.Size = new System.Drawing.Size(73, 17);
            this.rbLightFrame.TabIndex = 0;
            this.rbLightFrame.TabStop = true;
            this.rbLightFrame.Text = "light frame";
            this.rbLightFrame.UseVisualStyleBackColor = true;
            this.rbLightFrame.CheckedChanged += new System.EventHandler(this.rbLightFrame_CheckedChanged);
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.lblimageTimeRemaining);
            this.groupBox18.Controls.Add(this.progressBar1);
            this.groupBox18.Controls.Add(this.numMs);
            this.groupBox18.Controls.Add(this.numSecs);
            this.groupBox18.Controls.Add(this.numMins);
            this.groupBox18.Controls.Add(this.label85);
            this.groupBox18.Controls.Add(this.label84);
            this.groupBox18.Controls.Add(this.label83);
            this.groupBox18.Controls.Add(this.chkAutoRepeat);
            this.groupBox18.Controls.Add(this.lblCameraStatus);
            this.groupBox18.Controls.Add(this.label74);
            this.groupBox18.Controls.Add(this.btnAbort);
            this.groupBox18.Controls.Add(this.btnStop);
            this.groupBox18.Controls.Add(this.btnStart);
            this.groupBox18.Location = new System.Drawing.Point(12, 12);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(200, 258);
            this.groupBox18.TabIndex = 5;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Camera Control";
            // 
            // lblimageTimeRemaining
            // 
            this.lblimageTimeRemaining.AutoSize = true;
            this.lblimageTimeRemaining.Location = new System.Drawing.Point(34, 207);
            this.lblimageTimeRemaining.Name = "lblimageTimeRemaining";
            this.lblimageTimeRemaining.Size = new System.Drawing.Size(0, 13);
            this.lblimageTimeRemaining.TabIndex = 142;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 229);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(182, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 141;
            // 
            // numMs
            // 
            this.numMs.Location = new System.Drawing.Point(125, 117);
            this.numMs.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMs.Name = "numMs";
            this.numMs.Size = new System.Drawing.Size(63, 20);
            this.numMs.TabIndex = 140;
            // 
            // numSecs
            // 
            this.numSecs.Location = new System.Drawing.Point(125, 87);
            this.numSecs.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numSecs.Name = "numSecs";
            this.numSecs.Size = new System.Drawing.Size(63, 20);
            this.numSecs.TabIndex = 139;
            // 
            // numMins
            // 
            this.numMins.Location = new System.Drawing.Point(125, 54);
            this.numMins.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numMins.Name = "numMins";
            this.numMins.Size = new System.Drawing.Size(63, 20);
            this.numMins.TabIndex = 138;
            // 
            // label85
            // 
            this.label85.AutoSize = true;
            this.label85.Location = new System.Drawing.Point(10, 56);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(112, 13);
            this.label85.TabIndex = 136;
            this.label85.Text = "Exposure Length (min)";
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.Location = new System.Drawing.Point(10, 89);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(113, 13);
            this.label84.TabIndex = 134;
            this.label84.Text = "Exposure Length (sec)";
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(10, 119);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(109, 13);
            this.label83.TabIndex = 132;
            this.label83.Text = "Exposure Length (ms)";
            // 
            // chkAutoRepeat
            // 
            this.chkAutoRepeat.AutoSize = true;
            this.chkAutoRepeat.Location = new System.Drawing.Point(9, 150);
            this.chkAutoRepeat.Name = "chkAutoRepeat";
            this.chkAutoRepeat.Size = new System.Drawing.Size(86, 17);
            this.chkAutoRepeat.TabIndex = 128;
            this.chkAutoRepeat.Text = "Auto Repeat";
            this.chkAutoRepeat.UseVisualStyleBackColor = true;
            // 
            // lblCameraStatus
            // 
            this.lblCameraStatus.AutoSize = true;
            this.lblCameraStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCameraStatus.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblCameraStatus.Location = new System.Drawing.Point(88, 25);
            this.lblCameraStatus.Name = "lblCameraStatus";
            this.lblCameraStatus.Size = new System.Drawing.Size(73, 13);
            this.lblCameraStatus.TabIndex = 127;
            this.lblCameraStatus.Text = "Disconnected";
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(6, 25);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(76, 13);
            this.label74.TabIndex = 126;
            this.label74.Text = "Camera Status";
            // 
            // btnAbort
            // 
            this.btnAbort.Enabled = false;
            this.btnAbort.Location = new System.Drawing.Point(142, 177);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(46, 21);
            this.btnAbort.TabIndex = 21;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(76, 177);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(46, 21);
            this.btnStop.TabIndex = 20;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(9, 177);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(46, 21);
            this.btnStart.TabIndex = 19;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Location = new System.Drawing.Point(12, 524);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(82, 23);
            this.btnSaveAs.TabIndex = 143;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.numTargetTemp);
            this.groupBox19.Controls.Add(this.lblCoolerPower);
            this.groupBox19.Controls.Add(this.label82);
            this.groupBox19.Controls.Add(this.label80);
            this.groupBox19.Controls.Add(this.lblCameraTemp);
            this.groupBox19.Controls.Add(this.chkCoolerOn);
            this.groupBox19.Controls.Add(this.label79);
            this.groupBox19.Location = new System.Drawing.Point(15, 382);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(200, 136);
            this.groupBox19.TabIndex = 142;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Cooler Settings";
            // 
            // numTargetTemp
            // 
            this.numTargetTemp.Location = new System.Drawing.Point(127, 43);
            this.numTargetTemp.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numTargetTemp.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numTargetTemp.Name = "numTargetTemp";
            this.numTargetTemp.Size = new System.Drawing.Size(63, 20);
            this.numTargetTemp.TabIndex = 139;
            // 
            // lblCoolerPower
            // 
            this.lblCoolerPower.AutoSize = true;
            this.lblCoolerPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCoolerPower.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblCoolerPower.Location = new System.Drawing.Point(91, 108);
            this.lblCoolerPower.Name = "lblCoolerPower";
            this.lblCoolerPower.Size = new System.Drawing.Size(100, 13);
            this.lblCoolerPower.TabIndex = 133;
            this.lblCoolerPower.Text = "-------------------------------";
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Location = new System.Drawing.Point(9, 108);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(70, 13);
            this.label82.TabIndex = 132;
            this.label82.Text = "Cooler Power";
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Location = new System.Drawing.Point(6, 50);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(88, 13);
            this.label80.TabIndex = 130;
            this.label80.Text = "Target Temp (°C)";
            // 
            // lblCameraTemp
            // 
            this.lblCameraTemp.AutoSize = true;
            this.lblCameraTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCameraTemp.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblCameraTemp.Location = new System.Drawing.Point(91, 79);
            this.lblCameraTemp.Name = "lblCameraTemp";
            this.lblCameraTemp.Size = new System.Drawing.Size(100, 13);
            this.lblCameraTemp.TabIndex = 129;
            this.lblCameraTemp.Text = "-------------------------------";
            // 
            // chkCoolerOn
            // 
            this.chkCoolerOn.AutoSize = true;
            this.chkCoolerOn.Location = new System.Drawing.Point(6, 19);
            this.chkCoolerOn.Name = "chkCoolerOn";
            this.chkCoolerOn.Size = new System.Drawing.Size(73, 17);
            this.chkCoolerOn.TabIndex = 27;
            this.chkCoolerOn.Text = "Cooler On";
            this.chkCoolerOn.UseVisualStyleBackColor = true;
            this.chkCoolerOn.CheckedChanged += new System.EventHandler(this.chkCoolerOn_CheckedChanged);
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Location = new System.Drawing.Point(6, 79);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(73, 13);
            this.label79.TabIndex = 128;
            this.label79.Text = "Camera Temp";
            // 
            // progressbarTimer
            // 
            this.progressbarTimer.Tick += new System.EventHandler(this.progressbarTimer_Tick);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "fits";
            this.saveFileDialog1.Filter = "Fits Image (*.fit)|*.fit|Fits Image (*.fits)|*.fits";
            // 
            // frmImaging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1566, 814);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.groupBox19);
            this.Controls.Add(this.pnlImage);
            this.Controls.Add(this.groupBox20);
            this.Controls.Add(this.groupBox18);
            this.Name = "frmImaging";
            this.Text = "ImageForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlImage.ResumeLayout(false);
            this.pnlImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBinning)).EndInit();
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSecs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMins)).EndInit();
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetTemp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Timer timerUpdateCamState;
        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.Label lblimageTimeRemaining;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.Label label83;
        internal System.Windows.Forms.Label lblCameraStatus;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.GroupBox groupBox19;
        internal System.Windows.Forms.Label lblCoolerPower;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.Label label80;
        internal System.Windows.Forms.Label lblCameraTemp;
        private System.Windows.Forms.Label label79;
        public System.Windows.Forms.RadioButton rbLightFrame;
        public System.Windows.Forms.RadioButton rbFlatFrame;
        public System.Windows.Forms.RadioButton rbBiasFrame;
        public System.Windows.Forms.RadioButton rbDarkFrame;
        public System.Windows.Forms.NumericUpDown numMs;
        public System.Windows.Forms.NumericUpDown numSecs;
        public System.Windows.Forms.NumericUpDown numMins;
        public System.Windows.Forms.NumericUpDown numBinning;
        public System.Windows.Forms.CheckBox chkAutoRepeat;
        public System.Windows.Forms.CheckBox chkCoolerOn;
        public System.Windows.Forms.NumericUpDown numTargetTemp;
        private System.Windows.Forms.Timer progressbarTimer;
        internal System.Windows.Forms.Button btnAbort;
        internal System.Windows.Forms.Button btnStop;
        internal System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}