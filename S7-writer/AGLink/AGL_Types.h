/*******************************************************************************

 Projekt        : Neue Version der AGLink-Bibliothek

 Dateiname      : AGL_Types.H

 Beschreibung   : Definition der �ffentlichen Datentypen

 Copyright      : (c) 1998-2017
                  DELTALOGIC Automatisierungstechnik GmbH
                  Stuttgarter Str. 3
                  73525 Schw�bisch Gm�nd
                  Web : http://www.deltalogic.de
                  Tel.: +49-7171-916120
                  Fax : +49-7171-916220

 Erstellt       : 23.03.2004  RH

 Ge�ndert       : 16.12.2016  RH

 *******************************************************************************/

#if !defined( __AGL_TYPES__ )
#define __AGL_TYPES__

#if defined( _MSC_VER )
#pragma warning( disable : 4115 )                         // Warnungsmeldungen "Benannte Typdefinition in runden Klammern" unterdr�cken
#pragma warning( disable : 4201 )                         // Warnungsmeldungen "Nicht dem Standard entsprechende Erweiterung : Struktur/Union ohne Namen" unterdr�cken
#endif

/*******************************************************************************

 Definition der Typen

 *******************************************************************************/

#if defined( _MSC_VER ) && _MSC_VER > 600
#pragma pack( push, 1 )
#elif !defined( _UCC )
#pragma pack( 1 )
#endif

//
// SPS-Klasse f�r bIs200 (wurde erweitert wegen S7-1200, aber Name beibehalten)
//

struct PLC_Class
{
  enum enum_t
  {
    ePLC_300_400 = 0,
    ePLC_200 = 1,
    ePLC_1200 = 2,
    ePLC_Logo = 3
  };
};

enum PLC_ClassEx
{
  ePLCEx_300_400 = 0,
  ePLCEx_200 = 1,
  ePLCEx_1200_1500_Classic = 2,
  ePLCEx_Logo7_8 = 3,
  ePLCEx_NCK = 10,
  ePLCEx_Sinamics_MM = 11,
  ePLCEx_Simotion = 12,
  ePLCEx_1200_TIA = 20,
  ePLCEx_1200_GE_V4_TIA = 21,
  ePLCEx_1500_TIA = 30,
  ePLCEx_AUTO_TIA = 40
};

enum ConnTypeEx
{
  eCT_HMI = 0,
  eCT_ES = 1,
  eCT_OTHER = 2,
  eCT_DL = 3
};


//
// Struktur f�r das Ergebnis der Kommunikation
//

typedef struct tagRESULT40
{
  agl_long32_t         State;                              // Zustand der Kommunikation
  agl_long32_t         ErrCode;                            // Fehlercode bei Kommunikationsabbruch
  agl_systemtime_t     SysTime;                            // Datum und Uhrzeit beim Ende der Kommunikation
  agl_long32_t         SError;                             // ggf. Fehlercode von Hardware
  agl_ptrdiff_t        UserVal;                            // Wert zur freien Verwendung durch den Programmierer
} RESULT40, *LPRESULT40;


typedef struct tagPROGRESS
{
  agl_long32_t         AktVal;                             // Aktueller Byteszahl
  agl_long32_t         MaxVal;                             // Maximale Bytezahl
  agl_ptrdiff_t        UserVal;                            // Wert zur freien Verwendung durch den Programmierer
  agl_long32_t         Reserve[5];                         // For future extension
} PROGRESS, *LPPROGRESS;


typedef struct tagDATA_RW40
{
  agl_uint16_t        OpArea;                             // Bereich des Operanden auf den zugegriffen wird
  agl_uint16_t        OpType;                             // Datentyp
  agl_uint16_t        OpAnz;                              // Nur bei ReadMixEx und WriteMixEx von Bedeutung
  agl_uint16_t        DBNr;                               // Datenbausteinnummer
  agl_uint16_t        Offset;                             // Offset f�r den Speicherzugriff (bei T+Z Nummer)
  agl_uint16_t        BitNr;                              // Bitnummer (falls n�tig)
  agl_int32_t         Result;                             // Ergebnis (Fehlercode) der Lese- bzw. Schreiboperation
  union
  {
    agl_ulong32_t     Value;                              // Wert des Operanden als DWORD (ReadMix und WriteMix)
    agl_uint16_t      W[2];                               // F�r einfacheren Zugriff (ReadMix und WriteMix)
    agl_uint8_t       B[4];                               // F�r einfacheren Zugriff (ReadMix und WriteMix)
    agl_float32_t     fValue;                             // Wert des Operanden als Single (ReadMix und WriteMix)
    void*             Buff;                               // Zeiger auf Puffer (ReadMixEx und WriteMixEx)
  };
} DATA_RW40, *LPDATA_RW40;

/*******************************************************************************

 Definition der Funktionstypen

 *******************************************************************************/

typedef void (AGL_API *CB_FUNC)( agl_int32_t ConnNr, agl_int32_t JobNr, struct tagRESULT40 *AGResult );

/*******************************************************************************
//
// Struktur f�r Benachrichtigungsmechanismen
//
 *******************************************************************************/

//
// Achtung: Nicht verwendete Elemente m�ssen mit 0 initialisiert sein!!
//

typedef struct tagNOTIFICATION
{
  agl_handle_t        hWnd;                               // Fensterhandle f�r Benachrichtigung �ber Message
  agl_uint32_t        WndMessage;                         // Nummer der Nachricht
  agl_ulong32_t       dwThreadID;                         // Falls Nachricht direkt an einen Thread geschickt werden soll
  agl_uint32_t        ThreadMessage;                      // Gew�nschte Nachricht f�r Thread
  void*               hEvent;                             // Benachrichtigung �ber Event
  CB_FUNC             CB;                                 // Callback-Implementierung
} NOTIFICATION, *LPNOTIFICATION;


/*******************************************************************************
//
// Strukturen f�r Informationsabfragen
//
 *******************************************************************************/


typedef struct tagTOD
{
  agl_uint16_t        ClockStatus;                        // Siehe SPS-Beschreibung
  agl_uint8_t         Year;                               // Jahr in BCD (2-stellig)
  agl_uint8_t         Month;                              // Monat in BCD (2-stellig)
  agl_uint8_t         Day;                                // Tag in BCD (2-stellig)
  agl_uint8_t         Hour;                               // Stunde in BCD (2-stellig)
  agl_uint8_t         Minute;                             // Minute in BCD (2-stellig)
  agl_uint8_t         Second;                             // Sekunde in BCD (2-stellig)
  agl_uint8_t         Zero;                               // Sollte immer 0 sein
  agl_uint8_t         Weekday;                            // Wochentag, Sonntag = 1
} TOD, *LPTOD;


typedef struct tagDT
{
  agl_uint8_t         Year;                               // Jahr in BCD (2-stellig)
  agl_uint8_t         Month;                              // Monat in BCD (2-stellig)
  agl_uint8_t         Day;                                // Tag in BCD (2-stellig)
  agl_uint8_t         Hour;                               // Stunde in BCD (2-stellig)
  agl_uint8_t         Minute;                             // Minute in BCD (2-stellig)
  agl_uint8_t         Second;                             // Sekunde in BCD (2-stellig)
  agl_uint8_t         Zero;                               // Sollte immer 0 sein
  agl_uint8_t         Weekday;                            // Wochentag, Sonntag = 1
} DT, *LPDT;


//
// Mehrfachdeklaration der Type aus AGLink 3.x verhindern
//

#if !defined( __AGLINK__ )

typedef struct tagPLCINFO
{
  agl_ulong32_t        PAE;                                // Anzahl Eingangsbytes
  agl_ulong32_t        PAA;                                // Anzahl Ausgangsbytes
  agl_ulong32_t        Flags;                              // Anzahl Merkerbytes
  agl_ulong32_t        Timer;                              // Anzahl Timer
  agl_ulong32_t        Counter;                            // Anzahl Z�hler
  agl_ulong32_t        LogAddress;                         // Gr��e logischer Adressraum
  agl_ulong32_t        LocalData;                          // Gr��e Localdatenspeicher
} PLCINFO, *LPPLCINFO;


typedef struct tagMLFB
{
  agl_uint8_t         MLFB[21];                           // MLFB-Nummer als nullterminierter String
} MLFB, *LPMLFB;


typedef struct tagMLFBEX
{
  agl_uint16_t        PLCVer;                             // Ausgabestand PLC
  agl_uint16_t        PGASVer;                            // Ausgabestand PG-Anschaltung
  agl_uint8_t         MLFB[21];                           // MLFB-Nummer als nullterminierter String
} MLFBEX, *LPMLFBEX;


typedef struct tagCYCLETIME
{
  agl_int32_t         AktCycleTime;                       // Aktuelle Zykluszeit
  agl_int32_t         MinCycleTime;                       // Minimale Zykluszeit
  agl_int32_t         MaxCycleTime;                       // Maximale Zykluszeit
} CYCLETIME, *LPCYCLETIME;


typedef struct tagPROTLEVEL
{
  agl_int32_t         KeyProtLevel;                       // Durch Betriebsartenschalter eingestellte Schutzstufe
  agl_int32_t         ParaProtLevel;                      // Parametrierte Schutzstufe
  agl_int32_t         CPUProtLevel;                       // G�ltige Schutzstufe der CPU
  agl_int32_t         ModeSelector;                       // Stellung des Betriebsartenschalters
  agl_int32_t         StartupSwitch;                      // Stellung des Anlaufschalters
} PROTLEVEL, *LPPROTLEVEL;

#endif


//
// Und nun noch ein paar Strukturen f�r Infofunktionen
//

typedef struct tagS7_IDENT
{
  agl_char8_t         Text[16][40];                       // Platz genug f�r die ganzen Texte mit Reserve
} S7_IDENT, *LPS7_IDENT;


typedef struct tagS7_LED
{
  agl_ulong32_t        Led[32];                            // Platz f�r alle m�glichen LEDs mit Reserve f�r die Zukunft
} S7_LED, *LPS7_LED;


typedef struct tagEXT_MODULE_INFO
{
  agl_char8_t         Hardware[24];                       // Hardware-Bezeichnung als nullterminierter String
  agl_char8_t         HardwareVer[16];                    // Version der Hardware als nullterminierter String
  agl_char8_t         Firmware[24];                       // Firmware-Bezeichnung als nullterminierter String
  agl_char8_t         FirmwareVer[16];                    // Version der Firmware als nullterminierter String
  agl_char8_t         FirmwareExt1[24];                   // Firmware-Erweiterung1-Bezeichnung als nullterminierter String
  agl_char8_t         FirmwareExt1Ver[16];                // Version der Firmware-Erweiterung1 als nullterminierter String
  agl_char8_t         FirmwareExt2[24];                   // Firmware-Erweiterung2-Bezeichnung als nullterminierter String
  agl_char8_t         FirmwareExt2Ver[16];                // Version der Firmware-Erweiterung2 als nullterminierter String
} EXT_MODULE_INFO, *LPEXT_MODULE_INFO;


typedef struct tagBLOCK_COUNT
{
  agl_int32_t         BlockType;                          // Type des Bausteines
  agl_int32_t         BlockCount;                         // Anzahl der Bausteine
} BLOCK_COUNT, *LPBLOCK_COUNT;


typedef struct tagALL_BLOCK_COUNT
{
  BLOCK_COUNT         BC[8];                              // Platz f�r alle Bausteinarten (und auch Reserve)
} ALL_BLOCK_COUNT, *LPALL_BLOCK_COUNT;


typedef struct tagRED_CONN_STATE
{
  agl_int32_t         ActiveConn;                         // Momentan aktive Verbindung
  agl_int32_t         LinkStateConn1;                     // Zustand der Verbindung 1
  agl_int32_t         LinkStateConn2;                     // Zustand der Verbindung 2
  agl_int32_t         OpStateConn1;                       // Betriebszustand der SPS von Verbindung 1 (nur H-CPU-System)
  agl_int32_t         OpStateConn2;                       // Betriebszustand der SPS von Verbindung 2 (nur H-CPU-System)
} RED_CONN_STATE, *LPRED_CONN_STATE;


typedef struct tagDATA_RW40_RK
{
  agl_uint16_t        OpArea;                             // Bereich des Operanden auf den zugegriffen wird
  agl_uint16_t        OpAnz;                              // Anzahl der Elemente (Bytes oder Worte)
  agl_uint16_t        DBNr;                               // Datenbausteinnummer bei DB bzw. DX
  agl_uint16_t        Offset;                             // Offset f�r den Zugriff
  agl_uint8_t         KMByte;                             // Bytenummer des Koordinierungsmerkers (0-255)
  agl_uint8_t         KMBit;                              // Bitnummer des Koordinierungsmerkers (0-7 oder 15)
  agl_uint8_t         CPUNr;                              // CPU-Nummer (1-4 oder 15)
  agl_uint8_t         dummy;                              // F�llbyte f�r Alignement
  void*               Buff;                               // Zeiger auf Puffer
  agl_int32_t         BuffLen;                            // L�nge des Puffers f�r AGL_RKFetch, AGL_Recv_RKSend etc.
} DATA_RW40_RK, *LPDATA_RW40_RK;


/*******************************************************************************

 Definition der Ger�teparameter

 *******************************************************************************/

typedef struct tagS7_PROJ_1
{
  agl_long32_t        lPlcNr;                             // Zum logischen Mappen = virtuelle oder reale PLC-Nummer

  agl_uint8_t         bRemResource;                       // Entfernte Resourcen-ID aus Konfiguration
  agl_uint8_t         bRemRackNr;                         // Entfernte Rack-Nummer aus Konfiguration
  agl_uint8_t         bRemSlotNr;                         // Entfernte Slot-Nummer aus Konfiguration
  agl_uint8_t         bDummy1;                            // F�llbyte
  agl_uint8_t         bLocResource;                       // Lokale Resourcen-ID aus Konfiguration
  agl_uint8_t         bLocRackNr;                         // Lokale Rack-Nummer aus Konfiguration
  agl_uint8_t         bLocSlotNr;                         // Lokale Slot-Nummer aus Konfiguration
  agl_uint8_t         bDummy2;                            // F�llbyte
  union
  {
    agl_uint8_t       bIs200;                             // Flag ob es sich um eine Verbindung zu einer 200er handelt (alt)
    agl_uint8_t       bPLCClass;                          // Enum der SPS-Klasse (neu)
  };
  agl_uint8_t         bID;                                // Nur zur internen Verwendung bestimmt
  agl_uint8_t         bReserve[2];                        // Man wei� ja nie ...

  agl_ulong32_t        dwReserve[4];                       // Future extension
} S7_PROJ_1, *LPS7_PROJ_1;


typedef struct tagS7_PROJ
{
  S7_PROJ_1           Conn[MAX_PLCS];                    // Projektierte Verbindungen je Device
} S7_PROJ, *LPS7_PROJ;


typedef struct tagS7_ROUTE_1
{
  agl_long32_t        lPlcNr;                             // Zum logischen Mappen = virtuelle oder reale PLC-Nummer
  agl_ulong32_t       ProjectID;                          // Subnetz-ID Projekt
  agl_ulong32_t       SubnetID;                           // Subnetz-ID Subnetz
  agl_int32_t         AddressLen;                         // L�nge der folgenden Adresse (1 bei MPI, 4 bei TCP/IP)
  agl_uint8_t         Address[16];                        // Reserve tut gut
  agl_uint8_t         bConnType;                          // Verbindungsart der Ziel-CPU (CONN_PG, CONN_OP oder CONN_SONST)
  agl_uint8_t         bRackNr;                            // Racknummer Ziel-CPU
  agl_uint8_t         bSlotNr;                            // Slotnummer Ziel-CPU
  agl_uint8_t         bReserve[5];                        // sicher isch sicher
} S7_ROUTE_1, *LPS7_ROUTE_1;


typedef struct tagS7_ROUTE
{
  S7_ROUTE_1          Route[MAX_PLCS];                    // Routing Verbindungen je Device
} S7_ROUTE, *LPS7_ROUTE;


//
// Struktur f�r die S7-Verbindungs-Parameter
//

typedef struct tagS7CONN_IE_1
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_uint16_t        wConnID;                            // ID der Verbindung (nicht mehr notwendig seit 4.1.0)
  agl_uint16_t        wPlcNr;                             // Unused (wird nicht verwendet)
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bReserveL5;                         // Man wei� ja nie ...
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_uint8_t         bRemResource;                       // Entfernte Resourcen-ID aus Konfiguration
  agl_uint8_t         bRemRackNr;                         // Entfernte Rack-Nummer aus Konfiguration
  agl_uint8_t         bRemSlotNr;                         // Entfernte Slot-Nummer aus Konfiguration
  agl_uint8_t         bDummy1;                            // F�llbyte
  agl_uint8_t         bLocResource;                       // Lokale Resourcen-ID aus Konfiguration
  agl_uint8_t         bLocRackNr;                         // Lokale Rack-Nummer aus Konfiguration
  agl_uint8_t         bLocSlotNr;                         // Lokale Slot-Nummer aus Konfiguration
  agl_uint8_t         bDummy2;                            // F�llbyte
  agl_long32_t        lReserveL4[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_char8_t         Address[64];                        // TCP/IP-Adresse bzw. Namen der SPS
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_uint16_t        wPortNr;                            // PortNr wenn PAT erforderlich ist
  agl_uint16_t        wReserveL2;                         // Man wei� ja nie ...
  agl_long32_t        lReserveL2;                         // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t         lReserve[5];                        // Man wei� ja nie ...
} S7CONN_IE_1, *LPS7CONN_IE_1;


typedef struct tagS7CONN_IE
{
  agl_int32_t         iConnsUsed;                         // Anzahl verwendeter Verbindungen
  S7CONN_IE_1         Conn[MAX_PLCS];                     // Verbindungen je Device
  S7_PROJ             Proj;                               // Projektierte Verbindungen, hier nur zur internen Verwendung
} S7CONN_IE, *LPS7CONN_IE;


//
// Struktur f�r die S7-TCP/IP-Verbindungs-Parameter
//

typedef struct tagS7_TCPIP_1
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation
  agl_uint16_t        wDummy;                             // F�llwort f�r S7CONN_IE_1-Kompatibilit�t
  agl_uint16_t        wPlcNr;                             // Zum logischen Mappen = virtuelle PLC-Nummer
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bReserveL5;                         // Man wei� ja nie ...
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_uint8_t         bConnType;                          // Verbindungsart (PG/OP/Sonstige)
  agl_uint8_t         bRemRackNr;                         // Entfernte Rack-Nummer aus Konfiguration
  agl_uint8_t         bRemSlotNr;                         // Entfernte Slot-Nummer aus Konfiguration
  union
  {
    agl_uint8_t       bIs200;                             // Flag ob es sich um eine Verbindung zu einer 200er handelt (alt)
    agl_uint8_t       bPLCClass;                          // Enum der SPS-Klasse (neu)
  };
  agl_long32_t         lReserveL4[3];                      // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_char8_t         Address[64];                        // TCP/IP-Adresse bzw. Namen der SPS
  agl_long32_t         Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_uint16_t        wPortNr;                            // PortNr wenn PAT erforderlich ist
  agl_uint16_t        wOwnPortNr;                         // Eigene Portnummer wenn notwendig
  agl_ulong32_t        dwOwnAddress;                       // Adresse der gew�nschten Netzwerkkarte
  //
  // Sonstiges
  //
  agl_long32_t         lReserve[5];                        // Man wei� ja nie ...
} S7_TCPIP_1, *LPS7_TCPIP_1;


typedef struct tagS7_TCPIP
{
  S7_TCPIP_1          Conn[MAX_PLCS];                     // Verbindungen je Device
  S7_PROJ             Proj;                               // Projektierte Verbindungen
  S7_ROUTE            Route;                              // Routing-Verbindungen
} S7_TCPIP, *LPS7_TCPIP;


//
// Struktur f�r die S7-TCP/IP-TIA-Verbindungs-Parameter
//

typedef struct tagS7_TCPIP_TIA_1
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation
  agl_uint16_t        wDummy;                             // F�llwort f�r S7CONN_IE_1-Kompatibilit�t
  agl_uint16_t        wPlcNr;                             // Zum logischen Mappen = virtuelle PLC-Nummer
  agl_uint8_t         bPLCClassEx;                        // Enum der erweiterten SPS-Klasse
  agl_uint8_t         bReserveL5;                         // Man wei� ja nie ...
  agl_uint16_t        wReserveL5;                         // Man wei� ja nie ...
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_uint8_t         bConnTypeEx;                        // Verbindungsart (ES/HMI/Sonstige/DL)
  agl_uint8_t         bReserveL4[3];                      // Man wei� ja nie ...
  agl_long32_t        lReserveL4[3];                      // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_char8_t         Address[64];                        // TCP/IP-Adresse bzw. Namen der SPS
  agl_long32_t         Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_uint16_t        wPortNr;                            // PortNr wenn PAT erforderlich ist
  agl_uint16_t        wOwnPortNr;                         // Eigene Portnummer wenn notwendig
  agl_ulong32_t        dwOwnAddress;                       // Adresse der gew�nschten Netzwerkkarte
  //
  // Sonstiges
  //
  agl_long32_t         lReserve[5];                        // Man wei� ja nie ...
} S7_TCPIP_TIA_1, *LPS7_TCPIP_TIA_1;

// static assert( sizeof( S7_TCPIP_TIA_1 ) == sizeof( S7_TCPIP_1 ) );

typedef struct tagS7_TCPIP_TIA
{
  S7_TCPIP_TIA_1      Conn[MAX_PLCS];                     // Verbindungen je Device
} S7_TCPIP_TIA, *LPS7_TCPIP_TIA;



//
// Struktur f�r die S5-TCP/IP-Verbindungs-Parameter
//

typedef struct tagIPCONN
{
  agl_long32_t         LenLoc;                             // L�nge Local-TSAP
  agl_char8_t         Local[16];                          // Local-TSAP (max. 8 Bytes Nutzdaten laut INAT)
  agl_long32_t         LenRem;                             // L�nge Remote-TSAP
  agl_char8_t         Rem[16];                            // Remote-TSAP (max. 8 Bytes Nutzdaten laut INAT)
  agl_int32_t         PortNr;                             // Portnummer f�r Verbindungen ohne RFC 1006
} IPCONN, *LPIPCONN;


typedef struct tagS5_TCPIP_1
{
  //
  // Parameter f�r L5
  //
  agl_long32_t         lTimeOut;                           // Standard-Timeout f�r Kommunikation
  agl_long32_t         lPlcNr;                             // Zum logischen Mappen (future extension)
  agl_long32_t         lReserveL5[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_int32_t         boNoRFC1006;                        // Flag ob RFC 1006 nicht verwendet wird (nicht empfohlen)
  IPCONN              Send;                               // Einstellungen f�r Send
  IPCONN              Recv;                               // Einstellungen f�r Receive
  agl_long32_t        lReserveL4[3];                      // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_char8_t          Address[64];                        // TCP/IP-Adresse bzw. Namen der SPS
  agl_long32_t         Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_long32_t         lReserveL2[2];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t         lReserve[5];                        // Man wei� ja nie ...
} S5_TCPIP_1, *LPS5_TCPIP_1;


typedef struct tagS5_TCPIP
{
  S5_TCPIP_1          Conn[MAX_PLCS];                     // Verbindungen je Device (/2 da 2 IP-Verbindungen pro Verbindung notwendig sind)
} S5_TCPIP, *LPS5_TCPIP;


//
// Struktur f�r die RFC1006-Verbindungs-Parameter
//
typedef struct tagRFC_1006_SERVER
{
  agl_long32_t        LenLoc;                             // L�nge Local-TSAP
  agl_char8_t         Local[16];                          // Local-TSAP 
  agl_long32_t        LenRem;                             // L�nge Remote-TSAP
  agl_char8_t         Rem[16];                            // Remote-TSAP 
  agl_char8_t         RemAddress[64];                     // TCP/IP-Adresse der Gegenstelle
} RFC_1006_SERVER, *LPRFC_1006_SERVER;


typedef struct tagRFC_1006_1
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation
  agl_long32_t        lPlcNr;                             // Zum logischen Mappen (future extension)
  agl_long32_t        lReserveL5[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_long32_t        LenLoc;                             // L�nge Local-TSAP
  agl_char8_t         Local[16];                          // Local-TSAP 
  agl_long32_t        LenRem;                             // L�nge Remote-TSAP
  agl_char8_t         Rem[16];                            // Remote-TSAP 
  agl_long32_t        lReserveL4[4];                      // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_char8_t         Address[64];                        // TCP/IP-Adresse bzw. Namen der SPS
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_uint16_t        wPortNr;                            // PortNr wenn PAT erforderlich ist
  agl_uint16_t        wOwnPortNr;                         // Eigene Portnummer wenn notwendig
  agl_ulong32_t       dwOwnAddress;                       // Adresse der gew�nschten Netzwerkkarte
  agl_long32_t        lReserveL2[3];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[4];                        // Man wei� ja nie ...
} RFC_1006_1, *LPRFC_1006_1;


typedef struct tagRFC_1006
{
  RFC_1006_1          Conn[MAX_PLCS];                     // Verbindungen je Device 
} RFC_1006, *LPRFC_1006;


//
// Struktur f�r die NL-Verbindungs-Parameter
//

typedef struct tagS7_NL
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bReserveL5;                         // Man wei� ja nie ...
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_int32_t         ConnTyp;                            // Art der Verbindung (PG/OP/Sonstige)
  agl_long32_t        lReserveL4[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L3
  //
  agl_int32_t         UseFlashParas;                      // Parameter aus dem Flash �bernehmen
                                                          // Nur f�r UseFlashParas == false:
  agl_int32_t         HSA;                                // Default-HSA
  agl_int32_t         PGAdr;                              // Default-PGAdresse
  agl_int32_t         SingleMaster;                       // Flag ob PG/PC einziger Master am Bus ist
  agl_long32_t        MPIBaud;                            // Baudrate des MPI-Busses
  agl_int32_t         Profil;                             // Art der Parameter (MPI/DP/STD/FMS/USR/PPI)
                                                          // Nur f�r USR-Einstellungen:
  agl_ulong32_t       ttr;                                // Target Token Rotation Time
  agl_uint16_t        tslot;                              // Slot Time
  agl_uint16_t        tmin;                               // min. station delay responder
  agl_uint16_t        tmax;                               // max. station delay responder
  agl_uint16_t        align;                              // Nur zur Strukturausrichtung auf 4 Bytegrenze
  agl_uint8_t         tset;                               // Setup Time
  agl_uint8_t         tquiet;                             // Quiet Time
  agl_uint8_t         tgap;                               // Gap Update Time
  agl_uint8_t         retries;                            // Anzahl Wiederholungen
  agl_long32_t        L3Flags;                            // Flags f�r Advanced PPI etc.
  agl_long32_t        lReserveL3;                         // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_char8_t         Address[64];                        // TCP/IP-Adresse bzw. Namen des NetLinks
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_uint16_t        wSpecialPN;                         // Nur f�r internen Gebrauch
  agl_uint16_t        wOwnPortNr;                         // Eigene Portnummer wenn notwendig
  agl_ulong32_t       dwOwnAddress;                       // Adresse der gew�nschten Netzwerkkarte
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[7];                        // Man wei� ja nie ...

  //
  // Und jetzt noch die projektierten Verbindungen
  //
  S7_PROJ             Proj;                               // Projektierte Verbindungen
  S7_ROUTE            Route;                              // Routing-Verbindungen
} S7_NL, *LPS7_NL;


//
// Struktur f�r die NLPro-Verbindungs-Parameter
//

typedef struct tagS7_NLPRO
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bInternal;                          // For internal use only
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_int32_t         ConnTyp;                            // Art der Verbindung (PG/OP/Sonstige)
  agl_long32_t        lReserveL4[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L3
  //
  agl_int32_t         AutoDetect;                         // Bus-Parameter automatisch ermitteln
  agl_int32_t         HSA;                                // Default-HSA
  agl_int32_t         PGAdr;                              // Default-PGAdresse
  agl_int32_t         SingleMaster;                       // Flag ob PG/PC einziger Master am Bus ist
  agl_long32_t        MPIBaud;                            // Baudrate des MPI-Busses
  agl_int32_t         Profil;                             // Art der Parameter (MPI/DP/STD/FMS/USR/PPI)
                                                          // Nur f�r USR-Einstellungen:
  agl_ulong32_t        ttr;                                // Target Token Rotation Time
  agl_uint16_t        tslot;                              // Slot Time
  agl_uint16_t        tmin;                               // min. station delay responder
  agl_uint16_t        tmax;                               // max. station delay responder
  agl_uint16_t        align;                              // Nur zur Strukturausrichtung auf 4 Bytegrenze
  agl_uint8_t         tset;                               // Setup Time
  agl_uint8_t         tquiet;                             // Quiet Time
  agl_uint8_t         tgap;                               // Gap Update Time
  agl_uint8_t         retries;                            // Anzahl Wiederholungen
  agl_long32_t        L3Flags;                            // Flags f�r Advanced PPI etc.
  agl_long32_t        lReserveL3;                         // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_char8_t         Address[64];                        // TCP/IP-Adresse bzw. Namen des NetLinks
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_uint16_t        wSpecialPN;                         // Nur f�r internen Gebrauch
  agl_uint16_t        wOwnPortNr;                         // Eigene Portnummer wenn notwendig
  agl_ulong32_t       dwOwnAddress;                       // Adresse der gew�nschten Netzwerkkarte
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[7];                        // Man wei� ja nie ...

  //
  // Und jetzt noch die projektierten Verbindungen
  //
  S7_PROJ             Proj;                               // Projektierte Verbindungen
  S7_ROUTE            Route;                              // Routing-Verbindungen
} S7_NLPRO, *LPS7_NLPRO;


//
// Struktur f�r die NLUsb-Verbindungs-Parameter
//

typedef struct tagS7_NLUSB
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bInternal;                          // For internal use only
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_int32_t         ConnTyp;                            // Art der Verbindung (PG/OP/Sonstige)
  agl_long32_t        lReserveL4[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L3
  //
  agl_int32_t         AutoDetect;                         // Bus-Parameter automatisch ermitteln
  agl_int32_t         HSA;                                // Default-HSA
  agl_int32_t         PGAdr;                              // Default-PGAdresse
  agl_int32_t         SingleMaster;                       // Flag ob PG/PC einziger Master am Bus ist
  agl_long32_t        MPIBaud;                            // Baudrate des MPI-Busses
  agl_int32_t         Profil;                             // Art der Parameter (MPI/DP/STD/FMS/USR/PPI)
                                                          // Nur f�r USR-Einstellungen:
  agl_ulong32_t       ttr;                                // Target Token Rotation Time
  agl_uint16_t        tslot;                              // Slot Time
  agl_uint16_t        tmin;                               // min. station delay responder
  agl_uint16_t        tmax;                               // max. station delay responder
  agl_uint16_t        align;                              // Nur zur Strukturausrichtung auf 4 Bytegrenze
  agl_uint8_t         tset;                               // Setup Time
  agl_uint8_t         tquiet;                             // Quiet Time
  agl_uint8_t         tgap;                               // Gap Update Time
  agl_uint8_t         retries;                            // Anzahl Wiederholungen
  agl_long32_t        L3Flags;                            // Flags f�r Advanced PPI etc.
  agl_long32_t        lReserveL3;                         // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_char8_t         USBName[64];                        // Namen des NetLink-Treibers
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_long32_t        lReserveL2[2];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[7];                        // Man wei� ja nie ...

  //
  // Und jetzt noch die projektierten Verbindungen
  //
  S7_PROJ     Proj;                                        // Projektierte Verbindungen
  S7_ROUTE    Route;                                       // Routing-Verbindungen
} S7_NLUSB, *LPS7_NLUSB;


//
// Struktur f�r die Softing-Verbindungs-Parameter
//

typedef struct tagS7_Softing
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bInternal;                          // For internal use only
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_int32_t         ConnTyp;                            // Art der Verbindung (PG/OP/Sonstige)
  agl_long32_t        lReserveL4[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L3
  //
  agl_int32_t         NotUsed1;                           // Aus Kompatibilit�t zum NetLink
  agl_int32_t         HSA;                                // Default-HSA
  agl_int32_t         PGAdr;                              // Default-PGAdresse
  agl_int32_t         NotUsed2;                           // Aus Kompatibilit�t zum NetLink
  agl_long32_t        MPIBaud;                            // Baudrate des MPI-Busses
  agl_int32_t         Profil;                             // Art der Parameter (MPI/DP/STD/FMS/USR)
                                                          // Nur f�r USR-Einstellungen:
  agl_ulong32_t       ttr;                                // Target Token Rotation Time
  agl_uint16_t        tslot;                              // Slot Time
  agl_uint16_t        tmin;                               // min. station delay responder
  agl_uint16_t        tmax;                               // max. station delay responder
  agl_uint16_t        align;                              // Nur zur Strukturausrichtung auf 4 Bytegrenze
  agl_uint8_t         tset;                               // Setup Time
  agl_uint8_t         tquiet;                             // Quiet Time
  agl_uint8_t         tgap;                               // Gap Update Time
  agl_uint8_t         retries;                            // Anzahl Wiederholungen
  agl_long32_t        L3Flags;                            // Flags f�r Advanced PPI etc.
  agl_long32_t        lReserveL3;                         // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_int32_t         BoardNr;                            // Nummer des Profiboards
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_long32_t        lReserveL2[6];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[4];                       // Man wei� ja nie ...

  //
  // Und jetzt noch die projektierten Verbindungen
  //
  S7_PROJ             Proj;                               // Projektierte Verbindungen
  S7_ROUTE            Route;                              // Routing-Verbindungen
} S7_SOFTING, *LPS7_SOFTING;


//
// Struktur f�r die CIF-Verbindungs-Parameter
//

typedef struct tagS7_CIF
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bInternal;                          // For internal use only
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_int32_t         ConnTyp;                            // Art der Verbindung (PG/OP/Sonstige)
  agl_long32_t        lReserveL4[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L3
  //
  agl_int32_t         UseFlashParas;                      // Parameter aus dem Flash �bernehmen
                                                          // Nur f�r UseFlashParas == false:
  agl_int32_t         HSA;                                // Default-HSA
  agl_int32_t         PGAdr;                              // Default-PGAdresse
  agl_int32_t         NotUsed2;                           // Aus Kompatibilit�t zum NetLink
  agl_long32_t        MPIBaud;                            // Baudrate des MPI-Busses
  agl_int32_t         Profil;                             // Art der Parameter (MPI/DP/STD/FMS/USR)
                                                          // Nur f�r USR-Einstellungen:
  agl_ulong32_t       ttr;                                // Target Token Rotation Time
  agl_uint16_t        tslot;                              // Slot Time
  agl_uint16_t        tmin;                               // min. station delay responder
  agl_uint16_t        tmax;                               // max. station delay responder
  agl_uint16_t        align;                              // Nur zur Strukturausrichtung auf 4 Bytegrenze
  agl_uint8_t         tset;                               // Setup Time
  agl_uint8_t         tquiet;                             // Quiet Time
  agl_uint8_t         tgap;                               // Gap Update Time
  agl_uint8_t         retries;                            // Anzahl Wiederholungen
  agl_long32_t        L3Flags;                            // Flags f�r Advanced PPI etc.
  agl_long32_t        lReserveL3;                         // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_int32_t         BoardNr;                            // Nummer des CIF-Boards
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_long32_t        lReserveL2[6];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[4];                        // Man wei� ja nie ...

  //
  // Und jetzt noch die projektierten Verbindungen
  //
  S7_PROJ             Proj;                               // Projektierte Verbindungen
  S7_ROUTE            Route;                              // Routing-Verbindungen
} S7_CIF, *LPS7_CIF;


//
// Struktur f�r die CIFX-Verbindungs-Parameter
//

typedef struct tagS7_CIFX
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bInternal;                          // For internal use only
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_int32_t         ConnTyp;                            // Art der Verbindung (PG/OP/Sonstige)
  agl_long32_t        lReserveL4[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L3
  //
  agl_int32_t         UseFlashParas;                      // Parameter aus dem Flash �bernehmen
                                                          // Nur f�r UseFlashParas == false:
  agl_int32_t         HSA;                                // Default-HSA
  agl_int32_t         PGAdr;                              // Default-PGAdresse
  agl_int32_t         NotUsed2;                           // Aus Kompatibilit�t zum NetLink
  agl_long32_t        MPIBaud;                            // Baudrate des MPI-Busses
  agl_int32_t         Profil;                             // Art der Parameter (MPI/DP/STD/FMS/USR)
                                                          // Nur f�r USR-Einstellungen:
  agl_ulong32_t       ttr;                                // Target Token Rotation Time
  agl_uint16_t        tslot;                              // Slot Time
  agl_uint16_t        tmin;                               // min. station delay responder
  agl_uint16_t        tmax;                               // max. station delay responder
  agl_uint16_t        align;                              // Nur zur Strukturausrichtung auf 4 Bytegrenze
  agl_uint8_t         tset;                               // Setup Time
  agl_uint8_t         tquiet;                             // Quiet Time
  agl_uint8_t         tgap;                               // Gap Update Time
  agl_uint8_t         retries;                            // Anzahl Wiederholungen
  agl_long32_t        L3Flags;                            // Flags f�r Advanced PPI etc.
  agl_long32_t        lReserveL3;                         // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_int32_t         BoardNr;                            // Nummer des CifX-Boards
  agl_int32_t         ChannelNr;                          // Nummer des verwendeten Kanals auf dem CifX-Board
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_long32_t        lReserveL2[5];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[4];                        // Man wei� ja nie ...

  //
  // Und jetzt noch die projektierten Verbindungen
  //
  S7_PROJ             Proj;                               // Projektierte Verbindungen
  S7_ROUTE            Route;                              // Routing-Verbindungen
} S7_CIFX, *LPS7_CIFX;


//
// Struktur f�r die PC-Adapter-Verbindungs-Parameter
//

typedef struct tagS7_PCA
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bReserveL5;                         // Man wei� ja nie ...
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_int32_t         ConnTyp;                            // Art der Verbindung (PG/OP/Sonstige)
  agl_long32_t        lReserveL4[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L3
  //
  agl_int32_t         Reserved1;                          // Aus Kompatibilit�t zur NL-Familie
  agl_int32_t         HSA;                                // Default-HSA
  agl_int32_t         PGAdr;                              // Default-PGAdresse
  agl_int32_t         SingleMaster;                       // Flag ob PG/PC einziger Master am Bus ist
  agl_long32_t        MPIBaud;                            // Baudrate des MPI-Busses
  agl_int32_t         Profil;                             // Art der Parameter (MPI/DP/STD/FMS/USR/PPI)
                                                          // Nur f�r USR-Einstellungen:
  agl_ulong32_t       ttr;                                // Target Token Rotation Time
  agl_uint16_t        tslot;                              // Slot Time
  agl_uint16_t        tmin;                               // min. station delay responder
  agl_uint16_t        tmax;                               // max. station delay responder
  agl_uint16_t        align;                              // Nur zur Strukturausrichtung auf 4 Bytegrenze
  agl_uint8_t         tset;                               // Setup Time
  agl_uint8_t         tquiet;                             // Quiet Time
  agl_uint8_t         tgap;                               // Gap Update Time
  agl_uint8_t         retries;                            // Anzahl Wiederholungen
  agl_long32_t        lReserveL3[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_int32_t         PortNr;                             // Index der seriellen Schnittstelle (0=COM1, 1=COM2, ...)
  agl_long32_t        MaxBaud;                            // Maximal zu pr�fende Baudrate
  agl_long32_t        MinBaud;                            // Minimal zu pr�fende Baudrate
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_long32_t        lReserveL2;                         // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[7];                        // Man wei� ja nie ...

  //
  // Und jetzt noch die projektierten Verbindungen
  //
  S7_PROJ             Proj;                               // Projektierte Verbindungen
  S7_ROUTE            Route;                              // Routing-Verbindungen
} S7_PCA, *LPS7_PCA;


//
// Struktur f�r die RK512-Verbindungs-Parameter
//

typedef struct tagRK512_3964R
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_int32_t         CPUNr;                              // CPU-Nummer f�r RK512, g�ltige Werte 1-4 und 15
  agl_int32_t         KMByte;                             // Koppelmerkerbytenummer f�r RK512, g�ltige Werte  0-255
  agl_int32_t         KMBit;                              // Koppelmerkerbitnummer f�r RK512, g�ltige Werte 0-7 und 15
  agl_long32_t        lReserveL5[16];                     // Man wei� ja nie ...

  //
  // Parameter f�r L2
  //
  agl_int32_t         PortNr;                             // Index der seriellen Schnittstelle (0=COM1, 1=COM2, ...)
  agl_long32_t        Baud;                               // Zu verwendende Baudrate
  agl_int32_t         HighPrio;                           // Flag ob hohe Priorit�t verwendet wird
  agl_int32_t         UseBCC;                             // Flag ob 3964R (mit Pr�fsumme) verwendet wird
  agl_long32_t        lReserveL2[4];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[4];                        // Man wei� ja nie ...
} RK512_3964R, *LPRK512_3964R;


//
// Struktur f�r die AS511-Verbindungs-Parameter
//

typedef struct tagS5_AS511
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_long32_t        lReserveL5[3];                      // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_int32_t         PortNr;                             // Index der seriellen Schnittstelle (0=COM1, 1=COM2, ...)
  agl_long32_t        lReserveL2[3];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[8];                        // Man wei� ja nie ...
} S5_AS511, *LPS5_AS511;


//
// Struktur f�r die S7-PC/CP-Verbindungs-Parameter
//

typedef struct tagS7_PLCDEF
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lPlcNr;                             // Zum logischen Mappen = virtuelle PLC-Nummer
  agl_long32_t        lReserveL5[3];                      // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_uint8_t         bRemRackNr;                         // Entfernte Rack-Nummer aus Konfiguration
  agl_uint8_t         bRemSlotNr;                         // Entfernte Slot-Nummer aus Konfiguration
  union
  {
    agl_uint8_t       bIs200;                             // Flag ob es sich um eine Verbindung zu einer 200er handelt (alt)
    agl_uint8_t       bPLCClass;                          // Enum der SPS-Klasse (neu)
  };
  agl_uint8_t         bReserveL4;                         // Man wei� ja nie ...
  agl_long32_t        lReserveL4[3];                      // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_char8_t         Address[64];                        // TCP/IP- oder ISO-Adresse bzw. Namen der SPS
  agl_long32_t        lReserveL2[4];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[4];                        // Man wei� ja nie ...
} S7_PLCDEF, *LPS7_PLCDEF;


typedef struct tagS7_PCCP
{
  //
  // Parameter f�r L5
  //
  agl_char8_t         ConnName[64];                       // Zugriffspfad der Applikation
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bCPProto;                           // Nur zur internen Verwendung, Typ der Kommunikationsschnittstelle, wird bei OpenDevice eingetragen
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5[2];                      // Man wei� ja nie ...
  S7_PLCDEF           PlcTCPIP[MAX_PLCS];                 // F�r Lifelist etc. um mit "normaler" AG-Nummer zugreifen zu k�nnen
  S7_PLCDEF           PlcISO[MAX_PLCS];                   // F�r Lifelist etc. um mit "normaler" AG-Nummer zugreifen zu k�nnen
  //
  // Parameter f�r L4
  //
  agl_int32_t         ConnTyp;                            // Art der Verbindung (PG/OP/Sonstige)
  agl_long32_t        lReserveL4[3];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[4];                        // Man wei� ja nie ...

  //
  // Und jetzt noch die projektierten Verbindungen
  //
  S7_PROJ             Proj;                               // Projektierte Verbindungen
  S7_ROUTE            Route;                              // Routing-Verbindungen
} S7_PCCP, *LPS7_PCCP;


//
// Struktur f�r die PPI-Adapter-Verbindungs-Parameter
//

typedef struct tagS7_PPI
{
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_long32_t        lReserveL5[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_long32_t        lReserveL4[3];                      // Man wei� ja nie ...
  //
  // Parameter f�r L3
  //
  agl_int32_t         HSA;                                // Default-HSA
  agl_int32_t         PGAdr;                              // Default-PGAdresse
  agl_long32_t        lReserveL3[2];                      // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_int32_t         PortNr;                             // Index der seriellen Schnittstelle (0=COM1, 1=COM2, ...)
  agl_long32_t        Baudrate;                           // Baudrate
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_long32_t        lReserveL2[2];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[4];                        // Man wei� ja nie ...
} S7_PPI, *LPS7_PPI;


//
// Parameter f�r die Fernwartung
//

typedef struct tagS7_MODEM_AT
{
  //
  // Einstellung der G�ltigkeiten der Parameter. Damit k�nnen nur Teile ge�ndert werden, z.B. die entfernten Rufparameter
  //
  agl_int32_t         LocInterfaceValid;                  // Flag ob lokale Schnittstelleneinstellungen im Parametersatz g�ltig sind
  agl_int32_t         LocCallParasValid;                  // Flag ob die lokalen Rufparameter im Parametersatz g�ltig sind
  agl_int32_t         RemCallParasValid;                  // Flag ob die entfernten Rufparameter im Parametersatz g�ltig sind
  agl_int32_t         ReserveValid;                       // Man wei� ja nie ...

  //
  // Einstellung der lokalen Schnittstelle
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bReserveL5;                         // Man wei� ja nie ...
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_int32_t         ConnTyp;                            // Art der Verbindung (PG/OP/Sonstige)
  agl_long32_t        lReserveL4[3];                      // Man wei� ja nie ...
  //
  // Parameter f�r L3
  //
  agl_long32_t        lReserveL3[4];                      // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_int32_t         PortNr;                             // Index der seriellen Schnittstelle (0=COM1, 1=COM2, ...)
  agl_long32_t        MaxBaud;                            // Maximale Baudrate f�r Modem
  agl_int32_t         DataBits;                           // Datenbits:     7, 8
  agl_int32_t         Parity;                             // Parit�t:       AGL40_PARITY_NONE, AGL40_PARITY_EVEN, AGL40_PARITY_ODD
  agl_int32_t         StopBits;                           // Stopbits:      1, 2
  agl_int32_t         Handshake;                          // Handshakeart:  AGL40_HANDSHAKE_HW, AGL40_HANDSHAKE_SW, AGL40_HANDSHAKE_NONE
  agl_char8_t         InitModem1[MDM_INIT_LEN];           // Initialisierungsstring 1 f�r Modem, dieser wird auf Fehlerr�ckmeldung gepr�ft
  agl_char8_t         InitModem2[MDM_INIT_LEN];           // Initialisierungsstring 2 f�r Modem, dieser wird auf Fehlerr�ckmeldung gepr�ft
  agl_char8_t         InitModem3[MDM_INIT_LEN];           // Initialisierungsstring 3 f�r Modem, dieser wird auf Fehlerr�ckmeldung gepr�ft
  agl_char8_t         InitModem4[MDM_INIT_LEN];           // Initialisierungsstring 4 f�r Modem, dieser wird NICHT auf Fehlerr�ckmeldung gepr�ft (wegen AT+CPIN=xxxx)
  agl_char8_t         DialType;                           // W�hlverfahren: leer f�r Standard, 'T' f�r Tonwahl, 'P' f�r Pulswahl
  agl_char8_t         AutoAnswer;                         // Anzahl Klingelzeichen bis zum Annehmen eines Anrufes: leer f�r Standard, '0' bis '9'
  agl_char8_t         WaitForDialtone;                    // Ob auf W�hlton gewartet werden soll: leer f�r Standard, '4' f�r warten auf W�hlton, '3' f�r W�hlen ohne W�hlton
  agl_char8_t         dummy;                              // F�llbyte ohne Bedeutung
  agl_int32_t         DialRetries;                        // Anzahl Wahlwiederholungen, muss zwischen 1 und 5 (einschlie�lich) liegen
  agl_int32_t         RetryDelay;                         // Wartezeit in Sekunden, muss zwischen 0 und 30 (einschlie�lich) liegen
  agl_long32_t        MinDialUpTimeOut;                   // Mindest-Timeout in Sekunden f�r Funktion AGL_DialUp
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_long32_t        lReserveL2[3];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[4];                        // Man wei� ja nie ...

  //
  // Einstellung der lokalen Rufparameter
  //
  agl_char8_t         OwnNoCountry[MDM_PHONE_NO_LEN];     // Eigener Countrycode ohne internationalen Prefix, z.B. 49 f�r Deutschland, 41 f�r Schweiz ...
  agl_char8_t         OwnNoCity[MDM_PHONE_NO_LEN];        // Eigene Stadtvorwahl, z.B. 0711 f�r Stuttgart, 07171 f�r Schw�bisch Gm�nd ...
  agl_char8_t         OwnNoLocal[MDM_PHONE_NO_LEN];       // Eigene Rufnummer, z.B. 9160 f�r DELTALOGIC GmbH
  agl_char8_t         PreNoCountry[MDM_PHONE_NO_LEN];     // Amtsholung f�r internationale Verbindungen
  agl_char8_t         PreNoCity[MDM_PHONE_NO_LEN];        // Amtsholung f�r nationale Verbindungen
  agl_char8_t         PreNoLocal[MDM_PHONE_NO_LEN];       // Amtsholung f�r Ortsverbindungen
  agl_char8_t         ReserveLocPara[2*MDM_PHONE_NO_LEN]; // Man wei� ja nie ...

  //
  // Einstellung der entfernten Rufparameter
  //
  agl_char8_t         CallNoCountry[MDM_PHONE_NO_LEN];    // Countrycode der Zielnummer ohne internationalen Prefix
  agl_char8_t         CallNoCity[MDM_PHONE_NO_LEN];       // Stadtvorwahl der Zielnummer
  agl_char8_t         CallNoLocal[MDM_PHONE_NO_LEN];      // Rufnummer der Zielnummer
  agl_char8_t         User[MDM_PHONE_NO_LEN];             // Benutzerkennung f�r Anruf bei Adapter (8 Byte Nutzdaten!)
  agl_char8_t         Password[MDM_PHONE_NO_LEN];         // Passwort f�r Anruf bei Adapter (8 Byte Nutzdaten!)
  agl_char8_t         CallBackNo[MDM_PHONE_NO_LEN];       // Nummer f�r den R�ckruf
  agl_char8_t         ReserveRemPara[2*MDM_PHONE_NO_LEN]; // Man wei� ja nie ...
  agl_int32_t         UseCallBackNo;                      // Flag ob R�ckrufnummer mitgegeben werden soll
  agl_long32_t        lReserveCall[3];                    // Man wei� ja nie ...

  //
  // Und jetzt noch die projektierten Verbindungen
  //
  S7_PROJ             Proj;                               // Projektierte Verbindungen
  S7_ROUTE            Route;                              // Routing-Verbindungen
} S7_MODEM_AT, *LPS7_MODEM_AT;


typedef struct tagS7_MODEM_TAPI
{
  //
  // Einstellung der G�ltigkeiten der Parameter. Damit k�nnen nur Teile ge�ndert werden, z.B. die entfernten Rufparameter
  //
  agl_int32_t         LocInterfaceValid;                  // Flag ob lokale Schnittstelleneinstellungen im Parametersatz g�ltig sind
  agl_int32_t         LocCallParasValid;                  // Flag ob die lokalen Rufparameter im Parametersatz g�ltig sind
  agl_int32_t         RemCallParasValid;                  // Flag ob die entfernten Rufparameter im Parametersatz g�ltig sind
  agl_int32_t         ReserveValid;                       // Man wei� ja nie ...

  //
  // Einstellung der lokalen Schnittstelle
  //
  // Parameter f�r L5
  //
  agl_long32_t        lTimeOut;                           // Standard-Timeout f�r Kommunikation (synchrone Abfragen)
  agl_uint8_t         bCredits;                           // Maxmimale Anzahl Credits auf L5-Ebene, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_uint8_t         bReserveL5;                         // Man wei� ja nie ...
  agl_uint16_t        wMaxPDUSize;                        // Maximale PDU-Gr��e, 0 verwendet ACCON-AGLink-Standard-Einstellungen
  agl_long32_t        lReserveL5;                         // Man wei� ja nie ...
  //
  // Parameter f�r L4
  //
  agl_int32_t         ConnTyp;                            // Art der Verbindung (PG/OP/Sonstige)
  agl_long32_t        lReserveL4[3];                      // Man wei� ja nie ...
  //
  // Parameter f�r L3
  //
  agl_long32_t        lReserveL3[4];                      // Man wei� ja nie ...
  //
  // Parameter f�r L2
  //
  agl_char8_t         ModemName[128];                     // Name des Modems
  agl_char8_t         AutoAnswer;                         // Anzahl Klingelzeichen bis zum Annehmen eines Anrufes: leer f�r Standard, '0' bis '9'
  agl_char8_t         bDummy;                             // F�llbyte ohne Bedeutung
  agl_uint16_t        wDummy;                             // F�llwort ohne Bedeutung
  agl_int32_t         DialRetries;                        // Anzahl Wahlwiederholungen, muss zwischen 1 und 5 (einschlie�lich) liegen
  agl_int32_t         RetryDelay;                         // Wartezeit in Sekunden, muss zwischen 0 und 30 (einschlie�lich) liegen
  agl_long32_t        MinDialUpTimeOut;                   // Mindest-Timeout in Sekunden f�r Funktion AGL_DialUp
  agl_long32_t        Flags;                              // Flags f�r weitere Optionen (future extension, derzeit immer 0)
  agl_long32_t        lReserveL2[4];                      // Man wei� ja nie ...
  //
  // Sonstiges
  //
  agl_long32_t        lReserve[4];                        // Man wei� ja nie ...

  //
  // Einstellung der lokalen Rufparameter
  //
  agl_char8_t         OwnNoCountry[MDM_PHONE_NO_LEN];     // Eigener Countrycode ohne internationalen Prefix, z.B. 49 f�r Deutschland, 41 f�r Schweiz ...
  agl_char8_t         OwnNoCity[MDM_PHONE_NO_LEN];        // Eigene Stadtvorwahl, z.B. 0711 f�r Stuttgart, 07171 f�r Schw�bisch Gm�nd ...
  agl_char8_t         OwnNoLocal[MDM_PHONE_NO_LEN];       // Eigene Rufnummer, z.B. 9160 f�r DELTALOGIC GmbH
  agl_char8_t         PreNoCountry[MDM_PHONE_NO_LEN];     // Amtsholung f�r internationale Verbindungen
  agl_char8_t         PreNoCity[MDM_PHONE_NO_LEN];        // Amtsholung f�r nationale Verbindungen
  agl_char8_t         PreNoLocal[MDM_PHONE_NO_LEN];       // Amtsholung f�r Ortsverbindungen
  agl_char8_t         ReserveLocPara[2*MDM_PHONE_NO_LEN]; // Man wei� ja nie ...

  //
  // Einstellung der entfernten Rufparameter
  //
  agl_char8_t         CallNoCountry[MDM_PHONE_NO_LEN];    // Countrycode der Zielnummer ohne internationalen Prefix
  agl_char8_t         CallNoCity[MDM_PHONE_NO_LEN];       // Stadtvorwahl der Zielnummer
  agl_char8_t         CallNoLocal[MDM_PHONE_NO_LEN];      // Rufnummer der Zielnummer
  agl_char8_t         User[MDM_PHONE_NO_LEN];             // Benutzerkennung f�r Anruf bei Adapter (8 Byte Nutzdaten!)
  agl_char8_t         Password[MDM_PHONE_NO_LEN];         // Passwort f�r Anruf bei Adapter (8 Byte Nutzdaten!)
  agl_char8_t         CallBackNo[MDM_PHONE_NO_LEN];       // Nummer f�r den R�ckruf
  agl_char8_t         ReserveRemPara[2*MDM_PHONE_NO_LEN]; // Man wei� ja nie ...
  agl_int32_t         UseCallBackNo;                      // Flag ob R�ckrufnummer mitgegeben werden soll
  agl_long32_t        lReserveCall[3];                    // Man wei� ja nie ...

  //
  // Und jetzt noch die projektierten Verbindungen
  //
  S7_PROJ             Proj;                               // Projektierte Verbindungen
  S7_ROUTE            Route;                              // Routing-Verbindungen
} S7_MODEM_TAPI, *LPS7_MODEM_TAPI;


//
// Parameter f�r redundante Verbindungen
//

typedef struct tagS7_RED_CONN_2
{
  //
  // Parameter f�r L5
  //
  agl_int32_t           DevNr;                            // Device-Nummer der Verbindung
  agl_int32_t           PlcNr;                            // Plc-Nummer der Verbindung
  agl_int32_t           RackNr;                           // Rack-Nummer der Verbindung
  agl_int32_t           SlotNr;                           // Slot-Nummer der Verbindung
  agl_int32_t           KeepAliveTime;                    // Verbindungs�berwachungszeit der Verbindung (wenn keine Kommunikation stattfindet)
  agl_int32_t           RetryTime;                        // Zeitintervall in dem nach Ausfall ein Wiederaufbau der Verbindung versucht wird
  agl_ulong32_t         Reserve[2];                       // Man weiss ja nie ...
} S7_RED_CONN_2, *LPS7_RED_CONN_2;


typedef struct tagS7_RED_CONN_1
{
  S7_RED_CONN_2         Conn[2];                          // Verbindungsdaten f�r die prim�re/sekund�re Verbindung
  agl_ulong32_t         Flags;                            // Flags f�r die verbindungsabh�ngigen Optionen
  agl_ulong32_t         Reserve[3];                       // Man weiss ja nie ...
} S7_RED_CONN_1, *LPS7_RED_CONN_1;


typedef struct tagS7_RED_CONN
{
  S7_RED_CONN_1         Conn[MAX_PLCS];                   // Verbindungen je Device
  agl_ulong32_t         Flags;                            // Flags f�r die allgemeinen Optionen
  agl_ulong32_t         Reserve[7];                       // Man weiss ja nie ...
} S7_RED_CONN, *LPS7_RED_CONN;


//
// Strukturen f�r Scan und Alarm
//

typedef struct tagS7_DiagMsg
{
  agl_uint16_t        ID;                                 // Siehe Diagnosepuffer-Doku f�r Stopursache, bei WR_USMSG steht hier ID, entweder 0xA0??, 0xA1??, 0xB0??, 0xB1?? bzw.
  agl_uint16_t        Res1;                               // Bei Stop steht hier 0xFF + BZ�-Infos, bei WR_USMSG unbekannt
  agl_uint16_t        Res2;                               // Reserviert
  agl_uint16_t        Res3;                               // Reserviert, bei WR_USMSG steht hier der Parameter Info1
  agl_ulong32_t       AnlInfo;                            // Anlaufinfo, bei WR_USMSG steht hier der Parameter Info2
  DT                  Timestamp;                          // Zeitstempel der Meldung
} S7_DIAG_MSG, *LPS7_DIAG_MSG;


typedef struct tagS7_SCAN_ADD_VALUE                       // Einzelner Begleitwert f�r Scan-Wert
{
  agl_int32_t         DataType;                           // Datentyp des Zusatzwertes
  agl_int32_t         DataLen;                            // L�nge der Daten in Bytes
  agl_ulong32_t       Value;                              // Wert des Operanden als DWORD
} S7_SCAN_ADD_VALUE, *LPS7_SCAN_ADD_VALUE;


typedef struct tagS7_SCAN_VALUE                           // Scan-Wert mit Begleitwerten
{
  agl_ulong32_t       State;                              // Wert ist vorhanden oder nicht vorhanden (= Fehler)
  agl_ulong32_t       AckState;                           // Quittierungszustand des Ereignisses
  agl_ulong32_t       EventState;                         // Aktueller Zustand des Ereignisses
  agl_ulong32_t       EventId;                            // Meldungsnummer aus Projektierung
  agl_int32_t         AnzAddValues;                       // Anzahl der zus�tzlichen Werte
  S7_SCAN_ADD_VALUE AddValues[S7_SCAN_MAX_ADD_VALUES];    // Zusatzwerte (Begleitwerte)
} S7_SCAN_VALUE, *LPS7_SCAN_VALUE;


typedef struct tagS7_SCAN                                 // Scan-Ergebnisstruktur
{
  agl_ulong32_t       Intervall;                          // Intervall des Scans: 0x0200 = 100 ms, 0x0300 = 500 ms, 0x0400 = 1000 ms
  DT                  Timestamp;                          // Zeitstempel der Scan-Meldung
  agl_ulong32_t       Reserved;                           // F�r zuk�nftige Erweiterungen
  agl_int32_t         AnzValues;                          // Anzahl der gelesene Variablen
  S7_SCAN_VALUE ScanVal[S7_SCAN_MAX_VALUES];              // Damit wir f�r die Zukunft ger�stet sind gleich so viele in die Struktur aufnehmen
} S7_SCAN, *LPS7_SCAN;


typedef struct tagS7_ALARM_ADD_VALUE                      // Einzelner Begleitwert f�r Alarm-Wert
{
  agl_int32_t         DataType;                           // Datentyp des Zusatzwertes
  agl_int32_t         DataLen;                            // L�nge der Daten in Bytes
  agl_uint8_t         Data[S7_ALARM_MAX_ADD_VALUE_LEN+4]; // Daten als Bytearray
} S7_ALARM_ADD_VALUE, *LPS7_ALARM_ADD_VALUE;


typedef struct tagS7_ALARM                                // Alarm-Ergebnisstruktur
{
  agl_int32_t         MsgType;                            // Art der Meldung (MSG_SFB, MSG_SFC, ...) f�r Quittierung, Sperrung, Entsperrung, ...
  agl_ulong32_t       AckActive;                          // Quittierungsgetriggertes Melden f�r SFB 33-35 f�r diese Meldung relevant
  agl_ulong32_t       Severity;                           // Parameter Severity beim SFB-Aufruf
  agl_ulong32_t       State;                              // Wert ist vorhanden oder nicht vorhanden (= Fehler)
  agl_ulong32_t       AckState;                           // Quittierungszustand des Ereignisses
  agl_ulong32_t       EventState;                         // Aktueller Zustand des Ereignisses
  agl_ulong32_t       EventId;                            // Meldungsnummer aus Projektierung
  agl_ulong32_t       Reserved1;                          // F�r zuk�nftige Erweiterungen
  agl_ulong32_t       Reserved2;                          // F�r zuk�nftige Erweiterungen
  DT                  Timestamp;                          // Zeitstempel der Scan-Meldung
  agl_int32_t         AnzAddValues;                       // Anzahl der zus�tzlichen Werte
  S7_ALARM_ADD_VALUE  AlarmAddVal[S7_ALARM_MAX_ADD_VALUES]; // Damit wir f�r die Zukunft ger�stet sind gleich so viele in die Struktur aufnehmen
} S7_ALARM, *LPS7_ALARM;


typedef struct tagS7_MSG_STATE
{
    agl_ulong32_t       EventState;                         // Aktueller Zustand des Ereignisses
    agl_ulong32_t       EventId;                            // Meldungsnummer aus Projektierung
    agl_ulong32_t       AckState;                           // Quittierungszustand des Ereignisses (nur bei Quittierung)
} S7_MSG_STATE, *LPS7_MSG_STATE;


typedef struct tagS7_RCV_MSG_STATE
{
  agl_ulong32_t       MsgReason;                          // 0x01 = Meldung wurde quittiert, 0x02 = Meldung wurde gesperrt, 0x03 = Meldung wurde freigegeben
  DT                  Timestamp;                          // Zeitstempel der Ack-Meldung, bei Lock bzw. Unlock immer mit 0 gef�llt
  agl_int32_t         MsgAnz;                             // Anzahl der Nachrichten
  S7_MSG_STATE        Msg[60];                            // Die einzelnen Nachrichten. Bis jetzt kam immer nur eine vor, aber als Vorbereitung f�r die Zukunft ...
} S7_RCV_MSG_STATE, *LPS7_RCV_MSG_STATE;


typedef struct tagS7_CHANGE_MSG_STATE
{
  agl_ulong32_t        EventId;                            // Meldungsnummer, EV_ID bzw. AR_ID aus Projektierung
  agl_ulong32_t        AckState;                           // Quittierungszustand des Ereignisses (nur bei Quittierung)
  agl_ulong32_t        Result;                             // Ergebnis der �nderung (Fehlercode)
} S7_CHANGE_MSG_STATE, *LPS7_CHANGE_MSG_STATE;


typedef struct tagS7_OPEN_MSG_STATE
{
  agl_ulong32_t       State;                              // Wert ist vorhanden oder nicht vorhanden (= Fehler)
  agl_ulong32_t       AckState;                           // Quittierungszustand des Ereignisses
  agl_ulong32_t       EventState;                         // Aktueller Zustand des Ereignisses
  agl_ulong32_t       EventId;                            // Meldungsnummer aus Projektierung
  DT                  TimestampC;                         // Zeitstempel Kommt
  agl_int32_t         DataTypeC;                          // Datentyp des Zusatzwertes Kommt
  agl_int32_t         DataLenC;                           // L�nge der Daten in Bytes Kommt
  agl_uint8_t         DataC[12];                          // Daten als Bytearray Kommt
  DT                  TimestampG;                         // Zeitstempel Geht
  agl_int32_t         DataTypeG;                          // Datentyp des Zusatzwertes Geht
  agl_int32_t         DataLenG;                           // L�nge der Daten in Bytes Geht
  agl_uint8_t         DataG[12];                          // Daten als Bytearray Geht
} S7_OPEN_MSG_STATE, *LPS7_OPEN_MSG_STATE;


typedef struct tagS7_USEND_URCV_VAL
{
  agl_int32_t         DataType;                           // Datentyp des Zusatzwertes
  agl_int32_t         DataLen;                            // L�nge der Daten in Bytes
  agl_uint8_t         Data[S7_USEND_URCV_MAX_VALUE_LEN+4]; // Daten als Bytearray
} S7_USEND_URCV_VAL, *LPS7_USEND_URCV_VAL;


typedef struct tagS7_USEND_URCV
{
  agl_ulong32_t       R_ID;                               // Die R_ID f�r die Kommunikation
  agl_int32_t         AnzValues;                          // Anzahl der gelesenen Werte (1-4)
  S7_USEND_URCV_VAL   Val[S7_USEND_URCV_MAX_VALUES];
} S7_USEND_URCV, *LPS7_USEND_URCV;


/*******************************************************************************

 Strukturen f�r die NCK-Funktionen

*******************************************************************************/

typedef struct tagNCKDataRW
{
  agl_uint8_t         Area;                               // Area aus der gelesen / in die geschrieben werden soll (0-7)
  agl_uint8_t         Unit;                               // Unit aus der gelesen / in die geschrieben werden soll (0-31)
  agl_uint8_t         Block;                              // Baustein aus dem gelesen / in den geschrieben wird
  agl_uint8_t         RowCount;                           // Anzahl der Variablen die gelesen / geschrieben werden
  agl_uint16_t        Column;                             // Spalte die gelesen / geschrieben wird
  agl_uint16_t        Row;                                // Zeile die gelesen / geschrieben wird
  agl_int32_t         Result;                             // Ergebnis (Fehlercode) der Lese- bzw. Schreiboperation
  void*               Buff;                               // Zeiger auf Puffer f�r bzw. mit den Daten
  agl_int32_t         BuffLen;                            // L�nge des ben�tigten Daten und des allokierten Puffers
  agl_int32_t         DDEVarType;                         // Typ der Variablen aus nsk-Datei
  agl_int32_t         MDBVarType;                         // Typ der Variablen aus mdb- bzw. gud-Datei
} NCKDataRW, *LPNCKDataRW;

typedef struct tagNCKAlarmFilltext
{
  agl_int32_t Type;  // NCK_ALARM_TEXT_TYPE_?
  agl_char8_t Text[31+1];
} NCKAlarmFilltext, *LPNCKAlarmFilltext;

typedef struct tagNCKAlarm
{
  agl_ulong32_t Id; // Fehlernummer
  agl_ulong32_t AlarmNumber; // wird ab NCK-Start hochgezaehlt
  agl_ulong32_t ClearInfo; // wie wird der Alarm wieder geloescht - NCK_ALARM_CLEARED_?
  DT Timestamp; // S7 DT / DATETIME
  NCKAlarmFilltext Filltext[4];
} NCKAlarm, *LPNCKAlarm;

/*******************************************************************************

 Strukturen f�r die Antriebs-Funktionen

*******************************************************************************/

typedef struct tagDATA_RW40_DRIVE
{
  agl_uint16_t        OpType;                             // Angefragter Datentyp
  agl_uint16_t        OpAnz;                              // Anzahl der Variablen, die gelesen oder geschrieben werden sollen
  agl_uint16_t        Dev;                                // Nummer des Ger�tes, das gefragt werden soll. Kann aus der Hardwarekonfig ausgelesen werden.
  agl_uint16_t        ParaNum;                            // Nummer des Parameters, der gelesen oder geschrieben werden soll. Beim Parameter P0006 ist es die 6.
  agl_uint16_t        ParaInd;                            // Index des Parameters, der gelesen oder geschrieben werden soll. Beim Paramter P0013[0..49] ist es dir 0-49.
  agl_uint16_t        AlignWord;                          // Nur zu F�llzwecken
  agl_int32_t         Result;                             // Ergebnis der Operation
  union
  {
    agl_ulong32_t     Value;                              // Wert des Operanden als DWORD (ReadMix und WriteMix)
    agl_uint16_t      W[2];                               // F�r einfacheren Zugriff (ReadMix und WriteMix)
    agl_uint8_t       B[4];                               // F�r einfacheren Zugriff (ReadMix und WriteMix)
    agl_float32_t     fValue;                             // Wert des Operanden als Single (ReadMix und WriteMix)
    void*             Buff;                               // Zeiger auf Puffer (ReadMixEx und WriteMixEx)
  };
} DATA_RW40_DRIVE, *LPDATA_RW40_DRIVE;


/*******************************************************************************

 Strukturen f�r die 300/400 Symbolik-Funktionen

 *******************************************************************************/

typedef struct tagDATA_DBSYM40
{
  agl_char8_t         AbsOpd  [ AGLSYM_ABSOP_LEN ];       // DB-Komponente in absoluter Form (DB?.DBX/DBB/DBW/DBD?)
  agl_char8_t         Symbol  [ AGLSYM_SYMB_LEN ];        // DB-Komponente in symbolischer Form ("Symbol".Komponente)
  agl_char8_t         Comment [ AGLSYM_COMMENT_LEN ];     // Kommentar zur DB-Komponente
  agl_char8_t         DataType[ AGLSYM_DTYPE_LEN ];       // Datentyp in Textform ("BOOL", "STRING(100)")
  agl_int32_t         Format;                             // Datentyp in Bin�rform (AGLSYM_FORMAT_?)
  agl_uint16_t        DBNr;                               // DB-Nummer
  agl_uint16_t        Offset;                             // Byteadresse
  agl_uint16_t        BitNr;                              // Bitnummer
  agl_uint16_t        Size;                               // Datentypg��e in Bits
} DATA_DBSYM40, *LPDATA_DBSYM40;


typedef struct tagDATA_SIG40
{
  agl_char8_t         Text    [ AGLSYM_ALARM_TEXT_LEN ];  // Meldungstext (in der eingestellten Sprache)
  agl_char8_t         Info    [ AGLSYM_ALARM_TEXT_LEN ];  // Info-Text

  agl_char8_t         AddText [ AGLSYM_ALARM_ADDTEXT_NUM ][ AGLSYM_ALARM_TEXT_LEN ];  // Zusatz-Texte 1..9

  agl_int32_t         MsgClass;                           // Meldeklasse
  agl_int32_t         Priority;                           // Priorit�t
  agl_int32_t         AckGroup;                           // Quittiergruppe
  agl_int32_t         Acknowledge;                        // mit Quittierung
} DATA_SIG40, *LPDATA_SIG40;


typedef struct tagDATA_ALARM40
{
  agl_char8_t         Name    [ AGLSYM_ALARM_NAME_LEN ];  // Meldebezeichner
  agl_char8_t         Type    [ AGLSYM_ALARM_NAME_LEN ];  // Meldungstyp

  agl_char8_t         BaseName[ AGLSYM_ALARM_NAME_LEN ];  // Name des zugeordneten DBs ansonsten gleich Name
  agl_char8_t         TypeName[ AGLSYM_ALARM_NAME_LEN ];  // Name des zugrundeliegenden FBs ansonsten leer

  agl_int32_t         SignalCount;                        // Anzahl der Signale (= nachfolgend verwendete Eintr�ge)

  DATA_SIG40          Signal[ AGLSYM_ALARM_SIGNAL_NUM ];  // Meldungstexte und -daten der einzelnen Signale

  agl_int32_t         Protocol;                           // Protokollierung
  agl_int32_t         DispGroup;                          // Anzeigeklasse

  DATA_RW40           SCANOperand;                        // SCAN-Operand
  agl_int32_t         SCANInterval;                       // SCAN-Intervall (msec)

  DATA_RW40           SCANAddValue[ AGLSYM_ALARM_ADDVALUE_NUM ]; // SCAN-Begleitwerte 1..10
} DATA_ALARM40, *LPDATA_ALARM40;


typedef struct tagDATA_DECLARATION
{
  agl_int32_t  Size;                                   // Strukturgroesse in Bytes

  agl_char8_t  AddressTxt    [ AGLSYM_ADDRESS_LEN ];   // Adressierungsangabe in Textform z.B. "0.0", "+2.0", "=4.0", "*16.0", "+65534.0"
  agl_char8_t  MemoryClassTxt[ AGLSYM_MEMCLASS_LEN ];  // Speicherart in Textform z.B. "IN", "OUT", "IN_OUT", "STAT", "TEMP", "STAT:IN", "STAT:OUT"
  agl_char8_t  DepthTxt      [ AGLSYM_DEPTH_LEN ];     // Strukturtiefe in Textform (Leerzeichen)
  agl_char8_t  NameTxt       [ AGLSYM_NAME_LEN ];      // Name der Deklaration (max. 24 Zchn lt. Siemens)
  agl_char8_t  TypeTxt       [ AGLSYM_TYPE_LEN ];      // Datentypangabe in Textform z.B. "BOOL", "STRING[100]", "UDT 17", "ARRAY[1..10]"
  agl_char8_t  InitialValueTxt[ AGLSYM_INITVAL_LEN ];  // Anfangswert in Textform (ggf. weiterer Funktionsaufruf erforderlich)
  agl_char8_t  Comment       [ AGLSYM_COMMENT_LEN ];   // Kommentar zur Deklaration (max. 80 Zchn lt. Siemens)

  agl_int32_t  MemoryClass;                            // Speicherart (siehe Konstanten AGLSYM_MEMCLASS_...)
  agl_int32_t  Depth;                                  // Strukturtiefe (0 = oberste Strukturtiefe)
  agl_int32_t  Type;                                   // Datentyp (siehe Konstanten AGLSYM_TYPE_...)
  agl_int32_t  AdditionalInfo[ AGLSYM_ADDITIONAL_INFO_LEN ]; // Zusatzdaten entspr. Datentyp
  agl_int32_t  InitialValueLength;                     // L�nge des Anfangswerts in Textform 

} DATA_DECLARATION, *LPDATA_DECLARATION;

/*******************************************************************************

 Strukturen f�r die 300/400 Symbolik-Funktionen - Ende

 *******************************************************************************/

#if defined( _MSC_VER ) && _MSC_VER > 600
#pragma pack( pop )
#elif !defined( _UCC )
#pragma pack()
#endif


#endif  // #if !defined( __AGL_TYPES__ )
