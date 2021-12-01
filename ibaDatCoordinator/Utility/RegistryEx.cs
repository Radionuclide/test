using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Utility
{
    public static class RegistryEx
    {
        static RegistryKey localMachine32;

        static RegistryEx()
        {
            if (Environment.Is64BitProcess)
                localMachine32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            else
                localMachine32 = Registry.LocalMachine;
        }

        public static RegistryKey LocalMachine32 => localMachine32;
    }
}
