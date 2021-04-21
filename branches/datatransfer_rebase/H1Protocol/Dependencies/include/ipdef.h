// Datei IpDef.h
// Definitions to the TCP/IP Interface and Declarations
// Erstellt  W. Mehrbrodt
// Modified  W. Krings
// Mail werner.mehrbrodt@inat.de, mehrbrodt@oberberg-online.de or werner.krings@inat.de

// History ///////////////////////////////////////////////////////////////
// V 1.0  06.08.95 W.M.	Erstellt
// V 1.1  24.11.95 W.K.	Um Stationsparameter erweitert
// V2.0	  27.12.96 WK
// V2.1	  18.01.97 WK, WM
// V2.2	  15.02.97 WK, WM CRF_xx neu
// V2.3	  13.03.97 WK, WK IP_DIALOG_CONNECTION neu
// V2.4	  16.05.97 WK, WM IP_BAD_SIGNATURE neu
//	  10.06.97:WK, WM IP_DIALOG_CONNECTION erweitert
// V2.5   26.07.97:WK     IP_WMK_HEADER samt Flags dazu neu
// V2.6   20.06.98:WK     TimeoutLife in CrParams neu
// V2.7   23.06.98:WK     Selektives Verbindungstimeout
// V2.8   23.07.98:WK     Selektives Speichermanagement. Der DNS Name ist nun kÅrzer
// V2.9   14.10.98:AB, RB Englisch. Translations all in english
// V2.10  29.10.98:WK	  FTP_SET new
// V2.11  06.01.99:WK	  OSI functions new
// V2.12  12.01.99:W.K.   Number of Adapter in station parameters new
// V2.13  05.02.99:W.K.   Signature optionnaly in big endian format
// V2.14  08.12.99:W.K.   Option TCP automaticaly port equal dest port
// V2.15  19.10.2000 W.K. CRF_USE_FIN and CRF_USE_RESET new

#ifndef IPDEF_H_INTERN			// conditional for recursive calls
 #define IPDEF_H_INTERN

#include <WMKTypes.h>                   // Compiler and operating system dependencies
#include <DrvFCall.h>			// Driver calls

#include <NetCrRequest.h>

// Fehlercodes ///////////////////////////////////////////////////////////
// The values must not be changed!
#define IP_BAD_CR_PARAMS	1	// CR parameters bad
#define IP_NO_SLOT		2	// Maximun number of connections are running
#define IP_WAIT_CONNECT		3	// Connection doesn't exist
#define IP_NOT_IMPLEMENTED	4	// Function not implemented
#define IP_BAD_LINE		5	// Handle of the connection is not valid
#define IP_WAIT_DATA		6	// No data availible yet
#define IP_WAIT_SEND            7       // Wait for ack the sent data
#define IP_INTERNAL_ERROR	8	// This should happen never
#define IP_NO_REQUEST		9	// Polling a non existing job
#define IP_NO_DRIVER		10	// IfDriverOpen not called, or
					// no TCP/IP driver installed
					// or the network cannto be reached
#define IP_UEBERLAST		11	// Overload of the network or the destination station
#define IP_BLOCKED_DATA		12	// successfully received blocked data
#define IP_NO_ADAPTER		13	// selected adapter is not valid
#define IP_ALREADY_RUNNING	14	// job is already running
#define IP_NOT_SUPPORTED	15	// The function us not supported
#define IP_TRY_AGAIN		16	// Not enough resources temporary. Please call it again later
#define IP_NO_MEMORY		17	// Open Driver not possible under win3.x
#define IP_BAD_SIGNATURE	18	// The Industrial Ethernet Signature not received
// No 19 is reserved

#define IP_BUSY			IP_UEBERLAST

// Connection Types (IP_CONNECT_PARAMS.ConnectType)
#define SERVER_TYPE		1	// Passive connection
#define CLIENT_TYPE		2	// Aktive connection
#define TYPE_USED		0x80	// Bitwise added: Parameter used

// Protocol Types   (IP_CONNECT_PARAMS.Protocol)
#define TCP_PROTOCOL		4	// Transmittion of data with confirm
#define UDP_PROTOCOL		8	// Transmittion of data without confirm

#define IP_NAME_LEN             40      // IP-Address as a string name (for DNS)
#define IP_MAX_INIT		3	// Maximum number of DNS or Router possible

// IP_CONNECT_PARAMS.Flags, coded bitwise
#define CRF_USE_LIFE_ACKS	0x0001	// Use of life acks
#define CRF_USE_SIGNATURE	0x0002	// Signature and frame length at the start of the data added
#define CRF_FAST_SEND		0x0004	// Do not wait for the acknowledge after each frame
#define CRF_USE_LIFE_DATAACKS	0x0008	// Use of life data acks
#define CRF_ACK_DATA_WITH_DATA	0x0010	// Give a life data ack as response
#define CRF_NO_ACK_IN_BLOCKS	0x0020	// Do not wait for acks in blocked transmittions
#define CRF_IMMEDIATE_ACK	0x0040	// Give an acknowledge immediately after receiving
#define CRF_SELECTIVE_TIMEOUT	0x0080	// Timeout used selective for this connection
#define CRF_STREAM		0x0100	// Ignore the PUSH flag of TCP/IP
#define CRF_SET_BUFLEN		0x0200	// Selective memory set for this connection
#define CRF_USE_RFC1006		0x0400	// OSI over TCP
#define CRF_UDP_ONE_PORT	0x0800	// UDP has own and destination port the same
#define CRF_TCP_ONE_PORT	0x0800	// TCP has own and destination port the same
#define CRF_SIGLEN_BIG		0x1000	// Datalen element in signature is big endian format
#define CRF_USE_FIN		0x2000	// Close the connection with FIN
#define CRF_USE_RESET		0x4000	// Close the connection with RST
#define CRF_USE_OLD_LIFE_ACKS	0x8000	// Life acks with bad sequence number

// Flags if using the IP_WMK_HEADER (the signature)
#define WMK_BLOCKFLAG		1	// This is a part of a block, but not the last one
#define WMK_BIG_ENDIAN		2	// The length is in motorola format

// FTP_SET.Flags
#define FTP_WRITE_ALLOWED	1	// otherwise Read Only
#define FTP_WRITE_PASSWORD	2	// the Station Password
#define FTP_READ_PASSWORD	4	// the Name of the Connection is used

// Bits for IP_DIALOG_OSI_CONNECTION.Supported. Coded bitwise
#define IP_SUPPORTS_OSI		1
#define IP_SUPPORTS_SEL_TIMEOUT	2
#define IP_SUPPORTS_SEL_MEMORY	4

// Structurs ////////////////////////////////////////////////////////////

#pragma pack(1)

typedef struct _FTP_SET {
	short Cb;			// Number of bytes in the structure including this element
	unsigned char Flags;		// Bit 0 -> Write allowed
	char PasswordRead[32];		// Password for read
	char PasswordWrite[32];		// Password for write
}FTP_SET;

typedef struct _IP_CONNECT_PARAMS {
   unsigned short  Cb;                  // Number of bytes in the structure including this element
   unsigned long   DestIpAddr;          // IP destination address
   unsigned char   DestIpName [IP_NAME_LEN - 2]; // IP destination address for DNS
   unsigned short  MemLen;		// Memory size bei CRF_SET_BUFLEN
   unsigned short  Port;                // Port
   unsigned char   ConnectType;         // SERVER_TYPE, CLIENT_TYPE
   unsigned char   Protocol;            // TCP_PROTOCOL, UDP_PROTOCOL
   unsigned short  Flags;		// CRF_USE_LIFE_ACKS, CRF_USE_SIGNATURE
   unsigned short  TimeoutLife;		// Connection timeout in s. 0 = Default. 10 < s < 3000
} IP_CONNECT_PARAMS;

#define MAXLEN_OSI_TSAP		8

typedef struct _OSI_CONNECT_PARAMS
{	unsigned short  Cb;             // Number of bytes in the structure including this element
        unsigned char LenDestTSAP;	/* Len dest TSAP_ID */
        unsigned char DestTSAP[MAXLEN_OSI_TSAP];	/* Dest TSAP-ID */
        unsigned char LenOwnTSAP;	/* Len own TSAP_ID */
        unsigned char OwnTSAP[MAXLEN_OSI_TSAP];	/* Own TSAP-ID */
	unsigned short Flags;		// .. USED_TWOLINES, ..
	char reserved[2];
}OSI_CONNECT_PARAMS;

typedef struct {			// fÅr Dialoge
	unsigned short  Cb;             // Number of bytes in the structure including this element
	unsigned short Adapter;		// Number of the adapter. 0 = 1st adapter
	unsigned long Used;		// Flag for dialog options, s.o. multiple lines
	char ConnectionName[32];	// Name of the connection
	IP_CONNECT_PARAMS CrParams;     // Parameters of the connection
	unsigned long Interval;		// Poll intervall in ms
        OSI_CONNECT_PARAMS Osi;		// Connection parameters OSI
	unsigned long ModHandle;	// Module Handle for Dialogs
	unsigned short Supported;	// Flags for supported funtions
	unsigned short BlockLen;	// for OPC Servers: Frame Request Optimation Size (if Supported & 2)
	unsigned char res[12];		// must be 0
}IP_DIALOG_OSI_CONNECTION;

typedef struct {			// fÅr Dialoge
	unsigned short Cb;              // Number of bytes in the structure including this element
	unsigned short Adapter;		// Number of the adapter. 0 = 1st adapter
	unsigned long Used;		// Flag for dialog options, s.o. multiple lines
	char ConnectionName[32];	// Name of the connection
	IP_CONNECT_PARAMS CrParams;     // Parameters of the connection
	unsigned long Interval;		// Poll intervall in ms
//	unsigned long ModHandle;	// Module Handle for Dialogs
}IP_DIALOG_CONNECTION;

typedef struct {
   unsigned short  Cb;                  // Number of bytes in the structure including this element
   unsigned short  Adapter;             // 0 = adapter 1
   unsigned short  Handle;              // Handle of the connection
   unsigned short  Status;              // Status of the connection
   IP_CONNECT_PARAMS CrParams;          // Connection parameters
}IP_CONNECTION;

typedef struct {
   unsigned short  Cb;                  // Number of bytes in the structure including this element
   unsigned short  Adapter;             // 0 = adapter 1
   unsigned short  Handle;              // Handle of the connection
   unsigned short  Status;              // Status of the connection
   IP_CONNECT_PARAMS CrParams;          // Connection parameters TCP/IP
   OSI_CONNECT_PARAMS Osi;		// Connection parameters OSI
}IP_OSI_CONNECTION;

typedef struct _IN_SIGNATURE{		// Used if CRF_USE_SIGNATURE is set
	char MK[2];			// ASCII MK
	unsigned short DataLen;		// Number of following data bytes. 0 -> Life Data Ack
	unsigned short Flags;		// the WMK_ Flags. Bit 0 -> Block flag
	unsigned short SeqNo;		// Sequence number. Each frame with data increments this.
}IN_SIGNATURE;

typedef struct {
   unsigned short  Cb;                  // Number of bytes in the structure including this element
   unsigned short  Handle;              // Handle of the connection
   unsigned short  DataLen;             // Real length of sent or received data
   unsigned short  Status;              // Error and status code. IP_xx
   volatile unsigned long   reserved;   // Reserved for internals. Must be 0
   unsigned char   Data[1];             // Memory for the data
}IP_SENDREC;

typedef struct {
	unsigned short Handle;		// Handle of the connection
	unsigned short DataLen;		// Number of data to send
	unsigned short Status;		// Error and status code. IP_xx
	volatile unsigned long reserved;// Reserviert fÅr internals
	unsigned char Daten[1];		// Zu sendende Daten
}IP_SENDPARAMS;

typedef struct {
	unsigned short Handle;		// Handle of the connection
	unsigned short DataLen;		// Length of the buffer for received data
	unsigned short RecLen;		// Real length of received data
	unsigned short Status;		// Error and status code. IP_xx
	volatile unsigned long reserved;// Reserved for internals. Must be 0
	unsigned char Daten[1];		// Memory for the data
}IP_RECPARAMS;

typedef struct {
   unsigned short  Cb;                  	// Number of bytes in the structure including this element
   unsigned long   Station;			// 32 Bit IP station address
   unsigned long   Subnet;			// 32 Bit IP subnet
   unsigned long   DnsAddress[IP_MAX_INIT];	// Domain server addresses
   unsigned long   RouterAddress[IP_MAX_INIT];	// Router server addresses
   char            DomainName  [IP_NAME_LEN];	// Name in the domain
   char            StationName [IP_NAME_LEN];	// Name of the station
   unsigned short  Adapter;			// Number of Network Adapter
   char res[18];				// reserved, must be 0
}IP_OWN_STATION;

		// All timeouts in ms
typedef struct _IP_STANDARDVALUES {
	long Cb;			// Number of bytes in the structure including this element
	long TimeoutWait;		// Max wait for send and receive jobs if blocking functions used
	long TimeoutCrShort;		// the distance of the short SYN frames after starting the connection
	long TimeoutCrLong;		// the distance of the long SYN frames after counting CountCrShort to zero
	long CountCrShort;		// Count of the short SYN frames
	long TimeoutErr;		// After this time without any frame received the connection is declared as broken
	long LifeAcks;			// Time between life acks
	long TimeoutSend;		// Timeout for sending
	long TimeoutArp;		// Timeout for the arp cache. If an entry isn't used for this time it is lost
	long TimeoutResolve;		// Timeout between the DNS requests if no answer retries
	unsigned short NextTcpPort;	// Start value for the gererated tcp port
	unsigned short NextUdpPort;	// Start value for the gererated udp port
	long TimeoutRetrySend;		// After this time without an answer the send frame is resent
	short NumberRetrySend;		// Number of send retries
	unsigned short Mss;		// Max data frame length in a frame
	unsigned long LifeDataAcks;	// After a time without activity for a connection a life data ack is sent
	unsigned long DataAcks;		// Time value for acks as life data acks
	unsigned short Flags;		// The CRF_xx default values
	unsigned short HeaderFactor;	// This Factor is multiplied with TimeoutErr if the WMK Header is not used. 0 -> 1
	unsigned short MustAck;		// After this time received data are acked if no data are sended out
	unsigned short RFC1006Factor;	// This Factor is multiplied with TimeoutErr if the RFC 1006 Header is used. 0 -> 1
   char res[4];				// reserved, must be 0
}IP_STANDARDVALUES;

typedef struct _IP_SNMP {
	short Cb;			// Number of bytes in the structure including this element
	char MainCommunity[30];		// This community name allowes all
	char ReadCommunity[30];		// This community name allowes reading
	unsigned long Ip;		// If not zero only this station can write data
	unsigned long TrapIp;		// If not zero traps are sent to this station
 char res[256 - 2 - 60 - 8];		// reserved, must be 0
}IP_SNMP;

typedef struct IP_LINESTATUS {
	short Cb;				// Number of bytes in the structure including this element
	unsigned short Handle;			// Handle of the connection
	unsigned short Status;			// Error and status code. IP_xx
	unsigned long TimeoutErr;		// Remaining time until connection timeout
	unsigned short BytesWaitSending;	// Number of bytes wait for an ack
	unsigned short BytesWaitReceiving;	// Number of received and not handled bytes 
   char res[20];				// reserved, must be 0
}IP_LINESTATUS;

#pragma pack()

// Translations
#define IpStartSend			IpStarteSenden
#define IpCheckSend			IpAbfrageSenden
#define IpSendData			IpSendeDaten
#define IpStartRead			IpStarteLesen
#define IpCheckRead			IpAbfrageLesen
#define IpReadData			IpLeseDaten
#define IpTestStatus			IpTesteStatus
#define IpSetVector			IpSetzeVektor

#define IpGetVersion			IpQueryVersion
#define IpStartConnect			IpStartConnection
#define IpRestartConnect		IpRestartConnection
#define IpStopConnect			IpStopConnection
#define IpStopConnectAll		IpStopConnections
#define IpTestStatus			IpTesteStatus
#define IpGetStationParameter		IpQueryStationParameter
#define IpGetStandardValues		IpQueryStandardValues
#define IpGetSnmpValues			IpQuerySnmpValues

#define IpStartSend			IpStarteSenden
#define IpPollSend			IpAbfrageSenden
#define IpSendData 			IpSendeDaten
#define IpStartRead			IpStarteLesen
#define IpPollRead			IpAbfrageLesen
#define IpReadData 			IpLeseDaten

// Funktionsaufrufe ////////////////////////////////////////////////////////////
// Aufrufe fÅr C


EXTERN_C short WENTRY_C IpQueryVersion   (unsigned short W_POINTER version);
EXTERN_C short WENTRY_C IpQueryVersionEx   (unsigned long W_POINTER version);

EXTERN_C short WENTRY_C IpDriverOpen     (void);
EXTERN_C short WENTRY_C IpDriverClose    (void);

EXTERN_C short WENTRY_C IpGetHardwareAddress(unsigned char W_POINTER address,unsigned short adapter);

EXTERN_C short WENTRY_C IpQueryStationParameter (IP_OWN_STATION W_POINTER val);
EXTERN_C short WENTRY_C IpSetStationParameter   (IP_OWN_STATION W_POINTER val);

EXTERN_C short WENTRY_C IpQueryStandardValues   (IP_STANDARDVALUES W_POINTER val);
EXTERN_C short WENTRY_C IpSetStandardValues     (IP_STANDARDVALUES W_POINTER val);

EXTERN_C short WENTRY_C IpQuerySnmpValues   (IP_SNMP W_POINTER val);
EXTERN_C short WENTRY_C IpSetSnmpValues     (IP_SNMP W_POINTER val);

EXTERN_C short WENTRY_C IpStartConnection   (IP_CONNECTION W_POINTER cr);
EXTERN_C short WENTRY_C IpStartConnectionOSI(IP_OSI_CONNECTION W_POINTER crosi);
EXTERN_C short WENTRY_C IpStopConnection    (unsigned short handle);
EXTERN_C short WENTRY_C IpRestartConnection (unsigned short handle);
EXTERN_C short WENTRY_C IpStopConnections   (void);
EXTERN_C short WENTRY_C IpPassiveCR	(NET_PASSIVE_CONNECT W_POINTER cr);

EXTERN_C short WENTRY_C IpStarteSenden   (IP_SENDPARAMS W_POINTER para);
EXTERN_C short WENTRY_C IpAbfrageSenden  (IP_SENDPARAMS W_POINTER para);
EXTERN_C short WENTRY_C IpSendeDaten     (IP_SENDPARAMS W_POINTER para);

EXTERN_C short WENTRY_C IpStarteLesen    (IP_RECPARAMS W_POINTER para);
EXTERN_C short WENTRY_C IpAbfrageLesen   (IP_RECPARAMS W_POINTER para);
EXTERN_C short WENTRY_C IpLeseDaten      (IP_RECPARAMS W_POINTER para);

// EXTERN_C short WENTRY_C IpStartSend      (IP_SENDREC W_POINTER para);
// EXTERN_C short WENTRY_C IpPollSend       (IP_SENDREC W_POINTER para);
// EXTERN_C short WENTRY_C IpSendData       (IP_SENDREC W_POINTER para);

// EXTERN_C short WENTRY_C IpStartRead      (IP_SENDREC W_POINTER para);
// EXTERN_C short WENTRY_C IpPollRead       (IP_SENDREC W_POINTER para);
// EXTERN_C short WENTRY_C IpReadData       (IP_SENDREC W_POINTER para);

EXTERN_C short WENTRY_C IpTesteStatus    (IP_RECPARAMS W_POINTER para);

// EXTERN_C short WENTRY_C IpTestStatus     (IP_LINESTATUS W_POINTER para);

EXTERN_C short WENTRY_C IpReadDebugBuffer (char W_POINTER buffer);

#if !defined (OS2_FUNCTIONS_INCLUDED) && !defined (NT_FUNCTIONS_INCLUDED)
EXTERN_C unsigned short WENTRY_C IpSetzeVektor(unsigned short vektor);
#endif

#endif // IPDEF_H_INTERN
// End IPDef.H  /////////////////////////////////////////////////////////
