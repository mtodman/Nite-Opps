namespace Nite_Opps
{
    partial class ParentForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.MainPage = new System.Windows.Forms.TabPage();
            this.ImagingPage = new System.Windows.Forms.TabPage();
            this.GuidingPage = new System.Windows.Forms.TabPage();
            this.ConfigPage = new System.Windows.Forms.TabPage();
            this.ImageRunPage = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1314, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.MainPage);
            this.tabControl1.Controls.Add(this.ImagingPage);
            this.tabControl1.Controls.Add(this.GuidingPage);
            this.tabControl1.Controls.Add(this.ConfigPage);
            this.tabControl1.Controls.Add(this.ImageRunPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1314, 24);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // MainPage
            // 
            this.MainPage.Location = new System.Drawing.Point(4, 22);
            this.MainPage.Name = "MainPage";
            this.MainPage.Padding = new System.Windows.Forms.Padding(3);
            this.MainPage.Size = new System.Drawing.Size(1306, 0);
            this.MainPage.TabIndex = 0;
            this.MainPage.Text = "Main";
            this.MainPage.UseVisualStyleBackColor = true;
            // 
            // ImagingPage
            // 
            this.ImagingPage.Location = new System.Drawing.Point(4, 22);
            this.ImagingPage.Name = "ImagingPage";
            this.ImagingPage.Padding = new System.Windows.Forms.Padding(3);
            this.ImagingPage.Size = new System.Drawing.Size(1306, 0);
            this.ImagingPage.TabIndex = 1;
            this.ImagingPage.Text = "Imaging";
            this.ImagingPage.UseVisualStyleBackColor = true;
            // 
            // GuidingPage
            // 
            this.GuidingPage.Location = new System.Drawing.Point(4, 22);
            this.GuidingPage.Name = "GuidingPage";
            this.GuidingPage.Size = new System.Drawing.Size(1306, 0);
            this.GuidingPage.TabIndex = 2;
            this.GuidingPage.Text = "Guiding";
            this.GuidingPage.UseVisualStyleBackColor = true;
            // 
            // ConfigPage
            // 
            this.ConfigPage.Location = new System.Drawing.Point(4, 22);
            this.ConfigPage.Name = "ConfigPage";
            this.ConfigPage.Size = new System.Drawing.Size(1306, 0);
            this.ConfigPage.TabIndex = 3;
            this.ConfigPage.Text = "Configuration";
            this.ConfigPage.UseVisualStyleBackColor = true;
            // 
            // ImageRunPage
            // 
            this.ImageRunPage.Location = new System.Drawing.Point(4, 22);
            this.ImageRunPage.Name = "ImageRunPage";
            this.ImageRunPage.Size = new System.Drawing.Size(1306, 0);
            this.ImageRunPage.TabIndex = 4;
            this.ImageRunPage.Text = "Image Run";
            this.ImageRunPage.UseVisualStyleBackColor = true;
            // 
            // ParentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1314, 771);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ParentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nite Opps";
            this.Load += new System.EventHandler(this.ParentForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage MainPage;
        private System.Windows.Forms.TabPage ImagingPage;
        private System.Windows.Forms.TabPage GuidingPage;
        private System.Windows.Forms.TabPage ConfigPage;
        private System.Windows.Forms.TabPage ImageRunPage;
    }
}

