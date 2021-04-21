using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace iba.Utility
{
    public static class CPathCleaner
	{

        public static string CleanFile(string name)
        {
            for(int i = 0; i < invalidFileChars.Length; i++)
                name = name.Replace(invalidFileChars[i], '_');

            return name;
        }

        public static string RemoveInvalidFileCharacters(string name)
        {
            for(int i = 0; i < invalidFileChars.Length; i++)
                name = name.Replace(new string(invalidFileChars[i], 1), String.Empty);

            return name;
        }
        
        public static string CleanDirectory(string name)
        {
            int invalidIndex = FindInvalidCharInDirectoryName(name);
            while(invalidIndex >= 0)
            {
                if((name[invalidIndex] == ':') && (name.Length >= 2) && (name[1] == ':'))
                {
                    string newName = name.Substring(0, invalidIndex) + name.Substring(invalidIndex).Replace(':', '_');
                    name = newName;
                }
                else
                    name = name.Replace(name[invalidIndex], '_');
                invalidIndex = FindInvalidCharInDirectoryName(name, invalidIndex+1);
            }
            
            return name;
        }

        public static string RemoveInvalidDirectoryCharacters(string name)
        {
            int invalidIndex = FindInvalidCharInDirectoryName(name);
            while(invalidIndex >= 0)
            {
                name = name.Remove(invalidIndex, 1);
                invalidIndex = FindInvalidCharInDirectoryName(name, invalidIndex);
            }

            return name;
        }

        public static int FindInvalidCharInFileName(string name)
        {
            return name.IndexOfAny(invalidFileChars);
        }

        public static int FindInvalidCharInDirectoryName(string name)
        {
            return FindInvalidCharInDirectoryName(name, 0);
        }

        public static int FindInvalidCharInDirectoryName(string name, int startIndex)
        {
            int index = name.IndexOfAny(invalidDirChars, startIndex);
            if(index >= 0)
                return index;

            //Check if ':' is not on a forbidden place
            index = name.LastIndexOf(':');
            if((index < 0) || (index == 1))
                return -1;
            else
                return index;
        }

        public static string CleanInfofield(string value)
        {
            if(String.IsNullOrEmpty(value))
                return value;

            string filteredVal = value.Replace('\0', ' ');
            filteredVal = filteredVal.Replace('\r', ' ');
            filteredVal = filteredVal.Replace('\n', ' ');
            return filteredVal;
        }

		private static Char[] invalidFileChars;
		private static Char[] invalidDirChars;

		static CPathCleaner()
		{
			invalidFileChars = Path.GetInvalidFileNameChars();
            List<Char> invalid = new List<Char>();
            invalid.AddRange(Path.GetInvalidPathChars());
            invalid.Add('?');
            invalid.Add('*');
            invalid.Add('/');
            invalidDirChars = invalid.ToArray();
		}
	};
}
