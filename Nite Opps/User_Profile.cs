using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nite_Opps
{
    public class User_Profile
    {
        // Define all of the properties that will be stored in the profile

        public ASCOM.Utilities.Profile prof = new ASCOM.Utilities.Profile();
        public string APPLICATION_NAME = "Nite_Ops";
        public User_Profile()
        {
            if (!checkIfRegistered())
            {
                registerApp();
            }
        }


        public Boolean checkIfRegistered()
        {
            if (prof.IsRegistered(APPLICATION_NAME))
            { return true; }
            else { return false; }
        }

        public void registerApp()
        {
            if (!prof.IsRegistered(APPLICATION_NAME))
            {
                prof.Register(APPLICATION_NAME, "The Nite Ops Application");
            }
        }
    }
}

