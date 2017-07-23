// This is the main DLL file.

#include "stdafx.h"

#include "S7-writer.h"
#include "S7Operand.h"
#include "S7ConnectionParameters.h"

#include <string.h>
#include "AGLink/AGLink40.h"

#define	WAIT_FLAG 1

extern "C"
{
	agl_int32_t LoadDll(void);
}

using namespace System::IO;
using namespace System::Runtime::InteropServices;
using namespace iba::Logging;

namespace S7_writer
{

/******************************************************************************
/* S7Connection
/*****************************************************************************/

	void S7Connection::Initialize()
	{
		GlobalDeviceNr = -1;

		int res = LoadDll();
		InitializeOk = res == 1;
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
		String^ appData = Environment::GetFolderPath(Environment::SpecialFolder::ApplicationData);
		parameterPath = Path::Combine(appData, "S7Writer");
		pt = Marshal::StringToHGlobalAnsi(parameterPath);
		AGL_SetParaPath((char*)pt.ToPointer());
		Marshal::FreeHGlobal(pt);
	}

	S7Connection::S7Connection()
	{
		deviceNr = System::Threading::Interlocked::Increment(GlobalDeviceNr);
		if (deviceNr > 255)
			throw gcnew Exception("No more connections available");
	}

	S7Connection::~S7Connection()
	{
		if (hConn != 0)
			Disconnect();

		System::Threading::Interlocked::Decrement(GlobalDeviceNr);
	}

	void S7Connection::Connect(S7ConnectionParameters^ connPars)
	{
		if (!InitializeOk)
			throw gcnew Exception("AGLink40.dll couldn't be initialized");

		WriteParameters(connPars);

		int result;
		if ((result = AGL_OpenDevice(deviceNr)) != AGL40_SUCCESS)
			throw gcnew Exception(GetAglinkErrorMessage(result, "AGLOpenDevice"));

		if ((result = AGL_DialUp(deviceNr, WAIT_FLAG, 0)) != AGL40_SUCCESS)
		{
			AGL_CloseDevice(deviceNr);
			throw gcnew Exception(GetAglinkErrorMessage(result, "AGLDialUp"));
		}

		if ((result = AGL_InitAdapter(deviceNr, WAIT_FLAG, 0)) != AGL40_SUCCESS)
		{
			AGL_HangUp(deviceNr, WAIT_FLAG, 0);
			AGL_CloseDevice(deviceNr);
			throw gcnew Exception(GetAglinkErrorMessage(result, "AGLInitAdapter"));
		}
				
		int localConnNr;
		result = AGL_PLCConnectEx(deviceNr, 0, connPars->Rack, connPars->Slot, &localConnNr, WAIT_FLAG, 0);
		if (result == AGL40_PARAMETER_ERROR)
		{
			//Try again with slot number 0 because AGL_PLCConnectEx doesn't seem to accept slot numbers > 32 but this is required for some adapters
			result = AGL_PLCConnectEx(deviceNr, 0, connPars->Rack, 0, &localConnNr, WAIT_FLAG, 0);
		}

		if (result != AGL40_SUCCESS)
		{
			AGL_ExitAdapter(deviceNr, WAIT_FLAG, 0);
			AGL_HangUp(deviceNr, WAIT_FLAG, 0);
			AGL_CloseDevice(deviceNr);
			throw gcnew Exception(GetAglinkErrorMessage(result, "AGLPLCConnect"));
		}
		else
			hConn = localConnNr;

	}

	void S7Connection::WriteParameters(S7ConnectionParameters^ connPars)
	{
		S7_TCPIP para;
		memset(&para, 0, sizeof(para));

		AGL_GetParas(deviceNr, TYPE_S7_TCPIP, &para, sizeof(para));

		para.Conn[0].bConnType = connPars->ConnType;
		para.Conn[0].lTimeOut = connPars->TimeoutInSec*1000;
		para.Conn[0].wPlcNr = 0;
		para.Conn[0].bRemRackNr = connPars->Rack;
		para.Conn[0].bRemSlotNr = connPars->Slot;
		para.Conn[0].bPLCClass = PLC_Class::ePLC_300_400;

		IntPtr pt = Marshal::StringToHGlobalAnsi(connPars->Address);
		strcpy_s(para.Conn[0].Address, (char*)pt.ToPointer());
		Marshal::FreeHGlobal(pt);

		AGL_SetParas(deviceNr, TYPE_S7_TCPIP, &para, sizeof(para));
		AGL_SetDevType(deviceNr, TYPE_S7_TCPIP);
		AGL_WriteDevice(deviceNr);
	}

	void S7Connection::WriteOperands(List<S7Operand^>^ operands, List<double>^ values, bool bAllowErrors)
	{
		int totalSize = 0;
		for each(S7Operand^ op in operands)
			totalSize += Math::Max(1, op->GetDataTypeSize()); //count 1 byte for digitals

		unsigned char* buffer = new unsigned char[totalSize];
		memset(buffer, 0, totalSize);
		int bufferOffset = 0;
		
		DATA_RW40* dataRW = new DATA_RW40[operands->Count];
		memset(dataRW, 0, operands->Count * sizeof(DATA_RW40));
		int nrDataRW = 0;
		
		try
		{
			DATA_RW40* pPrev = nullptr;
			DATA_RW40* pNext = dataRW;
			for(int i=0; i<operands->Count; i++)
			{
				//Set address info
				S7Operand^ op = operands[i];
				FillInDataRW(pNext, op, true);
				pNext->Buff = buffer + bufferOffset;
				pNext++;
				nrDataRW++;

				////Check if we can merge it with the previous DATA_RW40
				//bool bCreateNew = true;
				//if (pPrev != nullptr)
				//{
				//	if ((pNext->OpArea == pPrev->OpArea) && (pNext->OpType == pPrev->OpType) && (pNext->DBNr == pPrev->DBNr) && (pPrev->OpType != TYP_BIT))
				//	{
				//		int multiplier = 1;
				//		if (pNext->OpType == TYP_WORD)
				//			multiplier = 2;
				//		else if (pNext->OpType == TYP_DWORD)
				//			multiplier = 4;

				//		if ((pNext->Offset == pPrev->Offset) || (pNext->Offset == (pPrev->Offset + pPrev->OpAnz*multiplier)))
				//		{
				//			//We can merge with the previous one
				//			pPrev->OpAnz = Math::Max(pPrev->OpAnz, (UInt16)((pNext->Offset + pNext->OpAnz*multiplier - pPrev->Offset) / multiplier));
				//			pNext = pPrev;
				//			bCreateNew = false;
				//		}
				//	}
				//}
				//
				//if (bCreateNew)
				//{
				//	pPrev = pNext;
				//	pPrev->Buff = buffer + bufferOffset;

				//	pNext++;
				//	nrDataRW++;
				//}

				//Write value to buffer
				double val = values[i];
				S7::S7Type^ dataType = S7::DataTypes[op->GetDataType()];
				switch ((S7DataTypeEnum)dataType->type)
				{
				case S7DataTypeEnum::S7Bool:
					buffer[bufferOffset] = val > 0.5 ? 1 : 0;
					break;
				case S7DataTypeEnum::S7Byte:
				case S7DataTypeEnum::S7Char:
				case S7DataTypeEnum::S7USInt:
					buffer[bufferOffset] = (unsigned char)Math::Max(0.0, Math::Min(255.0, val));
					break;
				case S7DataTypeEnum::S7SInt:
					*((char*)(buffer + bufferOffset)) = (char)Math::Max(-128.0, Math::Min(127.0, val));
					break;
				case S7DataTypeEnum::S7Word:
				case S7DataTypeEnum::S7UInt:
					*((UInt16*)(buffer + bufferOffset)) = (UInt16)Math::Max(0.0, Math::Min(65535.0, val));
					break;
				case S7DataTypeEnum::S7Int:
					*((Int16*)(buffer + bufferOffset)) = (Int16)Math::Max(-32768.0, Math::Min(32767.0, val));
					break;
				case S7DataTypeEnum::S7DWord:
				case S7DataTypeEnum::S7UDInt:
					*((UInt32*)(buffer + bufferOffset)) = (UInt32)Math::Max(0.0, Math::Min((double)UInt32::MaxValue, val));
					break;
				case S7DataTypeEnum::S7DInt:
					*((Int32*)(buffer + bufferOffset)) = (Int32)Math::Max((double)Int32::MinValue, Math::Min((double)Int32::MaxValue, val));
					break;
				case S7DataTypeEnum::S7Real:
					*((float*)(buffer + bufferOffset)) = (float) val;
					break;
				case S7DataTypeEnum::S7LReal:
					*((double*)(buffer + bufferOffset)) = val;
					break;
				}

				bufferOffset += Math::Max(1, op->GetDataTypeSize());
			}

			if (nrDataRW == 0)
				return; //Nothing to write

			//Write values to S7
			int result = AGL_WriteMixEx(hConn, dataRW, nrDataRW, WAIT_FLAG, 0);
			if (result != AGL40_SUCCESS)
				throw gcnew Exception(GetAglinkErrorMessage(result, "AGL_WriteMixEx"));

			for (int i = 0; i < nrDataRW; i++)
			{
				if (dataRW[i].Result != AGL40_SUCCESS)
				{
					if (bAllowErrors)
						ibaLogger::DebugFormat("Writing operand {0} returned error {1}", operands[i]->GetName(), GetAglinkErrorMessage(dataRW[i].Result, "AGL_WriteMixEx"));
					else
						throw gcnew Exception(String::Format("Writing operand {0} returned error {1}", operands[i]->GetName(), GetAglinkErrorMessage(dataRW[i].Result, "AGL_WriteMixEx")));
				}
			}
		}
		finally
		{
			delete[] buffer;
			delete[] dataRW;
		}
	}

	bool IsTimerOrCounter(unsigned short opArea)
	{
		return opArea == AREA_TIMER_200 || opArea == AREA_COUNTER_200 || opArea == AREA_TIMER || opArea == AREA_COUNTER;
	}

	void S7Connection::FillInDataRW(void* dataRW, S7Operand^ op, bool bOutput)
	{
		DATA_RW40* data = (DATA_RW40*)dataRW;
		S7::S7OperandType^ opType = S7::OperandTypes[op->GetOperandType()];
		
		data->OpArea = opType->agLinkType;
		switch (opType->size)
		{
		case 2: data->OpType = TYP_WORD; data->OpAnz = 1; break;
		case 4: data->OpType = TYP_DWORD; data->OpAnz = 1; break;
		default: data->OpType = TYP_BYTE; data->OpAnz = opType->size; break;
		}

		if (data->OpAnz == 0)
		{
			if (bOutput)
			{
				data->OpType = TYP_BIT; //in case of outputs write single bits
				data->OpAnz = 1;
				data->BitNr = op->GetBitNr();
			}
			else
				data->OpAnz = op->GetMaxBitNr() / 8;	//switch between byte, short and int masks for digitals!!
		}
		else if (op->GetDataType() == (int) S7DataTypeEnum::S7LReal)
		{
			data->OpType = TYP_BYTE;
			data->OpAnz = 8;
		}

		if (data->OpArea == AREA_DATA)
		{
			data->DBNr = op->GetAddress(0);
			data->Offset = op->GetAddress(1);

			if (opType->cpuType == S7CPUType::S5)
			{
				data->Offset *= 2; //S5 uses WORD addresses in DBs

				if (opType->name == "DR")
					data->Offset++;

				if (opType->parent == "DX")
					data->DBNr += 256;
			}
		}
		else
		{
			data->DBNr = 0;
			data->Offset = op->GetAddress(0);

			if (opType->cpuType == S7CPUType::S5)
			{
				if (opType->name[0] == 'S')
					data->OpArea = AREA_SFLAG_200;
			}
		}

		// For timers and counters we get WORDS
		if (IsTimerOrCounter(data->OpArea))
		{
			data->OpAnz = 1;
			data->OpType = TYP_WORD;
		}

		data->Buff = nullptr;
	}

	void S7Connection::Disconnect()
	{
		if (hConn != 0)
			AGL_PLCDisconnect(hConn, WAIT_FLAG, 0);
		AGL_ExitAdapter(deviceNr, WAIT_FLAG, 0);
		AGL_HangUp(deviceNr, WAIT_FLAG, 0);
		AGL_CloseDevice(deviceNr);
	}

	String^ S7Connection::GetAglinkErrorMessage(int errNr, String^ func)
	{
		char errorMsg[256];
		AGL_GetErrorMsg(errNr, errorMsg, sizeof(errorMsg));

		String^ msg = String::Format("Error {0} ({1}) when calling function {2}", "0x" + errNr.ToString("X8"), gcnew String(errorMsg), func);
		//iba::Logging::ibaLogger::DebugFormat(msg);
		return msg;
	}

}