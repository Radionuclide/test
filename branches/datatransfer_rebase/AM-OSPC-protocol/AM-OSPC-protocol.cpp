// This is the main DLL file.

#include "stdafx.h"

#include "AM-OSPC-protocol.h"
#include <msclr\marshal.h>
#include <msclr\marshal_windows.h>

using namespace System::Runtime::InteropServices;
using namespace msclr::interop;
namespace AMOSPCprotocol {

OSPCConnector::OSPCConnector() : m_ipDataProcessing(0)
{
	processNames = gcnew List<String^>();
	variableNames = gcnew List<String^>();
	values = gcnew List<double>();
}

void OSPCConnector::Connect(String^ server, String^ user, String^ pass)
{
	//int hrsec = CoInitializeSecurity( NULL, -1, NULL, NULL, RPC_C_AUTHN_LEVEL_NONE,
	//	RPC_C_IMP_LEVEL_IMPERSONATE, NULL, EOAC_NONE, NULL );
	marshal_context context;
	String^ domain = "";
	int domainIndex = user->IndexOf('\\');

	if (domainIndex >= 0)
	{
		domain = user->Substring(0,domainIndex);
		user = user->Substring(domainIndex+1);
	}
	COAUTHIDENTITY identity;
	identity.User = (USHORT*)((LPCWSTR)(context.marshal_as<const TCHAR*>(user)));
	identity.UserLength = user->Length;
	identity.Password = (USHORT*)(context.marshal_as<const TCHAR*>(pass));
	identity.PasswordLength = pass->Length;
	identity.Domain = (USHORT*)(context.marshal_as<const TCHAR*>(domain));
	identity.DomainLength = domain->Length;
	identity.Flags = UNICODE?SEC_WINNT_AUTH_IDENTITY_UNICODE:SEC_WINNT_AUTH_IDENTITY_ANSI;
	COAUTHINFO coauthinfo;
	coauthinfo.dwAuthnSvc = RPC_C_AUTHN_WINNT /*10*/;
	coauthinfo.dwAuthzSvc = RPC_C_AUTHZ_NONE /*0*/;
	coauthinfo.pwszServerPrincName = 0;
	coauthinfo.dwAuthnLevel = RPC_C_AUTHN_LEVEL_CONNECT /*2*/; //Perhaps CoinitializeSecurity must be called/modified
	coauthinfo.dwImpersonationLevel = RPC_C_IMP_LEVEL_IMPERSONATE /*3*/ ; //level 3
	coauthinfo.pAuthIdentityData = &identity;
	coauthinfo.dwCapabilities = EOAC_NONE /*0*/;
	COSERVERINFO serverInfo;
	serverInfo.dwReserved1 = serverInfo.dwReserved2 = 0;
	serverInfo.pwszName = (LPWSTR)(context.marshal_as<const TCHAR*>(server));
	serverInfo.pAuthInfo = &coauthinfo;


	HRESULT hr = S_OK;
	//IID id = __uuidof(IDataProcessing);
	MULTI_QI qi = {&IID_IUnknown,0,0};
	hr = CoCreateInstanceEx(__uuidof(SPCProcesManager),0,20,&serverInfo,1,&qi);
	if (SUCCEEDED(hr) && SUCCEEDED(qi.hr))
	{
		m_ipDataProcessing = new _com_ptr_t<_COM_SMARTPTR_LEVEL2 <IDataProcessing, &__uuidof(IDataProcessing)> >();
		(*m_ipDataProcessing) = qi.pItf;
	}
	else if (SUCCEEDED(hr))
	{
		iba::Logging::ibaLogger::DebugFormat("Cocreate instance SPCProcesManager succeeded, IDataProcessing QI failed... qi.HR {0}",qi.hr.ToString("X8"));
		Marshal::ThrowExceptionForHR(qi.hr);
	}
	else
	{
		iba::Logging::ibaLogger::DebugFormat("Cocreate instance SPCProcesManager failed, HR {0}", hr.ToString("X8"));
		Marshal::ThrowExceptionForHR(hr);
	}
}

OSPCConnector::~OSPCConnector()
{
	delete m_ipDataProcessing;
	m_ipDataProcessing = 0;
}

OSPCConnector::!OSPCConnector()
{
	delete m_ipDataProcessing;
	m_ipDataProcessing = 0;
}


void OSPCConnector::Send(DateTime time)
{
	SPC_VARIABELELIST varlist;
	int counted = variableNames->Count;
	varlist.size = counted;
	varlist.pVar = (SPC_VARIABELE*)CoTaskMemAlloc (sizeof(SPC_VARIABELE) * counted);

	//scope for marshal_context
	marshal_context context;
	double timedouble = time.ToOADate();
	BSTR emptyString = SysAllocString(L"");


	for (int i = 0; i < counted; i++)
	{
		(varlist.pVar)[i].type = VARIABELE;
		(varlist.pVar)[i].productIdentiteit = emptyString;
		(varlist.pVar)[i].varKenmerk = emptyString;
		(varlist.pVar)[i].procesNaam = context.marshal_as<BSTR>(processNames[i]);
		(varlist.pVar)[i].varNaam = context.marshal_as<BSTR>(variableNames[i]);
		(varlist.pVar)[i].status = !(System::Double::IsNaN(values[i]) || System::Double::IsInfinity(values[i]));
		(varlist.pVar)[i].waarde = values[i];
		(varlist.pVar)[i].tijd = timedouble;
	}

	HRESULT hr = (*m_ipDataProcessing)->AddData(varlist);
	SysFreeString(emptyString);
	::CoTaskMemFree(varlist.pVar);
	if (!SUCCEEDED(hr))
	{
		iba::Logging::ibaLogger::DebugFormat("Sending failed, HR {0}", hr.ToString("X8"));
		System::Text::StringBuilder^ sb = gcnew System::Text::StringBuilder();
		sb->AppendLine("Attempted params:");
		for (int i = 0; i < counted; i++)
		{
			sb->AppendLine(i.ToString());
			sb->AppendLine(String::Format("\tprocesNaam: {0}",processNames[i]));
			sb->AppendLine(String::Format("\tvariableleNaam: {0}", variableNames[i]));
			bool status = !(System::Double::IsNaN(values[i]) || System::Double::IsInfinity(values[i]));
			sb->AppendLine(String::Format("\tstatus: {0}", status));
			sb->AppendLine(String::Format("\twaarde: {0}", values[i]));
		}
		iba::Logging::ibaLogger::DebugFormat(sb->ToString());
		Marshal::ThrowExceptionForHR(hr);
	}
	//strings itself deleted by marshal_context 
}

void OSPCConnector::AddRecord(String^ processName, String^ variableName, double Value )
{
	processNames->Add(processName);
	variableNames->Add(variableName);
	values->Add(Value);
}

}
