using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Utility
{
    class PathUtil
    {
        private static string rootPath;

        static PathUtil()
		{
			rootPath = System.IO.Path.GetDirectoryName(typeof(PathUtil).Assembly.Location);
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
	}
}
