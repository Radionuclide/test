// Datei SerDef.h
// Definitionen zur WMK Lib Seriell
// Erstellt von den Werners (W.M. W.K.)
// Mail werner.mehrbrodt@inat.de, mehrbrodt@oberberg-online.de or werner.krings@inat.de
// Letzte Žnderung am 24.10.2000

#ifndef INCL_SERDEF_H
 #define INCL_SERDEF_H

// Definitionen von Leitung, Protokoll, Typ, nicht mehr kompatibel mit INT14 Protokolltreiber IPC
// Protokoll
#define NO_PROT		0x0
#define LOADER_PROT	0x1
#define MULTILDR_PROT	0x2
#define CIM_PTP_PROT	0x3
#define FIXPORT_PROT	0x4
#define SIEMENS_CPC	0x5
#define SIEMENS_LOADER	0x6
#define ABB_LOADER	0x7
#define ABB_CPC		0x8
#define KLOEMOE_S3	0x9
#define BONN_LOADER	0xa
#define BONN_CPC	0xb
#define D3964R_PROT	0xc
#define D3964_PROT	0xd

// values for GetUsed SetUsed. Bitwise coded
#define SER_USED_PG 	0x01
#define SER_USED_3964R 	0x02
#define SER_USED_IPC	0x04
#define SER_USED_SUCOM 	0x08
#define SER_USED_PARAM 	0x10

// values for RETCALL_LINE.Typ
#define RETCALL_AS511		1
#define RETCALL_3964R		2
#define RETCALL_CIM		3
#define RETCALL_LOADER		4

#pragma pack(1)

typedef struct _COMM_LINE {
	unsigned short Cb;		// size of this structure in bytes
	unsigned short Line;		// 0 = COM1
	unsigned short Code;		// NORMAL,CIM, LDR, L1, L2, .. 0 = unbelegt
	unsigned long Baud;		// 9600,4800,..
	unsigned short Bits;		// Datenbits 5,6,7,8
	unsigned short Stopbits;	// 1 oder 2
	unsigned short Parity;		// 'o','e','n'
	unsigned short Protocol;	// 'H'ardware, 'S'oftware, 'K'eins
	unsigned short Timeout;		// in Sec
	unsigned short Nodal;		// Node Address or MPI Station
	unsigned char RackNo;		// Rack MPI
	unsigned char RowNo;		// Row Number MPI
	char reserved[16];
}COMM_LINE;

typedef struct _HANDLE_LINE {
	unsigned short Line;		// 0 = COM1
	unsigned short No;		// number of chars to handle
	unsigned short Status;		// line status on return
	unsigned char Data[1];		// array with chars
}HANDLE_LINE;

#if defined DRIVER_VERSION
typedef struct _RETCALL_LINE {
	unsigned short Line;		// 0 = COM1
	unsigned short Typ;		// type of call
	void (far *RetCall)(unsigned short);
}RETCALL_LINE;
#endif

#pragma pack()

#ifdef COM_VERSION_2
EXTERN_C unsigned short WENTRY_C SerInitialiseLine(COMM_LINE W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerGetInitialiseLine(COMM_LINE W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerSendToLine(HANDLE_LINE W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerSendToLineNoWait(HANDLE_LINE W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerSetBreakLine(unsigned short W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerResetBreakLine(unsigned short W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerReadFromlLine(HANDLE_LINE W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerReadFromlLineNoWait(HANDLE_LINE W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerGetStatusOfLine(HANDLE_LINE W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerClearLine(unsigned short W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerSetRetCall(RETCALL_LINE W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerGetDataTable(HANDLE_LINE W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerGetUsed(HANDLE_LINE W_POINTER c);
EXTERN_C unsigned short WENTRY_C SerSetUsed(HANDLE_LINE W_POINTER c);
#else
#if defined DRIVER_VERSION
 unsigned short InitialiseSerialLine(unsigned short line,unsigned short init,unsigned short timeout);
 unsigned short SendToSerialLine(unsigned short line,unsigned char zeichen);
 unsigned short SetBreakSerialLine(unsigned short line);
 unsigned short ResetBreakSerialLine(unsigned short line);
 unsigned short SendToSerialLineNoWait(unsigned short line,unsigned char zeichen);
 unsigned short ReadCharFromSerialLine(unsigned short line);
 unsigned short GetStatusOfSerialLine(unsigned short line);
 unsigned short ClearSerialLine(unsigned short line);
 unsigned short SetAS511RetCall(unsigned short line,unsigned short typ,void (far *cdecl dispatch)(unsigned short));
//  unsigned short SetAS511RetCall(unsigned short line,unsigned short typ,void (far *cdecl dispatch)(void));
 unsigned short GetUsed(unsigned short line);
 unsigned short SetUsed(unsigned short line,unsigned short used);
#endif
#endif

#endif // INCL_SERDEF_H
