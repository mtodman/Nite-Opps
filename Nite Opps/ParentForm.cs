using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Nite_Opps
{
    public partial class ParentForm : Form
    {
        clsSharedData sharedData = new clsSharedData();
        clsTargetObject targetObject = new clsTargetObject();
        frmMain MainForm;
        frmImaging ImagingForm;
        frmGuiding GuidingForm;
        frmConfig ConfigForm;
        frmImageRun ImageRunForm;
        clsForms forms;

        //User_Profile Prof = new User_Profile();


        public ParentForm()
        {
            InitializeComponent();
            forms = new clsForms(ref MainForm, ref ImagingForm, ref ConfigForm, ref GuidingForm, ref ImageRunForm);

            forms.MainForm = new frmMain(ref sharedData, ref targetObject, ref forms);
            forms.ImagingForm = new frmImaging(ref sharedData, ref targetObject, ref forms);
            forms.GuidingForm = new frmGuiding(ref sharedData, ref forms);
            forms.ConfigForm = new frmConfig(ref sharedData, ref forms);
            forms.ImageRunForm = new frmImageRun(ref  sharedData, ref forms);
            forms.MainForm.MdiParent = this;
            forms.ImagingForm.MdiParent = this;
            forms.GuidingForm.MdiParent = this;
            forms.ConfigForm.MdiParent = this;
            forms.ImageRunForm.MdiParent = this;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "MainPage":
                    forms.MainForm.Select();
                    break;
                case "ImagingPage":
                    forms.ImagingForm.Select();
                    break;
                case "GuidingPage":
                    forms.GuidingForm.Select();
                    break;
                case "ConfigPage":
                    forms.ConfigForm.Select();
                    break;
                case "ImageRunPage":
                    forms.ImageRunForm.Select();
                    break;
            }
        }

        private void ParentForm_Load(object sender, EventArgs e)
        {
            forms.ImagingForm.Select();
            forms.GuidingForm.Select();
            forms.ConfigForm.Select();
            forms.ImageRunForm.Select();
            forms.MainForm.Select();
        }
    }
}
