using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace iba.Utility
{
    class IniParser
    {
        private string m_fileName;
        public STLLikeMap<string, STLLikeMapStringValue<String> > Sections;

        public IniParser(string fileName)
        {
            m_fileName = fileName;
            Sections = new STLLikeMap<String, STLLikeMapStringValue<String> >();
        }


        bool Read()
        {
            Sections.Clear();
            string currentSection = "";
            try
            {
                using(StreamReader reader = new StreamReader(m_fileName))
                {
                    while(reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        line = line.Trim();
                        if(line == "") continue;
                        if(line[0] == ']')
                        {
                            int index = line.IndexOf(']');
                            if(index < 1) return false;
                            currentSection = line.Substring(1, index - 1);
                        }
                        else
                        {
                            int index = line.IndexOf('=');
                            if(index < 1) return false;
                            Sections[currentSection][line.Substring(0, index)] = line.Substring(index + 1);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        void Write()
        {
            using(StreamWriter writer = new StreamWriter(m_fileName, false))
            {
                foreach (var section in Sections)
                {
                    string exportLine = "[" + section.Key + "]";
                    writer.WriteLine(exportLine);
                    foreach (var kvp in section.Value)
                    {
                        exportLine = kvp.Key + "=" + kvp.Value;
                        writer.WriteLine(exportLine);
                    }
                }
            }
        }

    }
}
