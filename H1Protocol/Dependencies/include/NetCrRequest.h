// Datei NetCrRequest.h
// Definitions to the Network Passive CR
// Erstellt  W. Mehrbrodt / Krings
// Mail werner.mehrbrodt@inat.de, mehrbrodt@oberberg-online.de or werner.krings@inat.de
// 03.10.2000	Created

#ifndef NETCRREQUEST_H_INTERN			// conditional for recursive calls
 #define NETCRREQUEST_H_INTERN

#define MAXLEN_JOKER_TSAP		8

typedef struct _NET_PASSIVE_CONNECT
{	unsigned short Cb;              // Number of bytes in the structure including this element
	unsigned short Function;	// 0 Start, 1 Result
        unsigned char LenDestTSAP;	/* Len dest TSAP_ID */
        unsigned char DestTSAP[MAXLEN_JOKER_TSAP];	/* Dest TSAP-ID */
        unsigned char LenOwnTSAP;	/* Len own TSAP_ID */
        unsigned char OwnTSAP[MAXLEN_JOKER_TSAP];	/* Own TSAP-ID */
	unsigned short Adapter;		// Adapter for the request. 0 = adapter 1
	unsigned short Flags;		// .. USED_TWOLINES, ..
	unsigned short Vnr;		// Handle for the connection on return
	unsigned short Status;		// status on return
	char reserved[10];
}NET_PASSIVE_CONNECT;


#endif // NETCRREQUEST_H_INTERN
