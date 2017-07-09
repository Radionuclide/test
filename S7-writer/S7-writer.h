// S7-writer.h

#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace S7_writer {

	ref class S7Operand;

	public ref class S7Connection
	{
	public:
		S7Connection();

		void Connect(String^ address, int rack, int slot, int connType, int timeout);
		void WriteOperands(List<S7Operand^>^ operands, List<double>^ values);
		void Disconnect();

	protected:
		int hConn;

		void WriteParameters(int deviceNr, String^ address, int rack, int slot, int connType, int timeout);

	private:
		static S7Connection()
		{
			Initialize();
		}

		static void Initialize();
		static bool InitializeOk;

		static String^ parameterPath;
	};
}
