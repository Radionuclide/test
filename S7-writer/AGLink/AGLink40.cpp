/*******************************************************************************
Projekt        : AGLink-Bibliothek
Dateiname      : AGLink40.cpp
Beschreibung   : Dynamisches Laden der öffentlichen Funktionen
Copyright      : (c) 1998-2017
                 DELTALOGIC Automatisierungstechnik GmbH
                 Stuttgarter Str. 3
                 73525 Schwäbisch Gmünd
                 Web : http://www.deltalogic.de
                 Tel.: +49-7171-916120
                 Fax : +49-7171-916220
Erstellt       : Automatisch generiert
*******************************************************************************/

#pragma warning( disable : 4115 )   // Warnungsmeldungen "Benannte Typdefinition in runden Klammern" unterdrücken

/*******************************************************************************
Einbinden der Headerfiles
*******************************************************************************/

#include "AGLink40.h"

#if defined( __WIN32__ )
  #include <Windows.h>
  #define __DLT(x) x
#elif defined( __WIN64__ )
  #include <Windows.h>
  #include <tchar.h>
#define __DLT(x) x
#elif defined( __WINCE__ )
  #include <Windows.h>
#define __DLT(x) L ## x 
#elif defined( __LINUX__ )
  #include <dlfcn.h>
  #define LoadLibrary( lib ) dlopen( lib, RTLD_NOW )
  #define FreeLibrary( lib ) dlclose( lib )
  #define GetProcAddress( lib, name ) dlsym( lib, name )
  #define __DLT(x) x
#endif

/*******************************************************************************
Modulglobale Variablen
*******************************************************************************/

static agl_handle_t hLib = 0;

DWORD AGL_CPP_BitMask[] = 
{
  0x00000001, 0x00000002, 0x00000004, 0x00000008, 0x00000010, 0x00000020, 0x00000040, 0x00000080,
  0x00000100, 0x00000200, 0x00000400, 0x00000800, 0x00001000, 0x00002000, 0x00004000, 0x00008000,
  0x00010000, 0x00020000, 0x00040000, 0x00080000, 0x00100000, 0x00200000, 0x00400000, 0x00800000,
  0x01000000, 0x02000000, 0x04000000, 0x08000000, 0x10000000, 0x20000000, 0x40000000, 0x80000000
};


/*******************************************************************************
Lokale Funktionen
*******************************************************************************/

#if defined( __cplusplus )
  extern "C" {
#endif

agl_int32_t LoadDll( void );
agl_int32_t LoadDllByPath( const agl_cstr8_t const );
agl_int32_t LoadFunctions( void );

#define Loaded( pFunc ) ( (hLib != 0 && pFunc != NULL) || (LoadDll() > 0 && pFunc != NULL) )

/*******************************************************************************
Definition der Funktionszeigertypen
*******************************************************************************/

typedef void (AGL_API * LPFN_AGL_Activate)(agl_cstr8_t Key);
typedef void (AGL_API * LPFN_AGL_GetVersion)(agl_int32_t* Major, agl_int32_t* Minor);
typedef void (AGL_API * LPFN_AGL_GetVersionEx)(agl_int32_t* Major, agl_int32_t* Minor, agl_int32_t* Build, agl_int32_t* Revision, agl_cstr8_t Date);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetOptions)(void);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetSerialNumber)(void);
typedef agl_cstr8_t (AGL_API * LPFN_AGL_GetClientName)(agl_cstr8_t Name);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetMaxDevices)(void);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetMaxQueues)(void);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetMaxPLCPerDevice)(void);
typedef void (AGL_API * LPFN_AGL_UseSystemTime)(agl_bool_t Flag);
typedef void (AGL_API * LPFN_AGL_ReturnJobNr)(agl_bool_t Flag);
typedef void (AGL_API * LPFN_AGL_SetBSendAutoResponse)(agl_bool_t Flag);
typedef agl_bool_t (AGL_API * LPFN_AGL_GetBSendAutoResponse)(void);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetPCCPConnNames)(agl_cstr8_t Names, agl_int32_t Len);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetPCCPProtocol)(agl_cstr8_t Name);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetTapiModemNames)(agl_cstr8_t Names, agl_int32_t Len);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetLocalIPAddresses)(agl_ulong32_t* Addresses, agl_ulong32_t NumAddresses);
typedef agl_ulong32_t (AGL_API * LPFN_AGL_GetTickCount)(void);
typedef agl_ulong32_t (AGL_API * LPFN_AGL_GetMicroSecs)(void);
typedef void (AGL_API * LPFN_AGL_UnloadDyn)(void);
typedef agl_int32_t (AGL_API * LPFN_AGL_Config)(agl_int32_t DevNr);
typedef agl_int32_t (AGL_API * LPFN_AGL_ConfigEx)(agl_int32_t DevNr, agl_cstr8_t CmdLine);
typedef agl_int32_t (AGL_API * LPFN_AGL_SetParas)(agl_int32_t DevNr, agl_int32_t ParaType, void* Para, agl_int32_t Len);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetParas)(agl_int32_t DevNr, agl_int32_t ParaType, void* Para, agl_int32_t Len);
typedef agl_int32_t (AGL_API * LPFN_AGL_SetDevType)(agl_int32_t DevNr, agl_int32_t DevType);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetDevType)(agl_int32_t DevNr);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadParas)(agl_int32_t DevNr, agl_int32_t ParaType);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteParas)(agl_int32_t DevNr, agl_int32_t ParaType);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadDevice)(agl_int32_t DevNr);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteDevice)(agl_int32_t DevNr);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadParasFromFile)(agl_int32_t DevNr, agl_cstr8_t FileName);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteParasToFile)(agl_int32_t DevNr, agl_cstr8_t FileName);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetParaPath)(agl_cstr8_t DirName, agl_int32_t MaxLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_SetParaPath)(const agl_cstr8_t DirName);
typedef agl_int32_t (AGL_API * LPFN_AGL_SetAndGetParaPath)(agl_cstr8_t CompanyName, agl_cstr8_t ProductName, agl_cstr8_t AktPath, agl_int32_t MaxLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetPLCType)(agl_int32_t DevNr, agl_int32_t PlcNr);
typedef agl_int32_t (AGL_API * LPFN_AGL_HasFunc)(agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t Func);
typedef agl_int32_t (AGL_API * LPFN_AGL_LoadErrorFile)(agl_cstr8_t FileName);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetErrorMsg)(agl_int32_t ErrNr, agl_cstr8_t Msg, agl_int32_t MaxLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetErrorCodeName)(agl_int32_t ErrNr, const agl_cstr8_t* const ErrorCodeName);
typedef agl_int32_t (AGL_API * LPFN_AGL_OpenDevice)(agl_int32_t DevNr);
typedef agl_int32_t (AGL_API * LPFN_AGL_CloseDevice)(agl_int32_t DevNr);
typedef agl_int32_t (AGL_API * LPFN_AGL_SetDevNotification)(agl_int32_t DevNr, LPNOTIFICATION pN);
typedef agl_int32_t (AGL_API * LPFN_AGL_SetConnNotification)(agl_int32_t ConnNr, LPNOTIFICATION pN);
typedef agl_int32_t (AGL_API * LPFN_AGL_SetJobNotification)(agl_int32_t DevNr, agl_int32_t JobNr, LPNOTIFICATION pN);
typedef agl_int32_t (AGL_API * LPFN_AGL_SetJobNotificationEx)(agl_int32_t DevNr, agl_int32_t JobNr, LPNOTIFICATION pN);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetJobResult)(agl_int32_t DevNr, agl_int32_t JobNr, LPRESULT40 pR);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetLastJobResult)(agl_int32_t ConnNr, LPRESULT40 pR);
typedef agl_int32_t (AGL_API * LPFN_AGL_DeleteJob)(agl_int32_t DevNr, agl_int32_t JobNr);
typedef agl_int32_t (AGL_API * LPFN_AGL_WaitForJob)(agl_int32_t DevNr, agl_int32_t JobNr);
typedef agl_int32_t (AGL_API * LPFN_AGL_WaitForJobEx)(agl_int32_t DevNr, agl_int32_t JobNr, LPRESULT40 pR);
typedef agl_int32_t (AGL_API * LPFN_AGL_DialUp)(agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_HangUp)(agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitAdapter)(agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ExitAdapter)(agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetLifelist)(agl_int32_t DevNr, agl_uint8_t* List, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetDirectPLC)(agl_int32_t DevNr, agl_uint8_t* pPlc, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_PLCConnect)(agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t* ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_PLCConnectEx)(agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t RackNr, agl_int32_t SlotNr, agl_int32_t* ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_PLCDisconnect)(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadMaxPacketSize)(agl_int32_t ConnNr);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetRedConnState)(agl_int32_t ConnNr, LPRED_CONN_STATE pState);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetRedConnStateMsg)(agl_int32_t ConnNr, LPRED_CONN_STATE pState, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadOpState)(agl_int32_t ConnNr, agl_int32_t* State, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadOpStateEx)(agl_int32_t ConnNr, agl_int32_t* State, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetPLCStartOptions)(agl_int32_t ConnNr, agl_int32_t* StartOptions, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_PLCStop)(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_PLCStart)(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_PLCResume)(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_PLCColdStart)(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_IsHPLC)(agl_int32_t ConnNr, agl_int32_t* IsHPLC, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_HPLCStop)(agl_int32_t ConnNr, agl_int32_t CPUNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_HPLCStart)(agl_int32_t ConnNr, agl_int32_t CPUNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_HPLCColdStart)(agl_int32_t ConnNr, agl_int32_t CPUNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetPLCClock)(agl_int32_t ConnNr, LPTOD pTOD, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_SetPLCClock)(agl_int32_t ConnNr, LPTOD pTOD, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_SyncPLCClock)(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadMLFBNr)(agl_int32_t ConnNr, LPMLFB pMLFBNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadMLFBNrEx)(agl_int32_t ConnNr, LPMLFBEX pMLFBNrEx, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadPLCInfo)(agl_int32_t ConnNr, LPPLCINFO pPLCInfo, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadCycleTime)(agl_int32_t ConnNr, LPCYCLETIME pCycleTime, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadProtLevel)(agl_int32_t ConnNr, LPPROTLEVEL pProtLevel, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadS7Ident)(agl_int32_t ConnNr, LPS7_IDENT pIdent, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadS7LED)(agl_int32_t ConnNr, LPS7_LED pLed, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetExtModuleInfo)(agl_int32_t ConnNr, LPEXT_MODULE_INFO pInfo, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadSzl)(agl_int32_t ConnNr, agl_int32_t SzlId, agl_int32_t Index, agl_uint8_t* Buff, agl_int32_t* BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_IsPasswordReq)(agl_int32_t ConnNr, agl_int32_t* IsPWReq, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_SetPassword)(agl_int32_t ConnNr, agl_cstr8_t  PW, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_UnSetPassword)(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadDiagBufferEntrys)(agl_int32_t ConnNr, agl_int32_t* Entrys, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadDiagBuffer)(agl_int32_t ConnNr, agl_int32_t* Entrys, agl_uint8_t* pDiagBuff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetDiagBufferEntry)(agl_int32_t Index, agl_uint8_t* pDiagBuff, agl_cstr8_t Text, agl_int32_t TextLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadDBCount)(agl_int32_t ConnNr, agl_int32_t* DBCount, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadDBList)(agl_int32_t ConnNr, agl_int32_t* DBCount, agl_uint16_t* DBList, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadDBLen)(agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t* DBLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadAllBlockCount)(agl_int32_t ConnNr, LPALL_BLOCK_COUNT pBC, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadBlockCount)(agl_int32_t ConnNr, agl_int32_t BlockType, agl_int32_t* BlockCount, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadBlockList)(agl_int32_t ConnNr, agl_int32_t BlockType, agl_int32_t* BlockCount, agl_uint16_t* BlockList, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadBlockLen)(agl_int32_t ConnNr, agl_int32_t BlockType, agl_int32_t BlockNr, agl_int32_t* BlockLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadInBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadPInBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadOutBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadFlagBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadSFlagBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadVarBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadDataBytes)(agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadDataWords)(agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadTimerWords)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadCounterWords)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadMix)(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadMixEx)(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteInBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteOutBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WritePOutBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteFlagBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteSFlagBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteVarBytes)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteDataBytes)(agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteDataWords)(agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteTimerWords)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteCounterWords)(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteMix)(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteMixEx)(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitOptReadMix)(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadOptReadMix)(agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_EndOptReadMix)(agl_ptrdiff_t Opt);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitOptReadMixEx)(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadOptReadMixEx)(agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_EndOptReadMixEx)(agl_ptrdiff_t Opt);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitOptWriteMix)(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteOptWriteMix)(agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_EndOptWriteMix)(agl_ptrdiff_t Opt);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitOptWriteMixEx)(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteOptWriteMixEx)(agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_EndOptWriteMixEx)(agl_ptrdiff_t Opt);
typedef agl_int32_t (AGL_API * LPFN_AGL_SetOptNotification)(agl_ptrdiff_t Opt, LPNOTIFICATION pN);
typedef agl_int32_t (AGL_API * LPFN_AGL_DeleteOptJob)(agl_int32_t ConnNr, agl_ptrdiff_t Opt);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetOptJobResult)(agl_int32_t ConnNr, agl_ptrdiff_t Opt, LPRESULT40 pR);
typedef agl_int32_t (AGL_API * LPFN_AGL_WaitForOptJob)(agl_int32_t ConnNr, agl_ptrdiff_t Opt);
typedef agl_ptrdiff_t (AGL_API * LPFN_AGL_AllocRWBuffs)(LPDATA_RW40 Buff, agl_int32_t Num);
typedef agl_int32_t (AGL_API * LPFN_AGL_FreeRWBuffs)(agl_ptrdiff_t Handle);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadRWBuff)(LPDATA_RW40 Buff, agl_int32_t Index, void* pData);
typedef agl_int32_t (AGL_API * LPFN_AGL_WriteRWBuff)(LPDATA_RW40 Buff, agl_int32_t Index, void* pData);
typedef agl_int32_t (AGL_API * LPFN_AGL_RKSend)(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_RKSendEx)(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_RKFetch)(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_RKFetchEx)(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Send_RKFetch)(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Recv_RKSend)(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Recv_RKFetch)(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Send_3964)(agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Recv_3964)(agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t* BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_BSend)(agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_BReceive)(agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_BSendEx)(agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t R_ID, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_BReceiveEx)(agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t* R_ID, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_USend)(agl_int32_t ConnNr, LPS7_USEND_URCV pUSR, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_UReceive)(agl_int32_t ConnNr, LPS7_USEND_URCV pUSR, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitOpStateMsg)(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ExitOpStateMsg)(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetOpStateMsg)(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitDiagMsg)(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t DiagMask, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ExitDiagMsg)(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetDiagMsg)(agl_int32_t ConnNr, LPS7_DIAG_MSG pDiag, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitCyclicRead)(agl_int32_t ConnNr, agl_int32_t CycleTime, agl_int32_t Flags, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t* Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitCyclicReadEx)(agl_int32_t ConnNr, agl_int32_t CycleTime, agl_int32_t Flags, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t* Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_StartCyclicRead)(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_StopCyclicRead)(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ExitCyclicRead)(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetCyclicRead)(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetCyclicReadEx)(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitScanMsg)(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ExitScanMsg)(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetScanMsg)(agl_int32_t ConnNr, LPS7_SCAN pScan, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_HasAckTriggeredMsg)(agl_int32_t ConnNr, agl_int32_t* Mode, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitAlarmMsg)(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ExitAlarmMsg)(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetAlarmMsg)(agl_int32_t ConnNr, LPS7_ALARM pAlarm, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadOpenMsg)(agl_int32_t ConnNr, LPS7_OPEN_MSG_STATE pState, agl_int32_t* MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetMsgStateChange)(agl_int32_t ConnNr, LPS7_RCV_MSG_STATE pState, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_AckMsg)(agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_LockMsg)(agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_UnlockMsg)(agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_InitARSend)(agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t ArAnz, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_ExitARSend)(agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t ArAnz, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetARSend)(agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t* AR_ID, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_RFC1006_Connect)(agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t* ConnNr, LPRFC_1006_SERVER ConnInfo, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_RFC1006_Disconnect)(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_RFC1006_Receive)(agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t BuffLen, agl_int32_t* ReceivedLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_RFC1006_Send)(agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_ReadMixEx)(agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_WriteMixEx)(agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_CheckVarSize)(agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_InitCyclicReadEx)(agl_int32_t ConnNr, agl_int32_t CycleTime, agl_int32_t OnlyChanged, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t* Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_StartCyclicRead)(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_StopCyclicRead)(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_ExitCyclicRead)(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_GetCyclicReadEx)(agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_EXTERN)(agl_int32_t ConnNr, agl_int32_t Channel, agl_cstr8_t ProgName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_EXTMOD)(agl_int32_t ConnNr, agl_int32_t Channel, agl_cstr8_t ProgName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_SELECT)(agl_int32_t ConnNr, agl_int32_t Channel, agl_cstr8_t ProgName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_F_DELE)(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_F_PROT)(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t Protection, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_F_RENA)(agl_int32_t ConnNr, agl_cstr8_t OldFileName, agl_cstr8_t NewFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_F_XFER)(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_LOGIN)(agl_int32_t ConnNr, agl_cstr8_t Password, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_LOGOUT)(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_F_OPEN)(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t WindowName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_F_OPER)(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t WindowName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_F_SEEK)(agl_int32_t ConnNr, agl_cstr8_t WindowName, agl_int32_t SeekMode, agl_int32_t SeekPointer, agl_int32_t WindowSize, agl_cstr8_t CompareString, agl_int32_t SkipCount, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_F_CLOS)(agl_int32_t ConnNr, agl_cstr8_t WindowName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_StartAll)(agl_int32_t ConnNr, agl_uint8_t* Para, agl_int32_t ParaLen, agl_cstr8_t Cmd, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_F_COPY)(agl_int32_t ConnNr, agl_int32_t Direction, agl_cstr8_t SourceFileName, agl_cstr8_t DestinationFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_F_PROR)(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t Protection, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_CANCEL)(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_CRCEDN)(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t DNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_DELECE)(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t DNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_DELETO)(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_IBN_SS)(agl_int32_t ConnNr, agl_int32_t Switch, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_MMCSEM)(agl_int32_t ConnNr, agl_int32_t ChannelNumber, agl_int32_t FunctionNumber, agl_int32_t SemaValue, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_TMCRTO)(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_cstr8_t ToolID, agl_int32_t ToolNumber, agl_int32_t DuploNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_TMMVTL)(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t ToolNumber, agl_int32_t SourcePlaceNumber, agl_int32_t SourceMagazineNumber, agl_int32_t DestinationPlaceNumber, agl_int32_t DestinationMagazineNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_TMCRTC)(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_cstr8_t ToolID, agl_int32_t ToolNumber, agl_int32_t DuploNumber, agl_int32_t EdgeNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_PI_CREATO)(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_CopyFileToNC)(agl_int32_t ConnNr, agl_cstr8_t NCFileName, agl_cstr8_t PCFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_CopyFileFromNC)(agl_int32_t ConnNr, agl_cstr8_t NCFileName, agl_cstr8_t PCFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_CopyToNC)(agl_int32_t ConnNr, agl_cstr8_t FileName, void* Buff, agl_int32_t BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_CopyFromNC)(agl_int32_t ConnNr, agl_cstr8_t FileName, void* Buff, agl_int32_t BuffLen, agl_int32_t* NeededLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_CopyFromNCAlloc)(agl_int32_t ConnNr, agl_cstr8_t FileName, void** Buff, agl_int32_t* BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_FreeBuff)(void* Buff);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_SetConnProgressNotification)(agl_int32_t ConnNr, LPNOTIFICATION pN);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_CheckNSKVarLine)(agl_cstr8_t Line, LPNCKDataRW pRW, agl_cstr8_t Name, agl_int32_t NameLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_ReadNSKVarFile)(agl_cstr8_t FileName, LPNCKDataRW* ppRW, agl_cstr8_t** pName);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_CheckCSVVarLine)(agl_cstr8_t Line, LPNCKDataRW pRW, agl_cstr8_t Name, agl_int32_t NameLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_ReadCSVVarFile)(agl_cstr8_t FileName, LPNCKDataRW* ppRW, agl_cstr8_t** pName);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_ReadGUDVarFile)(agl_cstr8_t FileName, LPNCKDataRW* ppRW, agl_cstr8_t** pName);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_ReadGUDVarFileEx)(agl_cstr8_t FileName, agl_int32_t GUDNr, agl_int32_t Area, LPNCKDataRW* ppRW, agl_cstr8_t** pName);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_FreeVarBuff)(LPNCKDataRW* ppRW, agl_cstr8_t** pName, agl_int32_t Anz);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_GetSingleVarDef)(LPNCKDataRW* ppRW, agl_cstr8_t** pName, agl_int32_t Index, LPNCKDataRW pRW, agl_cstr8_t Name, agl_int32_t NameLen, agl_int32_t AllocBuff);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_ExtractNckAlarm)(void* Buffer, agl_int32_t BufferSize, LPNCKAlarm NCKAlarm);
typedef agl_int32_t (AGL_API * LPFN_AGL_NCK_GetNCKDataRWByNCDDEItem)(const agl_cstr8_t const Item, LPNCKDataRW DataRW, agl_int32_t* ErrorPosition);
typedef agl_int32_t (AGL_API * LPFN_AGL_Drive_ReadMix)(agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Drive_ReadMixEx)(agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Drive_WriteMix)(agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Drive_WriteMixEx)(agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef void* (AGL_API * LPFN_AGL_malloc)(agl_int32_t Size);
typedef void* (AGL_API * LPFN_AGL_calloc)(agl_int32_t Anz, agl_int32_t Size);
typedef void* (AGL_API * LPFN_AGL_realloc)(void* Ptr, agl_int32_t Size);
typedef void* (AGL_API * LPFN_AGL_memcpy)(void* DestPtr, void* SrcPtr, agl_int32_t Len);
typedef void* (AGL_API * LPFN_AGL_memmove)(void* DestPtr, void* SrcPtr, agl_int32_t Len);
typedef agl_int32_t (AGL_API * LPFN_AGL_memcmp)(void* Ptr1, void* Ptr2, agl_int32_t Len);
typedef void (AGL_API * LPFN_AGL_free)(void* Ptr);
typedef agl_int16_t (AGL_API * LPFN_AGL_ReadInt16)(agl_uint8_t* Buff);
typedef agl_long32_t (AGL_API * LPFN_AGL_ReadInt32)(agl_uint8_t* Buff);
typedef agl_uint16_t (AGL_API * LPFN_AGL_ReadWord)(agl_uint8_t* Buff);
typedef agl_ulong32_t (AGL_API * LPFN_AGL_ReadDWord)(agl_uint8_t* Buff);
typedef agl_float32_t (AGL_API * LPFN_AGL_ReadReal)(agl_uint8_t* Buff);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadS5Time)(agl_uint8_t* Buff);
typedef agl_int32_t (AGL_API * LPFN_AGL_ReadS5TimeW)(agl_uint16_t* Buff);
typedef void (AGL_API * LPFN_AGL_WriteInt16)(agl_uint8_t* Buff, agl_int16_t Val);
typedef void (AGL_API * LPFN_AGL_WriteInt32)(agl_uint8_t* Buff, agl_long32_t Val);
typedef void (AGL_API * LPFN_AGL_WriteWord)(agl_uint8_t* Buff, agl_uint16_t Val);
typedef void (AGL_API * LPFN_AGL_WriteDWord)(agl_uint8_t* Buff, agl_ulong32_t Val);
typedef void (AGL_API * LPFN_AGL_WriteReal)(agl_uint8_t* Buff, agl_float32_t Val);
typedef void (AGL_API * LPFN_AGL_WriteS5Time)(agl_uint8_t* Buff, agl_int32_t Val);
typedef void (AGL_API * LPFN_AGL_WriteS5TimeW)(agl_uint16_t* Buff, agl_int32_t Val);
typedef void (AGL_API * LPFN_AGL_Byte2Word)(agl_uint16_t* OutBuff, agl_uint8_t* InBuff, agl_int32_t AnzWords);
typedef void (AGL_API * LPFN_AGL_Byte2DWord)(agl_ulong32_t* OutBuff, agl_uint8_t* InBuff, agl_int32_t AnzDWords);
typedef void (AGL_API * LPFN_AGL_Byte2Real)(agl_float32_t* OutBuff, agl_uint8_t* InBuff, agl_int32_t AnzReals);
typedef void (AGL_API * LPFN_AGL_Word2Byte)(agl_uint8_t* OutBuff, agl_uint16_t* InBuff, agl_int32_t AnzWords);
typedef void (AGL_API * LPFN_AGL_DWord2Byte)(agl_uint8_t* OutBuff, agl_ulong32_t* InBuff, agl_int32_t AnzDWords);
typedef void (AGL_API * LPFN_AGL_Real2Byte)(agl_uint8_t* OutBuff, agl_float32_t* InBuff, agl_int32_t AnzReals);
typedef agl_int32_t (AGL_API * LPFN_AGL_GetBit)(agl_uint8_t Wert, agl_int32_t BitNr);
typedef agl_uint8_t (AGL_API * LPFN_AGL_SetBit)(agl_uint8_t* Buff, agl_int32_t BitNr);
typedef agl_uint8_t (AGL_API * LPFN_AGL_ResetBit)(agl_uint8_t* Buff, agl_int32_t BitNr);
typedef agl_uint8_t (AGL_API * LPFN_AGL_SetBitVal)(agl_uint8_t* Buff, agl_int32_t BitNr, agl_int32_t Val);
typedef agl_int32_t (AGL_API * LPFN_AGL_Buff2String)(agl_uint8_t* Buff, agl_cstr8_t Text, agl_int32_t AnzChars);
typedef agl_int32_t (AGL_API * LPFN_AGL_String2Buff)(agl_uint8_t* Buff, agl_cstr8_t Text, agl_int32_t AnzChars);
typedef agl_int32_t (AGL_API * LPFN_AGL_Buff2WString)(agl_uint8_t* Buff, agl_wchar_t* Text, agl_int32_t AnzChars);
typedef agl_int32_t (AGL_API * LPFN_AGL_WString2Buff)(agl_uint8_t* Buff, agl_wchar_t* Text, agl_int32_t AnzChars);
typedef agl_int32_t (AGL_API * LPFN_AGL_S7String2String)(agl_uint8_t* S7String, agl_cstr8_t Text, agl_int32_t MaxChars);
typedef agl_int32_t (AGL_API * LPFN_AGL_String2S7String)(agl_uint8_t* S7String, agl_cstr8_t Text, agl_int32_t MaxChars);
typedef agl_int32_t (AGL_API * LPFN_AGL_BCD2Int16)(agl_int16_t BCD, agl_int16_t* Dual);
typedef agl_int32_t (AGL_API * LPFN_AGL_BCD2Int32)(agl_long32_t BCD, agl_long32_t* Dual);
typedef agl_int32_t (AGL_API * LPFN_AGL_Int162BCD)(agl_int16_t Dual, agl_int16_t* BCD);
typedef agl_int32_t (AGL_API * LPFN_AGL_Int322BCD)(agl_long32_t Dual, agl_long32_t* BCD);
typedef agl_float32_t (AGL_API * LPFN_AGL_LongAsFloat)(agl_long32_t Var);
typedef agl_long32_t (AGL_API * LPFN_AGL_FloatAsLong)(agl_float32_t Var);
typedef agl_int32_t (AGL_API * LPFN_AGL_Text2DataRW)(agl_cstr8_t Text, LPDATA_RW40 RW);
typedef agl_int32_t (AGL_API * LPFN_AGL_DataRW2Text)(LPDATA_RW40 RW, agl_cstr8_t Text);
typedef agl_int32_t (AGL_API * LPFN_AGL_S7DT2SysTime)(agl_uint8_t* Buff, agl_systemtime_t* SysTime);
typedef agl_int32_t (AGL_API * LPFN_AGL_SysTime2S7DT)(agl_systemtime_t* SysTime, agl_uint8_t* Buff);
typedef agl_int32_t (AGL_API * LPFN_AGL_TOD2SysTime)(LPTOD pTOD, agl_systemtime_t* SysTime);
typedef agl_int32_t (AGL_API * LPFN_AGL_SysTime2TOD)(agl_systemtime_t* SysTime, LPTOD pTOD);
typedef agl_int32_t (AGL_API * LPFN_AGL_S7Date2YMD)(agl_uint16_t Date, agl_uint16_t* Year, agl_uint16_t* Month, agl_uint16_t* Day);
typedef agl_int32_t (AGL_API * LPFN_AGL_Float2KG)(agl_uint16_t* pKG, agl_float32_t* pFloat, agl_int32_t AnzFloats);
typedef agl_int32_t (AGL_API * LPFN_AGL_KG2Float)(agl_float32_t* pFloat, agl_uint16_t* pKG, agl_int32_t AnzFloats);
typedef agl_int32_t (AGL_API * LPFN_AGL_Float2DWKG)(agl_ulong32_t* pKG, agl_float32_t* pFloat, agl_int32_t AnzFloats);
typedef agl_int32_t (AGL_API * LPFN_AGL_DWKG2Float)(agl_float32_t* pFloat, agl_ulong32_t* pKG, agl_int32_t AnzFloats);
typedef agl_int32_t (AGL_API * LPFN_AGL_S7Ident2String)(LPS7_IDENT pIdent, agl_int32_t Index, agl_cstr8_t Text, agl_int32_t MaxChars);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_OpenProject)(agl_cstr8_t  Project, agl_ptrdiff_t* PrjHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_CloseProject)(agl_ptrdiff_t PrjHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_WriteCpuListToFile)(agl_cstr8_t  FileName);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetProgramCount)(agl_ptrdiff_t PrjHandle, agl_int32_t* ProgCount);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindFirstProgram)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Program);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindNextProgram)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Program);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindCloseProgram)(agl_ptrdiff_t PrjHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_SelectProgram)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Program);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetSymbolCount)(agl_ptrdiff_t PrjHandle, agl_int32_t* SymCount);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetSymbolCountFilter)(agl_ptrdiff_t PrjHandle, agl_int32_t* SymCount, const agl_cstr8_t Filter);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindFirstSymbol)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindFirstSymbolFilter)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format, const agl_cstr8_t Filter);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindNextSymbol)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindCloseSymbol)(agl_ptrdiff_t PrjHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_ReadPrjDBCount)(agl_ptrdiff_t PrjHandle, agl_int32_t* DBCount);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_ReadPrjDBCountFilter)(agl_ptrdiff_t PrjHandle, agl_int32_t* DBCount, const agl_cstr8_t Filter);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_ReadPrjDBList)(agl_ptrdiff_t PrjHandle, agl_uint16_t* DBList, agl_int32_t DBCount);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_ReadPrjDBListFilter)(agl_ptrdiff_t PrjHandle, agl_uint16_t* DBList, agl_int32_t DBCount, const agl_cstr8_t Filter);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_ReadPrjBlkCountFilter)(agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_int32_t* BlkCount, const agl_cstr8_t Filter);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_ReadPrjBlkListFilter)(agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_uint16_t* BlkList, agl_int32_t BlkCount, const agl_cstr8_t Filter);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetDbSymbolCount)(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_int32_t* DBSymCount);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetDbSymbolCountFilter)(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_int32_t* DBSymCount, const agl_cstr8_t Filter);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindFirstDbSymbol)(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindFirstDbSymbolFilter)(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format, const agl_cstr8_t Filter);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindNextDbSymbol)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindFirstDbSymbolEx)(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, LPDATA_DBSYM40 Buff, const agl_cstr8_t Filter);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindNextDbSymbolEx)(agl_ptrdiff_t PrjHandle, LPDATA_DBSYM40 Buff);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetDbSymbolExComment)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  ExComment);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindCloseDbSymbol)(agl_ptrdiff_t PrjHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetDbDependency)(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_int32_t* BlkType, agl_int32_t* BlkNr);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetDeclarationCountFilter)(agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_int32_t BlkNr, const agl_cstr8_t Filter, agl_int32_t* DeclarationCount);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindFirstDeclarationFilter)(agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_int32_t BlkNr, const agl_cstr8_t Filter, agl_ptrdiff_t* FindHandle, LPDATA_DECLARATION Buff);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindNextDeclaration)(agl_ptrdiff_t PrjHandle, agl_ptrdiff_t FindHandle, LPDATA_DECLARATION Buff);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetDeclarationInitialValue)(agl_ptrdiff_t PrjHandle, agl_ptrdiff_t FindHandle, agl_int32_t* BufferLength, agl_cstr8_t  InitialValue);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindCloseDeclaration)(agl_ptrdiff_t PrjHandle, agl_ptrdiff_t FindHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetSymbolFromText)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetSymbolFromTextEx)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, LPDATA_DBSYM40 Buff);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetReadMixFromText)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, LPDATA_RW40 Buff, agl_int32_t* Format);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetReadMixFromTextEx)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, agl_cstr8_t  AbsOpd, LPDATA_RW40 Buff, agl_int32_t* Format);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetSymbol)(agl_ptrdiff_t PrjHandle, LPDATA_RW40 Buff, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetSymbolEx)(agl_ptrdiff_t PrjHandle, LPDATA_RW40 Buff, LPDATA_DBSYM40 Symbol);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_OpenAlarms)(agl_ptrdiff_t PrjHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_CloseAlarms)(agl_ptrdiff_t PrjHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindFirstAlarmData)(agl_ptrdiff_t PrjHandle, agl_int32_t* AlmNr);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindNextAlarmData)(agl_ptrdiff_t PrjHandle, agl_int32_t* AlmNr);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindCloseAlarmData)(agl_ptrdiff_t PrjHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmData)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, LPDATA_ALARM40 Buff);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmName)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmType)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmBaseName)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmTypeName)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmSignalCount)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* SignalCount);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmMsgClass)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* MsgClass);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmPriority)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* Priority);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmAckGroup)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* AckGroup);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmAcknowledge)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* Acknowledge);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmProtocol)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* Protocol);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmDispGroup)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* DispGroup);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindFirstAlarmTextLanguage)(agl_ptrdiff_t PrjHandle, agl_int32_t* Language, agl_int32_t* IsDefault);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindNextAlarmTextLanguage)(agl_ptrdiff_t PrjHandle, agl_int32_t* Language, agl_int32_t* IsDefault);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindCloseAlarmTextLanguage)(agl_ptrdiff_t PrjHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_SetAlarmTextDefaultLanguage)(agl_ptrdiff_t PrjHandle, agl_int32_t Language);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmText)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t Language, agl_cstr8_t Buff, agl_int32_t BuffLen);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmInfo)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t Language, agl_cstr8_t Buff, agl_int32_t BuffLen);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmAddText)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t Index, agl_int32_t Language, agl_cstr8_t Buff, agl_int32_t BuffLen);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmSCANOperand)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* OpArea, agl_int32_t* OpType, agl_int32_t* Offset, agl_int32_t* BitNr);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmSCANInterval)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* Interval);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmSCANAddValue)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Index, agl_int32_t* OpArea, agl_int32_t* OpType, agl_int32_t* Offset, agl_int32_t* BitNr);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmSCANOperandEx)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, LPDATA_RW40 Buff);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetAlarmSCANAddValueEx)(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Index, LPDATA_RW40 Buff);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FormatMessage)(agl_ptrdiff_t PrjHandle, const agl_cstr8_t AlarmText, LPS7_ALARM AlarmData, agl_cstr8_t Buff, agl_int32_t BuffLen);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindFirstTextlib)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Textlib, agl_int32_t* System);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindNextTextlib)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Textlib, agl_int32_t* System);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindCloseTextlib)(agl_ptrdiff_t PrjHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_SelectTextlib)(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Textlib);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindFirstTextlibText)(agl_ptrdiff_t PrjHandle, agl_int32_t* TextId, agl_cstr8_t Buff, agl_int32_t BuffLen);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindNextTextlibText)(agl_ptrdiff_t PrjHandle, agl_int32_t* TextId, agl_cstr8_t Buff, agl_int32_t BuffLen);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_FindCloseTextlibText)(agl_ptrdiff_t PrjHandle);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetTextlibText)(agl_ptrdiff_t PrjHandle, agl_int32_t  TextId, agl_cstr8_t Buff, agl_int32_t BuffLen);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetTextFromValue)(void* Value, agl_int32_t Format, agl_int32_t ValueFmt, agl_cstr8_t Text);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetValueFromText)(agl_cstr8_t Text, void* Value, agl_int32_t* Format, agl_int32_t* ValueFmt);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetRealFromText)(agl_cstr8_t Text, agl_float32_t* Value);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_GetTextFromReal)(agl_float32_t* Value, agl_cstr8_t Text);
typedef agl_int32_t (AGL_API * LPFN_AGLSym_SetLanguage)(agl_int32_t Language);
typedef agl_int32_t (AGL_API * LPFN_AGL_WLD_OpenFile)(const agl_cstr8_t FileName, agl_int32_t Access, agl_ptrdiff_t* Handle);
typedef agl_int32_t (AGL_API * LPFN_AGL_WLD_OpenFileEncrypted)(const agl_cstr8_t FileName, agl_int32_t Access, agl_uint8_t* Key, agl_uint32_t Len, agl_ptrdiff_t* Handle);
typedef agl_int32_t (AGL_API * LPFN_AGL_WLD_EncryptFile)(const agl_cstr8_t InFileName, const agl_cstr8_t OutFileName, agl_int32_t Access, agl_uint8_t* Key, agl_uint32_t Len);
typedef agl_int32_t (AGL_API * LPFN_AGL_WLD_DecryptFile)(const agl_cstr8_t InFileName, const agl_cstr8_t OutFileName, agl_uint8_t* Key, agl_uint32_t Len);
typedef agl_int32_t (AGL_API * LPFN_AGL_WLD_CloseFile)(agl_ptrdiff_t Handle);
typedef agl_int32_t (AGL_API * LPFN_AGL_WLD_ReadAllBlockCount)(agl_ptrdiff_t Handle, LPALL_BLOCK_COUNT pBC);
typedef agl_int32_t (AGL_API * LPFN_AGL_WLD_ReadBlockCount)(agl_ptrdiff_t Handle, agl_int32_t BlockType, agl_int32_t* BlockCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_WLD_ReadBlockList)(agl_ptrdiff_t Handle, agl_int32_t BlockType, agl_int32_t* BlockCount, agl_uint16_t* BlockList);
typedef agl_int32_t (AGL_API * LPFN_AGL_WLD_ReadBlockLen)(agl_ptrdiff_t Handle, agl_int32_t BlockType, agl_int32_t BlockNr, agl_int32_t* BlockLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_WLD_DeleteBlocks)(agl_ptrdiff_t Handle, const agl_cstr8_t Blocks);
typedef agl_int32_t (AGL_API * LPFN_AGL_WLD_GetReport)(agl_ptrdiff_t Handle, agl_int32_t* Length, agl_cstr8_t Buffer);
typedef agl_int32_t (AGL_API * LPFN_AGL_PLC_Backup)(agl_int32_t ConnNr, agl_ptrdiff_t Handle, const agl_cstr8_t Blocks, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_PLC_Restore)(agl_int32_t ConnNr, agl_ptrdiff_t Handle, const agl_cstr8_t Blocks, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_PLC_DeleteBlocks)(agl_int32_t ConnNr, const agl_cstr8_t Blocks, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Compress)(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_ReadMixEx)(agl_int32_t ConnNr, SymbolicRW_t* SymbolicRW, agl_int32_t Num, agl_int32_t* SError, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_WriteMixEx)(agl_int32_t ConnNr, SymbolicRW_t* SymbolicRW, agl_int32_t Num, agl_int32_t* SError, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_LoadTIAProjectSymbols)(const agl_cstr8_t const ProjectFile, HandleType* const RootNodeHandle, agl_int32_t AutoExpand);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_LoadAGLinkSymbolsFromPLC)(const int ConnNr, HandleType* const RootNodeHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SaveAGLinkSymbolsToFile)(HandleType RootNodeHandle, const agl_cstr8_t const AGLinkSymbolsFile);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_LoadAGLinkSymbolsFromFile)(const agl_cstr8_t const AGLinkSymbolsFile, HandleType* const RootNodeHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_FreeHandle)(const HandleType Handle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetChildCount)(const HandleType NodeHandle, agl_int32_t* const ChildCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetChild)(const HandleType NodeHandle, const agl_int32_t ChildIndex, HandleType* const ChildNodeHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetChildByName)(const HandleType NodeHandle, const agl_cstr8_t const ChildName, HandleType* const ChildNodeHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetName)(const HandleType NodeHandle, agl_cstr8_t const NameBuffer, const agl_int32_t NameBufferLen, agl_int32_t* const NameLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetLocalOffset)(const HandleType NodeHandle, agl_uint32_t* const LocalByteOffset, agl_uint32_t* const LocalBitOffset);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetSystemType)(const HandleType NodeHandle, SystemType_t::enum_t* SystemType);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetHierarchyType)(const HandleType NodeHandle, HierarchyType_t::enum_t* HierarchyType);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetArrayDimensionCount)(const HandleType ArrayNodeHandle, agl_int32_t* DimensionCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetArrayDimension)(const HandleType ArrayNodeHandle, const agl_int32_t Dimension, agl_int32_t* Lower, agl_int32_t* Upper);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetMaxStringSize)(const HandleType StringNodeHandle, agl_int32_t* const StringSize);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetValueType)(const HandleType NodeHandle, ValueType_t::enum_t* ValueType);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetTypeState)(const HandleType NodeHandle, TypeState_t::enum_t* TypeState);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetSegmentType)(const HandleType NodeHandle, SegmentType_t::enum_t* SegementType);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetPermissionType)(const HandleType NodeHandle, PermissionType_t::enum_t* PermissionType);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_EscapeString)(const agl_cstr8_t const RawString, agl_cstr8_t const EscapedString, const agl_int32_t EscapedStringMaxSize, agl_int32_t* ErrorPosition);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetNodeByPath)(const HandleType NodeHandle, const agl_cstr8_t const ItemPath, HandleType* const FoundNodeHandle, agl_int32_t* const ErrorPosition);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetIndexSize)(const HandleType IndexNodeHandle, agl_size_t* IndexSize);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetIndex)(const HandleType IndexNodeHandle, const agl_int32_t Element, agl_int32_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetLinearIndex)(const HandleType IndexNodeHandle, agl_size_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetArrayElementCount)(const HandleType ArrayNodeHandle, agl_int32_t* ElementCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_Expand)(const HandleType NodeHandle, const agl_int32_t Depth);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_Collapse)(const HandleType NodeHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetSystemScope)(const HandleType NodeHandle, SystemType_t::enum_t* const SystemType);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetSystemTypeState)(const HandleType NodeHandle, SystemTypeState_t::enum_t* SystemTypeState);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_CreateAccess)(const HandleType NodeHandle, HandleType* const AccessHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_CreateAccessByPath)(const HandleType ParentNodeHandle, const agl_cstr8_t const ItemPath, HandleType* const AccessHandle, agl_int32_t* const ErrorPosition);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_Get_DATA_RW40)(const HandleType NodeHandle, DATA_RW40* const DataRW, agl_int32_t* const Size);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_Get_DATA_RW40_ByPath)(const HandleType NodeHandle, const agl_cstr8_t const ItemPath, DATA_RW40* const DataRW, agl_int32_t* const ErrorPosition, agl_int32_t* const Size);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetType)(const HandleType NodeHandle, HandleType* const TypeNodeHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferSize)(const HandleType AccessHandle, agl_int32_t* const BufferSize);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferElementSize)(const HandleType AccessHandle, agl_int32_t* const ElementSize);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessStringSize)(const HandleType AccessHandle, agl_int32_t* const StringSize);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferElementCount)(const HandleType AccessHandle, agl_int32_t* const ElementCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessElementSystemType)(const HandleType AccessHandle, SystemType_t::enum_t* const SystemType);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessElementValueType)(const HandleType AccessHandle, ValueType_t::enum_t* const ValueType);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetArrayIndexAsLinearIndex)(const HandleType ArrayNodeHandle, const agl_int32_t* const Index, const agl_int32_t IndexCount, agl_int32_t* const LinearIndex);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetArrayLinearIndexAsIndex)(const HandleType ArrayNodeHandle, const agl_int32_t LinearIndex, agl_int32_t* const Index, const agl_int32_t MaxIndexCount, agl_int32_t* const IndexCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_CreateArrayAccessByLinearIndex)(const HandleType ArrayNodeHandle, const agl_int32_t LinearIndex, HandleType* const AccessHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_CreateArrayRangeAccessByLinearIndex)(const HandleType ArrayNodeHandle, const agl_int32_t LinearIndex, const agl_int32_t Count, HandleType* const AccessHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_CreateArrayAccessByIndex)(const HandleType ArrayNodeHandle, const agl_int32_t* Index, const agl_int32_t IndexCount, HandleType* const AccessHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_CreateArrayRangeAccessByIndex)(const HandleType ArrayNodeHandle, const agl_int32_t* Index, const agl_int32_t IndexCount, const agl_int32_t Count, HandleType* const AccessHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferUInt8)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint8_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferUInt16)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferUInt32)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint32_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferUInt64)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint64_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferInt8)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int8_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferInt16)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int16_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferInt32)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int32_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferInt64)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int64_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferFloat32)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_float32_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferFloat64)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_float64_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferChar8)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_cstr8_t const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferChar16)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_char16_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferString8)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_cstr8_t const StringBuffer, agl_int32_t MaxCharCount, agl_int32_t* const CharCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferString16)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_char16_t* const StringBuffer, agl_int32_t MaxCharCount, agl_int32_t* const CharCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferS7_DTLParts)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t* const Year, agl_uint8_t* const Month, agl_uint8_t* const Day, agl_uint8_t* const WeekDay, agl_uint8_t* const Hour, agl_uint8_t* const Minute, agl_uint8_t* const Second, agl_uint32_t* const Nanoseconds);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferS7_S5TimeParts)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t* const TimeBase, agl_uint16_t* const TimeValue);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferS7_S5TimeMs)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint32_t* const Milliseconds);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferS7_Counter)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t* const Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAccessBufferS7_Date_and_TimeParts)(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t* const Year, agl_uint8_t* const Month, agl_uint8_t* const Day, agl_uint8_t* const WeekDay, agl_uint8_t* const Hour, agl_uint8_t* const Minute, agl_uint8_t* const Second, agl_uint16_t* const Millisecond);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferUInt8)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint8_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferUInt16)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferUInt32)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint32_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferUInt64)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint64_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferInt8)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int8_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferInt16)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int16_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferInt32)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int32_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferInt64)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int64_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferFloat32)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_float32_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferFloat64)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_float64_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferChar8)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_char8_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferChar16)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_char16_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferString8)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_cstr8_t const StringBuffer, const agl_int32_t CharCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferString16)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_char16_t* const StringBuffer, const agl_int32_t CharCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferS7_DTLParts)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_uint16_t Year, const agl_uint8_t Month, const agl_uint8_t Day, agl_uint8_t WeekDay, const agl_uint8_t Hour, const agl_uint8_t Minute, const agl_uint8_t Second, const agl_uint32_t Nanoseconds);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferS7_S5TimeParts)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_uint16_t TimeBase, const agl_uint16_t TimeValue);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferS7_S5TimeMs)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_uint32_t Milliseconds, const agl_int32_t Round);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferS7_Counter)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_uint16_t Value);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SetAccessBufferS7_Date_and_TimeParts)(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_uint16_t Year, const agl_uint8_t Month, const agl_uint8_t Day, const agl_uint8_t WeekDay, const agl_uint8_t Hour, const agl_uint8_t Minute, const agl_uint8_t Second, const agl_uint16_t Milliseconds);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetProjectEditingCulture)(const HandleType RootNodeHandle, agl_int32_t* const LCID);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetProjectReferenceCulture)(const HandleType RootNodeHandle, agl_int32_t* const LCID);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetProjectCultureCount)(const HandleType RootNodeHandle, agl_int32_t* const Count);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetProjectCulture)(const HandleType RootNodeHandle, const agl_int32_t CultureIndex, agl_int32_t* const LCID);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetComment)(const HandleType NodeHandle, const agl_int32_t LCID, agl_cstr8_t const Comment, const agl_int32_t CommentMaxSize, agl_int32_t* const UsedByteCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetCommentCultureCount)(const HandleType NodeHandle, agl_int32_t* const Count);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetCommentCulture)(const HandleType NodeHandle, const agl_int32_t CultureIndex, agl_int32_t* const LCID);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_DatablockGetNumber)(const HandleType NodeHandle, agl_int32_t* const Number);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetPath)(const HandleType NodeHandle, agl_cstr8_t const PathBuffer, const agl_int32_t MaxPathBufferSize, agl_int32_t* const UsedPathBufferSize);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetEscapedPath)(const HandleType NodeHandle, agl_cstr8_t const PathBuffer, const agl_int32_t MaxPathBufferSize, agl_int32_t* const UsedPathBufferSize);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAttributeHMIAccessible)(const HandleType NodeHandle, agl_int32_t* const BooleanValue);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAttributeHMIVisible)(const HandleType NodeHandle, agl_int32_t* const BooleanValue);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetAttributeRemanent)(const HandleType NodeHandle, agl_int32_t* const BooleanValue);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetS7PlcTypeName)(const HandleType NodeHandle, agl_cstr8_t const TypeNameBuffer, const agl_int32_t TypeNameBufferLen, agl_int32_t* const TypeNameLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetS7PlcFirmware)(const HandleType NodeHandle, agl_cstr8_t const FirmwareBuffer, const agl_int32_t FirmwareBufferLen, agl_int32_t* const FirmwareLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetS7PlcMLFB)(const HandleType NodeHandle, agl_cstr8_t const MLFBBuffer, const agl_int32_t MLFBBufferLen, agl_int32_t* const MLFBLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_GetS7PlcFamily)(const HandleType NodeHandle, S7PlcFamily_t::enum_t* const S7PlcFamily);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_SaveSingleValueAccessSymbolsToFile)(const HandleType RootHandle, const agl_cstr8_t const SingleValueFilterFile, const agl_cstr8_t const LogFile, const agl_cstr8_t const AglinkSingleValueAccessSymbolFile);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_LoadSingleValueAccessSymbolsFromFile)(const agl_cstr8_t const AglinkSingleValueAccessSymbolsFile, HandleType* const SingleValueAccessSymbolsHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Symbolic_CreateAccessFromSingleValueAccessSymbols)(const HandleType SingleValueAccessSymbolsHandle, const agl_cstr8_t const Symbol, HandleType* const AccessHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_LoadSTISymbols)(const agl_cstr8_t const STIFile, HandleType* const RootNodeHandle, agl_bool_t FlatArrays);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_FreeHandle)(const HandleType RootNodeHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_GetName)(const HandleType NodeHandle, agl_cstr8_t const NameBuffer, const agl_int32_t NameBufferLen);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_GetHierarchyType)(const HandleType NodeHandle, HierarchyType_t::enum_t* HierarchyType);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_GetValueType)(const HandleType NodeHandle, ValueType_t::enum_t* ValueType);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_GetPermissionType)(const HandleType NodeHandle, PermissionType_t::enum_t* PermissionType);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_GetChildCount)(const HandleType NodeHandle, agl_int32_t* const ChildCount);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_GetChild)(const HandleType NodeHandle, const agl_int32_t ChildIndex, HandleType* const ChildNodeHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_GetNodeByPath)(const HandleType NodeHandle, const agl_cstr8_t const ItemPath, HandleType* const FoundNodeHandle, agl_int32_t* const ErrorPosition);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_CreateAccess)(const HandleType NodeHandle, HandleType* const AccessHandle);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_CreateAccessByPath)(const HandleType ParentNodeHandle, const agl_cstr8_t const ItemPath, HandleType* const AccessHandle, agl_int32_t* const ErrorPosition);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_ReadMixEx)(agl_int32_t ConnNr, SymbolicRW_t* Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);
typedef agl_int32_t (AGL_API * LPFN_AGL_Simotion_WriteMixEx)(agl_int32_t ConnNr, SymbolicRW_t* Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal);

/*******************************************************************************
Deklaration der lokalen Funktionszeiger
*******************************************************************************/

LPFN_AGL_Activate pAGL_Activate = NULL;
LPFN_AGL_GetVersion pAGL_GetVersion = NULL;
LPFN_AGL_GetVersionEx pAGL_GetVersionEx = NULL;
LPFN_AGL_GetOptions pAGL_GetOptions = NULL;
LPFN_AGL_GetSerialNumber pAGL_GetSerialNumber = NULL;
LPFN_AGL_GetClientName pAGL_GetClientName = NULL;
LPFN_AGL_GetMaxDevices pAGL_GetMaxDevices = NULL;
LPFN_AGL_GetMaxQueues pAGL_GetMaxQueues = NULL;
LPFN_AGL_GetMaxPLCPerDevice pAGL_GetMaxPLCPerDevice = NULL;
LPFN_AGL_UseSystemTime pAGL_UseSystemTime = NULL;
LPFN_AGL_ReturnJobNr pAGL_ReturnJobNr = NULL;
LPFN_AGL_SetBSendAutoResponse pAGL_SetBSendAutoResponse = NULL;
LPFN_AGL_GetBSendAutoResponse pAGL_GetBSendAutoResponse = NULL;
LPFN_AGL_GetPCCPConnNames pAGL_GetPCCPConnNames = NULL;
LPFN_AGL_GetPCCPProtocol pAGL_GetPCCPProtocol = NULL;
LPFN_AGL_GetTapiModemNames pAGL_GetTapiModemNames = NULL;
LPFN_AGL_GetLocalIPAddresses pAGL_GetLocalIPAddresses = NULL;
LPFN_AGL_GetTickCount pAGL_GetTickCount = NULL;
LPFN_AGL_GetMicroSecs pAGL_GetMicroSecs = NULL;
LPFN_AGL_UnloadDyn pAGL_UnloadDyn = NULL;
LPFN_AGL_Config pAGL_Config = NULL;
LPFN_AGL_ConfigEx pAGL_ConfigEx = NULL;
LPFN_AGL_SetParas pAGL_SetParas = NULL;
LPFN_AGL_GetParas pAGL_GetParas = NULL;
LPFN_AGL_SetDevType pAGL_SetDevType = NULL;
LPFN_AGL_GetDevType pAGL_GetDevType = NULL;
LPFN_AGL_ReadParas pAGL_ReadParas = NULL;
LPFN_AGL_WriteParas pAGL_WriteParas = NULL;
LPFN_AGL_ReadDevice pAGL_ReadDevice = NULL;
LPFN_AGL_WriteDevice pAGL_WriteDevice = NULL;
LPFN_AGL_ReadParasFromFile pAGL_ReadParasFromFile = NULL;
LPFN_AGL_WriteParasToFile pAGL_WriteParasToFile = NULL;
LPFN_AGL_GetParaPath pAGL_GetParaPath = NULL;
LPFN_AGL_SetParaPath pAGL_SetParaPath = NULL;
LPFN_AGL_SetAndGetParaPath pAGL_SetAndGetParaPath = NULL;
LPFN_AGL_GetPLCType pAGL_GetPLCType = NULL;
LPFN_AGL_HasFunc pAGL_HasFunc = NULL;
LPFN_AGL_LoadErrorFile pAGL_LoadErrorFile = NULL;
LPFN_AGL_GetErrorMsg pAGL_GetErrorMsg = NULL;
LPFN_AGL_GetErrorCodeName pAGL_GetErrorCodeName = NULL;
LPFN_AGL_OpenDevice pAGL_OpenDevice = NULL;
LPFN_AGL_CloseDevice pAGL_CloseDevice = NULL;
LPFN_AGL_SetDevNotification pAGL_SetDevNotification = NULL;
LPFN_AGL_SetConnNotification pAGL_SetConnNotification = NULL;
LPFN_AGL_SetJobNotification pAGL_SetJobNotification = NULL;
LPFN_AGL_SetJobNotificationEx pAGL_SetJobNotificationEx = NULL;
LPFN_AGL_GetJobResult pAGL_GetJobResult = NULL;
LPFN_AGL_GetLastJobResult pAGL_GetLastJobResult = NULL;
LPFN_AGL_DeleteJob pAGL_DeleteJob = NULL;
LPFN_AGL_WaitForJob pAGL_WaitForJob = NULL;
LPFN_AGL_WaitForJobEx pAGL_WaitForJobEx = NULL;
LPFN_AGL_DialUp pAGL_DialUp = NULL;
LPFN_AGL_HangUp pAGL_HangUp = NULL;
LPFN_AGL_InitAdapter pAGL_InitAdapter = NULL;
LPFN_AGL_ExitAdapter pAGL_ExitAdapter = NULL;
LPFN_AGL_GetLifelist pAGL_GetLifelist = NULL;
LPFN_AGL_GetDirectPLC pAGL_GetDirectPLC = NULL;
LPFN_AGL_PLCConnect pAGL_PLCConnect = NULL;
LPFN_AGL_PLCConnectEx pAGL_PLCConnectEx = NULL;
LPFN_AGL_PLCDisconnect pAGL_PLCDisconnect = NULL;
LPFN_AGL_ReadMaxPacketSize pAGL_ReadMaxPacketSize = NULL;
LPFN_AGL_GetRedConnState pAGL_GetRedConnState = NULL;
LPFN_AGL_GetRedConnStateMsg pAGL_GetRedConnStateMsg = NULL;
LPFN_AGL_ReadOpState pAGL_ReadOpState = NULL;
LPFN_AGL_ReadOpStateEx pAGL_ReadOpStateEx = NULL;
LPFN_AGL_GetPLCStartOptions pAGL_GetPLCStartOptions = NULL;
LPFN_AGL_PLCStop pAGL_PLCStop = NULL;
LPFN_AGL_PLCStart pAGL_PLCStart = NULL;
LPFN_AGL_PLCResume pAGL_PLCResume = NULL;
LPFN_AGL_PLCColdStart pAGL_PLCColdStart = NULL;
LPFN_AGL_IsHPLC pAGL_IsHPLC = NULL;
LPFN_AGL_HPLCStop pAGL_HPLCStop = NULL;
LPFN_AGL_HPLCStart pAGL_HPLCStart = NULL;
LPFN_AGL_HPLCColdStart pAGL_HPLCColdStart = NULL;
LPFN_AGL_GetPLCClock pAGL_GetPLCClock = NULL;
LPFN_AGL_SetPLCClock pAGL_SetPLCClock = NULL;
LPFN_AGL_SyncPLCClock pAGL_SyncPLCClock = NULL;
LPFN_AGL_ReadMLFBNr pAGL_ReadMLFBNr = NULL;
LPFN_AGL_ReadMLFBNrEx pAGL_ReadMLFBNrEx = NULL;
LPFN_AGL_ReadPLCInfo pAGL_ReadPLCInfo = NULL;
LPFN_AGL_ReadCycleTime pAGL_ReadCycleTime = NULL;
LPFN_AGL_ReadProtLevel pAGL_ReadProtLevel = NULL;
LPFN_AGL_ReadS7Ident pAGL_ReadS7Ident = NULL;
LPFN_AGL_ReadS7LED pAGL_ReadS7LED = NULL;
LPFN_AGL_GetExtModuleInfo pAGL_GetExtModuleInfo = NULL;
LPFN_AGL_ReadSzl pAGL_ReadSzl = NULL;
LPFN_AGL_IsPasswordReq pAGL_IsPasswordReq = NULL;
LPFN_AGL_SetPassword pAGL_SetPassword = NULL;
LPFN_AGL_UnSetPassword pAGL_UnSetPassword = NULL;
LPFN_AGL_ReadDiagBufferEntrys pAGL_ReadDiagBufferEntrys = NULL;
LPFN_AGL_ReadDiagBuffer pAGL_ReadDiagBuffer = NULL;
LPFN_AGL_GetDiagBufferEntry pAGL_GetDiagBufferEntry = NULL;
LPFN_AGL_ReadDBCount pAGL_ReadDBCount = NULL;
LPFN_AGL_ReadDBList pAGL_ReadDBList = NULL;
LPFN_AGL_ReadDBLen pAGL_ReadDBLen = NULL;
LPFN_AGL_ReadAllBlockCount pAGL_ReadAllBlockCount = NULL;
LPFN_AGL_ReadBlockCount pAGL_ReadBlockCount = NULL;
LPFN_AGL_ReadBlockList pAGL_ReadBlockList = NULL;
LPFN_AGL_ReadBlockLen pAGL_ReadBlockLen = NULL;
LPFN_AGL_ReadInBytes pAGL_ReadInBytes = NULL;
LPFN_AGL_ReadPInBytes pAGL_ReadPInBytes = NULL;
LPFN_AGL_ReadOutBytes pAGL_ReadOutBytes = NULL;
LPFN_AGL_ReadFlagBytes pAGL_ReadFlagBytes = NULL;
LPFN_AGL_ReadSFlagBytes pAGL_ReadSFlagBytes = NULL;
LPFN_AGL_ReadVarBytes pAGL_ReadVarBytes = NULL;
LPFN_AGL_ReadDataBytes pAGL_ReadDataBytes = NULL;
LPFN_AGL_ReadDataWords pAGL_ReadDataWords = NULL;
LPFN_AGL_ReadTimerWords pAGL_ReadTimerWords = NULL;
LPFN_AGL_ReadCounterWords pAGL_ReadCounterWords = NULL;
LPFN_AGL_ReadMix pAGL_ReadMix = NULL;
LPFN_AGL_ReadMixEx pAGL_ReadMixEx = NULL;
LPFN_AGL_WriteInBytes pAGL_WriteInBytes = NULL;
LPFN_AGL_WriteOutBytes pAGL_WriteOutBytes = NULL;
LPFN_AGL_WritePOutBytes pAGL_WritePOutBytes = NULL;
LPFN_AGL_WriteFlagBytes pAGL_WriteFlagBytes = NULL;
LPFN_AGL_WriteSFlagBytes pAGL_WriteSFlagBytes = NULL;
LPFN_AGL_WriteVarBytes pAGL_WriteVarBytes = NULL;
LPFN_AGL_WriteDataBytes pAGL_WriteDataBytes = NULL;
LPFN_AGL_WriteDataWords pAGL_WriteDataWords = NULL;
LPFN_AGL_WriteTimerWords pAGL_WriteTimerWords = NULL;
LPFN_AGL_WriteCounterWords pAGL_WriteCounterWords = NULL;
LPFN_AGL_WriteMix pAGL_WriteMix = NULL;
LPFN_AGL_WriteMixEx pAGL_WriteMixEx = NULL;
LPFN_AGL_InitOptReadMix pAGL_InitOptReadMix = NULL;
LPFN_AGL_ReadOptReadMix pAGL_ReadOptReadMix = NULL;
LPFN_AGL_EndOptReadMix pAGL_EndOptReadMix = NULL;
LPFN_AGL_InitOptReadMixEx pAGL_InitOptReadMixEx = NULL;
LPFN_AGL_ReadOptReadMixEx pAGL_ReadOptReadMixEx = NULL;
LPFN_AGL_EndOptReadMixEx pAGL_EndOptReadMixEx = NULL;
LPFN_AGL_InitOptWriteMix pAGL_InitOptWriteMix = NULL;
LPFN_AGL_WriteOptWriteMix pAGL_WriteOptWriteMix = NULL;
LPFN_AGL_EndOptWriteMix pAGL_EndOptWriteMix = NULL;
LPFN_AGL_InitOptWriteMixEx pAGL_InitOptWriteMixEx = NULL;
LPFN_AGL_WriteOptWriteMixEx pAGL_WriteOptWriteMixEx = NULL;
LPFN_AGL_EndOptWriteMixEx pAGL_EndOptWriteMixEx = NULL;
LPFN_AGL_SetOptNotification pAGL_SetOptNotification = NULL;
LPFN_AGL_DeleteOptJob pAGL_DeleteOptJob = NULL;
LPFN_AGL_GetOptJobResult pAGL_GetOptJobResult = NULL;
LPFN_AGL_WaitForOptJob pAGL_WaitForOptJob = NULL;
LPFN_AGL_AllocRWBuffs pAGL_AllocRWBuffs = NULL;
LPFN_AGL_FreeRWBuffs pAGL_FreeRWBuffs = NULL;
LPFN_AGL_ReadRWBuff pAGL_ReadRWBuff = NULL;
LPFN_AGL_WriteRWBuff pAGL_WriteRWBuff = NULL;
LPFN_AGL_RKSend pAGL_RKSend = NULL;
LPFN_AGL_RKSendEx pAGL_RKSendEx = NULL;
LPFN_AGL_RKFetch pAGL_RKFetch = NULL;
LPFN_AGL_RKFetchEx pAGL_RKFetchEx = NULL;
LPFN_AGL_Send_RKFetch pAGL_Send_RKFetch = NULL;
LPFN_AGL_Recv_RKSend pAGL_Recv_RKSend = NULL;
LPFN_AGL_Recv_RKFetch pAGL_Recv_RKFetch = NULL;
LPFN_AGL_Send_3964 pAGL_Send_3964 = NULL;
LPFN_AGL_Recv_3964 pAGL_Recv_3964 = NULL;
LPFN_AGL_BSend pAGL_BSend = NULL;
LPFN_AGL_BReceive pAGL_BReceive = NULL;
LPFN_AGL_BSendEx pAGL_BSendEx = NULL;
LPFN_AGL_BReceiveEx pAGL_BReceiveEx = NULL;
LPFN_AGL_USend pAGL_USend = NULL;
LPFN_AGL_UReceive pAGL_UReceive = NULL;
LPFN_AGL_InitOpStateMsg pAGL_InitOpStateMsg = NULL;
LPFN_AGL_ExitOpStateMsg pAGL_ExitOpStateMsg = NULL;
LPFN_AGL_GetOpStateMsg pAGL_GetOpStateMsg = NULL;
LPFN_AGL_InitDiagMsg pAGL_InitDiagMsg = NULL;
LPFN_AGL_ExitDiagMsg pAGL_ExitDiagMsg = NULL;
LPFN_AGL_GetDiagMsg pAGL_GetDiagMsg = NULL;
LPFN_AGL_InitCyclicRead pAGL_InitCyclicRead = NULL;
LPFN_AGL_InitCyclicReadEx pAGL_InitCyclicReadEx = NULL;
LPFN_AGL_StartCyclicRead pAGL_StartCyclicRead = NULL;
LPFN_AGL_StopCyclicRead pAGL_StopCyclicRead = NULL;
LPFN_AGL_ExitCyclicRead pAGL_ExitCyclicRead = NULL;
LPFN_AGL_GetCyclicRead pAGL_GetCyclicRead = NULL;
LPFN_AGL_GetCyclicReadEx pAGL_GetCyclicReadEx = NULL;
LPFN_AGL_InitScanMsg pAGL_InitScanMsg = NULL;
LPFN_AGL_ExitScanMsg pAGL_ExitScanMsg = NULL;
LPFN_AGL_GetScanMsg pAGL_GetScanMsg = NULL;
LPFN_AGL_HasAckTriggeredMsg pAGL_HasAckTriggeredMsg = NULL;
LPFN_AGL_InitAlarmMsg pAGL_InitAlarmMsg = NULL;
LPFN_AGL_ExitAlarmMsg pAGL_ExitAlarmMsg = NULL;
LPFN_AGL_GetAlarmMsg pAGL_GetAlarmMsg = NULL;
LPFN_AGL_ReadOpenMsg pAGL_ReadOpenMsg = NULL;
LPFN_AGL_GetMsgStateChange pAGL_GetMsgStateChange = NULL;
LPFN_AGL_AckMsg pAGL_AckMsg = NULL;
LPFN_AGL_LockMsg pAGL_LockMsg = NULL;
LPFN_AGL_UnlockMsg pAGL_UnlockMsg = NULL;
LPFN_AGL_InitARSend pAGL_InitARSend = NULL;
LPFN_AGL_ExitARSend pAGL_ExitARSend = NULL;
LPFN_AGL_GetARSend pAGL_GetARSend = NULL;
LPFN_AGL_RFC1006_Connect pAGL_RFC1006_Connect = NULL;
LPFN_AGL_RFC1006_Disconnect pAGL_RFC1006_Disconnect = NULL;
LPFN_AGL_RFC1006_Receive pAGL_RFC1006_Receive = NULL;
LPFN_AGL_RFC1006_Send pAGL_RFC1006_Send = NULL;
LPFN_AGL_NCK_ReadMixEx pAGL_NCK_ReadMixEx = NULL;
LPFN_AGL_NCK_WriteMixEx pAGL_NCK_WriteMixEx = NULL;
LPFN_AGL_NCK_CheckVarSize pAGL_NCK_CheckVarSize = NULL;
LPFN_AGL_NCK_InitCyclicReadEx pAGL_NCK_InitCyclicReadEx = NULL;
LPFN_AGL_NCK_StartCyclicRead pAGL_NCK_StartCyclicRead = NULL;
LPFN_AGL_NCK_StopCyclicRead pAGL_NCK_StopCyclicRead = NULL;
LPFN_AGL_NCK_ExitCyclicRead pAGL_NCK_ExitCyclicRead = NULL;
LPFN_AGL_NCK_GetCyclicReadEx pAGL_NCK_GetCyclicReadEx = NULL;
LPFN_AGL_NCK_PI_EXTERN pAGL_NCK_PI_EXTERN = NULL;
LPFN_AGL_NCK_PI_EXTMOD pAGL_NCK_PI_EXTMOD = NULL;
LPFN_AGL_NCK_PI_SELECT pAGL_NCK_PI_SELECT = NULL;
LPFN_AGL_NCK_PI_F_DELE pAGL_NCK_PI_F_DELE = NULL;
LPFN_AGL_NCK_PI_F_PROT pAGL_NCK_PI_F_PROT = NULL;
LPFN_AGL_NCK_PI_F_RENA pAGL_NCK_PI_F_RENA = NULL;
LPFN_AGL_NCK_PI_F_XFER pAGL_NCK_PI_F_XFER = NULL;
LPFN_AGL_NCK_PI_LOGIN pAGL_NCK_PI_LOGIN = NULL;
LPFN_AGL_NCK_PI_LOGOUT pAGL_NCK_PI_LOGOUT = NULL;
LPFN_AGL_NCK_PI_F_OPEN pAGL_NCK_PI_F_OPEN = NULL;
LPFN_AGL_NCK_PI_F_OPER pAGL_NCK_PI_F_OPER = NULL;
LPFN_AGL_NCK_PI_F_SEEK pAGL_NCK_PI_F_SEEK = NULL;
LPFN_AGL_NCK_PI_F_CLOS pAGL_NCK_PI_F_CLOS = NULL;
LPFN_AGL_NCK_PI_StartAll pAGL_NCK_PI_StartAll = NULL;
LPFN_AGL_NCK_PI_F_COPY pAGL_NCK_PI_F_COPY = NULL;
LPFN_AGL_NCK_PI_F_PROR pAGL_NCK_PI_F_PROR = NULL;
LPFN_AGL_NCK_PI_CANCEL pAGL_NCK_PI_CANCEL = NULL;
LPFN_AGL_NCK_PI_CRCEDN pAGL_NCK_PI_CRCEDN = NULL;
LPFN_AGL_NCK_PI_DELECE pAGL_NCK_PI_DELECE = NULL;
LPFN_AGL_NCK_PI_DELETO pAGL_NCK_PI_DELETO = NULL;
LPFN_AGL_NCK_PI_IBN_SS pAGL_NCK_PI_IBN_SS = NULL;
LPFN_AGL_NCK_PI_MMCSEM pAGL_NCK_PI_MMCSEM = NULL;
LPFN_AGL_NCK_PI_TMCRTO pAGL_NCK_PI_TMCRTO = NULL;
LPFN_AGL_NCK_PI_TMMVTL pAGL_NCK_PI_TMMVTL = NULL;
LPFN_AGL_NCK_PI_TMCRTC pAGL_NCK_PI_TMCRTC = NULL;
LPFN_AGL_NCK_PI_CREATO pAGL_NCK_PI_CREATO = NULL;
LPFN_AGL_NCK_CopyFileToNC pAGL_NCK_CopyFileToNC = NULL;
LPFN_AGL_NCK_CopyFileFromNC pAGL_NCK_CopyFileFromNC = NULL;
LPFN_AGL_NCK_CopyToNC pAGL_NCK_CopyToNC = NULL;
LPFN_AGL_NCK_CopyFromNC pAGL_NCK_CopyFromNC = NULL;
LPFN_AGL_NCK_CopyFromNCAlloc pAGL_NCK_CopyFromNCAlloc = NULL;
LPFN_AGL_NCK_FreeBuff pAGL_NCK_FreeBuff = NULL;
LPFN_AGL_NCK_SetConnProgressNotification pAGL_NCK_SetConnProgressNotification = NULL;
LPFN_AGL_NCK_CheckNSKVarLine pAGL_NCK_CheckNSKVarLine = NULL;
LPFN_AGL_NCK_ReadNSKVarFile pAGL_NCK_ReadNSKVarFile = NULL;
LPFN_AGL_NCK_CheckCSVVarLine pAGL_NCK_CheckCSVVarLine = NULL;
LPFN_AGL_NCK_ReadCSVVarFile pAGL_NCK_ReadCSVVarFile = NULL;
LPFN_AGL_NCK_ReadGUDVarFile pAGL_NCK_ReadGUDVarFile = NULL;
LPFN_AGL_NCK_ReadGUDVarFileEx pAGL_NCK_ReadGUDVarFileEx = NULL;
LPFN_AGL_NCK_FreeVarBuff pAGL_NCK_FreeVarBuff = NULL;
LPFN_AGL_NCK_GetSingleVarDef pAGL_NCK_GetSingleVarDef = NULL;
LPFN_AGL_NCK_ExtractNckAlarm pAGL_NCK_ExtractNckAlarm = NULL;
LPFN_AGL_NCK_GetNCKDataRWByNCDDEItem pAGL_NCK_GetNCKDataRWByNCDDEItem = NULL;
LPFN_AGL_Drive_ReadMix pAGL_Drive_ReadMix = NULL;
LPFN_AGL_Drive_ReadMixEx pAGL_Drive_ReadMixEx = NULL;
LPFN_AGL_Drive_WriteMix pAGL_Drive_WriteMix = NULL;
LPFN_AGL_Drive_WriteMixEx pAGL_Drive_WriteMixEx = NULL;
LPFN_AGL_malloc pAGL_malloc = NULL;
LPFN_AGL_calloc pAGL_calloc = NULL;
LPFN_AGL_realloc pAGL_realloc = NULL;
LPFN_AGL_memcpy pAGL_memcpy = NULL;
LPFN_AGL_memmove pAGL_memmove = NULL;
LPFN_AGL_memcmp pAGL_memcmp = NULL;
LPFN_AGL_free pAGL_free = NULL;
LPFN_AGL_ReadInt16 pAGL_ReadInt16 = NULL;
LPFN_AGL_ReadInt32 pAGL_ReadInt32 = NULL;
LPFN_AGL_ReadWord pAGL_ReadWord = NULL;
LPFN_AGL_ReadDWord pAGL_ReadDWord = NULL;
LPFN_AGL_ReadReal pAGL_ReadReal = NULL;
LPFN_AGL_ReadS5Time pAGL_ReadS5Time = NULL;
LPFN_AGL_ReadS5TimeW pAGL_ReadS5TimeW = NULL;
LPFN_AGL_WriteInt16 pAGL_WriteInt16 = NULL;
LPFN_AGL_WriteInt32 pAGL_WriteInt32 = NULL;
LPFN_AGL_WriteWord pAGL_WriteWord = NULL;
LPFN_AGL_WriteDWord pAGL_WriteDWord = NULL;
LPFN_AGL_WriteReal pAGL_WriteReal = NULL;
LPFN_AGL_WriteS5Time pAGL_WriteS5Time = NULL;
LPFN_AGL_WriteS5TimeW pAGL_WriteS5TimeW = NULL;
LPFN_AGL_Byte2Word pAGL_Byte2Word = NULL;
LPFN_AGL_Byte2DWord pAGL_Byte2DWord = NULL;
LPFN_AGL_Byte2Real pAGL_Byte2Real = NULL;
LPFN_AGL_Word2Byte pAGL_Word2Byte = NULL;
LPFN_AGL_DWord2Byte pAGL_DWord2Byte = NULL;
LPFN_AGL_Real2Byte pAGL_Real2Byte = NULL;
LPFN_AGL_GetBit pAGL_GetBit = NULL;
LPFN_AGL_SetBit pAGL_SetBit = NULL;
LPFN_AGL_ResetBit pAGL_ResetBit = NULL;
LPFN_AGL_SetBitVal pAGL_SetBitVal = NULL;
LPFN_AGL_Buff2String pAGL_Buff2String = NULL;
LPFN_AGL_String2Buff pAGL_String2Buff = NULL;
LPFN_AGL_Buff2WString pAGL_Buff2WString = NULL;
LPFN_AGL_WString2Buff pAGL_WString2Buff = NULL;
LPFN_AGL_S7String2String pAGL_S7String2String = NULL;
LPFN_AGL_String2S7String pAGL_String2S7String = NULL;
LPFN_AGL_BCD2Int16 pAGL_BCD2Int16 = NULL;
LPFN_AGL_BCD2Int32 pAGL_BCD2Int32 = NULL;
LPFN_AGL_Int162BCD pAGL_Int162BCD = NULL;
LPFN_AGL_Int322BCD pAGL_Int322BCD = NULL;
LPFN_AGL_LongAsFloat pAGL_LongAsFloat = NULL;
LPFN_AGL_FloatAsLong pAGL_FloatAsLong = NULL;
LPFN_AGL_Text2DataRW pAGL_Text2DataRW = NULL;
LPFN_AGL_DataRW2Text pAGL_DataRW2Text = NULL;
LPFN_AGL_S7DT2SysTime pAGL_S7DT2SysTime = NULL;
LPFN_AGL_SysTime2S7DT pAGL_SysTime2S7DT = NULL;
LPFN_AGL_TOD2SysTime pAGL_TOD2SysTime = NULL;
LPFN_AGL_SysTime2TOD pAGL_SysTime2TOD = NULL;
LPFN_AGL_S7Date2YMD pAGL_S7Date2YMD = NULL;
LPFN_AGL_Float2KG pAGL_Float2KG = NULL;
LPFN_AGL_KG2Float pAGL_KG2Float = NULL;
LPFN_AGL_Float2DWKG pAGL_Float2DWKG = NULL;
LPFN_AGL_DWKG2Float pAGL_DWKG2Float = NULL;
LPFN_AGL_S7Ident2String pAGL_S7Ident2String = NULL;
LPFN_AGLSym_OpenProject pAGLSym_OpenProject = NULL;
LPFN_AGLSym_CloseProject pAGLSym_CloseProject = NULL;
LPFN_AGLSym_WriteCpuListToFile pAGLSym_WriteCpuListToFile = NULL;
LPFN_AGLSym_GetProgramCount pAGLSym_GetProgramCount = NULL;
LPFN_AGLSym_FindFirstProgram pAGLSym_FindFirstProgram = NULL;
LPFN_AGLSym_FindNextProgram pAGLSym_FindNextProgram = NULL;
LPFN_AGLSym_FindCloseProgram pAGLSym_FindCloseProgram = NULL;
LPFN_AGLSym_SelectProgram pAGLSym_SelectProgram = NULL;
LPFN_AGLSym_GetSymbolCount pAGLSym_GetSymbolCount = NULL;
LPFN_AGLSym_GetSymbolCountFilter pAGLSym_GetSymbolCountFilter = NULL;
LPFN_AGLSym_FindFirstSymbol pAGLSym_FindFirstSymbol = NULL;
LPFN_AGLSym_FindFirstSymbolFilter pAGLSym_FindFirstSymbolFilter = NULL;
LPFN_AGLSym_FindNextSymbol pAGLSym_FindNextSymbol = NULL;
LPFN_AGLSym_FindCloseSymbol pAGLSym_FindCloseSymbol = NULL;
LPFN_AGLSym_ReadPrjDBCount pAGLSym_ReadPrjDBCount = NULL;
LPFN_AGLSym_ReadPrjDBCountFilter pAGLSym_ReadPrjDBCountFilter = NULL;
LPFN_AGLSym_ReadPrjDBList pAGLSym_ReadPrjDBList = NULL;
LPFN_AGLSym_ReadPrjDBListFilter pAGLSym_ReadPrjDBListFilter = NULL;
LPFN_AGLSym_ReadPrjBlkCountFilter pAGLSym_ReadPrjBlkCountFilter = NULL;
LPFN_AGLSym_ReadPrjBlkListFilter pAGLSym_ReadPrjBlkListFilter = NULL;
LPFN_AGLSym_GetDbSymbolCount pAGLSym_GetDbSymbolCount = NULL;
LPFN_AGLSym_GetDbSymbolCountFilter pAGLSym_GetDbSymbolCountFilter = NULL;
LPFN_AGLSym_FindFirstDbSymbol pAGLSym_FindFirstDbSymbol = NULL;
LPFN_AGLSym_FindFirstDbSymbolFilter pAGLSym_FindFirstDbSymbolFilter = NULL;
LPFN_AGLSym_FindNextDbSymbol pAGLSym_FindNextDbSymbol = NULL;
LPFN_AGLSym_FindFirstDbSymbolEx pAGLSym_FindFirstDbSymbolEx = NULL;
LPFN_AGLSym_FindNextDbSymbolEx pAGLSym_FindNextDbSymbolEx = NULL;
LPFN_AGLSym_GetDbSymbolExComment pAGLSym_GetDbSymbolExComment = NULL;
LPFN_AGLSym_FindCloseDbSymbol pAGLSym_FindCloseDbSymbol = NULL;
LPFN_AGLSym_GetDbDependency pAGLSym_GetDbDependency = NULL;
LPFN_AGLSym_GetDeclarationCountFilter pAGLSym_GetDeclarationCountFilter = NULL;
LPFN_AGLSym_FindFirstDeclarationFilter pAGLSym_FindFirstDeclarationFilter = NULL;
LPFN_AGLSym_FindNextDeclaration pAGLSym_FindNextDeclaration = NULL;
LPFN_AGLSym_GetDeclarationInitialValue pAGLSym_GetDeclarationInitialValue = NULL;
LPFN_AGLSym_FindCloseDeclaration pAGLSym_FindCloseDeclaration = NULL;
LPFN_AGLSym_GetSymbolFromText pAGLSym_GetSymbolFromText = NULL;
LPFN_AGLSym_GetSymbolFromTextEx pAGLSym_GetSymbolFromTextEx = NULL;
LPFN_AGLSym_GetReadMixFromText pAGLSym_GetReadMixFromText = NULL;
LPFN_AGLSym_GetReadMixFromTextEx pAGLSym_GetReadMixFromTextEx = NULL;
LPFN_AGLSym_GetSymbol pAGLSym_GetSymbol = NULL;
LPFN_AGLSym_GetSymbolEx pAGLSym_GetSymbolEx = NULL;
LPFN_AGLSym_OpenAlarms pAGLSym_OpenAlarms = NULL;
LPFN_AGLSym_CloseAlarms pAGLSym_CloseAlarms = NULL;
LPFN_AGLSym_FindFirstAlarmData pAGLSym_FindFirstAlarmData = NULL;
LPFN_AGLSym_FindNextAlarmData pAGLSym_FindNextAlarmData = NULL;
LPFN_AGLSym_FindCloseAlarmData pAGLSym_FindCloseAlarmData = NULL;
LPFN_AGLSym_GetAlarmData pAGLSym_GetAlarmData = NULL;
LPFN_AGLSym_GetAlarmName pAGLSym_GetAlarmName = NULL;
LPFN_AGLSym_GetAlarmType pAGLSym_GetAlarmType = NULL;
LPFN_AGLSym_GetAlarmBaseName pAGLSym_GetAlarmBaseName = NULL;
LPFN_AGLSym_GetAlarmTypeName pAGLSym_GetAlarmTypeName = NULL;
LPFN_AGLSym_GetAlarmSignalCount pAGLSym_GetAlarmSignalCount = NULL;
LPFN_AGLSym_GetAlarmMsgClass pAGLSym_GetAlarmMsgClass = NULL;
LPFN_AGLSym_GetAlarmPriority pAGLSym_GetAlarmPriority = NULL;
LPFN_AGLSym_GetAlarmAckGroup pAGLSym_GetAlarmAckGroup = NULL;
LPFN_AGLSym_GetAlarmAcknowledge pAGLSym_GetAlarmAcknowledge = NULL;
LPFN_AGLSym_GetAlarmProtocol pAGLSym_GetAlarmProtocol = NULL;
LPFN_AGLSym_GetAlarmDispGroup pAGLSym_GetAlarmDispGroup = NULL;
LPFN_AGLSym_FindFirstAlarmTextLanguage pAGLSym_FindFirstAlarmTextLanguage = NULL;
LPFN_AGLSym_FindNextAlarmTextLanguage pAGLSym_FindNextAlarmTextLanguage = NULL;
LPFN_AGLSym_FindCloseAlarmTextLanguage pAGLSym_FindCloseAlarmTextLanguage = NULL;
LPFN_AGLSym_SetAlarmTextDefaultLanguage pAGLSym_SetAlarmTextDefaultLanguage = NULL;
LPFN_AGLSym_GetAlarmText pAGLSym_GetAlarmText = NULL;
LPFN_AGLSym_GetAlarmInfo pAGLSym_GetAlarmInfo = NULL;
LPFN_AGLSym_GetAlarmAddText pAGLSym_GetAlarmAddText = NULL;
LPFN_AGLSym_GetAlarmSCANOperand pAGLSym_GetAlarmSCANOperand = NULL;
LPFN_AGLSym_GetAlarmSCANInterval pAGLSym_GetAlarmSCANInterval = NULL;
LPFN_AGLSym_GetAlarmSCANAddValue pAGLSym_GetAlarmSCANAddValue = NULL;
LPFN_AGLSym_GetAlarmSCANOperandEx pAGLSym_GetAlarmSCANOperandEx = NULL;
LPFN_AGLSym_GetAlarmSCANAddValueEx pAGLSym_GetAlarmSCANAddValueEx = NULL;
LPFN_AGLSym_FormatMessage pAGLSym_FormatMessage = NULL;
LPFN_AGLSym_FindFirstTextlib pAGLSym_FindFirstTextlib = NULL;
LPFN_AGLSym_FindNextTextlib pAGLSym_FindNextTextlib = NULL;
LPFN_AGLSym_FindCloseTextlib pAGLSym_FindCloseTextlib = NULL;
LPFN_AGLSym_SelectTextlib pAGLSym_SelectTextlib = NULL;
LPFN_AGLSym_FindFirstTextlibText pAGLSym_FindFirstTextlibText = NULL;
LPFN_AGLSym_FindNextTextlibText pAGLSym_FindNextTextlibText = NULL;
LPFN_AGLSym_FindCloseTextlibText pAGLSym_FindCloseTextlibText = NULL;
LPFN_AGLSym_GetTextlibText pAGLSym_GetTextlibText = NULL;
LPFN_AGLSym_GetTextFromValue pAGLSym_GetTextFromValue = NULL;
LPFN_AGLSym_GetValueFromText pAGLSym_GetValueFromText = NULL;
LPFN_AGLSym_GetRealFromText pAGLSym_GetRealFromText = NULL;
LPFN_AGLSym_GetTextFromReal pAGLSym_GetTextFromReal = NULL;
LPFN_AGLSym_SetLanguage pAGLSym_SetLanguage = NULL;
LPFN_AGL_WLD_OpenFile pAGL_WLD_OpenFile = NULL;
LPFN_AGL_WLD_OpenFileEncrypted pAGL_WLD_OpenFileEncrypted = NULL;
LPFN_AGL_WLD_EncryptFile pAGL_WLD_EncryptFile = NULL;
LPFN_AGL_WLD_DecryptFile pAGL_WLD_DecryptFile = NULL;
LPFN_AGL_WLD_CloseFile pAGL_WLD_CloseFile = NULL;
LPFN_AGL_WLD_ReadAllBlockCount pAGL_WLD_ReadAllBlockCount = NULL;
LPFN_AGL_WLD_ReadBlockCount pAGL_WLD_ReadBlockCount = NULL;
LPFN_AGL_WLD_ReadBlockList pAGL_WLD_ReadBlockList = NULL;
LPFN_AGL_WLD_ReadBlockLen pAGL_WLD_ReadBlockLen = NULL;
LPFN_AGL_WLD_DeleteBlocks pAGL_WLD_DeleteBlocks = NULL;
LPFN_AGL_WLD_GetReport pAGL_WLD_GetReport = NULL;
LPFN_AGL_PLC_Backup pAGL_PLC_Backup = NULL;
LPFN_AGL_PLC_Restore pAGL_PLC_Restore = NULL;
LPFN_AGL_PLC_DeleteBlocks pAGL_PLC_DeleteBlocks = NULL;
LPFN_AGL_Compress pAGL_Compress = NULL;
LPFN_AGL_Symbolic_ReadMixEx pAGL_Symbolic_ReadMixEx = NULL;
LPFN_AGL_Symbolic_WriteMixEx pAGL_Symbolic_WriteMixEx = NULL;
LPFN_AGL_Symbolic_LoadTIAProjectSymbols pAGL_Symbolic_LoadTIAProjectSymbols = NULL;
LPFN_AGL_Symbolic_LoadAGLinkSymbolsFromPLC pAGL_Symbolic_LoadAGLinkSymbolsFromPLC = NULL;
LPFN_AGL_Symbolic_SaveAGLinkSymbolsToFile pAGL_Symbolic_SaveAGLinkSymbolsToFile = NULL;
LPFN_AGL_Symbolic_LoadAGLinkSymbolsFromFile pAGL_Symbolic_LoadAGLinkSymbolsFromFile = NULL;
LPFN_AGL_Symbolic_FreeHandle pAGL_Symbolic_FreeHandle = NULL;
LPFN_AGL_Symbolic_GetChildCount pAGL_Symbolic_GetChildCount = NULL;
LPFN_AGL_Symbolic_GetChild pAGL_Symbolic_GetChild = NULL;
LPFN_AGL_Symbolic_GetChildByName pAGL_Symbolic_GetChildByName = NULL;
LPFN_AGL_Symbolic_GetName pAGL_Symbolic_GetName = NULL;
LPFN_AGL_Symbolic_GetLocalOffset pAGL_Symbolic_GetLocalOffset = NULL;
LPFN_AGL_Symbolic_GetSystemType pAGL_Symbolic_GetSystemType = NULL;
LPFN_AGL_Symbolic_GetHierarchyType pAGL_Symbolic_GetHierarchyType = NULL;
LPFN_AGL_Symbolic_GetArrayDimensionCount pAGL_Symbolic_GetArrayDimensionCount = NULL;
LPFN_AGL_Symbolic_GetArrayDimension pAGL_Symbolic_GetArrayDimension = NULL;
LPFN_AGL_Symbolic_GetMaxStringSize pAGL_Symbolic_GetMaxStringSize = NULL;
LPFN_AGL_Symbolic_GetValueType pAGL_Symbolic_GetValueType = NULL;
LPFN_AGL_Symbolic_GetTypeState pAGL_Symbolic_GetTypeState = NULL;
LPFN_AGL_Symbolic_GetSegmentType pAGL_Symbolic_GetSegmentType = NULL;
LPFN_AGL_Symbolic_GetPermissionType pAGL_Symbolic_GetPermissionType = NULL;
LPFN_AGL_Symbolic_EscapeString pAGL_Symbolic_EscapeString = NULL;
LPFN_AGL_Symbolic_GetNodeByPath pAGL_Symbolic_GetNodeByPath = NULL;
LPFN_AGL_Symbolic_GetIndexSize pAGL_Symbolic_GetIndexSize = NULL;
LPFN_AGL_Symbolic_GetIndex pAGL_Symbolic_GetIndex = NULL;
LPFN_AGL_Symbolic_GetLinearIndex pAGL_Symbolic_GetLinearIndex = NULL;
LPFN_AGL_Symbolic_GetArrayElementCount pAGL_Symbolic_GetArrayElementCount = NULL;
LPFN_AGL_Symbolic_Expand pAGL_Symbolic_Expand = NULL;
LPFN_AGL_Symbolic_Collapse pAGL_Symbolic_Collapse = NULL;
LPFN_AGL_Symbolic_GetSystemScope pAGL_Symbolic_GetSystemScope = NULL;
LPFN_AGL_Symbolic_GetSystemTypeState pAGL_Symbolic_GetSystemTypeState = NULL;
LPFN_AGL_Symbolic_CreateAccess pAGL_Symbolic_CreateAccess = NULL;
LPFN_AGL_Symbolic_CreateAccessByPath pAGL_Symbolic_CreateAccessByPath = NULL;
LPFN_AGL_Symbolic_Get_DATA_RW40 pAGL_Symbolic_Get_DATA_RW40 = NULL;
LPFN_AGL_Symbolic_Get_DATA_RW40_ByPath pAGL_Symbolic_Get_DATA_RW40_ByPath = NULL;
LPFN_AGL_Symbolic_GetType pAGL_Symbolic_GetType = NULL;
LPFN_AGL_Symbolic_GetAccessBufferSize pAGL_Symbolic_GetAccessBufferSize = NULL;
LPFN_AGL_Symbolic_GetAccessBufferElementSize pAGL_Symbolic_GetAccessBufferElementSize = NULL;
LPFN_AGL_Symbolic_GetAccessStringSize pAGL_Symbolic_GetAccessStringSize = NULL;
LPFN_AGL_Symbolic_GetAccessBufferElementCount pAGL_Symbolic_GetAccessBufferElementCount = NULL;
LPFN_AGL_Symbolic_GetAccessElementSystemType pAGL_Symbolic_GetAccessElementSystemType = NULL;
LPFN_AGL_Symbolic_GetAccessElementValueType pAGL_Symbolic_GetAccessElementValueType = NULL;
LPFN_AGL_Symbolic_GetArrayIndexAsLinearIndex pAGL_Symbolic_GetArrayIndexAsLinearIndex = NULL;
LPFN_AGL_Symbolic_GetArrayLinearIndexAsIndex pAGL_Symbolic_GetArrayLinearIndexAsIndex = NULL;
LPFN_AGL_Symbolic_CreateArrayAccessByLinearIndex pAGL_Symbolic_CreateArrayAccessByLinearIndex = NULL;
LPFN_AGL_Symbolic_CreateArrayRangeAccessByLinearIndex pAGL_Symbolic_CreateArrayRangeAccessByLinearIndex = NULL;
LPFN_AGL_Symbolic_CreateArrayAccessByIndex pAGL_Symbolic_CreateArrayAccessByIndex = NULL;
LPFN_AGL_Symbolic_CreateArrayRangeAccessByIndex pAGL_Symbolic_CreateArrayRangeAccessByIndex = NULL;
LPFN_AGL_Symbolic_GetAccessBufferUInt8 pAGL_Symbolic_GetAccessBufferUInt8 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferUInt16 pAGL_Symbolic_GetAccessBufferUInt16 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferUInt32 pAGL_Symbolic_GetAccessBufferUInt32 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferUInt64 pAGL_Symbolic_GetAccessBufferUInt64 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferInt8 pAGL_Symbolic_GetAccessBufferInt8 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferInt16 pAGL_Symbolic_GetAccessBufferInt16 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferInt32 pAGL_Symbolic_GetAccessBufferInt32 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferInt64 pAGL_Symbolic_GetAccessBufferInt64 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferFloat32 pAGL_Symbolic_GetAccessBufferFloat32 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferFloat64 pAGL_Symbolic_GetAccessBufferFloat64 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferChar8 pAGL_Symbolic_GetAccessBufferChar8 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferChar16 pAGL_Symbolic_GetAccessBufferChar16 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferString8 pAGL_Symbolic_GetAccessBufferString8 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferString16 pAGL_Symbolic_GetAccessBufferString16 = NULL;
LPFN_AGL_Symbolic_GetAccessBufferS7_DTLParts pAGL_Symbolic_GetAccessBufferS7_DTLParts = NULL;
LPFN_AGL_Symbolic_GetAccessBufferS7_S5TimeParts pAGL_Symbolic_GetAccessBufferS7_S5TimeParts = NULL;
LPFN_AGL_Symbolic_GetAccessBufferS7_S5TimeMs pAGL_Symbolic_GetAccessBufferS7_S5TimeMs = NULL;
LPFN_AGL_Symbolic_GetAccessBufferS7_Counter pAGL_Symbolic_GetAccessBufferS7_Counter = NULL;
LPFN_AGL_Symbolic_GetAccessBufferS7_Date_and_TimeParts pAGL_Symbolic_GetAccessBufferS7_Date_and_TimeParts = NULL;
LPFN_AGL_Symbolic_SetAccessBufferUInt8 pAGL_Symbolic_SetAccessBufferUInt8 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferUInt16 pAGL_Symbolic_SetAccessBufferUInt16 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferUInt32 pAGL_Symbolic_SetAccessBufferUInt32 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferUInt64 pAGL_Symbolic_SetAccessBufferUInt64 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferInt8 pAGL_Symbolic_SetAccessBufferInt8 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferInt16 pAGL_Symbolic_SetAccessBufferInt16 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferInt32 pAGL_Symbolic_SetAccessBufferInt32 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferInt64 pAGL_Symbolic_SetAccessBufferInt64 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferFloat32 pAGL_Symbolic_SetAccessBufferFloat32 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferFloat64 pAGL_Symbolic_SetAccessBufferFloat64 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferChar8 pAGL_Symbolic_SetAccessBufferChar8 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferChar16 pAGL_Symbolic_SetAccessBufferChar16 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferString8 pAGL_Symbolic_SetAccessBufferString8 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferString16 pAGL_Symbolic_SetAccessBufferString16 = NULL;
LPFN_AGL_Symbolic_SetAccessBufferS7_DTLParts pAGL_Symbolic_SetAccessBufferS7_DTLParts = NULL;
LPFN_AGL_Symbolic_SetAccessBufferS7_S5TimeParts pAGL_Symbolic_SetAccessBufferS7_S5TimeParts = NULL;
LPFN_AGL_Symbolic_SetAccessBufferS7_S5TimeMs pAGL_Symbolic_SetAccessBufferS7_S5TimeMs = NULL;
LPFN_AGL_Symbolic_SetAccessBufferS7_Counter pAGL_Symbolic_SetAccessBufferS7_Counter = NULL;
LPFN_AGL_Symbolic_SetAccessBufferS7_Date_and_TimeParts pAGL_Symbolic_SetAccessBufferS7_Date_and_TimeParts = NULL;
LPFN_AGL_Symbolic_GetProjectEditingCulture pAGL_Symbolic_GetProjectEditingCulture = NULL;
LPFN_AGL_Symbolic_GetProjectReferenceCulture pAGL_Symbolic_GetProjectReferenceCulture = NULL;
LPFN_AGL_Symbolic_GetProjectCultureCount pAGL_Symbolic_GetProjectCultureCount = NULL;
LPFN_AGL_Symbolic_GetProjectCulture pAGL_Symbolic_GetProjectCulture = NULL;
LPFN_AGL_Symbolic_GetComment pAGL_Symbolic_GetComment = NULL;
LPFN_AGL_Symbolic_GetCommentCultureCount pAGL_Symbolic_GetCommentCultureCount = NULL;
LPFN_AGL_Symbolic_GetCommentCulture pAGL_Symbolic_GetCommentCulture = NULL;
LPFN_AGL_Symbolic_DatablockGetNumber pAGL_Symbolic_DatablockGetNumber = NULL;
LPFN_AGL_Symbolic_GetPath pAGL_Symbolic_GetPath = NULL;
LPFN_AGL_Symbolic_GetEscapedPath pAGL_Symbolic_GetEscapedPath = NULL;
LPFN_AGL_Symbolic_GetAttributeHMIAccessible pAGL_Symbolic_GetAttributeHMIAccessible = NULL;
LPFN_AGL_Symbolic_GetAttributeHMIVisible pAGL_Symbolic_GetAttributeHMIVisible = NULL;
LPFN_AGL_Symbolic_GetAttributeRemanent pAGL_Symbolic_GetAttributeRemanent = NULL;
LPFN_AGL_Symbolic_GetS7PlcTypeName pAGL_Symbolic_GetS7PlcTypeName = NULL;
LPFN_AGL_Symbolic_GetS7PlcFirmware pAGL_Symbolic_GetS7PlcFirmware = NULL;
LPFN_AGL_Symbolic_GetS7PlcMLFB pAGL_Symbolic_GetS7PlcMLFB = NULL;
LPFN_AGL_Symbolic_GetS7PlcFamily pAGL_Symbolic_GetS7PlcFamily = NULL;
LPFN_AGL_Symbolic_SaveSingleValueAccessSymbolsToFile pAGL_Symbolic_SaveSingleValueAccessSymbolsToFile = NULL;
LPFN_AGL_Symbolic_LoadSingleValueAccessSymbolsFromFile pAGL_Symbolic_LoadSingleValueAccessSymbolsFromFile = NULL;
LPFN_AGL_Symbolic_CreateAccessFromSingleValueAccessSymbols pAGL_Symbolic_CreateAccessFromSingleValueAccessSymbols = NULL;
LPFN_AGL_Simotion_LoadSTISymbols pAGL_Simotion_LoadSTISymbols = NULL;
LPFN_AGL_Simotion_FreeHandle pAGL_Simotion_FreeHandle = NULL;
LPFN_AGL_Simotion_GetName pAGL_Simotion_GetName = NULL;
LPFN_AGL_Simotion_GetHierarchyType pAGL_Simotion_GetHierarchyType = NULL;
LPFN_AGL_Simotion_GetValueType pAGL_Simotion_GetValueType = NULL;
LPFN_AGL_Simotion_GetPermissionType pAGL_Simotion_GetPermissionType = NULL;
LPFN_AGL_Simotion_GetChildCount pAGL_Simotion_GetChildCount = NULL;
LPFN_AGL_Simotion_GetChild pAGL_Simotion_GetChild = NULL;
LPFN_AGL_Simotion_GetNodeByPath pAGL_Simotion_GetNodeByPath = NULL;
LPFN_AGL_Simotion_CreateAccess pAGL_Simotion_CreateAccess = NULL;
LPFN_AGL_Simotion_CreateAccessByPath pAGL_Simotion_CreateAccessByPath = NULL;
LPFN_AGL_Simotion_ReadMixEx pAGL_Simotion_ReadMixEx = NULL;
LPFN_AGL_Simotion_WriteMixEx pAGL_Simotion_WriteMixEx = NULL;

/*******************************************************************************
Definition der Funktionen
Die definierten Funktionen werden auf die Funktionen der Bibliothek AGLink40.DLL weitergeleitet.
*******************************************************************************/

void AGL_API AGL_Activate(agl_cstr8_t Key)
{
  if( Loaded(pAGL_Activate))
  {
    pAGL_Activate(Key);
  }
}

void AGL_API AGL_GetVersion(agl_int32_t* Major, agl_int32_t* Minor)
{
  if( Loaded(pAGL_GetVersion))
  {
    pAGL_GetVersion(Major, Minor);
  }
}

void AGL_API AGL_GetVersionEx(agl_int32_t* Major, agl_int32_t* Minor, agl_int32_t* Build, agl_int32_t* Revision, agl_cstr8_t Date)
{
  if( Loaded(pAGL_GetVersionEx))
  {
    pAGL_GetVersionEx(Major, Minor, Build, Revision, Date);
  }
}

agl_int32_t AGL_API AGL_GetOptions(void)
{
  if( Loaded(pAGL_GetOptions))
  {
    return pAGL_GetOptions();
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetSerialNumber(void)
{
  if( Loaded(pAGL_GetSerialNumber))
  {
    return pAGL_GetSerialNumber();
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_cstr8_t AGL_API AGL_GetClientName(agl_cstr8_t Name)
{
  if( Loaded(pAGL_GetClientName))
  {
    return pAGL_GetClientName(Name);
  }
  return Name;
}

agl_int32_t AGL_API AGL_GetMaxDevices(void)
{
  if( Loaded(pAGL_GetMaxDevices))
  {
    return pAGL_GetMaxDevices();
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetMaxQueues(void)
{
  if( Loaded(pAGL_GetMaxQueues))
  {
    return pAGL_GetMaxQueues();
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetMaxPLCPerDevice(void)
{
  if( Loaded(pAGL_GetMaxPLCPerDevice))
  {
    return pAGL_GetMaxPLCPerDevice();
  }
  return AGL40_DYN_DLL_ERROR;
}

void AGL_API AGL_UseSystemTime(agl_bool_t Flag)
{
  if( Loaded(pAGL_UseSystemTime))
  {
    pAGL_UseSystemTime(Flag);
  }
}

void AGL_API AGL_ReturnJobNr(agl_bool_t Flag)
{
  if( Loaded(pAGL_ReturnJobNr))
  {
    pAGL_ReturnJobNr(Flag);
  }
}

void AGL_API AGL_SetBSendAutoResponse(agl_bool_t Flag)
{
  if( Loaded(pAGL_SetBSendAutoResponse))
  {
    pAGL_SetBSendAutoResponse(Flag);
  }
}

agl_bool_t AGL_API AGL_GetBSendAutoResponse(void)
{
  if( Loaded(pAGL_GetBSendAutoResponse))
  {
    return pAGL_GetBSendAutoResponse();
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetPCCPConnNames(agl_cstr8_t Names, agl_int32_t Len)
{
  if( Loaded(pAGL_GetPCCPConnNames))
  {
    return pAGL_GetPCCPConnNames(Names, Len);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetPCCPProtocol(agl_cstr8_t Name)
{
  if( Loaded(pAGL_GetPCCPProtocol))
  {
    return pAGL_GetPCCPProtocol(Name);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetTapiModemNames(agl_cstr8_t Names, agl_int32_t Len)
{
  if( Loaded(pAGL_GetTapiModemNames))
  {
    return pAGL_GetTapiModemNames(Names, Len);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetLocalIPAddresses(agl_ulong32_t* Addresses, agl_ulong32_t NumAddresses)
{
  if( Loaded(pAGL_GetLocalIPAddresses))
  {
    return pAGL_GetLocalIPAddresses(Addresses, NumAddresses);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_ulong32_t AGL_API AGL_GetTickCount(void)
{
  if( Loaded(pAGL_GetTickCount))
  {
    return pAGL_GetTickCount();
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_ulong32_t AGL_API AGL_GetMicroSecs(void)
{
  if( Loaded(pAGL_GetMicroSecs))
  {
    return pAGL_GetMicroSecs();
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Config(agl_int32_t DevNr)
{
  if( Loaded(pAGL_Config))
  {
    return pAGL_Config(DevNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ConfigEx(agl_int32_t DevNr, agl_cstr8_t CmdLine)
{
  if( Loaded(pAGL_ConfigEx))
  {
    return pAGL_ConfigEx(DevNr, CmdLine);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SetParas(agl_int32_t DevNr, agl_int32_t ParaType, void* Para, agl_int32_t Len)
{
  if( Loaded(pAGL_SetParas))
  {
    return pAGL_SetParas(DevNr, ParaType, Para, Len);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetParas(agl_int32_t DevNr, agl_int32_t ParaType, void* Para, agl_int32_t Len)
{
  if( Loaded(pAGL_GetParas))
  {
    return pAGL_GetParas(DevNr, ParaType, Para, Len);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SetDevType(agl_int32_t DevNr, agl_int32_t DevType)
{
  if( Loaded(pAGL_SetDevType))
  {
    return pAGL_SetDevType(DevNr, DevType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetDevType(agl_int32_t DevNr)
{
  if( Loaded(pAGL_GetDevType))
  {
    return pAGL_GetDevType(DevNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadParas(agl_int32_t DevNr, agl_int32_t ParaType)
{
  if( Loaded(pAGL_ReadParas))
  {
    return pAGL_ReadParas(DevNr, ParaType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteParas(agl_int32_t DevNr, agl_int32_t ParaType)
{
  if( Loaded(pAGL_WriteParas))
  {
    return pAGL_WriteParas(DevNr, ParaType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadDevice(agl_int32_t DevNr)
{
  if( Loaded(pAGL_ReadDevice))
  {
    return pAGL_ReadDevice(DevNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteDevice(agl_int32_t DevNr)
{
  if( Loaded(pAGL_WriteDevice))
  {
    return pAGL_WriteDevice(DevNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadParasFromFile(agl_int32_t DevNr, agl_cstr8_t FileName)
{
  if( Loaded(pAGL_ReadParasFromFile))
  {
    return pAGL_ReadParasFromFile(DevNr, FileName);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteParasToFile(agl_int32_t DevNr, agl_cstr8_t FileName)
{
  if( Loaded(pAGL_WriteParasToFile))
  {
    return pAGL_WriteParasToFile(DevNr, FileName);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetParaPath(agl_cstr8_t DirName, agl_int32_t MaxLen)
{
  if( Loaded(pAGL_GetParaPath))
  {
    return pAGL_GetParaPath(DirName, MaxLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SetParaPath(const agl_cstr8_t DirName)
{
  if( Loaded(pAGL_SetParaPath))
  {
    return pAGL_SetParaPath(DirName);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SetAndGetParaPath(agl_cstr8_t CompanyName, agl_cstr8_t ProductName, agl_cstr8_t AktPath, agl_int32_t MaxLen)
{
  if( Loaded(pAGL_SetAndGetParaPath))
  {
    return pAGL_SetAndGetParaPath(CompanyName, ProductName, AktPath, MaxLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetPLCType(agl_int32_t DevNr, agl_int32_t PlcNr)
{
  if( Loaded(pAGL_GetPLCType))
  {
    return pAGL_GetPLCType(DevNr, PlcNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_HasFunc(agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t Func)
{
  if( Loaded(pAGL_HasFunc))
  {
    return pAGL_HasFunc(DevNr, PlcNr, Func);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_LoadErrorFile(agl_cstr8_t FileName)
{
  if( Loaded(pAGL_LoadErrorFile))
  {
    return pAGL_LoadErrorFile(FileName);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetErrorMsg(agl_int32_t ErrNr, agl_cstr8_t Msg, agl_int32_t MaxLen)
{
  if( Loaded(pAGL_GetErrorMsg))
  {
    return pAGL_GetErrorMsg(ErrNr, Msg, MaxLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetErrorCodeName(agl_int32_t ErrNr, const agl_cstr8_t* const ErrorCodeName)
{
  if( Loaded(pAGL_GetErrorCodeName))
  {
    return pAGL_GetErrorCodeName(ErrNr, ErrorCodeName);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_OpenDevice(agl_int32_t DevNr)
{
  if( Loaded(pAGL_OpenDevice))
  {
    return pAGL_OpenDevice(DevNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_CloseDevice(agl_int32_t DevNr)
{
  if( Loaded(pAGL_CloseDevice))
  {
    return pAGL_CloseDevice(DevNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SetDevNotification(agl_int32_t DevNr, LPNOTIFICATION pN)
{
  if( Loaded(pAGL_SetDevNotification))
  {
    return pAGL_SetDevNotification(DevNr, pN);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SetConnNotification(agl_int32_t ConnNr, LPNOTIFICATION pN)
{
  if( Loaded(pAGL_SetConnNotification))
  {
    return pAGL_SetConnNotification(ConnNr, pN);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SetJobNotification(agl_int32_t DevNr, agl_int32_t JobNr, LPNOTIFICATION pN)
{
  if( Loaded(pAGL_SetJobNotification))
  {
    return pAGL_SetJobNotification(DevNr, JobNr, pN);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SetJobNotificationEx(agl_int32_t DevNr, agl_int32_t JobNr, LPNOTIFICATION pN)
{
  if( Loaded(pAGL_SetJobNotificationEx))
  {
    return pAGL_SetJobNotificationEx(DevNr, JobNr, pN);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetJobResult(agl_int32_t DevNr, agl_int32_t JobNr, LPRESULT40 pR)
{
  if( Loaded(pAGL_GetJobResult))
  {
    return pAGL_GetJobResult(DevNr, JobNr, pR);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetLastJobResult(agl_int32_t ConnNr, LPRESULT40 pR)
{
  if( Loaded(pAGL_GetLastJobResult))
  {
    return pAGL_GetLastJobResult(ConnNr, pR);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_DeleteJob(agl_int32_t DevNr, agl_int32_t JobNr)
{
  if( Loaded(pAGL_DeleteJob))
  {
    return pAGL_DeleteJob(DevNr, JobNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WaitForJob(agl_int32_t DevNr, agl_int32_t JobNr)
{
  if( Loaded(pAGL_WaitForJob))
  {
    return pAGL_WaitForJob(DevNr, JobNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WaitForJobEx(agl_int32_t DevNr, agl_int32_t JobNr, LPRESULT40 pR)
{
  if( Loaded(pAGL_WaitForJobEx))
  {
    return pAGL_WaitForJobEx(DevNr, JobNr, pR);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_DialUp(agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_DialUp))
  {
    return pAGL_DialUp(DevNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_HangUp(agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_HangUp))
  {
    return pAGL_HangUp(DevNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitAdapter(agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_InitAdapter))
  {
    return pAGL_InitAdapter(DevNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ExitAdapter(agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ExitAdapter))
  {
    return pAGL_ExitAdapter(DevNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetLifelist(agl_int32_t DevNr, agl_uint8_t* List, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetLifelist))
  {
    return pAGL_GetLifelist(DevNr, List, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetDirectPLC(agl_int32_t DevNr, agl_uint8_t* pPlc, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetDirectPLC))
  {
    return pAGL_GetDirectPLC(DevNr, pPlc, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_PLCConnect(agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t* ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_PLCConnect))
  {
    return pAGL_PLCConnect(DevNr, PlcNr, ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_PLCConnectEx(agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t RackNr, agl_int32_t SlotNr, agl_int32_t* ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_PLCConnectEx))
  {
    return pAGL_PLCConnectEx(DevNr, PlcNr, RackNr, SlotNr, ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_PLCDisconnect(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_PLCDisconnect))
  {
    return pAGL_PLCDisconnect(ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadMaxPacketSize(agl_int32_t ConnNr)
{
  if( Loaded(pAGL_ReadMaxPacketSize))
  {
    return pAGL_ReadMaxPacketSize(ConnNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetRedConnState(agl_int32_t ConnNr, LPRED_CONN_STATE pState)
{
  if( Loaded(pAGL_GetRedConnState))
  {
    return pAGL_GetRedConnState(ConnNr, pState);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetRedConnStateMsg(agl_int32_t ConnNr, LPRED_CONN_STATE pState, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetRedConnStateMsg))
  {
    return pAGL_GetRedConnStateMsg(ConnNr, pState, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadOpState(agl_int32_t ConnNr, agl_int32_t* State, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadOpState))
  {
    return pAGL_ReadOpState(ConnNr, State, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadOpStateEx(agl_int32_t ConnNr, agl_int32_t* State, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadOpStateEx))
  {
    return pAGL_ReadOpStateEx(ConnNr, State, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetPLCStartOptions(agl_int32_t ConnNr, agl_int32_t* StartOptions, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetPLCStartOptions))
  {
    return pAGL_GetPLCStartOptions(ConnNr, StartOptions, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_PLCStop(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_PLCStop))
  {
    return pAGL_PLCStop(ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_PLCStart(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_PLCStart))
  {
    return pAGL_PLCStart(ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_PLCResume(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_PLCResume))
  {
    return pAGL_PLCResume(ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_PLCColdStart(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_PLCColdStart))
  {
    return pAGL_PLCColdStart(ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_IsHPLC(agl_int32_t ConnNr, agl_int32_t* IsHPLC, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_IsHPLC))
  {
    return pAGL_IsHPLC(ConnNr, IsHPLC, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_HPLCStop(agl_int32_t ConnNr, agl_int32_t CPUNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_HPLCStop))
  {
    return pAGL_HPLCStop(ConnNr, CPUNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_HPLCStart(agl_int32_t ConnNr, agl_int32_t CPUNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_HPLCStart))
  {
    return pAGL_HPLCStart(ConnNr, CPUNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_HPLCColdStart(agl_int32_t ConnNr, agl_int32_t CPUNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_HPLCColdStart))
  {
    return pAGL_HPLCColdStart(ConnNr, CPUNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetPLCClock(agl_int32_t ConnNr, LPTOD pTOD, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetPLCClock))
  {
    return pAGL_GetPLCClock(ConnNr, pTOD, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SetPLCClock(agl_int32_t ConnNr, LPTOD pTOD, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_SetPLCClock))
  {
    return pAGL_SetPLCClock(ConnNr, pTOD, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SyncPLCClock(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_SyncPLCClock))
  {
    return pAGL_SyncPLCClock(ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadMLFBNr(agl_int32_t ConnNr, LPMLFB pMLFBNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadMLFBNr))
  {
    return pAGL_ReadMLFBNr(ConnNr, pMLFBNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadMLFBNrEx(agl_int32_t ConnNr, LPMLFBEX pMLFBNrEx, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadMLFBNrEx))
  {
    return pAGL_ReadMLFBNrEx(ConnNr, pMLFBNrEx, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadPLCInfo(agl_int32_t ConnNr, LPPLCINFO pPLCInfo, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadPLCInfo))
  {
    return pAGL_ReadPLCInfo(ConnNr, pPLCInfo, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadCycleTime(agl_int32_t ConnNr, LPCYCLETIME pCycleTime, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadCycleTime))
  {
    return pAGL_ReadCycleTime(ConnNr, pCycleTime, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadProtLevel(agl_int32_t ConnNr, LPPROTLEVEL pProtLevel, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadProtLevel))
  {
    return pAGL_ReadProtLevel(ConnNr, pProtLevel, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadS7Ident(agl_int32_t ConnNr, LPS7_IDENT pIdent, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadS7Ident))
  {
    return pAGL_ReadS7Ident(ConnNr, pIdent, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadS7LED(agl_int32_t ConnNr, LPS7_LED pLed, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadS7LED))
  {
    return pAGL_ReadS7LED(ConnNr, pLed, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetExtModuleInfo(agl_int32_t ConnNr, LPEXT_MODULE_INFO pInfo, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetExtModuleInfo))
  {
    return pAGL_GetExtModuleInfo(ConnNr, pInfo, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadSzl(agl_int32_t ConnNr, agl_int32_t SzlId, agl_int32_t Index, agl_uint8_t* Buff, agl_int32_t* BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadSzl))
  {
    return pAGL_ReadSzl(ConnNr, SzlId, Index, Buff, BuffLen, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_IsPasswordReq(agl_int32_t ConnNr, agl_int32_t* IsPWReq, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_IsPasswordReq))
  {
    return pAGL_IsPasswordReq(ConnNr, IsPWReq, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SetPassword(agl_int32_t ConnNr, agl_cstr8_t  PW, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_SetPassword))
  {
    return pAGL_SetPassword(ConnNr, PW, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_UnSetPassword(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_UnSetPassword))
  {
    return pAGL_UnSetPassword(ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadDiagBufferEntrys(agl_int32_t ConnNr, agl_int32_t* Entrys, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadDiagBufferEntrys))
  {
    return pAGL_ReadDiagBufferEntrys(ConnNr, Entrys, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadDiagBuffer(agl_int32_t ConnNr, agl_int32_t* Entrys, agl_uint8_t* pDiagBuff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadDiagBuffer))
  {
    return pAGL_ReadDiagBuffer(ConnNr, Entrys, pDiagBuff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetDiagBufferEntry(agl_int32_t Index, agl_uint8_t* pDiagBuff, agl_cstr8_t Text, agl_int32_t TextLen)
{
  if( Loaded(pAGL_GetDiagBufferEntry))
  {
    return pAGL_GetDiagBufferEntry(Index, pDiagBuff, Text, TextLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadDBCount(agl_int32_t ConnNr, agl_int32_t* DBCount, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadDBCount))
  {
    return pAGL_ReadDBCount(ConnNr, DBCount, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadDBList(agl_int32_t ConnNr, agl_int32_t* DBCount, agl_uint16_t* DBList, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadDBList))
  {
    return pAGL_ReadDBList(ConnNr, DBCount, DBList, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadDBLen(agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t* DBLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadDBLen))
  {
    return pAGL_ReadDBLen(ConnNr, DBNr, DBLen, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadAllBlockCount(agl_int32_t ConnNr, LPALL_BLOCK_COUNT pBC, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadAllBlockCount))
  {
    return pAGL_ReadAllBlockCount(ConnNr, pBC, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadBlockCount(agl_int32_t ConnNr, agl_int32_t BlockType, agl_int32_t* BlockCount, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadBlockCount))
  {
    return pAGL_ReadBlockCount(ConnNr, BlockType, BlockCount, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadBlockList(agl_int32_t ConnNr, agl_int32_t BlockType, agl_int32_t* BlockCount, agl_uint16_t* BlockList, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadBlockList))
  {
    return pAGL_ReadBlockList(ConnNr, BlockType, BlockCount, BlockList, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadBlockLen(agl_int32_t ConnNr, agl_int32_t BlockType, agl_int32_t BlockNr, agl_int32_t* BlockLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadBlockLen))
  {
    return pAGL_ReadBlockLen(ConnNr, BlockType, BlockNr, BlockLen, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadInBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadInBytes))
  {
    return pAGL_ReadInBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadPInBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadPInBytes))
  {
    return pAGL_ReadPInBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadOutBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadOutBytes))
  {
    return pAGL_ReadOutBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadFlagBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadFlagBytes))
  {
    return pAGL_ReadFlagBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadSFlagBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadSFlagBytes))
  {
    return pAGL_ReadSFlagBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadVarBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadVarBytes))
  {
    return pAGL_ReadVarBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadDataBytes(agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadDataBytes))
  {
    return pAGL_ReadDataBytes(ConnNr, DBNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadDataWords(agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadDataWords))
  {
    return pAGL_ReadDataWords(ConnNr, DBNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadTimerWords(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadTimerWords))
  {
    return pAGL_ReadTimerWords(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadCounterWords(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadCounterWords))
  {
    return pAGL_ReadCounterWords(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadMix(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadMix))
  {
    return pAGL_ReadMix(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadMixEx(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadMixEx))
  {
    return pAGL_ReadMixEx(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteInBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteInBytes))
  {
    return pAGL_WriteInBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteOutBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteOutBytes))
  {
    return pAGL_WriteOutBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WritePOutBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WritePOutBytes))
  {
    return pAGL_WritePOutBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteFlagBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteFlagBytes))
  {
    return pAGL_WriteFlagBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteSFlagBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteSFlagBytes))
  {
    return pAGL_WriteSFlagBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteVarBytes(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteVarBytes))
  {
    return pAGL_WriteVarBytes(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteDataBytes(agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteDataBytes))
  {
    return pAGL_WriteDataBytes(ConnNr, DBNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteDataWords(agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteDataWords))
  {
    return pAGL_WriteDataWords(ConnNr, DBNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteTimerWords(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteTimerWords))
  {
    return pAGL_WriteTimerWords(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteCounterWords(agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteCounterWords))
  {
    return pAGL_WriteCounterWords(ConnNr, Start, Num, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteMix(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteMix))
  {
    return pAGL_WriteMix(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteMixEx(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteMixEx))
  {
    return pAGL_WriteMixEx(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitOptReadMix(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt)
{
  if( Loaded(pAGL_InitOptReadMix))
  {
    return pAGL_InitOptReadMix(ConnNr, Buff, Num, Opt);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadOptReadMix(agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadOptReadMix))
  {
    return pAGL_ReadOptReadMix(ConnNr, Opt, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_EndOptReadMix(agl_ptrdiff_t Opt)
{
  if( Loaded(pAGL_EndOptReadMix))
  {
    return pAGL_EndOptReadMix(Opt);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitOptReadMixEx(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt)
{
  if( Loaded(pAGL_InitOptReadMixEx))
  {
    return pAGL_InitOptReadMixEx(ConnNr, Buff, Num, Opt);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadOptReadMixEx(agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadOptReadMixEx))
  {
    return pAGL_ReadOptReadMixEx(ConnNr, Opt, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_EndOptReadMixEx(agl_ptrdiff_t Opt)
{
  if( Loaded(pAGL_EndOptReadMixEx))
  {
    return pAGL_EndOptReadMixEx(Opt);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitOptWriteMix(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt)
{
  if( Loaded(pAGL_InitOptWriteMix))
  {
    return pAGL_InitOptWriteMix(ConnNr, Buff, Num, Opt);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteOptWriteMix(agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteOptWriteMix))
  {
    return pAGL_WriteOptWriteMix(ConnNr, Opt, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_EndOptWriteMix(agl_ptrdiff_t Opt)
{
  if( Loaded(pAGL_EndOptWriteMix))
  {
    return pAGL_EndOptWriteMix(Opt);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitOptWriteMixEx(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt)
{
  if( Loaded(pAGL_InitOptWriteMixEx))
  {
    return pAGL_InitOptWriteMixEx(ConnNr, Buff, Num, Opt);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteOptWriteMixEx(agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_WriteOptWriteMixEx))
  {
    return pAGL_WriteOptWriteMixEx(ConnNr, Opt, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_EndOptWriteMixEx(agl_ptrdiff_t Opt)
{
  if( Loaded(pAGL_EndOptWriteMixEx))
  {
    return pAGL_EndOptWriteMixEx(Opt);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SetOptNotification(agl_ptrdiff_t Opt, LPNOTIFICATION pN)
{
  if( Loaded(pAGL_SetOptNotification))
  {
    return pAGL_SetOptNotification(Opt, pN);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_DeleteOptJob(agl_int32_t ConnNr, agl_ptrdiff_t Opt)
{
  if( Loaded(pAGL_DeleteOptJob))
  {
    return pAGL_DeleteOptJob(ConnNr, Opt);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetOptJobResult(agl_int32_t ConnNr, agl_ptrdiff_t Opt, LPRESULT40 pR)
{
  if( Loaded(pAGL_GetOptJobResult))
  {
    return pAGL_GetOptJobResult(ConnNr, Opt, pR);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WaitForOptJob(agl_int32_t ConnNr, agl_ptrdiff_t Opt)
{
  if( Loaded(pAGL_WaitForOptJob))
  {
    return pAGL_WaitForOptJob(ConnNr, Opt);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_ptrdiff_t AGL_API AGL_AllocRWBuffs(LPDATA_RW40 Buff, agl_int32_t Num)
{
  if( Loaded(pAGL_AllocRWBuffs))
  {
    return pAGL_AllocRWBuffs(Buff, Num);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_FreeRWBuffs(agl_ptrdiff_t Handle)
{
  if( Loaded(pAGL_FreeRWBuffs))
  {
    return pAGL_FreeRWBuffs(Handle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadRWBuff(LPDATA_RW40 Buff, agl_int32_t Index, void* pData)
{
  if( Loaded(pAGL_ReadRWBuff))
  {
    return pAGL_ReadRWBuff(Buff, Index, pData);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WriteRWBuff(LPDATA_RW40 Buff, agl_int32_t Index, void* pData)
{
  if( Loaded(pAGL_WriteRWBuff))
  {
    return pAGL_WriteRWBuff(Buff, Index, pData);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_RKSend(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_RKSend))
  {
    return pAGL_RKSend(ConnNr, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_RKSendEx(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_RKSendEx))
  {
    return pAGL_RKSendEx(ConnNr, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_RKFetch(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_RKFetch))
  {
    return pAGL_RKFetch(ConnNr, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_RKFetchEx(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_RKFetchEx))
  {
    return pAGL_RKFetchEx(ConnNr, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Send_RKFetch(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Send_RKFetch))
  {
    return pAGL_Send_RKFetch(ConnNr, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Recv_RKSend(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Recv_RKSend))
  {
    return pAGL_Recv_RKSend(ConnNr, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Recv_RKFetch(agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Recv_RKFetch))
  {
    return pAGL_Recv_RKFetch(ConnNr, Buff, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Send_3964(agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Send_3964))
  {
    return pAGL_Send_3964(ConnNr, Buff, BuffLen, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Recv_3964(agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t* BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Recv_3964))
  {
    return pAGL_Recv_3964(ConnNr, Buff, BuffLen, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_BSend(agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_BSend))
  {
    return pAGL_BSend(ConnNr, pwsa, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_BReceive(agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_BReceive))
  {
    return pAGL_BReceive(ConnNr, pwsa, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_BSendEx(agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t R_ID, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_BSendEx))
  {
    return pAGL_BSendEx(ConnNr, pwsa, R_ID, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_BReceiveEx(agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t* R_ID, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_BReceiveEx))
  {
    return pAGL_BReceiveEx(ConnNr, pwsa, R_ID, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_USend(agl_int32_t ConnNr, LPS7_USEND_URCV pUSR, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_USend))
  {
    return pAGL_USend(ConnNr, pUSR, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_UReceive(agl_int32_t ConnNr, LPS7_USEND_URCV pUSR, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_UReceive))
  {
    return pAGL_UReceive(ConnNr, pUSR, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitOpStateMsg(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_InitOpStateMsg))
  {
    return pAGL_InitOpStateMsg(ConnNr, OpState, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ExitOpStateMsg(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ExitOpStateMsg))
  {
    return pAGL_ExitOpStateMsg(ConnNr, OpState, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetOpStateMsg(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetOpStateMsg))
  {
    return pAGL_GetOpStateMsg(ConnNr, OpState, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitDiagMsg(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t DiagMask, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_InitDiagMsg))
  {
    return pAGL_InitDiagMsg(ConnNr, OpState, DiagMask, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ExitDiagMsg(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ExitDiagMsg))
  {
    return pAGL_ExitDiagMsg(ConnNr, OpState, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetDiagMsg(agl_int32_t ConnNr, LPS7_DIAG_MSG pDiag, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetDiagMsg))
  {
    return pAGL_GetDiagMsg(ConnNr, pDiag, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitCyclicRead(agl_int32_t ConnNr, agl_int32_t CycleTime, agl_int32_t Flags, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t* Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_InitCyclicRead))
  {
    return pAGL_InitCyclicRead(ConnNr, CycleTime, Flags, Buff, Num, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitCyclicReadEx(agl_int32_t ConnNr, agl_int32_t CycleTime, agl_int32_t Flags, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t* Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_InitCyclicReadEx))
  {
    return pAGL_InitCyclicReadEx(ConnNr, CycleTime, Flags, Buff, Num, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_StartCyclicRead(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_StartCyclicRead))
  {
    return pAGL_StartCyclicRead(ConnNr, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_StopCyclicRead(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_StopCyclicRead))
  {
    return pAGL_StopCyclicRead(ConnNr, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ExitCyclicRead(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ExitCyclicRead))
  {
    return pAGL_ExitCyclicRead(ConnNr, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetCyclicRead(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetCyclicRead))
  {
    return pAGL_GetCyclicRead(ConnNr, Buff, Num, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetCyclicReadEx(agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetCyclicReadEx))
  {
    return pAGL_GetCyclicReadEx(ConnNr, Buff, Num, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitScanMsg(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_InitScanMsg))
  {
    return pAGL_InitScanMsg(ConnNr, OpState, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ExitScanMsg(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ExitScanMsg))
  {
    return pAGL_ExitScanMsg(ConnNr, OpState, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetScanMsg(agl_int32_t ConnNr, LPS7_SCAN pScan, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetScanMsg))
  {
    return pAGL_GetScanMsg(ConnNr, pScan, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_HasAckTriggeredMsg(agl_int32_t ConnNr, agl_int32_t* Mode, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_HasAckTriggeredMsg))
  {
    return pAGL_HasAckTriggeredMsg(ConnNr, Mode, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitAlarmMsg(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_InitAlarmMsg))
  {
    return pAGL_InitAlarmMsg(ConnNr, OpState, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ExitAlarmMsg(agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ExitAlarmMsg))
  {
    return pAGL_ExitAlarmMsg(ConnNr, OpState, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetAlarmMsg(agl_int32_t ConnNr, LPS7_ALARM pAlarm, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetAlarmMsg))
  {
    return pAGL_GetAlarmMsg(ConnNr, pAlarm, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ReadOpenMsg(agl_int32_t ConnNr, LPS7_OPEN_MSG_STATE pState, agl_int32_t* MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ReadOpenMsg))
  {
    return pAGL_ReadOpenMsg(ConnNr, pState, MsgAnz, MsgType, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetMsgStateChange(agl_int32_t ConnNr, LPS7_RCV_MSG_STATE pState, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetMsgStateChange))
  {
    return pAGL_GetMsgStateChange(ConnNr, pState, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_AckMsg(agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_AckMsg))
  {
    return pAGL_AckMsg(ConnNr, pMsg, MsgAnz, MsgType, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_LockMsg(agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_LockMsg))
  {
    return pAGL_LockMsg(ConnNr, pMsg, MsgAnz, MsgType, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_UnlockMsg(agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_UnlockMsg))
  {
    return pAGL_UnlockMsg(ConnNr, pMsg, MsgAnz, MsgType, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_InitARSend(agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t ArAnz, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_InitARSend))
  {
    return pAGL_InitARSend(ConnNr, pMsg, ArAnz, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_ExitARSend(agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t ArAnz, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_ExitARSend))
  {
    return pAGL_ExitARSend(ConnNr, pMsg, ArAnz, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_GetARSend(agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t* AR_ID, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_GetARSend))
  {
    return pAGL_GetARSend(ConnNr, pwsa, AR_ID, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_RFC1006_Connect(agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t* ConnNr, LPRFC_1006_SERVER ConnInfo, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_RFC1006_Connect))
  {
    return pAGL_RFC1006_Connect(DevNr, PlcNr, ConnNr, ConnInfo, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_RFC1006_Disconnect(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_RFC1006_Disconnect))
  {
    return pAGL_RFC1006_Disconnect(ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_RFC1006_Receive(agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t BuffLen, agl_int32_t* ReceivedLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_RFC1006_Receive))
  {
    return pAGL_RFC1006_Receive(ConnNr, Buff, BuffLen, ReceivedLen, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_RFC1006_Send(agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_RFC1006_Send))
  {
    return pAGL_RFC1006_Send(ConnNr, Buff, BuffLen, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_ReadMixEx(agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_ReadMixEx))
  {
    return pAGL_NCK_ReadMixEx(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_WriteMixEx(agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_WriteMixEx))
  {
    return pAGL_NCK_WriteMixEx(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_CheckVarSize(agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_CheckVarSize))
  {
    return pAGL_NCK_CheckVarSize(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_InitCyclicReadEx(agl_int32_t ConnNr, agl_int32_t CycleTime, agl_int32_t OnlyChanged, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t* Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_InitCyclicReadEx))
  {
    return pAGL_NCK_InitCyclicReadEx(ConnNr, CycleTime, OnlyChanged, Buff, Num, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_StartCyclicRead(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_StartCyclicRead))
  {
    return pAGL_NCK_StartCyclicRead(ConnNr, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_StopCyclicRead(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_StopCyclicRead))
  {
    return pAGL_NCK_StopCyclicRead(ConnNr, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_ExitCyclicRead(agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_ExitCyclicRead))
  {
    return pAGL_NCK_ExitCyclicRead(ConnNr, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_GetCyclicReadEx(agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_GetCyclicReadEx))
  {
    return pAGL_NCK_GetCyclicReadEx(ConnNr, Buff, Num, Handle, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_EXTERN(agl_int32_t ConnNr, agl_int32_t Channel, agl_cstr8_t ProgName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_EXTERN))
  {
    return pAGL_NCK_PI_EXTERN(ConnNr, Channel, ProgName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_EXTMOD(agl_int32_t ConnNr, agl_int32_t Channel, agl_cstr8_t ProgName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_EXTMOD))
  {
    return pAGL_NCK_PI_EXTMOD(ConnNr, Channel, ProgName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_SELECT(agl_int32_t ConnNr, agl_int32_t Channel, agl_cstr8_t ProgName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_SELECT))
  {
    return pAGL_NCK_PI_SELECT(ConnNr, Channel, ProgName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_F_DELE(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_F_DELE))
  {
    return pAGL_NCK_PI_F_DELE(ConnNr, FileName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_F_PROT(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t Protection, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_F_PROT))
  {
    return pAGL_NCK_PI_F_PROT(ConnNr, FileName, Protection, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_F_RENA(agl_int32_t ConnNr, agl_cstr8_t OldFileName, agl_cstr8_t NewFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_F_RENA))
  {
    return pAGL_NCK_PI_F_RENA(ConnNr, OldFileName, NewFileName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_F_XFER(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_F_XFER))
  {
    return pAGL_NCK_PI_F_XFER(ConnNr, FileName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_LOGIN(agl_int32_t ConnNr, agl_cstr8_t Password, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_LOGIN))
  {
    return pAGL_NCK_PI_LOGIN(ConnNr, Password, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_LOGOUT(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_LOGOUT))
  {
    return pAGL_NCK_PI_LOGOUT(ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_F_OPEN(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t WindowName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_F_OPEN))
  {
    return pAGL_NCK_PI_F_OPEN(ConnNr, FileName, WindowName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_F_OPER(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t WindowName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_F_OPER))
  {
    return pAGL_NCK_PI_F_OPER(ConnNr, FileName, WindowName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_F_SEEK(agl_int32_t ConnNr, agl_cstr8_t WindowName, agl_int32_t SeekMode, agl_int32_t SeekPointer, agl_int32_t WindowSize, agl_cstr8_t CompareString, agl_int32_t SkipCount, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_F_SEEK))
  {
    return pAGL_NCK_PI_F_SEEK(ConnNr, WindowName, SeekMode, SeekPointer, WindowSize, CompareString, SkipCount, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_F_CLOS(agl_int32_t ConnNr, agl_cstr8_t WindowName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_F_CLOS))
  {
    return pAGL_NCK_PI_F_CLOS(ConnNr, WindowName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_StartAll(agl_int32_t ConnNr, agl_uint8_t* Para, agl_int32_t ParaLen, agl_cstr8_t Cmd, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_StartAll))
  {
    return pAGL_NCK_PI_StartAll(ConnNr, Para, ParaLen, Cmd, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_F_COPY(agl_int32_t ConnNr, agl_int32_t Direction, agl_cstr8_t SourceFileName, agl_cstr8_t DestinationFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_F_COPY))
  {
    return pAGL_NCK_PI_F_COPY(ConnNr, Direction, SourceFileName, DestinationFileName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_F_PROR(agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t Protection, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_F_PROR))
  {
    return pAGL_NCK_PI_F_PROR(ConnNr, FileName, Protection, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_CANCEL(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_CANCEL))
  {
    return pAGL_NCK_PI_CANCEL(ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_CRCEDN(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t DNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_CRCEDN))
  {
    return pAGL_NCK_PI_CRCEDN(ConnNr, ToolArea, TNumber, DNumber, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_DELECE(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t DNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_DELECE))
  {
    return pAGL_NCK_PI_DELECE(ConnNr, ToolArea, TNumber, DNumber, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_DELETO(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_DELETO))
  {
    return pAGL_NCK_PI_DELETO(ConnNr, ToolArea, TNumber, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_IBN_SS(agl_int32_t ConnNr, agl_int32_t Switch, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_IBN_SS))
  {
    return pAGL_NCK_PI_IBN_SS(ConnNr, Switch, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_MMCSEM(agl_int32_t ConnNr, agl_int32_t ChannelNumber, agl_int32_t FunctionNumber, agl_int32_t SemaValue, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_MMCSEM))
  {
    return pAGL_NCK_PI_MMCSEM(ConnNr, ChannelNumber, FunctionNumber, SemaValue, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_TMCRTO(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_cstr8_t ToolID, agl_int32_t ToolNumber, agl_int32_t DuploNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_TMCRTO))
  {
    return pAGL_NCK_PI_TMCRTO(ConnNr, ToolArea, ToolID, ToolNumber, DuploNumber, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_TMMVTL(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t ToolNumber, agl_int32_t SourcePlaceNumber, agl_int32_t SourceMagazineNumber, agl_int32_t DestinationPlaceNumber, agl_int32_t DestinationMagazineNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_TMMVTL))
  {
    return pAGL_NCK_PI_TMMVTL(ConnNr, ToolArea, ToolNumber, SourcePlaceNumber, SourceMagazineNumber, DestinationPlaceNumber, DestinationMagazineNumber, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_TMCRTC(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_cstr8_t ToolID, agl_int32_t ToolNumber, agl_int32_t DuploNumber, agl_int32_t EdgeNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_TMCRTC))
  {
    return pAGL_NCK_PI_TMCRTC(ConnNr, ToolArea, ToolID, ToolNumber, DuploNumber, EdgeNumber, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_PI_CREATO(agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_PI_CREATO))
  {
    return pAGL_NCK_PI_CREATO(ConnNr, ToolArea, TNumber, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_CopyFileToNC(agl_int32_t ConnNr, agl_cstr8_t NCFileName, agl_cstr8_t PCFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_CopyFileToNC))
  {
    return pAGL_NCK_CopyFileToNC(ConnNr, NCFileName, PCFileName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_CopyFileFromNC(agl_int32_t ConnNr, agl_cstr8_t NCFileName, agl_cstr8_t PCFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_CopyFileFromNC))
  {
    return pAGL_NCK_CopyFileFromNC(ConnNr, NCFileName, PCFileName, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_CopyToNC(agl_int32_t ConnNr, agl_cstr8_t FileName, void* Buff, agl_int32_t BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_CopyToNC))
  {
    return pAGL_NCK_CopyToNC(ConnNr, FileName, Buff, BuffLen, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_CopyFromNC(agl_int32_t ConnNr, agl_cstr8_t FileName, void* Buff, agl_int32_t BuffLen, agl_int32_t* NeededLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_CopyFromNC))
  {
    return pAGL_NCK_CopyFromNC(ConnNr, FileName, Buff, BuffLen, NeededLen, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_CopyFromNCAlloc(agl_int32_t ConnNr, agl_cstr8_t FileName, void** Buff, agl_int32_t* BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_NCK_CopyFromNCAlloc))
  {
    return pAGL_NCK_CopyFromNCAlloc(ConnNr, FileName, Buff, BuffLen, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_FreeBuff(void* Buff)
{
  if( Loaded(pAGL_NCK_FreeBuff))
  {
    return pAGL_NCK_FreeBuff(Buff);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_SetConnProgressNotification(agl_int32_t ConnNr, LPNOTIFICATION pN)
{
  if( Loaded(pAGL_NCK_SetConnProgressNotification))
  {
    return pAGL_NCK_SetConnProgressNotification(ConnNr, pN);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_CheckNSKVarLine(agl_cstr8_t Line, LPNCKDataRW pRW, agl_cstr8_t Name, agl_int32_t NameLen)
{
  if( Loaded(pAGL_NCK_CheckNSKVarLine))
  {
    return pAGL_NCK_CheckNSKVarLine(Line, pRW, Name, NameLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_ReadNSKVarFile(agl_cstr8_t FileName, LPNCKDataRW* ppRW, agl_cstr8_t** pName)
{
  if( Loaded(pAGL_NCK_ReadNSKVarFile))
  {
    return pAGL_NCK_ReadNSKVarFile(FileName, ppRW, pName);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_CheckCSVVarLine(agl_cstr8_t Line, LPNCKDataRW pRW, agl_cstr8_t Name, agl_int32_t NameLen)
{
  if( Loaded(pAGL_NCK_CheckCSVVarLine))
  {
    return pAGL_NCK_CheckCSVVarLine(Line, pRW, Name, NameLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_ReadCSVVarFile(agl_cstr8_t FileName, LPNCKDataRW* ppRW, agl_cstr8_t** pName)
{
  if( Loaded(pAGL_NCK_ReadCSVVarFile))
  {
    return pAGL_NCK_ReadCSVVarFile(FileName, ppRW, pName);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_ReadGUDVarFile(agl_cstr8_t FileName, LPNCKDataRW* ppRW, agl_cstr8_t** pName)
{
  if( Loaded(pAGL_NCK_ReadGUDVarFile))
  {
    return pAGL_NCK_ReadGUDVarFile(FileName, ppRW, pName);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_ReadGUDVarFileEx(agl_cstr8_t FileName, agl_int32_t GUDNr, agl_int32_t Area, LPNCKDataRW* ppRW, agl_cstr8_t** pName)
{
  if( Loaded(pAGL_NCK_ReadGUDVarFileEx))
  {
    return pAGL_NCK_ReadGUDVarFileEx(FileName, GUDNr, Area, ppRW, pName);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_FreeVarBuff(LPNCKDataRW* ppRW, agl_cstr8_t** pName, agl_int32_t Anz)
{
  if( Loaded(pAGL_NCK_FreeVarBuff))
  {
    return pAGL_NCK_FreeVarBuff(ppRW, pName, Anz);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_GetSingleVarDef(LPNCKDataRW* ppRW, agl_cstr8_t** pName, agl_int32_t Index, LPNCKDataRW pRW, agl_cstr8_t Name, agl_int32_t NameLen, agl_int32_t AllocBuff)
{
  if( Loaded(pAGL_NCK_GetSingleVarDef))
  {
    return pAGL_NCK_GetSingleVarDef(ppRW, pName, Index, pRW, Name, NameLen, AllocBuff);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_ExtractNckAlarm(void* Buffer, agl_int32_t BufferSize, LPNCKAlarm NCKAlarm)
{
  if( Loaded(pAGL_NCK_ExtractNckAlarm))
  {
    return pAGL_NCK_ExtractNckAlarm(Buffer, BufferSize, NCKAlarm);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_NCK_GetNCKDataRWByNCDDEItem(const agl_cstr8_t const Item, LPNCKDataRW DataRW, agl_int32_t* ErrorPosition)
{
  if( Loaded(pAGL_NCK_GetNCKDataRWByNCDDEItem))
  {
    return pAGL_NCK_GetNCKDataRWByNCDDEItem(Item, DataRW, ErrorPosition);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Drive_ReadMix(agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Drive_ReadMix))
  {
    return pAGL_Drive_ReadMix(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Drive_ReadMixEx(agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Drive_ReadMixEx))
  {
    return pAGL_Drive_ReadMixEx(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Drive_WriteMix(agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Drive_WriteMix))
  {
    return pAGL_Drive_WriteMix(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Drive_WriteMixEx(agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Drive_WriteMixEx))
  {
    return pAGL_Drive_WriteMixEx(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

void* AGL_API AGL_malloc(agl_int32_t Size)
{
  if( Loaded(pAGL_malloc))
  {
    return pAGL_malloc(Size);
  }
  return NULL;
}

void* AGL_API AGL_calloc(agl_int32_t Anz, agl_int32_t Size)
{
  if( Loaded(pAGL_calloc))
  {
    return pAGL_calloc(Anz, Size);
  }
  return NULL;
}

void* AGL_API AGL_realloc(void* Ptr, agl_int32_t Size)
{
  if( Loaded(pAGL_realloc))
  {
    return pAGL_realloc(Ptr, Size);
  }
  return NULL;
}

void* AGL_API AGL_memcpy(void* DestPtr, void* SrcPtr, agl_int32_t Len)
{
  if( Loaded(pAGL_memcpy))
  {
    return pAGL_memcpy(DestPtr, SrcPtr, Len);
  }
  return DestPtr;
}

void* AGL_API AGL_memmove(void* DestPtr, void* SrcPtr, agl_int32_t Len)
{
  if( Loaded(pAGL_memmove))
  {
    return pAGL_memmove(DestPtr, SrcPtr, Len);
  }
  return DestPtr;
}

agl_int32_t AGL_API AGL_memcmp(void* Ptr1, void* Ptr2, agl_int32_t Len)
{
  if( Loaded(pAGL_memcmp))
  {
    return pAGL_memcmp(Ptr1, Ptr2, Len);
  }
  return AGL40_DYN_DLL_ERROR;
}

void AGL_API AGL_free(void* Ptr)
{
  if( Loaded(pAGL_free))
  {
    pAGL_free(Ptr);
  }
}

agl_int16_t AGL_API AGL_ReadInt16(agl_uint8_t* Buff)
{
  if( Loaded(pAGL_ReadInt16))
  {
    return pAGL_ReadInt16(Buff);
  }
  return 0;
}

agl_long32_t AGL_API AGL_ReadInt32(agl_uint8_t* Buff)
{
  if( Loaded(pAGL_ReadInt32))
  {
    return pAGL_ReadInt32(Buff);
  }
  return 0;
}

agl_uint16_t AGL_API AGL_ReadWord(agl_uint8_t* Buff)
{
  if( Loaded(pAGL_ReadWord))
  {
    return pAGL_ReadWord(Buff);
  }
  return 0;
}

agl_ulong32_t AGL_API AGL_ReadDWord(agl_uint8_t* Buff)
{
  if( Loaded(pAGL_ReadDWord))
  {
    return pAGL_ReadDWord(Buff);
  }
  return 0;
}

agl_float32_t AGL_API AGL_ReadReal(agl_uint8_t* Buff)
{
  if( Loaded(pAGL_ReadReal))
  {
    return pAGL_ReadReal(Buff);
  }
  return 0;
}

agl_int32_t AGL_API AGL_ReadS5Time(agl_uint8_t* Buff)
{
  if( Loaded(pAGL_ReadS5Time))
  {
    return pAGL_ReadS5Time(Buff);
  }
  return 0;
}

agl_int32_t AGL_API AGL_ReadS5TimeW(agl_uint16_t* Buff)
{
  if( Loaded(pAGL_ReadS5TimeW))
  {
    return pAGL_ReadS5TimeW(Buff);
  }
  return 0;
}

void AGL_API AGL_WriteInt16(agl_uint8_t* Buff, agl_int16_t Val)
{
  if( Loaded(pAGL_WriteInt16))
  {
    pAGL_WriteInt16(Buff, Val);
  }
}

void AGL_API AGL_WriteInt32(agl_uint8_t* Buff, agl_long32_t Val)
{
  if( Loaded(pAGL_WriteInt32))
  {
    pAGL_WriteInt32(Buff, Val);
  }
}

void AGL_API AGL_WriteWord(agl_uint8_t* Buff, agl_uint16_t Val)
{
  if( Loaded(pAGL_WriteWord))
  {
    pAGL_WriteWord(Buff, Val);
  }
}

void AGL_API AGL_WriteDWord(agl_uint8_t* Buff, agl_ulong32_t Val)
{
  if( Loaded(pAGL_WriteDWord))
  {
    pAGL_WriteDWord(Buff, Val);
  }
}

void AGL_API AGL_WriteReal(agl_uint8_t* Buff, agl_float32_t Val)
{
  if( Loaded(pAGL_WriteReal))
  {
    pAGL_WriteReal(Buff, Val);
  }
}

void AGL_API AGL_WriteS5Time(agl_uint8_t* Buff, agl_int32_t Val)
{
  if( Loaded(pAGL_WriteS5Time))
  {
    pAGL_WriteS5Time(Buff, Val);
  }
}

void AGL_API AGL_WriteS5TimeW(agl_uint16_t* Buff, agl_int32_t Val)
{
  if( Loaded(pAGL_WriteS5TimeW))
  {
    pAGL_WriteS5TimeW(Buff, Val);
  }
}

void AGL_API AGL_Byte2Word(agl_uint16_t* OutBuff, agl_uint8_t* InBuff, agl_int32_t AnzWords)
{
  if( Loaded(pAGL_Byte2Word))
  {
    pAGL_Byte2Word(OutBuff, InBuff, AnzWords);
  }
}

void AGL_API AGL_Byte2DWord(agl_ulong32_t* OutBuff, agl_uint8_t* InBuff, agl_int32_t AnzDWords)
{
  if( Loaded(pAGL_Byte2DWord))
  {
    pAGL_Byte2DWord(OutBuff, InBuff, AnzDWords);
  }
}

void AGL_API AGL_Byte2Real(agl_float32_t* OutBuff, agl_uint8_t* InBuff, agl_int32_t AnzReals)
{
  if( Loaded(pAGL_Byte2Real))
  {
    pAGL_Byte2Real(OutBuff, InBuff, AnzReals);
  }
}

void AGL_API AGL_Word2Byte(agl_uint8_t* OutBuff, agl_uint16_t* InBuff, agl_int32_t AnzWords)
{
  if( Loaded(pAGL_Word2Byte))
  {
    pAGL_Word2Byte(OutBuff, InBuff, AnzWords);
  }
}

void AGL_API AGL_DWord2Byte(agl_uint8_t* OutBuff, agl_ulong32_t* InBuff, agl_int32_t AnzDWords)
{
  if( Loaded(pAGL_DWord2Byte))
  {
    pAGL_DWord2Byte(OutBuff, InBuff, AnzDWords);
  }
}

void AGL_API AGL_Real2Byte(agl_uint8_t* OutBuff, agl_float32_t* InBuff, agl_int32_t AnzReals)
{
  if( Loaded(pAGL_Real2Byte))
  {
    pAGL_Real2Byte(OutBuff, InBuff, AnzReals);
  }
}

agl_int32_t AGL_API AGL_GetBit(agl_uint8_t Wert, agl_int32_t BitNr)
{
  return ( (Wert & AGL_CPP_BitMask[BitNr&7]) != 0 );
}

agl_uint8_t AGL_API AGL_SetBit(agl_uint8_t* Buff, agl_int32_t BitNr)
{
  return( *Buff |= AGL_CPP_BitMask[BitNr&7] );
}

agl_uint8_t AGL_API AGL_ResetBit(agl_uint8_t* Buff, agl_int32_t BitNr)
{
  return( *Buff &= ~AGL_CPP_BitMask[BitNr&7] );
}

agl_uint8_t AGL_API AGL_SetBitVal(agl_uint8_t* Buff, agl_int32_t BitNr, agl_int32_t Val)
{
  if( Val )
  {
    return( *Buff |= AGL_CPP_BitMask[BitNr&7] );
  }
  return( *Buff &= ~AGL_CPP_BitMask[BitNr&7] );
}

agl_int32_t AGL_API AGL_Buff2String(agl_uint8_t* Buff, agl_cstr8_t Text, agl_int32_t AnzChars)
{
  if( Loaded(pAGL_Buff2String))
  {
    return pAGL_Buff2String(Buff, Text, AnzChars);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_String2Buff(agl_uint8_t* Buff, agl_cstr8_t Text, agl_int32_t AnzChars)
{
  if( Loaded(pAGL_String2Buff))
  {
    return pAGL_String2Buff(Buff, Text, AnzChars);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Buff2WString(agl_uint8_t* Buff, agl_wchar_t* Text, agl_int32_t AnzChars)
{
  if( Loaded(pAGL_Buff2WString))
  {
    return pAGL_Buff2WString(Buff, Text, AnzChars);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WString2Buff(agl_uint8_t* Buff, agl_wchar_t* Text, agl_int32_t AnzChars)
{
  if( Loaded(pAGL_WString2Buff))
  {
    return pAGL_WString2Buff(Buff, Text, AnzChars);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_S7String2String(agl_uint8_t* S7String, agl_cstr8_t Text, agl_int32_t MaxChars)
{
  if( Loaded(pAGL_S7String2String))
  {
    return pAGL_S7String2String(S7String, Text, MaxChars);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_String2S7String(agl_uint8_t* S7String, agl_cstr8_t Text, agl_int32_t MaxChars)
{
  if( Loaded(pAGL_String2S7String))
  {
    return pAGL_String2S7String(S7String, Text, MaxChars);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_BCD2Int16(agl_int16_t BCD, agl_int16_t* Dual)
{
  if( Loaded(pAGL_BCD2Int16))
  {
    return pAGL_BCD2Int16(BCD, Dual);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_BCD2Int32(agl_long32_t BCD, agl_long32_t* Dual)
{
  if( Loaded(pAGL_BCD2Int32))
  {
    return pAGL_BCD2Int32(BCD, Dual);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Int162BCD(agl_int16_t Dual, agl_int16_t* BCD)
{
  if( Loaded(pAGL_Int162BCD))
  {
    return pAGL_Int162BCD(Dual, BCD);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Int322BCD(agl_long32_t Dual, agl_long32_t* BCD)
{
  if( Loaded(pAGL_Int322BCD))
  {
    return pAGL_Int322BCD(Dual, BCD);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_float32_t AGL_API AGL_LongAsFloat(agl_long32_t Var)
{
  if( Loaded(pAGL_LongAsFloat))
  {
    return pAGL_LongAsFloat(Var);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_long32_t AGL_API AGL_FloatAsLong(agl_float32_t Var)
{
  if( Loaded(pAGL_FloatAsLong))
  {
    return pAGL_FloatAsLong(Var);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Text2DataRW(agl_cstr8_t Text, LPDATA_RW40 RW)
{
  if( Loaded(pAGL_Text2DataRW))
  {
    return pAGL_Text2DataRW(Text, RW);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_DataRW2Text(LPDATA_RW40 RW, agl_cstr8_t Text)
{
  if( Loaded(pAGL_DataRW2Text))
  {
    return pAGL_DataRW2Text(RW, Text);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_S7DT2SysTime(agl_uint8_t* Buff, agl_systemtime_t* SysTime)
{
  if( Loaded(pAGL_S7DT2SysTime))
  {
    return pAGL_S7DT2SysTime(Buff, SysTime);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SysTime2S7DT(agl_systemtime_t* SysTime, agl_uint8_t* Buff)
{
  if( Loaded(pAGL_SysTime2S7DT))
  {
    return pAGL_SysTime2S7DT(SysTime, Buff);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_TOD2SysTime(LPTOD pTOD, agl_systemtime_t* SysTime)
{
  if( Loaded(pAGL_TOD2SysTime))
  {
    return pAGL_TOD2SysTime(pTOD, SysTime);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_SysTime2TOD(agl_systemtime_t* SysTime, LPTOD pTOD)
{
  if( Loaded(pAGL_SysTime2TOD))
  {
    return pAGL_SysTime2TOD(SysTime, pTOD);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_S7Date2YMD(agl_uint16_t Date, agl_uint16_t* Year, agl_uint16_t* Month, agl_uint16_t* Day)
{
  if( Loaded(pAGL_S7Date2YMD))
  {
    return pAGL_S7Date2YMD(Date, Year, Month, Day);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Float2KG(agl_uint16_t* pKG, agl_float32_t* pFloat, agl_int32_t AnzFloats)
{
  if( Loaded(pAGL_Float2KG))
  {
    return pAGL_Float2KG(pKG, pFloat, AnzFloats);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_KG2Float(agl_float32_t* pFloat, agl_uint16_t* pKG, agl_int32_t AnzFloats)
{
  if( Loaded(pAGL_KG2Float))
  {
    return pAGL_KG2Float(pFloat, pKG, AnzFloats);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Float2DWKG(agl_ulong32_t* pKG, agl_float32_t* pFloat, agl_int32_t AnzFloats)
{
  if( Loaded(pAGL_Float2DWKG))
  {
    return pAGL_Float2DWKG(pKG, pFloat, AnzFloats);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_DWKG2Float(agl_float32_t* pFloat, agl_ulong32_t* pKG, agl_int32_t AnzFloats)
{
  if( Loaded(pAGL_DWKG2Float))
  {
    return pAGL_DWKG2Float(pFloat, pKG, AnzFloats);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_S7Ident2String(LPS7_IDENT pIdent, agl_int32_t Index, agl_cstr8_t Text, agl_int32_t MaxChars)
{
  if( Loaded(pAGL_S7Ident2String))
  {
    return pAGL_S7Ident2String(pIdent, Index, Text, MaxChars);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_OpenProject(agl_cstr8_t  Project, agl_ptrdiff_t* PrjHandle)
{
  if( Loaded(pAGLSym_OpenProject))
  {
    return pAGLSym_OpenProject(Project, PrjHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_CloseProject(agl_ptrdiff_t PrjHandle)
{
  if( Loaded(pAGLSym_CloseProject))
  {
    return pAGLSym_CloseProject(PrjHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_WriteCpuListToFile(agl_cstr8_t  FileName)
{
  if( Loaded(pAGLSym_WriteCpuListToFile))
  {
    return pAGLSym_WriteCpuListToFile(FileName);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetProgramCount(agl_ptrdiff_t PrjHandle, agl_int32_t* ProgCount)
{
  if( Loaded(pAGLSym_GetProgramCount))
  {
    return pAGLSym_GetProgramCount(PrjHandle, ProgCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindFirstProgram(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Program)
{
  if( Loaded(pAGLSym_FindFirstProgram))
  {
    return pAGLSym_FindFirstProgram(PrjHandle, Program);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindNextProgram(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Program)
{
  if( Loaded(pAGLSym_FindNextProgram))
  {
    return pAGLSym_FindNextProgram(PrjHandle, Program);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindCloseProgram(agl_ptrdiff_t PrjHandle)
{
  if( Loaded(pAGLSym_FindCloseProgram))
  {
    return pAGLSym_FindCloseProgram(PrjHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_SelectProgram(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Program)
{
  if( Loaded(pAGLSym_SelectProgram))
  {
    return pAGLSym_SelectProgram(PrjHandle, Program);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetSymbolCount(agl_ptrdiff_t PrjHandle, agl_int32_t* SymCount)
{
  if( Loaded(pAGLSym_GetSymbolCount))
  {
    return pAGLSym_GetSymbolCount(PrjHandle, SymCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetSymbolCountFilter(agl_ptrdiff_t PrjHandle, agl_int32_t* SymCount, const agl_cstr8_t Filter)
{
  if( Loaded(pAGLSym_GetSymbolCountFilter))
  {
    return pAGLSym_GetSymbolCountFilter(PrjHandle, SymCount, Filter);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindFirstSymbol(agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format)
{
  if( Loaded(pAGLSym_FindFirstSymbol))
  {
    return pAGLSym_FindFirstSymbol(PrjHandle, AbsOpd, Symbol, Comment, Format);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindFirstSymbolFilter(agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format, const agl_cstr8_t Filter)
{
  if( Loaded(pAGLSym_FindFirstSymbolFilter))
  {
    return pAGLSym_FindFirstSymbolFilter(PrjHandle, AbsOpd, Symbol, Comment, Format, Filter);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindNextSymbol(agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format)
{
  if( Loaded(pAGLSym_FindNextSymbol))
  {
    return pAGLSym_FindNextSymbol(PrjHandle, AbsOpd, Symbol, Comment, Format);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindCloseSymbol(agl_ptrdiff_t PrjHandle)
{
  if( Loaded(pAGLSym_FindCloseSymbol))
  {
    return pAGLSym_FindCloseSymbol(PrjHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_ReadPrjDBCount(agl_ptrdiff_t PrjHandle, agl_int32_t* DBCount)
{
  if( Loaded(pAGLSym_ReadPrjDBCount))
  {
    return pAGLSym_ReadPrjDBCount(PrjHandle, DBCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_ReadPrjDBCountFilter(agl_ptrdiff_t PrjHandle, agl_int32_t* DBCount, const agl_cstr8_t Filter)
{
  if( Loaded(pAGLSym_ReadPrjDBCountFilter))
  {
    return pAGLSym_ReadPrjDBCountFilter(PrjHandle, DBCount, Filter);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_ReadPrjDBList(agl_ptrdiff_t PrjHandle, agl_uint16_t* DBList, agl_int32_t DBCount)
{
  if( Loaded(pAGLSym_ReadPrjDBList))
  {
    return pAGLSym_ReadPrjDBList(PrjHandle, DBList, DBCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_ReadPrjDBListFilter(agl_ptrdiff_t PrjHandle, agl_uint16_t* DBList, agl_int32_t DBCount, const agl_cstr8_t Filter)
{
  if( Loaded(pAGLSym_ReadPrjDBListFilter))
  {
    return pAGLSym_ReadPrjDBListFilter(PrjHandle, DBList, DBCount, Filter);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_ReadPrjBlkCountFilter(agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_int32_t* BlkCount, const agl_cstr8_t Filter)
{
  if( Loaded(pAGLSym_ReadPrjBlkCountFilter))
  {
    return pAGLSym_ReadPrjBlkCountFilter(PrjHandle, BlkType, BlkCount, Filter);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_ReadPrjBlkListFilter(agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_uint16_t* BlkList, agl_int32_t BlkCount, const agl_cstr8_t Filter)
{
  if( Loaded(pAGLSym_ReadPrjBlkListFilter))
  {
    return pAGLSym_ReadPrjBlkListFilter(PrjHandle, BlkType, BlkList, BlkCount, Filter);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetDbSymbolCount(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_int32_t* DBSymCount)
{
  if( Loaded(pAGLSym_GetDbSymbolCount))
  {
    return pAGLSym_GetDbSymbolCount(PrjHandle, DBNr, DBSymCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetDbSymbolCountFilter(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_int32_t* DBSymCount, const agl_cstr8_t Filter)
{
  if( Loaded(pAGLSym_GetDbSymbolCountFilter))
  {
    return pAGLSym_GetDbSymbolCountFilter(PrjHandle, DBNr, DBSymCount, Filter);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindFirstDbSymbol(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format)
{
  if( Loaded(pAGLSym_FindFirstDbSymbol))
  {
    return pAGLSym_FindFirstDbSymbol(PrjHandle, DBNr, AbsOpd, Symbol, Comment, Format);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindFirstDbSymbolFilter(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format, const agl_cstr8_t Filter)
{
  if( Loaded(pAGLSym_FindFirstDbSymbolFilter))
  {
    return pAGLSym_FindFirstDbSymbolFilter(PrjHandle, DBNr, AbsOpd, Symbol, Comment, Format, Filter);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindNextDbSymbol(agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format)
{
  if( Loaded(pAGLSym_FindNextDbSymbol))
  {
    return pAGLSym_FindNextDbSymbol(PrjHandle, AbsOpd, Symbol, Comment, Format);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindFirstDbSymbolEx(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, LPDATA_DBSYM40 Buff, const agl_cstr8_t Filter)
{
  if( Loaded(pAGLSym_FindFirstDbSymbolEx))
  {
    return pAGLSym_FindFirstDbSymbolEx(PrjHandle, DBNr, Buff, Filter);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindNextDbSymbolEx(agl_ptrdiff_t PrjHandle, LPDATA_DBSYM40 Buff)
{
  if( Loaded(pAGLSym_FindNextDbSymbolEx))
  {
    return pAGLSym_FindNextDbSymbolEx(PrjHandle, Buff);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetDbSymbolExComment(agl_ptrdiff_t PrjHandle, agl_cstr8_t  ExComment)
{
  if( Loaded(pAGLSym_GetDbSymbolExComment))
  {
    return pAGLSym_GetDbSymbolExComment(PrjHandle, ExComment);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindCloseDbSymbol(agl_ptrdiff_t PrjHandle)
{
  if( Loaded(pAGLSym_FindCloseDbSymbol))
  {
    return pAGLSym_FindCloseDbSymbol(PrjHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetDbDependency(agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_int32_t* BlkType, agl_int32_t* BlkNr)
{
  if( Loaded(pAGLSym_GetDbDependency))
  {
    return pAGLSym_GetDbDependency(PrjHandle, DBNr, BlkType, BlkNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetDeclarationCountFilter(agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_int32_t BlkNr, const agl_cstr8_t Filter, agl_int32_t* DeclarationCount)
{
  if( Loaded(pAGLSym_GetDeclarationCountFilter))
  {
    return pAGLSym_GetDeclarationCountFilter(PrjHandle, BlkType, BlkNr, Filter, DeclarationCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindFirstDeclarationFilter(agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_int32_t BlkNr, const agl_cstr8_t Filter, agl_ptrdiff_t* FindHandle, LPDATA_DECLARATION Buff)
{
  if( Loaded(pAGLSym_FindFirstDeclarationFilter))
  {
    return pAGLSym_FindFirstDeclarationFilter(PrjHandle, BlkType, BlkNr, Filter, FindHandle, Buff);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindNextDeclaration(agl_ptrdiff_t PrjHandle, agl_ptrdiff_t FindHandle, LPDATA_DECLARATION Buff)
{
  if( Loaded(pAGLSym_FindNextDeclaration))
  {
    return pAGLSym_FindNextDeclaration(PrjHandle, FindHandle, Buff);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetDeclarationInitialValue(agl_ptrdiff_t PrjHandle, agl_ptrdiff_t FindHandle, agl_int32_t* BufferLength, agl_cstr8_t  InitialValue)
{
  if( Loaded(pAGLSym_GetDeclarationInitialValue))
  {
    return pAGLSym_GetDeclarationInitialValue(PrjHandle, FindHandle, BufferLength, InitialValue);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindCloseDeclaration(agl_ptrdiff_t PrjHandle, agl_ptrdiff_t FindHandle)
{
  if( Loaded(pAGLSym_FindCloseDeclaration))
  {
    return pAGLSym_FindCloseDeclaration(PrjHandle, FindHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetSymbolFromText(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format)
{
  if( Loaded(pAGLSym_GetSymbolFromText))
  {
    return pAGLSym_GetSymbolFromText(PrjHandle, Text, AbsOpd, Symbol, Comment, Format);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetSymbolFromTextEx(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, LPDATA_DBSYM40 Buff)
{
  if( Loaded(pAGLSym_GetSymbolFromTextEx))
  {
    return pAGLSym_GetSymbolFromTextEx(PrjHandle, Text, Buff);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetReadMixFromText(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, LPDATA_RW40 Buff, agl_int32_t* Format)
{
  if( Loaded(pAGLSym_GetReadMixFromText))
  {
    return pAGLSym_GetReadMixFromText(PrjHandle, Text, Buff, Format);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetReadMixFromTextEx(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, agl_cstr8_t  AbsOpd, LPDATA_RW40 Buff, agl_int32_t* Format)
{
  if( Loaded(pAGLSym_GetReadMixFromTextEx))
  {
    return pAGLSym_GetReadMixFromTextEx(PrjHandle, Text, AbsOpd, Buff, Format);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetSymbol(agl_ptrdiff_t PrjHandle, LPDATA_RW40 Buff, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format)
{
  if( Loaded(pAGLSym_GetSymbol))
  {
    return pAGLSym_GetSymbol(PrjHandle, Buff, AbsOpd, Symbol, Comment, Format);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetSymbolEx(agl_ptrdiff_t PrjHandle, LPDATA_RW40 Buff, LPDATA_DBSYM40 Symbol)
{
  if( Loaded(pAGLSym_GetSymbolEx))
  {
    return pAGLSym_GetSymbolEx(PrjHandle, Buff, Symbol);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_OpenAlarms(agl_ptrdiff_t PrjHandle)
{
  if( Loaded(pAGLSym_OpenAlarms))
  {
    return pAGLSym_OpenAlarms(PrjHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_CloseAlarms(agl_ptrdiff_t PrjHandle)
{
  if( Loaded(pAGLSym_CloseAlarms))
  {
    return pAGLSym_CloseAlarms(PrjHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindFirstAlarmData(agl_ptrdiff_t PrjHandle, agl_int32_t* AlmNr)
{
  if( Loaded(pAGLSym_FindFirstAlarmData))
  {
    return pAGLSym_FindFirstAlarmData(PrjHandle, AlmNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindNextAlarmData(agl_ptrdiff_t PrjHandle, agl_int32_t* AlmNr)
{
  if( Loaded(pAGLSym_FindNextAlarmData))
  {
    return pAGLSym_FindNextAlarmData(PrjHandle, AlmNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindCloseAlarmData(agl_ptrdiff_t PrjHandle)
{
  if( Loaded(pAGLSym_FindCloseAlarmData))
  {
    return pAGLSym_FindCloseAlarmData(PrjHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmData(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, LPDATA_ALARM40 Buff)
{
  if( Loaded(pAGLSym_GetAlarmData))
  {
    return pAGLSym_GetAlarmData(PrjHandle, AlmNr, Buff);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmName(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen)
{
  if( Loaded(pAGLSym_GetAlarmName))
  {
    return pAGLSym_GetAlarmName(PrjHandle, AlmNr, Buff, BuffLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmType(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen)
{
  if( Loaded(pAGLSym_GetAlarmType))
  {
    return pAGLSym_GetAlarmType(PrjHandle, AlmNr, Buff, BuffLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmBaseName(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen)
{
  if( Loaded(pAGLSym_GetAlarmBaseName))
  {
    return pAGLSym_GetAlarmBaseName(PrjHandle, AlmNr, Buff, BuffLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmTypeName(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen)
{
  if( Loaded(pAGLSym_GetAlarmTypeName))
  {
    return pAGLSym_GetAlarmTypeName(PrjHandle, AlmNr, Buff, BuffLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmSignalCount(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* SignalCount)
{
  if( Loaded(pAGLSym_GetAlarmSignalCount))
  {
    return pAGLSym_GetAlarmSignalCount(PrjHandle, AlmNr, SignalCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmMsgClass(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* MsgClass)
{
  if( Loaded(pAGLSym_GetAlarmMsgClass))
  {
    return pAGLSym_GetAlarmMsgClass(PrjHandle, AlmNr, Signal, MsgClass);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmPriority(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* Priority)
{
  if( Loaded(pAGLSym_GetAlarmPriority))
  {
    return pAGLSym_GetAlarmPriority(PrjHandle, AlmNr, Signal, Priority);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmAckGroup(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* AckGroup)
{
  if( Loaded(pAGLSym_GetAlarmAckGroup))
  {
    return pAGLSym_GetAlarmAckGroup(PrjHandle, AlmNr, Signal, AckGroup);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmAcknowledge(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* Acknowledge)
{
  if( Loaded(pAGLSym_GetAlarmAcknowledge))
  {
    return pAGLSym_GetAlarmAcknowledge(PrjHandle, AlmNr, Signal, Acknowledge);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmProtocol(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* Protocol)
{
  if( Loaded(pAGLSym_GetAlarmProtocol))
  {
    return pAGLSym_GetAlarmProtocol(PrjHandle, AlmNr, Protocol);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmDispGroup(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* DispGroup)
{
  if( Loaded(pAGLSym_GetAlarmDispGroup))
  {
    return pAGLSym_GetAlarmDispGroup(PrjHandle, AlmNr, DispGroup);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindFirstAlarmTextLanguage(agl_ptrdiff_t PrjHandle, agl_int32_t* Language, agl_int32_t* IsDefault)
{
  if( Loaded(pAGLSym_FindFirstAlarmTextLanguage))
  {
    return pAGLSym_FindFirstAlarmTextLanguage(PrjHandle, Language, IsDefault);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindNextAlarmTextLanguage(agl_ptrdiff_t PrjHandle, agl_int32_t* Language, agl_int32_t* IsDefault)
{
  if( Loaded(pAGLSym_FindNextAlarmTextLanguage))
  {
    return pAGLSym_FindNextAlarmTextLanguage(PrjHandle, Language, IsDefault);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindCloseAlarmTextLanguage(agl_ptrdiff_t PrjHandle)
{
  if( Loaded(pAGLSym_FindCloseAlarmTextLanguage))
  {
    return pAGLSym_FindCloseAlarmTextLanguage(PrjHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_SetAlarmTextDefaultLanguage(agl_ptrdiff_t PrjHandle, agl_int32_t Language)
{
  if( Loaded(pAGLSym_SetAlarmTextDefaultLanguage))
  {
    return pAGLSym_SetAlarmTextDefaultLanguage(PrjHandle, Language);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmText(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t Language, agl_cstr8_t Buff, agl_int32_t BuffLen)
{
  if( Loaded(pAGLSym_GetAlarmText))
  {
    return pAGLSym_GetAlarmText(PrjHandle, AlmNr, Signal, Language, Buff, BuffLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmInfo(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t Language, agl_cstr8_t Buff, agl_int32_t BuffLen)
{
  if( Loaded(pAGLSym_GetAlarmInfo))
  {
    return pAGLSym_GetAlarmInfo(PrjHandle, AlmNr, Signal, Language, Buff, BuffLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmAddText(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t Index, agl_int32_t Language, agl_cstr8_t Buff, agl_int32_t BuffLen)
{
  if( Loaded(pAGLSym_GetAlarmAddText))
  {
    return pAGLSym_GetAlarmAddText(PrjHandle, AlmNr, Signal, Index, Language, Buff, BuffLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmSCANOperand(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* OpArea, agl_int32_t* OpType, agl_int32_t* Offset, agl_int32_t* BitNr)
{
  if( Loaded(pAGLSym_GetAlarmSCANOperand))
  {
    return pAGLSym_GetAlarmSCANOperand(PrjHandle, AlmNr, OpArea, OpType, Offset, BitNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmSCANInterval(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* Interval)
{
  if( Loaded(pAGLSym_GetAlarmSCANInterval))
  {
    return pAGLSym_GetAlarmSCANInterval(PrjHandle, AlmNr, Interval);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmSCANAddValue(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Index, agl_int32_t* OpArea, agl_int32_t* OpType, agl_int32_t* Offset, agl_int32_t* BitNr)
{
  if( Loaded(pAGLSym_GetAlarmSCANAddValue))
  {
    return pAGLSym_GetAlarmSCANAddValue(PrjHandle, AlmNr, Index, OpArea, OpType, Offset, BitNr);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmSCANOperandEx(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, LPDATA_RW40 Buff)
{
  if( Loaded(pAGLSym_GetAlarmSCANOperandEx))
  {
    return pAGLSym_GetAlarmSCANOperandEx(PrjHandle, AlmNr, Buff);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetAlarmSCANAddValueEx(agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Index, LPDATA_RW40 Buff)
{
  if( Loaded(pAGLSym_GetAlarmSCANAddValueEx))
  {
    return pAGLSym_GetAlarmSCANAddValueEx(PrjHandle, AlmNr, Index, Buff);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FormatMessage(agl_ptrdiff_t PrjHandle, const agl_cstr8_t AlarmText, LPS7_ALARM AlarmData, agl_cstr8_t Buff, agl_int32_t BuffLen)
{
  if( Loaded(pAGLSym_FormatMessage))
  {
    return pAGLSym_FormatMessage(PrjHandle, AlarmText, AlarmData, Buff, BuffLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindFirstTextlib(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Textlib, agl_int32_t* System)
{
  if( Loaded(pAGLSym_FindFirstTextlib))
  {
    return pAGLSym_FindFirstTextlib(PrjHandle, Textlib, System);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindNextTextlib(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Textlib, agl_int32_t* System)
{
  if( Loaded(pAGLSym_FindNextTextlib))
  {
    return pAGLSym_FindNextTextlib(PrjHandle, Textlib, System);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindCloseTextlib(agl_ptrdiff_t PrjHandle)
{
  if( Loaded(pAGLSym_FindCloseTextlib))
  {
    return pAGLSym_FindCloseTextlib(PrjHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_SelectTextlib(agl_ptrdiff_t PrjHandle, agl_cstr8_t  Textlib)
{
  if( Loaded(pAGLSym_SelectTextlib))
  {
    return pAGLSym_SelectTextlib(PrjHandle, Textlib);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindFirstTextlibText(agl_ptrdiff_t PrjHandle, agl_int32_t* TextId, agl_cstr8_t Buff, agl_int32_t BuffLen)
{
  if( Loaded(pAGLSym_FindFirstTextlibText))
  {
    return pAGLSym_FindFirstTextlibText(PrjHandle, TextId, Buff, BuffLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindNextTextlibText(agl_ptrdiff_t PrjHandle, agl_int32_t* TextId, agl_cstr8_t Buff, agl_int32_t BuffLen)
{
  if( Loaded(pAGLSym_FindNextTextlibText))
  {
    return pAGLSym_FindNextTextlibText(PrjHandle, TextId, Buff, BuffLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_FindCloseTextlibText(agl_ptrdiff_t PrjHandle)
{
  if( Loaded(pAGLSym_FindCloseTextlibText))
  {
    return pAGLSym_FindCloseTextlibText(PrjHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetTextlibText(agl_ptrdiff_t PrjHandle, agl_int32_t  TextId, agl_cstr8_t Buff, agl_int32_t BuffLen)
{
  if( Loaded(pAGLSym_GetTextlibText))
  {
    return pAGLSym_GetTextlibText(PrjHandle, TextId, Buff, BuffLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetTextFromValue(void* Value, agl_int32_t Format, agl_int32_t ValueFmt, agl_cstr8_t Text)
{
  if( Loaded(pAGLSym_GetTextFromValue))
  {
    return pAGLSym_GetTextFromValue(Value, Format, ValueFmt, Text);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetValueFromText(agl_cstr8_t Text, void* Value, agl_int32_t* Format, agl_int32_t* ValueFmt)
{
  if( Loaded(pAGLSym_GetValueFromText))
  {
    return pAGLSym_GetValueFromText(Text, Value, Format, ValueFmt);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetRealFromText(agl_cstr8_t Text, agl_float32_t* Value)
{
  if( Loaded(pAGLSym_GetRealFromText))
  {
    return pAGLSym_GetRealFromText(Text, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_GetTextFromReal(agl_float32_t* Value, agl_cstr8_t Text)
{
  if( Loaded(pAGLSym_GetTextFromReal))
  {
    return pAGLSym_GetTextFromReal(Value, Text);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGLSym_SetLanguage(agl_int32_t Language)
{
  if( Loaded(pAGLSym_SetLanguage))
  {
    return pAGLSym_SetLanguage(Language);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WLD_OpenFile(const agl_cstr8_t FileName, agl_int32_t Access, agl_ptrdiff_t* Handle)
{
  if( Loaded(pAGL_WLD_OpenFile))
  {
    return pAGL_WLD_OpenFile(FileName, Access, Handle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WLD_OpenFileEncrypted(const agl_cstr8_t FileName, agl_int32_t Access, agl_uint8_t* Key, agl_uint32_t Len, agl_ptrdiff_t* Handle)
{
  if( Loaded(pAGL_WLD_OpenFileEncrypted))
  {
    return pAGL_WLD_OpenFileEncrypted(FileName, Access, Key, Len, Handle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WLD_EncryptFile(const agl_cstr8_t InFileName, const agl_cstr8_t OutFileName, agl_int32_t Access, agl_uint8_t* Key, agl_uint32_t Len)
{
  if( Loaded(pAGL_WLD_EncryptFile))
  {
    return pAGL_WLD_EncryptFile(InFileName, OutFileName, Access, Key, Len);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WLD_DecryptFile(const agl_cstr8_t InFileName, const agl_cstr8_t OutFileName, agl_uint8_t* Key, agl_uint32_t Len)
{
  if( Loaded(pAGL_WLD_DecryptFile))
  {
    return pAGL_WLD_DecryptFile(InFileName, OutFileName, Key, Len);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WLD_CloseFile(agl_ptrdiff_t Handle)
{
  if( Loaded(pAGL_WLD_CloseFile))
  {
    return pAGL_WLD_CloseFile(Handle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WLD_ReadAllBlockCount(agl_ptrdiff_t Handle, LPALL_BLOCK_COUNT pBC)
{
  if( Loaded(pAGL_WLD_ReadAllBlockCount))
  {
    return pAGL_WLD_ReadAllBlockCount(Handle, pBC);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WLD_ReadBlockCount(agl_ptrdiff_t Handle, agl_int32_t BlockType, agl_int32_t* BlockCount)
{
  if( Loaded(pAGL_WLD_ReadBlockCount))
  {
    return pAGL_WLD_ReadBlockCount(Handle, BlockType, BlockCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WLD_ReadBlockList(agl_ptrdiff_t Handle, agl_int32_t BlockType, agl_int32_t* BlockCount, agl_uint16_t* BlockList)
{
  if( Loaded(pAGL_WLD_ReadBlockList))
  {
    return pAGL_WLD_ReadBlockList(Handle, BlockType, BlockCount, BlockList);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WLD_ReadBlockLen(agl_ptrdiff_t Handle, agl_int32_t BlockType, agl_int32_t BlockNr, agl_int32_t* BlockLen)
{
  if( Loaded(pAGL_WLD_ReadBlockLen))
  {
    return pAGL_WLD_ReadBlockLen(Handle, BlockType, BlockNr, BlockLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WLD_DeleteBlocks(agl_ptrdiff_t Handle, const agl_cstr8_t Blocks)
{
  if( Loaded(pAGL_WLD_DeleteBlocks))
  {
    return pAGL_WLD_DeleteBlocks(Handle, Blocks);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_WLD_GetReport(agl_ptrdiff_t Handle, agl_int32_t* Length, agl_cstr8_t Buffer)
{
  if( Loaded(pAGL_WLD_GetReport))
  {
    return pAGL_WLD_GetReport(Handle, Length, Buffer);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_PLC_Backup(agl_int32_t ConnNr, agl_ptrdiff_t Handle, const agl_cstr8_t Blocks, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_PLC_Backup))
  {
    return pAGL_PLC_Backup(ConnNr, Handle, Blocks, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_PLC_Restore(agl_int32_t ConnNr, agl_ptrdiff_t Handle, const agl_cstr8_t Blocks, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_PLC_Restore))
  {
    return pAGL_PLC_Restore(ConnNr, Handle, Blocks, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_PLC_DeleteBlocks(agl_int32_t ConnNr, const agl_cstr8_t Blocks, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_PLC_DeleteBlocks))
  {
    return pAGL_PLC_DeleteBlocks(ConnNr, Blocks, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Compress(agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Compress))
  {
    return pAGL_Compress(ConnNr, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_ReadMixEx(agl_int32_t ConnNr, SymbolicRW_t* SymbolicRW, agl_int32_t Num, agl_int32_t* SError, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Symbolic_ReadMixEx))
  {
    return pAGL_Symbolic_ReadMixEx(ConnNr, SymbolicRW, Num, SError, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_WriteMixEx(agl_int32_t ConnNr, SymbolicRW_t* SymbolicRW, agl_int32_t Num, agl_int32_t* SError, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Symbolic_WriteMixEx))
  {
    return pAGL_Symbolic_WriteMixEx(ConnNr, SymbolicRW, Num, SError, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_LoadTIAProjectSymbols(const agl_cstr8_t const ProjectFile, HandleType* const RootNodeHandle, agl_int32_t AutoExpand)
{
  if( Loaded(pAGL_Symbolic_LoadTIAProjectSymbols))
  {
    return pAGL_Symbolic_LoadTIAProjectSymbols(ProjectFile, RootNodeHandle, AutoExpand);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_LoadAGLinkSymbolsFromPLC(const int ConnNr, HandleType* const RootNodeHandle)
{
  if( Loaded(pAGL_Symbolic_LoadAGLinkSymbolsFromPLC))
  {
    return pAGL_Symbolic_LoadAGLinkSymbolsFromPLC(ConnNr, RootNodeHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SaveAGLinkSymbolsToFile(HandleType RootNodeHandle, const agl_cstr8_t const AGLinkSymbolsFile)
{
  if( Loaded(pAGL_Symbolic_SaveAGLinkSymbolsToFile))
  {
    return pAGL_Symbolic_SaveAGLinkSymbolsToFile(RootNodeHandle, AGLinkSymbolsFile);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_LoadAGLinkSymbolsFromFile(const agl_cstr8_t const AGLinkSymbolsFile, HandleType* const RootNodeHandle)
{
  if( Loaded(pAGL_Symbolic_LoadAGLinkSymbolsFromFile))
  {
    return pAGL_Symbolic_LoadAGLinkSymbolsFromFile(AGLinkSymbolsFile, RootNodeHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_FreeHandle(const HandleType Handle)
{
  if( Loaded(pAGL_Symbolic_FreeHandle))
  {
    return pAGL_Symbolic_FreeHandle(Handle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetChildCount(const HandleType NodeHandle, agl_int32_t* const ChildCount)
{
  if( Loaded(pAGL_Symbolic_GetChildCount))
  {
    return pAGL_Symbolic_GetChildCount(NodeHandle, ChildCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetChild(const HandleType NodeHandle, const agl_int32_t ChildIndex, HandleType* const ChildNodeHandle)
{
  if( Loaded(pAGL_Symbolic_GetChild))
  {
    return pAGL_Symbolic_GetChild(NodeHandle, ChildIndex, ChildNodeHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetChildByName(const HandleType NodeHandle, const agl_cstr8_t const ChildName, HandleType* const ChildNodeHandle)
{
  if( Loaded(pAGL_Symbolic_GetChildByName))
  {
    return pAGL_Symbolic_GetChildByName(NodeHandle, ChildName, ChildNodeHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetName(const HandleType NodeHandle, agl_cstr8_t const NameBuffer, const agl_int32_t NameBufferLen, agl_int32_t* const NameLen)
{
  if( Loaded(pAGL_Symbolic_GetName))
  {
    return pAGL_Symbolic_GetName(NodeHandle, NameBuffer, NameBufferLen, NameLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetLocalOffset(const HandleType NodeHandle, agl_uint32_t* const LocalByteOffset, agl_uint32_t* const LocalBitOffset)
{
  if( Loaded(pAGL_Symbolic_GetLocalOffset))
  {
    return pAGL_Symbolic_GetLocalOffset(NodeHandle, LocalByteOffset, LocalBitOffset);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetSystemType(const HandleType NodeHandle, SystemType_t::enum_t* SystemType)
{
  if( Loaded(pAGL_Symbolic_GetSystemType))
  {
    return pAGL_Symbolic_GetSystemType(NodeHandle, SystemType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetHierarchyType(const HandleType NodeHandle, HierarchyType_t::enum_t* HierarchyType)
{
  if( Loaded(pAGL_Symbolic_GetHierarchyType))
  {
    return pAGL_Symbolic_GetHierarchyType(NodeHandle, HierarchyType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetArrayDimensionCount(const HandleType ArrayNodeHandle, agl_int32_t* DimensionCount)
{
  if( Loaded(pAGL_Symbolic_GetArrayDimensionCount))
  {
    return pAGL_Symbolic_GetArrayDimensionCount(ArrayNodeHandle, DimensionCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetArrayDimension(const HandleType ArrayNodeHandle, const agl_int32_t Dimension, agl_int32_t* Lower, agl_int32_t* Upper)
{
  if( Loaded(pAGL_Symbolic_GetArrayDimension))
  {
    return pAGL_Symbolic_GetArrayDimension(ArrayNodeHandle, Dimension, Lower, Upper);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetMaxStringSize(const HandleType StringNodeHandle, agl_int32_t* const StringSize)
{
  if( Loaded(pAGL_Symbolic_GetMaxStringSize))
  {
    return pAGL_Symbolic_GetMaxStringSize(StringNodeHandle, StringSize);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetValueType(const HandleType NodeHandle, ValueType_t::enum_t* ValueType)
{
  if( Loaded(pAGL_Symbolic_GetValueType))
  {
    return pAGL_Symbolic_GetValueType(NodeHandle, ValueType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetTypeState(const HandleType NodeHandle, TypeState_t::enum_t* TypeState)
{
  if( Loaded(pAGL_Symbolic_GetTypeState))
  {
    return pAGL_Symbolic_GetTypeState(NodeHandle, TypeState);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetSegmentType(const HandleType NodeHandle, SegmentType_t::enum_t* SegementType)
{
  if( Loaded(pAGL_Symbolic_GetSegmentType))
  {
    return pAGL_Symbolic_GetSegmentType(NodeHandle, SegementType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetPermissionType(const HandleType NodeHandle, PermissionType_t::enum_t* PermissionType)
{
  if( Loaded(pAGL_Symbolic_GetPermissionType))
  {
    return pAGL_Symbolic_GetPermissionType(NodeHandle, PermissionType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_EscapeString(const agl_cstr8_t const RawString, agl_cstr8_t const EscapedString, const agl_int32_t EscapedStringMaxSize, agl_int32_t* ErrorPosition)
{
  if( Loaded(pAGL_Symbolic_EscapeString))
  {
    return pAGL_Symbolic_EscapeString(RawString, EscapedString, EscapedStringMaxSize, ErrorPosition);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetNodeByPath(const HandleType NodeHandle, const agl_cstr8_t const ItemPath, HandleType* const FoundNodeHandle, agl_int32_t* const ErrorPosition)
{
  if( Loaded(pAGL_Symbolic_GetNodeByPath))
  {
    return pAGL_Symbolic_GetNodeByPath(NodeHandle, ItemPath, FoundNodeHandle, ErrorPosition);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetIndexSize(const HandleType IndexNodeHandle, agl_size_t* IndexSize)
{
  if( Loaded(pAGL_Symbolic_GetIndexSize))
  {
    return pAGL_Symbolic_GetIndexSize(IndexNodeHandle, IndexSize);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetIndex(const HandleType IndexNodeHandle, const agl_int32_t Element, agl_int32_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetIndex))
  {
    return pAGL_Symbolic_GetIndex(IndexNodeHandle, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetLinearIndex(const HandleType IndexNodeHandle, agl_size_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetLinearIndex))
  {
    return pAGL_Symbolic_GetLinearIndex(IndexNodeHandle, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetArrayElementCount(const HandleType ArrayNodeHandle, agl_int32_t* ElementCount)
{
  if( Loaded(pAGL_Symbolic_GetArrayElementCount))
  {
    return pAGL_Symbolic_GetArrayElementCount(ArrayNodeHandle, ElementCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_Expand(const HandleType NodeHandle, const agl_int32_t Depth)
{
  if( Loaded(pAGL_Symbolic_Expand))
  {
    return pAGL_Symbolic_Expand(NodeHandle, Depth);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_Collapse(const HandleType NodeHandle)
{
  if( Loaded(pAGL_Symbolic_Collapse))
  {
    return pAGL_Symbolic_Collapse(NodeHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetSystemScope(const HandleType NodeHandle, SystemType_t::enum_t* const SystemType)
{
  if( Loaded(pAGL_Symbolic_GetSystemScope))
  {
    return pAGL_Symbolic_GetSystemScope(NodeHandle, SystemType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetSystemTypeState(const HandleType NodeHandle, SystemTypeState_t::enum_t* SystemTypeState)
{
  if( Loaded(pAGL_Symbolic_GetSystemTypeState))
  {
    return pAGL_Symbolic_GetSystemTypeState(NodeHandle, SystemTypeState);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_CreateAccess(const HandleType NodeHandle, HandleType* const AccessHandle)
{
  if( Loaded(pAGL_Symbolic_CreateAccess))
  {
    return pAGL_Symbolic_CreateAccess(NodeHandle, AccessHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_CreateAccessByPath(const HandleType ParentNodeHandle, const agl_cstr8_t const ItemPath, HandleType* const AccessHandle, agl_int32_t* const ErrorPosition)
{
  if( Loaded(pAGL_Symbolic_CreateAccessByPath))
  {
    return pAGL_Symbolic_CreateAccessByPath(ParentNodeHandle, ItemPath, AccessHandle, ErrorPosition);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_Get_DATA_RW40(const HandleType NodeHandle, DATA_RW40* const DataRW, agl_int32_t* const Size)
{
  if( Loaded(pAGL_Symbolic_Get_DATA_RW40))
  {
    return pAGL_Symbolic_Get_DATA_RW40(NodeHandle, DataRW, Size);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_Get_DATA_RW40_ByPath(const HandleType NodeHandle, const agl_cstr8_t const ItemPath, DATA_RW40* const DataRW, agl_int32_t* const ErrorPosition, agl_int32_t* const Size)
{
  if( Loaded(pAGL_Symbolic_Get_DATA_RW40_ByPath))
  {
    return pAGL_Symbolic_Get_DATA_RW40_ByPath(NodeHandle, ItemPath, DataRW, ErrorPosition, Size);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetType(const HandleType NodeHandle, HandleType* const TypeNodeHandle)
{
  if( Loaded(pAGL_Symbolic_GetType))
  {
    return pAGL_Symbolic_GetType(NodeHandle, TypeNodeHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferSize(const HandleType AccessHandle, agl_int32_t* const BufferSize)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferSize))
  {
    return pAGL_Symbolic_GetAccessBufferSize(AccessHandle, BufferSize);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferElementSize(const HandleType AccessHandle, agl_int32_t* const ElementSize)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferElementSize))
  {
    return pAGL_Symbolic_GetAccessBufferElementSize(AccessHandle, ElementSize);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessStringSize(const HandleType AccessHandle, agl_int32_t* const StringSize)
{
  if( Loaded(pAGL_Symbolic_GetAccessStringSize))
  {
    return pAGL_Symbolic_GetAccessStringSize(AccessHandle, StringSize);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferElementCount(const HandleType AccessHandle, agl_int32_t* const ElementCount)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferElementCount))
  {
    return pAGL_Symbolic_GetAccessBufferElementCount(AccessHandle, ElementCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessElementSystemType(const HandleType AccessHandle, SystemType_t::enum_t* const SystemType)
{
  if( Loaded(pAGL_Symbolic_GetAccessElementSystemType))
  {
    return pAGL_Symbolic_GetAccessElementSystemType(AccessHandle, SystemType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessElementValueType(const HandleType AccessHandle, ValueType_t::enum_t* const ValueType)
{
  if( Loaded(pAGL_Symbolic_GetAccessElementValueType))
  {
    return pAGL_Symbolic_GetAccessElementValueType(AccessHandle, ValueType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetArrayIndexAsLinearIndex(const HandleType ArrayNodeHandle, const agl_int32_t* const Index, const agl_int32_t IndexCount, agl_int32_t* const LinearIndex)
{
  if( Loaded(pAGL_Symbolic_GetArrayIndexAsLinearIndex))
  {
    return pAGL_Symbolic_GetArrayIndexAsLinearIndex(ArrayNodeHandle, Index, IndexCount, LinearIndex);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetArrayLinearIndexAsIndex(const HandleType ArrayNodeHandle, const agl_int32_t LinearIndex, agl_int32_t* const Index, const agl_int32_t MaxIndexCount, agl_int32_t* const IndexCount)
{
  if( Loaded(pAGL_Symbolic_GetArrayLinearIndexAsIndex))
  {
    return pAGL_Symbolic_GetArrayLinearIndexAsIndex(ArrayNodeHandle, LinearIndex, Index, MaxIndexCount, IndexCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_CreateArrayAccessByLinearIndex(const HandleType ArrayNodeHandle, const agl_int32_t LinearIndex, HandleType* const AccessHandle)
{
  if( Loaded(pAGL_Symbolic_CreateArrayAccessByLinearIndex))
  {
    return pAGL_Symbolic_CreateArrayAccessByLinearIndex(ArrayNodeHandle, LinearIndex, AccessHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_CreateArrayRangeAccessByLinearIndex(const HandleType ArrayNodeHandle, const agl_int32_t LinearIndex, const agl_int32_t Count, HandleType* const AccessHandle)
{
  if( Loaded(pAGL_Symbolic_CreateArrayRangeAccessByLinearIndex))
  {
    return pAGL_Symbolic_CreateArrayRangeAccessByLinearIndex(ArrayNodeHandle, LinearIndex, Count, AccessHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_CreateArrayAccessByIndex(const HandleType ArrayNodeHandle, const agl_int32_t* Index, const agl_int32_t IndexCount, HandleType* const AccessHandle)
{
  if( Loaded(pAGL_Symbolic_CreateArrayAccessByIndex))
  {
    return pAGL_Symbolic_CreateArrayAccessByIndex(ArrayNodeHandle, Index, IndexCount, AccessHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_CreateArrayRangeAccessByIndex(const HandleType ArrayNodeHandle, const agl_int32_t* Index, const agl_int32_t IndexCount, const agl_int32_t Count, HandleType* const AccessHandle)
{
  if( Loaded(pAGL_Symbolic_CreateArrayRangeAccessByIndex))
  {
    return pAGL_Symbolic_CreateArrayRangeAccessByIndex(ArrayNodeHandle, Index, IndexCount, Count, AccessHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferUInt8(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint8_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferUInt8))
  {
    return pAGL_Symbolic_GetAccessBufferUInt8(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferUInt16(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferUInt16))
  {
    return pAGL_Symbolic_GetAccessBufferUInt16(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferUInt32(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint32_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferUInt32))
  {
    return pAGL_Symbolic_GetAccessBufferUInt32(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferUInt64(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint64_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferUInt64))
  {
    return pAGL_Symbolic_GetAccessBufferUInt64(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferInt8(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int8_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferInt8))
  {
    return pAGL_Symbolic_GetAccessBufferInt8(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferInt16(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int16_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferInt16))
  {
    return pAGL_Symbolic_GetAccessBufferInt16(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferInt32(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int32_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferInt32))
  {
    return pAGL_Symbolic_GetAccessBufferInt32(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferInt64(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int64_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferInt64))
  {
    return pAGL_Symbolic_GetAccessBufferInt64(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferFloat32(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_float32_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferFloat32))
  {
    return pAGL_Symbolic_GetAccessBufferFloat32(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferFloat64(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_float64_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferFloat64))
  {
    return pAGL_Symbolic_GetAccessBufferFloat64(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferChar8(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_cstr8_t const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferChar8))
  {
    return pAGL_Symbolic_GetAccessBufferChar8(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferChar16(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_char16_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferChar16))
  {
    return pAGL_Symbolic_GetAccessBufferChar16(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferString8(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_cstr8_t const StringBuffer, agl_int32_t MaxCharCount, agl_int32_t* const CharCount)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferString8))
  {
    return pAGL_Symbolic_GetAccessBufferString8(AccessHandle, Buffer, BufferLen, Element, StringBuffer, MaxCharCount, CharCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferString16(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_char16_t* const StringBuffer, agl_int32_t MaxCharCount, agl_int32_t* const CharCount)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferString16))
  {
    return pAGL_Symbolic_GetAccessBufferString16(AccessHandle, Buffer, BufferLen, Element, StringBuffer, MaxCharCount, CharCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferS7_DTLParts(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t* const Year, agl_uint8_t* const Month, agl_uint8_t* const Day, agl_uint8_t* const WeekDay, agl_uint8_t* const Hour, agl_uint8_t* const Minute, agl_uint8_t* const Second, agl_uint32_t* const Nanoseconds)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferS7_DTLParts))
  {
    return pAGL_Symbolic_GetAccessBufferS7_DTLParts(AccessHandle, Buffer, BufferLen, Element, Year, Month, Day, WeekDay, Hour, Minute, Second, Nanoseconds);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferS7_S5TimeParts(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t* const TimeBase, agl_uint16_t* const TimeValue)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferS7_S5TimeParts))
  {
    return pAGL_Symbolic_GetAccessBufferS7_S5TimeParts(AccessHandle, Buffer, BufferLen, Element, TimeBase, TimeValue);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferS7_S5TimeMs(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint32_t* const Milliseconds)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferS7_S5TimeMs))
  {
    return pAGL_Symbolic_GetAccessBufferS7_S5TimeMs(AccessHandle, Buffer, BufferLen, Element, Milliseconds);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferS7_Counter(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t* const Value)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferS7_Counter))
  {
    return pAGL_Symbolic_GetAccessBufferS7_Counter(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferS7_Date_and_TimeParts(const HandleType AccessHandle, const void* const Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t* const Year, agl_uint8_t* const Month, agl_uint8_t* const Day, agl_uint8_t* const WeekDay, agl_uint8_t* const Hour, agl_uint8_t* const Minute, agl_uint8_t* const Second, agl_uint16_t* const Millisecond)
{
  if( Loaded(pAGL_Symbolic_GetAccessBufferS7_Date_and_TimeParts))
  {
    return pAGL_Symbolic_GetAccessBufferS7_Date_and_TimeParts(AccessHandle, Buffer, BufferLen, Element, Year, Month, Day, WeekDay, Hour, Minute, Second, Millisecond);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferUInt8(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint8_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferUInt8))
  {
    return pAGL_Symbolic_SetAccessBufferUInt8(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferUInt16(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint16_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferUInt16))
  {
    return pAGL_Symbolic_SetAccessBufferUInt16(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferUInt32(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint32_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferUInt32))
  {
    return pAGL_Symbolic_SetAccessBufferUInt32(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferUInt64(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_uint64_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferUInt64))
  {
    return pAGL_Symbolic_SetAccessBufferUInt64(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferInt8(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int8_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferInt8))
  {
    return pAGL_Symbolic_SetAccessBufferInt8(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferInt16(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int16_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferInt16))
  {
    return pAGL_Symbolic_SetAccessBufferInt16(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferInt32(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int32_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferInt32))
  {
    return pAGL_Symbolic_SetAccessBufferInt32(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferInt64(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_int64_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferInt64))
  {
    return pAGL_Symbolic_SetAccessBufferInt64(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferFloat32(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_float32_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferFloat32))
  {
    return pAGL_Symbolic_SetAccessBufferFloat32(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferFloat64(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_float64_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferFloat64))
  {
    return pAGL_Symbolic_SetAccessBufferFloat64(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferChar8(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_char8_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferChar8))
  {
    return pAGL_Symbolic_SetAccessBufferChar8(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferChar16(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, agl_char16_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferChar16))
  {
    return pAGL_Symbolic_SetAccessBufferChar16(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferString8(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_cstr8_t const StringBuffer, const agl_int32_t CharCount)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferString8))
  {
    return pAGL_Symbolic_SetAccessBufferString8(AccessHandle, Buffer, BufferLen, Element, StringBuffer, CharCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferString16(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_char16_t* const StringBuffer, const agl_int32_t CharCount)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferString16))
  {
    return pAGL_Symbolic_SetAccessBufferString16(AccessHandle, Buffer, BufferLen, Element, StringBuffer, CharCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferS7_DTLParts(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_uint16_t Year, const agl_uint8_t Month, const agl_uint8_t Day, agl_uint8_t WeekDay, const agl_uint8_t Hour, const agl_uint8_t Minute, const agl_uint8_t Second, const agl_uint32_t Nanoseconds)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferS7_DTLParts))
  {
    return pAGL_Symbolic_SetAccessBufferS7_DTLParts(AccessHandle, Buffer, BufferLen, Element, Year, Month, Day, WeekDay, Hour, Minute, Second, Nanoseconds);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferS7_S5TimeParts(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_uint16_t TimeBase, const agl_uint16_t TimeValue)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferS7_S5TimeParts))
  {
    return pAGL_Symbolic_SetAccessBufferS7_S5TimeParts(AccessHandle, Buffer, BufferLen, Element, TimeBase, TimeValue);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferS7_S5TimeMs(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_uint32_t Milliseconds, const agl_int32_t Round)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferS7_S5TimeMs))
  {
    return pAGL_Symbolic_SetAccessBufferS7_S5TimeMs(AccessHandle, Buffer, BufferLen, Element, Milliseconds, Round);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferS7_Counter(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_uint16_t Value)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferS7_Counter))
  {
    return pAGL_Symbolic_SetAccessBufferS7_Counter(AccessHandle, Buffer, BufferLen, Element, Value);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferS7_Date_and_TimeParts(const HandleType AccessHandle, void* Buffer, agl_long32_t BufferLen, agl_int32_t Element, const agl_uint16_t Year, const agl_uint8_t Month, const agl_uint8_t Day, const agl_uint8_t WeekDay, const agl_uint8_t Hour, const agl_uint8_t Minute, const agl_uint8_t Second, const agl_uint16_t Milliseconds)
{
  if( Loaded(pAGL_Symbolic_SetAccessBufferS7_Date_and_TimeParts))
  {
    return pAGL_Symbolic_SetAccessBufferS7_Date_and_TimeParts(AccessHandle, Buffer, BufferLen, Element, Year, Month, Day, WeekDay, Hour, Minute, Second, Milliseconds);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetProjectEditingCulture(const HandleType RootNodeHandle, agl_int32_t* const LCID)
{
  if( Loaded(pAGL_Symbolic_GetProjectEditingCulture))
  {
    return pAGL_Symbolic_GetProjectEditingCulture(RootNodeHandle, LCID);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetProjectReferenceCulture(const HandleType RootNodeHandle, agl_int32_t* const LCID)
{
  if( Loaded(pAGL_Symbolic_GetProjectReferenceCulture))
  {
    return pAGL_Symbolic_GetProjectReferenceCulture(RootNodeHandle, LCID);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetProjectCultureCount(const HandleType RootNodeHandle, agl_int32_t* const Count)
{
  if( Loaded(pAGL_Symbolic_GetProjectCultureCount))
  {
    return pAGL_Symbolic_GetProjectCultureCount(RootNodeHandle, Count);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetProjectCulture(const HandleType RootNodeHandle, const agl_int32_t CultureIndex, agl_int32_t* const LCID)
{
  if( Loaded(pAGL_Symbolic_GetProjectCulture))
  {
    return pAGL_Symbolic_GetProjectCulture(RootNodeHandle, CultureIndex, LCID);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetComment(const HandleType NodeHandle, const agl_int32_t LCID, agl_cstr8_t const Comment, const agl_int32_t CommentMaxSize, agl_int32_t* const UsedByteCount)
{
  if( Loaded(pAGL_Symbolic_GetComment))
  {
    return pAGL_Symbolic_GetComment(NodeHandle, LCID, Comment, CommentMaxSize, UsedByteCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetCommentCultureCount(const HandleType NodeHandle, agl_int32_t* const Count)
{
  if( Loaded(pAGL_Symbolic_GetCommentCultureCount))
  {
    return pAGL_Symbolic_GetCommentCultureCount(NodeHandle, Count);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetCommentCulture(const HandleType NodeHandle, const agl_int32_t CultureIndex, agl_int32_t* const LCID)
{
  if( Loaded(pAGL_Symbolic_GetCommentCulture))
  {
    return pAGL_Symbolic_GetCommentCulture(NodeHandle, CultureIndex, LCID);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_DatablockGetNumber(const HandleType NodeHandle, agl_int32_t* const Number)
{
  if( Loaded(pAGL_Symbolic_DatablockGetNumber))
  {
    return pAGL_Symbolic_DatablockGetNumber(NodeHandle, Number);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetPath(const HandleType NodeHandle, agl_cstr8_t const PathBuffer, const agl_int32_t MaxPathBufferSize, agl_int32_t* const UsedPathBufferSize)
{
  if( Loaded(pAGL_Symbolic_GetPath))
  {
    return pAGL_Symbolic_GetPath(NodeHandle, PathBuffer, MaxPathBufferSize, UsedPathBufferSize);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetEscapedPath(const HandleType NodeHandle, agl_cstr8_t const PathBuffer, const agl_int32_t MaxPathBufferSize, agl_int32_t* const UsedPathBufferSize)
{
  if( Loaded(pAGL_Symbolic_GetEscapedPath))
  {
    return pAGL_Symbolic_GetEscapedPath(NodeHandle, PathBuffer, MaxPathBufferSize, UsedPathBufferSize);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAttributeHMIAccessible(const HandleType NodeHandle, agl_int32_t* const BooleanValue)
{
  if( Loaded(pAGL_Symbolic_GetAttributeHMIAccessible))
  {
    return pAGL_Symbolic_GetAttributeHMIAccessible(NodeHandle, BooleanValue);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAttributeHMIVisible(const HandleType NodeHandle, agl_int32_t* const BooleanValue)
{
  if( Loaded(pAGL_Symbolic_GetAttributeHMIVisible))
  {
    return pAGL_Symbolic_GetAttributeHMIVisible(NodeHandle, BooleanValue);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetAttributeRemanent(const HandleType NodeHandle, agl_int32_t* const BooleanValue)
{
  if( Loaded(pAGL_Symbolic_GetAttributeRemanent))
  {
    return pAGL_Symbolic_GetAttributeRemanent(NodeHandle, BooleanValue);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetS7PlcTypeName(const HandleType NodeHandle, agl_cstr8_t const TypeNameBuffer, const agl_int32_t TypeNameBufferLen, agl_int32_t* const TypeNameLen)
{
  if( Loaded(pAGL_Symbolic_GetS7PlcTypeName))
  {
    return pAGL_Symbolic_GetS7PlcTypeName(NodeHandle, TypeNameBuffer, TypeNameBufferLen, TypeNameLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetS7PlcFirmware(const HandleType NodeHandle, agl_cstr8_t const FirmwareBuffer, const agl_int32_t FirmwareBufferLen, agl_int32_t* const FirmwareLen)
{
  if( Loaded(pAGL_Symbolic_GetS7PlcFirmware))
  {
    return pAGL_Symbolic_GetS7PlcFirmware(NodeHandle, FirmwareBuffer, FirmwareBufferLen, FirmwareLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetS7PlcMLFB(const HandleType NodeHandle, agl_cstr8_t const MLFBBuffer, const agl_int32_t MLFBBufferLen, agl_int32_t* const MLFBLen)
{
  if( Loaded(pAGL_Symbolic_GetS7PlcMLFB))
  {
    return pAGL_Symbolic_GetS7PlcMLFB(NodeHandle, MLFBBuffer, MLFBBufferLen, MLFBLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_GetS7PlcFamily(const HandleType NodeHandle, S7PlcFamily_t::enum_t* const S7PlcFamily)
{
  if( Loaded(pAGL_Symbolic_GetS7PlcFamily))
  {
    return pAGL_Symbolic_GetS7PlcFamily(NodeHandle, S7PlcFamily);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_SaveSingleValueAccessSymbolsToFile(const HandleType RootHandle, const agl_cstr8_t const SingleValueFilterFile, const agl_cstr8_t const LogFile, const agl_cstr8_t const AglinkSingleValueAccessSymbolFile)
{
  if( Loaded(pAGL_Symbolic_SaveSingleValueAccessSymbolsToFile))
  {
    return pAGL_Symbolic_SaveSingleValueAccessSymbolsToFile(RootHandle, SingleValueFilterFile, LogFile, AglinkSingleValueAccessSymbolFile);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_LoadSingleValueAccessSymbolsFromFile(const agl_cstr8_t const AglinkSingleValueAccessSymbolsFile, HandleType* const SingleValueAccessSymbolsHandle)
{
  if( Loaded(pAGL_Symbolic_LoadSingleValueAccessSymbolsFromFile))
  {
    return pAGL_Symbolic_LoadSingleValueAccessSymbolsFromFile(AglinkSingleValueAccessSymbolsFile, SingleValueAccessSymbolsHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Symbolic_CreateAccessFromSingleValueAccessSymbols(const HandleType SingleValueAccessSymbolsHandle, const agl_cstr8_t const Symbol, HandleType* const AccessHandle)
{
  if( Loaded(pAGL_Symbolic_CreateAccessFromSingleValueAccessSymbols))
  {
    return pAGL_Symbolic_CreateAccessFromSingleValueAccessSymbols(SingleValueAccessSymbolsHandle, Symbol, AccessHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_LoadSTISymbols(const agl_cstr8_t const STIFile, HandleType* const RootNodeHandle, agl_bool_t FlatArrays)
{
  if( Loaded(pAGL_Simotion_LoadSTISymbols))
  {
    return pAGL_Simotion_LoadSTISymbols(STIFile, RootNodeHandle, FlatArrays);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_FreeHandle(const HandleType RootNodeHandle)
{
  if( Loaded(pAGL_Simotion_FreeHandle))
  {
    return pAGL_Simotion_FreeHandle(RootNodeHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_GetName(const HandleType NodeHandle, agl_cstr8_t const NameBuffer, const agl_int32_t NameBufferLen)
{
  if( Loaded(pAGL_Simotion_GetName))
  {
    return pAGL_Simotion_GetName(NodeHandle, NameBuffer, NameBufferLen);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_GetHierarchyType(const HandleType NodeHandle, HierarchyType_t::enum_t* HierarchyType)
{
  if( Loaded(pAGL_Simotion_GetHierarchyType))
  {
    return pAGL_Simotion_GetHierarchyType(NodeHandle, HierarchyType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_GetValueType(const HandleType NodeHandle, ValueType_t::enum_t* ValueType)
{
  if( Loaded(pAGL_Simotion_GetValueType))
  {
    return pAGL_Simotion_GetValueType(NodeHandle, ValueType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_GetPermissionType(const HandleType NodeHandle, PermissionType_t::enum_t* PermissionType)
{
  if( Loaded(pAGL_Simotion_GetPermissionType))
  {
    return pAGL_Simotion_GetPermissionType(NodeHandle, PermissionType);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_GetChildCount(const HandleType NodeHandle, agl_int32_t* const ChildCount)
{
  if( Loaded(pAGL_Simotion_GetChildCount))
  {
    return pAGL_Simotion_GetChildCount(NodeHandle, ChildCount);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_GetChild(const HandleType NodeHandle, const agl_int32_t ChildIndex, HandleType* const ChildNodeHandle)
{
  if( Loaded(pAGL_Simotion_GetChild))
  {
    return pAGL_Simotion_GetChild(NodeHandle, ChildIndex, ChildNodeHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_GetNodeByPath(const HandleType NodeHandle, const agl_cstr8_t const ItemPath, HandleType* const FoundNodeHandle, agl_int32_t* const ErrorPosition)
{
  if( Loaded(pAGL_Simotion_GetNodeByPath))
  {
    return pAGL_Simotion_GetNodeByPath(NodeHandle, ItemPath, FoundNodeHandle, ErrorPosition);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_CreateAccess(const HandleType NodeHandle, HandleType* const AccessHandle)
{
  if( Loaded(pAGL_Simotion_CreateAccess))
  {
    return pAGL_Simotion_CreateAccess(NodeHandle, AccessHandle);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_CreateAccessByPath(const HandleType ParentNodeHandle, const agl_cstr8_t const ItemPath, HandleType* const AccessHandle, agl_int32_t* const ErrorPosition)
{
  if( Loaded(pAGL_Simotion_CreateAccessByPath))
  {
    return pAGL_Simotion_CreateAccessByPath(ParentNodeHandle, ItemPath, AccessHandle, ErrorPosition);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_ReadMixEx(agl_int32_t ConnNr, SymbolicRW_t* Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Simotion_ReadMixEx))
  {
    return pAGL_Simotion_ReadMixEx(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}

agl_int32_t AGL_API AGL_Simotion_WriteMixEx(agl_int32_t ConnNr, SymbolicRW_t* Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal)
{
  if( Loaded(pAGL_Simotion_WriteMixEx))
  {
    return pAGL_Simotion_WriteMixEx(ConnNr, Buff, Num, Timeout, UserVal);
  }
  return AGL40_DYN_DLL_ERROR;
}


/*******************************************************************************
Entladen der Bibliothek
*******************************************************************************/

void AGL_API AGL_UnloadDyn()
{
  if( hLib != 0 )
  {
    FreeLibrary( (HMODULE)hLib );
    hLib = 0;
  }

  pAGL_Activate = NULL;
  pAGL_GetVersion = NULL;
  pAGL_GetVersionEx = NULL;
  pAGL_GetOptions = NULL;
  pAGL_GetSerialNumber = NULL;
  pAGL_GetClientName = NULL;
  pAGL_GetMaxDevices = NULL;
  pAGL_GetMaxQueues = NULL;
  pAGL_GetMaxPLCPerDevice = NULL;
  pAGL_UseSystemTime = NULL;
  pAGL_ReturnJobNr = NULL;
  pAGL_SetBSendAutoResponse = NULL;
  pAGL_GetBSendAutoResponse = NULL;
  pAGL_GetPCCPConnNames = NULL;
  pAGL_GetPCCPProtocol = NULL;
  pAGL_GetTapiModemNames = NULL;
  pAGL_GetLocalIPAddresses = NULL;
  pAGL_GetTickCount = NULL;
  pAGL_GetMicroSecs = NULL;
  pAGL_UnloadDyn = NULL;
  pAGL_Config = NULL;
  pAGL_ConfigEx = NULL;
  pAGL_SetParas = NULL;
  pAGL_GetParas = NULL;
  pAGL_SetDevType = NULL;
  pAGL_GetDevType = NULL;
  pAGL_ReadParas = NULL;
  pAGL_WriteParas = NULL;
  pAGL_ReadDevice = NULL;
  pAGL_WriteDevice = NULL;
  pAGL_ReadParasFromFile = NULL;
  pAGL_WriteParasToFile = NULL;
  pAGL_GetParaPath = NULL;
  pAGL_SetParaPath = NULL;
  pAGL_SetAndGetParaPath = NULL;
  pAGL_GetPLCType = NULL;
  pAGL_HasFunc = NULL;
  pAGL_LoadErrorFile = NULL;
  pAGL_GetErrorMsg = NULL;
  pAGL_GetErrorCodeName = NULL;
  pAGL_OpenDevice = NULL;
  pAGL_CloseDevice = NULL;
  pAGL_SetDevNotification = NULL;
  pAGL_SetConnNotification = NULL;
  pAGL_SetJobNotification = NULL;
  pAGL_SetJobNotificationEx = NULL;
  pAGL_GetJobResult = NULL;
  pAGL_GetLastJobResult = NULL;
  pAGL_DeleteJob = NULL;
  pAGL_WaitForJob = NULL;
  pAGL_WaitForJobEx = NULL;
  pAGL_DialUp = NULL;
  pAGL_HangUp = NULL;
  pAGL_InitAdapter = NULL;
  pAGL_ExitAdapter = NULL;
  pAGL_GetLifelist = NULL;
  pAGL_GetDirectPLC = NULL;
  pAGL_PLCConnect = NULL;
  pAGL_PLCConnectEx = NULL;
  pAGL_PLCDisconnect = NULL;
  pAGL_ReadMaxPacketSize = NULL;
  pAGL_GetRedConnState = NULL;
  pAGL_GetRedConnStateMsg = NULL;
  pAGL_ReadOpState = NULL;
  pAGL_ReadOpStateEx = NULL;
  pAGL_GetPLCStartOptions = NULL;
  pAGL_PLCStop = NULL;
  pAGL_PLCStart = NULL;
  pAGL_PLCResume = NULL;
  pAGL_PLCColdStart = NULL;
  pAGL_IsHPLC = NULL;
  pAGL_HPLCStop = NULL;
  pAGL_HPLCStart = NULL;
  pAGL_HPLCColdStart = NULL;
  pAGL_GetPLCClock = NULL;
  pAGL_SetPLCClock = NULL;
  pAGL_SyncPLCClock = NULL;
  pAGL_ReadMLFBNr = NULL;
  pAGL_ReadMLFBNrEx = NULL;
  pAGL_ReadPLCInfo = NULL;
  pAGL_ReadCycleTime = NULL;
  pAGL_ReadProtLevel = NULL;
  pAGL_ReadS7Ident = NULL;
  pAGL_ReadS7LED = NULL;
  pAGL_GetExtModuleInfo = NULL;
  pAGL_ReadSzl = NULL;
  pAGL_IsPasswordReq = NULL;
  pAGL_SetPassword = NULL;
  pAGL_UnSetPassword = NULL;
  pAGL_ReadDiagBufferEntrys = NULL;
  pAGL_ReadDiagBuffer = NULL;
  pAGL_GetDiagBufferEntry = NULL;
  pAGL_ReadDBCount = NULL;
  pAGL_ReadDBList = NULL;
  pAGL_ReadDBLen = NULL;
  pAGL_ReadAllBlockCount = NULL;
  pAGL_ReadBlockCount = NULL;
  pAGL_ReadBlockList = NULL;
  pAGL_ReadBlockLen = NULL;
  pAGL_ReadInBytes = NULL;
  pAGL_ReadPInBytes = NULL;
  pAGL_ReadOutBytes = NULL;
  pAGL_ReadFlagBytes = NULL;
  pAGL_ReadSFlagBytes = NULL;
  pAGL_ReadVarBytes = NULL;
  pAGL_ReadDataBytes = NULL;
  pAGL_ReadDataWords = NULL;
  pAGL_ReadTimerWords = NULL;
  pAGL_ReadCounterWords = NULL;
  pAGL_ReadMix = NULL;
  pAGL_ReadMixEx = NULL;
  pAGL_WriteInBytes = NULL;
  pAGL_WriteOutBytes = NULL;
  pAGL_WritePOutBytes = NULL;
  pAGL_WriteFlagBytes = NULL;
  pAGL_WriteSFlagBytes = NULL;
  pAGL_WriteVarBytes = NULL;
  pAGL_WriteDataBytes = NULL;
  pAGL_WriteDataWords = NULL;
  pAGL_WriteTimerWords = NULL;
  pAGL_WriteCounterWords = NULL;
  pAGL_WriteMix = NULL;
  pAGL_WriteMixEx = NULL;
  pAGL_InitOptReadMix = NULL;
  pAGL_ReadOptReadMix = NULL;
  pAGL_EndOptReadMix = NULL;
  pAGL_InitOptReadMixEx = NULL;
  pAGL_ReadOptReadMixEx = NULL;
  pAGL_EndOptReadMixEx = NULL;
  pAGL_InitOptWriteMix = NULL;
  pAGL_WriteOptWriteMix = NULL;
  pAGL_EndOptWriteMix = NULL;
  pAGL_InitOptWriteMixEx = NULL;
  pAGL_WriteOptWriteMixEx = NULL;
  pAGL_EndOptWriteMixEx = NULL;
  pAGL_SetOptNotification = NULL;
  pAGL_DeleteOptJob = NULL;
  pAGL_GetOptJobResult = NULL;
  pAGL_WaitForOptJob = NULL;
  pAGL_AllocRWBuffs = NULL;
  pAGL_FreeRWBuffs = NULL;
  pAGL_ReadRWBuff = NULL;
  pAGL_WriteRWBuff = NULL;
  pAGL_RKSend = NULL;
  pAGL_RKSendEx = NULL;
  pAGL_RKFetch = NULL;
  pAGL_RKFetchEx = NULL;
  pAGL_Send_RKFetch = NULL;
  pAGL_Recv_RKSend = NULL;
  pAGL_Recv_RKFetch = NULL;
  pAGL_Send_3964 = NULL;
  pAGL_Recv_3964 = NULL;
  pAGL_BSend = NULL;
  pAGL_BReceive = NULL;
  pAGL_BSendEx = NULL;
  pAGL_BReceiveEx = NULL;
  pAGL_USend = NULL;
  pAGL_UReceive = NULL;
  pAGL_InitOpStateMsg = NULL;
  pAGL_ExitOpStateMsg = NULL;
  pAGL_GetOpStateMsg = NULL;
  pAGL_InitDiagMsg = NULL;
  pAGL_ExitDiagMsg = NULL;
  pAGL_GetDiagMsg = NULL;
  pAGL_InitCyclicRead = NULL;
  pAGL_InitCyclicReadEx = NULL;
  pAGL_StartCyclicRead = NULL;
  pAGL_StopCyclicRead = NULL;
  pAGL_ExitCyclicRead = NULL;
  pAGL_GetCyclicRead = NULL;
  pAGL_GetCyclicReadEx = NULL;
  pAGL_InitScanMsg = NULL;
  pAGL_ExitScanMsg = NULL;
  pAGL_GetScanMsg = NULL;
  pAGL_HasAckTriggeredMsg = NULL;
  pAGL_InitAlarmMsg = NULL;
  pAGL_ExitAlarmMsg = NULL;
  pAGL_GetAlarmMsg = NULL;
  pAGL_ReadOpenMsg = NULL;
  pAGL_GetMsgStateChange = NULL;
  pAGL_AckMsg = NULL;
  pAGL_LockMsg = NULL;
  pAGL_UnlockMsg = NULL;
  pAGL_InitARSend = NULL;
  pAGL_ExitARSend = NULL;
  pAGL_GetARSend = NULL;
  pAGL_RFC1006_Connect = NULL;
  pAGL_RFC1006_Disconnect = NULL;
  pAGL_RFC1006_Receive = NULL;
  pAGL_RFC1006_Send = NULL;
  pAGL_NCK_ReadMixEx = NULL;
  pAGL_NCK_WriteMixEx = NULL;
  pAGL_NCK_CheckVarSize = NULL;
  pAGL_NCK_InitCyclicReadEx = NULL;
  pAGL_NCK_StartCyclicRead = NULL;
  pAGL_NCK_StopCyclicRead = NULL;
  pAGL_NCK_ExitCyclicRead = NULL;
  pAGL_NCK_GetCyclicReadEx = NULL;
  pAGL_NCK_PI_EXTERN = NULL;
  pAGL_NCK_PI_EXTMOD = NULL;
  pAGL_NCK_PI_SELECT = NULL;
  pAGL_NCK_PI_F_DELE = NULL;
  pAGL_NCK_PI_F_PROT = NULL;
  pAGL_NCK_PI_F_RENA = NULL;
  pAGL_NCK_PI_F_XFER = NULL;
  pAGL_NCK_PI_LOGIN = NULL;
  pAGL_NCK_PI_LOGOUT = NULL;
  pAGL_NCK_PI_F_OPEN = NULL;
  pAGL_NCK_PI_F_OPER = NULL;
  pAGL_NCK_PI_F_SEEK = NULL;
  pAGL_NCK_PI_F_CLOS = NULL;
  pAGL_NCK_PI_StartAll = NULL;
  pAGL_NCK_PI_F_COPY = NULL;
  pAGL_NCK_PI_F_PROR = NULL;
  pAGL_NCK_PI_CANCEL = NULL;
  pAGL_NCK_PI_CRCEDN = NULL;
  pAGL_NCK_PI_DELECE = NULL;
  pAGL_NCK_PI_DELETO = NULL;
  pAGL_NCK_PI_IBN_SS = NULL;
  pAGL_NCK_PI_MMCSEM = NULL;
  pAGL_NCK_PI_TMCRTO = NULL;
  pAGL_NCK_PI_TMMVTL = NULL;
  pAGL_NCK_PI_TMCRTC = NULL;
  pAGL_NCK_PI_CREATO = NULL;
  pAGL_NCK_CopyFileToNC = NULL;
  pAGL_NCK_CopyFileFromNC = NULL;
  pAGL_NCK_CopyToNC = NULL;
  pAGL_NCK_CopyFromNC = NULL;
  pAGL_NCK_CopyFromNCAlloc = NULL;
  pAGL_NCK_FreeBuff = NULL;
  pAGL_NCK_SetConnProgressNotification = NULL;
  pAGL_NCK_CheckNSKVarLine = NULL;
  pAGL_NCK_ReadNSKVarFile = NULL;
  pAGL_NCK_CheckCSVVarLine = NULL;
  pAGL_NCK_ReadCSVVarFile = NULL;
  pAGL_NCK_ReadGUDVarFile = NULL;
  pAGL_NCK_ReadGUDVarFileEx = NULL;
  pAGL_NCK_FreeVarBuff = NULL;
  pAGL_NCK_GetSingleVarDef = NULL;
  pAGL_NCK_ExtractNckAlarm = NULL;
  pAGL_NCK_GetNCKDataRWByNCDDEItem = NULL;
  pAGL_Drive_ReadMix = NULL;
  pAGL_Drive_ReadMixEx = NULL;
  pAGL_Drive_WriteMix = NULL;
  pAGL_Drive_WriteMixEx = NULL;
  pAGL_malloc = NULL;
  pAGL_calloc = NULL;
  pAGL_realloc = NULL;
  pAGL_memcpy = NULL;
  pAGL_memmove = NULL;
  pAGL_memcmp = NULL;
  pAGL_free = NULL;
  pAGL_ReadInt16 = NULL;
  pAGL_ReadInt32 = NULL;
  pAGL_ReadWord = NULL;
  pAGL_ReadDWord = NULL;
  pAGL_ReadReal = NULL;
  pAGL_ReadS5Time = NULL;
  pAGL_ReadS5TimeW = NULL;
  pAGL_WriteInt16 = NULL;
  pAGL_WriteInt32 = NULL;
  pAGL_WriteWord = NULL;
  pAGL_WriteDWord = NULL;
  pAGL_WriteReal = NULL;
  pAGL_WriteS5Time = NULL;
  pAGL_WriteS5TimeW = NULL;
  pAGL_Byte2Word = NULL;
  pAGL_Byte2DWord = NULL;
  pAGL_Byte2Real = NULL;
  pAGL_Word2Byte = NULL;
  pAGL_DWord2Byte = NULL;
  pAGL_Real2Byte = NULL;
  pAGL_GetBit = NULL;
  pAGL_SetBit = NULL;
  pAGL_ResetBit = NULL;
  pAGL_SetBitVal = NULL;
  pAGL_Buff2String = NULL;
  pAGL_String2Buff = NULL;
  pAGL_Buff2WString = NULL;
  pAGL_WString2Buff = NULL;
  pAGL_S7String2String = NULL;
  pAGL_String2S7String = NULL;
  pAGL_BCD2Int16 = NULL;
  pAGL_BCD2Int32 = NULL;
  pAGL_Int162BCD = NULL;
  pAGL_Int322BCD = NULL;
  pAGL_LongAsFloat = NULL;
  pAGL_FloatAsLong = NULL;
  pAGL_Text2DataRW = NULL;
  pAGL_DataRW2Text = NULL;
  pAGL_S7DT2SysTime = NULL;
  pAGL_SysTime2S7DT = NULL;
  pAGL_TOD2SysTime = NULL;
  pAGL_SysTime2TOD = NULL;
  pAGL_S7Date2YMD = NULL;
  pAGL_Float2KG = NULL;
  pAGL_KG2Float = NULL;
  pAGL_Float2DWKG = NULL;
  pAGL_DWKG2Float = NULL;
  pAGL_S7Ident2String = NULL;
  pAGLSym_OpenProject = NULL;
  pAGLSym_CloseProject = NULL;
  pAGLSym_WriteCpuListToFile = NULL;
  pAGLSym_GetProgramCount = NULL;
  pAGLSym_FindFirstProgram = NULL;
  pAGLSym_FindNextProgram = NULL;
  pAGLSym_FindCloseProgram = NULL;
  pAGLSym_SelectProgram = NULL;
  pAGLSym_GetSymbolCount = NULL;
  pAGLSym_GetSymbolCountFilter = NULL;
  pAGLSym_FindFirstSymbol = NULL;
  pAGLSym_FindFirstSymbolFilter = NULL;
  pAGLSym_FindNextSymbol = NULL;
  pAGLSym_FindCloseSymbol = NULL;
  pAGLSym_ReadPrjDBCount = NULL;
  pAGLSym_ReadPrjDBCountFilter = NULL;
  pAGLSym_ReadPrjDBList = NULL;
  pAGLSym_ReadPrjDBListFilter = NULL;
  pAGLSym_ReadPrjBlkCountFilter = NULL;
  pAGLSym_ReadPrjBlkListFilter = NULL;
  pAGLSym_GetDbSymbolCount = NULL;
  pAGLSym_GetDbSymbolCountFilter = NULL;
  pAGLSym_FindFirstDbSymbol = NULL;
  pAGLSym_FindFirstDbSymbolFilter = NULL;
  pAGLSym_FindNextDbSymbol = NULL;
  pAGLSym_FindFirstDbSymbolEx = NULL;
  pAGLSym_FindNextDbSymbolEx = NULL;
  pAGLSym_GetDbSymbolExComment = NULL;
  pAGLSym_FindCloseDbSymbol = NULL;
  pAGLSym_GetDbDependency = NULL;
  pAGLSym_GetDeclarationCountFilter = NULL;
  pAGLSym_FindFirstDeclarationFilter = NULL;
  pAGLSym_FindNextDeclaration = NULL;
  pAGLSym_GetDeclarationInitialValue = NULL;
  pAGLSym_FindCloseDeclaration = NULL;
  pAGLSym_GetSymbolFromText = NULL;
  pAGLSym_GetSymbolFromTextEx = NULL;
  pAGLSym_GetReadMixFromText = NULL;
  pAGLSym_GetReadMixFromTextEx = NULL;
  pAGLSym_GetSymbol = NULL;
  pAGLSym_GetSymbolEx = NULL;
  pAGLSym_OpenAlarms = NULL;
  pAGLSym_CloseAlarms = NULL;
  pAGLSym_FindFirstAlarmData = NULL;
  pAGLSym_FindNextAlarmData = NULL;
  pAGLSym_FindCloseAlarmData = NULL;
  pAGLSym_GetAlarmData = NULL;
  pAGLSym_GetAlarmName = NULL;
  pAGLSym_GetAlarmType = NULL;
  pAGLSym_GetAlarmBaseName = NULL;
  pAGLSym_GetAlarmTypeName = NULL;
  pAGLSym_GetAlarmSignalCount = NULL;
  pAGLSym_GetAlarmMsgClass = NULL;
  pAGLSym_GetAlarmPriority = NULL;
  pAGLSym_GetAlarmAckGroup = NULL;
  pAGLSym_GetAlarmAcknowledge = NULL;
  pAGLSym_GetAlarmProtocol = NULL;
  pAGLSym_GetAlarmDispGroup = NULL;
  pAGLSym_FindFirstAlarmTextLanguage = NULL;
  pAGLSym_FindNextAlarmTextLanguage = NULL;
  pAGLSym_FindCloseAlarmTextLanguage = NULL;
  pAGLSym_SetAlarmTextDefaultLanguage = NULL;
  pAGLSym_GetAlarmText = NULL;
  pAGLSym_GetAlarmInfo = NULL;
  pAGLSym_GetAlarmAddText = NULL;
  pAGLSym_GetAlarmSCANOperand = NULL;
  pAGLSym_GetAlarmSCANInterval = NULL;
  pAGLSym_GetAlarmSCANAddValue = NULL;
  pAGLSym_GetAlarmSCANOperandEx = NULL;
  pAGLSym_GetAlarmSCANAddValueEx = NULL;
  pAGLSym_FormatMessage = NULL;
  pAGLSym_FindFirstTextlib = NULL;
  pAGLSym_FindNextTextlib = NULL;
  pAGLSym_FindCloseTextlib = NULL;
  pAGLSym_SelectTextlib = NULL;
  pAGLSym_FindFirstTextlibText = NULL;
  pAGLSym_FindNextTextlibText = NULL;
  pAGLSym_FindCloseTextlibText = NULL;
  pAGLSym_GetTextlibText = NULL;
  pAGLSym_GetTextFromValue = NULL;
  pAGLSym_GetValueFromText = NULL;
  pAGLSym_GetRealFromText = NULL;
  pAGLSym_GetTextFromReal = NULL;
  pAGLSym_SetLanguage = NULL;
  pAGL_WLD_OpenFile = NULL;
  pAGL_WLD_OpenFileEncrypted = NULL;
  pAGL_WLD_EncryptFile = NULL;
  pAGL_WLD_DecryptFile = NULL;
  pAGL_WLD_CloseFile = NULL;
  pAGL_WLD_ReadAllBlockCount = NULL;
  pAGL_WLD_ReadBlockCount = NULL;
  pAGL_WLD_ReadBlockList = NULL;
  pAGL_WLD_ReadBlockLen = NULL;
  pAGL_WLD_DeleteBlocks = NULL;
  pAGL_WLD_GetReport = NULL;
  pAGL_PLC_Backup = NULL;
  pAGL_PLC_Restore = NULL;
  pAGL_PLC_DeleteBlocks = NULL;
  pAGL_Compress = NULL;
  pAGL_Symbolic_ReadMixEx = NULL;
  pAGL_Symbolic_WriteMixEx = NULL;
  pAGL_Symbolic_LoadTIAProjectSymbols = NULL;
  pAGL_Symbolic_LoadAGLinkSymbolsFromPLC = NULL;
  pAGL_Symbolic_SaveAGLinkSymbolsToFile = NULL;
  pAGL_Symbolic_LoadAGLinkSymbolsFromFile = NULL;
  pAGL_Symbolic_FreeHandle = NULL;
  pAGL_Symbolic_GetChildCount = NULL;
  pAGL_Symbolic_GetChild = NULL;
  pAGL_Symbolic_GetChildByName = NULL;
  pAGL_Symbolic_GetName = NULL;
  pAGL_Symbolic_GetLocalOffset = NULL;
  pAGL_Symbolic_GetSystemType = NULL;
  pAGL_Symbolic_GetHierarchyType = NULL;
  pAGL_Symbolic_GetArrayDimensionCount = NULL;
  pAGL_Symbolic_GetArrayDimension = NULL;
  pAGL_Symbolic_GetMaxStringSize = NULL;
  pAGL_Symbolic_GetValueType = NULL;
  pAGL_Symbolic_GetTypeState = NULL;
  pAGL_Symbolic_GetSegmentType = NULL;
  pAGL_Symbolic_GetPermissionType = NULL;
  pAGL_Symbolic_EscapeString = NULL;
  pAGL_Symbolic_GetNodeByPath = NULL;
  pAGL_Symbolic_GetIndexSize = NULL;
  pAGL_Symbolic_GetIndex = NULL;
  pAGL_Symbolic_GetLinearIndex = NULL;
  pAGL_Symbolic_GetArrayElementCount = NULL;
  pAGL_Symbolic_Expand = NULL;
  pAGL_Symbolic_Collapse = NULL;
  pAGL_Symbolic_GetSystemScope = NULL;
  pAGL_Symbolic_GetSystemTypeState = NULL;
  pAGL_Symbolic_CreateAccess = NULL;
  pAGL_Symbolic_CreateAccessByPath = NULL;
  pAGL_Symbolic_Get_DATA_RW40 = NULL;
  pAGL_Symbolic_Get_DATA_RW40_ByPath = NULL;
  pAGL_Symbolic_GetType = NULL;
  pAGL_Symbolic_GetAccessBufferSize = NULL;
  pAGL_Symbolic_GetAccessBufferElementSize = NULL;
  pAGL_Symbolic_GetAccessStringSize = NULL;
  pAGL_Symbolic_GetAccessBufferElementCount = NULL;
  pAGL_Symbolic_GetAccessElementSystemType = NULL;
  pAGL_Symbolic_GetAccessElementValueType = NULL;
  pAGL_Symbolic_GetArrayIndexAsLinearIndex = NULL;
  pAGL_Symbolic_GetArrayLinearIndexAsIndex = NULL;
  pAGL_Symbolic_CreateArrayAccessByLinearIndex = NULL;
  pAGL_Symbolic_CreateArrayRangeAccessByLinearIndex = NULL;
  pAGL_Symbolic_CreateArrayAccessByIndex = NULL;
  pAGL_Symbolic_CreateArrayRangeAccessByIndex = NULL;
  pAGL_Symbolic_GetAccessBufferUInt8 = NULL;
  pAGL_Symbolic_GetAccessBufferUInt16 = NULL;
  pAGL_Symbolic_GetAccessBufferUInt32 = NULL;
  pAGL_Symbolic_GetAccessBufferUInt64 = NULL;
  pAGL_Symbolic_GetAccessBufferInt8 = NULL;
  pAGL_Symbolic_GetAccessBufferInt16 = NULL;
  pAGL_Symbolic_GetAccessBufferInt32 = NULL;
  pAGL_Symbolic_GetAccessBufferInt64 = NULL;
  pAGL_Symbolic_GetAccessBufferFloat32 = NULL;
  pAGL_Symbolic_GetAccessBufferFloat64 = NULL;
  pAGL_Symbolic_GetAccessBufferChar8 = NULL;
  pAGL_Symbolic_GetAccessBufferChar16 = NULL;
  pAGL_Symbolic_GetAccessBufferString8 = NULL;
  pAGL_Symbolic_GetAccessBufferString16 = NULL;
  pAGL_Symbolic_GetAccessBufferS7_DTLParts = NULL;
  pAGL_Symbolic_GetAccessBufferS7_S5TimeParts = NULL;
  pAGL_Symbolic_GetAccessBufferS7_S5TimeMs = NULL;
  pAGL_Symbolic_GetAccessBufferS7_Counter = NULL;
  pAGL_Symbolic_GetAccessBufferS7_Date_and_TimeParts = NULL;
  pAGL_Symbolic_SetAccessBufferUInt8 = NULL;
  pAGL_Symbolic_SetAccessBufferUInt16 = NULL;
  pAGL_Symbolic_SetAccessBufferUInt32 = NULL;
  pAGL_Symbolic_SetAccessBufferUInt64 = NULL;
  pAGL_Symbolic_SetAccessBufferInt8 = NULL;
  pAGL_Symbolic_SetAccessBufferInt16 = NULL;
  pAGL_Symbolic_SetAccessBufferInt32 = NULL;
  pAGL_Symbolic_SetAccessBufferInt64 = NULL;
  pAGL_Symbolic_SetAccessBufferFloat32 = NULL;
  pAGL_Symbolic_SetAccessBufferFloat64 = NULL;
  pAGL_Symbolic_SetAccessBufferChar8 = NULL;
  pAGL_Symbolic_SetAccessBufferChar16 = NULL;
  pAGL_Symbolic_SetAccessBufferString8 = NULL;
  pAGL_Symbolic_SetAccessBufferString16 = NULL;
  pAGL_Symbolic_SetAccessBufferS7_DTLParts = NULL;
  pAGL_Symbolic_SetAccessBufferS7_S5TimeParts = NULL;
  pAGL_Symbolic_SetAccessBufferS7_S5TimeMs = NULL;
  pAGL_Symbolic_SetAccessBufferS7_Counter = NULL;
  pAGL_Symbolic_SetAccessBufferS7_Date_and_TimeParts = NULL;
  pAGL_Symbolic_GetProjectEditingCulture = NULL;
  pAGL_Symbolic_GetProjectReferenceCulture = NULL;
  pAGL_Symbolic_GetProjectCultureCount = NULL;
  pAGL_Symbolic_GetProjectCulture = NULL;
  pAGL_Symbolic_GetComment = NULL;
  pAGL_Symbolic_GetCommentCultureCount = NULL;
  pAGL_Symbolic_GetCommentCulture = NULL;
  pAGL_Symbolic_DatablockGetNumber = NULL;
  pAGL_Symbolic_GetPath = NULL;
  pAGL_Symbolic_GetEscapedPath = NULL;
  pAGL_Symbolic_GetAttributeHMIAccessible = NULL;
  pAGL_Symbolic_GetAttributeHMIVisible = NULL;
  pAGL_Symbolic_GetAttributeRemanent = NULL;
  pAGL_Symbolic_GetS7PlcTypeName = NULL;
  pAGL_Symbolic_GetS7PlcFirmware = NULL;
  pAGL_Symbolic_GetS7PlcMLFB = NULL;
  pAGL_Symbolic_GetS7PlcFamily = NULL;
  pAGL_Symbolic_SaveSingleValueAccessSymbolsToFile = NULL;
  pAGL_Symbolic_LoadSingleValueAccessSymbolsFromFile = NULL;
  pAGL_Symbolic_CreateAccessFromSingleValueAccessSymbols = NULL;
  pAGL_Simotion_LoadSTISymbols = NULL;
  pAGL_Simotion_FreeHandle = NULL;
  pAGL_Simotion_GetName = NULL;
  pAGL_Simotion_GetHierarchyType = NULL;
  pAGL_Simotion_GetValueType = NULL;
  pAGL_Simotion_GetPermissionType = NULL;
  pAGL_Simotion_GetChildCount = NULL;
  pAGL_Simotion_GetChild = NULL;
  pAGL_Simotion_GetNodeByPath = NULL;
  pAGL_Simotion_CreateAccess = NULL;
  pAGL_Simotion_CreateAccessByPath = NULL;
  pAGL_Simotion_ReadMixEx = NULL;
  pAGL_Simotion_WriteMixEx = NULL;
}

/*******************************************************************************
Laden der Bibliothek und Abbilden auf die dort enthaltenen Funktionen
*******************************************************************************/

agl_int32_t LoadDllByPath( const agl_char8_t* const p_path)
{
#if defined( __LINUX__ )
  hLib = LoadLibrary( p_path );
#else
  hLib = LoadLibraryA( p_path );
#endif
  return LoadFunctions();
}

agl_int32_t LoadDll()
{
#if defined( __WINCE__ )
  hLib = LoadLibrary( _T("AGLink40_CE.DLL"));
#elif defined( __WIN64__ )
  hLib = LoadLibraryA( "AGLink40_x64.DLL" );
#elif defined( __LINUX__ )
  #if __WORDSIZE == 32
    #if defined( USB_INTERFACE )
      hLib = LoadLibrary( "libAGLink40_usb.so" );
    #else
      hLib = LoadLibrary( "libAGLink40.so" );
    #endif
  #elif __WORDSIZE == 64
    #if defined( USB_INTERFACE )
      hLib = LoadLibrary( "libAGLink40_x64_usb.so" );
    #else
      hLib = LoadLibrary( "libAGLink40_x64.so" );
    #endif
  #endif
#else
  hLib = LoadLibraryA( "AGLink40.DLL" );
#endif
   return LoadFunctions();
}

agl_int32_t LoadFunctions() {
  if( hLib == NULL )
  {
    return( -1 );
  }

  pAGL_Activate = (LPFN_AGL_Activate)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Activate") );
  pAGL_GetVersion = (LPFN_AGL_GetVersion)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetVersion") );
  pAGL_GetVersionEx = (LPFN_AGL_GetVersionEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetVersionEx") );
  pAGL_GetOptions = (LPFN_AGL_GetOptions)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetOptions") );
  pAGL_GetSerialNumber = (LPFN_AGL_GetSerialNumber)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetSerialNumber") );
  pAGL_GetClientName = (LPFN_AGL_GetClientName)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetClientName") );
  pAGL_GetMaxDevices = (LPFN_AGL_GetMaxDevices)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetMaxDevices") );
  pAGL_GetMaxQueues = (LPFN_AGL_GetMaxQueues)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetMaxQueues") );
  pAGL_GetMaxPLCPerDevice = (LPFN_AGL_GetMaxPLCPerDevice)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetMaxPLCPerDevice") );
  pAGL_UseSystemTime = (LPFN_AGL_UseSystemTime)GetProcAddress( (HMODULE)hLib, __DLT("AGL_UseSystemTime") );
  pAGL_ReturnJobNr = (LPFN_AGL_ReturnJobNr)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReturnJobNr") );
  pAGL_SetBSendAutoResponse = (LPFN_AGL_SetBSendAutoResponse)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetBSendAutoResponse") );
  pAGL_GetBSendAutoResponse = (LPFN_AGL_GetBSendAutoResponse)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetBSendAutoResponse") );
  pAGL_GetPCCPConnNames = (LPFN_AGL_GetPCCPConnNames)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetPCCPConnNames") );
  pAGL_GetPCCPProtocol = (LPFN_AGL_GetPCCPProtocol)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetPCCPProtocol") );
  pAGL_GetTapiModemNames = (LPFN_AGL_GetTapiModemNames)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetTapiModemNames") );
  pAGL_GetLocalIPAddresses = (LPFN_AGL_GetLocalIPAddresses)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetLocalIPAddresses") );
  pAGL_GetTickCount = (LPFN_AGL_GetTickCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetTickCount") );
  pAGL_GetMicroSecs = (LPFN_AGL_GetMicroSecs)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetMicroSecs") );
  pAGL_UnloadDyn = (LPFN_AGL_UnloadDyn)GetProcAddress( (HMODULE)hLib, __DLT("AGL_UnloadDyn") );
  pAGL_Config = (LPFN_AGL_Config)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Config") );
  pAGL_ConfigEx = (LPFN_AGL_ConfigEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ConfigEx") );
  pAGL_SetParas = (LPFN_AGL_SetParas)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetParas") );
  pAGL_GetParas = (LPFN_AGL_GetParas)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetParas") );
  pAGL_SetDevType = (LPFN_AGL_SetDevType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetDevType") );
  pAGL_GetDevType = (LPFN_AGL_GetDevType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetDevType") );
  pAGL_ReadParas = (LPFN_AGL_ReadParas)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadParas") );
  pAGL_WriteParas = (LPFN_AGL_WriteParas)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteParas") );
  pAGL_ReadDevice = (LPFN_AGL_ReadDevice)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadDevice") );
  pAGL_WriteDevice = (LPFN_AGL_WriteDevice)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteDevice") );
  pAGL_ReadParasFromFile = (LPFN_AGL_ReadParasFromFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadParasFromFile") );
  pAGL_WriteParasToFile = (LPFN_AGL_WriteParasToFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteParasToFile") );
  pAGL_GetParaPath = (LPFN_AGL_GetParaPath)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetParaPath") );
  pAGL_SetParaPath = (LPFN_AGL_SetParaPath)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetParaPath") );
  pAGL_SetAndGetParaPath = (LPFN_AGL_SetAndGetParaPath)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetAndGetParaPath") );
  pAGL_GetPLCType = (LPFN_AGL_GetPLCType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetPLCType") );
  pAGL_HasFunc = (LPFN_AGL_HasFunc)GetProcAddress( (HMODULE)hLib, __DLT("AGL_HasFunc") );
  pAGL_LoadErrorFile = (LPFN_AGL_LoadErrorFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_LoadErrorFile") );
  pAGL_GetErrorMsg = (LPFN_AGL_GetErrorMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetErrorMsg") );
  pAGL_GetErrorCodeName = (LPFN_AGL_GetErrorCodeName)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetErrorCodeName") );
  pAGL_OpenDevice = (LPFN_AGL_OpenDevice)GetProcAddress( (HMODULE)hLib, __DLT("AGL_OpenDevice") );
  pAGL_CloseDevice = (LPFN_AGL_CloseDevice)GetProcAddress( (HMODULE)hLib, __DLT("AGL_CloseDevice") );
  pAGL_SetDevNotification = (LPFN_AGL_SetDevNotification)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetDevNotification") );
  pAGL_SetConnNotification = (LPFN_AGL_SetConnNotification)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetConnNotification") );
  pAGL_SetJobNotification = (LPFN_AGL_SetJobNotification)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetJobNotification") );
  pAGL_SetJobNotificationEx = (LPFN_AGL_SetJobNotificationEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetJobNotificationEx") );
  pAGL_GetJobResult = (LPFN_AGL_GetJobResult)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetJobResult") );
  pAGL_GetLastJobResult = (LPFN_AGL_GetLastJobResult)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetLastJobResult") );
  pAGL_DeleteJob = (LPFN_AGL_DeleteJob)GetProcAddress( (HMODULE)hLib, __DLT("AGL_DeleteJob") );
  pAGL_WaitForJob = (LPFN_AGL_WaitForJob)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WaitForJob") );
  pAGL_WaitForJobEx = (LPFN_AGL_WaitForJobEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WaitForJobEx") );
  pAGL_DialUp = (LPFN_AGL_DialUp)GetProcAddress( (HMODULE)hLib, __DLT("AGL_DialUp") );
  pAGL_HangUp = (LPFN_AGL_HangUp)GetProcAddress( (HMODULE)hLib, __DLT("AGL_HangUp") );
  pAGL_InitAdapter = (LPFN_AGL_InitAdapter)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitAdapter") );
  pAGL_ExitAdapter = (LPFN_AGL_ExitAdapter)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ExitAdapter") );
  pAGL_GetLifelist = (LPFN_AGL_GetLifelist)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetLifelist") );
  pAGL_GetDirectPLC = (LPFN_AGL_GetDirectPLC)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetDirectPLC") );
  pAGL_PLCConnect = (LPFN_AGL_PLCConnect)GetProcAddress( (HMODULE)hLib, __DLT("AGL_PLCConnect") );
  pAGL_PLCConnectEx = (LPFN_AGL_PLCConnectEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_PLCConnectEx") );
  pAGL_PLCDisconnect = (LPFN_AGL_PLCDisconnect)GetProcAddress( (HMODULE)hLib, __DLT("AGL_PLCDisconnect") );
  pAGL_ReadMaxPacketSize = (LPFN_AGL_ReadMaxPacketSize)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadMaxPacketSize") );
  pAGL_GetRedConnState = (LPFN_AGL_GetRedConnState)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetRedConnState") );
  pAGL_GetRedConnStateMsg = (LPFN_AGL_GetRedConnStateMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetRedConnStateMsg") );
  pAGL_ReadOpState = (LPFN_AGL_ReadOpState)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadOpState") );
  pAGL_ReadOpStateEx = (LPFN_AGL_ReadOpStateEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadOpStateEx") );
  pAGL_GetPLCStartOptions = (LPFN_AGL_GetPLCStartOptions)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetPLCStartOptions") );
  pAGL_PLCStop = (LPFN_AGL_PLCStop)GetProcAddress( (HMODULE)hLib, __DLT("AGL_PLCStop") );
  pAGL_PLCStart = (LPFN_AGL_PLCStart)GetProcAddress( (HMODULE)hLib, __DLT("AGL_PLCStart") );
  pAGL_PLCResume = (LPFN_AGL_PLCResume)GetProcAddress( (HMODULE)hLib, __DLT("AGL_PLCResume") );
  pAGL_PLCColdStart = (LPFN_AGL_PLCColdStart)GetProcAddress( (HMODULE)hLib, __DLT("AGL_PLCColdStart") );
  pAGL_IsHPLC = (LPFN_AGL_IsHPLC)GetProcAddress( (HMODULE)hLib, __DLT("AGL_IsHPLC") );
  pAGL_HPLCStop = (LPFN_AGL_HPLCStop)GetProcAddress( (HMODULE)hLib, __DLT("AGL_HPLCStop") );
  pAGL_HPLCStart = (LPFN_AGL_HPLCStart)GetProcAddress( (HMODULE)hLib, __DLT("AGL_HPLCStart") );
  pAGL_HPLCColdStart = (LPFN_AGL_HPLCColdStart)GetProcAddress( (HMODULE)hLib, __DLT("AGL_HPLCColdStart") );
  pAGL_GetPLCClock = (LPFN_AGL_GetPLCClock)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetPLCClock") );
  pAGL_SetPLCClock = (LPFN_AGL_SetPLCClock)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetPLCClock") );
  pAGL_SyncPLCClock = (LPFN_AGL_SyncPLCClock)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SyncPLCClock") );
  pAGL_ReadMLFBNr = (LPFN_AGL_ReadMLFBNr)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadMLFBNr") );
  pAGL_ReadMLFBNrEx = (LPFN_AGL_ReadMLFBNrEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadMLFBNrEx") );
  pAGL_ReadPLCInfo = (LPFN_AGL_ReadPLCInfo)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadPLCInfo") );
  pAGL_ReadCycleTime = (LPFN_AGL_ReadCycleTime)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadCycleTime") );
  pAGL_ReadProtLevel = (LPFN_AGL_ReadProtLevel)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadProtLevel") );
  pAGL_ReadS7Ident = (LPFN_AGL_ReadS7Ident)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadS7Ident") );
  pAGL_ReadS7LED = (LPFN_AGL_ReadS7LED)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadS7LED") );
  pAGL_GetExtModuleInfo = (LPFN_AGL_GetExtModuleInfo)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetExtModuleInfo") );
  pAGL_ReadSzl = (LPFN_AGL_ReadSzl)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadSzl") );
  pAGL_IsPasswordReq = (LPFN_AGL_IsPasswordReq)GetProcAddress( (HMODULE)hLib, __DLT("AGL_IsPasswordReq") );
  pAGL_SetPassword = (LPFN_AGL_SetPassword)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetPassword") );
  pAGL_UnSetPassword = (LPFN_AGL_UnSetPassword)GetProcAddress( (HMODULE)hLib, __DLT("AGL_UnSetPassword") );
  pAGL_ReadDiagBufferEntrys = (LPFN_AGL_ReadDiagBufferEntrys)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadDiagBufferEntrys") );
  pAGL_ReadDiagBuffer = (LPFN_AGL_ReadDiagBuffer)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadDiagBuffer") );
  pAGL_GetDiagBufferEntry = (LPFN_AGL_GetDiagBufferEntry)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetDiagBufferEntry") );
  pAGL_ReadDBCount = (LPFN_AGL_ReadDBCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadDBCount") );
  pAGL_ReadDBList = (LPFN_AGL_ReadDBList)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadDBList") );
  pAGL_ReadDBLen = (LPFN_AGL_ReadDBLen)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadDBLen") );
  pAGL_ReadAllBlockCount = (LPFN_AGL_ReadAllBlockCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadAllBlockCount") );
  pAGL_ReadBlockCount = (LPFN_AGL_ReadBlockCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadBlockCount") );
  pAGL_ReadBlockList = (LPFN_AGL_ReadBlockList)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadBlockList") );
  pAGL_ReadBlockLen = (LPFN_AGL_ReadBlockLen)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadBlockLen") );
  pAGL_ReadInBytes = (LPFN_AGL_ReadInBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadInBytes") );
  pAGL_ReadPInBytes = (LPFN_AGL_ReadPInBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadPInBytes") );
  pAGL_ReadOutBytes = (LPFN_AGL_ReadOutBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadOutBytes") );
  pAGL_ReadFlagBytes = (LPFN_AGL_ReadFlagBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadFlagBytes") );
  pAGL_ReadSFlagBytes = (LPFN_AGL_ReadSFlagBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadSFlagBytes") );
  pAGL_ReadVarBytes = (LPFN_AGL_ReadVarBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadVarBytes") );
  pAGL_ReadDataBytes = (LPFN_AGL_ReadDataBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadDataBytes") );
  pAGL_ReadDataWords = (LPFN_AGL_ReadDataWords)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadDataWords") );
  pAGL_ReadTimerWords = (LPFN_AGL_ReadTimerWords)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadTimerWords") );
  pAGL_ReadCounterWords = (LPFN_AGL_ReadCounterWords)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadCounterWords") );
  pAGL_ReadMix = (LPFN_AGL_ReadMix)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadMix") );
  pAGL_ReadMixEx = (LPFN_AGL_ReadMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadMixEx") );
  pAGL_WriteInBytes = (LPFN_AGL_WriteInBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteInBytes") );
  pAGL_WriteOutBytes = (LPFN_AGL_WriteOutBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteOutBytes") );
  pAGL_WritePOutBytes = (LPFN_AGL_WritePOutBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WritePOutBytes") );
  pAGL_WriteFlagBytes = (LPFN_AGL_WriteFlagBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteFlagBytes") );
  pAGL_WriteSFlagBytes = (LPFN_AGL_WriteSFlagBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteSFlagBytes") );
  pAGL_WriteVarBytes = (LPFN_AGL_WriteVarBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteVarBytes") );
  pAGL_WriteDataBytes = (LPFN_AGL_WriteDataBytes)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteDataBytes") );
  pAGL_WriteDataWords = (LPFN_AGL_WriteDataWords)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteDataWords") );
  pAGL_WriteTimerWords = (LPFN_AGL_WriteTimerWords)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteTimerWords") );
  pAGL_WriteCounterWords = (LPFN_AGL_WriteCounterWords)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteCounterWords") );
  pAGL_WriteMix = (LPFN_AGL_WriteMix)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteMix") );
  pAGL_WriteMixEx = (LPFN_AGL_WriteMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteMixEx") );
  pAGL_InitOptReadMix = (LPFN_AGL_InitOptReadMix)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitOptReadMix") );
  pAGL_ReadOptReadMix = (LPFN_AGL_ReadOptReadMix)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadOptReadMix") );
  pAGL_EndOptReadMix = (LPFN_AGL_EndOptReadMix)GetProcAddress( (HMODULE)hLib, __DLT("AGL_EndOptReadMix") );
  pAGL_InitOptReadMixEx = (LPFN_AGL_InitOptReadMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitOptReadMixEx") );
  pAGL_ReadOptReadMixEx = (LPFN_AGL_ReadOptReadMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadOptReadMixEx") );
  pAGL_EndOptReadMixEx = (LPFN_AGL_EndOptReadMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_EndOptReadMixEx") );
  pAGL_InitOptWriteMix = (LPFN_AGL_InitOptWriteMix)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitOptWriteMix") );
  pAGL_WriteOptWriteMix = (LPFN_AGL_WriteOptWriteMix)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteOptWriteMix") );
  pAGL_EndOptWriteMix = (LPFN_AGL_EndOptWriteMix)GetProcAddress( (HMODULE)hLib, __DLT("AGL_EndOptWriteMix") );
  pAGL_InitOptWriteMixEx = (LPFN_AGL_InitOptWriteMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitOptWriteMixEx") );
  pAGL_WriteOptWriteMixEx = (LPFN_AGL_WriteOptWriteMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteOptWriteMixEx") );
  pAGL_EndOptWriteMixEx = (LPFN_AGL_EndOptWriteMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_EndOptWriteMixEx") );
  pAGL_SetOptNotification = (LPFN_AGL_SetOptNotification)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetOptNotification") );
  pAGL_DeleteOptJob = (LPFN_AGL_DeleteOptJob)GetProcAddress( (HMODULE)hLib, __DLT("AGL_DeleteOptJob") );
  pAGL_GetOptJobResult = (LPFN_AGL_GetOptJobResult)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetOptJobResult") );
  pAGL_WaitForOptJob = (LPFN_AGL_WaitForOptJob)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WaitForOptJob") );
  pAGL_AllocRWBuffs = (LPFN_AGL_AllocRWBuffs)GetProcAddress( (HMODULE)hLib, __DLT("AGL_AllocRWBuffs") );
  pAGL_FreeRWBuffs = (LPFN_AGL_FreeRWBuffs)GetProcAddress( (HMODULE)hLib, __DLT("AGL_FreeRWBuffs") );
  pAGL_ReadRWBuff = (LPFN_AGL_ReadRWBuff)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadRWBuff") );
  pAGL_WriteRWBuff = (LPFN_AGL_WriteRWBuff)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteRWBuff") );
  pAGL_RKSend = (LPFN_AGL_RKSend)GetProcAddress( (HMODULE)hLib, __DLT("AGL_RKSend") );
  pAGL_RKSendEx = (LPFN_AGL_RKSendEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_RKSendEx") );
  pAGL_RKFetch = (LPFN_AGL_RKFetch)GetProcAddress( (HMODULE)hLib, __DLT("AGL_RKFetch") );
  pAGL_RKFetchEx = (LPFN_AGL_RKFetchEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_RKFetchEx") );
  pAGL_Send_RKFetch = (LPFN_AGL_Send_RKFetch)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Send_RKFetch") );
  pAGL_Recv_RKSend = (LPFN_AGL_Recv_RKSend)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Recv_RKSend") );
  pAGL_Recv_RKFetch = (LPFN_AGL_Recv_RKFetch)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Recv_RKFetch") );
  pAGL_Send_3964 = (LPFN_AGL_Send_3964)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Send_3964") );
  pAGL_Recv_3964 = (LPFN_AGL_Recv_3964)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Recv_3964") );
  pAGL_BSend = (LPFN_AGL_BSend)GetProcAddress( (HMODULE)hLib, __DLT("AGL_BSend") );
  pAGL_BReceive = (LPFN_AGL_BReceive)GetProcAddress( (HMODULE)hLib, __DLT("AGL_BReceive") );
  pAGL_BSendEx = (LPFN_AGL_BSendEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_BSendEx") );
  pAGL_BReceiveEx = (LPFN_AGL_BReceiveEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_BReceiveEx") );
  pAGL_USend = (LPFN_AGL_USend)GetProcAddress( (HMODULE)hLib, __DLT("AGL_USend") );
  pAGL_UReceive = (LPFN_AGL_UReceive)GetProcAddress( (HMODULE)hLib, __DLT("AGL_UReceive") );
  pAGL_InitOpStateMsg = (LPFN_AGL_InitOpStateMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitOpStateMsg") );
  pAGL_ExitOpStateMsg = (LPFN_AGL_ExitOpStateMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ExitOpStateMsg") );
  pAGL_GetOpStateMsg = (LPFN_AGL_GetOpStateMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetOpStateMsg") );
  pAGL_InitDiagMsg = (LPFN_AGL_InitDiagMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitDiagMsg") );
  pAGL_ExitDiagMsg = (LPFN_AGL_ExitDiagMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ExitDiagMsg") );
  pAGL_GetDiagMsg = (LPFN_AGL_GetDiagMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetDiagMsg") );
  pAGL_InitCyclicRead = (LPFN_AGL_InitCyclicRead)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitCyclicRead") );
  pAGL_InitCyclicReadEx = (LPFN_AGL_InitCyclicReadEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitCyclicReadEx") );
  pAGL_StartCyclicRead = (LPFN_AGL_StartCyclicRead)GetProcAddress( (HMODULE)hLib, __DLT("AGL_StartCyclicRead") );
  pAGL_StopCyclicRead = (LPFN_AGL_StopCyclicRead)GetProcAddress( (HMODULE)hLib, __DLT("AGL_StopCyclicRead") );
  pAGL_ExitCyclicRead = (LPFN_AGL_ExitCyclicRead)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ExitCyclicRead") );
  pAGL_GetCyclicRead = (LPFN_AGL_GetCyclicRead)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetCyclicRead") );
  pAGL_GetCyclicReadEx = (LPFN_AGL_GetCyclicReadEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetCyclicReadEx") );
  pAGL_InitScanMsg = (LPFN_AGL_InitScanMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitScanMsg") );
  pAGL_ExitScanMsg = (LPFN_AGL_ExitScanMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ExitScanMsg") );
  pAGL_GetScanMsg = (LPFN_AGL_GetScanMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetScanMsg") );
  pAGL_HasAckTriggeredMsg = (LPFN_AGL_HasAckTriggeredMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_HasAckTriggeredMsg") );
  pAGL_InitAlarmMsg = (LPFN_AGL_InitAlarmMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitAlarmMsg") );
  pAGL_ExitAlarmMsg = (LPFN_AGL_ExitAlarmMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ExitAlarmMsg") );
  pAGL_GetAlarmMsg = (LPFN_AGL_GetAlarmMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetAlarmMsg") );
  pAGL_ReadOpenMsg = (LPFN_AGL_ReadOpenMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadOpenMsg") );
  pAGL_GetMsgStateChange = (LPFN_AGL_GetMsgStateChange)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetMsgStateChange") );
  pAGL_AckMsg = (LPFN_AGL_AckMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_AckMsg") );
  pAGL_LockMsg = (LPFN_AGL_LockMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_LockMsg") );
  pAGL_UnlockMsg = (LPFN_AGL_UnlockMsg)GetProcAddress( (HMODULE)hLib, __DLT("AGL_UnlockMsg") );
  pAGL_InitARSend = (LPFN_AGL_InitARSend)GetProcAddress( (HMODULE)hLib, __DLT("AGL_InitARSend") );
  pAGL_ExitARSend = (LPFN_AGL_ExitARSend)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ExitARSend") );
  pAGL_GetARSend = (LPFN_AGL_GetARSend)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetARSend") );
  pAGL_RFC1006_Connect = (LPFN_AGL_RFC1006_Connect)GetProcAddress( (HMODULE)hLib, __DLT("AGL_RFC1006_Connect") );
  pAGL_RFC1006_Disconnect = (LPFN_AGL_RFC1006_Disconnect)GetProcAddress( (HMODULE)hLib, __DLT("AGL_RFC1006_Disconnect") );
  pAGL_RFC1006_Receive = (LPFN_AGL_RFC1006_Receive)GetProcAddress( (HMODULE)hLib, __DLT("AGL_RFC1006_Receive") );
  pAGL_RFC1006_Send = (LPFN_AGL_RFC1006_Send)GetProcAddress( (HMODULE)hLib, __DLT("AGL_RFC1006_Send") );
  pAGL_NCK_ReadMixEx = (LPFN_AGL_NCK_ReadMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_ReadMixEx") );
  pAGL_NCK_WriteMixEx = (LPFN_AGL_NCK_WriteMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_WriteMixEx") );
  pAGL_NCK_CheckVarSize = (LPFN_AGL_NCK_CheckVarSize)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_CheckVarSize") );
  pAGL_NCK_InitCyclicReadEx = (LPFN_AGL_NCK_InitCyclicReadEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_InitCyclicReadEx") );
  pAGL_NCK_StartCyclicRead = (LPFN_AGL_NCK_StartCyclicRead)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_StartCyclicRead") );
  pAGL_NCK_StopCyclicRead = (LPFN_AGL_NCK_StopCyclicRead)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_StopCyclicRead") );
  pAGL_NCK_ExitCyclicRead = (LPFN_AGL_NCK_ExitCyclicRead)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_ExitCyclicRead") );
  pAGL_NCK_GetCyclicReadEx = (LPFN_AGL_NCK_GetCyclicReadEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_GetCyclicReadEx") );
  pAGL_NCK_PI_EXTERN = (LPFN_AGL_NCK_PI_EXTERN)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_EXTERN") );
  pAGL_NCK_PI_EXTMOD = (LPFN_AGL_NCK_PI_EXTMOD)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_EXTMOD") );
  pAGL_NCK_PI_SELECT = (LPFN_AGL_NCK_PI_SELECT)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_SELECT") );
  pAGL_NCK_PI_F_DELE = (LPFN_AGL_NCK_PI_F_DELE)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_F_DELE") );
  pAGL_NCK_PI_F_PROT = (LPFN_AGL_NCK_PI_F_PROT)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_F_PROT") );
  pAGL_NCK_PI_F_RENA = (LPFN_AGL_NCK_PI_F_RENA)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_F_RENA") );
  pAGL_NCK_PI_F_XFER = (LPFN_AGL_NCK_PI_F_XFER)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_F_XFER") );
  pAGL_NCK_PI_LOGIN = (LPFN_AGL_NCK_PI_LOGIN)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_LOGIN") );
  pAGL_NCK_PI_LOGOUT = (LPFN_AGL_NCK_PI_LOGOUT)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_LOGOUT") );
  pAGL_NCK_PI_F_OPEN = (LPFN_AGL_NCK_PI_F_OPEN)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_F_OPEN") );
  pAGL_NCK_PI_F_OPER = (LPFN_AGL_NCK_PI_F_OPER)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_F_OPER") );
  pAGL_NCK_PI_F_SEEK = (LPFN_AGL_NCK_PI_F_SEEK)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_F_SEEK") );
  pAGL_NCK_PI_F_CLOS = (LPFN_AGL_NCK_PI_F_CLOS)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_F_CLOS") );
  pAGL_NCK_PI_StartAll = (LPFN_AGL_NCK_PI_StartAll)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_StartAll") );
  pAGL_NCK_PI_F_COPY = (LPFN_AGL_NCK_PI_F_COPY)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_F_COPY") );
  pAGL_NCK_PI_F_PROR = (LPFN_AGL_NCK_PI_F_PROR)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_F_PROR") );
  pAGL_NCK_PI_CANCEL = (LPFN_AGL_NCK_PI_CANCEL)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_CANCEL") );
  pAGL_NCK_PI_CRCEDN = (LPFN_AGL_NCK_PI_CRCEDN)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_CRCEDN") );
  pAGL_NCK_PI_DELECE = (LPFN_AGL_NCK_PI_DELECE)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_DELECE") );
  pAGL_NCK_PI_DELETO = (LPFN_AGL_NCK_PI_DELETO)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_DELETO") );
  pAGL_NCK_PI_IBN_SS = (LPFN_AGL_NCK_PI_IBN_SS)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_IBN_SS") );
  pAGL_NCK_PI_MMCSEM = (LPFN_AGL_NCK_PI_MMCSEM)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_MMCSEM") );
  pAGL_NCK_PI_TMCRTO = (LPFN_AGL_NCK_PI_TMCRTO)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_TMCRTO") );
  pAGL_NCK_PI_TMMVTL = (LPFN_AGL_NCK_PI_TMMVTL)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_TMMVTL") );
  pAGL_NCK_PI_TMCRTC = (LPFN_AGL_NCK_PI_TMCRTC)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_TMCRTC") );
  pAGL_NCK_PI_CREATO = (LPFN_AGL_NCK_PI_CREATO)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_PI_CREATO") );
  pAGL_NCK_CopyFileToNC = (LPFN_AGL_NCK_CopyFileToNC)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_CopyFileToNC") );
  pAGL_NCK_CopyFileFromNC = (LPFN_AGL_NCK_CopyFileFromNC)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_CopyFileFromNC") );
  pAGL_NCK_CopyToNC = (LPFN_AGL_NCK_CopyToNC)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_CopyToNC") );
  pAGL_NCK_CopyFromNC = (LPFN_AGL_NCK_CopyFromNC)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_CopyFromNC") );
  pAGL_NCK_CopyFromNCAlloc = (LPFN_AGL_NCK_CopyFromNCAlloc)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_CopyFromNCAlloc") );
  pAGL_NCK_FreeBuff = (LPFN_AGL_NCK_FreeBuff)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_FreeBuff") );
  pAGL_NCK_SetConnProgressNotification = (LPFN_AGL_NCK_SetConnProgressNotification)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_SetConnProgressNotification") );
  pAGL_NCK_CheckNSKVarLine = (LPFN_AGL_NCK_CheckNSKVarLine)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_CheckNSKVarLine") );
  pAGL_NCK_ReadNSKVarFile = (LPFN_AGL_NCK_ReadNSKVarFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_ReadNSKVarFile") );
  pAGL_NCK_CheckCSVVarLine = (LPFN_AGL_NCK_CheckCSVVarLine)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_CheckCSVVarLine") );
  pAGL_NCK_ReadCSVVarFile = (LPFN_AGL_NCK_ReadCSVVarFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_ReadCSVVarFile") );
  pAGL_NCK_ReadGUDVarFile = (LPFN_AGL_NCK_ReadGUDVarFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_ReadGUDVarFile") );
  pAGL_NCK_ReadGUDVarFileEx = (LPFN_AGL_NCK_ReadGUDVarFileEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_ReadGUDVarFileEx") );
  pAGL_NCK_FreeVarBuff = (LPFN_AGL_NCK_FreeVarBuff)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_FreeVarBuff") );
  pAGL_NCK_GetSingleVarDef = (LPFN_AGL_NCK_GetSingleVarDef)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_GetSingleVarDef") );
  pAGL_NCK_ExtractNckAlarm = (LPFN_AGL_NCK_ExtractNckAlarm)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_ExtractNckAlarm") );
  pAGL_NCK_GetNCKDataRWByNCDDEItem = (LPFN_AGL_NCK_GetNCKDataRWByNCDDEItem)GetProcAddress( (HMODULE)hLib, __DLT("AGL_NCK_GetNCKDataRWByNCDDEItem") );
  pAGL_Drive_ReadMix = (LPFN_AGL_Drive_ReadMix)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Drive_ReadMix") );
  pAGL_Drive_ReadMixEx = (LPFN_AGL_Drive_ReadMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Drive_ReadMixEx") );
  pAGL_Drive_WriteMix = (LPFN_AGL_Drive_WriteMix)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Drive_WriteMix") );
  pAGL_Drive_WriteMixEx = (LPFN_AGL_Drive_WriteMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Drive_WriteMixEx") );
  pAGL_malloc = (LPFN_AGL_malloc)GetProcAddress( (HMODULE)hLib, __DLT("AGL_malloc") );
  pAGL_calloc = (LPFN_AGL_calloc)GetProcAddress( (HMODULE)hLib, __DLT("AGL_calloc") );
  pAGL_realloc = (LPFN_AGL_realloc)GetProcAddress( (HMODULE)hLib, __DLT("AGL_realloc") );
  pAGL_memcpy = (LPFN_AGL_memcpy)GetProcAddress( (HMODULE)hLib, __DLT("AGL_memcpy") );
  pAGL_memmove = (LPFN_AGL_memmove)GetProcAddress( (HMODULE)hLib, __DLT("AGL_memmove") );
  pAGL_memcmp = (LPFN_AGL_memcmp)GetProcAddress( (HMODULE)hLib, __DLT("AGL_memcmp") );
  pAGL_free = (LPFN_AGL_free)GetProcAddress( (HMODULE)hLib, __DLT("AGL_free") );
  pAGL_ReadInt16 = (LPFN_AGL_ReadInt16)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadInt16") );
  pAGL_ReadInt32 = (LPFN_AGL_ReadInt32)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadInt32") );
  pAGL_ReadWord = (LPFN_AGL_ReadWord)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadWord") );
  pAGL_ReadDWord = (LPFN_AGL_ReadDWord)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadDWord") );
  pAGL_ReadReal = (LPFN_AGL_ReadReal)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadReal") );
  pAGL_ReadS5Time = (LPFN_AGL_ReadS5Time)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadS5Time") );
  pAGL_ReadS5TimeW = (LPFN_AGL_ReadS5TimeW)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ReadS5TimeW") );
  pAGL_WriteInt16 = (LPFN_AGL_WriteInt16)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteInt16") );
  pAGL_WriteInt32 = (LPFN_AGL_WriteInt32)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteInt32") );
  pAGL_WriteWord = (LPFN_AGL_WriteWord)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteWord") );
  pAGL_WriteDWord = (LPFN_AGL_WriteDWord)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteDWord") );
  pAGL_WriteReal = (LPFN_AGL_WriteReal)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteReal") );
  pAGL_WriteS5Time = (LPFN_AGL_WriteS5Time)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteS5Time") );
  pAGL_WriteS5TimeW = (LPFN_AGL_WriteS5TimeW)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WriteS5TimeW") );
  pAGL_Byte2Word = (LPFN_AGL_Byte2Word)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Byte2Word") );
  pAGL_Byte2DWord = (LPFN_AGL_Byte2DWord)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Byte2DWord") );
  pAGL_Byte2Real = (LPFN_AGL_Byte2Real)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Byte2Real") );
  pAGL_Word2Byte = (LPFN_AGL_Word2Byte)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Word2Byte") );
  pAGL_DWord2Byte = (LPFN_AGL_DWord2Byte)GetProcAddress( (HMODULE)hLib, __DLT("AGL_DWord2Byte") );
  pAGL_Real2Byte = (LPFN_AGL_Real2Byte)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Real2Byte") );
  pAGL_GetBit = (LPFN_AGL_GetBit)GetProcAddress( (HMODULE)hLib, __DLT("AGL_GetBit") );
  pAGL_SetBit = (LPFN_AGL_SetBit)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetBit") );
  pAGL_ResetBit = (LPFN_AGL_ResetBit)GetProcAddress( (HMODULE)hLib, __DLT("AGL_ResetBit") );
  pAGL_SetBitVal = (LPFN_AGL_SetBitVal)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SetBitVal") );
  pAGL_Buff2String = (LPFN_AGL_Buff2String)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Buff2String") );
  pAGL_String2Buff = (LPFN_AGL_String2Buff)GetProcAddress( (HMODULE)hLib, __DLT("AGL_String2Buff") );
  pAGL_Buff2WString = (LPFN_AGL_Buff2WString)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Buff2WString") );
  pAGL_WString2Buff = (LPFN_AGL_WString2Buff)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WString2Buff") );
  pAGL_S7String2String = (LPFN_AGL_S7String2String)GetProcAddress( (HMODULE)hLib, __DLT("AGL_S7String2String") );
  pAGL_String2S7String = (LPFN_AGL_String2S7String)GetProcAddress( (HMODULE)hLib, __DLT("AGL_String2S7String") );
  pAGL_BCD2Int16 = (LPFN_AGL_BCD2Int16)GetProcAddress( (HMODULE)hLib, __DLT("AGL_BCD2Int16") );
  pAGL_BCD2Int32 = (LPFN_AGL_BCD2Int32)GetProcAddress( (HMODULE)hLib, __DLT("AGL_BCD2Int32") );
  pAGL_Int162BCD = (LPFN_AGL_Int162BCD)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Int162BCD") );
  pAGL_Int322BCD = (LPFN_AGL_Int322BCD)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Int322BCD") );
  pAGL_LongAsFloat = (LPFN_AGL_LongAsFloat)GetProcAddress( (HMODULE)hLib, __DLT("AGL_LongAsFloat") );
  pAGL_FloatAsLong = (LPFN_AGL_FloatAsLong)GetProcAddress( (HMODULE)hLib, __DLT("AGL_FloatAsLong") );
  pAGL_Text2DataRW = (LPFN_AGL_Text2DataRW)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Text2DataRW") );
  pAGL_DataRW2Text = (LPFN_AGL_DataRW2Text)GetProcAddress( (HMODULE)hLib, __DLT("AGL_DataRW2Text") );
  pAGL_S7DT2SysTime = (LPFN_AGL_S7DT2SysTime)GetProcAddress( (HMODULE)hLib, __DLT("AGL_S7DT2SysTime") );
  pAGL_SysTime2S7DT = (LPFN_AGL_SysTime2S7DT)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SysTime2S7DT") );
  pAGL_TOD2SysTime = (LPFN_AGL_TOD2SysTime)GetProcAddress( (HMODULE)hLib, __DLT("AGL_TOD2SysTime") );
  pAGL_SysTime2TOD = (LPFN_AGL_SysTime2TOD)GetProcAddress( (HMODULE)hLib, __DLT("AGL_SysTime2TOD") );
  pAGL_S7Date2YMD = (LPFN_AGL_S7Date2YMD)GetProcAddress( (HMODULE)hLib, __DLT("AGL_S7Date2YMD") );
  pAGL_Float2KG = (LPFN_AGL_Float2KG)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Float2KG") );
  pAGL_KG2Float = (LPFN_AGL_KG2Float)GetProcAddress( (HMODULE)hLib, __DLT("AGL_KG2Float") );
  pAGL_Float2DWKG = (LPFN_AGL_Float2DWKG)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Float2DWKG") );
  pAGL_DWKG2Float = (LPFN_AGL_DWKG2Float)GetProcAddress( (HMODULE)hLib, __DLT("AGL_DWKG2Float") );
  pAGL_S7Ident2String = (LPFN_AGL_S7Ident2String)GetProcAddress( (HMODULE)hLib, __DLT("AGL_S7Ident2String") );
  pAGL_WLD_OpenFile = (LPFN_AGL_WLD_OpenFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WLD_OpenFile") );
  pAGL_WLD_OpenFileEncrypted = (LPFN_AGL_WLD_OpenFileEncrypted)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WLD_OpenFileEncrypted") );
  pAGL_WLD_EncryptFile = (LPFN_AGL_WLD_EncryptFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WLD_EncryptFile") );
  pAGL_WLD_DecryptFile = (LPFN_AGL_WLD_DecryptFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WLD_DecryptFile") );
  pAGL_WLD_CloseFile = (LPFN_AGL_WLD_CloseFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WLD_CloseFile") );
  pAGL_WLD_ReadAllBlockCount = (LPFN_AGL_WLD_ReadAllBlockCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WLD_ReadAllBlockCount") );
  pAGL_WLD_ReadBlockCount = (LPFN_AGL_WLD_ReadBlockCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WLD_ReadBlockCount") );
  pAGL_WLD_ReadBlockList = (LPFN_AGL_WLD_ReadBlockList)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WLD_ReadBlockList") );
  pAGL_WLD_ReadBlockLen = (LPFN_AGL_WLD_ReadBlockLen)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WLD_ReadBlockLen") );
  pAGL_WLD_DeleteBlocks = (LPFN_AGL_WLD_DeleteBlocks)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WLD_DeleteBlocks") );
  pAGL_WLD_GetReport = (LPFN_AGL_WLD_GetReport)GetProcAddress( (HMODULE)hLib, __DLT("AGL_WLD_GetReport") );
  pAGL_PLC_Backup = (LPFN_AGL_PLC_Backup)GetProcAddress( (HMODULE)hLib, __DLT("AGL_PLC_Backup") );
  pAGL_PLC_Restore = (LPFN_AGL_PLC_Restore)GetProcAddress( (HMODULE)hLib, __DLT("AGL_PLC_Restore") );
  pAGL_PLC_DeleteBlocks = (LPFN_AGL_PLC_DeleteBlocks)GetProcAddress( (HMODULE)hLib, __DLT("AGL_PLC_DeleteBlocks") );
  pAGL_Compress = (LPFN_AGL_Compress)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Compress") );
  pAGL_Symbolic_ReadMixEx = (LPFN_AGL_Symbolic_ReadMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_ReadMixEx") );
  pAGL_Symbolic_WriteMixEx = (LPFN_AGL_Symbolic_WriteMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_WriteMixEx") );
  pAGL_Symbolic_LoadTIAProjectSymbols = (LPFN_AGL_Symbolic_LoadTIAProjectSymbols)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_LoadTIAProjectSymbols") );
  pAGL_Symbolic_LoadAGLinkSymbolsFromPLC = (LPFN_AGL_Symbolic_LoadAGLinkSymbolsFromPLC)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_LoadAGLinkSymbolsFromPLC") );
  pAGL_Symbolic_SaveAGLinkSymbolsToFile = (LPFN_AGL_Symbolic_SaveAGLinkSymbolsToFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SaveAGLinkSymbolsToFile") );
  pAGL_Symbolic_LoadAGLinkSymbolsFromFile = (LPFN_AGL_Symbolic_LoadAGLinkSymbolsFromFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_LoadAGLinkSymbolsFromFile") );
  pAGL_Symbolic_FreeHandle = (LPFN_AGL_Symbolic_FreeHandle)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_FreeHandle") );
  pAGL_Symbolic_GetChildCount = (LPFN_AGL_Symbolic_GetChildCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetChildCount") );
  pAGL_Symbolic_GetChild = (LPFN_AGL_Symbolic_GetChild)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetChild") );
  pAGL_Symbolic_GetChildByName = (LPFN_AGL_Symbolic_GetChildByName)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetChildByName") );
  pAGL_Symbolic_GetName = (LPFN_AGL_Symbolic_GetName)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetName") );
  pAGL_Symbolic_GetLocalOffset = (LPFN_AGL_Symbolic_GetLocalOffset)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetLocalOffset") );
  pAGL_Symbolic_GetSystemType = (LPFN_AGL_Symbolic_GetSystemType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetSystemType") );
  pAGL_Symbolic_GetHierarchyType = (LPFN_AGL_Symbolic_GetHierarchyType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetHierarchyType") );
  pAGL_Symbolic_GetArrayDimensionCount = (LPFN_AGL_Symbolic_GetArrayDimensionCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetArrayDimensionCount") );
  pAGL_Symbolic_GetArrayDimension = (LPFN_AGL_Symbolic_GetArrayDimension)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetArrayDimension") );
  pAGL_Symbolic_GetMaxStringSize = (LPFN_AGL_Symbolic_GetMaxStringSize)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetMaxStringSize") );
  pAGL_Symbolic_GetValueType = (LPFN_AGL_Symbolic_GetValueType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetValueType") );
  pAGL_Symbolic_GetTypeState = (LPFN_AGL_Symbolic_GetTypeState)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetTypeState") );
  pAGL_Symbolic_GetSegmentType = (LPFN_AGL_Symbolic_GetSegmentType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetSegmentType") );
  pAGL_Symbolic_GetPermissionType = (LPFN_AGL_Symbolic_GetPermissionType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetPermissionType") );
  pAGL_Symbolic_EscapeString = (LPFN_AGL_Symbolic_EscapeString)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_EscapeString") );
  pAGL_Symbolic_GetNodeByPath = (LPFN_AGL_Symbolic_GetNodeByPath)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetNodeByPath") );
  pAGL_Symbolic_GetIndexSize = (LPFN_AGL_Symbolic_GetIndexSize)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetIndexSize") );
  pAGL_Symbolic_GetIndex = (LPFN_AGL_Symbolic_GetIndex)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetIndex") );
  pAGL_Symbolic_GetLinearIndex = (LPFN_AGL_Symbolic_GetLinearIndex)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetLinearIndex") );
  pAGL_Symbolic_GetArrayElementCount = (LPFN_AGL_Symbolic_GetArrayElementCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetArrayElementCount") );
  pAGL_Symbolic_Expand = (LPFN_AGL_Symbolic_Expand)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_Expand") );
  pAGL_Symbolic_Collapse = (LPFN_AGL_Symbolic_Collapse)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_Collapse") );
  pAGL_Symbolic_GetSystemScope = (LPFN_AGL_Symbolic_GetSystemScope)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetSystemScope") );
  pAGL_Symbolic_GetSystemTypeState = (LPFN_AGL_Symbolic_GetSystemTypeState)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetSystemTypeState") );
  pAGL_Symbolic_CreateAccess = (LPFN_AGL_Symbolic_CreateAccess)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_CreateAccess") );
  pAGL_Symbolic_CreateAccessByPath = (LPFN_AGL_Symbolic_CreateAccessByPath)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_CreateAccessByPath") );
  pAGL_Symbolic_Get_DATA_RW40 = (LPFN_AGL_Symbolic_Get_DATA_RW40)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_Get_DATA_RW40") );
  pAGL_Symbolic_Get_DATA_RW40_ByPath = (LPFN_AGL_Symbolic_Get_DATA_RW40_ByPath)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_Get_DATA_RW40_ByPath") );
  pAGL_Symbolic_GetType = (LPFN_AGL_Symbolic_GetType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetType") );
  pAGL_Symbolic_GetAccessBufferSize = (LPFN_AGL_Symbolic_GetAccessBufferSize)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferSize") );
  pAGL_Symbolic_GetAccessBufferElementSize = (LPFN_AGL_Symbolic_GetAccessBufferElementSize)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferElementSize") );
  pAGL_Symbolic_GetAccessStringSize = (LPFN_AGL_Symbolic_GetAccessStringSize)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessStringSize") );
  pAGL_Symbolic_GetAccessBufferElementCount = (LPFN_AGL_Symbolic_GetAccessBufferElementCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferElementCount") );
  pAGL_Symbolic_GetAccessElementSystemType = (LPFN_AGL_Symbolic_GetAccessElementSystemType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessElementSystemType") );
  pAGL_Symbolic_GetAccessElementValueType = (LPFN_AGL_Symbolic_GetAccessElementValueType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessElementValueType") );
  pAGL_Symbolic_GetArrayIndexAsLinearIndex = (LPFN_AGL_Symbolic_GetArrayIndexAsLinearIndex)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetArrayIndexAsLinearIndex") );
  pAGL_Symbolic_GetArrayLinearIndexAsIndex = (LPFN_AGL_Symbolic_GetArrayLinearIndexAsIndex)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetArrayLinearIndexAsIndex") );
  pAGL_Symbolic_CreateArrayAccessByLinearIndex = (LPFN_AGL_Symbolic_CreateArrayAccessByLinearIndex)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_CreateArrayAccessByLinearIndex") );
  pAGL_Symbolic_CreateArrayRangeAccessByLinearIndex = (LPFN_AGL_Symbolic_CreateArrayRangeAccessByLinearIndex)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_CreateArrayRangeAccessByLinearIndex") );
  pAGL_Symbolic_CreateArrayAccessByIndex = (LPFN_AGL_Symbolic_CreateArrayAccessByIndex)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_CreateArrayAccessByIndex") );
  pAGL_Symbolic_CreateArrayRangeAccessByIndex = (LPFN_AGL_Symbolic_CreateArrayRangeAccessByIndex)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_CreateArrayRangeAccessByIndex") );
  pAGL_Symbolic_GetAccessBufferUInt8 = (LPFN_AGL_Symbolic_GetAccessBufferUInt8)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferUInt8") );
  pAGL_Symbolic_GetAccessBufferUInt16 = (LPFN_AGL_Symbolic_GetAccessBufferUInt16)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferUInt16") );
  pAGL_Symbolic_GetAccessBufferUInt32 = (LPFN_AGL_Symbolic_GetAccessBufferUInt32)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferUInt32") );
  pAGL_Symbolic_GetAccessBufferUInt64 = (LPFN_AGL_Symbolic_GetAccessBufferUInt64)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferUInt64") );
  pAGL_Symbolic_GetAccessBufferInt8 = (LPFN_AGL_Symbolic_GetAccessBufferInt8)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferInt8") );
  pAGL_Symbolic_GetAccessBufferInt16 = (LPFN_AGL_Symbolic_GetAccessBufferInt16)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferInt16") );
  pAGL_Symbolic_GetAccessBufferInt32 = (LPFN_AGL_Symbolic_GetAccessBufferInt32)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferInt32") );
  pAGL_Symbolic_GetAccessBufferInt64 = (LPFN_AGL_Symbolic_GetAccessBufferInt64)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferInt64") );
  pAGL_Symbolic_GetAccessBufferFloat32 = (LPFN_AGL_Symbolic_GetAccessBufferFloat32)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferFloat32") );
  pAGL_Symbolic_GetAccessBufferFloat64 = (LPFN_AGL_Symbolic_GetAccessBufferFloat64)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferFloat64") );
  pAGL_Symbolic_GetAccessBufferChar8 = (LPFN_AGL_Symbolic_GetAccessBufferChar8)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferChar8") );
  pAGL_Symbolic_GetAccessBufferChar16 = (LPFN_AGL_Symbolic_GetAccessBufferChar16)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferChar16") );
  pAGL_Symbolic_GetAccessBufferString8 = (LPFN_AGL_Symbolic_GetAccessBufferString8)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferString8") );
  pAGL_Symbolic_GetAccessBufferString16 = (LPFN_AGL_Symbolic_GetAccessBufferString16)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferString16") );
  pAGL_Symbolic_GetAccessBufferS7_DTLParts = (LPFN_AGL_Symbolic_GetAccessBufferS7_DTLParts)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferS7_DTLParts") );
  pAGL_Symbolic_GetAccessBufferS7_S5TimeParts = (LPFN_AGL_Symbolic_GetAccessBufferS7_S5TimeParts)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferS7_S5TimeParts") );
  pAGL_Symbolic_GetAccessBufferS7_S5TimeMs = (LPFN_AGL_Symbolic_GetAccessBufferS7_S5TimeMs)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferS7_S5TimeMs") );
  pAGL_Symbolic_GetAccessBufferS7_Counter = (LPFN_AGL_Symbolic_GetAccessBufferS7_Counter)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferS7_Counter") );
  pAGL_Symbolic_GetAccessBufferS7_Date_and_TimeParts = (LPFN_AGL_Symbolic_GetAccessBufferS7_Date_and_TimeParts)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAccessBufferS7_Date_and_TimeParts") );
  pAGL_Symbolic_SetAccessBufferUInt8 = (LPFN_AGL_Symbolic_SetAccessBufferUInt8)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferUInt8") );
  pAGL_Symbolic_SetAccessBufferUInt16 = (LPFN_AGL_Symbolic_SetAccessBufferUInt16)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferUInt16") );
  pAGL_Symbolic_SetAccessBufferUInt32 = (LPFN_AGL_Symbolic_SetAccessBufferUInt32)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferUInt32") );
  pAGL_Symbolic_SetAccessBufferUInt64 = (LPFN_AGL_Symbolic_SetAccessBufferUInt64)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferUInt64") );
  pAGL_Symbolic_SetAccessBufferInt8 = (LPFN_AGL_Symbolic_SetAccessBufferInt8)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferInt8") );
  pAGL_Symbolic_SetAccessBufferInt16 = (LPFN_AGL_Symbolic_SetAccessBufferInt16)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferInt16") );
  pAGL_Symbolic_SetAccessBufferInt32 = (LPFN_AGL_Symbolic_SetAccessBufferInt32)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferInt32") );
  pAGL_Symbolic_SetAccessBufferInt64 = (LPFN_AGL_Symbolic_SetAccessBufferInt64)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferInt64") );
  pAGL_Symbolic_SetAccessBufferFloat32 = (LPFN_AGL_Symbolic_SetAccessBufferFloat32)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferFloat32") );
  pAGL_Symbolic_SetAccessBufferFloat64 = (LPFN_AGL_Symbolic_SetAccessBufferFloat64)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferFloat64") );
  pAGL_Symbolic_SetAccessBufferChar8 = (LPFN_AGL_Symbolic_SetAccessBufferChar8)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferChar8") );
  pAGL_Symbolic_SetAccessBufferChar16 = (LPFN_AGL_Symbolic_SetAccessBufferChar16)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferChar16") );
  pAGL_Symbolic_SetAccessBufferString8 = (LPFN_AGL_Symbolic_SetAccessBufferString8)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferString8") );
  pAGL_Symbolic_SetAccessBufferString16 = (LPFN_AGL_Symbolic_SetAccessBufferString16)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferString16") );
  pAGL_Symbolic_SetAccessBufferS7_DTLParts = (LPFN_AGL_Symbolic_SetAccessBufferS7_DTLParts)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferS7_DTLParts") );
  pAGL_Symbolic_SetAccessBufferS7_S5TimeParts = (LPFN_AGL_Symbolic_SetAccessBufferS7_S5TimeParts)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferS7_S5TimeParts") );
  pAGL_Symbolic_SetAccessBufferS7_S5TimeMs = (LPFN_AGL_Symbolic_SetAccessBufferS7_S5TimeMs)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferS7_S5TimeMs") );
  pAGL_Symbolic_SetAccessBufferS7_Counter = (LPFN_AGL_Symbolic_SetAccessBufferS7_Counter)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferS7_Counter") );
  pAGL_Symbolic_SetAccessBufferS7_Date_and_TimeParts = (LPFN_AGL_Symbolic_SetAccessBufferS7_Date_and_TimeParts)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SetAccessBufferS7_Date_and_TimeParts") );
  pAGL_Symbolic_GetProjectEditingCulture = (LPFN_AGL_Symbolic_GetProjectEditingCulture)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetProjectEditingCulture") );
  pAGL_Symbolic_GetProjectReferenceCulture = (LPFN_AGL_Symbolic_GetProjectReferenceCulture)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetProjectReferenceCulture") );
  pAGL_Symbolic_GetProjectCultureCount = (LPFN_AGL_Symbolic_GetProjectCultureCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetProjectCultureCount") );
  pAGL_Symbolic_GetProjectCulture = (LPFN_AGL_Symbolic_GetProjectCulture)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetProjectCulture") );
  pAGL_Symbolic_GetComment = (LPFN_AGL_Symbolic_GetComment)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetComment") );
  pAGL_Symbolic_GetCommentCultureCount = (LPFN_AGL_Symbolic_GetCommentCultureCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetCommentCultureCount") );
  pAGL_Symbolic_GetCommentCulture = (LPFN_AGL_Symbolic_GetCommentCulture)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetCommentCulture") );
  pAGL_Symbolic_DatablockGetNumber = (LPFN_AGL_Symbolic_DatablockGetNumber)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_DatablockGetNumber") );
  pAGL_Symbolic_GetPath = (LPFN_AGL_Symbolic_GetPath)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetPath") );
  pAGL_Symbolic_GetEscapedPath = (LPFN_AGL_Symbolic_GetEscapedPath)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetEscapedPath") );
  pAGL_Symbolic_GetAttributeHMIAccessible = (LPFN_AGL_Symbolic_GetAttributeHMIAccessible)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAttributeHMIAccessible") );
  pAGL_Symbolic_GetAttributeHMIVisible = (LPFN_AGL_Symbolic_GetAttributeHMIVisible)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAttributeHMIVisible") );
  pAGL_Symbolic_GetAttributeRemanent = (LPFN_AGL_Symbolic_GetAttributeRemanent)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetAttributeRemanent") );
  pAGL_Symbolic_GetS7PlcTypeName = (LPFN_AGL_Symbolic_GetS7PlcTypeName)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetS7PlcTypeName") );
  pAGL_Symbolic_GetS7PlcFirmware = (LPFN_AGL_Symbolic_GetS7PlcFirmware)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetS7PlcFirmware") );
  pAGL_Symbolic_GetS7PlcMLFB = (LPFN_AGL_Symbolic_GetS7PlcMLFB)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetS7PlcMLFB") );
  pAGL_Symbolic_GetS7PlcFamily = (LPFN_AGL_Symbolic_GetS7PlcFamily)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_GetS7PlcFamily") );
  pAGL_Symbolic_SaveSingleValueAccessSymbolsToFile = (LPFN_AGL_Symbolic_SaveSingleValueAccessSymbolsToFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_SaveSingleValueAccessSymbolsToFile") );
  pAGL_Symbolic_LoadSingleValueAccessSymbolsFromFile = (LPFN_AGL_Symbolic_LoadSingleValueAccessSymbolsFromFile)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_LoadSingleValueAccessSymbolsFromFile") );
  pAGL_Symbolic_CreateAccessFromSingleValueAccessSymbols = (LPFN_AGL_Symbolic_CreateAccessFromSingleValueAccessSymbols)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Symbolic_CreateAccessFromSingleValueAccessSymbols") );
  pAGL_Simotion_LoadSTISymbols = (LPFN_AGL_Simotion_LoadSTISymbols)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_LoadSTISymbols") );
  pAGL_Simotion_FreeHandle = (LPFN_AGL_Simotion_FreeHandle)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_FreeHandle") );
  pAGL_Simotion_GetName = (LPFN_AGL_Simotion_GetName)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_GetName") );
  pAGL_Simotion_GetHierarchyType = (LPFN_AGL_Simotion_GetHierarchyType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_GetHierarchyType") );
  pAGL_Simotion_GetValueType = (LPFN_AGL_Simotion_GetValueType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_GetValueType") );
  pAGL_Simotion_GetPermissionType = (LPFN_AGL_Simotion_GetPermissionType)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_GetPermissionType") );
  pAGL_Simotion_GetChildCount = (LPFN_AGL_Simotion_GetChildCount)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_GetChildCount") );
  pAGL_Simotion_GetChild = (LPFN_AGL_Simotion_GetChild)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_GetChild") );
  pAGL_Simotion_GetNodeByPath = (LPFN_AGL_Simotion_GetNodeByPath)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_GetNodeByPath") );
  pAGL_Simotion_CreateAccess = (LPFN_AGL_Simotion_CreateAccess)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_CreateAccess") );
  pAGL_Simotion_CreateAccessByPath = (LPFN_AGL_Simotion_CreateAccessByPath)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_CreateAccessByPath") );
  pAGL_Simotion_ReadMixEx = (LPFN_AGL_Simotion_ReadMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_ReadMixEx") );
  pAGL_Simotion_WriteMixEx = (LPFN_AGL_Simotion_WriteMixEx)GetProcAddress( (HMODULE)hLib, __DLT("AGL_Simotion_WriteMixEx") );
#if !defined( __LINUX__ )
  pAGLSym_OpenProject = (LPFN_AGLSym_OpenProject)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_OpenProject") );
  pAGLSym_CloseProject = (LPFN_AGLSym_CloseProject)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_CloseProject") );
  pAGLSym_WriteCpuListToFile = (LPFN_AGLSym_WriteCpuListToFile)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_WriteCpuListToFile") );
  pAGLSym_GetProgramCount = (LPFN_AGLSym_GetProgramCount)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetProgramCount") );
  pAGLSym_FindFirstProgram = (LPFN_AGLSym_FindFirstProgram)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindFirstProgram") );
  pAGLSym_FindNextProgram = (LPFN_AGLSym_FindNextProgram)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindNextProgram") );
  pAGLSym_FindCloseProgram = (LPFN_AGLSym_FindCloseProgram)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindCloseProgram") );
  pAGLSym_SelectProgram = (LPFN_AGLSym_SelectProgram)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_SelectProgram") );
  pAGLSym_GetSymbolCount = (LPFN_AGLSym_GetSymbolCount)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetSymbolCount") );
  pAGLSym_GetSymbolCountFilter = (LPFN_AGLSym_GetSymbolCountFilter)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetSymbolCountFilter") );
  pAGLSym_FindFirstSymbol = (LPFN_AGLSym_FindFirstSymbol)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindFirstSymbol") );
  pAGLSym_FindFirstSymbolFilter = (LPFN_AGLSym_FindFirstSymbolFilter)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindFirstSymbolFilter") );
  pAGLSym_FindNextSymbol = (LPFN_AGLSym_FindNextSymbol)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindNextSymbol") );
  pAGLSym_FindCloseSymbol = (LPFN_AGLSym_FindCloseSymbol)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindCloseSymbol") );
  pAGLSym_ReadPrjDBCount = (LPFN_AGLSym_ReadPrjDBCount)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_ReadPrjDBCount") );
  pAGLSym_ReadPrjDBCountFilter = (LPFN_AGLSym_ReadPrjDBCountFilter)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_ReadPrjDBCountFilter") );
  pAGLSym_ReadPrjDBList = (LPFN_AGLSym_ReadPrjDBList)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_ReadPrjDBList") );
  pAGLSym_ReadPrjDBListFilter = (LPFN_AGLSym_ReadPrjDBListFilter)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_ReadPrjDBListFilter") );
  pAGLSym_ReadPrjBlkCountFilter = (LPFN_AGLSym_ReadPrjBlkCountFilter)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_ReadPrjBlkCountFilter") );
  pAGLSym_ReadPrjBlkListFilter = (LPFN_AGLSym_ReadPrjBlkListFilter)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_ReadPrjBlkListFilter") );
  pAGLSym_GetDbSymbolCount = (LPFN_AGLSym_GetDbSymbolCount)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetDbSymbolCount") );
  pAGLSym_GetDbSymbolCountFilter = (LPFN_AGLSym_GetDbSymbolCountFilter)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetDbSymbolCountFilter") );
  pAGLSym_FindFirstDbSymbol = (LPFN_AGLSym_FindFirstDbSymbol)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindFirstDbSymbol") );
  pAGLSym_FindFirstDbSymbolFilter = (LPFN_AGLSym_FindFirstDbSymbolFilter)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindFirstDbSymbolFilter") );
  pAGLSym_FindNextDbSymbol = (LPFN_AGLSym_FindNextDbSymbol)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindNextDbSymbol") );
  pAGLSym_FindFirstDbSymbolEx = (LPFN_AGLSym_FindFirstDbSymbolEx)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindFirstDbSymbolEx") );
  pAGLSym_FindNextDbSymbolEx = (LPFN_AGLSym_FindNextDbSymbolEx)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindNextDbSymbolEx") );
  pAGLSym_GetDbSymbolExComment = (LPFN_AGLSym_GetDbSymbolExComment)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetDbSymbolExComment") );
  pAGLSym_FindCloseDbSymbol = (LPFN_AGLSym_FindCloseDbSymbol)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindCloseDbSymbol") );
  pAGLSym_GetDbDependency = (LPFN_AGLSym_GetDbDependency)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetDbDependency") );
  pAGLSym_GetDeclarationCountFilter = (LPFN_AGLSym_GetDeclarationCountFilter)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetDeclarationCountFilter") );
  pAGLSym_FindFirstDeclarationFilter = (LPFN_AGLSym_FindFirstDeclarationFilter)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindFirstDeclarationFilter") );
  pAGLSym_FindNextDeclaration = (LPFN_AGLSym_FindNextDeclaration)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindNextDeclaration") );
  pAGLSym_GetDeclarationInitialValue = (LPFN_AGLSym_GetDeclarationInitialValue)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetDeclarationInitialValue") );
  pAGLSym_FindCloseDeclaration = (LPFN_AGLSym_FindCloseDeclaration)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindCloseDeclaration") );
  pAGLSym_GetSymbolFromText = (LPFN_AGLSym_GetSymbolFromText)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetSymbolFromText") );
  pAGLSym_GetSymbolFromTextEx = (LPFN_AGLSym_GetSymbolFromTextEx)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetSymbolFromTextEx") );
  pAGLSym_GetReadMixFromText = (LPFN_AGLSym_GetReadMixFromText)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetReadMixFromText") );
  pAGLSym_GetReadMixFromTextEx = (LPFN_AGLSym_GetReadMixFromTextEx)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetReadMixFromTextEx") );
  pAGLSym_GetSymbol = (LPFN_AGLSym_GetSymbol)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetSymbol") );
  pAGLSym_GetSymbolEx = (LPFN_AGLSym_GetSymbolEx)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetSymbolEx") );
  pAGLSym_OpenAlarms = (LPFN_AGLSym_OpenAlarms)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_OpenAlarms") );
  pAGLSym_CloseAlarms = (LPFN_AGLSym_CloseAlarms)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_CloseAlarms") );
  pAGLSym_FindFirstAlarmData = (LPFN_AGLSym_FindFirstAlarmData)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindFirstAlarmData") );
  pAGLSym_FindNextAlarmData = (LPFN_AGLSym_FindNextAlarmData)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindNextAlarmData") );
  pAGLSym_FindCloseAlarmData = (LPFN_AGLSym_FindCloseAlarmData)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindCloseAlarmData") );
  pAGLSym_GetAlarmData = (LPFN_AGLSym_GetAlarmData)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmData") );
  pAGLSym_GetAlarmName = (LPFN_AGLSym_GetAlarmName)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmName") );
  pAGLSym_GetAlarmType = (LPFN_AGLSym_GetAlarmType)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmType") );
  pAGLSym_GetAlarmBaseName = (LPFN_AGLSym_GetAlarmBaseName)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmBaseName") );
  pAGLSym_GetAlarmTypeName = (LPFN_AGLSym_GetAlarmTypeName)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmTypeName") );
  pAGLSym_GetAlarmSignalCount = (LPFN_AGLSym_GetAlarmSignalCount)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmSignalCount") );
  pAGLSym_GetAlarmMsgClass = (LPFN_AGLSym_GetAlarmMsgClass)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmMsgClass") );
  pAGLSym_GetAlarmPriority = (LPFN_AGLSym_GetAlarmPriority)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmPriority") );
  pAGLSym_GetAlarmAckGroup = (LPFN_AGLSym_GetAlarmAckGroup)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmAckGroup") );
  pAGLSym_GetAlarmAcknowledge = (LPFN_AGLSym_GetAlarmAcknowledge)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmAcknowledge") );
  pAGLSym_GetAlarmProtocol = (LPFN_AGLSym_GetAlarmProtocol)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmProtocol") );
  pAGLSym_GetAlarmDispGroup = (LPFN_AGLSym_GetAlarmDispGroup)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmDispGroup") );
  pAGLSym_FindFirstAlarmTextLanguage = (LPFN_AGLSym_FindFirstAlarmTextLanguage)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindFirstAlarmTextLanguage") );
  pAGLSym_FindNextAlarmTextLanguage = (LPFN_AGLSym_FindNextAlarmTextLanguage)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindNextAlarmTextLanguage") );
  pAGLSym_FindCloseAlarmTextLanguage = (LPFN_AGLSym_FindCloseAlarmTextLanguage)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindCloseAlarmTextLanguage") );
  pAGLSym_SetAlarmTextDefaultLanguage = (LPFN_AGLSym_SetAlarmTextDefaultLanguage)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_SetAlarmTextDefaultLanguage") );
  pAGLSym_GetAlarmText = (LPFN_AGLSym_GetAlarmText)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmText") );
  pAGLSym_GetAlarmInfo = (LPFN_AGLSym_GetAlarmInfo)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmInfo") );
  pAGLSym_GetAlarmAddText = (LPFN_AGLSym_GetAlarmAddText)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmAddText") );
  pAGLSym_GetAlarmSCANOperand = (LPFN_AGLSym_GetAlarmSCANOperand)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmSCANOperand") );
  pAGLSym_GetAlarmSCANInterval = (LPFN_AGLSym_GetAlarmSCANInterval)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmSCANInterval") );
  pAGLSym_GetAlarmSCANAddValue = (LPFN_AGLSym_GetAlarmSCANAddValue)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmSCANAddValue") );
  pAGLSym_GetAlarmSCANOperandEx = (LPFN_AGLSym_GetAlarmSCANOperandEx)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmSCANOperandEx") );
  pAGLSym_GetAlarmSCANAddValueEx = (LPFN_AGLSym_GetAlarmSCANAddValueEx)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetAlarmSCANAddValueEx") );
  pAGLSym_FormatMessage = (LPFN_AGLSym_FormatMessage)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FormatMessage") );
  pAGLSym_FindFirstTextlib = (LPFN_AGLSym_FindFirstTextlib)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindFirstTextlib") );
  pAGLSym_FindNextTextlib = (LPFN_AGLSym_FindNextTextlib)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindNextTextlib") );
  pAGLSym_FindCloseTextlib = (LPFN_AGLSym_FindCloseTextlib)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindCloseTextlib") );
  pAGLSym_SelectTextlib = (LPFN_AGLSym_SelectTextlib)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_SelectTextlib") );
  pAGLSym_FindFirstTextlibText = (LPFN_AGLSym_FindFirstTextlibText)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindFirstTextlibText") );
  pAGLSym_FindNextTextlibText = (LPFN_AGLSym_FindNextTextlibText)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindNextTextlibText") );
  pAGLSym_FindCloseTextlibText = (LPFN_AGLSym_FindCloseTextlibText)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_FindCloseTextlibText") );
  pAGLSym_GetTextlibText = (LPFN_AGLSym_GetTextlibText)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetTextlibText") );
  pAGLSym_GetTextFromValue = (LPFN_AGLSym_GetTextFromValue)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetTextFromValue") );
  pAGLSym_GetValueFromText = (LPFN_AGLSym_GetValueFromText)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetValueFromText") );
  pAGLSym_GetRealFromText = (LPFN_AGLSym_GetRealFromText)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetRealFromText") );
  pAGLSym_GetTextFromReal = (LPFN_AGLSym_GetTextFromReal)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_GetTextFromReal") );
  pAGLSym_SetLanguage = (LPFN_AGLSym_SetLanguage)GetProcAddress( (HMODULE)hLib, __DLT("AGLSym_SetLanguage") );
#endif

  return 1;
}
#if defined( __cplusplus )
}
#endif
