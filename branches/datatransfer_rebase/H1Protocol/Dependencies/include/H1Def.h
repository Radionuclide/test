// Datei H1Def.h
// Definitionen zum H1 Protokolltreiber Datenstrukturen
// Diese Datei enthÑlt alle Definitionen zum Aufruf in Schicht 4A
// Erstellt von Werner
// Version 1.3
// (C) W.K. W.M.
// énderung am 1.4.1994
// Mail werner.mehrbrodt@inat.de, mehrbrodt@oberberg-online.de or werner.krings@inat.de
// Erweiterung am 14.08.95:	Treiber Standardwerte
//		  30.11.95:	Timeout bei Send und Rec
//		  06.01.96:	Weitere englische Variablennamen
//		  24.12.96:	H1_CONNECTION neu, Namen mit Defines umgesetzt
//		  13.03.97:	H1_DIALOG_CONNECTION neu
//		  10.06.97:	H1_DIALOG_CONNECTION erweitert
//		  18.07.98:	Event Fetch erweitert
// 		  23.03.99:     Englisch. Translations all in english
//		  12.09.2000	Start Credit as Bit 7 from CONNECT_PARAMS.Priority new

#ifndef H1DEF_H_INTERN // Verriegelung
#define H1DEF_H_INTERN

#include <H1Engl.h>	// Alle Variablen und Funktionen in Englisch
// #include <H1Fra.h>	// Alle Variablen und Funktionen in Franzîsisch
// #include <H1Spa.h>	// Alle Variablen und Funktionen in Spanisch
#include <WMKTypes.h>	// Pointer und Aufrufe compilerunabhÑngig
#include <DrvFCall.h>	// Treiber Aufrufcodes

#include <NetCrRequest.h>

// Werte des Elementes ..->Fehler
// The values must not be changed!
#define H1_BAD_CR_PARAMS	1	// CR parameters bad
#define H1_NO_SLOT		2	// Maximun number of connections are running
#define H1_WAIT_CONNECT		3	// Connection doesn't exist
#define H1_NOT_IMPLEMENTED	4	// Function not implemented
#define H1_BAD_LINE		5	// Handle of the connection is not valid
#define H1_WAIT_DATA		6	// No data availible yet
#define H1_WAIT_SEND            7       // Wait for ack the sent data
#define H1_INTERNAL_ERROR	8	// This should happen never
#define H1_NO_REQUEST		9	// Polling a non existing job
#define H1_NO_DRIVER		10	// IfDriverOpen not called, or
					// no TCP/IP driver installed
					// or the network cannto be reached
#define H1_UEBERLAST		11	// Overload of the network or the destination station
#define H1_BLOCKED_DATA		12	// successfully received blocked data
#define H1_NO_ADAPTER		13	// selected adapter is not valid
#define H1_ALREADY_RUNNING	14	// job is already running
#define H1_NOT_SUPPORTED	15	// The function us not supported
#define H1_TRY_AGAIN		16	// Not enough resources temporary. Please call it again later
#define H1_NO_MEMORY		17	// Open Driver not possible under win3.x
#define H1_BAD_SIGNATURE	18	// The Industrial Ethernet Signature not received
// No 19 is reserved

// Verbindungsdaten

#ifndef NORMAL_LINE	// Diese Parameter stehen auch in OS2Ethernet.h

/* Verbindungstypen (ConnectType) */
#define NORMAL_LINE             1
#define DATAGRAMM_LINE          2
#define MULTICAST_LINE          4
#define BROADCAST_LINE          8
#define ALLWAYS_CHECKSUM	0x20
#define PASSIVE_LINE            0x80
#define ACTIVE_LINE             0

/* PrioritÑten (Priority) */
#define EXPRESS_PRIORITY_0		1
#define EXPRESS_PRIORITY_1		2
#define PRIORITY_2			4
#define PRIORITY_3			8
#define PRIORITY_4			16
#define STARTCREDIT			128

#define Handle	Vnr
#define Fehler	Status
#define Data	Daten
#define Adapter	Karte


/* Strukturen */

#pragma pack(1)


#ifndef _CONN_PARAMS_DEFINED
 #define _CONN_PARAMS_DEFINED

typedef struct _CONNECT_PARAMS
{
	unsigned char Priority;		/* PrioritÑt */
        unsigned char ConnectType;	/* Verbindungstyp */
        unsigned char LenDestAddr;	/* LÑnge Zieladresse */
        unsigned char DestAddr[6];	/* Zieladresse */
        unsigned char Multicast;	/* Multicastkreisnummer */
        unsigned char LenNSAP;		/* LÑnge Ziel-NSAP_ID */
	unsigned char NSAP[12];		/* Ziel NSAP-ID */
        unsigned char LenDestTSAP;	/* LÑnge Ziel TSAP_ID */
        unsigned char DestTSAP[16];	/* Ziel TSAP-ID */
        unsigned char LenOwnTSAP;	/* LÑnge eigener TSAP_ID */
        unsigned char OwnTSAP[16];	/* eigener TSAP-ID */
	unsigned char LenConnParams;	/* Anzahl der zusÑtzlichen Connect Daten */
	unsigned char ConnParams[16];	/* ZusÑtzliche Connect Daten */
}CONNECT_PARAMS;

#endif // _CONN_PARAMS_DEFINED

typedef struct {			// fÅr Dialoge
	unsigned short  Cb;             // Anzahl Bytes der Struktur
	unsigned short Adapter;		// Kartennummer
	unsigned long Used;		// Zustandsflag
	char ConnectionName[32];	// Name der Verbindung
	CONNECT_PARAMS CrParams;        // Verbindungsdaten
	unsigned long Interval;		// Pollintervall in ms
	unsigned long ModHandle;	// Module Handle for Dialogs
	unsigned short Supported;	// Flags for supported funtions
	unsigned short BlockLen;	// for OPC Servers: Frame Request Optimation Size (if Supported & 2)
	char res[12];
}H1_DIALOG_CONNECTION;

#endif // NORMAL_LINE

typedef struct _H1_INITVALUES {
	unsigned short Cb;			// Grî·e dieser Struktur in Bytes inclusive cb
	unsigned short TimeoutAck;		// Nach dieser Zeit erfolgt ein Ack
	unsigned short TimeoutCrSchnell;	// Nach dieser Zeit erfolgt ein neues CR Schnell
	unsigned short TimeoutCrLangsam;	// Nach dieser Zeit erfolgt ein neues CR langsam
	unsigned short TimeoutSend;		// Nach dieser Zeit gilt: Kann Daten nicht senden
	unsigned short TimeoutRec;		// Nach dieser Zeit gilt: Keine Daten gelesen
	unsigned short TimeoutLive;		// Nach dieser Zeit gilt die Verbindung als tot
	unsigned short TimeoutRetrySend;	// Nach dieser Zeit gilt die Verbindung als tot
	unsigned short TimeoutNewSend;		// Nach dieser Zeit wird erneut gesendet wenn kein Confirm
	unsigned short NoCrKurz;		// So viele CR's erfolgen schnell, danach langsam
	unsigned short NoRetrySend;		// Anzahl der wiederholungen bei Send ohne Antwort
	unsigned short MaxCredit;		// Maximaler Wert fÅr Credit
	unsigned short TPDUSize;		// Maximaler Wert fÅr die DatenlÑnge, im H1 Format
	unsigned short ClassOptions;		// Standard oder Extended Mode
	unsigned short ProtOption;		// Checksum, Expediated Data Transfer
	unsigned long TimeoutWait;		// Timeout bei Send Rec mit Semaphore. -1 = warte immer
	unsigned char res[12];			// mu· 0 sein
}H1_INITVALUES;

typedef struct H1_LINEVAL {
	short Cb;				// Grî·e dieser Struktur in Bytes inclusive cb
	unsigned short Vnr;			// Verbindungsnummer
	unsigned short Fehler;			// Fehlercode beim Bearbeiten
	short MaxFrameLen;			// Maximale TelegrammlÑnge
	short OwnCredit;			// Eigenes Credit
	short DestCredit;			// Credit der Gegenstation
	unsigned short ClassOptions;		// Standard oder Extended Mode
	unsigned short ProtOption;		// Checksum, Expediated Data Transfer
	unsigned long TimeoutErr;		// Zeit bis Verbindungsfehler
	unsigned short BytesWaitSending;	// Soviele Daten warten darauf, gesendet zu werden
	unsigned short BytesWaitReceiving;	// Soviele Daten warten darauf, abgeholt zu werden
}H1_LINEVAL;

typedef struct {
	CONNECT_PARAMS CrParams;	// Verbindungsdaten
	unsigned short Vnr;		// Verbindungsnummer
	unsigned short Fehler;		// Fehlercode beim Aufbauen
}H1_CONNECT_PARAMS;

#define H1_CONNECTION H1_CONNECT_PARAMS

typedef struct {
	CONNECT_PARAMS CrParams;	// Verbindungsdaten
	unsigned short Vnr;		// Verbindungsnummer
	unsigned short Fehler;		// Fehlercode beim Aufbauen
	unsigned short Karte;		// 0 = Karte 1
	long Timeout;			// Timeout fÅr Send und Rec. -1 = Warte bis Daten da sind
}H1_CONNECT_PARAMS_LINE;

typedef struct {
	unsigned short Vnr;		// Verbindungsnummer
	unsigned short DataLen;		// Anzahl der Daten
	unsigned short Fehler;		// Fehlercode beim Senden
	volatile unsigned long reserved;// Reserviert fÅr internals
	unsigned char Daten[1];		// Zu sendende Daten
}H1_SENDPARAMS;

typedef struct {
	unsigned short Vnr;		// Verbindungsnummer
	unsigned short DataLen;		// Maximale LÑnge der Daten
	unsigned short RecLen;		// TatsÑchliche LÑnge der gelesenen Daten
	unsigned short Fehler;		// Fehlercode beim Empfangen
	volatile unsigned long reserved;// Reserviert fÅr internals. Mu· 0 sein
	unsigned char Daten[1];		// Speicher fÅr Daten
}H1_RECPARAMS;

typedef struct {
	unsigned short DataLen;         // Anzahl der Daten
	unsigned short Handle;		// Verbindungsnummer
	unsigned short RecLen;		// TatsÑchliche LÑnge der gelesenen Daten
	unsigned short Status;		// Fehlercode beim Senden
	volatile unsigned long reserved;// Reserviert fÅr internals
	unsigned char Data[1];		// Speicher fÅr Daten
}H1_SENDREC;

#pragma pack()

// Funktionsaufrufe
// Aufrufe fÅr C
EXTERN_C unsigned short WENTRY_C H1HoleVersion(unsigned short W_POINTER version);
EXTERN_C unsigned short WENTRY_C H1HoleVersionEx(unsigned long W_POINTER version);
EXTERN_C unsigned short WENTRY_C H1DriverOpen(void);
EXTERN_C unsigned short WENTRY_C H1DriverClose(void);
EXTERN_C unsigned short WENTRY_C H1HoleStationsAdresse(unsigned char W_POINTER adresse);
EXTERN_C unsigned short WENTRY_C H1SetzeStationsAdresse(unsigned char W_POINTER adresse);
EXTERN_C unsigned short WENTRY_C H1HoleStationsAdresseKarte(unsigned char W_POINTER adresse,unsigned short karte);
EXTERN_C unsigned short WENTRY_C H1SetzeStationsAdresseKarte(unsigned char W_POINTER adresse,unsigned short karte);
EXTERN_C unsigned short WENTRY_C H1StarteVerbindung(H1_CONNECT_PARAMS W_POINTER cr);
EXTERN_C unsigned short WENTRY_C H1StarteVerbindungKarte(H1_CONNECT_PARAMS_LINE W_POINTER cr);
EXTERN_C unsigned short WENTRY_C H1StoppeVerbindung(unsigned short handle);
EXTERN_C unsigned short WENTRY_C H1StoppeVerbindungen(void);
EXTERN_C unsigned short WENTRY_C H1SoppeVerbindung(unsigned short handle);
EXTERN_C unsigned short WENTRY_C H1RestartConnection (unsigned short handle);
EXTERN_C unsigned short WENTRY_C H1PassiveCR(NET_PASSIVE_CONNECT W_POINTER cr);
EXTERN_C unsigned short WENTRY_C H1StarteSenden(H1_SENDPARAMS W_POINTER send);
EXTERN_C unsigned short WENTRY_C H1AbfrageSenden(H1_SENDPARAMS W_POINTER send);
EXTERN_C unsigned short WENTRY_C H1SendeDaten(H1_SENDPARAMS W_POINTER send);
EXTERN_C unsigned short WENTRY_C H1SendStart(H1_SENDREC W_POINTER send);
EXTERN_C unsigned short WENTRY_C H1SendPoll(H1_SENDREC W_POINTER send);
EXTERN_C unsigned short WENTRY_C H1Send(H1_SENDREC W_POINTER send);
EXTERN_C unsigned short WENTRY_C H1SendeDatenEx(H1_SENDPARAMS W_POINTER send);
EXTERN_C unsigned short WENTRY_C H1StarteLesen(H1_RECPARAMS W_POINTER rec);
EXTERN_C unsigned short WENTRY_C H1AbfrageLesen(H1_RECPARAMS W_POINTER rec);
EXTERN_C unsigned short WENTRY_C H1LeseDaten(H1_RECPARAMS W_POINTER rec);
EXTERN_C unsigned short WENTRY_C H1RecStart(H1_SENDREC W_POINTER rec);
EXTERN_C unsigned short WENTRY_C H1RecPoll(H1_SENDREC W_POINTER rec);
EXTERN_C unsigned short WENTRY_C H1Rec(H1_SENDREC W_POINTER rec);
EXTERN_C unsigned short WENTRY_C H1StarteLesenEx(H1_RECPARAMS W_POINTER rec);
EXTERN_C unsigned short WENTRY_C H1AbfrageLesenEx(H1_RECPARAMS W_POINTER rec);
EXTERN_C unsigned short WENTRY_C H1LeseDatenEx(H1_RECPARAMS W_POINTER rec);
EXTERN_C unsigned short WENTRY_C H1TesteStatus(H1_RECPARAMS W_POINTER rec);
EXTERN_C unsigned short WENTRY_C H1WaitConnect(H1_RECPARAMS W_POINTER rec);
EXTERN_C unsigned short WENTRY_C H1HoleStandardwerte(H1_INITVALUES W_POINTER init);
EXTERN_C unsigned short WENTRY_C H1SetzeStandardwerte(H1_INITVALUES W_POINTER init);
EXTERN_C unsigned short WENTRY_C H1HoleLeitungsparameter(H1_LINEVAL W_POINTER val);
EXTERN_C unsigned short WENTRY_C H1LeseDebugBuffer(char W_POINTER buffer);
EXTERN_C unsigned short WENTRY_C H1LeseDebugFrame(unsigned char W_POINTER buffer);
EXTERN_C unsigned short WENTRY_C NetSendFrame(unsigned char W_POINTER buffer);
EXTERN_C unsigned short WENTRY_C H1SetROMAddress(unsigned char W_POINTER address);
#if !defined (OS2_FUNCTIONS_INCLUDED) && !defined (NT_FUNCTIONS_INCLUDED)
EXTERN_C unsigned short WENTRY_C H1SetzeVektor(unsigned short vektor);
#endif

#endif // H1DEF_H_INTERN
