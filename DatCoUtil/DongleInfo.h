#pragma once

//#include "HW configuration\\ioconfigcommon.h"

//using namespace iba::IoConfiguration;
using namespace System;

namespace iba {

	public ref class CDongleInfo
	{
	private:
		bool dongleFound;
		String^ customer;
		String^ serialnumber;
		String^ hwId;
		String^ dongleType;
		int timeLimit;
		int demoTimeLimit;
		array<Byte>^ options;

	public:
		property bool DongleFound {bool	get() {return dongleFound;}}
		property String^ Customer {String^	get() {return customer;}}
		property String^ SerialNr {String^	get() {return serialnumber;}}
		property String^ HwId {String^	get() { return hwId; }}
		property String^ DongleType {String^	get() { return dongleType; }}
		property int TimeLimit {int	get() { return timeLimit; }}
		property int DemoTimeLimit {int	get() { return demoTimeLimit; }}
		property array<Byte>^ Options {array<Byte>^ get() { return options; }}

		void Clear()
		{
			options = gcnew array<Byte>(256);
			dongleFound = false;
		}

		static CDongleInfo^ ReadDongle();
		static int AcquireAnyLicenseFromLicenseService(array<int>^ licenseNumbers); //Try to acquire one of the licenses

		bool IsPluginLicensed(int pluginbit);

	private:
		CDongleInfo() 
		{
			Clear();
		}
	};	
}