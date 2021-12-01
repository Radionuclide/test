using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Licensing
{
    public static class LicenseHelper
    {
        public static DateTime DaysToDateTime(int days)
        {
            return new DateTime(2016, 1, 1) + TimeSpan.FromDays(days);
        }

        public static int DateTimeToDays(DateTime dt)
        {
            return (dt.Date - new DateTime(2016, 1, 1)).Days;
        }
    }
}
