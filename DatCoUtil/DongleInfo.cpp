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
		//BYTE multi[NR_MULTI_OPTIONS];
		DongleContents contents;
		memset(&contents,0,sizeof(contents));
		try
		{
			BOOL bLicenseOk = FALSE;
			for (int iRetry = 0 ; iRetry < 5 && !bLicenseOk; iRetry++)
			{
				PdaDongle dongle;
				bLicenseOk = dongle.ReadDongleContents(&contents);
			}
			if (bLicenseOk)
			{
				info->dongleFound = true;
				info->readByte = contents.multiOptions[0][61];
				info->customer = gcnew String(contents.customer);
				info->serialnumber = gcnew String(contents.serialNr,0,7);
				// added by kolesnik - begin
				info->dongleType = gcnew String(contents.type == E_USBSx ? "SmarxOS" : "MPI");
				info->timeLimit = contents.limit;
				info->demoTimeLimit = contents.demolimit;
				// format hwid
				array<Object^>^ temp = gcnew array<Object^> 
				{
					contents.hwId[0].ToString("X2"), contents.hwId[1].ToString("X2"), contents.hwId[2].ToString("X2"),
						contents.hwId[3].ToString("X2"), contents.hwId[4].ToString("X2"), contents.hwId[5].ToString("X2"),
						contents.hwId[6].ToString("X2"), contents.hwId[7].ToString("X2")
				};
				info->hwId = String::Format("{0} {1} {2} {3} {4} {5} {6} {7}", temp);
				// added by kolesnik - end
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
		if (pluginbit < 0) return true; // indicate -1 to have no plugins
		if (pluginbit > 7 || !dongleFound) return false;
		Byte mask = 1 << pluginbit;
		return ((readByte & 	mask) == mask);
	};

}