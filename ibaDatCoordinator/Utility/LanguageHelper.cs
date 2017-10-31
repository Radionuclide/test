using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iba.Utility
{
    static class LanguageHelper
    {
        public static string SelectedLanguage = "";

        public static void SetupLanguage(string[] args)
        {
            //Check command line arguments
            for (int i = 0; i < args.Length; i++)
            {
                string arg1 = args[i].ToUpper();
                if (arg1.StartsWith("/LANG:"))
                {
                    try
                    {
                        string language = args[i].Substring(6, arg1.Length - 6);
                        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(language);
                        System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture; //Also set the default culture for any new thread!
                        SelectedLanguage = culture.Name;
                    }
                    catch (Exception)
                    {
                    }

                    return;
                }
            }

            //No command line overrides so use the registry setting
            Profiler.ProfileString(true, "Client", "Language", ref SelectedLanguage, "");
            if (SelectedLanguage != "")
            {
                try
                {
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(SelectedLanguage);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                    System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture; //Also set the default culture for any new thread!
                    SelectedLanguage = culture.Name;
                }
                catch (Exception)
                {
                    SelectedLanguage = "";
                }
            }
        }
    }
}
