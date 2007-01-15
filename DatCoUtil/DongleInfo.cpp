#include "StdAfx.h"
#include "DongleInfo.h"

#define DONGLE_LITE
#define WIN32_LEAN_AND_MEAN
#include "..\Dependencies\pdadongle.h"

#include <string>

#include "vcclr.h"

using namespace System::Runtime::InteropServices;
using namespace System::Threading;
using namespace System::Diagnostics;

namespace iba {

	CDongleInfo^ CDongleInfo::ReadDongle()
	{
		CDongleInfo^ info = gcnew CDongleInfo();

		//Boost thread priority
		ThreadPriority savedPriority = Thread::CurrentThread->Priority;
		Thread::CurrentThread->Priority = ThreadPriority::Highest;
		BYTE multi[NR_MULTI_OPTIONS];
		memset(multi,0,sizeof(multi));
		try
		{
			BOOL bLicenseOk = FALSE;
			PdaDongle dongle;
			for (int iRetry = 0 ; iRetry < 50 && !bLicenseOk; iRetry++)
				bLicenseOk = dongle.ReadMultiLicenses(multi);
			if (bLicenseOk)
			{
				info->dongleFound = true;
				info->readByte = multi[61];
			}
		}
		catch(char*)
		{
			info->Clear();
		}

		//Restore thread priority
		Thread::CurrentThread->Priority = savedPriority;

		return info;
	}

	String^ CDongleInfo::GetDongleLibVersion()
	{
		char* str = PdaDongle::GetLibVersion();
		String^ ver = Marshal::PtrToStringAnsi(IntPtr(str));
		return ver;
	}

	bool CDongleInfo::IsPluginLicensed(int pluginbit)
	{
		if (pluginbit == 0) return true;
		if (pluginbit < 1 || pluginbit > 7 || !dongleFound) return false;
		Byte mask = 1 << (pluginbit-1);
		return ((readByte & 	mask) == mask);
	};

	bool CDongleInfo::PluginsLicensed()
	{
		return dongleFound && ((readByte & 1) == 1);
	};
}