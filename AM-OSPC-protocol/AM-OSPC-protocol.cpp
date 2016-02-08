// This is the main DLL file.

#include "stdafx.h"

#include "AM-OSPC-protocol.h"
#include <atlstr.h>
using namespace System::Runtime::InteropServices;
namespace AMOSPCprotocol {

OSPCConnector::OSPCConnector(String^ server, String^ _user, String^ _pass)
{
	//int hrsec = CoInitializeSecurity( NULL, -1, NULL, NULL, RPC_C_AUTHN_LEVEL_NONE,
	//	RPC_C_IMP_LEVEL_IMPERSONATE, NULL, EOAC_NONE, NULL );

	COAUTHIDENTITY identity;
	CString user = "michael";
	CString pass = "gent";
	identity.User = (USHORT*)((LPCWSTR)(user));
	identity.UserLength = user.GetLength();
	identity.Password = (USHORT*)((LPCWSTR)(pass));
	identity.PasswordLength = pass.GetLength();
	CString domain = "";
	identity.Domain = (USHORT*)((LPCWSTR)(domain));
	identity.DomainLength = 0;
	identity.Flags = SEC_WINNT_AUTH_IDENTITY_UNICODE;
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
	CStringW serverName = L"note-nic2";
	serverInfo.pwszName = (LPWSTR)((LPCWSTR)(serverName));
	serverInfo.pAuthInfo = &coauthinfo;


	HRESULT hr = 0;
	//IID id = __uuidof(IDataProcessing);
	MULTI_QI qi = {&IID_IUnknown,0,0};
	hr = CoCreateInstanceEx(__uuidof(SPCProcesManager),0,20,&serverInfo,1,&qi);

	if (SUCCEEDED(hr) && SUCCEEDED(qi.hr))
	{
		(*m_ipDataProcessing) = qi.pItf;
		//StartPolling();
	}
	else if (SUCCEEDED(hr))
	{
		Marshal::ThrowExceptionForHR(qi.hr);
	}
	else
		Marshal::ThrowExceptionForHR(hr);
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


void OSPCConnector::Send()
{

}

void OSPCConnector::AddRecord( DateTime time, String^ processName, String^ variableName, double Value )
{

}

}
