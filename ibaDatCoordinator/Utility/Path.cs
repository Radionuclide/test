using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace iba.Utility
{
    class PathUtil
    {
        private static string rootPath;

        static PathUtil()
		{
			rootPath = Path.GetDirectoryName(typeof(PathUtil).Assembly.Location);
			rootPath += @"\\";
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
                result.AddRange(dir.GetFiles(search));
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
    }
}
