using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace iba.Utility
{
    class PathUtil
    {
        private static string rootPath;
        //private static char[] invalidChars;

        static PathUtil()
		{
			rootPath = Path.GetDirectoryName(typeof(PathUtil).Assembly.Location);
			rootPath += @"\\";

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
    }    
}
