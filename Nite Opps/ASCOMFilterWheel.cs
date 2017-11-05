using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nite_Opps
{
    public class ASCOMFilterWheel : ASCOM.DriverAccess.FilterWheel
    {
        clsSharedData sd;
        clsForms form;

        public ASCOMFilterWheel(string FWID, ref clsSharedData d, ref clsForms f)
            : base(FWID)
        {
            sd = d;
            form = f;
        }

        // Code to create the event which sends a string message to 
        // the Main form's Status Box (txtStatusBox).
        public delegate void LogHandler(string message, bool displaytime);
        public event LogHandler Log;
        public void writetolog(string message, bool displaytime)
        {
            Log(message, displaytime);
        }

    }
}


