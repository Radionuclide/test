// Datei CpDef.h
// Definitionen zu den Schnittstellen zu CP Baugruppen
// Das betrifft alle Parametrierungen Åber Seriell, Netz, ..
// Mail werner.mehrbrodt@inat.de, mehrbrodt@oberberg-online.de or werner.krings@inat.de
// Erstellt von Werner Krings

// History ///////////////////////////////////////////////////////////////
// V 1.0  25.11.95 W.K.	Erstellt
//	  02.12.95 W.K. Die USED_ Bits erweitert um S5 und SER
// V 1.1  08.12.95 W.K. Strukturen CP_STATION um Passwort erweitert
//			SETUP_INTERNAL von H1Msh nach hier verlegt
// V2.0	  24.12.96 WK   Angepasst fÅr neue Version S5-TCP+H1
// V2.1	  10.06.97 WK   Interval in CP_VERBINDUNGSDATEN neu
// V2.2   01.11.98 WK	ECHO_CONNECTION neu
// V2.3   01.03.99 WK	CP_STATION nun mit Adapternummer

// Letzte énderung am 01.03.99 WK

#ifndef CPDEF_H_INTERN			// Verriegelung
 #define CPDEF_H_INTERN

#include <WMKTypes.h>                   // Compiler Anpassungen
#include <DrvFCall.h>			// Treiber Aufrufcodes
#include <S5Access.h>
#include <H1Def.h>
#include <IpDef.h>
#include <SerDef.h>
#include <DrvListe.h>

// Definitionen //////////////////////////////////////////////////////////

#define MAX_NET_DATALEN		((520 * 3) + 4)	// 3 Telegramme + Header

#define CP_MINREV_IP		120	// Ab dieser Version ist IP mîglich
#define MAX_RELEASE		1000	// Das ist die maximale Version
#define CP_MINREV_MAKEROM 	126	// Ab dieser Version kann die Hardware ROMs erstellen
#define CP_MINREV_CPPARAM	125	// Ab dieser Version gehen CP Parameter Kachel

#define FTP_WRITE_ALLOWED	1	//

// Werte fÅr Used. Die unteren 8 Bits geben an, was gelten soll, die oberen Bits
// geben die GÅltigkeit der Werte nach einer PrÅfung an
// Die Bits werden auch bei bei ListeVerbindungen benutzt
#define USED_LEITUNG_BENUTZT	0x0000001L	// Diese Verbindung wird automatisch eingetragen im CP
#define USED_DOPPELVERBINDUNG	0x0000002L	// Das ist ein Teil einer Doppelverbindung
#define USED_H1			0x0000004L	// H1 Parameter vorhanden
#define USED_IP			0x0000008L	// IP Parameter vorhanden
#define USED_ECHO		0x0000010L	// EchoChange Parameter vorhanden
#define USED_TF			0x0000020L	// TF Parameter vorhanden
#define USED_S5			0x0000040L	// S5 Parameter vorhanden
#define USED_FTP		0x0000080L	// Ftp Parameter vorhanden
#define USED_S7			0x0000100L	// S7 Parameter vorhanden
#define USED_RFC1006		0x0000200L	// RFC1006 Parameter vorhanden
#define USED_PG			0x0000400L	// Over PG serial
#define USED_EVENT_FETCH	0x0000800L	// Event Fetch benutzen
#define USED_SIMULATOR		0x0001000L	// Simulator Parameter vorhanden
#define USED_TWOLINES		0x0002000L	// Bei Visus: Fetch Write: 2 Verbindungen
#define USED_WRITE_ALLOWED	0x0004000L	// Bei Visus: Schreiben erlaubt
#define USED_EXT_S5		0x0008000L	// Erweiterte S5 Parameter vorhanden

#define USED_COMMLINE	(USED_IP | USED_H1 | USED_ECHO | USED_FTP)
// #define USED_H1IP	(USED_IP | USED_H1)	// fÅr EchoChange

#define VALID_H1		0x0010000L	// H1 Parameter syntaktisch korrekt
#define VALID_IP		0x0020000L	// IP Parameter syntaktisch korrekt
#define VALID_ECHO		0x0040000L	// EchoChange Parameter syntaktisch korrekt
#define VALID_DP		0x0080000L	// DP Parameter syntaktisch korrekt
#define VALID_S5		0x0100000L	// S5 Parameter syntaktisch korrekt
#define VALID_FTP		0x0200000L	// Ftp Parameter syntaktisch korrekt
#define VALID_S7		0x0400000L	// S7 Parameter syntaktisch korrekt
#define VALID_RFC1006		0x0800000L	// RFC1006 Parameter syntaktisch korrekt
#define VALID_PG		0x1000000L	// PG Parameter syntaktisch korrekt
//#define VALID_SER		0x2000000L	// Seriell Parameter syntaktisch korrekt
#define VALID_SIMULATOR		0x4000000L	// DDE Parameter syntaktisch korrekt
#define VALID_EXT_S5		0x8000000L	// Erweiterte S5 Parameter syntaktisch korrekt

#define USED_INTERVAL		0x10000000L	// Die Intarvallzeit ist benutzt
#define USED_EVENT		0x20000000L	// Bei visus: Eventline erlaubt
#define NOTUSED_CYCLIC		0x40000000L	// Bei visus: Zyklisches Lesen
#define USED_ALL_INT		0x80000000L	// Kachel: Immer ALL benutzen

#define PRODUCT_H1		0x00000001L	// 
#define PRODUCT_TCPIP		0x00000002L	// 
#define PRODUCT_ECHOLINK	0x00000004L	// 
#define PRODUCT_ECHOCHANGE	0x00000008L	// 
#define PRODUCT_FTP		0x00000010L	// 
#define PRODUCT_IPC620		0x00000020L	// IPC620 Funktions
#define PRODUCT_LINK_AS511	0x00001000L	// combine with PRODUCT_ECHOLINK
#define PRODUCT_LINK_3964R	0x00002000L	// combine with PRODUCT_ECHOLINK
#define PRODUCT_TWOADAPTER	0x00100000L	// combine with PRODUCT_ECHOCHANGE

// Strukturen ////////////////////////////////////////////////////////////

#pragma pack(1)

typedef struct _CP_VALS {
	unsigned short cb;		// LÑnge der Struktur in Bytes
	unsigned short SerNo;		// Seriennummer der CP
	unsigned short KachelNo;	// Nummer der eingestellten Kachel
	unsigned short KachelRev;	// Revision des Kacheltreibers
	unsigned short H1Rev;		// Revision des H1 Treibers
	unsigned short IpRev;		// Revision der TCP/IP Treibers
	unsigned short TermRev;		// Revision des Terminals
	unsigned short MainRev;		// Revision der Zentralverwaltung
	unsigned short MacRev;		// Revision des MAC Treibers
	unsigned char StationAdr[6];	// Ethernet ROM Stationsadresse Karte 1
	unsigned long IpAddr;		// IP Adresse
	unsigned long IpSubnet;		// IP Adresse
	unsigned char Copyright[16];	// Copyright Text
	unsigned char TypText[16];	// BG Typ Text
	unsigned short RomRev;		// Revisionsstand des Firmware ROM
	unsigned short FtpRev;		// Revisionsstand FTP Dienst
	unsigned short EchoRev;		// Version EchoLink
	unsigned long Produkt;		// PRODUCT_xx
	unsigned char Station2Adr[6];	// Ethernet ROM Stationsadresse Karte 2
	unsigned short EcRev;		// 
	char res[10];			// reserve, must be 0
}CP_VALS;

#ifndef CPMEM_H_INTERN
typedef struct _EPROM_DATEN {
	unsigned long StartOffset;	// Startoffset zur EEPROM Basisadresse
	unsigned long Len;		// Anzahl Bytes
	short Fehler;			// Fehlermeldungen
	unsigned char Daten[1];         // Datenfeld
}EPROM_DATEN;
#endif

typedef struct _ECHO_COM_SETUP {
	unsigned short Cb;		// Grî·e der Struktur in Bytes
	unsigned short PortAvailible[6];// 
	COMM_LINE Ser[6];		// Parameter der Leitung
	char reserved[100];		//
}ECHO_COM_SETUP;

typedef struct _ECHO_COM_DIAGNOSTICS {
	unsigned short Cb;		// Grî·e der Struktur in Bytes
	unsigned short Port;		// 0 = COM1
	char reserved[100];		//
}ECHO_COM_DIAGNOSTICS;

typedef struct _TELNET_SETUP {
	unsigned short Cb;		// Grî·e der Struktur in Bytes
	COMM_LINE Ser;			// Parameter der Leitung
	char reserved[40];
}TELNET_SETUP;

typedef struct _TELNET_DIAGNOSTICS {
	unsigned short Cb;		// Grî·e der Struktur in Bytes
	unsigned long SendTelegramme;	//
	unsigned long RecTelegramme;	//
	unsigned short Disconnects;	// Sooft ist die Verbindung beendet
	unsigned long SendBytes;	//
	unsigned long RecBytes;		//
	unsigned short Line;		// 0 = line 1
	unsigned short NetSendStatus;	// net.Status
	unsigned short NetRecStatus;	// net.Status
	char reserved[14];
}TELNET_DIAGNOSTICS;

typedef struct {
	short AuftragNo;		// Auftragsnummer
	short AuftragOffset;		// Kacheloffset
	short Anzw;			// Das Anzeigewort der Leitung
	short Status;			// Zustand der Leitung
	long SendTelegramme;		// Statistik: Anzahl der gesendeten Telegramme
	long RecTelegramme;		// Statistik: Anzahl der gelesenen Telegramme
}CP_LINE;

typedef struct _F_REQUEST {
	unsigned short Cb;		// Grî·e der Struktur in Bytes
	unsigned short PollType;	// Type of the poll area for request. 0 = no poll
	unsigned short PollStart;	// Start of the poll area for request
	unsigned short PollLen;		// len of the poll area for request in units
	unsigned short AnswerType;	// Type of the answer area for request. 0 = no answer
	unsigned short AnswerStart;	// Start of the answer area for request
	unsigned short AnswerLen;	// len of the answer area for request in units
	unsigned short ConfirmType;	// Type of the confirm area for request. 0 = no confirm
	unsigned short ConfirmStart;	// Start of the confirm area for request
	unsigned short ConfirmLen;	// len of the confirm area for request in units
}F_REQUEST;

typedef struct _IPC_DIAGNOSTICS {
	unsigned short Cb;		// Grî·e der Struktur in Bytes
	unsigned short NetStatus;	// 0 -> no error
	unsigned short SerStatus;	// 0 -> no error
	unsigned long SerSendBytes;
	unsigned long SerRecBytes;
	unsigned long NetSendFrames;
	unsigned long NetRecFrames;
	unsigned short ReqVal;		// 1st value of the request buffer
	unsigned short DataVal;		// 1st value of the data buffer
	unsigned char reserved[16];
}IPC_DIAGNOSTICS;

typedef struct _IPC_CONNECTION {
	unsigned short Cb;		// Grî·e der Struktur in Bytes
	unsigned long Used;		// Bitcodierter Wert fÅr belegten Inhalt
	short Status;			// Wert fÅr RÅckgabewerte
	char Verbindungsname[32];	// ASCII Verbindungsname
	unsigned short Karte;		// 0 fÅr Karte 1
// Bis hier gleich zu ECHO_CONNECTION
	CONNECT_PARAMS H1;		// TSAP, NSAP, ..     fÅr H1
	IP_CONNECT_PARAMS Ip;		// Ports, Station, .. fÅr IP
	COMM_LINE Ser;			// parameter of the serial line
	F_REQUEST Param;		// parametrised values
	unsigned short PollTime;	// Zeit in 1/100 ms
	char reserved[60];
}IPC_CONNECTION;

typedef struct {
	short Kennung;			// Typ des Bausteins 0 = unbenutzt
	short Baustein;			// DB Nummer
	unsigned short Offset;		// Startoffset im Baustein
	unsigned short Len;		// Anzahl Daten
	short AnzwKennung;		// Typ des Bausteins 0 = unbenutzt
	short AnzwDb;			// DB des Anzeigewortes 0 = unbenutzt
	unsigned short AnzwDw;		// Offset des Anzeigewortes im Baustein
}ERW_KACHELPARAMS;

typedef struct {
	S5_ANSCHALTUNG S5;		// AuftragsNo, ..
	ERW_KACHELPARAMS Erw;		// Erweiterte Kachelanschaltung
}S5_CONNECT_PARAMS;

typedef struct {
	short Cb;			// Grî·e der Struktur in Bytes
	unsigned char NoConnections;	// Anzahl FTP Verbindungen
	unsigned char Flags;		// Bit 0 -> Write allowed
	unsigned short Anr;		// Auftragsnummer
//	unsigned char AnrRec;		// Auftragsnummer fÅr die RecauftrÑge
//	char PasswordRead[32];		// Passwort zum Lesen
//	char PasswordWrite[32];		// Passwort zum Schreiben
}FTP_CONNECT_PARAMS;

typedef struct {
	short Cb;			// Grî·e der Struktur in Bytes
	unsigned long Used;		// Bitcodierter Wert fÅr belegten Inhalt
	short Status;			// Wert fÅr RÅckgabewerte
	char Verbindungsname[32];	// ASCII Verbindungsname
	unsigned short Karte;		// 0 fÅr Karte 1
	S5_CONNECT_PARAMS S5;		// AuftragsNo, ..
	CONNECT_PARAMS H1;		// TSAP, NSAP, ..     fÅr H1
	IP_CONNECT_PARAMS Ip;		// Ports, Station, .. fÅr IP
	unsigned long Interval;		// z.B. Pollrate, in ms
	FTP_CONNECT_PARAMS Ftp;		// Nummern, ..        fÅr FTP
	char reserved[8];		// Reserviert fÅr Erweiterungen, mu· 0 sein
}CP_VERBINDUNGSDATEN_ALT;

typedef struct _TF_VALS{
	short Cb;			// Grî·e der Struktur in Bytes
	unsigned short VarTyp;		// TF_KOMMANDO_xx
}TF_VALS;

typedef struct {
	short Cb;			// Grî·e der Struktur in Bytes
	unsigned long Used;		// Bitcodierter Wert fÅr belegten Inhalt
	short Status;			// Wert fÅr RÅckgabewerte
	char Verbindungsname[32];	// ASCII Verbindungsname
	unsigned short Karte;		// 0 fÅr Karte 1
	S5_CONNECT_PARAMS S5;		// AuftragsNo, ..
// Bis hier gleich zu ECHO_CONNECTION
	CONNECT_PARAMS H1;		// TSAP, NSAP, ..     fÅr H1
	IP_CONNECT_PARAMS Ip;		// Ports, Station, .. fÅr IP
	unsigned long Interval;		// z.B. Pollrate, in ms
	FTP_CONNECT_PARAMS Ftp;		// Nummern, ..        fÅr FTP
	TF_VALS Tf;			// Koordinierung zu TF
	char H1Adapter;			// H1 Kartennummer
	char IpAdapter;			// TCP/IP Kartennummer
	char reserved[2];		// Reserviert fÅr Erweiterungen, mu· 0 sein
}CP_VERBINDUNGSDATEN;

typedef struct _ECHO_CONNECT_PARAMS {
	unsigned short Adapter;		// Kartennummer
	unsigned short Used;		// USED_xx. USED_EVENT_FETCH = Slaveverbindung
	CONNECT_PARAMS H1;		// TSAP, NSAP, ..     fÅr H1
	IP_CONNECT_PARAMS Ip;		// Ports, Station, .. fÅr IP
}ECHO_CONNECT_PARAMS;

typedef struct _ECHO_CONNECTION {
	short Cb;			// Grî·e der Struktur in Bytes
	unsigned long Used;		// Bitcodierter Wert fÅr belegten Inhalt
	short Status;			// Wert fÅr RÅckgabewerte
	char Verbindungsname[32];	// ASCII Verbindungsname
	unsigned short Karte;		// 0 fÅr Karte 1. UNBENUTZT bei Echo
	S5_CONNECT_PARAMS S5;		// AuftragsNo, ..
// Bis hier gleich zu CP_VERBINDUNGSDATEN
	ECHO_CONNECT_PARAMS InConn;	// 
	ECHO_CONNECT_PARAMS OutConn;	// 
}ECHO_CONNECTION;

// Werte fÅr CP_STATION.SlowMode
#define CP_SLOWMODE		1
#define CP_SYNCNORESET		2
#define CP_RECBUFFER		4

typedef struct {
	short Cb;			// Grî·e der Struktur in Bytes
	unsigned short Kachelstart;	// Kachel Basisadresse
	unsigned short SlowMode;	// Bitcodierter Wert
	unsigned char Station[6];	// Eigene Stationsadresse Etnernet
	char Stationsname[128];		// Dokumentationsname der Station
	IP_OWN_STATION Ip;		// Parameter bei IP eigene Station
	char Password[32];		// Passwort zum Zugriff
	unsigned short Adapter;		// Adapternummer
	char reserved[38];		// Reserviert fÅr Erweiterungen, mu· 0 sein
}CP_STATION;

typedef struct {
	unsigned short Cb;		// LÑnge der Struktur mit cb
	unsigned short Sekunde;			// 0 - 59
	unsigned short Minute;			// 0 - 59
	unsigned short Stunde;			// 0 - 23
	unsigned short Wochentag;		// 1 - 8
	unsigned short Tag;			// 1 - 31
	unsigned short Monat;			// 1 - 12
	short Jahr;			// Gregorianischer Kalender. Zahlen < 0 -> v. Chr.
}CP_UHRZEIT;

typedef struct {
	unsigned short Cb;		// LÑnge der Struktur mit cb
	short Master;			// 0 -> Slave
	short Typ;			// Multicast oder Broadcast
	short Rate;			// Rate der Sends in s
}CP_UHRSETUP;

#pragma pack()


// Funktionsaufrufe ////////////////////////////////////////////////////////////
EXTERN_C unsigned short WENTRY_C NetLeseStationsparameter(char W_POINTER netfile,CP_STATION W_POINTER cp);
EXTERN_C unsigned short WENTRY_C NetSchreibeStationsparameter(char W_POINTER netfile,CP_STATION W_POINTER cp);
EXTERN_C unsigned short WENTRY_C NetLeseStationsparameter2(char W_POINTER netfile,CP_STATION W_POINTER cp);
EXTERN_C unsigned short WENTRY_C NetSchreibeStationsparameter2(char W_POINTER netfile,CP_STATION W_POINTER cp);
EXTERN_C unsigned short WENTRY_C NetLeseSnmpWerte(char W_POINTER netfile,IP_SNMP W_POINTER snmp);
EXTERN_C unsigned short WENTRY_C NetSchreibeSnmpWerte(char W_POINTER netfile,IP_SNMP W_POINTER snmp);
EXTERN_C unsigned short WENTRY_C NetLeseVerbindung(char W_POINTER netfile,CP_VERBINDUNGSDATEN W_POINTER cp);
EXTERN_C unsigned short WENTRY_C NetSchreibeEchoVerbindung(char W_POINTER netfile,ECHO_CONNECTION W_POINTER ec);
EXTERN_C unsigned short WENTRY_C NetLeseEchoVerbindung(char W_POINTER netfile,ECHO_CONNECTION W_POINTER ec);
EXTERN_C unsigned short WENTRY_C NetSchreibeVerbindung(char W_POINTER netfile,CP_VERBINDUNGSDATEN W_POINTER cp);
EXTERN_C unsigned short WENTRY_C NetLeseAlleEcho(char W_POINTER netfile,ECHO_CONNECTION W_POINTER cp,unsigned short anzahl,unsigned short W_POINTER gelesen);
EXTERN_C unsigned short WENTRY_C NetSchreibeAlleEcho(char W_POINTER netfile,ECHO_CONNECTION W_POINTER cp,unsigned short anzahl);
EXTERN_C unsigned short WENTRY_C NetLeseAlleVerbindungen(char W_POINTER netfile,CP_VERBINDUNGSDATEN W_POINTER cp,unsigned short anzahl,unsigned short W_POINTER gelesen);
EXTERN_C unsigned short WENTRY_C NetHoleAnzahlVerbindungen(char W_POINTER netfile,unsigned short W_POINTER len);
EXTERN_C unsigned short WENTRY_C NetLeseAnrVerbindungen(char W_POINTER netfile,char W_POINTER buffer,unsigned short anzahl,unsigned short W_POINTER gelesen);
EXTERN_C unsigned short WENTRY_C NetSchreibeAlleVerbindungen(char W_POINTER netfile,CP_VERBINDUNGSDATEN W_POINTER cp,unsigned short anzahl);
EXTERN_C unsigned short WENTRY_C NetLeseH1Setup(char W_POINTER netfile,H1_INITVALUES W_POINTER initval);
EXTERN_C unsigned short WENTRY_C NetSchreibeH1Setup(char W_POINTER netfile,H1_INITVALUES W_POINTER initval);
EXTERN_C unsigned short WENTRY_C NetLeseIpSetup(char W_POINTER netfile,IP_STANDARDVALUES W_POINTER initval);
EXTERN_C unsigned short WENTRY_C NetSchreibeIpSetup(char W_POINTER netfile,IP_STANDARDVALUES W_POINTER initval);
EXTERN_C unsigned short WENTRY_C NetLeseEchoComSetup(char W_POINTER netfile,ECHO_COM_SETUP W_POINTER ecs);
EXTERN_C unsigned short WENTRY_C NetSchreibeEchoComSetup(char W_POINTER netfile,ECHO_COM_SETUP W_POINTER ecs);
EXTERN_C int WENTRY_C NetGetH1Connection(char W_POINTER netfile,H1_DIALOG_CONNECTION W_POINTER net);
EXTERN_C int WENTRY_C NetPutH1Connection(char W_POINTER netfile,H1_DIALOG_CONNECTION W_POINTER net);
EXTERN_C int WENTRY_C NetGetIpConnection(char W_POINTER netfile,IP_DIALOG_CONNECTION W_POINTER net);
EXTERN_C int WENTRY_C NetPutIpConnection(char W_POINTER netfile,IP_DIALOG_CONNECTION W_POINTER net);
EXTERN_C int WENTRY_C NetGetIpOsiConnection(char W_POINTER netfile,IP_DIALOG_OSI_CONNECTION W_POINTER net);
EXTERN_C int WENTRY_C NetPutIpOsiConnection(char W_POINTER netfile,IP_DIALOG_OSI_CONNECTION W_POINTER net);
EXTERN_C void WENTRY_C NetGetStandardFileName(char W_POINTER netfile);
EXTERN_C int WENTRY_C NetDeleteConnection(char W_POINTER NetFileName,char W_POINTER verbindungsname);

#endif // CPDEF_H_INTERN
// Ende CPDef.H  /////////////////////////////////////////////////////////
