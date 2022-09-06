using FluentAssertions;
using iba.Utility;
using Microsoft.Win32;
using System;
using System.IO;
using Xunit;

namespace ibaDatCoordinatorTests
{
    public class RegistryExporterTest : IDisposable
    {
        string regexeFile = "regexe.reg";
        string regExporterFile = "exporter.reg";

        [Theory]
        [InlineData("HKCU", @"Software\iba\ibaAnalyzer", true)]
        [InlineData("HKCU", @"Software\iba\ibaAnalyzer", false)]
        [InlineData("HKLM", @"Software\dotnet", true)]
        [InlineData("HKLM", @"Software\dotnet", false)]
        public void RegExeShouldBeSameAsRegistryExporter(string hive, string subKey, bool recursive)
        {
            using RegistryKey key = (hive switch
            {
                "HKLM" => Registry.LocalMachine,
                "HKCU" => Registry.CurrentUser,
                _ => Registry.LocalMachine
            }).OpenSubKey(subKey);
            RegistryExporter.ExportRegistry(key, regExporterFile, recursive);
            File.Exists(regExporterFile).Should().BeTrue();

            RegistryExporter.ExportRegistry(key, regexeFile, recursive, true);
            File.Exists(regexeFile).Should().BeTrue();

            string regExeContent = File.ReadAllText(regexeFile);
            string regExporterContent = File.ReadAllText(regExporterFile);

            regExporterContent.Should().BeEquivalentTo(regExeContent);
        }

        public void Dispose()
        {
            if (File.Exists(regExporterFile))
                File.Delete(regExporterFile);
            if (File.Exists(regexeFile))
                File.Delete(regexeFile);
        }
    }
}
