// File H1Engl.h
// English Names for all German Functions and Variables
// Created on 06.01.96
// Author: Werner Krings, Werner Mehrbrodt
// Expanded at 08.12.96
// INAT GmbH, Nuremberg
// Mail werner.mehrbrodt@inat.de, mehrbrodt@oberberg-online.de or werner.krings@inat.de
// Last changes at 18.05.99: S7 functions new

#ifdef INCL_ENGLISH_NAMES
 #define INCL_ENGLISH_NAMES

// Variables
#define ConnectionName			Verbindungsname
#define NetError			Fehler
#define NetLine				Vnr
#define NetCard				Karte
#define NetData				Daten
#define S5RequestNumber			Auftragsnummer
#define S5RequestOffset			Offset
#define S5RequestType			Auftragsart
#define S5RequestUsed			Benutzt
#define TimeoutCrFast			TimeoutCrSchnell
#define TimeoutCrSlow			TimeoutCrLangsam

#define PlcType				Kennung
#define ConnectionNumber		Verbindungsnummer

#define S5_CONNECTION			S5_VERBINDUNGSDATEN
#define S5_CONNECTION_NETCARD		S5_VERBINDUNGSDATEN_KARTE
#define S5_USERPARAMS			S5_ANSCHALTUNG

#define CP_CONNECTION			CP_VERBINDUNGSDATEN

#define H1_BUSY				H1_UEBERLAST

// Functions from H1_xx
#define H1GetVersion			H1HoleVersion
#define H1GetStationAddress		H1HoleStationsAdresse
#define H1SetStationAddress		H1SetzeStationsAdresse
#define H1GetStationAddressCard		H1HoleStationsAdresseKarte
#define H1SetStationAddressCard		H1SetzeStationsAdresseKarte
#define H1StartConnect			H1StarteVerbindung
#define H1StartConnectCard		H1StarteVerbindungKarte
#define H1StopConnect			H1StoppeVerbindung
#define H1StopConnectAll		H1StoppeVerbindungen
#define H1StartSend			H1StarteSenden
#define H1PollSend			H1AbfrageSenden
#define H1SendData			H1SendeDaten
#define H1SendDataEx			H1SendeDatenEx
#define H1StartRead			H1StarteLesen
#define H1PollRead			H1AbfrageLesen
#define H1ReadData			H1LeseDaten
#define H1StartReadEx			H1StarteLesenEx
#define H1PollReadEx			H1AbfrageLesenEx
#define H1ReadDataEx			H1LeseDatenEx
#define H1TestStatus			H1TesteStatus
#define H1ReadDebugBuffer		H1LeseDebugBuffer
#define H1ReadDebugFrame		H1LeseDebugFrame
#define H1SetVector			H1SetzeVektor
#define H1GetStandardvalues		H1HoleStandardwerte
#define H1SetStandardvalues		H1SetzeStandardwerte
#define H1GetLineCharacteristics	H1HoleLeitungsparameter
#define H1ListRunningConnections	H1ListeEingetrageneVerbindungen
#define H1ReadParameter			H1LeseParameter
#define H1WriteParameter		H1SchreibeParameter


// Functions from S5_xx
#define S5SetStationAddress		S5SetzeStationsAdresse
#define S5SetStationAddressCard		S5SetzeStationsAdresseKarte
#define S5StartConnection		S5StarteVerbindung
#define S5StartConnectionCard		S5StarteVerbindungKarte
#define S5StartConnectionIp		S5StarteVerbindungIp
#define S5StopConnection		S5StoppeVerbindung
#define S5StopAllConnections		S5StoppeVerbindungen
#define S5ReadFromPLC			S5LeseAusSPS
#define S5StartRead			S5StartLesen
#define S5PollRead			S5AbfrageLesen
#define S5GetConnectionParameter	S5HoleVerbindungsparameter
#define S5PutConnectionParameter	S5SchreibeVerbindungsparameter
#define S5DeleteConnection		S5EntferneVerbindung
#define S5ListConnections		S5ListeVerbindungen
#define S5WriteToPLC			S5SchreibeInSPS
#define S5StartWrite			S5StartSchreiben
#define S5PollWrite			S5AbfrageSchreiben
#define S5SetVector			S5SetzeVektor
#define S5ListNetConnections		S5ListeNetVerbindungen
#define S5WriteConnection		SchreibeS5Anschaltung
#define S5ReadConnection		LeseS5Anschaltung
#define S5ReadConnectionCard		S5HoleVerbindungsparamsKarte
#define S5WriteConnectionCard		S5SchreibeVerbindungKarte
#define S5SetNetfileName		S5SetzeNetDateiname
#define S5GetRevision			S5HoleRevision
#define S5GetStationAddress		S5HoleStationsAdresse
#define S5GetStationAddressCard		S5HoleStationsAdresseKarte
#define S5WriteConnectionCard		S5SchreibeVerbindungsParamsKarte
#define S5ReadParameter			S5LeseParameter
#define S5WriteParameter		S5SchreibeParameter
#define S5PollFetchPassiv		S5AbfrageFetchPassiv
#define S5PollWritePassiv		S5AbfrageWritePassiv

#define S7StartConnectionH1		S7StarteVerbindungH1
#define S7StartConnectionIp		S7StarteVerbindungIp
#define S7StartConnectionIpOsi		S7StarteVerbindungIpOsi

#define NetReadStationParams		NetLeseStationsparameter
#define NetWriteStationParams		NetSchreibeStationsparameter
#define NetReadConnectionParams		NetLeseVerbindung
#define NetWriteConnectionParams	NetSchreibeVerbindung

#define PLCTYPE_DATABLOCK		KENNUNG_BAUSTEIN
#define PLCTYPE_FLAG			KENNUNG_MERKER
#define PLCTYPE_INPUT			KENNUNG_EINGANG
#define PLCTYPE_OUTPUT			KENNUNG_AUSGANG
#define PLCTYPE_PERIPHERIE		KENNUNG_PERIPHERIE
#define PLCTYPE_COUNTER			KENNUNG_ZAEHLER
#define PLCTYPE_TIMER			KENNUNG_TIMER
#define PLCTYPE_SYSTEMDATA		KENNUNG_SYSTEMDATEN
#define PLCTYPE_ABSOLUTE		KENNUNG_ABSOLUT
#define PLCTYPE_EXT_DATABLOCK		KENNUNG_ERW_BAUSTEIN
#define PLCTYPE_EXTMEM			KENNUNG_EXTMEM

#endif // INCL_ENGLISH_NAMES locking
