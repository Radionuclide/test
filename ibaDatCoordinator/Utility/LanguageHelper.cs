using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Utility
{
    static class LanguageHelper
    {
        public static void SetupLanguage(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg1 = args[i].ToUpper();
                if (arg1.StartsWith("/LANG:"))
                {
                    string language = args[i].Substring(6, arg1.Length - 6);
                    if (language.IndexOf('-') < 0)
                    {
                        if (language == "en")
                            language = "en-us";
                        else
                            language = language + "-" + language;
                    }

                    try
                    {
                        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(language);
                        System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                        //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    }
                    catch (Exception)
                    {
                    }
                    break;
                }
            }
        }
    }
}
