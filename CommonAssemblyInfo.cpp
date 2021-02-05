#include "stdafx.h"
#include "CommonVersion.h"

using namespace System;
using namespace System::Reflection;
using namespace System::Runtime::CompilerServices;
using namespace System::Runtime::InteropServices;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly:AssemblyCompanyAttribute("iba AG")];
[assembly:AssemblyProductAttribute("ibaDatCoordinator")];
[assembly:AssemblyCopyrightAttribute("© iba AG. All rights reserved.")];
[assembly:AssemblyTrademarkAttribute("")];
[assembly:AssemblyCultureAttribute("")];
[assembly:System::Resources::NeutralResourcesLanguage("en")]

#ifdef _DEBUG
[assembly:AssemblyConfigurationAttribute("Debug")];
#else
[assembly:AssemblyConfigurationAttribute("Release")];
#endif

[assembly:ComVisible(false)];

[assembly:AssemblyVersionAttribute(FILEVERSTRING)];
[assembly:AssemblyFileVersionAttribute(FILEVERSTRING)];
[assembly:AssemblyInformationalVersionAttribute(PRODUCTVERSTRING)];




