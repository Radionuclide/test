using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Management;

namespace iba.Utility
{
    public class SystemInfoCollector
    {
        public static void SaveSystemInfo(string extraInfo, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(extraInfo);
                writer.WriteLine("OS: " + Environment.OSVersion.VersionString);
                writer.WriteLine("CLR version: " + Environment.Version.ToString());
                writer.WriteLine("Current user: " + Environment.UserName);
                writer.WriteLine("PC name: " + Environment.MachineName);              
                writer.WriteLine("Nr CPUs: " + Environment.ProcessorCount);
                RegistryKey rootKey = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor", false);
                if (rootKey != null)
                {
                    for (int i = 0; i < Environment.ProcessorCount; i++)
                    {
                        string processorName = null;
                        RegistryKey cpuKey = rootKey.OpenSubKey(i.ToString());
                        if (cpuKey != null)
                        {
                            processorName = cpuKey.GetValue("ProcessorNameString") as string;
                            if (processorName != null)
                                processorName = processorName.Trim();
                        }
                        if (processorName == null)
                            processorName = "?";
                        writer.WriteLine("CPU {0}: {1}", i, processorName);
                    }
                }

                try
                {
                    //Retrieve memory 
                    ManagementObjectSearcher objMOS = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_LogicalMemoryConfiguration");
                    foreach(ManagementObject obj in objMOS.Get())
                    {
                        writer.WriteLine("Total Physical Memory: " + Convert.ToString(obj.GetPropertyValue("TotalPhysicalMemory")) + " bytes");
                    }

                    //Retrieve motherboard settings
                    objMOS = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
                    foreach(ManagementObject obj in objMOS.Get())
                    {
                        writer.WriteLine();
                        writer.WriteLine("Motherboard");
                        writer.WriteLine("***********");
                        writer.WriteLine("Manufacturer: " + Convert.ToString(obj.GetPropertyValue("Manufacturer")));
                        writer.WriteLine("Model:        " + Convert.ToString(obj.GetPropertyValue("Model")));
                        writer.WriteLine("Name:         " + Convert.ToString(obj.GetPropertyValue("Name")));
                        writer.WriteLine("PartNumber:   " + Convert.ToString(obj.GetPropertyValue("PartNumber")));
                        writer.WriteLine("Product:      " + Convert.ToString(obj.GetPropertyValue("Product")));
                        writer.WriteLine("SerialNumber: " + Convert.ToString(obj.GetPropertyValue("SerialNumber")));
                        writer.WriteLine("SKU:          " + Convert.ToString(obj.GetPropertyValue("SKU")));
                        writer.WriteLine("Version:      " + Convert.ToString(obj.GetPropertyValue("Version")));
                    }
                }
                catch(Exception)
                {
                }
            }
        }
    }
}
