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
	public:
		property bool DongleFound {bool	get() {return dongleFound;}}
		property String^ Customer {String^	get() {return customer;}}
		property String^ SerialNr {String^	get() {return serialnumber;}}
		void Clear()
		{
			readByte = 0;
			dongleFound = false;
		}
		bool IsPluginLicensed(int pluginbit);
		bool PluginsLicensed();
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