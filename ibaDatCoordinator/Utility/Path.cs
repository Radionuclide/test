using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Microsoft.Win32;

namespace iba.Utility
{
    class PathUtil
    {
        public static string rootPath;
        //private static char[] invalidChars;

        static PathUtil()
		{
			rootPath = Path.GetDirectoryName(typeof(PathUtil).Assembly.Location);
			rootPath += "\\";

            //List<char> chars = new List<char>(Path.InvalidPathChars);
		}

        public static string GetRelativePath(string absPath)
        {
            if (absPath == null)
                return null;
            int index = absPath.IndexOf(rootPath);
            if (index > -1)
                return absPath.Remove(index, rootPath.Length);
            else
                return absPath;
        }

        public static string GetAbsolutePath(string relPath)
        {
	        if(relPath == null)
		        return null;
	        if(relPath.Length == 0)
		        return rootPath;

	        if((relPath.IndexOf(':') != -1) || (relPath[0] == '\\'))
		        return relPath; //already absolute path
	        else
		        return rootPath + relPath;
        }

        public static List<FileInfo> GetFilesInSubsSafe(string search, DirectoryInfo dir)
        {
            List<FileInfo> result = new List<FileInfo>();
            if (dir==null) return result;
            try
            {
                result.AddRange(DirInfoGetFilesMultipleExtensions(dir, search));
            }
            catch
            {
            }
            try
            {
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo subdir in dirs)
                {
                    try
                    {
                        result.AddRange(GetFilesInSubsSafe(search, subdir));
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        public static List<FileInfo> DirInfoGetFilesMultipleExtensions(DirectoryInfo dir, string search)
        {
            List<FileInfo> result = new List<FileInfo>();
            string[] mult = search.Split(',');
            foreach (string pattern in mult)
            {
                result.AddRange(dir.GetFiles(pattern));
            }
            return result;
        }

        public static List<string> GetFilesMultipleExtensions(string dir, string search)
        {
            List<string> result = new List<string>();
            string[] mult = search.Split(',');
            foreach (string pattern in mult)
            {
                result.AddRange(Directory.GetFiles(dir,pattern));
            }
            return result;
        }

        public static List<string> GetFilesMultipleExtensions(string dir, string search, SearchOption searchoption)
        {
            List<string> result = new List<string>();
            string[] mult = search.Split(',');
            foreach (string pattern in mult)
            {
                result.AddRange(Directory.GetFiles(dir,pattern,searchoption));
            }
            return result;
        }

        public static string FilterInvalidFileNameChars(string toFilter)
        {
            if (String.IsNullOrEmpty(toFilter)) return toFilter;
            foreach( char oldChar in Path.GetInvalidFileNameChars())
                toFilter = toFilter.Replace(oldChar, '_');
            return toFilter;
        }

        // Returns the human-readable file size for an arbitrary, 64-bit file size
        //  The default format is "0.## XB", e.g. "4.2 KB" or "1.434 GB"
        private static readonly string[] _suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
        public static string GetSizeReadable(long i)
        {
            if (i == 0)
                return "0 " + _suf[0];

            long bytes = Math.Abs(i);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 2);
            return $"{Math.Sign(i) * num:0.##} {_suf[place]}";
        }

        /// <summary>
        /// Compact string like paths 
        /// </summary>
        public static string CompactPath(string str, int width, Font font, System.Windows.Forms.TextFormatFlags formatFlags)
        {
            // Need to copy string here since MeasureText will change result internally
            string result = String.Copy(str);
            System.Windows.Forms.TextRenderer.MeasureText(result, font, new Size(width, 0), formatFlags | System.Windows.Forms.TextFormatFlags.ModifyString);
            return result;
        }

        public static string FindAnalyzerPath()
        {
            string output;
            try
            {
                RegistryKey key = null;
                string keyName = @"CLSID\{C4B00861-0324-11D3-A677-000000000001}\LocalServer32";
                if (IbaAnalyzerIs64Bit())
                    key = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot,RegistryView.Registry64).OpenSubKey(keyName, false);
                else
                    key = Registry.ClassesRoot.OpenSubKey(keyName, false);

                //HKEY_CLASSES_ROOT\Wow6432Node\CLSID\{ C4B00861 - 0324 - 11D3 - A677 - 000000000001}\LocalServer32
                string t = key.GetValue("").ToString();
                int quoteFirst = t.IndexOf('\"');
                int quoteLast = t.LastIndexOf('\"');
                t = t.Substring(quoteFirst + 1, quoteLast - quoteFirst-1);
                output = Path.GetFullPath(t);
            }
            catch
            {
                output = iba.Properties.Resources.noIbaAnalyser;
            }
            return output;
        }

        public static bool IbaAnalyzerIs64Bit()
        {
            bool res = false;
            try
            {
                IbaAnalyzer.IbaAnalysis MyIbaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                string ver = MyIbaAnalyzer.GetVersion();
                if (ver.Contains("x64"))
                    res = true;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(MyIbaAnalyzer);
            }
            catch
            {
            }
            return res;
        }
    }    
}
