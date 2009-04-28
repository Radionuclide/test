using System;
using Microsoft.Win32;

namespace iba.Utility
{
	public class Profiler
	{
		public static void ProfileInt(bool bLoading, string section, string entry, ref int val, int defaultVal)
		{
			if(bLoading)
				val = (int) ReadProfileObject(section, entry, defaultVal);
			else
				WriteProfileObject(section, entry, val);
		}

		public static void ProfileBool(bool bLoading, string section, string entry, ref bool val, bool defaultVal)
		{
			if(bLoading)
			{
				string t = (string) ReadProfileObject(section, entry, defaultVal ? "1" : "0");
				val = t == "1";
			}
			else
				WriteProfileObject(section, entry, val ? "1" : "0");	
		}

		public static void ProfileColor(bool bLoading, string section, string entry, ref System.Drawing.Color cl, System.Drawing.Color defaultCl)
		{
			if(bLoading)
			{
				string temp = (string) ReadProfileObject(section, entry, string.Empty);
				if(temp == string.Empty)
					cl = defaultCl;
				else
				{
					int argb = Int32.Parse(temp, System.Globalization.NumberStyles.HexNumber);
					cl = System.Drawing.Color.FromArgb(argb);
				}
			}
			else
			{
				string val = cl.ToArgb().ToString("X8");
				WriteProfileObject(section, entry, val);
			}
		}

		public static void ProfileFont(bool bLoading, string section, string entry, ref System.Drawing.Font f, System.Drawing.Font defaultFont)
		{
			if(bLoading)
			{
				string temp = (string) ReadProfileObject(section, entry, string.Empty);
				if(temp == string.Empty)
					f = defaultFont;
				else
				{
					System.Drawing.FontConverter fc = new System.Drawing.FontConverter();
					f = (System.Drawing.Font) fc.ConvertFromString(temp);
				}
			}
			else
			{
				System.Drawing.FontConverter fc = new System.Drawing.FontConverter();
				string val = fc.ConvertToString(f);
				WriteProfileObject(section, entry, val);
			}
		}

		public static void ProfileString(bool bLoading, string section, string entry, ref string val, string defaultVal)
		{
			if(bLoading)
			{
				val = (string) ReadProfileObject(section, entry, defaultVal);
			}
			else
			{
				WriteProfileObject(section, entry, val);	
			}
		}

		public static void ProfileDouble(bool bLoading, string section, string entry, ref double val, double defaultVal)
		{
			if(bLoading)
			{
				string t = (string) ReadProfileObject(section, entry, String.Empty);
				if(t == String.Empty)
					val = defaultVal;
				else
					val = Double.Parse(t, ni);
			}
			else
			{
				WriteProfileObject(section, entry, val.ToString(ni));	
			}
		}

		#region Base implementation
		public static object ReadProfileObject(string section, string entry, object defaultVal)
		{
			RegistryKey key = GetKey(section, true);
			if(key == null)
				return defaultVal;

			object val = defaultVal;
			try
			{
				val = key.GetValue(entry, defaultVal);
			}
			catch(Exception)
			{
			}

			key.Close();
			return val;
		}

		public static void WriteProfileObject(string section, string entry, object val)
		{
			RegistryKey key = GetKey(section, true);
			if(key == null)
				return;

			key.SetValue(entry, val);
			key.Close();	
		}

        public static bool KeyExists(string section)
        {
            try
            {
                string keyName = @"Software\iba\DATCoordinator";
                if (section != String.Empty)
                    keyName = keyName + @"\" + section;

                RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName);
                key = Registry.CurrentUser.OpenSubKey(keyName);
                if (key != null)
                {
                    key.Close();
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        
		private static RegistryKey GetKey(string section, bool bWriteable)
		{
			try
			{
				string keyName = @"Software\iba\DATCoordinator";
				if(section != String.Empty)
					keyName = keyName + @"\" + section;

				RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, bWriteable);
				if(key == null)
					key = Registry.CurrentUser.CreateSubKey(keyName);
				return key;
			}
			catch(Exception)
			{
			}

			return null;
		}

		static System.Globalization.NumberFormatInfo ni;
		static Profiler()
		{
			ni = new System.Globalization.NumberFormatInfo();
			ni.NumberDecimalSeparator = ".";
		}
		#endregion
	}
}
