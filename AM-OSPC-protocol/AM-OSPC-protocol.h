// AM-OSPC-protocol.h
#include <comdef.h>
#include "spcserver.h"
#pragma once

using namespace System;
using namespace Collections::Generic;

namespace AMOSPCprotocol {

	public ref class OSPCConnector
	{
	private:
		int recordCount;
		List<DateTime>^ timeStamps;
		List<String^>^ processNames;
		List<String^>^ variableNames;
		List<double>^ values;
		_com_ptr_t<_COM_SMARTPTR_LEVEL2 <IDataProcessing, &__uuidof(IDataProcessing)> >* m_ipDataProcessing;
	public:
		OSPCConnector(String^ server, String^ user, String^ pass);  
		~OSPCConnector(); 

		void AddRecord(DateTime time, String^ processName, String^ variableName, double Value);
		void Send();
	protected:
		!OSPCConnector();
	};
}
