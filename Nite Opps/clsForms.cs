using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nite_Opps
{
    public class clsForms
    {
        public frmMain MainForm;
        public frmImaging ImagingForm;
        public frmGuiding GuidingForm;
        public frmConfig ConfigForm;
        public frmImageRun ImageRunForm;

        public clsForms(ref frmMain main, ref frmImaging imaging, ref frmConfig config, ref frmGuiding guiding, ref frmImageRun imagerun)
        {
            MainForm = main; ImagingForm = imaging; ConfigForm = config; GuidingForm = guiding; ImageRunForm = imagerun;
        }
    }
}
