/*******************************************************************************

 Projekt        : Neue Version der AGLink-Bibliothek

 Dateiname      : AGL_Funcs.H

 Beschreibung   : Definition der öffentlichen Funktionen

 Copyright      : (c) 1998-2017
                  DELTALOGIC Automatisierungstechnik GmbH
                  Stuttgarter Str. 3
                  73525 Schwäbisch Gmünd
                  Web : http://www.deltalogic.de
                  Tel.: +49-7171-916120
                  Fax : +49-7171-916220

 Erstellt       : 23.03.2004  RH

 Geändert       : 16.01.2017 JBa

 *******************************************************************************/

#if !defined( __AGL_FUNCS__ )
#define __AGL_FUNCS__

/*******************************************************************************

 Deklaration der Funktionen

 *******************************************************************************/

#if defined( __cplusplus )
  extern "C" {
#endif

//
// Lizenzierungsschlüssel für Entwicklerlizenzen eingeben (Muss als erstes durchgeführt werden)
//
void          AGL_API AGL_Activate                      ( agl_cstr8_t Key );

//
// Versionsinformationen ermitteln
//
void          AGL_API AGL_GetVersion                    ( agl_int32_t* Major, agl_int32_t* Minor );
void          AGL_API AGL_GetVersionEx                  ( agl_int32_t* Major, agl_int32_t* Minor, agl_int32_t* Build, agl_int32_t* Revision, agl_cstr8_t Date );
agl_int32_t   AGL_API AGL_GetOptions                    ( void );
agl_int32_t   AGL_API AGL_GetSerialNumber               ( void );
agl_cstr8_t   AGL_API AGL_GetClientName                 ( agl_cstr8_t Name );

//
// Konfigurationsinformationen der Bibliothek ermitteln/setzen
//
agl_int32_t   AGL_API AGL_GetMaxDevices                  ( void );
agl_int32_t   AGL_API AGL_GetMaxQueues                   ( void );
agl_int32_t   AGL_API AGL_GetMaxPLCPerDevice             ( void );
void          AGL_API AGL_UseSystemTime                  ( agl_bool_t Flag );
void          AGL_API AGL_ReturnJobNr                    ( agl_bool_t Flag );
void          AGL_API AGL_SetBSendAutoResponse           ( agl_bool_t Flag );
agl_bool_t    AGL_API AGL_GetBSendAutoResponse           ( void );

//
// Umgebungsinformationen ermitteln/setzen (keine Kommunikationsverbindung)
//
agl_int32_t   AGL_API AGL_GetPCCPConnNames               ( agl_cstr8_t Names, agl_int32_t Len );
agl_int32_t   AGL_API AGL_GetPCCPProtocol                ( agl_cstr8_t Name );
agl_int32_t   AGL_API AGL_GetTapiModemNames              ( agl_cstr8_t Names, agl_int32_t Len );
agl_int32_t   AGL_API AGL_GetLocalIPAddresses            ( agl_ulong32_t* Addresses, agl_ulong32_t NumAddresses );
agl_int32_t   AGL_API AGL_GetAdaptersInfo                ( LPADAPTER_INFO p_pAdapterInfo, agl_uint32_t MaxAdapters );

//
// Zeitinformationen ermitteln
//
agl_ulong32_t  AGL_API AGL_GetTickCount                   ( void );
agl_ulong32_t  AGL_API AGL_GetMicroSecs                   ( void );

//
// Dynamisch geladene DLL wieder entladen
//
void          AGL_API AGL_UnloadDyn                      ( void );

//
// Konfigurationsprogramm starten
//
agl_int32_t   AGL_API AGL_Config                         ( agl_int32_t DevNr );
agl_int32_t   AGL_API AGL_ConfigEx                       ( agl_int32_t DevNr, agl_cstr8_t CmdLine );

//
// Parameter einstellen/lesen
//
agl_int32_t   AGL_API AGL_SetParas                       ( agl_int32_t DevNr, agl_int32_t ParaType, void* Para, agl_int32_t Len );
agl_int32_t   AGL_API AGL_GetParas                       ( agl_int32_t DevNr, agl_int32_t ParaType, void* Para, agl_int32_t Len );
agl_int32_t   AGL_API AGL_SetDevType                     ( agl_int32_t DevNr, agl_int32_t DevType );
agl_int32_t   AGL_API AGL_GetDevType                     ( agl_int32_t DevNr );

agl_int32_t   AGL_API AGL_ReadParas                      ( agl_int32_t DevNr, agl_int32_t ParaType );
agl_int32_t   AGL_API AGL_WriteParas                     ( agl_int32_t DevNr, agl_int32_t ParaType );
agl_int32_t   AGL_API AGL_ReadDevice                     ( agl_int32_t DevNr );
agl_int32_t   AGL_API AGL_WriteDevice                    ( agl_int32_t DevNr );

agl_int32_t   AGL_API AGL_ReadParasFromFile              ( agl_int32_t DevNr, agl_cstr8_t FileName );
agl_int32_t   AGL_API AGL_WriteParasToFile               ( agl_int32_t DevNr, agl_cstr8_t FileName );

agl_int32_t   AGL_API AGL_GetParaPath                    ( agl_cstr8_t DirName, agl_int32_t MaxLen );
agl_int32_t   AGL_API AGL_SetParaPath                    ( const agl_cstr8_t DirName );
agl_int32_t   AGL_API AGL_SetAndGetParaPath              ( agl_cstr8_t CompanyName, agl_cstr8_t ProductName, agl_cstr8_t AktPath, agl_int32_t MaxLen );

//
// Geräteinformationen ermitteln
//
agl_int32_t   AGL_API AGL_GetPLCType                     ( agl_int32_t DevNr, agl_int32_t PlcNr );
agl_int32_t   AGL_API AGL_HasFunc                        ( agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t Func );

//
// Fehlertexte
//
agl_int32_t   AGL_API AGL_LoadErrorFile                  ( agl_cstr8_t FileName );
agl_int32_t   AGL_API AGL_GetErrorMsg                    ( agl_int32_t ErrNr, agl_cstr8_t Msg, agl_int32_t MaxLen );
agl_int32_t   AGL_API AGL_GetErrorCodeName               ( agl_int32_t ErrNr, const agl_cstr8_t* const ErrorCodeName );

//
// Verbindungsaufbau initialisieren
//
agl_int32_t   AGL_API AGL_OpenDevice                     ( agl_int32_t DevNr );
agl_int32_t   AGL_API AGL_CloseDevice                    ( agl_int32_t DevNr );

//
// Ab hier können Funktionen, auch asynchron bearbeitet werden
//

//
// Asynchrone Aufrufe verwalten
//
agl_int32_t   AGL_API AGL_SetDevNotification             ( agl_int32_t DevNr, LPNOTIFICATION pN );
agl_int32_t   AGL_API AGL_SetConnNotification            ( agl_int32_t ConnNr, LPNOTIFICATION pN );
agl_int32_t   AGL_API AGL_SetJobNotification             ( agl_int32_t DevNr, agl_int32_t JobNr, LPNOTIFICATION pN );
agl_int32_t   AGL_API AGL_SetJobNotificationEx           ( agl_int32_t DevNr, agl_int32_t JobNr, LPNOTIFICATION pN );
agl_int32_t   AGL_API AGL_GetJobResult                   ( agl_int32_t DevNr, agl_int32_t JobNr, LPRESULT40 pR );
agl_int32_t   AGL_API AGL_GetLastJobResult               ( agl_int32_t ConnNr, LPRESULT40 pR );
agl_int32_t   AGL_API AGL_DeleteJob                      ( agl_int32_t DevNr, agl_int32_t JobNr );
agl_int32_t   AGL_API AGL_WaitForJob                     ( agl_int32_t DevNr, agl_int32_t JobNr );
agl_int32_t   AGL_API AGL_WaitForJobEx                   ( agl_int32_t DevNr, agl_int32_t JobNr, LPRESULT40 pR );

//
// Verbindung zum Adapter auf- und abbauen
// Achtung: Sämtliche übergebenen Puffer müssen über die Laufzeit der Funktionen gültig bleiben!
//
agl_int32_t   AGL_API AGL_DialUp                         ( agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_HangUp                         ( agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_InitAdapter                    ( agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitAdapter                    ( agl_int32_t DevNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

//
// Ab hier die Funktionen nur aufrufen, wenn Adapter auch initialisiert wurde
//

//
// Durch den Adapter ermittelbare Informationen
//
agl_int32_t   AGL_API AGL_GetLifelist                    ( agl_int32_t DevNr, agl_uint8_t* List, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetDirectPLC                   ( agl_int32_t DevNr, agl_uint8_t* pPlc, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

//
// Verbindung bis zur Steuerung auf- und abbauen
//
agl_int32_t   AGL_API AGL_PLCConnect                     ( agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t* ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCConnectEx                   ( agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t RackNr, agl_int32_t SlotNr, agl_int32_t* ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCDisconnect                  ( agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

//
// Ab hier die Funktionen nur aufrufen, wenn auch eine Verbindung besteht
//

//
// Verbindungsabhängige Informationen ermittlen
//
agl_int32_t   AGL_API AGL_ReadMaxPacketSize              ( agl_int32_t ConnNr );
agl_int32_t   AGL_API AGL_GetRedConnState                ( agl_int32_t ConnNr, LPRED_CONN_STATE pState );
agl_int32_t   AGL_API AGL_GetRedConnStateMsg             ( agl_int32_t ConnNr, LPRED_CONN_STATE pState, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

//
// Kommunikationsfunktionen
//

//
// Daten lesen/schreiben
//
agl_int32_t   AGL_API AGL_ReadOpState                    ( agl_int32_t ConnNr, agl_int32_t* State, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadOpStateEx                  ( agl_int32_t ConnNr, agl_int32_t* State, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetPLCStartOptions             ( agl_int32_t ConnNr, agl_int32_t* StartOptions, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCStop                        ( agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCStart                       ( agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCResume                      ( agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCColdStart                   ( agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_IsHPLC                         ( agl_int32_t ConnNr, agl_int32_t* IsHPLC, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_HPLCStop                       ( agl_int32_t ConnNr, agl_int32_t CPUNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_HPLCStart                      ( agl_int32_t ConnNr, agl_int32_t CPUNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_HPLCColdStart                  ( agl_int32_t ConnNr, agl_int32_t CPUNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_GetPLCClock                    ( agl_int32_t ConnNr, LPTOD pTOD, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_SetPLCClock                    ( agl_int32_t ConnNr, LPTOD pTOD, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_SyncPLCClock                   ( agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_ReadMLFBNr                     ( agl_int32_t ConnNr, LPMLFB pMLFBNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadMLFBNrEx                   ( agl_int32_t ConnNr, LPMLFBEX pMLFBNrEx, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadPLCInfo                    ( agl_int32_t ConnNr, LPPLCINFO pPLCInfo, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadCycleTime                  ( agl_int32_t ConnNr, LPCYCLETIME pCycleTime, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadProtLevel                  ( agl_int32_t ConnNr, LPPROTLEVEL pProtLevel, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadS7Ident                    ( agl_int32_t ConnNr, LPS7_IDENT pIdent, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadS7LED                      ( agl_int32_t ConnNr, LPS7_LED pLed, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetExtModuleInfo               ( agl_int32_t ConnNr, LPEXT_MODULE_INFO pInfo, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadSzl                        ( agl_int32_t ConnNr, agl_int32_t SzlId, agl_int32_t Index, agl_uint8_t* Buff, agl_int32_t* BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_IsPasswordReq                  ( agl_int32_t ConnNr, agl_int32_t* IsPWReq, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_SetPassword                    ( agl_int32_t ConnNr, agl_cstr8_t PW, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_SetPasswordEx                  ( agl_int32_t ConnNr, const agl_cstr8_t const PW, agl_uint32_t* NewProtectionLevel, const agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_UnSetPassword                  ( agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_ReadDiagBufferEntrys           ( agl_int32_t ConnNr, agl_int32_t* Entrys, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadDiagBuffer                 ( agl_int32_t ConnNr, agl_int32_t* Entrys, agl_uint8_t* pDiagBuff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetDiagBufferEntry             ( agl_int32_t Index, agl_uint8_t* pDiagBuff, agl_cstr8_t Text, agl_int32_t TextLen );

agl_int32_t   AGL_API AGL_ReadDBCount                    ( agl_int32_t ConnNr, agl_int32_t* DBCount, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadDBList                     ( agl_int32_t ConnNr, agl_int32_t* DBCount, agl_uint16_t* DBList, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadDBLen                      ( agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t* DBLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_ReadAllBlockCount              ( agl_int32_t ConnNr, LPALL_BLOCK_COUNT pBC, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadBlockCount                 ( agl_int32_t ConnNr, agl_int32_t BlockType, agl_int32_t* BlockCount, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadBlockList                  ( agl_int32_t ConnNr, agl_int32_t BlockType, agl_int32_t* BlockCount, agl_uint16_t* BlockList, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadBlockLen                   ( agl_int32_t ConnNr, agl_int32_t BlockType, agl_int32_t BlockNr, agl_int32_t* BlockLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_ReadInBytes                    ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadPInBytes                   ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadOutBytes                   ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadFlagBytes                  ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadSFlagBytes                 ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadVarBytes                   ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadDataBytes                  ( agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadDataWords                  ( agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadTimerWords                 ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadCounterWords               ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadMix                        ( agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadMixEx                      ( agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_WriteInBytes                   ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteOutBytes                  ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WritePOutBytes                 ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteFlagBytes                 ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteSFlagBytes                ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteVarBytes                  ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteDataBytes                 ( agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint8_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteDataWords                 ( agl_int32_t ConnNr, agl_int32_t DBNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteTimerWords                ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteCounterWords              ( agl_int32_t ConnNr, agl_int32_t Start, agl_int32_t Num, agl_uint16_t* Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteMix                       ( agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteMixEx                     ( agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

//
// Daten optimiert lesen/schreiben
//
agl_int32_t   AGL_API AGL_InitOptReadMix                 ( agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt );
agl_int32_t   AGL_API AGL_ReadOptReadMix                 ( agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_EndOptReadMix                  ( agl_ptrdiff_t Opt );

agl_int32_t   AGL_API AGL_InitOptReadMixEx               ( agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt );
agl_int32_t   AGL_API AGL_ReadOptReadMixEx               ( agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_EndOptReadMixEx                ( agl_ptrdiff_t Opt );

agl_int32_t   AGL_API AGL_InitOptWriteMix                ( agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt );
agl_int32_t   AGL_API AGL_WriteOptWriteMix               ( agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_EndOptWriteMix                 ( agl_ptrdiff_t Opt );

agl_int32_t   AGL_API AGL_InitOptWriteMixEx              ( agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_ptrdiff_t* Opt );
agl_int32_t   AGL_API AGL_WriteOptWriteMixEx             ( agl_int32_t ConnNr, agl_ptrdiff_t Opt, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_EndOptWriteMixEx               ( agl_ptrdiff_t Opt );

agl_int32_t   AGL_API AGL_SetOptNotification             ( agl_ptrdiff_t Opt, LPNOTIFICATION pN );
agl_int32_t   AGL_API AGL_DeleteOptJob                   ( agl_int32_t ConnNr, agl_ptrdiff_t Opt );
agl_int32_t   AGL_API AGL_GetOptJobResult                ( agl_int32_t ConnNr, agl_ptrdiff_t Opt, LPRESULT40 pR );
agl_int32_t   AGL_API AGL_WaitForOptJob                  ( agl_int32_t ConnNr, agl_ptrdiff_t Opt );

agl_ptrdiff_t AGL_API AGL_AllocRWBuffs                   ( LPDATA_RW40 Buff, agl_int32_t Num );
agl_int32_t   AGL_API AGL_FreeRWBuffs                    ( agl_ptrdiff_t Handle );
agl_int32_t   AGL_API AGL_ReadRWBuff                     ( LPDATA_RW40 Buff, agl_int32_t Index, void* pData );
agl_int32_t   AGL_API AGL_WriteRWBuff                    ( LPDATA_RW40 Buff, agl_int32_t Index, void* pData );

//
// Funktionen für die RK512/3964/3964R-Kommunikation
//
agl_int32_t   AGL_API AGL_RKSend                         ( agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RKSendEx                       ( agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RKFetch                        ( agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RKFetchEx                      ( agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Send_RKFetch                   ( agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Recv_RKSend                    ( agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Recv_RKFetch                   ( agl_int32_t ConnNr, LPDATA_RW40_RK Buff, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Send_3964                      ( agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Recv_3964                      ( agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t* BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

//
// Kommunikationsfunktionen nur für projektierte Verbindungen
//
agl_int32_t   AGL_API AGL_BSend                          ( agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_BReceive                       ( agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_BSendEx                        ( agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t R_ID, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_BReceiveEx                     ( agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t* R_ID, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_USend                          ( agl_int32_t ConnNr, LPS7_USEND_URCV pUSR, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_UReceive                       ( agl_int32_t ConnNr, LPS7_USEND_URCV pUSR, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

//
// Funktionen für Alarme, Scans, Archive, Zyklisches Lesen
//
agl_int32_t   AGL_API AGL_InitOpStateMsg                 ( agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitOpStateMsg                 ( agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetOpStateMsg                  ( agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_InitDiagMsg                    ( agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t DiagMask, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitDiagMsg                    ( agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetDiagMsg                     ( agl_int32_t ConnNr, LPS7_DIAG_MSG pDiag, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_InitCyclicRead                 ( agl_int32_t ConnNr, agl_int32_t CycleTime, agl_int32_t Flags, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t* Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_InitCyclicReadEx               ( agl_int32_t ConnNr, agl_int32_t CycleTime, agl_int32_t Flags, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t* Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_StartCyclicRead                ( agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_StopCyclicRead                 ( agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitCyclicRead                 ( agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetCyclicRead                  ( agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetCyclicReadEx                ( agl_int32_t ConnNr, LPDATA_RW40 Buff, agl_int32_t Num, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_InitScanMsg                    ( agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitScanMsg                    ( agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetScanMsg                     ( agl_int32_t ConnNr, LPS7_SCAN pScan, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_HasAckTriggeredMsg             ( agl_int32_t ConnNr, agl_int32_t* Mode, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_InitAlarmMsg                   ( agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitAlarmMsg                   ( agl_int32_t ConnNr, agl_int32_t* OpState, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetAlarmMsg                    ( agl_int32_t ConnNr, LPS7_ALARM pAlarm, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_ReadOpenMsg                    ( agl_int32_t ConnNr, LPS7_OPEN_MSG_STATE pState, agl_int32_t* MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_GetMsgStateChange              ( agl_int32_t ConnNr, LPS7_RCV_MSG_STATE pState, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_AckMsg                         ( agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_LockMsg                        ( agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_UnlockMsg                      ( agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t MsgAnz, agl_int32_t MsgType, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_InitARSend                     ( agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t ArAnz, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitARSend                     ( agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, agl_int32_t ArAnz, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetARSend                      ( agl_int32_t ConnNr, agl_wsabuf_t* pwsa, agl_int32_t* AR_ID, agl_int32_t Timeout, agl_ptrdiff_t UserVal );


/*******************************************************************************

 RFC1006-Funktionen

*******************************************************************************/

agl_int32_t   AGL_API AGL_RFC1006_Connect                ( agl_int32_t DevNr, agl_int32_t PlcNr, agl_int32_t* ConnNr, LPRFC_1006_SERVER ConnInfo, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RFC1006_Disconnect             ( agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RFC1006_Receive                ( agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t BuffLen, agl_int32_t* ReceivedLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RFC1006_Send                   ( agl_int32_t ConnNr, agl_uint8_t* Buff, agl_int32_t BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal );


/*******************************************************************************

 NCK-Funktionen

*******************************************************************************/

//
// Lesen und Schreiben von Variablen
//
agl_int32_t   AGL_API AGL_NCK_ReadMixEx                  ( agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_WriteMixEx                 ( agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_CheckVarSize               ( agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

//
// Zyklisches Lesen von Variablen
//
agl_int32_t   AGL_API AGL_NCK_InitCyclicReadEx           ( agl_int32_t ConnNr, agl_int32_t CycleTime, agl_int32_t OnlyChanged, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t* Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_StartCyclicRead            ( agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_StopCyclicRead             ( agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_ExitCyclicRead             ( agl_int32_t ConnNr, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_GetCyclicReadEx            ( agl_int32_t ConnNr, LPNCKDataRW Buff, agl_int32_t Num, agl_int32_t Handle, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

//
// Ausführen von PI-Diensten
//
agl_int32_t   AGL_API AGL_NCK_PI_EXTERN                  ( agl_int32_t ConnNr, agl_int32_t Channel, agl_cstr8_t ProgName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_EXTMOD                  ( agl_int32_t ConnNr, agl_int32_t Channel, agl_cstr8_t ProgName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_SELECT                  ( agl_int32_t ConnNr, agl_int32_t Channel, agl_cstr8_t ProgName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_DELE                  ( agl_int32_t ConnNr, agl_cstr8_t FileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_PROT                  ( agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t Protection, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_RENA                  ( agl_int32_t ConnNr, agl_cstr8_t OldFileName, agl_cstr8_t NewFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_XFER                  ( agl_int32_t ConnNr, agl_cstr8_t FileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_LOGIN                   ( agl_int32_t ConnNr, agl_cstr8_t Password, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_LOGOUT                  ( agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_NCK_PI_F_OPEN                  ( agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t WindowName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_OPER                  ( agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t WindowName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_SEEK                  ( agl_int32_t ConnNr, agl_cstr8_t WindowName, agl_int32_t SeekMode, agl_int32_t SeekPointer, agl_int32_t WindowSize, agl_cstr8_t CompareString, agl_int32_t SkipCount, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_CLOS                  ( agl_int32_t ConnNr, agl_cstr8_t WindowName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_NCK_PI_StartAll                ( agl_int32_t ConnNr, agl_uint8_t* Para, agl_int32_t ParaLen, agl_cstr8_t Cmd, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

//
// Ab hier neu für 5.1
//
agl_int32_t   AGL_API AGL_NCK_PI_F_COPY                  ( agl_int32_t ConnNr, agl_int32_t Direction, agl_cstr8_t SourceFileName, agl_cstr8_t DestinationFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_PROR                  ( agl_int32_t ConnNr, agl_cstr8_t FileName, agl_cstr8_t Protection, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_CANCEL                  ( agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_CRCEDN                  ( agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t DNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_DELECE                  ( agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t DNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_DELETO                  ( agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_IBN_SS                  ( agl_int32_t ConnNr, agl_int32_t Switch, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_MMCSEM                  ( agl_int32_t ConnNr, agl_int32_t ChannelNumber, agl_int32_t FunctionNumber, agl_int32_t SemaValue, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_TMCRTO                  ( agl_int32_t ConnNr, agl_int32_t ToolArea, agl_cstr8_t ToolID, agl_int32_t ToolNumber, agl_int32_t DuploNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_TMMVTL                  ( agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t ToolNumber, agl_int32_t SourcePlaceNumber, agl_int32_t SourceMagazineNumber, agl_int32_t DestinationPlaceNumber, agl_int32_t DestinationMagazineNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_TMCRTC                  ( agl_int32_t ConnNr, agl_int32_t ToolArea, agl_cstr8_t ToolID, agl_int32_t ToolNumber, agl_int32_t DuploNumber, agl_int32_t EdgeNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_CREATO                  ( agl_int32_t ConnNr, agl_int32_t ToolArea, agl_int32_t TNumber, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

//
// Dateitransferfunktionen
//
agl_int32_t   AGL_API AGL_NCK_CopyFileToNC               ( agl_int32_t ConnNr, agl_cstr8_t NCFileName, agl_cstr8_t PCFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_CopyFileFromNC             ( agl_int32_t ConnNr, agl_cstr8_t NCFileName, agl_cstr8_t PCFileName, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_NCK_CopyToNC                   ( agl_int32_t ConnNr, agl_cstr8_t FileName, void* Buff, agl_int32_t BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_CopyFromNC                 ( agl_int32_t ConnNr, agl_cstr8_t FileName, void* Buff, agl_int32_t BuffLen, agl_int32_t* NeededLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_CopyFromNCAlloc            ( agl_int32_t ConnNr, agl_cstr8_t FileName, void** Buff, agl_int32_t* BuffLen, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_FreeBuff                   ( void* Buff );

agl_int32_t   AGL_API AGL_NCK_SetConnProgressNotification( agl_int32_t ConnNr, LPNOTIFICATION pN );

//
// Einlesen von Variableninformationen aus verschiedenen Dateien (Typen und Formaten)
//
agl_int32_t   AGL_API AGL_NCK_CheckNSKVarLine            ( agl_cstr8_t Line, LPNCKDataRW pRW, agl_cstr8_t Name, agl_int32_t NameLen );
agl_int32_t   AGL_API AGL_NCK_ReadNSKVarFile             ( agl_cstr8_t FileName, LPNCKDataRW* ppRW, agl_cstr8_t** pName );
agl_int32_t   AGL_API AGL_NCK_CheckCSVVarLine            ( agl_cstr8_t Line, LPNCKDataRW pRW, agl_cstr8_t Name, agl_int32_t NameLen );
agl_int32_t   AGL_API AGL_NCK_ReadCSVVarFile             ( agl_cstr8_t FileName, LPNCKDataRW* ppRW, agl_cstr8_t** pName );
agl_int32_t   AGL_API AGL_NCK_ReadGUDVarFile             ( agl_cstr8_t FileName, LPNCKDataRW* ppRW, agl_cstr8_t** pName );
agl_int32_t   AGL_API AGL_NCK_ReadGUDVarFileEx           ( agl_cstr8_t FileName, agl_int32_t GUDNr, agl_int32_t Area, LPNCKDataRW* ppRW, agl_cstr8_t** pName );
agl_int32_t   AGL_API AGL_NCK_FreeVarBuff                ( LPNCKDataRW* ppRW, agl_cstr8_t** pName, agl_int32_t Anz );
agl_int32_t   AGL_API AGL_NCK_GetSingleVarDef            ( LPNCKDataRW* ppRW, agl_cstr8_t** pName, agl_int32_t Index, LPNCKDataRW pRW, agl_cstr8_t Name, agl_int32_t NameLen, agl_int32_t AllocBuff );

agl_int32_t   AGL_API AGL_NCK_ExtractNckAlarm            ( void* Buffer, agl_int32_t BufferSize, LPNCKAlarm NCKAlarm );
agl_int32_t   AGL_API AGL_NCK_GetNCKDataRWByNCDDEItem    ( const agl_cstr8_t const Item, LPNCKDataRW DataRW, agl_int32_t* ErrorPosition);


/*******************************************************************************

 Antriebs-Funktionen

*******************************************************************************/

//
// Lesen und Schreiben von Parametern
//
agl_int32_t   AGL_API AGL_Drive_ReadMix                  ( agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Drive_ReadMixEx                ( agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Drive_WriteMix                 ( agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Drive_WriteMixEx               ( agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, agl_int32_t Num, agl_int32_t Timeout, agl_ptrdiff_t UserVal );


/*******************************************************************************

 Speicherverwaltungsfunktionen

*******************************************************************************/

void*         AGL_API AGL_malloc                         ( agl_int32_t Size );
void*         AGL_API AGL_calloc                         ( agl_int32_t Anz, agl_int32_t Size );
void*         AGL_API AGL_realloc                        ( void* Ptr, agl_int32_t Size );
void*         AGL_API AGL_memcpy                         ( void* DestPtr, void* SrcPtr, agl_int32_t Len );
void*         AGL_API AGL_memmove                        ( void* DestPtr, void* SrcPtr, agl_int32_t Len );
agl_int32_t   AGL_API AGL_memcmp                         ( void* Ptr1, void* Ptr2, agl_int32_t Len );
void          AGL_API AGL_free                           ( void* Ptr );


/*******************************************************************************

 Konvertierfunktionen (diese benötigen keine Kommunikationsverbindung und arbeiten nur im Speicher)

*******************************************************************************/

agl_int16_t   AGL_API AGL_ReadInt16                      ( agl_uint8_t* Buff );
agl_long32_t   AGL_API AGL_ReadInt32                      ( agl_uint8_t* Buff );
agl_uint16_t  AGL_API AGL_ReadWord                       ( agl_uint8_t* Buff );
agl_ulong32_t  AGL_API AGL_ReadDWord                      ( agl_uint8_t* Buff );
agl_float32_t AGL_API AGL_ReadReal                       ( agl_uint8_t* Buff );
agl_int32_t   AGL_API AGL_ReadS5Time                     ( agl_uint8_t* Buff );
agl_int32_t   AGL_API AGL_ReadS5TimeW                    ( agl_uint16_t* Buff );

void          AGL_API AGL_WriteInt16                     ( agl_uint8_t* Buff, agl_int16_t Val );
void          AGL_API AGL_WriteInt32                     ( agl_uint8_t* Buff, agl_long32_t Val );
void          AGL_API AGL_WriteWord                      ( agl_uint8_t* Buff, agl_uint16_t Val );
void          AGL_API AGL_WriteDWord                     ( agl_uint8_t* Buff, agl_ulong32_t Val );
void          AGL_API AGL_WriteReal                      ( agl_uint8_t* Buff, agl_float32_t Val );
void          AGL_API AGL_WriteS5Time                    ( agl_uint8_t* Buff, agl_int32_t Val );
void          AGL_API AGL_WriteS5TimeW                   ( agl_uint16_t* Buff, agl_int32_t Val );

void          AGL_API AGL_Byte2Word                      ( agl_uint16_t* OutBuff, agl_uint8_t* InBuff, agl_int32_t AnzWords );
void          AGL_API AGL_Byte2DWord                     ( agl_ulong32_t* OutBuff, agl_uint8_t* InBuff, agl_int32_t AnzDWords );
void          AGL_API AGL_Byte2Real                      ( agl_float32_t* OutBuff, agl_uint8_t* InBuff, agl_int32_t AnzReals );
void          AGL_API AGL_Word2Byte                      ( agl_uint8_t* OutBuff, agl_uint16_t* InBuff, agl_int32_t AnzWords );
void          AGL_API AGL_DWord2Byte                     ( agl_uint8_t* OutBuff, agl_ulong32_t* InBuff, agl_int32_t AnzDWords );
void          AGL_API AGL_Real2Byte                      ( agl_uint8_t* OutBuff, agl_float32_t* InBuff, agl_int32_t AnzReals );

agl_int32_t   AGL_API AGL_GetBit                         ( agl_uint8_t Wert, agl_int32_t BitNr );
agl_uint8_t   AGL_API AGL_SetBit                         ( agl_uint8_t* Buff, agl_int32_t BitNr );
agl_uint8_t   AGL_API AGL_ResetBit                       ( agl_uint8_t* Buff, agl_int32_t BitNr );
agl_uint8_t   AGL_API AGL_SetBitVal                      ( agl_uint8_t* Buff, agl_int32_t BitNr, agl_int32_t Val );

agl_int32_t   AGL_API AGL_Buff2String                    ( agl_uint8_t* Buff, agl_cstr8_t Text, agl_int32_t AnzChars );
agl_int32_t   AGL_API AGL_String2Buff                    ( agl_uint8_t* Buff, agl_cstr8_t Text, agl_int32_t AnzChars );
agl_int32_t   AGL_API AGL_Buff2WString                   ( agl_uint8_t* Buff, agl_wchar_t* Text, agl_int32_t AnzChars );
agl_int32_t   AGL_API AGL_WString2Buff                   ( agl_uint8_t* Buff, agl_wchar_t* Text, agl_int32_t AnzChars );
agl_int32_t   AGL_API AGL_S7String2String                ( agl_uint8_t* S7String, agl_cstr8_t Text, agl_int32_t MaxChars );
agl_int32_t   AGL_API AGL_String2S7String                ( agl_uint8_t* S7String, agl_cstr8_t Text, agl_int32_t MaxChars );

agl_int32_t   AGL_API AGL_BCD2Int16                      ( agl_int16_t BCD, agl_int16_t* Dual );
agl_int32_t   AGL_API AGL_BCD2Int32                      ( agl_long32_t BCD, agl_long32_t* Dual );
agl_int32_t   AGL_API AGL_Int162BCD                      ( agl_int16_t Dual, agl_int16_t* BCD );
agl_int32_t   AGL_API AGL_Int322BCD                      ( agl_long32_t Dual, agl_long32_t* BCD );
agl_float32_t AGL_API AGL_LongAsFloat                    ( agl_long32_t Var );
agl_long32_t   AGL_API AGL_FloatAsLong                    ( agl_float32_t Var );

agl_int32_t   AGL_API AGL_Text2DataRW                    ( agl_cstr8_t Text, LPDATA_RW40 RW );
agl_int32_t   AGL_API AGL_DataRW2Text                    ( LPDATA_RW40 RW, agl_cstr8_t Text );

agl_int32_t   AGL_API AGL_S7DT2SysTime                   ( agl_uint8_t* Buff, agl_systemtime_t* SysTime );
agl_int32_t   AGL_API AGL_SysTime2S7DT                   ( agl_systemtime_t* SysTime, agl_uint8_t* Buff );
agl_int32_t   AGL_API AGL_TOD2SysTime                    ( LPTOD pTOD, agl_systemtime_t* SysTime );
agl_int32_t   AGL_API AGL_SysTime2TOD                    ( agl_systemtime_t* SysTime, LPTOD pTOD );
agl_int32_t   AGL_API AGL_S7Date2YMD                     ( agl_uint16_t Date, agl_uint16_t* Year, agl_uint16_t* Month, agl_uint16_t* Day );

agl_int32_t   AGL_API AGL_Float2KG                       ( agl_uint16_t* pKG, agl_float32_t* pFloat, agl_int32_t AnzFloats );
agl_int32_t   AGL_API AGL_KG2Float                       ( agl_float32_t* pFloat, agl_uint16_t* pKG, agl_int32_t AnzFloats );
agl_int32_t   AGL_API AGL_Float2DWKG                     ( agl_ulong32_t* pKG, agl_float32_t* pFloat, agl_int32_t AnzFloats );
agl_int32_t   AGL_API AGL_DWKG2Float                     ( agl_float32_t* pFloat, agl_ulong32_t* pKG, agl_int32_t AnzFloats );

agl_int32_t   AGL_API AGL_S7Ident2String                 ( LPS7_IDENT pIdent, agl_int32_t Index, agl_cstr8_t Text, agl_int32_t MaxChars );


/*******************************************************************************

 Symbolik-Funktionen

*******************************************************************************/

//
// Versionsabfrage, Lizenznehmerdaten, Aktivierung, Fehlermeldungstext
//
#define AGLSym_GetVersion                         AGL_GetVersion
#define AGLSym_GetVersionEx                       AGL_GetVersionEx

#define AGLSym_GetSerialNumber                    AGL_GetSerialNumber
#define AGLSym_GetClientName                      AGL_GetClientName
#define AGLSym_Activate                           AGL_Activate

#define AGLSym_GetErrorMsg                        AGL_GetErrorMsg

//
// STEP7(r)-Projekt öffnen bzw. schließen
//
agl_int32_t   AGL_API AGLSym_OpenProject                 ( agl_cstr8_t  Project, agl_ptrdiff_t* PrjHandle );
agl_int32_t   AGL_API AGLSym_CloseProject                ( agl_ptrdiff_t PrjHandle );

//
// Liste der unterstützten S7-CPU-Typen in Textdatei schreiben
//
agl_int32_t   AGL_API AGLSym_WriteCpuListToFile          ( agl_cstr8_t  FileName );

//
// S7-Programme aufzählen und für den Zugriff auswählen
//
agl_int32_t   AGL_API AGLSym_GetProgramCount             ( agl_ptrdiff_t PrjHandle, agl_int32_t* ProgCount );
agl_int32_t   AGL_API AGLSym_FindFirstProgram            ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  Program );
agl_int32_t   AGL_API AGLSym_FindNextProgram             ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  Program );
agl_int32_t   AGL_API AGLSym_FindCloseProgram            ( agl_ptrdiff_t PrjHandle );
agl_int32_t   AGL_API AGLSym_SelectProgram               ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  Program );


//
// Symbole einer Symboltabelle eines S7-Programms aufzählen
//
agl_int32_t   AGL_API AGLSym_GetSymbolCount              ( agl_ptrdiff_t PrjHandle, agl_int32_t* SymCount);
agl_int32_t   AGL_API AGLSym_GetSymbolCountFilter        ( agl_ptrdiff_t PrjHandle, agl_int32_t* SymCount, const agl_cstr8_t Filter);

agl_int32_t   AGL_API AGLSym_FindFirstSymbol             ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format );
agl_int32_t   AGL_API AGLSym_FindFirstSymbolFilter       ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format, const agl_cstr8_t Filter);
agl_int32_t   AGL_API AGLSym_FindNextSymbol              ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format );
agl_int32_t   AGL_API AGLSym_FindCloseSymbol             ( agl_ptrdiff_t PrjHandle );


//
// Datenbausteine eines S7-Programms aufzählen
//
agl_int32_t   AGL_API AGLSym_ReadPrjDBCount              ( agl_ptrdiff_t PrjHandle, agl_int32_t* DBCount );
agl_int32_t   AGL_API AGLSym_ReadPrjDBCountFilter        ( agl_ptrdiff_t PrjHandle, agl_int32_t* DBCount, const agl_cstr8_t Filter );

agl_int32_t   AGL_API AGLSym_ReadPrjDBList               ( agl_ptrdiff_t PrjHandle, agl_uint16_t* DBList, agl_int32_t DBCount );
agl_int32_t   AGL_API AGLSym_ReadPrjDBListFilter         ( agl_ptrdiff_t PrjHandle, agl_uint16_t* DBList, agl_int32_t DBCount, const agl_cstr8_t Filter );

//
// Programmbausteine, Datenbausteine und Datentypen eines S7-Programms auflisten
//
agl_int32_t   AGL_API AGLSym_ReadPrjBlkCountFilter       ( agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_int32_t* BlkCount, const agl_cstr8_t Filter );
agl_int32_t   AGL_API AGLSym_ReadPrjBlkListFilter        ( agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_uint16_t* BlkList, agl_int32_t BlkCount, const agl_cstr8_t Filter );


//
// Komponenten (DB-Symbole) eines Datenbausteins im S7-Programm aufzählen
//
agl_int32_t   AGL_API AGLSym_GetDbSymbolCount            ( agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_int32_t* DBSymCount);
agl_int32_t   AGL_API AGLSym_GetDbSymbolCountFilter      ( agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_int32_t* DBSymCount, const agl_cstr8_t Filter);

agl_int32_t   AGL_API AGLSym_FindFirstDbSymbol           ( agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format );
agl_int32_t   AGL_API AGLSym_FindFirstDbSymbolFilter     ( agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format, const agl_cstr8_t Filter );
agl_int32_t   AGL_API AGLSym_FindNextDbSymbol            ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format );
agl_int32_t   AGL_API AGLSym_FindFirstDbSymbolEx         ( agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, LPDATA_DBSYM40 Buff, const agl_cstr8_t Filter );
agl_int32_t   AGL_API AGLSym_FindNextDbSymbolEx          ( agl_ptrdiff_t PrjHandle, LPDATA_DBSYM40 Buff );
agl_int32_t   AGL_API AGLSym_GetDbSymbolExComment        ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  ExComment );
agl_int32_t   AGL_API AGLSym_FindCloseDbSymbol           ( agl_ptrdiff_t PrjHandle );


//
// Abhängigkeit eines DBs ermitteln
//
agl_int32_t   AGL_API AGLSym_GetDbDependency       ( agl_ptrdiff_t PrjHandle, agl_int32_t DBNr, agl_int32_t* BlkType, agl_int32_t* BlkNr );

//
// Deklarationen eines Programmbausteins, Datenbausteins oder Datentyps im S7-Programm auflisten
//
agl_int32_t   AGL_API AGLSym_GetDeclarationCountFilter ( agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_int32_t BlkNr, const agl_cstr8_t Filter, agl_int32_t* DeclarationCount );
agl_int32_t   AGL_API AGLSym_FindFirstDeclarationFilter( agl_ptrdiff_t PrjHandle, agl_int32_t BlkType, agl_int32_t BlkNr, const agl_cstr8_t Filter, agl_ptrdiff_t* FindHandle, LPDATA_DECLARATION Buff );
agl_int32_t   AGL_API AGLSym_FindNextDeclaration       ( agl_ptrdiff_t PrjHandle, agl_ptrdiff_t FindHandle, LPDATA_DECLARATION Buff );
agl_int32_t   AGL_API AGLSym_GetDeclarationInitialValue( agl_ptrdiff_t PrjHandle, agl_ptrdiff_t FindHandle, agl_int32_t* BufferLength, agl_cstr8_t  InitialValue );
agl_int32_t   AGL_API AGLSym_FindCloseDeclaration      ( agl_ptrdiff_t PrjHandle, agl_ptrdiff_t FindHandle );


//
// Übersetzung von Operanden- bzw. Symbolangaben
//
agl_int32_t   AGL_API AGLSym_GetSymbolFromText           ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format );
agl_int32_t   AGL_API AGLSym_GetSymbolFromTextEx         ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, LPDATA_DBSYM40 Buff );
agl_int32_t   AGL_API AGLSym_GetReadMixFromText          ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, LPDATA_RW40 Buff, agl_int32_t* Format );
agl_int32_t   AGL_API AGLSym_GetReadMixFromTextEx        ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  Text, agl_cstr8_t  AbsOpd, LPDATA_RW40 Buff, agl_int32_t* Format );

agl_int32_t   AGL_API AGLSym_GetSymbol                   ( agl_ptrdiff_t PrjHandle, LPDATA_RW40 Buff, agl_cstr8_t  AbsOpd, agl_cstr8_t  Symbol, agl_cstr8_t  Comment, agl_int32_t* Format );
agl_int32_t   AGL_API AGLSym_GetSymbolEx                 ( agl_ptrdiff_t PrjHandle, LPDATA_RW40 Buff, LPDATA_DBSYM40 Symbol );

//
// Zugriff auf die Meldungskonfiguration und die Meldetexte eines S7-Programms
//
agl_int32_t   AGL_API AGLSym_OpenAlarms                  ( agl_ptrdiff_t PrjHandle );
agl_int32_t   AGL_API AGLSym_CloseAlarms                 ( agl_ptrdiff_t PrjHandle );

agl_int32_t   AGL_API AGLSym_FindFirstAlarmData          ( agl_ptrdiff_t PrjHandle, agl_int32_t* AlmNr);
agl_int32_t   AGL_API AGLSym_FindNextAlarmData           ( agl_ptrdiff_t PrjHandle, agl_int32_t* AlmNr);
agl_int32_t   AGL_API AGLSym_FindCloseAlarmData          ( agl_ptrdiff_t PrjHandle );

agl_int32_t   AGL_API AGLSym_GetAlarmData                ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, LPDATA_ALARM40 Buff );

agl_int32_t   AGL_API AGLSym_GetAlarmName                ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_GetAlarmType                ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen );

agl_int32_t   AGL_API AGLSym_GetAlarmBaseName            ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_GetAlarmTypeName            ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_cstr8_t Buff, agl_int32_t BuffLen );

agl_int32_t   AGL_API AGLSym_GetAlarmSignalCount         ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* SignalCount );

agl_int32_t   AGL_API AGLSym_GetAlarmMsgClass            ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* MsgClass );
agl_int32_t   AGL_API AGLSym_GetAlarmPriority            ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* Priority );
agl_int32_t   AGL_API AGLSym_GetAlarmAckGroup            ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* AckGroup );
agl_int32_t   AGL_API AGLSym_GetAlarmAcknowledge         ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t* Acknowledge );

agl_int32_t   AGL_API AGLSym_GetAlarmProtocol            ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* Protocol );
agl_int32_t   AGL_API AGLSym_GetAlarmDispGroup           ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* DispGroup );

//
// Language = Windows LCID bzw. LANGID
//  intern wird nur der LANGID-Anteil LANGIDFROMLCID( LCID ) beachtet, nicht jedoch der SORTID-Anteil
//
agl_int32_t   AGL_API AGLSym_FindFirstAlarmTextLanguage  ( agl_ptrdiff_t PrjHandle, agl_int32_t* Language, agl_int32_t* IsDefault);
agl_int32_t   AGL_API AGLSym_FindNextAlarmTextLanguage   ( agl_ptrdiff_t PrjHandle, agl_int32_t* Language, agl_int32_t* IsDefault);
agl_int32_t   AGL_API AGLSym_FindCloseAlarmTextLanguage  ( agl_ptrdiff_t PrjHandle );

//
// Language = Windows LCID bzw. LANGID
// Language = -1 -> Zurücksetzen auf Anfangswert [ System-Default-Sprache -> ::GetSystemDefaultLCID() ]
//
agl_int32_t   AGL_API AGLSym_SetAlarmTextDefaultLanguage ( agl_ptrdiff_t PrjHandle, agl_int32_t Language );

//
// Language = Windows LCID bzw. LANGID
// Language = -1 -> Verwendung der mittels AGLSym_SetAlarmTextDefaultLanguage() eingestellen Sprache
//
agl_int32_t   AGL_API AGLSym_GetAlarmText                ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t Language, agl_cstr8_t Buff, agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_GetAlarmInfo                ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t Language, agl_cstr8_t Buff, agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_GetAlarmAddText             ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Signal, agl_int32_t Index, agl_int32_t Language, agl_cstr8_t Buff, agl_int32_t BuffLen );

//
// SCAN-Operand, SCAN-Intervall und SCAN-Begleitwerte 1 - 10
//
agl_int32_t   AGL_API AGLSym_GetAlarmSCANOperand         ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* OpArea, agl_int32_t* OpType, agl_int32_t* Offset, agl_int32_t* BitNr );
agl_int32_t   AGL_API AGLSym_GetAlarmSCANInterval        ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t* Interval );
agl_int32_t   AGL_API AGLSym_GetAlarmSCANAddValue        ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Index, agl_int32_t* OpArea, agl_int32_t* OpType, agl_int32_t* Offset, agl_int32_t* BitNr );

agl_int32_t   AGL_API AGLSym_GetAlarmSCANOperandEx       ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, LPDATA_RW40 Buff );
agl_int32_t   AGL_API AGLSym_GetAlarmSCANAddValueEx      ( agl_ptrdiff_t PrjHandle, agl_int32_t AlmNr, agl_int32_t Index, LPDATA_RW40 Buff );

//
// Formatierte Ausgabe von Begleitwerten in Meldetexten mit Formatangaben
//
agl_int32_t   AGL_API AGLSym_FormatMessage               ( agl_ptrdiff_t PrjHandle, const agl_cstr8_t AlarmText, LPS7_ALARM AlarmData, agl_cstr8_t Buff, agl_int32_t BuffLen );

//
// Textbibliotheken eines S7-Programms aufzählen
//
agl_int32_t   AGL_API AGLSym_FindFirstTextlib            ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  Textlib, agl_int32_t* System );
agl_int32_t   AGL_API AGLSym_FindNextTextlib             ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  Textlib, agl_int32_t* System );
agl_int32_t   AGL_API AGLSym_FindCloseTextlib            ( agl_ptrdiff_t PrjHandle );
agl_int32_t   AGL_API AGLSym_SelectTextlib               ( agl_ptrdiff_t PrjHandle, agl_cstr8_t  Textlib );

//
// Texte einer Textbibliothek eines S7-Programms aufzählen
//
agl_int32_t   AGL_API AGLSym_FindFirstTextlibText        ( agl_ptrdiff_t PrjHandle, agl_int32_t* TextId, agl_cstr8_t Buff, agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_FindNextTextlibText         ( agl_ptrdiff_t PrjHandle, agl_int32_t* TextId, agl_cstr8_t Buff, agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_FindCloseTextlibText        ( agl_ptrdiff_t PrjHandle );
agl_int32_t   AGL_API AGLSym_GetTextlibText              ( agl_ptrdiff_t PrjHandle, agl_int32_t  TextId, agl_cstr8_t Buff, agl_int32_t BuffLen );

//
// Formatierte Darstellung von Werten
//
agl_int32_t   AGL_API AGLSym_GetTextFromValue            ( void* Value, agl_int32_t Format, agl_int32_t ValueFmt, agl_cstr8_t Text );
agl_int32_t   AGL_API AGLSym_GetValueFromText            ( agl_cstr8_t Text, void* Value, agl_int32_t* Format, agl_int32_t* ValueFmt );
agl_int32_t   AGL_API AGLSym_GetRealFromText             ( agl_cstr8_t Text, agl_float32_t* Value );
agl_int32_t   AGL_API AGLSym_GetTextFromReal             ( agl_float32_t* Value, agl_cstr8_t Text );

//
// Einstellung der Befehls- bzw. Operandensprache
//
agl_int32_t   AGL_API AGLSym_SetLanguage                 ( agl_int32_t Language );

//   EscapedStringMaxSize - Maximale Anzahl an Zeichen, die in den Ergebnispuffer kopiert warden dürfen
//   CommentMaxSize - Maximal verfügbare Puffergröße für das Kommentar

/*******************************************************************************

 Nicht verbindungsorientierte Funktionen im Zusammenhang mit .WLD Dateien

 *******************************************************************************/

/*******************************************************************************

 Funktionsname  : AGL_WLD_OpenFile

 Parameter      : LPCSTR      FileName    Zeiger auf vollständigen Dateinamen der WLD-Datei
                  agl_int32_t Access      Zugriffsoptionen (Bitmaske, WLD_ACCESS_...)
                  INT_PTR*    Handle      Handle für weitere Zugriffe

 Beschreibung   : Diese Funktion öffnet bzw. erzeugt die angegebene WLD-Datei,
                  führt ggf. Konsistenzprüfungen durch und liefert einen Handle
                  für weitere Zugriffe

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_OpenFile( const agl_cstr8_t FileName, agl_int32_t Access, agl_ptrdiff_t* Handle );

/*******************************************************************************

 Funktionsname  : AGL_WLD_OpenFileEncrypted

 Parameter      : LPCSTR      FileName    Zeiger auf vollständigen Dateinamen der WLD-Datei
                  agl_int32_t Access      Zugriffsoptionen (Bitmaske, WLD_ACCESS_...)
                  LPBYTE      Key         Schlüssel als Byte-Folge
                  UINT        Len         Länge des Schlüssels in Bytes
                  INT_PTR*    Handle      Handle für weitere Zugriffe

 Beschreibung   : Diese Funktion öffnet bzw. erzeugt die angegebene, verschlüsselte WLD-Datei,
                  führt ggf. Konsistenzprüfungen durch und liefert einen Handle
                  für weitere Zugriffe

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_OpenFileEncrypted( const agl_cstr8_t FileName, agl_int32_t Access, agl_uint8_t* Key, agl_uint32_t Len, agl_ptrdiff_t* Handle );

/*******************************************************************************

 Funktionsname  : AGL_WLD_EncryptFile

 Parameter      : LPCSTR      InFileName  Zeiger auf vollständigen Dateinamen der unverschlüsselten WLD-Datei
                  LPCSTR      OutFileName Zeiger auf vollständigen Dateinamen der verschlüsselten WLD-Datei
                  agl_int32_t Access      Zugriffsoptionen (Bitmaske, WLD_ACCESS_...)
                  LPBYTE      Key         Schlüssel als Byte-Folge
                  UINT        Len         Länge des Schlüssels in Bytes

 Beschreibung   : Diese Funktion wandelt die angegebene, unverschlüsselte WLD-Datei
                  in eine mit dem angegebenen Schlüssel verschlüsselte WLD-Datei um.
                  Die beiden Dateinamen müssen unterschiedlich sein.
                  Die unverschlüsselte Datei muss vorhanden sein.
                  Von den angegebenen Zugriffsoptionen werden nur die Optionen
                  zur Behandlung mehrfach vorhandener Bausteine ausgewertet.

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_EncryptFile( const agl_cstr8_t InFileName, const agl_cstr8_t OutFileName, agl_int32_t Access, agl_uint8_t* Key, agl_uint32_t Len );

/*******************************************************************************

 Funktionsname  : AGL_WLD_DecryptFile

 Parameter      : LPCSTR    InFileName  Zeiger auf vollständigen Dateinamen der verschlüsselten WLD-Datei
                  LPCSTR    OutFileName Zeiger auf vollständigen Dateinamen der unverschlüsselten WLD-Datei
                  LPBYTE    Key         Schlüssel als Byte-Folge
                  UINT      Len         Länge des Schlüssels in Bytes

 Beschreibung   : Diese Funktion wandelt die angegebene, verschlüsselte WLD-Datei
                  unter Verwendung des angegebenen Schlüssels in eine unverschlüsselte WLD-Datei um.
                  Die beiden Dateinamen müssen unterschiedlich sein.
                  Die verschlüsselte Datei muss vorhanden sein.

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_DecryptFile( const agl_cstr8_t InFileName, const agl_cstr8_t OutFileName, agl_uint8_t* Key, agl_uint32_t Len );

/*******************************************************************************

 Funktionsname  : AGL_WLD_CloseFile

 Parameter      : INT_PTR   Handle      Zugriffs-Handle auf Datei
                  
 Beschreibung   : Diese Funktion schreibt den Inhalt falls dieser geändert wurde
                  und schliesst die angegebene Datei ab

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_CloseFile( agl_ptrdiff_t Handle );

/*******************************************************************************

 Funktionsname  : AGL_WLD_ReadAllBlockCount

 Parameter      : INT_PTR   Handle      Zugriffs-Handle auf Datei
                  LPALL_BLOCK_COUNT pBC Zeiger auf Struktur für Bausteine-Anzahl

 Beschreibung   : Diese Funktion liest die Anzahl aller vorhandenen Bausteine
                  innerhalb der angegebenen Datei

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_ReadAllBlockCount( agl_ptrdiff_t Handle, LPALL_BLOCK_COUNT pBC );

/*******************************************************************************

 Funktionsname  : AGL_WLD_ReadBlockCount

 Parameter      : INT_PTR     Handle      Zugriffs-Handle auf Datei
                  agl_int32_t BlockType   Typ des Bausteines
                  agl_int32_t* BlockCount Zeiger auf Variable für Anzahl Bausteine

 Beschreibung   : Diese Funktion liest die Anzahl der vorhandenen Bausteine
                  innerhalb der angegebenen Datei für den angegebenen Bausteintyp

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_ReadBlockCount( agl_ptrdiff_t Handle, agl_int32_t BlockType, agl_int32_t* BlockCount );

/*******************************************************************************

 Funktionsname  : AGL_WLD_ReadBlockList

 Parameter      : INT_PTR   Handle      Zugriffs-Handle auf Datei
                  agl_int32_t BlockType         Typ des Bausteines
                  agl_int32_t* BlockCount       Eingabe: Buchhalter-Länge in Einheiten (Block-Nummer und Block-Flags)
                                        Rückgabe: Benötigte Buchhalter-Länge in Einheiten (Block-Nummer und Block-Flags)
                  PWORD BlockList       Buchhalter (Zeiger auf Puffer für Block-Nummern und Block-Flags)

 Beschreibung   : Diese Funktion liest die Anzahl der vorhandenen Bausteine
                  und den Buchhalter im S7-Format (2-Wort)
                  innerhalb der angegebenen Datei für den angegebenen Bausteintyp

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_ReadBlockList( agl_ptrdiff_t Handle, agl_int32_t BlockType, agl_int32_t* BlockCount, agl_uint16_t* BlockList );

/*******************************************************************************

 Funktionsname  : AGL_WLD_ReadBlockLen

 Parameter      : INT_PTR   Handle      Zugriffs-Handle auf Datei
                  agl_int32_t BlockType         Typ des Bausteines
                  agl_int32_t BlockNr           Nummer des zu prüfenden Bausteines
                  agl_int32_t* BlockLen         Rückgabe: Daten- bzw. Programmcodelänge des Bausteins in Bytes

 Beschreibung   : Diese Funktion liest die Daten- bzw. Programmcodelänge 
                  des angegebenen Bausteins innerhalb der angegebenen Datei

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_ReadBlockLen( agl_ptrdiff_t Handle, agl_int32_t BlockType, agl_int32_t BlockNr, agl_int32_t* BlockLen );

/*******************************************************************************

 Funktionsname  : AGL_WLD_DeleteBlocks

 Parameter      : INT_PTR   Handle      Zugriffs-Handle auf Datei
                  LPCSTR  Blocks        Liste der Bausteine
                                        "All"  für "Alle Bausteine" bzw.
                                        "AllU" für "Alle Benutzer-Bausteine" bzw.
                                        "OB", "FC", "FB", "DB", "SFC", "SFB", "SDB" für "Alle Bausteine einer Art" bzw.
                                        Bausteinbereich (z.B. "FC 1 - 10") oder
                                        Einzelner Baustein (z.B. "OB 1")

 Beschreibung   : Diese Funktion löscht die angegebenen Bausteine
                  in der angegebenen Datei
                  
 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_DeleteBlocks( agl_ptrdiff_t Handle, const agl_cstr8_t Blocks );

/*******************************************************************************

 Funktionsname  : AGL_WLD_GetReport

 Parameter      : INT_PTR   Handle  Zugriffs-Handle auf Datei
                  agl_int32_t*      Length  Eingabe: Puffer-Länge in Zeichen
                                    Rückgabe: Benötigte Puffer-Länge in Zeichen (incl. Abschluss)
                  LPSTR     Buffer  Speicher für Report in Textform

 Beschreibung   : Diese Funktion liefert den Report für die zuletzt
                  unter Verwendung der angegebenen Datei durchgeführte Aktion
                  in Textform.
                  
                  Es handelt sich um eine mehrzeilige Darstellung,
                  d.h. der gelieferte Report enthält Zeilentrennzeichen.

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_GetReport( agl_ptrdiff_t Handle, agl_int32_t* Length, agl_cstr8_t Buffer );


/*******************************************************************************

 Verbindungsorientierte Funktionen zum Sichern, Wiederherstellen
 und Löschen von Bausteinen 

 *******************************************************************************/

/*******************************************************************************

 Funktionsname  : AGL_PLC_Backup

 Parameter      : agl_int32_t ConnNr        Verbindungshandle
                  INT_PTR     Handle        Zugriffs-Handle auf Datei
                  LPCSTR      Blocks        Liste der Bausteine
                                        "All"  für "Alle Bausteine" (Vorgabe) bzw.
                                        "AllU" für "Alle Benutzer-Bausteine" bzw.
                                        "OB", "FC", "FB", "DB", "SFC", "SFB", "SDB" für "Alle Bausteine einer Art" bzw.
                                        Bausteinbereich (z.B. "FC 1 - 10") oder
                                        Einzelner Baustein (z.B. "OB 1")
                  agl_int32_t Timeout       Timeoutwert für diese Funktion
                  agl_ptrdiff_t UserVal      Benutzerdefinierter Wert

 Beschreibung   : Diese Funktion sichert die Bausteine in der SPS
                  in die angegebene Datei

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_PLC_Backup( agl_int32_t ConnNr, agl_ptrdiff_t Handle, const agl_cstr8_t Blocks, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

/*******************************************************************************

 Funktionsname  : AGL_PLC_Restore

 Parameter      : agl_int32_t ConnNr        Verbindungshandle
                  INT_PTR     Handle        Zugriffs-Handle auf Datei
                  LPCSTR      Blocks        Liste der Bausteine
                                        "All"  für "Alle Bausteine" bzw.
                                        "AllU" für "Alle Benutzer-Bausteine" (Vorgabe) bzw.
                                        "OB", "FC", "FB", "DB", "SFC", "SFB", "SDB" für "Alle Bausteine einer Art" bzw.
                                        Bausteinbereich (z.B. "FC 1 - 10") oder
                                        Einzelner Baustein (z.B. "OB 1")
                  agl_int32_t Timeout       Timeoutwert für diese Funktion
                  LONG_PTR    UserVal      Benutzerdefinierter Wert

 Beschreibung   : Diese Funktion überträgt die Bausteine in der angegebenen Datei
                  in die SPS

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_PLC_Restore( agl_int32_t ConnNr, agl_ptrdiff_t Handle, const agl_cstr8_t Blocks, agl_int32_t Timeout, agl_ptrdiff_t UserVal );

/*******************************************************************************

 Funktionsname  : AGL_PLC_DeleteBlocks

 Parameter      : agl_int32_t ConnNr        Verbindungshandle
                  LPCSTR  Blocks        Liste der Bausteine
                                        "All"  für "Alle Bausteine" bzw.
                                        "AllU" für "Alle Benutzer-Bausteine" bzw.
                                        "OB", "FC", "FB", "DB", "SFC", "SFB", "SDB" für "Alle Bausteine einer Art" bzw.
                                        Bausteinbereich (z.B. "FC 1 - 10") oder
                                        Einzelner Baustein (z.B. "OB 1")
                  agl_int32_t Timeout       Timeoutwert für diese Funktion
                  LONG_PTR UserVal      Benutzerdefinierter Wert

 Beschreibung   : Diese Funktion löscht die angegebenen Bausteine in der SPS

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_PLC_DeleteBlocks( agl_int32_t ConnNr, const agl_cstr8_t Blocks, agl_int32_t Timeout, agl_ptrdiff_t UserVal );


/*******************************************************************************

 Funktionsname  : AGL_Compress

 Parameter      : agl_int32_t ConnNr     Verbindungshandle
                  agl_int32_t Timeout    Timeoutwert für diese Funktion
                  LONG_PTR    UserVal    Benutzerdefinierter Wert

 Beschreibung   : Diese Funktion komprimiert den Speicher der SPS

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_Compress( agl_int32_t ConnNr, agl_int32_t Timeout, agl_ptrdiff_t UserVal );


#if defined( __cplusplus )
  }
#endif

#endif  // #if !defined( __AGL_FUNCS__ )
