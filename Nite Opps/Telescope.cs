using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Nite_Opps
{

    public class Telescope : ASCOM.DriverAccess.Telescope
    {

        clsSharedData sd;
        #region Constructor
        public Telescope(string ScopeID, ref clsSharedData d)
            : base(ScopeID)
        {
            sd = d;
        }
        #endregion

        #region Private Properties

        #endregion


        #region Public Properties and Methods

        public bool isSlewing
        {
            get
            {
                return Slewing;
            }
        }
        

        public struct slewData
        {
            public double RA;
            public double Dec;
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Slews the connected mount to the provided RA and Dec coordinates
        /// </summary>
        /// <param name="ra"></param>
        /// <param name="dec"></param>
        public void slewScope(double ra, double dec)
        {

            var d = new slewData
            {
                RA = ra, Dec = dec
            };
            writetolog("Slewing Started\r\n", true);
            var t1 = new Thread(t_slewScope);
            t1.Start(d);

        }

        private void t_slewScope(object o)
        {
            slewData d = (slewData)o;
            SlewToCoordinatesAsync(d.RA, d.Dec);
            writetolog("Slewing", false);
            while (Slewing)
            {
                writetolog(".", false);
                Thread.Sleep(500);
            }
            writetolog("\r\n", false);
            writetolog("Slewing Finished\r\n", true);
            sd.slewcomplete = true;

        }

        public void parkScope()
        {
            writetolog("Parking Scope Started\r\n", true);
            var t1 = new Thread(t_parkScope);
            t1.Start();
            //writetolog("Parking\r\n", false);
            //while (!AtPark)
            //{
            //    writetolog(".", false);
            //    Thread.Sleep(500);
            //}
            //writetolog("\r\n", false);

        }

        private void t_parkScope()
        {
            Park();
            //writetolog("Parking\r\n", false);
            //while (!AtPark)
            //{
            //    writetolog(".", false);
            //    Thread.Sleep(500);
            //}
            //writetolog("\r\n", false);
            writetolog("Scope is now Parked\r\n", true);
        }

        // Code to create the event which sends a string message to 
        // the Main form's Status Box (txtStatusBox).
        public delegate void LogHandler(string message, bool displaytime);
        public event LogHandler Log;
        public void writetolog(string message, bool displaytime)
        {
            Log(message, displaytime);
        }

        #endregion


        #region Public Methods
        
        #endregion
    }
}

