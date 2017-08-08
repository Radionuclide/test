// Datei DrvListe.h
// Erstellt von W.K.
// Version 1.0
// Mail werner.mehrbrodt@inat.de, mehrbrodt@oberberg-online.de or werner.krings@inat.de
// vom 09.05.99

#ifndef DRVLISTE_INTERN
 #define DRVLISTE_INTERN

#define MAX_CP_CONNECTIONS	223
#define SEGMENT_ECHO_CONNECTION	30

#pragma pack(1)

typedef struct {			// 
	unsigned char Auftragsnummer;	// Verbindungsnummer
	unsigned char Anzw;		// Anzeigenwort
}VERBINDUNGSZUSTAND;

typedef struct {			// 
	unsigned char No;		// Slotnummer
	unsigned char Auftragsnummer;	// Verbindungsnummer SPS
	unsigned short Handle;		// Verbindungsnummer Netz
	unsigned char Flags;		// Bit 0 Doppel, 1 Masterdoppel, 2 IP, 3 H1
}MAIN_VERBINDUNGSZUSTAND;

typedef struct {			// 
	unsigned short Anzahl;		// Anzahl EintrÑge im Pool
	unsigned short Belegt;		// Anzahl belegter EintrÑge im Pool
	unsigned short Highest;		// hîchster belegter im Pool
	unsigned short HighestStart;	// hîchster belegter im Pool seit Start
	unsigned short TestStatus;	// 0 = OK
	char res[12];			// reserve
}MAIN_SPEICHERZUSTAND;

// EC_STATUS->Flags
#define IN_REC_DATA_DA		0x0001
#define OUT_REC_DATA_DA		0x0002
#define IN_SEND_ACTIV		0x0004
#define OUT_SEND_ACTIV		0x0008
#define IN_VERBINDUNG_STEHT	0x0010
#define OUT_VERBINDUNG_STEHT	0x0020
#define IN_REC_ACTIV		0x0040
#define OUT_REC_ACTIV		0x0080
#define IN_RESET_REQUEST	0x0100
#define OUT_RESET_REQUEST	0x0200

// values for EC_STATUS.ErrStatus
#define EC_STATUS_NOT_AVAILIBLE	0x100	//

typedef struct _EC_STATUS {
	char Verbindungsname[34];	// Der Name der Anzeige
	unsigned short ErrStatus;
	short Flag;			// Beschreibung oben
	unsigned long InSendTelegramme;	//
	unsigned long InRecTelegramme;	//
	unsigned long OutSendTelegramme;	//
	unsigned long OutRecTelegramme;	//
	unsigned short InDisconnects;	// Sooft ist die Verbindung beendet
	unsigned short OutDisconnects;	// Sooft ist die Verbindung beendet
	unsigned short TypIn;		// The lower USED flag
	unsigned short TypOut;		// The lower USED flag
	char reserved[16];		//
}EC_STATUS;

typedef struct {			// 
	unsigned short Vnr;		// Verbindungsnummer
	unsigned short SourceReference;	// Eigene Referenz fÅr Ethernet
	unsigned short DestReference;	// Des Gegners Referenz fÅr Ethernet
	unsigned short OwnSequNummer;	// Meine Laufende Sequenznummer
	unsigned short DestSequNummer;	// Des Gegners Laufende Sequenznummer
}H1_VERBINDUNGSZUSTAND;

typedef struct {			// 
	unsigned short Handle;		// Verbindungsnummer
	unsigned short OwnPort	;	// Eigener Port
	unsigned short DestPort;	// Des Gegners Port
	unsigned short OwnSeq;		// Meine Laufende Sequenznummer
	unsigned short DestSeq;		// Des Gegners Laufende Sequenznummer
	unsigned long DestIp;		// 
	unsigned short State;		// der State machine
}IP_VERBINDUNGSZUSTAND;

typedef struct {			// 
	unsigned short Anzahl;		// Anzahl der aktiven Verbindungen
	VERBINDUNGSZUSTAND Vz[MAX_CP_CONNECTIONS];// Max 223 Anzeigenworte
}ALL_ANZEIGE;

typedef struct {			// 
	unsigned short Anzahl;		// Anzahl der aktiven Verbindungen
	H1_VERBINDUNGSZUSTAND Vz[MAX_CP_CONNECTIONS];	// Max 223 Anzeigenworte
}ALL_H1_ANZEIGE;

typedef struct {			// 
	unsigned short Anzahl;		// Anzahl der aktiven Verbindungen
	IP_VERBINDUNGSZUSTAND Vz[MAX_CP_CONNECTIONS];	// Max 223 Anzeigenworte
}ALL_IP_ANZEIGE;

typedef struct {			// 
	unsigned short Anzahl;		// Anzahl der aktiven Verbindungen
	MAIN_VERBINDUNGSZUSTAND Vz[MAX_CP_CONNECTIONS];	// Max 223 Anzeigenworte
}ALL_MAIN_ANZEIGE;

typedef struct {			// 
	unsigned short Anzahl;		// Anzahl der aktiven Verbindungen
	MAIN_SPEICHERZUSTAND Vz[6];	// Max 6 verschiedene Speicherpools
}ALL_MAIN_SPEICHERANZEIGE;

typedef struct {			// 
	unsigned short Anzahl;		// Anzahl der zu holenden Verbindungen
	unsigned short Startoffset;	// ab hier holen
//	ECHO_VERBINDUNGSZUSTAND Vz[SEGMENT_ECHO_CONNECTION];
	EC_STATUS Vz[SEGMENT_ECHO_CONNECTION];
}SEG_ECHO_ANZEIGE;

#pragma pack()

EXTERN_C unsigned short WENTRY_C H1ListeEingetrageneVerbindungen(ALL_H1_ANZEIGE W_POINTER array);
EXTERN_C unsigned short WENTRY_C IpListeEingetrageneVerbindungen(ALL_IP_ANZEIGE W_POINTER array);

#endif // DRVLISTE_INTERN
