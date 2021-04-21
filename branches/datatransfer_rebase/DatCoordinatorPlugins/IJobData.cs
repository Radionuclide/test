﻿using System;

namespace iba.Plugins
{
    public interface IJobData
    {
        bool AutoStart { get; set; }
        string DatDirectory { get; set; }
        string DatDirectoryUNC { get; set; }
        bool Enabled { get; set; }
        string IbaAnalyzerExe { get; set; }
        string Name { get; set; }
        string Password { get; set; }
        string HdUser { get;}
        string HdPass { get;}
        bool SubDirs { get; set; }
        string Username { get; set; }
        string FileEncryptionPassword { get; }
		bool DatTriggered { get; }
		Guid Guid { get; set; }
	}
}
