using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Licensing
{
    class LicenseContents
    {
        public LicenseContents()
        {
            ContainerId = "";
            ContainerType = "";
            Identifier = "";
            Customer = "";
            DemoDays = 0;
        }

        public string ContainerId;   //The hardware id of the container + location
        public string ContainerType; //The type of container, e.g. MARX SmarxOS vx.y or WIBU CmDongle
        public string Identifier;    //License identifier in Wibu dongle (PC 1000), serial number in Marx dongle 
        public string Customer;      //The customer stored in the dongle

        public bool ContainerFound => !String.IsNullOrEmpty(ContainerId);

        public int DemoDays;         //End time of demo in days since 1/1/2016, 0 = no demo

    }
}
