// This is the main DLL file.

#include "stdafx.h"

#include "S7-writer.h"
#include "S7Operand.h"

#include <string.h>
#include "AGLink/AGLink40.h"

#define	WAIT_FLAG 1

extern "C"
{
	agl_int32_t LoadDll(void);
}

using namespace System::IO;
using namespace System::Runtime::InteropServices;

namespace S7_writer
{

/******************************************************************************
/* S7Connection
/*****************************************************************************/

	void S7Connection::Initialize()
	{
		int res = LoadDll();
		InitializeOk = res == AGL40_SUCCESS;
		if (!InitializeOk)
			return;

		AGL_Activate("908B65-B1D0-83D9F8");

		String^ rootPath = System::IO::Path::GetDirectoryName(S7Connection::typeid->Assembly->Location);

		//Set error messages path
		String^ path;
		if (String::Compare(System::Globalization::CultureInfo::CurrentUICulture->TwoLetterISOLanguageName, "de") == 0)
			path = Path::Combine(rootPath, "aglink40_error_de.txt");
		else
			path = Path::Combine(rootPath, "aglink40_error.txt");
		IntPtr pt = Marshal::StringToHGlobalAnsi(path);
		AGL_LoadErrorFile((char*)pt.ToPointer());
		Marshal::FreeHGlobal(pt);

		//Set parameters path
		parameterPath = Path::Combine(rootPath, "agl_temp");
		pt = Marshal::StringToHGlobalAnsi(parameterPath);
		AGL_SetParaPath((char*)pt.ToPointer());
		Marshal::FreeHGlobal(pt);
	}

	S7Connection::S7Connection()
	{
	}

	void S7Connection::Connect(String^ address, int rack, int slot, int connType, int timeout)
	{
		if (!InitializeOk)
			throw gcnew Exception("AGLink40.dll couldn't be initialized");

		WriteParameters(0, address, rack, slot, connType, timeout);

		
	}

	void S7Connection::WriteParameters(int deviceNr, String^ address, int rack, int slot, int connType, int timeout)
	{
		S7_TCPIP para;
		memset(&para, 0, sizeof(para));

		AGL_GetParas(deviceNr, TYPE_S7_TCPIP, &para, sizeof(para));

		para.Conn[0].bConnType = connType;
		para.Conn[0].lTimeOut = timeout;
		para.Conn[0].wPlcNr = 0;
		para.Conn[0].bRemRackNr = rack;
		para.Conn[0].bRemSlotNr = slot;
		para.Conn[0].bPLCClass = PLC_Class::ePLC_300_400;

		IntPtr pt = Marshal::StringToHGlobalAnsi(address);
		strcpy_s(para.Conn[0].Address, (char*)pt.ToPointer());
		Marshal::FreeHGlobal(pt);

		AGL_SetParas(deviceNr, TYPE_S7_TCPIP, &para, sizeof(para));
		AGL_SetDevType(deviceNr, TYPE_S7_TCPIP);
		AGL_WriteDevice(deviceNr);
	}

	void S7Connection::WriteOperands(List<S7Operand^>^ operands, List<double>^ values)
	{
	}

	void S7Connection::Disconnect()
	{
	}

}