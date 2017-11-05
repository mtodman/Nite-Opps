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
    public partial class frmGuiding : Form
    {
        clsSharedData sd;
        clsForms form;

        public frmGuiding()
        {
            InitializeComponent();
        }

        public frmGuiding(ref clsSharedData d, ref clsForms f)
        {
            InitializeComponent();
            sd = d;
            form = f;
        }
    }
}
