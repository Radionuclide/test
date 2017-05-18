#pragma once

//#include "HW configuration\\ioconfigcommon.h"

//using namespace iba::IoConfiguration;
using namespace System;

namespace iba {
	public enum class DongleTestRes
	{
		Ok = 0,
		NoDongle = 1,
		DongleChanged = 2,
		DongleInserted = 3
	};

	public ref class CDongleInfo
	{
	private:
		Byte readByte;
		bool dongleFound;
		String^ customer;
		String^ serialnumber;
		// added by kolesnik - begin
		String^ hwId;
		String^ dongleType;
		int timeLimit;
		int demoTimeLimit;
		// added by kolesnik - end
	public:
		property bool DongleFound {bool	get() {return dongleFound;}}
		property String^ Customer {String^	get() {return customer;}}
		property String^ SerialNr {String^	get() {return serialnumber;}}
		// added by kolesnik - begin
		property String^ HwId {String^	get() { return hwId; }}
		property String^ DongleType {String^	get() { return dongleType; }}
		property int TimeLimit {int	get() { return timeLimit; }}
		property int DemoTimeLimit {int	get() { return demoTimeLimit; }}
		// added by kolesnik - end

		void Clear()
		{
			readByte = 0;
			dongleFound = false;
		}
		bool IsPluginLicensed(int pluginbit);
		static CDongleInfo^ ReadDongle();
		static String^ GetDongleLibVersion();
	private:
		CDongleInfo() 
		{
			readByte = 0;
			dongleFound = false;
		}
	};	
}