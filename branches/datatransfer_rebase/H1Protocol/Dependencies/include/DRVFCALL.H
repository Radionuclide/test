// Datei DrvFCall.h
// Version 1.0
// Erstellt von W.M. W.K.
// Erstellt 10.9.94
// Mail werner.mehrbrodt@inat.de, mehrbrodt@oberberg-online.de or werner.krings@inat.de
// Letzte Bearbeitung 30.04.2000

// Definitionen der Treiber Aufrufcodes (bei DevIOCtl und INTxx)

#ifndef DRVFUNCTIONCALLS
 #define DRVFUNCTIONCALLS

// FÅr DosDevIOCtl
#define LANMAN_PROTOKOLL		0x81

// Aufrufe
#if defined (NT_FUNCTIONS_INCLUDED) || defined (WIN95_FUNCTIONS_INCLUDED)

// Die NT Nummern mÅssen auf 8 gerade sein, sonst INTERNAL ERROR IN NT OPERATING SYSTEM
#define H1_GET_DEBUGBUFFER		2048	// Nur fÅr Test und Diagnosezwecke
#define IP_GET_DEBUGBUFFER		2048	// Nur fÅr Test und Diagnosezwecke
#define SNIF_GET_DEBUGBUFFER		2048	// Nur fÅr Test und Diagnosezwecke
#define FELDBUS_GET_DEBUGBUFFER         2048	// Nur fÅr Test und Diagnosezwecke
#define COMPORT_GET_DEBUGBUFFER         2048	// Nur fÅr Test und Diagnosezwecke
#define H1_GET_DEBUGFRAME		2056	// Nur fÅr Test und Diagnosezwecke
#define H1_GET_FRAMEBUFFER		2064	// Netzwerkanalyser !! Gleich !!
#define SNIF_GET_FRAMEBUFFER		2064	// Netzwerkanalyser !! Gleich !!
#define FELDBUS_GET_FRAMEBUFFER         2064	// Netzwerkanalyser !! Gleich !!
#define H1_SET_FILTER			2072	// Netzwerkanalyser: Protokolliermodus umsetzen
#define SNIF_SET_FILTER                 2072	// Netzwerkanalyser: Protokolliermodus umsetzen
#define H1_FRAME_WAIT_SEM		2080	// Warte auf FrameSem mit Timeout
#define SNIF_FRAME_WAIT_SEM		2080	// Warte auf FrameSem mit Timeout
#define LANMAN_ENTRY			2080	// FÅr interne Zwecke
#define H1_ALLOCATE_FRAMEBUFFER         2088	// Treiberinternen (Ring)buffer allokieren oder freigeben
#define SNIF_ALLOCATE_FRAMEBUFFER	2088	// Treiberinternen (Ring)buffer allokieren oder freigeben
#define SNIF_GET_FRAMES                 2096	// Netzwerkanalyser: Neues Framelesen
#define SNIF_GET_FRAME_HEADER		2104	// Netzwerkanalyser: FrameHeader bearbeiten
#define H1_AWAIT_CONNECT		2104	// H1_RECPARAMS
#define H1_SET_ROM_ADDRESS		2112	// 6 bytes rom address
#define SNIF_GET_SYSVALUES		2112	// BETRIEBSWERTE

#define H1_GET_ETHERNET_ADDRESS         2120	// Pointer auf 14 Bytes. Byte 1 und 2 = len
#define H1_SET_ETHERNET_ADDRESS         2128	// Pointer auf 6 Bytes
#define H1_CONNECT_REQUEST		2136	// Pointer auf H1_CONNECTPARAMS
#define IP_CONNECT_REQUEST		2136	// Pointer auf H1_CONNECTPARAMS
#define FELDBUS_CONNECT_REQUEST		2136	// Pointer auf FB_CONNECTPARAMS
#define COMPORT_CONNECT_REQUEST		2136	// Pointer auf FB_CONNECTPARAMS
#define H1_DISCONNECT_REQUEST		2144	// Pointer auf Verbindungsnummer
#define IP_DISCONNECT_REQUEST		2144	// Pointer auf Verbindungsnummer
#define FELDBUS_DISCONNECT_REQUEST	2144	// Pointer auf Verbindungsnummer
#define COMPORT_DISCONNECT_REQUEST	2144	// Pointer auf Verbindungsnummer
#define H1_SEND_DATA			2152	// Pointer auf H1_SENDPARAMS
#define IP_SEND_DATA			2152	// Pointer auf H1_SENDPARAMS
#define FELDBUS_SEND_DATA		2152	// Pointer auf FB_SENDPARAMS
#define COMPORT_SEND_DATA		2152	// Pointer auf FB_SENDPARAMS
#define H1_RECEIVE_DATA                 2160	// Pointer auf H1_RECPARAMS
#define IP_RECEIVE_DATA                 2160	// Pointer auf H1_RECPARAMS
#define FELDBUS_RECEIVE_DATA		2160	// Pointer auf FB_RECPARAMS
#define COMPORT_RECEIVE_DATA		2160	// Pointer auf FB_RECPARAMS
#define H1_GET_LINEPARAMS		2168	// Pointer auf H1_RECPARAMS
#define IP_GET_LINEPARAMS		2168	// Pointer auf H1_RECPARAMS
#define FELDBUS_GET_LINEPARAMS		2168	// Pointer auf FB_RECPARAMS
#define COMPORT_GET_LINEPARAMS		2168	// Pointer auf FB_RECPARAMS
#define H1_START_SEND			2176	// Pointer auf H1_SENDPARAMS
#define IP_START_SEND			2176	// Pointer auf H1_SENDPARAMS
#define FELDBUS_START_SEND		2176	// Pointer auf FB_SENDPARAMS
#define COMPORT_START_SEND		2176	// Pointer auf FB_SENDPARAMS
#define H1_START_RECEIVE		2184	// Pointer auf H1_RECPARAMS
#define IP_START_RECEIVE		2184	// Pointer auf H1_RECPARAMS
#define FELDBUS_START_RECEIVE		2184	// Pointer auf FB_RECPARAMS
#define COMPORT_START_RECEIVE		2184	// Pointer auf FB_RECPARAMS
#define H1_CHECK_SEND			2192	// Pointer auf H1_SENDPARAMS
#define IP_CHECK_SEND			2192	// Pointer auf H1_SENDPARAMS
#define FELDBUS_CHECK_SEND		2192	// Pointer auf FB_SENDPARAMS
#define COMPORT_CHECK_SEND		2192	// Pointer auf FB_SENDPARAMS
#define H1_CHECK_RECEIVE		2200	// Pointer auf H1_RECPARAMS
#define IP_CHECK_RECEIVE		2200	// Pointer auf H1_RECPARAMS
#define FELDBUS_CHECK_RECEIVE		2200	// Pointer auf FB_RECPARAMS
#define COMPORT_CHECK_RECEIVE		2200	// Pointer auf FB_RECPARAMS
#define H1_BIND_TO_S5			2208	// Pointer auf S5_BINDPARAMS
#define H1_SEND_FRAME			2216	// Netzwerkanalyser: Beliebiges Frame senden
#define SNIF_SEND_FRAME                 2216	// Netzwerkanalyser: Beliebiges Frame senden
#define H1_DRIVER_RESET                 2224	// PlugAndPlay: Treiber intern zurÅcksetzen
#define IP_DRIVER_RESET                 2224	// PlugAndPlay: Treiber intern zurÅcksetzen
#define FELDBUS_DRIVER_RESET		2224	// PlugAndPlay: Treiber intern zurÅcksetzen
#define SNIF_DRIVER_RESET		2224	// PlugAndPlay: Treiber intern zurÅcksetzen
#define COMPORT_DRIVER_RESET		2224	// PlugAndPlay: Treiber intern zurÅcksetzen
#define H1_GET_VERSION			2232	// Pointer auf Versionsnummer allgemein
#define IP_GET_VERSION			2232	// Pointer auf Versionsnummer allgemein
#define SNIF_GET_VERSION		2232	// Pointer auf Versionsnummer allgemein
#define COMPORT_GET_VERSION		2232	// Pointer auf Versionsnummer allgemein
#define SNIFFER_GET_VERSION		2240	// Pointer auf Versionsnummer Netzwerkanalyser
#define FELDBUS_GET_VERSION		2232	// Pointer auf Versionsnummer allgemein
#define H1_SET_ETHERNET_ADDRESS_LINE	2248	// Pointer auf 6 Bytes mit der Adresse, 2 Bytes Karte
#define SNIF_SET_ETHERNET_ADDRESS	2248	// Pointer auf 6 Bytes mit der Adresse, 2 Bytes Karte
#define H1_CONNECT_REQUEST_LINE         2256	// Pointer auf H1_CONNECTPARAMS
#define IP_OSI_CONNECT_REQUEST          2256	// Pointer auf IP_OSI_CONNECT_PARAMS
#define H1_GET_ETHERNET_ADDRESS_LINE	2264	// Pointer auf 14 Bytes. Byte 1 und 2 = len
#define SNIF_GET_ETHERNET_ADDRESS	2264	// Pointer auf 14 Bytes. Byte 1 und 2 = len
#define H1_GET_INITVALUES		2272	// Pointer auf H1_INITVALUES
#define IP_GET_INITVALUES		2272	// Pointer auf H1_INITVALUES
#define FELDBUS_GET_INITVALUES		2272	// Pointer auf H1_INITVALUES
#define COMPORT_GET_INITVALUES		2272	// Pointer auf H1_INITVALUES
#define H1_SET_INITVALUES		2280	// Pointer auf FB_INITVALUES
#define IP_SET_INITVALUES		2280	// Pointer auf FB_INITVALUES
#define FELDBUS_SET_INITVALUES		2280	// Pointer auf FB_INITVALUES
#define COMPORT_SET_INITVALUES		2280	// Pointer auf FB_INITVALUES
#define H1_SEND_EX			2288	// Pointer auf H1_SENDPARAMS
#define H1_RECEIVE_DATA_EX		2296	// Pointer auf H1_RECPARAMS
#define H1_START_RECEIVE_EX		2304	// Pointer auf H1_RECPARAMS
#define H1_CHECK_RECEIVE_EX		2312	// Pointer auf H1_RECPARAMS
#define H1_GET_LINE_VALUES		2320	// Pointer auf H1_LINEVAL
#define IP_GET_LINE_VALUES		2320	// Pointer auf IP_LINEVAL
#define COMPORT_GET_LINE_STATUS		2320	// Pointer auf COMPORT_LINESTATUS
#define H1_GET_ALL_DEBUG		2328	// Alle Anzeigenworte und ZustÑnde
#define IP_GET_ALL_DEBUG		2328	// Alle Anzeigenworte und ZustÑnde
#define COMPORT_GET_ALL_DEBUG		2328	// Alle Anzeigenworte und ZustÑnde
#define SNIF_STATISTIK			2336	// Pointer auf STATISTIKBLOCK
#define H1_LAMPEN			2336	// Schalten der Lampen bei Telegrammen
#define H1_DRIVER_SYNC			2344	// void
#define FELDBUS_SET_TYPE		2352	// Feldbustyp
#define H1_RESTART_CONNECTION		2352	// handle
#define IP_RESTART_CONNECTION		2352	// handle
#define COMPORT_RESTART_CONNECTION		2352	// handle
#define FELDBUS_READ_EEPROM		2360	// EEPROM_DATA *
#define FELDBUS_WRITE_EEPROM		2368	// EEPROM_DATA *
#define FELDBUS_DOWNLOAD		2376	//
#define FELDBUS_GET_TYPE		2384	// long *
#define FELDBUS_IBS_START_BUS		2392	// void
#define FELDBUS_IBS_STOP_BUS		2400	// void
#define FELDBUS_IBS_ALARMSTOP_BUS	2408	// void
#define FELDBUS_IBS_GET_CONFIG		2416	// IBS_CONFIG
#define FELDBUS_CAN_ENABLE		2424	// karte
#define FELDBUS_CAN_DISABLE		2432	// karte
#define FELDBUS_CAN_RESET		2440	// karte
#define FELDBUS_CAN_SETBAUD		2448	// CAN_CONFIG
#define FELDBUS_IBS_RECEIVE_OUTPUT	2456	// FB_READPARAMS
#define FELDBUS_DP_SET_BUS		2464	// DP_MASTERCONFIG
#define FELDBUS_DP_GET_MODE		2472	// FIELDBUS_GENERAL
#define FELDBUS_DP_SET_MODE		2480	// FIELDBUS_GENERAL
#define FELDBUS_DP_GET_DIAG		2488	// DP_PACKEDDIAG
#define FELDBUS_DP_GET_PARAMS		2496	// DP_PARAMS
#define FELDBUS_DP_SET_PARAMS		2504	// DP_PARAMS
#define FELDBUS_DP_CHECK_PARAMS		2512	// DP_CHECKPARAMS
#define FELDBUS_DP_SETADDRESS		2520	// DP_SLAVEADDRESS
#define FELDBUS_DP_RECEIVE_OUTPUT	2528	// FB_READPARAMS
#define FELDBUS_DP_EXCHANGE_DATA	2536	// DP_EXCHANGEDATA
#define FELDBUS_DP_CONTROL_DATA		2542	// DP_CONTROLDATA
#define FELDBUS_DP_GET_BUS		2550	// DP_MASTERCONFIG
#define H1_SEND_NEW			2400	// H1_SENDREC
#define IP_SEND_NEW			2400	// H1_SENDREC
#define H1_CHECK_SEND_NEW		2408	// H1_SENDREC
#define IP_CHECK_SEND_NEW		2408	// H1_SENDREC
#define H1_START_SEND_NEW		2416	// H1_SENDREC
#define IP_START_SEND_NEW		2416	// H1_SENDREC
#define H1_RECEIVE_NEW			2424	// H1_SENDREC
#define IP_RECEIVE_NEW			2424	// H1_SENDREC
#define H1_CHECK_RECEIVE_NEW		2432	// H1_SENDREC
#define IP_CHECK_RECEIVE_NEW		2432	// H1_SENDREC
#define H1_START_RECEIVE_NEW		2440	// H1_SENDREC
#define IP_START_RECEIVE_NEW		2440	// H1_SENDREC
#define IP_GET_STATION			2448	// IP_EIGENE_STATION
#define IP_SET_STATION			2456	// IP_EIGENE_STATION
#define FREECODE			2464	// 10 Byte Freigabecode
#define FELDBUS_DPSLAVE_SET_CONFIG	2558	// DP_SLAVECONFIG
#define FELDBUS_DPSLAVE_GET_CONFIG	2566	// DP_SLAVECONFIG
#define FELDBUS_DPSLAVE_SET_DIAG	2574	// 
#define DRV_GET_VERSION			2574	// Versionsnummer long
#define FELDBUS_DPSLAVE_GET_DIAG     	2582	// 
#define FELDBUS_DPSLAVE_ENABLE          2590	// 
#define FELDBUS_DPSLAVE_DISABLE         2598	// 
#define FELDBUS_DPSLAVE_SET_ADDRESS   	2606	// 
#define FELDBUS_DPSLAVE_SET_ID          2614	// 
#define FELDBUS_SEND_DATA_		2122	// Pointer auf FB_SENDPARAMS
#define FELDBUS_START_SEND_		2130	// Pointer auf FB_SENDPARAMS
#define FELDBUS_CHECK_SEND_		2138	// Pointer auf FB_SENDPARAMS
#define FELDBUS_RECEIVE_DATA_		2146	// Pointer auf FB_RECPARAMS
#define FELDBUS_START_RECEIVE_		2154	// Pointer auf FB_RECPARAMS
#define FELDBUS_CHECK_RECEIVE_		2162	// Pointer auf FB_RECARAMS
#define FELDBUS_CAN_SETIDENT		2170
#define FELDBUS_DP_QUERY_RESPOND	2178
#define FELDBUS_DP_SET_RESPOND		2186
#define MEM_BIND			2194	// Pointer auf Funktion
#define KACHEL_GET_S5EPROM	        2202	// EEPROM_DATEN *
#define KACHEL_SET_S5EPROM	        2210	// EEPROM_DATEN *
#define GENERAL_OUTPORT                 2218	// Debug: Setze Port
#define GENERAL_INPORT			2226	// Debug: Lese Port
#define GENERAL_MEMREAD                 2234	// Debug: Lese Speicher
#define GENERAL_MEMWRITE		2242	// Debug: Schreibe Speicher
#define H1_GET_MEM_DEBUG		2250	// ALL_MAIN_SPEICHERANZEIGE




#else // NT_FUNCTIONS_INCLUDED
// Bei Unixen, OS/2, WIN3.x und Dos UCHAR
#define H1_GET_DEBUGBUFFER		0x10	// Nur fÅr Test und Diagnosezwecke
#define IP_GET_DEBUGBUFFER		0x10	// Nur fÅr Test und Diagnosezwecke
#define SNIF_GET_DEBUGBUFFER		0x10	// Nur fÅr Test und Diagnosezwecke
#define FELDBUS_GET_DEBUGBUFFER		0x10	// Nur fÅr Test und Diagnosezwecke
#define SER_GET_DEBUGBUFFER		0x10	// Nur fÅr Test und Diagnosezwecke
#define COMPORT_GET_DEBUGBUFFER		0x10	// Nur fÅr Test und Diagnosezwecke
#define H1_GET_DEBUGFRAME		0x11	// Nur fÅr Test und Diagnosezwecke
#define H1_GET_FRAMEBUFFER		0x12	// NetzwerkanalyserNur fÅr Testzwecke
#define SNIF_GET_FRAMEBUFFER		0x12	// NetzwerkanalyserNur fÅr Testzwecke
#define FELDBUS_GET_FRAMEBUFFER		0x12	// NetzwerkanalyserNur fÅr Testzwecke
#define H1_SET_FILTER			0x13	// Netzwerkanalyser: Filter setzen
#define SNIF_SET_FILTER                 0x13	// Netzwerkanalyser: Filter setzen
#define SER_GETUSED			0x13	// Pointer auf HANDLE_LINE
#define H1_FRAME_WAIT_SEM		0x14	// Warte auf FrameSem mit Timeout
#define SNIF_FRAME_WAIT_SEM		0x14	// Warte auf FrameSem mit Timeout
#define SER_GET_RETCALL			0x14	// pointer auf HANDLE_LINE
#define H1_ALLOCATE_FRAMEBUFFER         0x15	// Treiberinternen (Ring)buffer allokieren oder freigeben
#define SNIF_ALLOCATE_FRAMEBUFFER	0x15	// Treiberinternen (Ring)buffer allokieren oder freigeben
#define SER_SET_RETCALL			0x15	// pointer auf HANDLE_LINE
#define SNIF_GET_FRAMES                 0x16	// Netzwerkanalyser: Neues Framelesen
#define H1_GET_LINE_DEBUG		0x16	// CP_LINE
#define SNIF_GET_FRAME_HEADER		0x17	// Netzwerkanalyser: FrameHeader bearbeiten
#define H1_AWAIT_CONNECT		0x17	// H1_RECPARAMS
#define SER_GET_DATATABLE		0x17	// Pointer auf HANDLE_LINE
#define H1_SET_ROM_ADDRESS		0x18	// 6 bytes rom address
#define SNIF_STATISTIK			0x18	// Pointer auf STATISTIKBLOCK
#define KACHEL_GET_S5EPROM	        0x18	// EEPROM_DATEN *
#define SNIF_GET_SYSVALUES		0x19	// BETRIEBSWERTE
#define SER_SETUSED			0x19	// Pointer auf HANDLE_LINE
#define KACHEL_GET_EEPROM		0x19	// DUMPSTRUCT
#define KACHEL_SET_EEPROM		0x1a	// DUMPSTRUCT

#define KACHEL_START_PGFUNC	0x1d	// PG FunktionalitÑt starten, Param: COMx, 0<x<10
#define KACHEL_STOP_PGFUNC	0x1e	// PG FunktionalitÑt stoppen
#define KACHEL_TEST_PGVERBINDUNG 0x1f	// *val != 0 -> PG rennt

#define LANMAN_ENTRY			0x58	// FÅr interne Zwecke
#define H1_GET_ETHERNET_ADDRESS         0x20	// Pointer auf 14 Bytes. Byte 1 und 2 = len
#define H1_GET_ETHERNET_ADDRESS_LINE	0x21	// Pointer auf 14 Bytes. Byte 1 und 2 = len
#define SNIF_GET_ETHERNET_ADDRESS	0x21	// Pointer auf 14 Bytes. Byte 1 und 2 = len
#define H1_SET_ETHERNET_ADDRESS_LINE	0x22	// Pointer auf 6 Bytes
#define SNIF_SET_ETHERNET_ADDRESS	0x22	// Pointer auf 6 Bytes
#define SER_DOS_ENTRY			0x22	// Pointer auf seriell Parameter
#define KACHEL_GET_SERIENNUMMER		0x22
#define KACHEL_GET_KACHELSTART		0x23
#define SER_SETBREAK			0x23	// line
#define KACHEL_SET_KACHELSTART		0x24
#define SER_RESETBREAK			0x24	// line
#define KACHEL_GET_KACHELANZAHL		0x25	// short Kachelanzahl (4 oder 8)
#define TIME_TIC_DECREMENT		0x26	// short far *
#define HANDLE_CONNECTION		0x27	// IP_OSI_CONNECTION far *
#define KACHEL_WORK_PGFUNC      0x28    // PG FunktionalitÑt bearbeiten, keinen Parameter
#define KACHEL_WORK_PGAKTIV     0x2b    // PG FunktionalitÑt Aktiv Start. STX schon weg
#define KACHEL_SET_S5EPROM		0x2d	// EEPROM_DATEN *
#define KACHEL_GET_KACHELFEHLER		0x2e	// KACHELFEHLER
#define H1_SET_ETHERNET_ADDRESS         0x30	// Pointer auf 6 Bytes
#define MAIN_GET_KENNUNG		0x30	// KENNUNGS_ROM
#define H1_CONNECT_REQUEST		0x31	// Pointer auf H1_CONNECTPARAMS
#define IP_CONNECT_REQUEST		0x31	// Pointer auf H1_CONNECTPARAMS
#define FELDBUS_CONNECT_REQUEST		0x31	// Pointer auf FB_CONNECTPARAMS
#define COMPORT_CONNECT_REQUEST		0x31	// Pointer auf H1_CONNECTPARAMS
#define SER_SET_LINEPARAMS		0x31	// Pointer auf COMM_LINE
#define H1_DISCONNECT_REQUEST		0x32	// Pointer auf Verbindungsnummer
#define IP_DISCONNECT_REQUEST		0x32	// Pointer auf Verbindungsnummer
#define SER_CLEAR			0x32	// Pointer auf line
#define FELDBUS_DISCONNECT_REQUEST	0x32	// Pointer auf Verbindungsnummer
#define COMPORT_DISCONNECT_REQUEST	0x32	// Pointer auf Verbindungsnummer
#define H1_SEND_DATA			0x33	// Pointer auf H1_SENDPARAMS
#define IP_SEND_DATA			0x33	// Pointer auf H1_SENDPARAMS
#define SER_SEND			0x33	// Pointer auf HANDLE_LINE
#define FELDBUS_SEND_DATA		0x33	// Pointer auf H1_SENDPARAMS
#define COMPORT_SEND_DATA		0x33	// Pointer auf FB_SENDPARAMS
#define KACHEL_PG_RESET			0x33	// void
#define KACHEL_ERROR_BLOCK		0x33	// TF_ERR_BLOCK
#define MAIN_SET_KENNUNG		0x33	// KENNUNGS_ROM
#define H1_RECEIVE_DATA                 0x34	// Pointer auf H1_RECPARAMS
#define IP_RECEIVE_DATA                 0x34	// Pointer auf H1_RECPARAMS
#define SER_READ			0x34	// Pointer auf HANDLE_LINE
#define FELDBUS_RECEIVE_DATA            0x34	// Pointer auf H1_RECPARAMS
#define COMPORT_RECEIVE_DATA            0x34	// Pointer auf H1_RECPARAMS
#define KACHEL_ANZEIGEWORT		0x34	// 3 Bytes Anzeigewort
#define H1_GET_LINEPARAMS		0x35	// Pointer auf H1_CONNECTPARAMS
#define IP_GET_LINEPARAMS		0x35	// Pointer auf IP_CONNECTPARAMS
#define FELDBUS_GET_LINEPARAMS		0x35	// Pointer auf H1_CONNECTPARAMS
#define SER_GET_LINEPARAMS		0x35	// Pointer auf COMM_LINE
#define COMPORT_GET_LINEPARAMS		0x35	// Pointer auf COMM_LINE
#define H1_START_SEND			0x36	// Pointer auf H1_SENDPARAMS
#define IP_START_SEND			0x36	// Pointer auf H1_SENDPARAMS
#define FELDBUS_START_SEND		0x36	// Pointer auf H1_SENDPARAMS
#define COMPORT_START_SEND		0x36	// Pointer auf H1_SENDPARAMS
#define SER_SEND_NOWAIT			0x36	// Pointer auf HANDLE_LINE
#define H1_START_RECEIVE		0x37	// Pointer auf H1_RECPARAMS
#define IP_START_RECEIVE		0x37	// Pointer auf H1_RECPARAMS
#define FELDBUS_START_RECEIVE		0x37	// Pointer auf H1_RECPARAMS
#define COMPORT_START_RECEIVE		0x37	// Pointer auf H1_RECPARAMS
#define SER_READ_NOWAIT			0x37	// Pointer auf HANDLE_LINE
#define H1_CHECK_SEND			0x38	// Pointer auf H1_SENDPARAMS
#define IP_CHECK_SEND			0x38	// Pointer auf H1_SENDPARAMS
#define FELDBUS_CHECK_SEND		0x38	// Pointer auf H1_SENDPARAMS
#define COMPORT_CHECK_SEND		0x38	// Pointer auf H1_SENDPARAMS
#define SER_CHECK			0x38	// Pointer auf HANDLE_LINE
#define H1_CHECK_RECEIVE		0x39	// Pointer auf H1_RECPARAMS
#define IP_CHECK_RECEIVE		0x39	// Pointer auf H1_RECPARAMS
#define FELDBUS_CHECK_RECEIVE		0x39	// Pointer auf H1_RECPARAMS
#define COMPORT_CHECK_RECEIVE		0x39	// Pointer auf H1_RECPARAMS
#define H1_BIND_TO_S5			0x3A	// Pointer auf S5_BINDPARAMS
#define IP_BIND_TO_S5			0x3A	// Pointer auf S5_BINDPARAMS
#define H1_SEND_FRAME			0x3b	// Beliebiges Frame senden
#define SNIF_SEND_FRAME                 0x3b	// Beliebiges Frame senden
#define H1_DRIVER_RESET                 0x3c	// Treiber intern zurÅcksetzen
#define IP_DRIVER_RESET                 0x3c	// Treiber intern zurÅcksetzen
#define FELDBUS_DRIVER_RESET            0x3c	// Treiber intern zurÅcksetzen
#define SNIF_DRIVER_RESET		0x3c	// Treiber intern zurÅcksetzen
#define H1_CONNECT_REQUEST_LINE         0x3d	// Pointer auf H1_CONNECTPARAMS
#define IP_OSI_CONNECT_REQUEST		0x3d	// Pointer auf IP_OSI_CONNECT_PARAMS
#define H1_GET_VERSION			0x40	// Versionsnummer ermitteln
#define IP_GET_VERSION			0x40	// Versionsnummer ermitteln
#define SNIF_GET_VERSION		0x40	// Versionsnummer ermitteln
#define FELDBUS_GET_VERSION		0x40	// Versionsnummer ermitteln
#define KACHEL_GET_VERSION		0x40	// Versionsnummer ermitteln
#define COMPORT_GET_VERSION		0x40	// Versionsnummer ermitteln
#define KACHEL_GET_UHRZEIT		0x41	// UHRZEIT
#define KACHEL_SET_UHRZEIT		0x42	// UHRZEIT
#define KACHEL_GET_UHRSETUP		0x43	// UHRSETUP
#define KACHEL_SET_UHRSETUP		0x44	// UHRSETUP
#define SNIFFER_GET_VERSION		0x45	// Versionsnummer ermitteln
#define KACHEL_GET_KOMPATIBILITY	0x45	// Kompatibilityflag. 1 = set
#define H1_GET_ALL_DEBUG		0x46	// Alle Anzeigenworte und ZustÑnde
#define IP_GET_ALL_DEBUG		0x46	// Alle Anzeigenworte und ZustÑnde
#define COMPORT_GET_ALL_DEBUG		0x46	// Alle Anzeigenworte und ZustÑnde
#define GENERAL_OUTPORT                 0x47	// Debug: Setze Port
#define KACHEL_SET_KOMPATIBILITY	0x47	// Kompatibilityflag. 1 = set
#define GENERAL_INPORT			0x48	// Debug: Lese Port
#define GENERAL_MEMREAD                 0x49	// Debug: Lese Speicher
#define KACHEL_REBOOT			0x4a	// Hardwar
#define H1_GET_INITVALUES		0x4b	// Pointer auf H1_INITVALUES
#define IP_GET_INITVALUES		0x4b	// Pointer auf IP_INITVALUES
#define FELDBUS_GET_INITVALUES		0x4b	// Pointer auf FB_INITVALUES
#define COMPORT_GET_INITVALUES		0x4b	// Pointer auf FB_INITVALUES
#define MAIN_SET_DEBUGBUFFER		0x4b	// Nur fÅr Test und Diagnosezwecke
#define H1_SET_INITVALUES		0x4c	// Pointer auf H1_INITVALUES
#define IP_SET_INITVALUES		0x4c	// Pointer auf IP_INITVALUES
#define FELDBUS_SET_INITVALUES		0x4c	// Pointer auf FB_INITVALUES
#define COMPORT_SET_INITVALUES		0x4c	// Pointer auf FB_INITVALUES
#define H1_SEND_EX			0x4d	// Pointer auf H1_SENDPARAMS
#define GENERAL_MEMWRITE		0x4d	// Debug: Schreibe Speicher
#define GENERAL_FLASHID			0x4e	// short (low manufacturer hinh device)
#define H1_RECEIVE_DATA_EX		0x4e	// Pointer auf H1_RECPARAMS
#define H1_START_RECEIVE_EX		0x4f	// Pointer auf H1_RECPARAMS
#define H1_CHECK_RECEIVE_EX		0x50	// Pointer auf H1_RECPARAMS
#define H1_GET_LINE_VALUES		0x51	// Pointer auf H1_LINEVAL
#define IP_GET_LINE_VALUES		0x51	// Pointer auf IP_LINEVAL
#define COMPORT_GET_LINE_STATUS		0x51	// Pointer auf COMPORT_LINESTATUS
#define H1_LAMPEN			0x52	// Schalten der Lampen bei Telegrammen
#define H1_BROADCAST			0x53	// RÅckruf bei allen Broadcasts != H1 (FEFE in Kennung)
#define H1_DRIVER_SYNC			0x54	// void
#define FELDBUS_SET_TYPE                0x55    // Feldbustyp
#define H1_RESTART_CONNECTION		0x55	// handle
#define IP_RESTART_CONNECTION		0x55	// handle
#define COMPORT_RESTART_CONNECTION		0x55	// handle
#define FELDBUS_READ_EEPROM             0x56    // EEPROM_DATA *
#define FELDBUS_WRITE_EEPROM            0x57    // EEPROM_DATA *
#define FELDBUS_DOWNLOAD                0x58    //
#define FELDBUS_GET_TYPE                0x59    // long *
#define FELDBUS_IBS_START_BUS           0x5a    // void
#define FELDBUS_IBS_STOP_BUS            0x5b    // void
#define FELDBUS_IBS_ALARMSTOP_BUS       0x5c    // void
#define FELDBUS_IBS_GET_CONFIG          0x5d    // IBS_CONFIG
#define FELDBUS_CAN_ENABLE              0x5e    // karte
#define FELDBUS_CAN_DISABLE             0x5f    // karte
#define FELDBUS_CAN_RESET               0x60    // karte
#define FELDBUS_CAN_SETBAUD             0x61    // CAN_CONFIG
#define FELDBUS_IBS_RECEIVE_OUTPUT	0x62	// FB_READPARAMS
#define FELDBUS_DP_SET_BUS		0x63	// DP_MASTERCONFIG
#define FELDBUS_DP_GET_MODE		0x64	// FIELDBUS_GENERAL
#define FELDBUS_DP_SET_MODE		0x65	// FIELDBUS_GENERAL
#define FELDBUS_DP_GET_DIAG		0x66	// DP_PACKEDDIAG
#define FELDBUS_DP_GET_PARAMS		0x67	// DP_PARAMS
#define FELDBUS_DP_SET_PARAMS		0x68	// DP_PARAMS
#define FELDBUS_DP_CHECK_PARAMS		0x69	// DP_CHECKPARAMS
#define FELDBUS_DP_SETADDRESS		0x6A	// DP_SLAVEADDRESS
#define FELDBUS_DP_RECEIVE_OUTPUT	0x6B	// FB_READPARAMS
#define FELDBUS_DP_EXCHANGE_DATA	0x6C	// DP_EXCHANGEDATA
#define FELDBUS_DP_CONTROL_DATA		0x6D	// DP_CONTROLDATA
#define FELDBUS_DP_GET_BUS		0x6E	// DP_MASTERCONFIG
#define H1_SEND_NEW			0x5b	// H1_SENDREC
#define IP_SEND_NEW			0x5b	// H1_SENDREC
#define H1_CHECK_SEND_NEW		0x5c	// H1_SENDREC
#define IP_CHECK_SEND_NEW		0x5c	// H1_SENDREC
#define GET_MODBUS_DB_TABLE		0x5c	// MODBUS_DB_TABLE
#define H1_START_SEND_NEW		0x5d	// H1_SENDREC
#define IP_START_SEND_NEW		0x5d	// H1_SENDREC
#define SET_MODBUS_DB_TABLE		0x5d	// MODBUS_DB_TABLE
#define H1_RECEIVE_NEW			0x5e	// H1_SENDREC
#define IP_RECEIVE_NEW			0x5e	// H1_SENDREC
#define KACHEL_GET_PASSWORD		0x5f	// PG_PASSWORD
#define H1_CHECK_RECEIVE_NEW		0x5f	// H1_SENDREC
#define IP_CHECK_RECEIVE_NEW		0x5f	// H1_SENDREC
#define KACHEL_SET_PASSWORD		0x60	// PG_PASSWORD
#define H1_START_RECEIVE_NEW		0x60	// H1_SENDREC
#define IP_START_RECEIVE_NEW		0x60	// H1_SENDREC
#define IP_GET_STATION			0x61	// IP_OWN_STATION
#define IP_SET_STATION			0x62	// IP_OWN_STATION
#define FREECODE			0x63	// 10 Byte Freigabecode
#define IP_GET_SNMPVALUES		0x64	// IP_OWN_STATION
#define IP_SET_SNMPVALUES		0x65	// IP_OWN_STATION
#define H1_GET_MEM_DEBUG		0x66	// ALL_MAIN_SPEICHERANZEIGE
#define IP_GET_ALL_IPOSI_DEBUG		0x66	// ALL_IPOSI_ANZEIGE
#define SER_GET_AVAIL			0x67	// ECHO_SERIAL_AVAILIBLE
#define CP_CACHESTATI                   0x68	// Pointer auf CACHE_LIST

#define FELDBUS_DPSLAVE_SET_CONFIG	0x70	// DP_SLAVECONFIG
#define IP_FTP_FILE_POLL		0x70	// FTP_FILE_STRUCT
#define MEM_SET_TF_VARS			0x70	// Pointer auf TF_STORE_VARIABLES
#define IP_FTP_FILE_SEND		0x71	// FTP_FILE_STRUCT
#define FELDBUS_DPSLAVE_GET_CONFIG	0x71	// DP_SLAVECONFIG
#define MEM_SET_TF_GROUPS		0x71	// Pointer auf TF_STORE_GROUPS
#define FELDBUS_DPSLAVE_SET_DIAG	0x72	// 
#define DRV_GET_VERSION			0x73	// Versionsnummer long
#define FELDBUS_DPSLAVE_GET_DIAG     	0x73	// 
#define FELDBUS_DPSLAVE_ENABLE          0x74	// 
#define FELDBUS_DPSLAVE_DISABLE         0x75	// 
#define FELDBUS_DPSLAVE_SET_ADDRESS   	0x76	// 
#define FELDBUS_DPSLAVE_SET_ID          0x77	// 
#define FELDBUS_SEND_DATA_		0x78	// Pointer auf FB_SENDPARAMS
#define FELDBUS_START_SEND_		0x79	// Pointer auf FB_SENDPARAMS
#define FELDBUS_CHECK_SEND_		0x7a	// Pointer auf FB_SENDPARAMS
#define FELDBUS_RECEIVE_DATA_		0x7b	// Pointer auf FB_RECPARAMS
#define FELDBUS_START_RECEIVE_		0x7c	// Pointer auf FB_RECPARAMS
#define FELDBUS_CHECK_RECEIVE_		0x7d	// Pointer auf FB_RECARAMS
#define MEM_BIND			0x7e	// Pointer auf Funktion
// 0x80 - 0x8f belegt
#define KACHEL_GET_EEPROM_EX		0x92	// DUMPSTRUCT

#endif // NT_FUNCTIONS_INCLUDED

#endif // DRVFUNCTIONCALLS
