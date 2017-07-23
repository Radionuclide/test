// S7-writer.h

#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace S7_writer {

	ref class S7Operand;
	ref class S7ConnectionParameters;

	public ref class S7Connection
	{
	public:
		S7Connection();
		~S7Connection();

		void Connect(S7ConnectionParameters^ connPars);
		void WriteOperands(List<S7Operand^>^ operands, List<double>^ values, bool bAllowErrors);
		void Disconnect();

	protected:
		int hConn;
		int deviceNr;

		void WriteParameters(S7ConnectionParameters^ connPars);

	private:
		void FillInDataRW(void* dataRW, S7Operand^ op, bool bOutput);
		
	private:
		static S7Connection()
		{
			Initialize();
		}

		static void Initialize();
		static bool InitializeOk;

		static String^ parameterPath;
		static int GlobalDeviceNr;

		static String^ GetAglinkErrorMessage(int errNr, String^ func);
	};
}
