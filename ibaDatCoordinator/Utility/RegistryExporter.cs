using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace iba.Utility
{
    public class RegistryExporter
    {
        public static bool ExportRegistry(RegistryKey key, string destFile, bool bRecursive)
        {
            if(key == null)
                return false;

            StreamWriter writer = null;
            try
            {
                FileStream stream = File.Open(destFile, FileMode.Create, FileAccess.Write, FileShare.Read);
                writer = new StreamWriter(stream, Encoding.Unicode);

                writer.WriteLine("Windows Registry Editor Version 5.00");
                writer.WriteLine();

                WriteKey(writer, key, bRecursive);
            }
            catch(Exception)
            {
                return false;
            }
            finally
            {
                if(writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
            }

            return true;
        }

        static void WriteKey(StreamWriter writer, RegistryKey key, bool bRecursive)
        {
            writer.WriteLine("[{0}]", key.Name);

            foreach(string valueName in key.GetValueNames())
            {
                object data = key.GetValue(valueName);
                if(data == null)
                    continue;

                switch(key.GetValueKind(valueName))
                {
                    case RegistryValueKind.DWord:
                        {
                            writer.WriteLine("\"{0}\"=dword:{1}", valueName, ((Int32)data).ToString("x8"));
                            break;
                        }

                    case RegistryValueKind.String:
                        {
                            writer.WriteLine("\"{0}\"=\"{1}\"", valueName, data.ToString());
                            break;
                        }

                    case RegistryValueKind.Binary:
                        {
                            byte[] bytes = data as byte[];
                            string val = WriteBinaryValue(bytes, valueName.Length + 6);
                            writer.WriteLine("\"{0}\"=hex:{1}", valueName, val);
                            break;
                        }

                    case RegistryValueKind.MultiString:
                        {
                            string[] strings = data as string[];
                            StringBuilder fullString = new StringBuilder();
                            foreach(string s in strings)
                            {
                                fullString.Append(s);
                                fullString.Append('\0');
                            }
                            fullString.Append('\0');
                            byte[] bytes = Encoding.Unicode.GetBytes(fullString.ToString());
                            string val = WriteBinaryValue(bytes, valueName.Length + 9);
                            writer.WriteLine("\"{0}\"=hex(7):{1}", valueName, val);
                            break;
                        }

                    case RegistryValueKind.ExpandString:
                        {
                            data = key.GetValue(valueName, "", RegistryValueOptions.DoNotExpandEnvironmentNames);
                            string str = data as string;
                            str = str + "\0";
                            byte[] bytes = Encoding.Unicode.GetBytes(str);
                            string val = WriteBinaryValue(bytes, valueName.Length + 9);
                            writer.WriteLine("\"{0}\"=hex(2):{1}", valueName, val);
                            break;
                        }

                    default:
                        break;
                }
            }

            writer.WriteLine();

            if(!bRecursive)
                return;

            foreach(string subKeyName in key.GetSubKeyNames())
            {
                RegistryKey subKey = key.OpenSubKey(subKeyName);
                WriteKey(writer, subKey, bRecursive);
                subKey.Close();
            }
        }

        static string WriteBinaryValue(byte[] bytes, int startPos)
        {
            StringBuilder sb = new StringBuilder();
            int pos = startPos;
            int nrBytes = bytes.Length - 1;
            for(int i = 0; i < nrBytes; i++)
            {
                byte val = i < bytes.Length ? bytes[i] : (byte)0;
                if(pos > 75)
                {
                    sb.AppendLine("\\");
                    sb.AppendFormat("  {0},", val.ToString("x2"));
                    pos = 5;
                }
                else
                {
                    sb.AppendFormat("{0},", val.ToString("x2"));
                    pos += 3;
                }
            }

            if(bytes.Length != 0)
            {
                byte lastVal = bytes[bytes.Length - 1];
                if(pos > 75)
                {
                    sb.AppendLine("\\");
                    sb.AppendFormat("  {0}", lastVal.ToString("x2"));
                }
                else
                    sb.Append(lastVal.ToString("x2"));
            }

            return sb.ToString();
        }

        public static bool ExportIbaAnalyzerKey(string regFileName)
        {
            Microsoft.Win32.RegistryKey analyzerKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\iba\ibaAnalyzer");
            if (analyzerKey != null)
            {
                return (RegistryExporter.ExportRegistry(analyzerKey, regFileName, true));
            }
            return false;
        }
    }
}
