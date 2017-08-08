// Datei S5Access.h
// Erstellt von Werner Krings, Werner Mehrbrodt
// Mail werner.mehrbrodt@inat.de, mehrbrodt@oberberg-online.de or werner.krings@inat.de
// Version 1.1 vom 3.5.94
// Version 1.2 vom 1.4.95
// Version 1.3 vom 7.9.95
// Version 2.0 vom 24.12.96	Deklarationen verbessert
// Version 2.1 vom 26.07.97	S5_DATA_ERR neu
// Version 2.2 vom 03.05.98	Alle Werte nun unsigned
// Version 2.3 vom 19.07.98	SINEC_AP erweitert fÅr Eventfunktionen
// Version 2.4 vom 18.11.98	S7 Kennungen
// Version 2.5 vom 10.03.99	Echo Connections
// Version 2.6 of 19.08.1999	Tolerance at event functions
// Version 2.7 of 2.02.2000	Request Block new
// Version 2.8 of 28.04.2000	Scattered Read new

#ifndef S5ACCESS_INTERN	// Verriegelung
#define S5ACCESS_INTERN

#if defined (WINDOWS_FUNCTIONS_INCLUDED)	// Nur Win 3.x
 #define SPEZIALFALL_AP_IM_INT
#endif

#include <H1Def.h>
#include <IpDef.h>
#include <SerDef.h>

#define S5_DATA_ERR		19	// Datenfehler bei Fetch

// Auftragsarten
#define TYP_SEND_DIREKT		1	// Send Direkt
#define TYP_REC_DIREKT		2	// Rec Direkt
#define TYP_SEND_ALL		3	// Send ALL
#define TYP_REC_ALL		4	// Read ALL
#define TYP_SEND_FETCH		5	// Fetch Send (Write)
#define TYP_REC_FETCH		6	// Fetch Read
#define TYP_EVENT_FETCH_SINGLE	7	// Fetch Read Eventbetrieb Single return
#define TYP_EVENT_FETCH_CONT	8	// Fetch Read Eventbetrieb Continous return
#define TYP_TF			9	//
#define TYP_ALL			10	// all jobs with a protocol, all allowed
#define TYP_ALL_NO_PASS_REC	11	// all jobs with a protocol, no passive receive
#define TYP_DIREKT		12	// all direct jobs without a protocol

// Werte fÅr Kennung und OrgKennung	Bit 0 - 6 erlaubte Werte. Bit 7 -> Folgekennung, weiterer Auftrag dahinter
#define KENNUNG_BAUSTEIN		1
#define KENNUNG_MERKER			2
#define KENNUNG_EINGANG			3
#define KENNUNG_AUSGANG			4
#define KENNUNG_PERIPHERIE		5
#define KENNUNG_ZAEHLER			6
#define KENNUNG_TIMER			7
#define KENNUNG_SYSTEMDATEN		8
#define KENNUNG_ABSOLUT			9
#define KENNUNG_ERW_BAUSTEIN		10
#define KENNUNG_EXTMEM			16
#define KENNUNG_EXT_PERIPHERIE		17
#define KENNUNG_S7_BIT_BAUSTEIN		20
#define KENNUNG_S7_BIT_MERKER		21
#define KENNUNG_S7_BIT_EINGANG		22
#define KENNUNG_S7_BIT_AUSGANG		23
#define KENNUNG_S7_BIT_ERW_BAUSTEIN	24
#define KENNUNG_S7_8BIT			28
#define KENNUNG_S7_16BIT		29
#define KENNUNG_S7_32BIT		30
#define KENNUNG_STRING			31
#define KENNUNG_BOOL			32

#define KENNUNG_FOLLOW			0x80

// Values for Tolerance.Tolerance
#define TOLERANCE_NONE		0
#define TOLERANCE_BYTE		0x100
#define TOLERANCE_WORD		0x200
#define TOLERANCE_DWORD		0x300
#define TOLERANCE_KG		0x400
#define TOLERANCE_CHAR		0x500
#define TOLERANCE_SHORT		0x600
#define TOLERANCE_LONG		0x700

// Values for .Fehler
#define PLC_ERR_NO_ERROR		0
#define PLC_ERR_NO_BLOCK		2
#define PLC_ERR_BLOCK_TOO_SHORT		3
#define PLC_ERR_TIMEOUT			4
#define PLC_ERR_NO_HARDWARE		7

// Opcodes in AP Header
#define AP_OPCODE_WRITE			3	// Write Anfrage
#define AP_OPCODE_WRITE_ACK		4	// Write Antwort
#define AP_OPCODE_READ			5	// Fetch Anfrage
#define AP_OPCODE_READ_ACK		6	// Fetch Antwort
#define AP_OPCODE_V			7	// Konkurrenz
#define AP_OPCODE_REQUEST_WRITE		13	// Write Request Anfrage
#define AP_OPCODE_REQUEST_READ		15	// Fetch Request Anfrage
#define AP_OPCODE_EVENT_START		19	// 0x13 Eventfetch Init
#define AP_OPCODE_EVENT_STOP_ONE	20	// 0x14 Eventfetch Stop eine Anfrage
#define AP_OPCODE_EVENT_STOP_ALL	21	// 0x15 Eventfetch Stop alle Anfragen
#define AP_OPCODE_EVENT_STOP_ONE_ACK	22	// 0x16 Eventfetch Antwort Stop einen
#define AP_OPCODE_EVENT_STOP_ALL_ACK	23	// 0x17 Eventfetch Antwort stop alle
#define AP_OPCODE_EVENT_ACK		24	// 0x18 Eventfetch Antwort
#define AP_OPCODE_EVENT_START_DDE	25	// 0x19 Eventfetch Init Single for one block

#pragma pack(1)

typedef struct {
short s5;			// "S5"
unsigned char Headerlen;	// 0x10
unsigned char KennungOpcode;	// 1
unsigned char LenKennungOpcode;	// 3
unsigned char Opcode;		// AP_OPCODE_xxx
union {
	struct {
		unsigned short Kennung;		// KENNUNG_REQUEST_xxx
		unsigned short DB;		// Bausteinnummer
		unsigned short DW;		// Start DW
		unsigned short Len;		// Anzahl Elemente
		unsigned char Leerblock;	// ff
		unsigned char LenLeerblock;	// 2
		}Request;				// AP_OPCODE_WRITE AP_OPCODE_READ
	struct {
		unsigned char OrgBlock;		// Send Rec: 3
		unsigned char LenOrgblock;	// SendRec: 8
		unsigned char OrgKennung;	// KENNUNG_xxx
		unsigned char DB;		// Bausteinnummer
		unsigned short DW;		// Start DW
		unsigned short Len;		// Anzahl Elemente
		unsigned char Leerblock;	// ff
		unsigned char LenLeerblock;	// 2
		}Start;				// AP_OPCODE_WRITE AP_OPCODE_READ
	struct {
		unsigned char Quitblock;	// f
		unsigned char LenQBlock;	// 3
		unsigned char Fehler;		// 0 = Kein Fehler
		unsigned char Leerblock;	// ff
		unsigned char LenLeerblock;	// 7
		unsigned char Dummy1;
		unsigned char Dummy2;
		unsigned char Dummy3;		// manchmal 64
		unsigned char Dummy4;		// manchmal ff
		unsigned char Dummy5;		// manchmal 2
		}Quittung;
	struct {
		unsigned char Quitblock;	// f
		unsigned char LenQBlock;	// 8
		unsigned char Fehler;		// 0 = Kein Fehler
		unsigned char OrgKennung;	// KENNUNG_xxx
		unsigned char DB;		// Bausteinnummer
		unsigned short DW;		// Start DW
		unsigned short Len;		// Anzahl Elemente
		unsigned char Count;		// wird bei jeder Antwort incrementiert
		}EventQuittung;
	struct {
		unsigned char PgBlock;		// 3
		unsigned char LenPgBlock;	// 6
		unsigned char Startblock;	// 2 = noch weitere, 1 = Letzter
		unsigned char Folgeblock;	// 2 = 1. Block, 0 = Folgeblock
		unsigned char Opcode;		// AS511 Opcode
		unsigned char TermCode;		// AS511 Termcode
		unsigned char t3;		// 4
		unsigned char t4;		// 2 oder 4
		unsigned char rt1;		//
		unsigned char rt2;		//
		}Pg;
	}p;
}SIENEC_AP;

#define SINEC_AP	SIENEC_AP

typedef struct _S5_TOLERANCE {
	unsigned short ToleranceTyp;	// 0 = unused
	unsigned long TolerancePlus;	// positive tolerance
	unsigned long ToleranceMinus;	// negatige tolerance
}S5_TOLERANCE;

typedef struct _S5_PARAMS {
	unsigned short Verbindungsnummer;
	unsigned short Kennung;		// Kennungen fÅr Parameter
	unsigned short DB;		// Bausteinnummer
	unsigned short DW;		// Datenwortnummer
	unsigned short Len;		// Anzahl der Daten
}S5_PARAMS;

typedef struct _PLC_REQ {
	unsigned short Kennung;		// Kennungen fÅr Parameter
	unsigned short DB;		// Bausteinnummer
	unsigned short DW;		// Datenwortnummer
	unsigned short Len;		// Anzahl der Daten
}PLC_REQ;
// Answer ist an array with short len, short error, data, len,error,data, .. 0

typedef struct {			// Parameters for internal handling all plc types
	unsigned short QuellTyp;	// Source. all as unsigned short
	unsigned short QuellDB;		// Source 
	unsigned short QuellDW;		// Source 
	unsigned short QuellLen;	// Source 
	unsigned short ZielTyp;		// Destination
	unsigned short ZielDB;		// Destination
	unsigned short ZielDW;		// Destination
	unsigned short ZielLen;		// Destination
}FETCH_PARAMETER;

typedef struct {
	unsigned short Auftragsnummer;	// Nummer.
	unsigned short Offset;		// Offset zur Basiskachel
	unsigned short Auftragsart;	// defines aus Auftragsarten
	unsigned short Benutzt;		// Verbindung wird beim Start automatisch benutzt
}S5_ANSCHALTUNG;

typedef struct _S5_VERBINDUNGSDATEN {
	char Verbindungsname[32];	// ASCII Verbindungsname
	S5_ANSCHALTUNG S5Params;	// AuftragsNo, ..
	CONNECT_PARAMS CrParams;	// TSAP, NSAP, ..
}S5_VERBINDUNGSDATEN;

typedef struct _S5_VERBINDUNGSDATEN_KARTE {
	char Verbindungsname[32];	// ASCII Verbindungsname
	S5_ANSCHALTUNG S5Params;	// AuftragsNo, ..
	CONNECT_PARAMS CrParams;	// TSAP, NSAP, ..
	unsigned short Karte;		// 0 = Karte 1
}S5_VERBINDUNGSDATEN_KARTE;

typedef struct _CP_REVISION {
	unsigned short cb;		// LÑnge der Struktur in Bytes
	unsigned short SerNo;		// Seriennummer der CP
	unsigned short KachelNo;	// Nummer der eingestellten Kachel
	unsigned short KachelRev;	// Revision des Kacheltreibers
	unsigned short H1Rev;		// Revision des H1 Treibers
	unsigned short IpRev;		// Revision der TCP/IP Treibers
	unsigned short TermRev;		// Revision des Terminals
	unsigned char StationAdr[6];	// Ethernet ROM Stationsadresse
}CP_REVISION;

typedef struct _PG_CONNECTION {
   unsigned short cb;			// LÑnge der Struktur in Bytes
   unsigned short Adapter;		// Kartennummer
   unsigned long  Used;			// Bitcodierter Wert fÅr belegten Inhalt
   unsigned short Handle;          	// Handle of the connection
   unsigned short Status;          	// Status of the connection
   unsigned long  DestIpAddr;      	// IP destination address
   unsigned char  DestIpName [IP_NAME_LEN]; // IP destination address for DNS
   unsigned short Port;                	// Port
   unsigned char  DestAddr[6];		/* Zieladresse */
   unsigned char  LenTSAP;		/* L‰nge TSAP_ID*/
   unsigned char  TSAP[16];		/* TSAP-ID, eigener und fremder ist gleich */
   unsigned short Line;			// 0 = COM1
}PG_CONNECTION;


typedef struct _LISTE_TYP {
	char Null;			// null of name
	unsigned long Used;		// USED_xx
	unsigned long res;		// reserved
	unsigned short Anr;		//
}LISTE_TYP;

#pragma pack()

// Prototypen: Zugriffsfunktionen auf die .DLL oder .LIB
// Echo access
EXTERN_C unsigned short WENTRY_C EcStarteVerbindung(PG_CONNECTION W_POINTER cr);
EXTERN_C unsigned short WENTRY_C EcLeseAusSPS(S5_PARAMS W_POINTER s5,unsigned short memorylen,void W_POINTER memory,unsigned short W_POINTER s5error);
EXTERN_C unsigned short WENTRY_C EcStartLesen(S5_PARAMS W_POINTER s5);
EXTERN_C unsigned short WENTRY_C EcAbfrageLesen(unsigned short memorylen,void W_POINTER memory,unsigned short W_POINTER s5error,unsigned short conn_no);
EXTERN_C unsigned short WENTRY_C EcSchreibeInSPS(S5_PARAMS W_POINTER s5,void W_POINTER data,unsigned short W_POINTER s5error);
EXTERN_C unsigned short WENTRY_C EcStartSchreiben(S5_PARAMS W_POINTER s5,void W_POINTER data);
EXTERN_C unsigned short WENTRY_C EcAbfrageSchreiben(unsigned short W_POINTER s5error,unsigned short conn_no);
EXTERN_C unsigned short WENTRY_C EcStoppeVerbindung(unsigned short conn_no);
EXTERN_C unsigned short WENTRY_C EcStoppeVerbindungen(void);
EXTERN_C unsigned short WENTRY_C EcTesteStatus(unsigned short conn_no);

EXTERN_C unsigned short WENTRY_C S5SetzeStationsAdresse(unsigned char W_POINTER adresse);
EXTERN_C unsigned short WENTRY_C S5SetzeStationsAdresseKarte(unsigned char W_POINTER adresse,unsigned short karte);
EXTERN_C unsigned short WENTRY_C S5StarteVerbindung(H1_CONNECT_PARAMS W_POINTER cr);
EXTERN_C unsigned short WENTRY_C S5StarteVerbindungKarte(H1_CONNECT_PARAMS_LINE W_POINTER cr);
EXTERN_C unsigned short WENTRY_C S5StarteVerbindungIp(IP_CONNECTION W_POINTER cr);
EXTERN_C unsigned short WENTRY_C S5StarteVerbindungIpOsi(IP_OSI_CONNECTION W_POINTER cr);
EXTERN_C unsigned short WENTRY_C S5RestartVerbindung(unsigned short conn_no);
EXTERN_C unsigned short WENTRY_C S5StoppeVerbindung(unsigned short conn_no);
EXTERN_C unsigned short WENTRY_C S5StoppeVerbindungen(void);
EXTERN_C unsigned short WENTRY_C S5LeseAusSPS(S5_PARAMS W_POINTER s5,unsigned short memorylen,void W_POINTER memory,unsigned short W_POINTER s5error);
EXTERN_C unsigned short WENTRY_C S5StartLesen(S5_PARAMS W_POINTER s5);
EXTERN_C unsigned short WENTRY_C S5AbfrageLesen(unsigned short memorylen,void W_POINTER memory,unsigned short W_POINTER s5error,unsigned short conn_no);
EXTERN_C unsigned short WENTRY_C PlcStartLesenMulti(unsigned short conn_no,unsigned short no,PLC_REQ W_POINTER req);
EXTERN_C unsigned short WENTRY_C PlcAbfrageLesenMulti(unsigned short conn_no,unsigned short memorylen,void W_POINTER memory,unsigned short W_POINTER s5error);

// S7
EXTERN_C unsigned short WENTRY_C S7StarteVerbindungH1(H1_CONNECT_PARAMS_LINE W_POINTER cr,void W_POINTER p);
EXTERN_C unsigned short WENTRY_C S7StarteVerbindungIp(IP_CONNECTION W_POINTER cr,void W_POINTER p);
EXTERN_C unsigned short WENTRY_C S7StarteVerbindungIpOsi(IP_OSI_CONNECTION W_POINTER cr,void W_POINTER p);
#define S7StoppeVerbindung(a) S5StoppeVerbindung(a)

// Modbus
EXTERN_C unsigned short WENTRY_C ModbusStarteVerbindungIp(IP_CONNECTION W_POINTER cr);

// Serial
EXTERN_C unsigned short WENTRY_C S7StarteVerbindungSerial(COMM_LINE W_POINTER cl,unsigned short W_POINTER handle); // 0 = COM1

// Event Funktionen
EXTERN_C unsigned short WENTRY_C S5InitEvent(S5_PARAMS W_POINTER s5);
EXTERN_C unsigned short WENTRY_C S5InitEventDde(S5_PARAMS W_POINTER s5);
EXTERN_C unsigned short WENTRY_C S5InitEventStart(S5_PARAMS W_POINTER s5);
EXTERN_C unsigned short WENTRY_C S5InitEventStartDde(S5_PARAMS W_POINTER s5);
EXTERN_C unsigned short WENTRY_C S5InitEventPoll(unsigned short conn_no);
EXTERN_C unsigned short WENTRY_C S5TerminateEvent(S5_PARAMS W_POINTER s5);
EXTERN_C unsigned short WENTRY_C S5InitEventDde(S5_PARAMS W_POINTER s5);
EXTERN_C unsigned short WENTRY_C S5InitEventStartTol(S5_PARAMS W_POINTER s5,S5_TOLERANCE W_POINTER tol);
EXTERN_C unsigned short WENTRY_C S5InitEventStartDdeTol(S5_PARAMS W_POINTER s5,S5_TOLERANCE W_POINTER tol);
EXTERN_C unsigned short WENTRY_C S5TerminateEventStart(S5_PARAMS W_POINTER s5);
EXTERN_C unsigned short WENTRY_C S5TerminateEventPoll(unsigned short conn_no);
EXTERN_C unsigned short WENTRY_C S5EventRead(unsigned short memorylen,void W_POINTER memory,unsigned short W_POINTER s5error,S5_PARAMS W_POINTER s5);
EXTERN_C unsigned short WENTRY_C S5StartEventRead(unsigned short memorylen,void W_POINTER memory,unsigned short W_POINTER s5error,S5_PARAMS W_POINTER s5,unsigned short W_POINTER opcode);
EXTERN_C unsigned short WENTRY_C S5PollEventRead(unsigned short memorylen,void W_POINTER memory,unsigned short W_POINTER s5error,S5_PARAMS W_POINTER s5,unsigned short W_POINTER opcode);
EXTERN_C unsigned short WENTRY_C S5StartEventLifeAck(S5_PARAMS W_POINTER s5);
EXTERN_C unsigned short WENTRY_C S5PollEventLifeAck(unsigned short handle);

EXTERN_C unsigned short WENTRY_C S5StartReadS5Header(unsigned short conn_no,unsigned short speicherlen,void W_POINTER speicher,unsigned short W_POINTER retlen);
EXTERN_C unsigned short WENTRY_C S5PollReadS5Header(unsigned short conn_no,unsigned short speicherlen,void W_POINTER speicher,unsigned short W_POINTER retlen);
EXTERN_C unsigned short WENTRY_C S5StartWriteS5Header(unsigned short conn_no,unsigned short speicherlen,void W_POINTER speicher);
EXTERN_C unsigned short WENTRY_C S5PollWriteS5Header(unsigned short conn_no);

EXTERN_C unsigned short WENTRY_C S5SchreibeInSPS(S5_PARAMS W_POINTER s5,void W_POINTER data,unsigned short W_POINTER s5error);
EXTERN_C unsigned short WENTRY_C S5StartSchreiben(S5_PARAMS W_POINTER s5,void W_POINTER data);
EXTERN_C unsigned short WENTRY_C S5AbfrageSchreiben(unsigned short W_POINTER s5error,unsigned short conn_no);
EXTERN_C unsigned short WENTRY_C PlcStartSchreibenMulti(unsigned short conn_no,unsigned short no,PLC_REQ W_POINTER req,unsigned char W_POINTER data);
EXTERN_C unsigned short WENTRY_C PlcAbfrageSchreibenMulti(unsigned short conn_no,unsigned short memlen,unsigned W_POINTER error);

EXTERN_C unsigned short WENTRY_C S5FetchPassiv(unsigned short conn_no,short(WENTRY_C *RetCall)(short DB,short DW,short Typ,short Len,short W_POINTER S5Err,void W_POINTER W_POINTER data),void W_POINTER W_POINTER retptr);
EXTERN_C unsigned short WENTRY_C S5StartFetchPassiv(unsigned short conn_no);
EXTERN_C unsigned short WENTRY_C S5AbfrageFetchPassiv(unsigned short conn_no,short(WENTRY_C *RetCall)(short DB,short DW,short Typ,short Len,short W_POINTER S5Err,void W_POINTER W_POINTER data),void W_POINTER W_POINTER retptr);
EXTERN_C unsigned short WENTRY_C S5WritePassiv(unsigned short conn_no,short(WENTRY_C *RetCall)(short DB,short DW,short Typ,short Len,short W_POINTER Err,void W_POINTER W_POINTER data),void W_POINTER W_POINTER retptr);
EXTERN_C unsigned short WENTRY_C S5StartWritePassiv(unsigned short conn_no);
EXTERN_C unsigned short WENTRY_C S5AbfrageWritePassiv(unsigned short conn_no,short(WENTRY_C *RetCall)(short DB,short DW,short Typ,short Len,short W_POINTER Err,void W_POINTER W_POINTER data),void W_POINTER W_POINTER retptr);
EXTERN_C unsigned short WENTRY_C S5TesteStatus(unsigned short conn_no);
#if !defined(OS2_FUNCTIONS_INCLUDED) | !defined(WIN95_FUNCTIONS_INCLUDED) | !defined(NT_FUNCTIONS_INCLUDED)
EXTERN_C unsigned short WENTRY_C S5SetzeVektor(unsigned short vektor);
EXTERN_C unsigned short WENTRY_C S5SetzeVektorIp(unsigned short vektor);
#endif

// S5 INI Datei Zugriffsfunktionen
EXTERN_C unsigned short WENTRY_C S5HoleVerbindungsparameter(S5_VERBINDUNGSDATEN W_POINTER s5data);
EXTERN_C unsigned short WENTRY_C S5SchreibeVerbindungsparameter(S5_VERBINDUNGSDATEN W_POINTER s5data);

EXTERN_C unsigned short WENTRY_C S5EntferneVerbindung(char W_POINTER verbindungsname);
EXTERN_C unsigned short WENTRY_C S5ListeVerbindungen(unsigned short len,char W_POINTER mem);

EXTERN_C unsigned short WENTRY_C S5ListeNetVerbindungen(char W_POINTER filename,unsigned short len,char W_POINTER mem);
EXTERN_C unsigned short WENTRY_C SchreibeS5Anschaltung(char W_POINTER netfile,char W_POINTER vname,S5_ANSCHALTUNG W_POINTER s5A);
EXTERN_C unsigned short WENTRY_C LeseS5Anschaltung(char W_POINTER netfile,char W_POINTER vname,S5_ANSCHALTUNG W_POINTER s5A);
EXTERN_C unsigned short WENTRY_C SortiereAnschaltEin(char W_POINTER buffer,S5_ANSCHALTUNG W_POINTER s5);
EXTERN_C char W_POINTER WENTRY_C BildeAuftragsNamen(unsigned short auftragsart);

EXTERN_C unsigned short WENTRY_C H1LeseParameter(char W_POINTER netfile,char W_POINTER vname,H1_CONNECT_PARAMS_LINE W_POINTER cr);
EXTERN_C unsigned short WENTRY_C S5LeseParameter(char W_POINTER netfile,char W_POINTER vname,S5_ANSCHALTUNG W_POINTER s5);
EXTERN_C unsigned short WENTRY_C H1SchreibeParameter(char W_POINTER netfile,char W_POINTER vname,H1_CONNECT_PARAMS_LINE W_POINTER cr);
EXTERN_C unsigned short WENTRY_C S5SchreibeParameter(char W_POINTER netfile,char W_POINTER vname,S5_ANSCHALTUNG W_POINTER s5);
EXTERN_C unsigned short WENTRY_C LeseParameter(char W_POINTER netfile,char W_POINTER vname,H1_CONNECT_PARAMS_LINE W_POINTER cr,S5_ANSCHALTUNG W_POINTER s5);
EXTERN_C unsigned short WENTRY_C SchreibeParameter(char W_POINTER netfile,char W_POINTER vname,H1_CONNECT_PARAMS_LINE W_POINTER cr,S5_ANSCHALTUNG W_POINTER s5);
EXTERN_C unsigned short WENTRY_C S5HoleRevision(CP_REVISION W_POINTER cprev);

EXTERN_C unsigned short WENTRY_C S5HoleVerbindungsparamsKarte(S5_VERBINDUNGSDATEN_KARTE W_POINTER s5data);
EXTERN_C unsigned short WENTRY_C S5SchreibeVerbindungKarte(S5_VERBINDUNGSDATEN_KARTE W_POINTER s5data);

EXTERN_C unsigned short WENTRY_C S5SetzeNetDateiname(char W_POINTER Dateiname);


#endif // S5ACCESS_INTERN Verriegelung
