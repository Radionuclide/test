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
void          AGL_API AGL_Activate                      ( const agl_cstr8_t const Key );

//
// Versionsinformationen ermitteln
//
void          AGL_API AGL_GetVersion                    ( agl_int32_t* const Major, agl_int32_t* const Minor );
void          AGL_API AGL_GetVersionEx                  ( agl_int32_t* const Major, agl_int32_t* const Minor, agl_int32_t* const Build, agl_int32_t* const Revision, agl_cstr8_t const Date );
agl_int32_t   AGL_API AGL_GetOptions                    ( void );
agl_int32_t   AGL_API AGL_GetSerialNumber               ( void );
agl_cstr8_t   AGL_API AGL_GetClientName                 ( agl_cstr8_t const Name );

//
// Konfigurationsinformationen der Bibliothek ermitteln/setzen
//
agl_int32_t   AGL_API AGL_GetMaxDevices                  ( void );
agl_int32_t   AGL_API AGL_GetMaxQueues                   ( void );
agl_int32_t   AGL_API AGL_GetMaxPLCPerDevice             ( void );
void          AGL_API AGL_UseSystemTime                  ( const agl_bool_t Flag );
void          AGL_API AGL_ReturnJobNr                    ( const agl_bool_t Flag );
void          AGL_API AGL_SetBSendAutoResponse           ( const agl_bool_t Flag );
agl_bool_t    AGL_API AGL_GetBSendAutoResponse           ( void );

//
// Umgebungsinformationen ermitteln/setzen (keine Kommunikationsverbindung)
//
agl_int32_t   AGL_API AGL_GetPCCPConnNames               ( agl_cstr8_t const Names, const agl_int32_t Len );
agl_int32_t   AGL_API AGL_GetPCCPProtocol                ( agl_cstr8_t const Name );
agl_int32_t   AGL_API AGL_GetTapiModemNames              ( agl_cstr8_t const Names, const agl_int32_t Len );
agl_int32_t   AGL_API AGL_GetLocalIPAddresses            ( agl_ulong32_t* const Addresses, const agl_ulong32_t NumAddresses );
agl_int32_t   AGL_API AGL_GetAdaptersInfo                ( LPADAPTER_INFO p_pAdapterInfo, const agl_uint32_t MaxAdapters );

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
agl_int32_t   AGL_API AGL_Config                         ( const agl_int32_t DevNr );
agl_int32_t   AGL_API AGL_ConfigEx                       ( const agl_int32_t DevNr, const agl_cstr8_t const CmdLine );

//
// Parameter einstellen/lesen
//
agl_int32_t   AGL_API AGL_SetParas                       ( const agl_int32_t DevNr, const agl_int32_t ParaType, const void* const Para, const agl_int32_t Len );
agl_int32_t   AGL_API AGL_GetParas                       ( const agl_int32_t DevNr, const agl_int32_t ParaType, void* const Para, const agl_int32_t Len );
agl_int32_t   AGL_API AGL_SetDevType                     ( const agl_int32_t DevNr, const agl_int32_t DevType );
agl_int32_t   AGL_API AGL_GetDevType                     ( const agl_int32_t DevNr );

agl_int32_t   AGL_API AGL_ReadParas                      ( const agl_int32_t DevNr, const agl_int32_t ParaType );
agl_int32_t   AGL_API AGL_WriteParas                     ( const agl_int32_t DevNr, const agl_int32_t ParaType );
agl_int32_t   AGL_API AGL_ReadDevice                     ( const agl_int32_t DevNr );
agl_int32_t   AGL_API AGL_WriteDevice                    ( const agl_int32_t DevNr );

agl_int32_t   AGL_API AGL_ReadParasFromFile              ( const agl_int32_t DevNr, const agl_cstr8_t const FileName );
agl_int32_t   AGL_API AGL_WriteParasToFile               ( const agl_int32_t DevNr, const agl_cstr8_t const FileName );

agl_int32_t   AGL_API AGL_GetParaPath                    ( agl_cstr8_t const DirName, const agl_int32_t MaxLen );
agl_int32_t   AGL_API AGL_SetParaPath                    ( const agl_cstr8_t const DirName );
agl_int32_t   AGL_API AGL_SetAndGetParaPath              ( const agl_cstr8_t const CompanyName, const agl_cstr8_t const ProductName, agl_cstr8_t const AktPath, const agl_int32_t MaxLen );

//
// Geräteinformationen ermitteln
//
agl_int32_t   AGL_API AGL_GetPLCType                     ( const agl_int32_t DevNr, const agl_int32_t PlcNr );
agl_int32_t   AGL_API AGL_HasFunc                        ( const agl_int32_t DevNr, const agl_int32_t PlcNr, const agl_int32_t Func );

//
// Fehlertexte
//
agl_int32_t   AGL_API AGL_LoadErrorFile                  ( const agl_cstr8_t const FileName );
agl_int32_t   AGL_API AGL_GetErrorMsg                    ( const agl_int32_t ErrNr, agl_cstr8_t const Msg, const agl_int32_t MaxLen );
agl_int32_t   AGL_API AGL_GetErrorCodeName               ( const agl_int32_t ErrNr, const agl_cstr8_t* const ErrorCodeName );

//
// Verbindungsaufbau initialisieren
//
agl_int32_t   AGL_API AGL_OpenDevice                     ( const agl_int32_t DevNr );
agl_int32_t   AGL_API AGL_CloseDevice                    ( const agl_int32_t DevNr );

//
// Ab hier können Funktionen, auch asynchron bearbeitet werden
//

//
// Asynchrone Aufrufe verwalten
//
agl_int32_t   AGL_API AGL_SetDevNotification             ( const agl_int32_t DevNr, const LPNOTIFICATION pN );
agl_int32_t   AGL_API AGL_SetConnNotification            ( const agl_int32_t ConnNr, const LPNOTIFICATION pN );
agl_int32_t   AGL_API AGL_SetJobNotification             ( const agl_int32_t DevNr, const agl_int32_t JobNr, const LPNOTIFICATION pN );
agl_int32_t   AGL_API AGL_SetJobNotificationEx           ( const agl_int32_t DevNr, const agl_int32_t JobNr, const LPNOTIFICATION pN );
agl_int32_t   AGL_API AGL_GetJobResult                   ( const agl_int32_t DevNr, const agl_int32_t JobNr, LPRESULT40 pR );
agl_int32_t   AGL_API AGL_GetLastJobResult               ( const agl_int32_t ConnNr, LPRESULT40 pR );
agl_int32_t   AGL_API AGL_DeleteJob                      ( const agl_int32_t DevNr, const agl_int32_t JobNr );
agl_int32_t   AGL_API AGL_WaitForJob                     ( const agl_int32_t DevNr, const agl_int32_t JobNr );
agl_int32_t   AGL_API AGL_WaitForJobEx                   ( const agl_int32_t DevNr, const agl_int32_t JobNr, LPRESULT40 pR );

//
// Verbindung zum Adapter auf- und abbauen
// Achtung: Sämtliche übergebenen Puffer müssen über die Laufzeit der Funktionen gültig bleiben!
//
agl_int32_t   AGL_API AGL_DialUp                         ( const agl_int32_t DevNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_HangUp                         ( const agl_int32_t DevNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_InitAdapter                    ( const agl_int32_t DevNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitAdapter                    ( const agl_int32_t DevNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

//
// Ab hier die Funktionen nur aufrufen, wenn Adapter auch initialisiert wurde
//

//
// Durch den Adapter ermittelbare Informationen
//
agl_int32_t   AGL_API AGL_GetLifelist                    ( const agl_int32_t DevNr, agl_uint8_t* const List, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetDirectPLC                   ( const agl_int32_t DevNr, agl_uint8_t* const pPlc, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

//
// Verbindung bis zur Steuerung auf- und abbauen
//
agl_int32_t   AGL_API AGL_PLCConnect                     ( const agl_int32_t DevNr, const agl_int32_t PlcNr, agl_int32_t* const ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCConnectEx                   ( const agl_int32_t DevNr, const agl_int32_t PlcNr, const agl_int32_t RackNr, const agl_int32_t SlotNr, agl_int32_t* const ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCDisconnect                  ( const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

//
// Ab hier die Funktionen nur aufrufen, wenn auch eine Verbindung besteht
//

//
// Verbindungsabhängige Informationen ermittlen
//
agl_int32_t   AGL_API AGL_ReadMaxPacketSize              ( const agl_int32_t ConnNr );
agl_int32_t   AGL_API AGL_GetRedConnState                ( const agl_int32_t ConnNr, LPRED_CONN_STATE pState );
agl_int32_t   AGL_API AGL_GetRedConnStateMsg             ( const agl_int32_t ConnNr, LPRED_CONN_STATE pState, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

//
// Kommunikationsfunktionen
//

//
// Daten lesen/schreiben
//
agl_int32_t   AGL_API AGL_ReadOpState                    ( const agl_int32_t ConnNr, agl_int32_t* const State, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadOpStateEx                  ( const agl_int32_t ConnNr, agl_int32_t* const State, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetPLCStartOptions             ( const agl_int32_t ConnNr, agl_int32_t* const StartOptions, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCStop                        ( const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCStart                       ( const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCResume                      ( const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_PLCColdStart                   ( const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_IsHPLC                         ( const agl_int32_t ConnNr, agl_int32_t* const IsHPLC, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_HPLCStop                       ( const agl_int32_t ConnNr, const agl_int32_t CPUNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_HPLCStart                      ( const agl_int32_t ConnNr, const agl_int32_t CPUNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_HPLCColdStart                  ( const agl_int32_t ConnNr, const agl_int32_t CPUNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_GetPLCClock                    ( const agl_int32_t ConnNr, LPTOD pTOD, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_SetPLCClock                    ( const agl_int32_t ConnNr, const LPTOD pTOD, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_SyncPLCClock                   ( const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_ReadMLFBNr                     ( const agl_int32_t ConnNr, LPMLFB pMLFBNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadMLFBNrEx                   ( const agl_int32_t ConnNr, LPMLFBEX pMLFBNrEx, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadPLCInfo                    ( const agl_int32_t ConnNr, LPPLCINFO pPLCInfo, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadCycleTime                  ( const agl_int32_t ConnNr, LPCYCLETIME pCycleTime, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadProtLevel                  ( const agl_int32_t ConnNr, LPPROTLEVEL pProtLevel, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadS7Ident                    ( const agl_int32_t ConnNr, LPS7_IDENT pIdent, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadS7LED                      ( const agl_int32_t ConnNr, LPS7_LED pLed, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetExtModuleInfo               ( const agl_int32_t ConnNr, LPEXT_MODULE_INFO pInfo, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadSzl                        ( const agl_int32_t ConnNr, const agl_int32_t SzlId, const agl_int32_t Index, agl_uint8_t* const Buff, agl_int32_t* const BuffLen, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_IsPasswordReq                  ( const agl_int32_t ConnNr, agl_int32_t* const IsPWReq, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_SetPassword                    ( const agl_int32_t ConnNr, const agl_cstr8_t const PW, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_SetPasswordEx                  ( const agl_int32_t ConnNr, const agl_cstr8_t const PW, agl_uint32_t* const NewProtectionLevel, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_UnSetPassword                  ( const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_ReadDiagBufferEntrys           ( const agl_int32_t ConnNr, agl_int32_t* const Entrys, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadDiagBuffer                 ( const agl_int32_t ConnNr, agl_int32_t* const Entrys, agl_uint8_t* const pDiagBuff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetDiagBufferEntry             ( const agl_int32_t Index, agl_uint8_t* const pDiagBuff, agl_cstr8_t const Text, const agl_int32_t TextLen );

agl_int32_t   AGL_API AGL_ReadDBCount                    ( const agl_int32_t ConnNr, agl_int32_t* const DBCount, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadDBList                     ( const agl_int32_t ConnNr, agl_int32_t* const DBCount, agl_uint16_t* const DBList, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadDBLen                      ( const agl_int32_t ConnNr, const agl_int32_t DBNr, agl_int32_t* const DBLen, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_ReadAllBlockCount              ( const agl_int32_t ConnNr, LPALL_BLOCK_COUNT pBC, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadBlockCount                 ( const agl_int32_t ConnNr, const agl_int32_t BlockType, agl_int32_t* const BlockCount, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadBlockList                  ( const agl_int32_t ConnNr, const agl_int32_t BlockType, agl_int32_t* const BlockCount, agl_uint16_t* const BlockList, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadBlockLen                   ( const agl_int32_t ConnNr, const agl_int32_t BlockType, const agl_int32_t BlockNr, agl_int32_t* const BlockLen, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_ReadInBytes                    ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadPInBytes                   ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadOutBytes                   ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadFlagBytes                  ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadSFlagBytes                 ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadVarBytes                   ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadDataBytes                  ( const agl_int32_t ConnNr, const agl_int32_t DBNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadDataWords                  ( const agl_int32_t ConnNr, const agl_int32_t DBNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint16_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadTimerWords                 ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint16_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadCounterWords               ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint16_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadMix                        ( const agl_int32_t ConnNr, LPDATA_RW40 Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ReadMixEx                      ( const agl_int32_t ConnNr, LPDATA_RW40 Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_WriteInBytes                   ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteOutBytes                  ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WritePOutBytes                 ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteFlagBytes                 ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteSFlagBytes                ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteVarBytes                  ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteDataBytes                 ( const agl_int32_t ConnNr, const agl_int32_t DBNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint8_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteDataWords                 ( const agl_int32_t ConnNr, const agl_int32_t DBNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint16_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteTimerWords                ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint16_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteCounterWords              ( const agl_int32_t ConnNr, const agl_int32_t Start, const agl_int32_t Num, agl_uint16_t* const Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteMix                       ( const agl_int32_t ConnNr, LPDATA_RW40 Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_WriteMixEx                     ( const agl_int32_t ConnNr, LPDATA_RW40 Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

//
// Daten optimiert lesen/schreiben
//
agl_int32_t   AGL_API AGL_InitOptReadMix                 ( const agl_int32_t ConnNr, LPDATA_RW40 Buff, const agl_int32_t Num, agl_ptrdiff_t* const Opt );
agl_int32_t   AGL_API AGL_ReadOptReadMix                 ( const agl_int32_t ConnNr, const agl_ptrdiff_t Opt, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_EndOptReadMix                  ( const agl_ptrdiff_t Opt );

agl_int32_t   AGL_API AGL_InitOptReadMixEx               ( const agl_int32_t ConnNr, LPDATA_RW40 Buff, const agl_int32_t Num, agl_ptrdiff_t* const Opt );
agl_int32_t   AGL_API AGL_ReadOptReadMixEx               ( const agl_int32_t ConnNr, const agl_ptrdiff_t Opt, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_EndOptReadMixEx                ( const agl_ptrdiff_t Opt );

agl_int32_t   AGL_API AGL_InitOptWriteMix                ( const agl_int32_t ConnNr, LPDATA_RW40 Buff, const agl_int32_t Num, agl_ptrdiff_t* const Opt );
agl_int32_t   AGL_API AGL_WriteOptWriteMix               ( const agl_int32_t ConnNr, const agl_ptrdiff_t Opt, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_EndOptWriteMix                 ( const agl_ptrdiff_t Opt );

agl_int32_t   AGL_API AGL_InitOptWriteMixEx              ( const agl_int32_t ConnNr, LPDATA_RW40 Buff, const agl_int32_t Num, agl_ptrdiff_t* const Opt );
agl_int32_t   AGL_API AGL_WriteOptWriteMixEx             ( const agl_int32_t ConnNr, const agl_ptrdiff_t Opt, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_EndOptWriteMixEx               ( const agl_ptrdiff_t Opt );

agl_int32_t   AGL_API AGL_SetOptNotification             ( const agl_ptrdiff_t Opt, LPNOTIFICATION pN );
agl_int32_t   AGL_API AGL_DeleteOptJob                   ( const agl_int32_t ConnNr, const agl_ptrdiff_t Opt );
agl_int32_t   AGL_API AGL_GetOptJobResult                ( const agl_int32_t ConnNr, const agl_ptrdiff_t Opt, LPRESULT40 pR );
agl_int32_t   AGL_API AGL_WaitForOptJob                  ( const agl_int32_t ConnNr, const agl_ptrdiff_t Opt );

agl_ptrdiff_t AGL_API AGL_AllocRWBuffs                   ( LPDATA_RW40 Buff, const agl_int32_t Num );
agl_int32_t   AGL_API AGL_FreeRWBuffs                    ( const agl_ptrdiff_t Handle );
agl_int32_t   AGL_API AGL_ReadRWBuff                     ( LPDATA_RW40 Buff, const agl_int32_t Index, void* const pData );
agl_int32_t   AGL_API AGL_WriteRWBuff                    ( LPDATA_RW40 Buff, const agl_int32_t Index, void* const pData );

//
// Funktionen für die RK512/3964/3964R-Kommunikation
//
agl_int32_t   AGL_API AGL_RKSend                         ( const agl_int32_t ConnNr, LPDATA_RW40_RK Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RKSendEx                       ( const agl_int32_t ConnNr, LPDATA_RW40_RK Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RKFetch                        ( const agl_int32_t ConnNr, LPDATA_RW40_RK Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RKFetchEx                      ( const agl_int32_t ConnNr, LPDATA_RW40_RK Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Send_RKFetch                   ( const agl_int32_t ConnNr, LPDATA_RW40_RK Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Recv_RKSend                    ( const agl_int32_t ConnNr, LPDATA_RW40_RK Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Recv_RKFetch                   ( const agl_int32_t ConnNr, LPDATA_RW40_RK Buff, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Send_3964                      ( const agl_int32_t ConnNr, const agl_uint8_t* const Buff, const agl_int32_t BuffLen, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Recv_3964                      ( const agl_int32_t ConnNr, agl_uint8_t* const Buff, agl_int32_t* const BuffLen, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

//
// Kommunikationsfunktionen nur für projektierte Verbindungen
//
agl_int32_t   AGL_API AGL_BSend                          ( const agl_int32_t ConnNr, const agl_wsabuf_t* const pwsa, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_BReceive                       ( const agl_int32_t ConnNr, agl_wsabuf_t* const pwsa, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_BSendEx                        ( const agl_int32_t ConnNr, const agl_wsabuf_t* const pwsa, const agl_int32_t R_ID, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_BReceiveEx                     ( const agl_int32_t ConnNr, agl_wsabuf_t* const pwsa, agl_int32_t* const R_ID, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_USend                          ( const agl_int32_t ConnNr, LPS7_USEND_URCV pUSR, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_UReceive                       ( const agl_int32_t ConnNr, LPS7_USEND_URCV pUSR, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

//
// Funktionen für Alarme, Scans, Archive, Zyklisches Lesen
//
agl_int32_t   AGL_API AGL_InitOpStateMsg                 ( const agl_int32_t ConnNr, agl_int32_t* const OpState, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitOpStateMsg                 ( const agl_int32_t ConnNr, agl_int32_t* const OpState, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetOpStateMsg                  ( const agl_int32_t ConnNr, agl_int32_t* const OpState, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_InitDiagMsg                    ( const agl_int32_t ConnNr, agl_int32_t* const OpState, const agl_int32_t DiagMask, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitDiagMsg                    ( const agl_int32_t ConnNr, agl_int32_t* const OpState, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetDiagMsg                     ( const agl_int32_t ConnNr, LPS7_DIAG_MSG pDiag, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_InitCyclicRead                 ( const agl_int32_t ConnNr, const agl_int32_t CycleTime, const agl_int32_t Flags, LPDATA_RW40 Buff, const agl_int32_t Num, agl_int32_t* const Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_InitCyclicReadEx               ( const agl_int32_t ConnNr, const agl_int32_t CycleTime, const agl_int32_t Flags, LPDATA_RW40 Buff, const agl_int32_t Num, agl_int32_t* const Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_StartCyclicRead                ( const agl_int32_t ConnNr, const agl_int32_t Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_StopCyclicRead                 ( const agl_int32_t ConnNr, const agl_int32_t Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitCyclicRead                 ( const agl_int32_t ConnNr, const agl_int32_t Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetCyclicRead                  ( const agl_int32_t ConnNr, LPDATA_RW40 Buff, const agl_int32_t Num, const agl_int32_t Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetCyclicReadEx                ( const agl_int32_t ConnNr, LPDATA_RW40 Buff, const agl_int32_t Num, const agl_int32_t Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_InitScanMsg                    ( const agl_int32_t ConnNr, agl_int32_t* const OpState, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitScanMsg                    ( const agl_int32_t ConnNr, agl_int32_t* const OpState, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetScanMsg                     ( const agl_int32_t ConnNr, LPS7_SCAN pScan, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_HasAckTriggeredMsg             ( const agl_int32_t ConnNr, agl_int32_t* const Mode, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_InitAlarmMsg                   ( const agl_int32_t ConnNr, agl_int32_t* const OpState, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitAlarmMsg                   ( const agl_int32_t ConnNr, agl_int32_t* const OpState, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetAlarmMsg                    ( const agl_int32_t ConnNr, LPS7_ALARM pAlarm, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_ReadOpenMsg                    ( const agl_int32_t ConnNr, LPS7_OPEN_MSG_STATE pState, agl_int32_t* const MsgAnz, const agl_int32_t MsgType, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_GetMsgStateChange              ( const agl_int32_t ConnNr, LPS7_RCV_MSG_STATE pState, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_AckMsg                         ( const agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, const agl_int32_t MsgAnz, const agl_int32_t MsgType, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_LockMsg                        ( const agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, const agl_int32_t MsgAnz, const agl_int32_t MsgType, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_UnlockMsg                      ( const agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, const agl_int32_t MsgAnz, const agl_int32_t MsgType, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_InitARSend                     ( const agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, const agl_int32_t ArAnz, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_ExitARSend                     ( const agl_int32_t ConnNr, LPS7_CHANGE_MSG_STATE pMsg, const agl_int32_t ArAnz, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_GetARSend                      ( const agl_int32_t ConnNr, agl_wsabuf_t* const pwsa, agl_int32_t* const AR_ID, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );


/*******************************************************************************

 RFC1006-Funktionen

*******************************************************************************/

agl_int32_t   AGL_API AGL_RFC1006_Connect                ( const agl_int32_t DevNr, const agl_int32_t PlcNr, agl_int32_t* const ConnNr, LPRFC_1006_SERVER ConnInfo, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RFC1006_Disconnect             ( const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RFC1006_Receive                ( const agl_int32_t ConnNr, agl_uint8_t* const Buff, const agl_int32_t BuffLen, agl_int32_t* const ReceivedLen, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_RFC1006_Send                   ( const agl_int32_t ConnNr, const agl_uint8_t* const Buff, const agl_int32_t BuffLen, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );


/*******************************************************************************

 NCK-Funktionen

*******************************************************************************/

//
// Lesen und Schreiben von Variablen
//
agl_int32_t   AGL_API AGL_NCK_ReadMixEx                  ( const agl_int32_t ConnNr, LPNCKDataRW Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_WriteMixEx                 ( const agl_int32_t ConnNr, LPNCKDataRW Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_CheckVarSize               ( const agl_int32_t ConnNr, LPNCKDataRW Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

//
// Zyklisches Lesen von Variablen
//
agl_int32_t   AGL_API AGL_NCK_InitCyclicReadEx           ( const agl_int32_t ConnNr, const agl_int32_t CycleTime, const agl_int32_t OnlyChanged, LPNCKDataRW Buff, const agl_int32_t Num, agl_int32_t* const Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_StartCyclicRead            ( const agl_int32_t ConnNr, const agl_int32_t Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_StopCyclicRead             ( const agl_int32_t ConnNr, const agl_int32_t Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_ExitCyclicRead             ( const agl_int32_t ConnNr, const agl_int32_t Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_GetCyclicReadEx            ( const agl_int32_t ConnNr, LPNCKDataRW Buff, const agl_int32_t Num, const agl_int32_t Handle, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

//
// Ausführen von PI-Diensten
//
agl_int32_t   AGL_API AGL_NCK_PI_EXTERN                  ( const agl_int32_t ConnNr, const agl_int32_t Channel, const agl_cstr8_t const ProgName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_EXTMOD                  ( const agl_int32_t ConnNr, const agl_int32_t Channel, const agl_cstr8_t const ProgName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_SELECT                  ( const agl_int32_t ConnNr, const agl_int32_t Channel, const agl_cstr8_t const ProgName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_DELE                  ( const agl_int32_t ConnNr, const agl_cstr8_t const FileName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_PROT                  ( const agl_int32_t ConnNr, const agl_cstr8_t const FileName, const agl_cstr8_t const Protection, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_RENA                  ( const agl_int32_t ConnNr, const agl_cstr8_t const OldFileName, const agl_cstr8_t const NewFileName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_XFER                  ( const agl_int32_t ConnNr, const agl_cstr8_t const FileName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_LOGIN                   ( const agl_int32_t ConnNr, const agl_cstr8_t const Password, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_LOGOUT                  ( const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_NCK_PI_F_OPEN                  ( const agl_int32_t ConnNr, const agl_cstr8_t const FileName, const agl_cstr8_t const WindowName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_OPER                  ( const agl_int32_t ConnNr, const agl_cstr8_t const FileName, const agl_cstr8_t const WindowName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_SEEK                  ( const agl_int32_t ConnNr, const agl_cstr8_t const WindowName, const agl_int32_t SeekMode, const agl_int32_t SeekPointer, const agl_int32_t WindowSize, const agl_cstr8_t const CompareString, const agl_int32_t SkipCount, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_CLOS                  ( const agl_int32_t ConnNr, const agl_cstr8_t const WindowName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_NCK_PI_StartAll                ( const agl_int32_t ConnNr, const agl_uint8_t* const Para, const agl_int32_t ParaLen, const agl_cstr8_t Cmd, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

//
// Ab hier neu für 5.1
//
agl_int32_t   AGL_API AGL_NCK_PI_F_COPY                  ( const agl_int32_t ConnNr, const agl_int32_t Direction, const agl_cstr8_t const SourceFileName, const agl_cstr8_t const DestinationFileName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_F_PROR                  ( const agl_int32_t ConnNr, const agl_cstr8_t const FileName, const agl_cstr8_t const Protection, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_CANCEL                  ( const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_CRCEDN                  ( const agl_int32_t ConnNr, const agl_int32_t ToolArea, const agl_int32_t TNumber, const agl_int32_t DNumber, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_DELECE                  ( const agl_int32_t ConnNr, const agl_int32_t ToolArea, const agl_int32_t TNumber, const agl_int32_t DNumber, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_DELETO                  ( const agl_int32_t ConnNr, const agl_int32_t ToolArea, const agl_int32_t TNumber, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_IBN_SS                  ( const agl_int32_t ConnNr, const agl_int32_t Switch, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_MMCSEM                  ( const agl_int32_t ConnNr, const agl_int32_t ChannelNumber, const agl_int32_t FunctionNumber, const agl_int32_t SemaValue, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_TMCRTO                  ( const agl_int32_t ConnNr, const agl_int32_t ToolArea, const agl_cstr8_t const ToolID, const agl_int32_t ToolNumber, const agl_int32_t DuploNumber, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_TMMVTL                  ( const agl_int32_t ConnNr, const agl_int32_t ToolArea, const agl_int32_t ToolNumber, const agl_int32_t SourcePlaceNumber, const agl_int32_t SourceMagazineNumber, const agl_int32_t DestinationPlaceNumber, const agl_int32_t DestinationMagazineNumber, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_TMCRTC                  ( const agl_int32_t ConnNr, const agl_int32_t ToolArea, const agl_cstr8_t const ToolID, const agl_int32_t ToolNumber, const agl_int32_t DuploNumber, const agl_int32_t EdgeNumber, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_PI_CREATO                  ( const agl_int32_t ConnNr, const agl_int32_t ToolArea, const agl_int32_t TNumber, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

//
// Dateitransferfunktionen
//
agl_int32_t   AGL_API AGL_NCK_CopyFileToNC               ( const agl_int32_t ConnNr, const agl_cstr8_t const NCFileName, const agl_cstr8_t const PCFileName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_CopyFileFromNC             ( const agl_int32_t ConnNr, const agl_cstr8_t const NCFileName, const agl_cstr8_t const PCFileName, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

agl_int32_t   AGL_API AGL_NCK_CopyToNC                   ( const agl_int32_t ConnNr, const agl_cstr8_t const FileName, const void* const Buff, const agl_int32_t BuffLen, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_CopyFromNC                 ( const agl_int32_t ConnNr, const agl_cstr8_t const FileName, void* const Buff, const agl_int32_t BuffLen, agl_int32_t* const NeededLen, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_CopyFromNCAlloc            ( const agl_int32_t ConnNr, const agl_cstr8_t const FileName, void** const Buff, agl_int32_t* const BuffLen, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_NCK_FreeBuff                   ( void* const Buff );

agl_int32_t   AGL_API AGL_NCK_SetConnProgressNotification( const agl_int32_t ConnNr, LPNOTIFICATION pN );

//
// Einlesen von Variableninformationen aus verschiedenen Dateien (Typen und Formaten)
//
agl_int32_t   AGL_API AGL_NCK_CheckNSKVarLine            ( const agl_cstr8_t const Line, LPNCKDataRW pRW, agl_cstr8_t const Name, const agl_int32_t NameLen );
agl_int32_t   AGL_API AGL_NCK_ReadNSKVarFile             ( const agl_cstr8_t const FileName, LPNCKDataRW* const ppRW, agl_cstr8_t** const pName );
agl_int32_t   AGL_API AGL_NCK_CheckCSVVarLine            ( const agl_cstr8_t const Line, LPNCKDataRW pRW, agl_cstr8_t const Name, const agl_int32_t NameLen );
agl_int32_t   AGL_API AGL_NCK_ReadCSVVarFile             ( const agl_cstr8_t const FileName, LPNCKDataRW* const ppRW, agl_cstr8_t** const pName );
agl_int32_t   AGL_API AGL_NCK_ReadGUDVarFile             ( const agl_cstr8_t const FileName, LPNCKDataRW* const ppRW, agl_cstr8_t** const pName );
agl_int32_t   AGL_API AGL_NCK_ReadGUDVarFileEx           ( const agl_cstr8_t const FileName, const agl_int32_t GUDNr, const agl_int32_t Area, LPNCKDataRW* const ppRW, agl_cstr8_t** const pName );
agl_int32_t   AGL_API AGL_NCK_FreeVarBuff                ( LPNCKDataRW* const ppRW, agl_cstr8_t** const pName, const agl_int32_t Anz );
agl_int32_t   AGL_API AGL_NCK_GetSingleVarDef            ( const LPNCKDataRW* const ppRW, const agl_cstr8_t** const pName, const agl_int32_t Index, LPNCKDataRW pRW, agl_cstr8_t const Name, const agl_int32_t NameLen, const agl_int32_t AllocBuff );

agl_int32_t   AGL_API AGL_NCK_ExtractNckAlarm            ( const void* const Buffer, const agl_int32_t BufferSize, LPNCKAlarm NCKAlarm );
agl_int32_t   AGL_API AGL_NCK_GetNCKDataRWByNCDDEItem    ( const agl_cstr8_t const Item, LPNCKDataRW DataRW, agl_int32_t* const ErrorPosition);


/*******************************************************************************

 Antriebs-Funktionen

*******************************************************************************/

//
// Lesen und Schreiben von Parametern
//
agl_int32_t   AGL_API AGL_Drive_ReadMix                  ( const agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Drive_ReadMixEx                ( const agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Drive_WriteMix                 ( const agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );
agl_int32_t   AGL_API AGL_Drive_WriteMixEx               ( const agl_int32_t ConnNr, LPDATA_RW40_DRIVE Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );


/*******************************************************************************

 Speicherverwaltungsfunktionen

*******************************************************************************/

void*         AGL_API AGL_malloc                         ( const agl_int32_t Size );
void*         AGL_API AGL_calloc                         ( const agl_int32_t Anz, const agl_int32_t Size );
void*         AGL_API AGL_realloc                        ( void* const Ptr, const agl_int32_t Size );
void*         AGL_API AGL_memcpy                         ( void* const DestPtr, const void* const SrcPtr, const agl_int32_t Len );
void*         AGL_API AGL_memmove                        ( void* const DestPtr, const void* const SrcPtr, const agl_int32_t Len );
agl_int32_t   AGL_API AGL_memcmp                         ( const void* const Ptr1, const void* const Ptr2, const agl_int32_t Len );
void          AGL_API AGL_free                           ( void* const Ptr );


/*******************************************************************************

 Konvertierfunktionen (diese benötigen keine Kommunikationsverbindung und arbeiten nur im Speicher)

*******************************************************************************/

agl_int16_t   AGL_API AGL_ReadInt16                      ( const agl_uint8_t* const Buff );
agl_long32_t  AGL_API AGL_ReadInt32                      ( const agl_uint8_t* const Buff );
agl_uint16_t  AGL_API AGL_ReadWord                       ( const agl_uint8_t* const Buff );
agl_ulong32_t AGL_API AGL_ReadDWord                      ( const agl_uint8_t* const Buff );
agl_float32_t AGL_API AGL_ReadReal                       ( const agl_uint8_t* const Buff );
agl_int32_t   AGL_API AGL_ReadS5Time                     ( const agl_uint8_t* const Buff );
agl_int32_t   AGL_API AGL_ReadS5TimeW                    ( const agl_uint16_t* const Buff );

void          AGL_API AGL_WriteInt16                     ( agl_uint8_t* const Buff, const agl_int16_t Val );
void          AGL_API AGL_WriteInt32                     ( agl_uint8_t* const Buff, const agl_int32_t Val );
void          AGL_API AGL_WriteWord                      ( agl_uint8_t* const Buff, const agl_uint16_t Val );
void          AGL_API AGL_WriteDWord                     ( agl_uint8_t* const Buff, const agl_uint32_t Val );
void          AGL_API AGL_WriteReal                      ( agl_uint8_t* const Buff, const agl_float32_t Val );
void          AGL_API AGL_WriteS5Time                    ( agl_uint8_t* const Buff, const agl_int32_t Val );
void          AGL_API AGL_WriteS5TimeW                   ( agl_uint16_t* const Buff, const agl_int32_t Val );

void          AGL_API AGL_Byte2Word                      ( agl_uint16_t* const OutBuff, const agl_uint8_t* const InBuff, const agl_int32_t AnzWords );
void          AGL_API AGL_Byte2DWord                     ( agl_ulong32_t* const OutBuff, const agl_uint8_t* const InBuff, const agl_int32_t AnzDWords );
void          AGL_API AGL_Byte2Real                      ( agl_float32_t* const OutBuff, const agl_uint8_t* const InBuff, const agl_int32_t AnzReals );
void          AGL_API AGL_Word2Byte                      ( agl_uint8_t* const OutBuff, const agl_uint16_t* const InBuff, const agl_int32_t AnzWords );
void          AGL_API AGL_DWord2Byte                     ( agl_uint8_t* const OutBuff, const agl_ulong32_t* const InBuff, const agl_int32_t AnzDWords );
void          AGL_API AGL_Real2Byte                      ( agl_uint8_t* const OutBuff, const agl_float32_t* const InBuff, const agl_int32_t AnzReals );

agl_int32_t   AGL_API AGL_GetBit                         ( const agl_uint8_t Wert, const agl_int32_t BitNr );
agl_uint8_t   AGL_API AGL_SetBit                         ( agl_uint8_t* const Buff, const agl_int32_t BitNr );
agl_uint8_t   AGL_API AGL_ResetBit                       ( agl_uint8_t* const Buff, const agl_int32_t BitNr );
agl_uint8_t   AGL_API AGL_SetBitVal                      ( agl_uint8_t* const Buff, const agl_int32_t BitNr, const agl_int32_t Val );

agl_int32_t   AGL_API AGL_Buff2String                    ( const agl_uint8_t* const Buff, agl_cstr8_t const Text, const agl_int32_t AnzChars );
agl_int32_t   AGL_API AGL_String2Buff                    ( agl_uint8_t* const Buff, const agl_cstr8_t const Text, const agl_int32_t AnzChars );
agl_int32_t   AGL_API AGL_Buff2WString                   ( const agl_uint8_t* const Buff, agl_wchar_t* const Text, const agl_int32_t AnzChars );
agl_int32_t   AGL_API AGL_WString2Buff                   ( agl_uint8_t* const Buff, const agl_wchar_t* const Text, const agl_int32_t AnzChars );
agl_int32_t   AGL_API AGL_S7String2String                ( const agl_uint8_t* const S7String, agl_cstr8_t const Text, const agl_int32_t MaxChars );
agl_int32_t   AGL_API AGL_String2S7String                ( agl_uint8_t* const S7String, const agl_cstr8_t const Text, const agl_int32_t MaxChars );

agl_int32_t   AGL_API AGL_BCD2Int16                      ( const agl_int16_t BCD, agl_int16_t* const Dual );
agl_int32_t   AGL_API AGL_BCD2Int32                      ( const agl_long32_t BCD, agl_long32_t* const Dual );
agl_int32_t   AGL_API AGL_Int162BCD                      ( const agl_int16_t Dual, agl_int16_t* const BCD );
agl_int32_t   AGL_API AGL_Int322BCD                      ( const agl_long32_t Dual, agl_long32_t* const BCD );
agl_float32_t AGL_API AGL_LongAsFloat                    ( const agl_long32_t Var );
agl_long32_t  AGL_API AGL_FloatAsLong                    ( const agl_float32_t Var );

agl_int32_t   AGL_API AGL_Text2DataRW                    ( const agl_cstr8_t const Text, LPDATA_RW40 RW );
agl_int32_t   AGL_API AGL_DataRW2Text                    ( LPDATA_RW40 RW, agl_cstr8_t const Text );

agl_int32_t   AGL_API AGL_S7DT2SysTime                   ( const agl_uint8_t* const Buff, agl_systemtime_t* const SysTime );
agl_int32_t   AGL_API AGL_SysTime2S7DT                   ( const agl_systemtime_t* const SysTime, agl_uint8_t* const Buff );
agl_int32_t   AGL_API AGL_TOD2SysTime                    ( const LPTOD pTOD, agl_systemtime_t* const SysTime );
agl_int32_t   AGL_API AGL_SysTime2TOD                    ( const agl_systemtime_t* const SysTime, LPTOD pTOD );
agl_int32_t   AGL_API AGL_S7Date2YMD                     ( const agl_uint16_t Date, agl_uint16_t* const Year, agl_uint16_t* const Month, agl_uint16_t* const Day );

agl_int32_t   AGL_API AGL_Float2KG                       ( agl_uint16_t* const pKG, const agl_float32_t* const pFloat, const agl_int32_t AnzFloats );
agl_int32_t   AGL_API AGL_KG2Float                       ( agl_float32_t* const pFloat, const agl_uint16_t* const pKG, const agl_int32_t AnzFloats );
agl_int32_t   AGL_API AGL_Float2DWKG                     ( agl_ulong32_t* const pKG, const agl_float32_t* const pFloat, const agl_int32_t AnzFloats );
agl_int32_t   AGL_API AGL_DWKG2Float                     ( agl_float32_t* const pFloat, const agl_ulong32_t* const pKG, const agl_int32_t AnzFloats );

agl_int32_t   AGL_API AGL_S7Ident2String                 ( const LPS7_IDENT pIdent, const agl_int32_t Index, agl_cstr8_t const Text, const agl_int32_t MaxChars );


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
agl_int32_t   AGL_API AGLSym_OpenProject                 ( const agl_cstr8_t const  Project, agl_ptrdiff_t* const PrjHandle );
agl_int32_t   AGL_API AGLSym_CloseProject                ( const agl_ptrdiff_t PrjHandle );

//
// Liste der unterstützten S7-CPU-Typen in Textdatei schreiben
//
agl_int32_t   AGL_API AGLSym_WriteCpuListToFile          ( const agl_cstr8_t const FileName );

//
// S7-Programme aufzählen und für den Zugriff auswählen
//
agl_int32_t   AGL_API AGLSym_GetProgramCount             ( const agl_ptrdiff_t PrjHandle, agl_int32_t* const ProgCount );
agl_int32_t   AGL_API AGLSym_FindFirstProgram            ( const agl_ptrdiff_t PrjHandle, agl_cstr8_t const Program );
agl_int32_t   AGL_API AGLSym_FindNextProgram             ( const agl_ptrdiff_t PrjHandle, agl_cstr8_t const Program );
agl_int32_t   AGL_API AGLSym_FindCloseProgram            ( const agl_ptrdiff_t PrjHandle );
agl_int32_t   AGL_API AGLSym_SelectProgram               ( const agl_ptrdiff_t PrjHandle, const agl_cstr8_t const Program );


//
// Symbole einer Symboltabelle eines S7-Programms aufzählen
//
agl_int32_t   AGL_API AGLSym_GetSymbolCount              ( const agl_ptrdiff_t PrjHandle, agl_int32_t* const SymCount);
agl_int32_t   AGL_API AGLSym_GetSymbolCountFilter        ( const agl_ptrdiff_t PrjHandle, agl_int32_t* const SymCount, const agl_cstr8_t const Filter);

agl_int32_t   AGL_API AGLSym_FindFirstSymbol             ( const agl_ptrdiff_t PrjHandle, agl_cstr8_t const AbsOpd, agl_cstr8_t const Symbol, agl_cstr8_t const Comment, agl_int32_t* const Format );
agl_int32_t   AGL_API AGLSym_FindFirstSymbolFilter       ( const agl_ptrdiff_t PrjHandle, agl_cstr8_t const AbsOpd, agl_cstr8_t const Symbol, agl_cstr8_t const Comment, agl_int32_t* const Format, const agl_cstr8_t const Filter);
agl_int32_t   AGL_API AGLSym_FindNextSymbol              ( const agl_ptrdiff_t PrjHandle, agl_cstr8_t const AbsOpd, agl_cstr8_t const Symbol, agl_cstr8_t const Comment, agl_int32_t* const Format );
agl_int32_t   AGL_API AGLSym_FindCloseSymbol             ( const agl_ptrdiff_t PrjHandle );


//
// Datenbausteine eines S7-Programms aufzählen
//
agl_int32_t   AGL_API AGLSym_ReadPrjDBCount              ( const agl_ptrdiff_t PrjHandle, agl_int32_t* const DBCount );
agl_int32_t   AGL_API AGLSym_ReadPrjDBCountFilter        ( const agl_ptrdiff_t PrjHandle, agl_int32_t* const DBCount, const agl_cstr8_t const Filter );

agl_int32_t   AGL_API AGLSym_ReadPrjDBList               ( const agl_ptrdiff_t PrjHandle, agl_uint16_t* const DBList, const agl_int32_t DBCount );
agl_int32_t   AGL_API AGLSym_ReadPrjDBListFilter         ( const agl_ptrdiff_t PrjHandle, agl_uint16_t* const DBList, const agl_int32_t DBCount, const agl_cstr8_t const Filter );

//
// Programmbausteine, Datenbausteine und Datentypen eines S7-Programms auflisten
//
agl_int32_t   AGL_API AGLSym_ReadPrjBlkCountFilter       ( const agl_ptrdiff_t PrjHandle, const agl_int32_t BlkType, agl_int32_t* const BlkCount, const agl_cstr8_t const Filter );
agl_int32_t   AGL_API AGLSym_ReadPrjBlkListFilter        ( const agl_ptrdiff_t PrjHandle, const agl_int32_t BlkType, agl_uint16_t* const BlkList, const agl_int32_t BlkCount, const agl_cstr8_t const Filter );


//
// Komponenten (DB-Symbole) eines Datenbausteins im S7-Programm aufzählen
//
agl_int32_t   AGL_API AGLSym_GetDbSymbolCount            ( const agl_ptrdiff_t PrjHandle, const agl_int32_t DBNr, agl_int32_t* const DBSymCount);
agl_int32_t   AGL_API AGLSym_GetDbSymbolCountFilter      ( const agl_ptrdiff_t PrjHandle, const agl_int32_t DBNr, agl_int32_t* const DBSymCount, const agl_cstr8_t const Filter);

agl_int32_t   AGL_API AGLSym_FindFirstDbSymbol           ( const agl_ptrdiff_t PrjHandle, const agl_int32_t DBNr, agl_cstr8_t const AbsOpd, agl_cstr8_t const Symbol, agl_cstr8_t const Comment, agl_int32_t* const Format );
agl_int32_t   AGL_API AGLSym_FindFirstDbSymbolFilter     ( const agl_ptrdiff_t PrjHandle, const agl_int32_t DBNr, agl_cstr8_t const AbsOpd, agl_cstr8_t const Symbol, agl_cstr8_t const Comment, agl_int32_t* const Format, const agl_cstr8_t const Filter );
agl_int32_t   AGL_API AGLSym_FindNextDbSymbol            ( const agl_ptrdiff_t PrjHandle, agl_cstr8_t const AbsOpd, agl_cstr8_t const Symbol, agl_cstr8_t const Comment, agl_int32_t* const Format );
agl_int32_t   AGL_API AGLSym_FindFirstDbSymbolEx         ( const agl_ptrdiff_t PrjHandle, const agl_int32_t DBNr, LPDATA_DBSYM40 Buff, const agl_cstr8_t const Filter );
agl_int32_t   AGL_API AGLSym_FindNextDbSymbolEx          ( const agl_ptrdiff_t PrjHandle, LPDATA_DBSYM40 Buff );
agl_int32_t   AGL_API AGLSym_GetDbSymbolExComment        ( const agl_ptrdiff_t PrjHandle, agl_cstr8_t const ExComment );
agl_int32_t   AGL_API AGLSym_FindCloseDbSymbol           ( const agl_ptrdiff_t PrjHandle );


//
// Abhängigkeit eines DBs ermitteln
//
agl_int32_t   AGL_API AGLSym_GetDbDependency       ( const agl_ptrdiff_t PrjHandle, const agl_int32_t DBNr, agl_int32_t* const BlkType, agl_int32_t* const BlkNr );

//
// Deklarationen eines Programmbausteins, Datenbausteins oder Datentyps im S7-Programm auflisten
//
agl_int32_t   AGL_API AGLSym_GetDeclarationCountFilter ( const agl_ptrdiff_t PrjHandle, const agl_int32_t BlkType, const agl_int32_t BlkNr, const agl_cstr8_t const Filter, agl_int32_t* const DeclarationCount );
agl_int32_t   AGL_API AGLSym_FindFirstDeclarationFilter( const agl_ptrdiff_t PrjHandle, const agl_int32_t BlkType, const agl_int32_t BlkNr, const agl_cstr8_t const Filter, agl_ptrdiff_t* const FindHandle, LPDATA_DECLARATION Buff );
agl_int32_t   AGL_API AGLSym_FindNextDeclaration       ( const agl_ptrdiff_t PrjHandle, const agl_ptrdiff_t FindHandle, LPDATA_DECLARATION Buff );
agl_int32_t   AGL_API AGLSym_GetDeclarationInitialValue( const agl_ptrdiff_t PrjHandle, const agl_ptrdiff_t FindHandle, agl_int32_t* const BufferLength, agl_cstr8_t const InitialValue );
agl_int32_t   AGL_API AGLSym_FindCloseDeclaration      ( const agl_ptrdiff_t PrjHandle, const agl_ptrdiff_t FindHandle );


//
// Übersetzung von Operanden- bzw. Symbolangaben
//
agl_int32_t   AGL_API AGLSym_GetSymbolFromText           ( const agl_ptrdiff_t PrjHandle, const agl_cstr8_t const Text, agl_cstr8_t const AbsOpd, agl_cstr8_t const Symbol, agl_cstr8_t const Comment, agl_int32_t* const Format );
agl_int32_t   AGL_API AGLSym_GetSymbolFromTextEx         ( const agl_ptrdiff_t PrjHandle, const agl_cstr8_t const Text, LPDATA_DBSYM40 Buff );
agl_int32_t   AGL_API AGLSym_GetReadMixFromText          ( const agl_ptrdiff_t PrjHandle, const agl_cstr8_t const Text, LPDATA_RW40 Buff, agl_int32_t* const Format );
agl_int32_t   AGL_API AGLSym_GetReadMixFromTextEx        ( const agl_ptrdiff_t PrjHandle, const agl_cstr8_t const Text, agl_cstr8_t const AbsOpd, LPDATA_RW40 Buff, agl_int32_t* const Format );

agl_int32_t   AGL_API AGLSym_GetSymbol                   ( const agl_ptrdiff_t PrjHandle, const LPDATA_RW40 Buff, agl_cstr8_t const AbsOpd, agl_cstr8_t const Symbol, agl_cstr8_t const Comment, agl_int32_t* const Format );
agl_int32_t   AGL_API AGLSym_GetSymbolEx                 ( const agl_ptrdiff_t PrjHandle, const LPDATA_RW40 Buff, LPDATA_DBSYM40 Symbol );

//
// Zugriff auf die Meldungskonfiguration und die Meldetexte eines S7-Programms
//
agl_int32_t   AGL_API AGLSym_OpenAlarms                  ( const agl_ptrdiff_t PrjHandle );
agl_int32_t   AGL_API AGLSym_CloseAlarms                 ( const agl_ptrdiff_t PrjHandle );

agl_int32_t   AGL_API AGLSym_FindFirstAlarmData          ( const agl_ptrdiff_t PrjHandle, agl_int32_t* const AlmNr);
agl_int32_t   AGL_API AGLSym_FindNextAlarmData           ( const agl_ptrdiff_t PrjHandle, agl_int32_t* const AlmNr);
agl_int32_t   AGL_API AGLSym_FindCloseAlarmData          ( const agl_ptrdiff_t PrjHandle );

agl_int32_t   AGL_API AGLSym_GetAlarmData                ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, LPDATA_ALARM40 Buff );

agl_int32_t   AGL_API AGLSym_GetAlarmName                ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, agl_cstr8_t const Buff, const agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_GetAlarmType                ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, agl_cstr8_t const Buff, const agl_int32_t BuffLen );

agl_int32_t   AGL_API AGLSym_GetAlarmBaseName            ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, agl_cstr8_t const Buff, const agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_GetAlarmTypeName            ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, agl_cstr8_t const Buff, const agl_int32_t BuffLen );

agl_int32_t   AGL_API AGLSym_GetAlarmSignalCount         ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, agl_int32_t* const SignalCount );

agl_int32_t   AGL_API AGLSym_GetAlarmMsgClass            ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, const agl_int32_t Signal, agl_int32_t* const MsgClass );
agl_int32_t   AGL_API AGLSym_GetAlarmPriority            ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, const agl_int32_t Signal, agl_int32_t* const Priority );
agl_int32_t   AGL_API AGLSym_GetAlarmAckGroup            ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, const agl_int32_t Signal, agl_int32_t* const AckGroup );
agl_int32_t   AGL_API AGLSym_GetAlarmAcknowledge         ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, const agl_int32_t Signal, agl_int32_t* const Acknowledge );

agl_int32_t   AGL_API AGLSym_GetAlarmProtocol            ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, agl_int32_t* const Protocol );
agl_int32_t   AGL_API AGLSym_GetAlarmDispGroup           ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, agl_int32_t* const DispGroup );

//
// Language = Windows LCID bzw. LANGID
//  intern wird nur der LANGID-Anteil LANGIDFROMLCID( LCID ) beachtet, nicht jedoch der SORTID-Anteil
//
agl_int32_t   AGL_API AGLSym_FindFirstAlarmTextLanguage  ( const agl_ptrdiff_t PrjHandle, agl_int32_t* const Language, agl_int32_t* const IsDefault);
agl_int32_t   AGL_API AGLSym_FindNextAlarmTextLanguage   ( const agl_ptrdiff_t PrjHandle, agl_int32_t* const Language, agl_int32_t* const IsDefault);
agl_int32_t   AGL_API AGLSym_FindCloseAlarmTextLanguage  ( const agl_ptrdiff_t PrjHandle );

//
// Language = Windows LCID bzw. LANGID
// Language = -1 -> Zurücksetzen auf Anfangswert [ System-Default-Sprache -> ::GetSystemDefaultLCID() ]
//
agl_int32_t   AGL_API AGLSym_SetAlarmTextDefaultLanguage ( const agl_ptrdiff_t PrjHandle, const agl_int32_t Language );

//
// Language = Windows LCID bzw. LANGID
// Language = -1 -> Verwendung der mittels AGLSym_SetAlarmTextDefaultLanguage() eingestellen Sprache
//
agl_int32_t   AGL_API AGLSym_GetAlarmText                ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, const agl_int32_t Signal, const agl_int32_t Language, agl_cstr8_t const Buff, const agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_GetAlarmInfo                ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, const agl_int32_t Signal, const agl_int32_t Language, agl_cstr8_t const Buff, const agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_GetAlarmAddText             ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, const agl_int32_t Signal, const agl_int32_t Index, const agl_int32_t Language, agl_cstr8_t const Buff, const agl_int32_t BuffLen );

//
// SCAN-Operand, SCAN-Intervall und SCAN-Begleitwerte 1 - 10
//
agl_int32_t   AGL_API AGLSym_GetAlarmSCANOperand         ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, agl_int32_t* const OpArea, agl_int32_t* const OpType, agl_int32_t* const Offset, agl_int32_t* const BitNr );
agl_int32_t   AGL_API AGLSym_GetAlarmSCANInterval        ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, agl_int32_t* const Interval );
agl_int32_t   AGL_API AGLSym_GetAlarmSCANAddValue        ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, const agl_int32_t Index, agl_int32_t* const OpArea, agl_int32_t* const OpType, agl_int32_t* const Offset, agl_int32_t* const BitNr );

agl_int32_t   AGL_API AGLSym_GetAlarmSCANOperandEx       ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, LPDATA_RW40 Buff );
agl_int32_t   AGL_API AGLSym_GetAlarmSCANAddValueEx      ( const agl_ptrdiff_t PrjHandle, const agl_int32_t AlmNr, const agl_int32_t Index, LPDATA_RW40 Buff );

//
// Formatierte Ausgabe von Begleitwerten in Meldetexten mit Formatangaben
//
agl_int32_t   AGL_API AGLSym_FormatMessage               ( const agl_ptrdiff_t PrjHandle, const agl_cstr8_t const AlarmText, const LPS7_ALARM AlarmData, agl_cstr8_t const Buff, const agl_int32_t BuffLen );

//
// Textbibliotheken eines S7-Programms aufzählen
//
agl_int32_t   AGL_API AGLSym_FindFirstTextlib            ( const agl_ptrdiff_t PrjHandle, agl_cstr8_t const Textlib, agl_int32_t* const System );
agl_int32_t   AGL_API AGLSym_FindNextTextlib             ( const agl_ptrdiff_t PrjHandle, agl_cstr8_t const Textlib, agl_int32_t* const System );
agl_int32_t   AGL_API AGLSym_FindCloseTextlib            ( const agl_ptrdiff_t PrjHandle );
agl_int32_t   AGL_API AGLSym_SelectTextlib               ( const agl_ptrdiff_t PrjHandle, const agl_cstr8_t const Textlib );

//
// Texte einer Textbibliothek eines S7-Programms aufzählen
//
agl_int32_t   AGL_API AGLSym_FindFirstTextlibText        ( const agl_ptrdiff_t PrjHandle, agl_int32_t* const TextId, agl_cstr8_t const Buff, const agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_FindNextTextlibText         ( const agl_ptrdiff_t PrjHandle, agl_int32_t* const TextId, agl_cstr8_t const Buff, const agl_int32_t BuffLen );
agl_int32_t   AGL_API AGLSym_FindCloseTextlibText        ( const agl_ptrdiff_t PrjHandle );
agl_int32_t   AGL_API AGLSym_GetTextlibText              ( const agl_ptrdiff_t PrjHandle, const agl_int32_t TextId, agl_cstr8_t const Buff, const agl_int32_t BuffLen );

//
// Formatierte Darstellung von Werten
//
agl_int32_t   AGL_API AGLSym_GetTextFromValue            ( const void* const Value, const agl_int32_t Format, const agl_int32_t ValueFmt, agl_cstr8_t const Text );
agl_int32_t   AGL_API AGLSym_GetValueFromText            ( const agl_cstr8_t const Text, void* const Value, agl_int32_t* const Format, agl_int32_t* const ValueFmt );
agl_int32_t   AGL_API AGLSym_GetRealFromText             ( const agl_cstr8_t const Text, agl_float32_t* const Value );
agl_int32_t   AGL_API AGLSym_GetTextFromReal             ( const agl_float32_t* const Value, agl_cstr8_t const Text );

//
// Einstellung der Befehls- bzw. Operandensprache
//
agl_int32_t   AGL_API AGLSym_SetLanguage                 ( const agl_int32_t Language );

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
agl_int32_t AGL_API AGL_WLD_OpenFile( const agl_cstr8_t const FileName, const agl_int32_t Access, agl_ptrdiff_t* const Handle );

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
agl_int32_t AGL_API AGL_WLD_OpenFileEncrypted( const agl_cstr8_t const FileName, const agl_int32_t Access, agl_uint8_t* const Key, const agl_uint32_t Len, agl_ptrdiff_t* const Handle );

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
agl_int32_t AGL_API AGL_WLD_EncryptFile( const agl_cstr8_t const InFileName, const agl_cstr8_t const OutFileName, const agl_int32_t Access, agl_uint8_t* const Key, const agl_uint32_t Len );

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
agl_int32_t AGL_API AGL_WLD_DecryptFile( const agl_cstr8_t const InFileName, const agl_cstr8_t const OutFileName, agl_uint8_t* const Key, const agl_uint32_t Len );

/*******************************************************************************

 Funktionsname  : AGL_WLD_CloseFile

 Parameter      : INT_PTR   Handle      Zugriffs-Handle auf Datei
                  
 Beschreibung   : Diese Funktion schreibt den Inhalt falls dieser geändert wurde
                  und schliesst die angegebene Datei ab

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_CloseFile( const agl_ptrdiff_t Handle );

/*******************************************************************************

 Funktionsname  : AGL_WLD_ReadAllBlockCount

 Parameter      : INT_PTR   Handle      Zugriffs-Handle auf Datei
                  LPALL_BLOCK_COUNT pBC Zeiger auf Struktur für Bausteine-Anzahl

 Beschreibung   : Diese Funktion liest die Anzahl aller vorhandenen Bausteine
                  innerhalb der angegebenen Datei

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_ReadAllBlockCount( const agl_ptrdiff_t Handle, LPALL_BLOCK_COUNT pBC );

/*******************************************************************************

 Funktionsname  : AGL_WLD_ReadBlockCount

 Parameter      : INT_PTR     Handle      Zugriffs-Handle auf Datei
                  agl_int32_t BlockType   Typ des Bausteines
                  agl_int32_t* BlockCount Zeiger auf Variable für Anzahl Bausteine

 Beschreibung   : Diese Funktion liest die Anzahl der vorhandenen Bausteine
                  innerhalb der angegebenen Datei für den angegebenen Bausteintyp

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_WLD_ReadBlockCount( const agl_ptrdiff_t Handle, const agl_int32_t BlockType, agl_int32_t* const BlockCount );

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
agl_int32_t AGL_API AGL_WLD_ReadBlockList( const agl_ptrdiff_t Handle, const agl_int32_t BlockType, agl_int32_t* const BlockCount, agl_uint16_t* const BlockList );

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
agl_int32_t AGL_API AGL_WLD_ReadBlockLen( const agl_ptrdiff_t Handle, const agl_int32_t BlockType, const agl_int32_t BlockNr, agl_int32_t* const BlockLen );

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
agl_int32_t AGL_API AGL_WLD_DeleteBlocks( const agl_ptrdiff_t Handle, const agl_cstr8_t const Blocks );

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
agl_int32_t AGL_API AGL_WLD_GetReport( const agl_ptrdiff_t Handle, agl_int32_t* const Length, agl_cstr8_t const Buffer );


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
agl_int32_t AGL_API AGL_PLC_Backup( const agl_int32_t ConnNr, const agl_ptrdiff_t Handle, const agl_cstr8_t Blocks, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

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
agl_int32_t AGL_API AGL_PLC_Restore( const agl_int32_t ConnNr, const agl_ptrdiff_t Handle, const agl_cstr8_t Blocks, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

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
agl_int32_t AGL_API AGL_PLC_DeleteBlocks( const agl_int32_t ConnNr, const agl_cstr8_t Blocks, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );


/*******************************************************************************

 Funktionsname  : AGL_Compress

 Parameter      : agl_int32_t ConnNr     Verbindungshandle
                  agl_int32_t Timeout    Timeoutwert für diese Funktion
                  LONG_PTR    UserVal    Benutzerdefinierter Wert

 Beschreibung   : Diese Funktion komprimiert den Speicher der SPS

 Rückgabewert   : Fehlernummer

 *******************************************************************************/
agl_int32_t AGL_API AGL_Compress( const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );


#if defined( __cplusplus )
  }
#endif

#endif  // #if !defined( __AGL_FUNCS__ )
