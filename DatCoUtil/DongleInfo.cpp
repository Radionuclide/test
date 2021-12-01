#include "StdAfx.h"
#include "DongleInfo.h"

#define DONGLE_LITE
#define _SERVICE
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
		memset(&contents, 0, sizeof(contents));
		try
		{
			BOOL bLicenseOk = FALSE;
			for (int iRetry = 0; iRetry < 5 && !bLicenseOk; iRetry++)
			{
				PdaDongle dongle;
				bLicenseOk = dongle.ReadDongleContents(&contents);
			}

			if (bLicenseOk)
			{
				info->dongleFound = true;
				info->customer = gcnew String(contents.customer, 0, Math::Min(sizeof(contents.customer), strlen(contents.customer)), Text::Encoding::GetEncoding(1252));;
				
				char serialStr[8];
				memcpy(serialStr, contents.serialNr, 7);
				serialStr[7] = 0;
				info->serialnumber = gcnew String(serialStr);

				info->dongleType = String::Format("MARX {0} v{1}.{2}", contents.type == E_USBSx ? "SmarxOS" : "MPI", contents.fwMajor, contents.fwMinor);
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

				info->options = gcnew array<Byte>(NR_MULTI_OPTIONS);
				System::Runtime::InteropServices::Marshal::Copy(IntPtr(contents.multiOptions), info->options, 0, info->options->Length);
			}
		}
		catch (char*)
		{
			info->Clear();
		}

		//Restore thread priority
		Thread::CurrentThread->Priority = savedPriority;

		return info;
	}

	int CDongleInfo::AcquireAnyLicenseFromLicenseService(array<int>^ licenseNumbers)
	{
		CibaLicense lic;
		for each (int licNr in licenseNumbers)
		{
			if (lic.HasLicense(licNr) != 0)
				return licNr;
		}

		return -1;
	}

	bool CDongleInfo::IsPluginLicensed(int pluginbit)
	{
		Byte readByte = options[61];
		if (pluginbit < 0) return true; // indicate -1 to have no plugins
		if (pluginbit > 7 || !dongleFound) return false;
		Byte mask = 1 << pluginbit;
		return ((readByte & 	mask) == mask);
	};

}