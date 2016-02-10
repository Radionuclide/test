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
		List<String^>^ processNames;
		List<String^>^ variableNames;
		List<double>^ values;
		_com_ptr_t<_COM_SMARTPTR_LEVEL2 <IDataProcessing, &__uuidof(IDataProcessing)> >* m_ipDataProcessing;
	public:
		OSPCConnector();
		void Connect(String^ server, String^ user, String^ pass);  
		~OSPCConnector(); 

		void AddRecord(String^ processName, String^ variableName, double Value);
		void Send(DateTime time);
	protected:
		!OSPCConnector();
	};
}
