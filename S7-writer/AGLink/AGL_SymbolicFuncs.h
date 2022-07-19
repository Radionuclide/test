/*******************************************************************************

 Projekt        : Neue Version der AGLink-Bibliothek

 Dateiname      : AGL_SymbolikFuncs.H

 Beschreibung   : Definition der öffentlichen Funktionen

 Copyright      : (c) 1998-2017
                  DELTALOGIC Automatisierungstechnik GmbH
                  Stuttgarter Str. 3
                  73525 Schwäbisch Gmünd
                  Web : http://www.deltalogic.de
                  Tel.: +49-7171-916120
                  Fax : +49-7171-916220

 Erstellt       : 23.03.2004  RH

 Geändert       : 03.01.2017  RH

 *******************************************************************************/

#if !defined( __AGL_SYMBOLIC_FUNCS__ )
#define __AGL_SYMBOLIC_FUNCS__

/*******************************************************************************

 Deklaration der Funktionen

 *******************************************************************************/

#if defined( __cplusplus )
  extern "C" {
#endif

//
// 300/400, 1200, 1500 Symbolik
//

// ******************************
//*******************************
// Lesen der Daten von der SPS
//*******************************
// Eingabe:
//   ConnNr - Verbindungsnummer von AGL_PLCConnect oder AGL_PLCConnectEx
//   SymbolicRW - Zeiger auf 1-n SymbolicRW Strukturen mit AccesHandle und Lesepufferinformation
//   Num - Anzahl der SymbolicRW Strukturen
//   Timeout - Maximale Wartezeit auf eine Anfrage
//   UserVal - Benutzerspezifischer Parameter zur Kennung bei z.b. asynchronen Aufrufen.
// Ausgabe
//   SError - Bei einem Lesenfehler wird hier der interne Siemensfehlercode abgelegt (darf NULL sein wenn nicht gebraucht)
agl_int32_t AGL_API AGL_Symbolic_ReadMixEx( const agl_int32_t ConnNr, SymbolicRW_t* const SymbolicRW, const agl_int32_t Num, agl_int32_t* const SError, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

// *******************************
// Schreiben der Daten in die SPS
// *******************************
// Eingabe:
//   ConnNr - Verbindungsnummer von AGL_PLCConnect oder AGL_PLCConnectEx
//   SymbolicRW - Zeiger auf 1-n SymbolicRW Strukturen mit AccesHandle und Schreibpufferinformation
//   Num - Anzahl der SymbolicRW Strukturen
//   Timeout - Maximale Wartezeit auf eine Anfrage
//   UserVal - Benutzerspezifischer Parameter zur Kennung bei z.b. asynchronen Aufrufen.
// Ausgabe
//   SError - Bei einem Schreibfehler wird hier der interne Siemensfehlercode abgelegt (darf NULL sein wenn nicht gebraucht)
agl_int32_t AGL_API AGL_Symbolic_WriteMixEx( const agl_int32_t ConnNr, SymbolicRW_t* const SymbolicRW, const agl_int32_t Num, agl_int32_t* const SError, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

// *******************************
// Öffnen eines TIA Projektes.
// Ausgangspunkt für alle weiteren Funktionen.
// *******************************
// Eingabe:
//  ProjectFile - Pfad auf eine TIA Projektdatei (.ap*, .als*)
//  AutoExpand - Wenn dieser Parameter auf 1 steht werden Kind-Knoten von Arrays automatisch bei Zugriff expandiert - ansonsten muss die Funktion AGL_Symbolic_Expand verwendet werden
//  Große Arrays mit vielen Knoten (Hundertausende, Millionen) können mit aktiviertem AutoExpand und einem direkten Knotenzugriff auf solche Arrays zu einem sofortigem sehr hohem Speicherverbrauch und Ladezeit führen.
// Ausgabe:
//  RootNodeHandle - Handle auf den Root-Knoten des Symbolbaumes - das ist der Startpunkt für alle Knoten Funktionen
agl_int32_t AGL_API AGL_Symbolic_LoadTIAProjectSymbols(const agl_cstr8_t const ProjectFile, HandleType* const RootNodeHandle, const agl_int32_t AutoExpand);

// *******************************
// Öffnen eines TIA Projektes mit Filter
// Ausgangspunkt für alle weiteren Funktionen.
// *******************************
// Eingabe:
//  ProjectFile - Pfad auf eine TIA Projektdatei ab TIA Portal V14 (.ap*, .als*)
//  SymbolFilter   - Parameter wird in dieser Version noch nicht verwendet
//  Flags          - Zusaetzliche Filter Flags. Kombinationen aus define TIA_FILTER_FLAGS_* (AGL_Defines.h)
//  ErrorLine      - Parameter wird in dieser Version noch nicht verwendet
//  ErrorPos       - Parameter wird in dieser Version noch nicht verwendet
// Ausgabe:
//  RootNodeHandle - Handle auf den Root-Knoten des Symbolbaumes - das ist der Startpunkt für alle Knoten Funktionen
agl_int32_t AGL_API AGL_Symbolic_LoadTIAProjectSymbolsWithFilter(const agl_cstr8_t const ProjectFile, HandleType* const RootNodeHandle, const agl_cstr8_t const SymbolFilter, const agl_uint32_t Flags, agl_int32_t* const ErrorLine, agl_int32_t* const ErrorPos);


// *******************************
// Laden von Symbolen aus einer PLC
// *******************************
// Eingabe:
//  ConnNr - aktive Verbindung zu einer S7-1200(FW 2.2-4.x) oder S7-1500
// Ausgabe:
//  RootNodeHandle - Handle auf den Root-Knoten des Symbolbaumes
// Hinweis: bei Verwendung dieser Funktion heisst bei Symbol-Pfadangaben der SPS-Knoten immer 'PLC'. Beispiel: PLC.Blocks.DB_Name.VariablenName
//
agl_int32_t AGL_API AGL_Symbolic_LoadAGLinkSymbolsFromPLC(const agl_int32_t ConnNr, HandleType* const RootNodeHandle);

// *******************************
// Laden von Symbolen aus einer PLC
// *******************************
// Eingabe:
//  ConnNr - aktive Verbindung zu einer S7-1200(FW 2.2-4.x) oder S7-1500
// Ausgabe:
//  RootNodeHandle - Handle auf den Root-Knoten des Symbolbaumes
// Hinweis: Im Gegensatz zur Funktion AGL_Symbolic_LoadAGLinkSymbolsFromPLC wird bei dieser Funktion der Projektierte SPS Name verwendet
//
agl_int32_t AGL_API AGL_Symbolic_LoadAGLinkSymbolsFromPLCEx(const agl_int32_t ConnNr, HandleType* const RootNodeHandle, const agl_uint32_t Flags);

// *******************************
// Speichern von Symbolen in einer AGLink Symbolformat Datei
// *******************************
// Eingabe:
//  RootNodeHandle - Handle auf den Root-Knoten des Symbolbaumes
//  TIASymbolsFile - Datei in welche die Symboldaten gespeichert werden
//  Hinweis: Der Zielorder muss bereits existieren und wird nicht automatisch erstellt
agl_int32_t AGL_API AGL_Symbolic_SaveAGLinkSymbolsToFile(const HandleType RootNodeHandle, const agl_cstr8_t const AGLinkSymbolsFile);

// *******************************
// Speichern von Symbolen in einer AGLink Symbolformat Datei mit Filter
// *******************************
// Eingabe:
//  RootNodeHandle - Handle auf den Root-Knoten des Symbolbaumes
//  TIASymbolsFile - Datei in welche die Symboldaten gespeichert werden
//  SymbolFilter   - Filter fuer PLCs/DBs/UDTs/Tags/Symbole. Angabe wie Symbolpfad (UTF-8), getrennt mit Newline. Siehe Dokumentation.
//  Flags          - Zusaetzliche Filter Flags. Kombinationen sind moeglich. Siehe define AGL_FILTER_FLAGS* (AGL_Defines.h)
//  ErrorLine      - Bei einem fehlerhaften Filterstring steht hier die Zeile des Fehlers. Zeile beginnt bei 0
//  ErrorPos       - Bei einem fehlerhaften Filterstring steht hier die Position des Fehlers. Position beginnt bei 0
//  Hinweis: Der Zielorder muss bereits existieren und wird nicht automatisch erstellt
agl_int32_t AGL_API AGL_Symbolic_SaveAGLinkSymbolsToFileWithFilter(const HandleType RootNodeHandle, const agl_cstr8_t const AGLinkSymbolsFile, const agl_cstr8_t const SymbolFilter, const agl_uint32_t Flags, agl_int32_t* const ErrorLine, agl_int32_t* const ErrorPos);

// *******************************
// Laden von Symbolen aus einer AGLink Symbolformat Datei
// *******************************
// Eingabe:
//  TIASymbolsFile - Datei aus welcher die Symboldaten geladen werden
// Ausgabe:
//  RootNodeHandle - Handle auf den Root-Knoten des Symbolbaumes
agl_int32_t AGL_API AGL_Symbolic_LoadAGLinkSymbolsFromFile(const agl_cstr8_t const AGLinkSymbolsFile, HandleType* const RootNodeHandle);

// *******************************
// Gibt den Speicher für den angegebenen Knoten frei.
// Wird der Wurzelknoten angegeben wird das gesamte Projekt geschlossen.
// Beachten Sie, dass evtl erstelle AccessHandles separat freigegeben werden müssen.
// *******************************
// Eingabe:
//   Handle - Kann ein NodeHandle oder AccessHandle sein
agl_int32_t AGL_API AGL_Symbolic_FreeHandle(const HandleType Handle);

// *******************************
// Ermittelt die Anzahl der direkten Kindknoten des gegebenen Ausgangsknotens
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum dessen Kinderanzahl ausgegeben werden soll
// Ausgabe:
//   ChildCount - Anzahl der Kinder
agl_int32_t AGL_API AGL_Symbolic_GetChildCount(const HandleType NodeHandle, agl_int32_t* const ChildCount);

// *******************************
// Ermittelt den X. direkten Kindknoten des Ausgangsknotens
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum von dem ein Kind-Knoten geliefert werden soll
//   ChildIndex - Nr (0...AGL_Symbolic_GetChildCount-1) des zu liefernden Kind-Knotens
// Ausgabe:
//   ChildNodeHandle - Handle von dem Kind-Knoten
agl_int32_t AGL_API AGL_Symbolic_GetChild(const HandleType NodeHandle, const agl_int32_t ChildIndex, HandleType* const ChildNodeHandle);

// *******************************
// Zugriff auf einen Kindknoten anhand seiner Bezeichnung.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum von dem ein Kind-Knoten geliefert werden soll
//   ChildName - Name des zu liefernden Kind-Knotens
// Ausgabe:
//   ChildNodeHandle - Handle von dem Kind-Knoten
agl_int32_t AGL_API AGL_Symbolic_GetChildByName(const HandleType NodeHandle, const agl_cstr8_t const ChildName, HandleType* const ChildNodeHandle);

// *******************************
// Ermittelt die Bezeichnung des Knotens
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
//   NameBuffer - Puffer fuer den Namen
//   NameBufferLen - Länge des Puffers
// Ausgabe:
//   NameLen - Länge des Namens
agl_int32_t AGL_API AGL_Symbolic_GetName(const HandleType NodeHandle, agl_cstr8_t const NameBuffer, const agl_int32_t NameBufferLen, agl_int32_t* const NameLen);

// *******************************
// Liefert den lokalen Offset des Knotens
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   LocalByteOffset - Lokaler Byte Offset
//   LocalBitOffset - Lokaler Bit Offset
agl_int32_t AGL_API AGL_Symbolic_GetLocalOffset(const HandleType NodeHandle, agl_uint32_t* const LocalByteOffset, agl_uint32_t* const LocalBitOffset);

// *******************************
// Bestimmt den notwendigen SPS-Systemdatentyp
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   SystemType - Systemtyp z.B. auf einer SPS, z.b. S7-Time, S7-Bool, DTL.
agl_int32_t AGL_API AGL_Symbolic_GetSystemType(const HandleType NodeHandle, SystemType_t::enum_t* const SystemType);

// *******************************
// Bestimmt ob ein Knoten eine Struktur, Array oder Einzelelement ist.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   HierarchyType - Der Hierarchytyp des Knotens
agl_int32_t AGL_API AGL_Symbolic_GetHierarchyType(const HandleType NodeHandle, HierarchyType_t::enum_t* const HierarchyType);

// *******************************
// Ermittelt die Anzahl der Dimensionen eines Arrays. Z.b. Array[1..2] of agl_int32_t => 1. Array[1..2, 1..2] of agl_int32_t => 2.
// *******************************
// Eingabe:
//   NodeHandle - Arrayknoten im Baum
// Ausgabe:
//   DimensionCount - Anzahl der Dimensionen
agl_int32_t AGL_API AGL_Symbolic_GetArrayDimensionCount(const HandleType ArrayNodeHandle, agl_int32_t* const DimensionCount);

// *******************************
// Ermittelt für eine Arraydimension den unterne und oberen Indexwert. Array[1..20] of agl_int32_t => {1, 20}. Array[-1..20, 1..10] of agl_int32_t => für Dimension 0 = {-1, 20}, bzw. für Dimension 1 = {1, 10}
// *******************************
// Eingabe:
//   ArrayNodeHandle - Arrayknoten im Baum
//   Dimension - Der Dimensionsindex
// Ausgabe:
//   Lower - Untereer Indexwert
//   Upper - Oberer Indexwert
agl_int32_t AGL_API AGL_Symbolic_GetArrayDimension(const HandleType ArrayNodeHandle, const agl_int32_t Dimension, agl_int32_t* const Lower, agl_int32_t* const Upper);

// *******************************
// Ermittelt die maximal erlaubte Anzahl an Zeichen eines S7-Strings, S7-WStrings, etc. 
// *******************************
// Eingabe:
//   StringNodeHandle - Knoten im Baum
// Ausgabe:
//   StringSize - Maximale Stringlänge, Anzahl Zeichen.
agl_int32_t AGL_API AGL_Symbolic_GetMaxStringSize(const HandleType StringNodeHandle, agl_int32_t* const StringSize); // -1 means unlimited

// *******************************
// Bestimmt den Datentyp, der für die Abbildung des Systemdatentyps der Steuerung auf dem "PC" notwendig ist. z.b S7-Bool => UInt8, S7-Int => Int16 und ULInt => UInt64.
// Typen die nicht direkt auf Werttypen abgebildet werden können, werden als "SystemSpecific" geliefert. Z.B. S7-DTL. Für diese gibt es gesonderte Konverter.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   ValueType - Der Werttyp
agl_int32_t AGL_API AGL_Symbolic_GetValueType(const HandleType NodeHandle, ValueType_t::enum_t* const ValueType);

// *******************************
// Liefert den Zustand des Symbols aus dem Projekt zurück. D.h. Nutzbar, nicht Nutzbar, Fehlerhaft, etc.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   TypeState - Der Zustand des Symbols
agl_int32_t AGL_API AGL_Symbolic_GetTypeState(const HandleType NodeHandle, TypeState_t::enum_t* const TypeState);

// *******************************
// Differenziert die Knotenarten. Handelt es sich dabei um ein normales Feld, einen Index in einem Array oder ist es der Root-Knoten.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   SegementType - Die Knotenart
agl_int32_t AGL_API AGL_Symbolic_GetSegmentType(const HandleType NodeHandle, SegmentType_t::enum_t* const SegementType);

// *******************************
// Gibt zurück ob ein Knoten Gelesen und/oder Geschrieben werden kann
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   PermissionType - Die Zugriffsmöglichkeiten auf diesen Knoten
agl_int32_t AGL_API AGL_Symbolic_GetPermissionType(const HandleType NodeHandle, PermissionType_t::enum_t* const PermissionType);

// *******************************
// Maskiert Sonderzeichen in Elementbezeichnungen, wie  . ( ) [ ] und "
// Diese Funktion erwartet nur den Elementnamen, nicht den gesamten Pfad. Da sonst die Ebenentrennpunkte ebenfalls maskiert werden.
// *******************************
// Eingabe:
//   RawString - Roher Text zur Maskierung   
//   EscapedStringMaxSize - Maximale Anzahl an Zeichen, die in den Ergebnispuffer kopiert warden dürfen
// Ausgabe:
//   EscapedString - Variable für maskierten Text
//   ErrorPosition - Bei Maskierungsfehler, die Position im Text an der der Fehler auftrat
agl_int32_t AGL_API AGL_Symbolic_EscapeString(const agl_cstr8_t const RawString, agl_cstr8_t const EscapedString, const agl_int32_t EscapedStringMaxSize, agl_int32_t* const ErrorPosition);

// *******************************
// Zugriff auf einen Kindknoten über den vollständigen Knotennamen, wenn als NodeHandle der Root mitgegegebn wird. Z.B. PLC_1.Blocks.Datenblock_1.ElementX
// Alternativ kann ein beliebiger Knoten übergeben werden und dann der Zugriff auf diesem Erfolgen. Z.B. Wenn das NodeHandle "PLC_1.Blocks" entspräche, dann wäre der Kindname "Datenblock_1.ElementX"
// *******************************
// Eingabe:
//   NodeHandle - Startknoten
//   ItemPath - Pfad zum gewünschten Element ausgehend vom Startknoten (Hinweis: Maskierungsregeln siehe AGL_Symbolic_EscapeString())
// Ausgabe:
//   FoundNodeHandle - Handle auf das gefundene Element
//   ErrorPosition  - Im Feherfall die Position im Text an der der Fehler auftrat
agl_int32_t AGL_API AGL_Symbolic_GetNodeByPath(const HandleType NodeHandle, const agl_cstr8_t const ItemPath, HandleType* const FoundNodeHandle, agl_int32_t* const ErrorPosition);

// *******************************
// Ermittelt für einen systemspezifischen Arrayindexzugriff die Dimensionsanzahl. (Gleiches Ergebnis wie bei AGL_Symbolic_GetArrayDimensionCount). Index = [1,4,2] => 3, [1,6] => 2
// Jeder Index eines Arrays liefert dieselbe Dimensionsanzahl.
// *******************************
// Eingabe:
//   IndexNodeHandle - Indexknoten
// Ausgabe:
//   IndexSize - Die Dimensionsanzahl
agl_int32_t AGL_API AGL_Symbolic_GetIndexSize(const HandleType IndexNodeHandle, agl_size_t* const IndexSize);

// *******************************
// Ermittelt den Systemspezifischen Wert der Indexkomponete. Z.b für [1,2,4] die einzelne 1, die 2 oder die 4.
// *******************************
// Eingabe:
//   IndexNodeHandle - Indexknoten
//   Element - 0-Basierte Nummer innerhalb der Indexkomponente.
// Ausgabe:
//   Value - Wert der Indexkomponente
agl_int32_t AGL_API AGL_Symbolic_GetIndex(const HandleType IndexNodeHandle, const agl_int32_t Element, agl_int32_t* const Value);

// *******************************
// Ermittelt den Linearen (0-n) Index des Indexknotens.
// *******************************
// Eingabe:
//   IndexNodeHandle - Indexknoten
// Ausgabe:
//   Value - Linearindex
agl_int32_t AGL_API AGL_Symbolic_GetLinearIndex(const HandleType IndexNodeHandle, agl_size_t* const Value);

// *******************************
// Ermittelt die Anzahl an Elementen im Array.
// *******************************
// Eingabe:
//   ArrayNodeHandle - Arrayknoten
// Ausgabe:
//   ElementCount - Anzahl der Elemente
agl_int32_t AGL_API AGL_Symbolic_GetArrayElementCount(const HandleType ArrayNodeHandle, agl_int32_t* const ElementCount);

// *******************************
// Expandiert einen Knoten und lädt dessen Kindelemente. Notwendig vor Arrayzugriffen, sofern AutoExpand beim Aufruf von AGL_Symbolic_LoadTIAProjectSymbols deaktiviert (0) wurde. 
// Man kann definieren, wie viele Ebenen nach unten expandiert werden.
// *******************************
// Eingabe:
//   NodeHandle - Knotem im Baum
//   Depth - Zu expandierende Tiefe. 0 = Nur die Kinder des angegebenen Knotens, -1 = Vollständig
agl_int32_t AGL_API AGL_Symbolic_Expand(const HandleType NodeHandle, const agl_int32_t Depth);

// *******************************
// Kollabiert einen Knoten und gibt den Speicher geladenen Kindelemente frei.
// *******************************
// Eingabe:
//   NodeHandle - Knotem im Baum
agl_int32_t AGL_API AGL_Symbolic_Collapse(const HandleType NodeHandle);

// *******************************
// Ermittelt den Bereich in dem sich der Knoten befindet. z.b: S7_Datablock, S7_Tag_Table oder S7_UDT.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   SystemType - Der Bereich in dem sich der Knoten befindet
agl_int32_t AGL_API AGL_Symbolic_GetSystemScope(const HandleType NodeHandle, SystemType_t::enum_t* const SystemType);

// *******************************
// Liefert den Systemzustand des Symbols aus dem Projekt zurück. D.h. Nutzbar, nicht Nutzbar, Fehlerhaft, etc.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   TypeState - Der Zustand des Symbols
agl_int32_t AGL_API AGL_Symbolic_GetSystemTypeState(const HandleType NodeHandle, SystemTypeState_t::enum_t* const SystemTypeState);

// *******************************
// Erzeugt einen Zugriffshandle, über den Daten geschrieben/gelesen werden können.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
agl_int32_t AGL_API AGL_Symbolic_CreateAccess(const HandleType NodeHandle, HandleType* const AccessHandle);

// *******************************
// Erzeugt einen Zugriffshandle anhand des Elementpfades, wenn als NodeHandle der Root mitgegegebn wird. Z.B. PLC_1.Datablocks.Datenblock_1.ElementX.
// Alternativ kann ein beliebiger Knoten übergeben werden und dann der Zugriff auf diesem Erfolgen. Z.B. Wenn das NodeHandle "PLC_1.Datablocks" entspräche, dann wäre der Kindname "Datenblock_1.ElementX"
// Funktionalität vergleichbar mit AGL_Symbolic_CreateAccess.
// *******************************
// Eingabe:
//   ParentNodeHandle - Knoten im Baum
//   ItemPath - Pfad zum gewünschten Element ausgehend vom Startknoten (Hinweis: Maskierungsregeln siehe AGL_Symbolic_EscapeString())
// Ausgabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
//   ErrorPosition  - Im Feherfall die Position im Text an der der Fehler auftrat
agl_int32_t AGL_API AGL_Symbolic_CreateAccessByPath(const HandleType ParentNodeHandle, const agl_cstr8_t const ItemPath, HandleType* const AccessHandle, agl_int32_t* const ErrorPosition);

// *******************************
// Füllt eine DATA_RW40-Struktur um mit PUT/GET auf nicht optimierte Datenblöcke oder Tags zuzugreifen
// Hinweis: DataRW.Buff wird nicht gesetzt und muss noch allokiert werden 
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   DataRW - Zeiger auf eine leere DATA_RW-Struktur
//   Size - Zeiger für Variable für benötigte Puffergröße
agl_int32_t AGL_API AGL_Symbolic_Get_DATA_RW40(const HandleType NodeHandle, DATA_RW40* const DataRW, agl_int32_t* const Size);

// *******************************
// Erzeugt eine DATA_RW40-Struktur anhand des Elementpfades, wenn als NodeHandle der Root mitgegegebn wird. Z.B. PLC_1.Datablocks.Datenblock_1.ElementX.
// Alternativ kann ein beliebiger Knoten übergeben werden und dann der Zugriff auf diesem Erfolgen. Z.B. Wenn das NodeHandle "PLC_1.Datablocks" entspräche, dann wäre der Kindname "Datenblock_1.ElementX"
// Funktionalität vergleichbar mit AGL_Symbolic_Get_DATA_RW40.
// Hinweis: DataRW.Buff wird nicht gesetzt und muss noch allokiert werden 
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
//   ItemPath - Pfad zum gewünschten Element ausgehend vom Startknoten (Hinweis: Maskierungsregeln siehe AGL_Symbolic_EscapeString())
// Ausgabe:
//   DataRW - Zeiger auf eine leere DATA_RW-Struktur
//   ErrorPosition  - Im Feherfall die Position im ItemPath an der der Fehler auftrat
//   Size - Zeiger für Variable für benötigte Puffergröße
agl_int32_t AGL_API AGL_Symbolic_Get_DATA_RW40_ByPath(const HandleType NodeHandle, const agl_cstr8_t const ItemPath, DATA_RW40* const DataRW, agl_int32_t* const ErrorPosition, agl_int32_t* const Size);
  
// *******************************
// Ermittelt zu einem komplexen Knoten, z.B. Array oder UDT, das Handle auf den Basistypknoten.
// Z.b. bei einem Array of struct den Structknoten. Bei einer UDT-Instanz den UDT Knoten. Bei einem S7-Memory Tag auf Bool den Bool.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   TypeNodeHandle - Handle auf den Basistypknoten des Elements.
agl_int32_t AGL_API AGL_Symbolic_GetType(const HandleType NodeHandle, HandleType* const TypeNodeHandle);

// *******************************
// Ermittelt wieviel Speicher in Bytes zum Lesen oder zum Schreiben des Zugriffes benötigt wird. Für Array[1..2] of S7-DWORD z.B. 8 Bytes = 2 x sizoef(S7-DWORD).
// *******************************
// Eingabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
// Ausgabe:
//   BufferSize - Benötigte Größe in Bytes
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferSize(const HandleType AccessHandle, agl_int32_t* const BufferSize);

// *******************************
// Ermittelt die Bytegröße eines Elements im Zugriffspuffer. Für ein Element aus dem Array[1..2] of S7-DWORD z.B. 4 Bytes.
// *******************************
// Eingabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
// Ausgabe:
//   ElementSize - Benötigte Größe in Bytes für ein Element
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferElementSize(const HandleType AccessHandle, agl_int32_t* const ElementSize);

// *******************************
// Ermittelt die Zeichenanzahl eines Strings.
// *******************************
// Eingabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
// Ausgabe:
//   ElementSize - Benötigte Größe in Bytes für ein Element
agl_int32_t AGL_API AGL_Symbolic_GetAccessStringSize(const HandleType AccessHandle, agl_int32_t* const StringSize);

// *******************************
// Wieviele Elemente kommen fuer diesen Access z.B. bei Array oder Range-Zugriff koennen es mehr als 1 Element sein.
// *******************************
// Eingabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
// Ausgabe:
//   ElementCount - Anzahl Elemente
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferElementCount(const HandleType AccessHandle, agl_int32_t* const ElementCount);

// *******************************
// Ermittelt den Systemtyp des Elements
// *******************************
// Eingabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
// Ausgabe:
//   SystemType - Liefert den Systemtyp eines Elements
agl_int32_t AGL_API AGL_Symbolic_GetAccessElementSystemType(const HandleType AccessHandle, SystemType_t::enum_t* const SystemType);

// *******************************
// Ermittelt den Werttyp des Elements
// *******************************
// Eingabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
// Ausgabe:
//   SystemType - Liefert den Werttyp eines Elements
agl_int32_t AGL_API AGL_Symbolic_GetAccessElementValueType(const HandleType AccessHandle, ValueType_t::enum_t* const ValueType);

// *******************************
//Zusammenhang Index und Linearindex 
//Array[-1..2, 2..3] ergibt:

//Index <=> LinearIndex:
//[-1,2] <=> 0
//[-1,3] <=> 1
//[ 0,2] <=> 2 
//[ 0,3] <=> 3 
//[ 1,2] <=> 4
//[ 1,3] <=> 5
//[ 2,2] <=> 6
//[ 2,3] <=> 7
// *******************************

// *******************************
// Konvertiert zwischen Systemdarstellung eines Arrayindexes und des 0-n basierten Linearindexes.
// *******************************
// Eingabe:
//   ArrayNodeHandle - Arrayknoten
//   Index - Int-Array mit den Indexkomponenten
//   IndexCount - Anzahl der Indexkomponenten
// Ausgabe:
//   LinearIndex - Linearindex
agl_int32_t AGL_API AGL_Symbolic_GetArrayIndexAsLinearIndex(const HandleType ArrayNodeHandle, const agl_int32_t* const Index, const agl_int32_t IndexCount, agl_int32_t* const LinearIndex);

// *******************************
// Konvertiert zwischen Systemdarstellung eines Arrayindexes und des 0-n basierten Linearindexes.
// *******************************
// Eingabe:
//   ArrayNodeHandle - Arrayknoten
//   LinearIndex - Linearindex
// Ausgabe:
//   Index - Int-Array mit den Indexkomponenten
//   MaxIndexCount - Anzahl der Indexkomponenten im Indexpuffer
//   IndexCount - Anzahl der Indexkomponenten
agl_int32_t AGL_API AGL_Symbolic_GetArrayLinearIndexAsIndex(const HandleType ArrayNodeHandle, const agl_int32_t LinearIndex, agl_int32_t* const Index, const agl_int32_t MaxIndexCount, agl_int32_t* const IndexCount);

// *******************************
// Erzeugt ein Zugriffsobjekt auf ein Arrayelement anhand des Arrayknotens und dem übergebenen Linearindex.
// *******************************
// Eingabe:
//   ArrayNodeHandle - Arrayknoten
//   LinearIndex - Linearindex des Elements
// Ausgabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
agl_int32_t AGL_API AGL_Symbolic_CreateArrayAccessByLinearIndex(const HandleType ArrayNodeHandle, const agl_int32_t LinearIndex, HandleType* const AccessHandle);

// *******************************
//  Zugriff auf ein Arrayelementbereich über Linearenindex und Anzahl an Elementen ab Linearindex
// *******************************
// Eingabe:
//   ArrayNodeHandle - Arrayknoten
//   LinearIndex - Linearindex des Elements ab dem zugegriffen werden soll
//   Count - Anzahl der Elemente auf die zugegriffen wird
// Ausgabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
agl_int32_t AGL_API AGL_Symbolic_CreateArrayRangeAccessByLinearIndex(const HandleType ArrayNodeHandle, const agl_int32_t LinearIndex, const agl_int32_t Count, HandleType* const AccessHandle);

// *******************************
// Zugriff auf ein Arrayelement über einen Systemindex. Systemindex ist ein INT Array [Index1, Index2,...]. Länge entspricht der Dimension. Beispiel  [2,3] bei einem 2-Dimensionalen Array
// *******************************
// Eingabe:
//   ArrayNodeHandle - Arrayknoten
//   Index - Der Systemindex
//   IndexCount - Anzahl der Indexkomponenten
// Ausgabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
agl_int32_t AGL_API AGL_Symbolic_CreateArrayAccessByIndex(const HandleType ArrayNodeHandle, const agl_int32_t* Index, const agl_int32_t IndexCount, HandleType* const AccessHandle);

// *******************************
// Zugriff auf ein Arrayelement über Index. Index ist ein INT Array [Index1, Index2,...]. 
// *******************************
// Eingabe:
//   ArrayNodeHandle - Arrayknoten
//   Index - Der Systemindex ab dem zugegriffen werden soll
//   IndexCount - Anzahl der Indexkomponenten
//   Count - Anzahl der Elemente auf die zugegriffen wird
// Ausgabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
agl_int32_t AGL_API AGL_Symbolic_CreateArrayRangeAccessByIndex(const HandleType ArrayNodeHandle, const agl_int32_t* Index, const agl_int32_t IndexCount, const agl_int32_t Count, HandleType* const AccessHandle);


// *******************************
// Hilfsroutinen um Werte aus dem AccessBuffer zu lesen.
// *******************************
// Eingabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
//   Buffer - Datenbytebuffer
//   BufferLen - Datenbytebufferlänge - Entspricht der normalweise der AccessBufferSize
//   Element - Auf welches Element zugegriffen werden soll bei Array oder Ranges. Mit 0 beginnend. Bei einzelelementen muss mit 0 begonnen werden.
// Ausgabe:
//   Value - Werttyp
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferUInt8(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_uint8_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferUInt16(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_uint16_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferUInt32(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_uint32_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferUInt64(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_uint64_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferInt8(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_int8_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferInt16(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_int16_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferInt32(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_int32_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferInt64(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_int64_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferFloat32(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_float32_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferFloat64(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_float64_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferChar8(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_cstr8_t const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferChar16(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_char16_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferString8(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_cstr8_t const StringBuffer, const agl_int32_t MaxCharCount, agl_int32_t* const CharCount);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferString16(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_char16_t* const StringBuffer, const agl_int32_t MaxCharCount, agl_int32_t* const CharCount);
// Ausgabe: Systemtypspezifisch
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferS7_DTLParts(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_uint16_t* const Year, agl_uint8_t* const Month, agl_uint8_t* const Day, agl_uint8_t* const WeekDay, agl_uint8_t* const Hour, agl_uint8_t* const Minute, agl_uint8_t* const Second, agl_uint32_t* const Nanoseconds);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferS7_S5TimeParts(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_uint16_t* const TimeBase, agl_uint16_t* const TimeValue);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferS7_S5TimeMs(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_uint32_t* const Milliseconds);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferS7_Counter(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_uint16_t* const Value);
agl_int32_t AGL_API AGL_Symbolic_GetAccessBufferS7_Date_and_TimeParts(const HandleType AccessHandle, const void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, agl_uint16_t* const Year, agl_uint8_t* const Month, agl_uint8_t* const Day, agl_uint8_t* const WeekDay, agl_uint8_t* const Hour, agl_uint8_t* const Minute, agl_uint8_t* const Second, agl_uint16_t* const Millisecond);

//----------------------

// *******************************
// Hilfsroutinen um Werte in den AccessBuffer zu schreiben.
// *******************************
// Eingabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
//   Buffer - Datenbytebuffer
//   BufferLen - Datenbytebufferlänge - Entspricht der normalweise der AccessBufferSize
//   Element - Auf welches Element zugegriffen werden soll bei Array oder Ranges. Mit 0 beginnend. Bei einzelelementen muss mit 0 begonnen werden.
//   Value - Werttyp
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferUInt8(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_uint8_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferUInt16(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_uint16_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferUInt32(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_uint32_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferUInt64(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_uint64_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferInt8(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_int8_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferInt16(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_int16_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferInt32(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_int32_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferInt64(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_int64_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferFloat32(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_float32_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferFloat64(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_float64_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferChar8(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_char8_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferChar16(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_char16_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferString8(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_cstr8_t const StringBuffer, const agl_int32_t CharCount);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferString16(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_char16_t* const StringBuffer, const agl_int32_t CharCount);

// Eingabe: Systemtypspezifisch
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferS7_DTLParts(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_uint16_t Year, const agl_uint8_t Month, const agl_uint8_t Day, const agl_uint8_t WeekDay, const agl_uint8_t Hour, const agl_uint8_t Minute, const agl_uint8_t Second, const agl_uint32_t Nanoseconds);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferS7_S5TimeParts(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_uint16_t TimeBase, const agl_uint16_t TimeValue);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferS7_S5TimeMs(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_uint32_t Milliseconds, const agl_int32_t Round);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferS7_Counter(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_uint16_t Value);
agl_int32_t AGL_API AGL_Symbolic_SetAccessBufferS7_Date_and_TimeParts(const HandleType AccessHandle, void* const Buffer, const agl_long32_t BufferLen, const agl_int32_t Element, const agl_uint16_t Year, const agl_uint8_t Month, const agl_uint8_t Day, const agl_uint8_t WeekDay, const agl_uint8_t Hour, const agl_uint8_t Minute, const agl_uint8_t Second, const agl_uint16_t Milliseconds);

// Kommentare

// *******************************
// Gibt für das Handle des Projektwurzelelementes den eingestellten Ländercode der Kommentare zurück.
// *******************************
// Eingabe:
//   RootNodeHandle - Wurzelknoten des Projektes. Handle, welches nach Aufruf von AGL_Symbolic_LoadTIAProjectSymbols oder AGL_Symbolic_LoadAGLinkSymbolsFromFile erzeugt wird.
// Ausgabe:
//   LCID - Ländercode der eingestellten Sprache z.B. 1031 für Deutschland, 1033 für Vereinigte Staaten, etc.
agl_int32_t AGL_API AGL_Symbolic_GetProjectEditingCulture(const HandleType RootNodeHandle, agl_int32_t* const LCID);
// *******************************
// Gibt den Ländercode der Referenzsprache des TIA Portal Projektes zurück.
// *******************************
// Eingabe:
//   RootNodeHandle - Wurzelknoten des Projektes. Handle, welches nach Aufruf von AGL_Symbolic_LoadTIAProjectSymbols oder AGL_Symbolic_LoadAGLinkSymbolsFromFile erzeugt wird.
// Ausgabe:
//   LCID - Ländercode der Referenzsprache z.B. 1031 für Deutschland, 1033 für Vereinigte Staaten, etc.
agl_int32_t AGL_API AGL_Symbolic_GetProjectReferenceCulture(const HandleType RootNodeHandle, agl_int32_t* const LCID);
// *******************************
// Ermittelt die Anzahl an Sprachen, die im Projekt parametriert sind.
// *******************************
// Eingabe:
//   RootNodeHandle - Wurzelknoten des Projektes. Handle, welches nach Aufruf von AGL_Symbolic_LoadTIAProjectSymbols oder AGL_Symbolic_LoadAGLinkSymbolsFromFile erzeugt wird.
// Ausgabe:
//   Count - Die Anzahl an Sprachen, die im Projekt parametriert ist
agl_int32_t AGL_API AGL_Symbolic_GetProjectCultureCount(const HandleType RootNodeHandle, agl_int32_t* const Count);
// *******************************
// Gibt den Ländercode an der übergebenen Stelle zurück.
// *******************************
// Eingabe:
//   RootNodeHandle - Wurzelknoten des Projektes. Handle, welches nach Aufruf von AGL_Symbolic_LoadTIAProjectSymbols oder AGL_Symbolic_LoadAGLinkSymbolsFromFile erzeugt wird.
//   CultureIndex - Linearer Index (0 basiert) des gewünschten Ländercodes.
// Ausgabe:
//   LCID - Ländercode z.B. 1031 für Deutschland, 1033 für Vereinigte Staaten, etc.
agl_int32_t AGL_API AGL_Symbolic_GetProjectCulture(const HandleType RootNodeHandle, const agl_int32_t CultureIndex, agl_int32_t* const LCID);
// *******************************
// Gibt das Kommentar zum übergebenen Knoten, welches über den übergebenen Ländercode erreichbar ist zurück.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
//   LCID - Ländercode z.B. 1031 für Deutschland, 1033 für Vereinigte Staaten, etc. Wird dieser nicht gefunden, wird versucht den Standartkommentar (LCID = -1) zu laden
//   CommentMaxSize - Maximal verfügbare Puffergröße für das Kommentar
// Ausgabe:
//   Comment - Kommentar zum übergebenen Knoten
//   UsedByteCount - Anzahl gelesener Bytes
agl_int32_t AGL_API AGL_Symbolic_GetComment(const HandleType NodeHandle, const agl_int32_t LCID, agl_cstr8_t const Comment, const agl_int32_t CommentMaxSize, agl_int32_t* const UsedByteCount);
//*******************************
// Ermittelt die Anzahl an Sprachen, die für den übergebenen Knoten verfügbar sind.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   Count - Die Anzahl an Sprachen
agl_int32_t AGL_API AGL_Symbolic_GetCommentCultureCount(const HandleType NodeHandle, agl_int32_t* const Count);
// *******************************
// Gibt den Ländercode an der übergebenen Stelle zurück.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
//   CultureIndex - Linearer Index (0 basiert) des gewünschten Ländercodes.
// Ausgabe:
//   LCID - Ländercode z.B. 1031 für Deutschland, 1033 für Vereinigte Staaten, etc.
agl_int32_t AGL_API AGL_Symbolic_GetCommentCulture(const HandleType NodeHandle, const agl_int32_t CultureIndex, agl_int32_t* const LCID);

// *******************************
// Liefert fuer einen Datenblock-Knoten die Datenblock-Nummer zurueck
// *******************************
// Eingabe:
//   NodeHandle - Datenblock-Knoten im Baum
// Ausgabe:
//   Number - Datenblock-Nummer
agl_int32_t AGL_API AGL_Symbolic_DatablockGetNumber(const HandleType NodeHandle, agl_int32_t* const Number);

// *******************************
// Liefert fuer einen Datenblock-Knoten, ob dieser Optimiert und damit nur Symbolischer Zugriff erlaubt ist, zurueck
// *******************************
// Eingabe:
//   NodeHandle - Datenblock-Knoten im Baum
// Ausgabe:
//   BooleanValue - Wert des Attributes
agl_int32_t AGL_API AGL_Symbolic_DatablockIsSymbolic(const HandleType NodeHandle, agl_int32_t* const BooleanValue);

// *******************************
// Liefert fuer einen Datenblock-Knoten den Typ zurueck
// *******************************
// Eingabe:
//   NodeHandle - Datenblock-Knoten im Baum
// Ausgabe:
//   BooleanValue - Wert des Attributes
agl_int32_t AGL_API AGL_Symbolic_DatablockGetType(const HandleType NodeHandle, DatablockTypes_t::enum_t* const DataBlockType);

// *******************************
// Liefert zu einem Knoten den vollen Pfad
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
//   PathBuffer - Zeiger auf Pfad Puffer
//   MaxPathBufferSize - Maximale groesse des Puffers
// Ausgabe:
//   UsedPathBufferSize - Anzahl verwendeter Bytes im Puffer
agl_int32_t AGL_API AGL_Symbolic_GetPath(const HandleType NodeHandle, agl_cstr8_t const PathBuffer, const agl_int32_t MaxPathBufferSize, agl_int32_t* const UsedPathBufferSize);

// *******************************
// Liefert zu einem Knoten den vollen Pfad mit passenden Escape-Zeichen fuer die ByPath-Routinen
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
//   PathBuffer - Zeiger auf Pfad Puffer
//   MaxPathBufferSize - Maximale groesse des Puffers
// Ausgabe:
//   UsedPathBufferSize - Anzahl verwendeter Bytes im Puffer
agl_int32_t AGL_API AGL_Symbolic_GetEscapedPath(const HandleType NodeHandle, agl_cstr8_t const PathBuffer, const agl_int32_t MaxPathBufferSize, agl_int32_t* const UsedPathBufferSize);

// *******************************
// Liefert zu einem Knoten den Wert des Attributes "Erreichbar aus HMI"
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   BooleanValue - Wert des Attributes
agl_int32_t AGL_API AGL_Symbolic_GetAttributeHMIAccessible(const HandleType NodeHandle, agl_int32_t* const BooleanValue);

// *******************************
// Liefert zu einem Knoten den Wert des Attributes "Sichtbar im HMI"
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   BooleanValue - Wert des Attributes
agl_int32_t AGL_API AGL_Symbolic_GetAttributeHMIVisible(const HandleType NodeHandle, agl_int32_t* const BooleanValue);

// *******************************
// Liefert zu einem Knoten den Wert des Attributes "Remanenz"
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   BooleanValue - Wert des Attributes
agl_int32_t AGL_API AGL_Symbolic_GetAttributeRemanent(const HandleType NodeHandle, agl_int32_t* const BooleanValue);

// *******************************
// Liefert zu einem S7-SPS Knoten den SPS Typ-Namen
// *******************************
// Eingabe:
//   NodeHandle - S7-SPS Knoten im Baum
// Ausgabe:
//   TypeNameBuffer - Typ der SPS
agl_int32_t AGL_API AGL_Symbolic_GetS7PlcTypeName(const HandleType NodeHandle, agl_cstr8_t const TypeNameBuffer, const agl_int32_t TypeNameBufferLen, agl_int32_t* const TypeNameLen);

// *******************************
// Liefert zu einem S7-SPS Knoten die Firmware-Kennung
// *******************************
// Eingabe:
//   NodeHandle - S7-SPS Knoten im Baum
// Ausgabe:
//   FirmwareBuffer - Firmware Kennung
agl_int32_t AGL_API AGL_Symbolic_GetS7PlcFirmware(const HandleType NodeHandle, agl_cstr8_t const FirmwareBuffer, const agl_int32_t FirmwareBufferLen, agl_int32_t* const FirmwareLen);

// *******************************
// Liefert zu einem S7-SPS Knoten die MLFB-Kennung
// *******************************
// Eingabe:
//   NodeHandle - S7-SPS Knoten im Baum
// Ausgabe:
//   MLFBBuffer - MLFB-Kennung
agl_int32_t AGL_API AGL_Symbolic_GetS7PlcMLFB(const HandleType NodeHandle, agl_cstr8_t const MLFBBuffer, const agl_int32_t MLFBBufferLen, agl_int32_t* const MLFBLen);

// *******************************
// Liefert zu einem S7-SPS Knoten die SPS-Familie
// *******************************
// Eingabe:
//   NodeHandle - S7-SPS Knoten im Baum
// Ausgabe:
//   S7PlcFamily - S7-SPS Familie (300_400,1200,1500)
agl_int32_t AGL_API AGL_Symbolic_GetS7PlcFamily(const HandleType NodeHandle, S7PlcFamily_t::enum_t* const S7PlcFamily);

agl_int32_t AGL_API AGL_Symbolic_SaveSingleValueAccessSymbolsToFile(const HandleType RootHandle, const agl_cstr8_t const SingleValueFilterFile, const agl_cstr8_t const LogFile, const agl_cstr8_t const AglinkSingleValueAccessSymbolFile);
agl_int32_t AGL_API AGL_Symbolic_LoadSingleValueAccessSymbolsFromFile(const agl_cstr8_t const AglinkSingleValueAccessSymbolsFile, HandleType* const SingleValueAccessSymbolsHandle);
agl_int32_t AGL_API AGL_Symbolic_CreateAccessFromSingleValueAccessSymbols(const HandleType SingleValueAccessSymbolsHandle, const agl_cstr8_t const Symbol, HandleType* const AccessHandle);
agl_int32_t AGL_API AGL_Symbolic_GetSingleValueAccessSymbolCount(const HandleType SingleValueAccessSymbolsHandle, agl_int32_t* const SymbolCount);
agl_int32_t AGL_API AGL_Symbolic_GetSingleValueAccessSymbolPath(const HandleType SingleValueAccessSymbolsHandle, const agl_int32_t Index, agl_cstr8_t const PathBuffer, const agl_int32_t PathBufferLen, agl_int32_t* const PathLen);

// *******************************
// Symbolic Alarm Funktionen
// *******************************

//Alarminformation aus Symboldaten extrahieren
agl_int32_t AGL_API AGL_Symbolic_FindFirstAlarmData(const HandleType PlcNodeHandle, agl_uint32_t* const AlarmNr);
agl_int32_t AGL_API AGL_Symbolic_FindNextAlarmData(const HandleType PlcNodeHandle, agl_uint32_t* const AlarmNr);
agl_int32_t AGL_API AGL_Symbolic_FindCloseAlarmData(const HandleType PlcNodeHandle);
agl_int32_t AGL_API AGL_Symbolic_GetAlarmData(const HandleType PlcNodeHandle, agl_uint32_t AlarmNr, const agl_int32_t Language, LPDATA_ALARM40_TIA Buff);
agl_int32_t AGL_API AGL_Symbolic_GetAlarmText(const HandleType PlcNodeHandle, const agl_uint32_t AlarmNr, const agl_int32_t Language, agl_cstr8_t const TextBuff, const agl_int32_t BuffLen, agl_int32_t* const NeededBuffLen);
agl_int32_t AGL_API AGL_Symbolic_GetAlarmInfo(const HandleType PlcNodeHandle, const agl_uint32_t AlarmNr, const agl_int32_t Language, agl_cstr8_t const TextBuff, const agl_int32_t BuffLen, agl_int32_t* const NeededBuffLen);
agl_int32_t AGL_API AGL_Symbolic_GetAlarmAddText(const HandleType PlcNodeHandle, const agl_uint32_t AlarmNr, agl_int32_t Index, const agl_int32_t Language, agl_cstr8_t const TextBuff, const agl_int32_t BuffLen, agl_int32_t* const NeededBuffLen);

//Alarminformation formatiert als Text
agl_int32_t AGL_API AGL_Symbolic_FormatAlarmMessage(const LPS7_ALARM_TIA AlarmData, const HandleType PlcNodeHandle, const agl_int32_t Language, const agl_cstr8_t const AlarmText, agl_cstr8_t const TextBuff, const agl_int32_t BuffLen, agl_int32_t* const NeededBuffLen);

//aktuell anstehende Alarme abfragen
agl_int32_t AGL_API AGL_Symbolic_ReadOpenMsg(const agl_int32_t ConnNr, LPS7_ALARM_TIA AlarmData, const agl_int32_t AlarmCount, agl_int32_t* const NeededAlarmCount, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal);

//an Alarm-Meldungen der SPS anmelden
agl_int32_t AGL_API AGL_Symbolic_InitAlarmMsg(const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal);
agl_int32_t AGL_API AGL_Symbolic_ExitAlarmMsg(const agl_int32_t ConnNr, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal);
agl_int32_t AGL_API AGL_Symbolic_GetAlarmMsg(const agl_int32_t ConnNr, LPS7_ALARM_TIA AlarmData, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal);

//Alarm quittieren
agl_int32_t AGL_API AGL_Symbolic_AckMsg(const agl_int32_t ConnNr, const agl_uint64_t CPUAlarmID, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal);

agl_int32_t AGL_API AGL_Symbolic_GetCurrentProtectionLevel(const agl_int32_t ConnNr, agl_uint32_t* const CurrentProtectionLevel, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal);


//
// Simotion Symbols (STI Export eines Simotion Scout Projektes)
//

// *******************************
// Öffnen einer STI Scout Exportdatei
// Ausgangspunkt für alle weiteren Funktionen.
// *******************************
// Eingabe:
//  STIFile - Pfad auf eine STI Scout Exportdatei
//  FlatArrays - Arrays werden flach ausgeprägt. (Zerlegen wird noch nicht unterstützt)
// Ausgabe:
//  RootNodeHandle - Handle auf den Root-Knoten des Symbolbaumes - das ist der Startpunkt für alle Knoten Funktionen
agl_int32_t AGL_API AGL_Simotion_LoadSTISymbols(const agl_cstr8_t const STIFile, HandleType* const RootNodeHandle, const agl_bool_t FlatArrays);

// *******************************
// Gibt den Speicher für den angegebenen Knoten frei.
// Wird der Wurzelknoten angegeben wird das gesamte Projekt geschlossen.
// Beachten Sie, dass evtl erstelle AccessHandles separat freigegeben werden müssen.
// *******************************
// Eingabe:
//   Handle - Kann ein NodeHandle oder AccessHandle sein
agl_int32_t AGL_API AGL_Simotion_FreeHandle(const HandleType RootNodeHandle);

// *******************************
// Ermittelt die Bezeichnung des Knotens
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
//   NameBuffer - Puffer fuer den Namen
//   NameBufferLen - Länge des Puffers
agl_int32_t AGL_API AGL_Simotion_GetName(const HandleType NodeHandle, agl_cstr8_t const NameBuffer, const agl_int32_t NameBufferLen);

// *******************************
// Bestimmt den ob es Struktur, Array oder Einzelelement ist.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   HierarchyType - Der Hierarchytyp des Knotens
agl_int32_t AGL_API AGL_Simotion_GetHierarchyType(const HandleType NodeHandle, HierarchyType_t::enum_t* const HierarchyType);

//*******************************
// Bestimmt den Datentyp, der für die Abbildung des Systemdatentyps der Steuerung auf dem "PC" notwendig ist.
// Typen die nicht direkt auf Werttypen abgebildet werden können, werden als "SystemSpecific" geliefert.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   ValueType - Der Werttyp
agl_int32_t AGL_API AGL_Simotion_GetValueType(const HandleType NodeHandle, ValueType_t::enum_t* const ValueType);


// *******************************
// Bestimmt den notwendigen SPS-Systemdatentyp
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   SystemType - Systemtyp z.B. auf einer Simotion, z.b. Time, DateAndTime, TOD.
agl_int32_t AGL_API AGL_Simotion_GetSystemType(const HandleType NodeHandle, SystemType_t::enum_t* const SystemType);


// *******************************
// Gibt zurück ob ein Knoten Gelesen und/oder Geschrieben werden kann
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   PermissionType - Die Zugriffsmöglichkeiten auf diesen Knoten
agl_int32_t AGL_API AGL_Simotion_GetPermissionType(const HandleType NodeHandle, PermissionType_t::enum_t* const PermissionType);

// *******************************
// Ermittelt die Anzahl der direkten Kindknoten des gegebenen Ausgangsknotens
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum dessen Kinderanzahl ausgegeben werden soll
// Ausgabe:
//   ChildCount - Anzahl der Kinder
agl_int32_t AGL_API AGL_Simotion_GetChildCount(const HandleType NodeHandle, agl_int32_t* const ChildCount);

// *******************************
// Ermittelt den X. direkten Kindknoten des Ausgangsknotens
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum von dem ein Kind-Knoten geliefert werden soll
//   ChildIndex - Nr (0...AGL_Symbolic_GetChildCount-1) des zu liefernden Kind-Knotens
// Ausgabe:
//   ChildNodeHandle - Handle von dem Kind-Knoten
agl_int32_t AGL_API AGL_Simotion_GetChild(const HandleType NodeHandle, const agl_int32_t ChildIndex, HandleType* const ChildNodeHandle);

// *******************************
// Zugriff auf einen Kindknoten über den vollständigen Knotennamen, wenn als NodeHandle der Root mitgegegebn wird. Z.B. PLC_1.Datablocks.Datenblock_1.ElementX
// Alternativ kann ein beliebiger Knoten übergeben werden und dann der Zugriff auf diesem Erfolgen. Z.B. Wenn das NodeHandle "PLC_1.Blocks" entspräche, dann wäre der Kindname "Datenblock_1.ElementX"
// *******************************
// Eingabe:
//   NodeHandle - Startknoten
//   ItemPath - Pfad zum gewünschten Element ausgehend vom Startknoten (Hinweis: Maskierungsregeln siehe AGL_Symbolic_EscapeString())
// Ausgabe:
//   FoundNodeHandle - Handle auf das gefundene Element
//   ErrorPosition  - Im Feherfall die Position im Text an der der Fehler auftrat
agl_int32_t AGL_API AGL_Simotion_GetNodeByPath(const HandleType NodeHandle, const agl_cstr8_t const ItemPath, HandleType* const FoundNodeHandle, agl_int32_t* const ErrorPosition);

// *******************************
// Erzeugt einen Zugriffshandle, über den Daten geschrieben/gelesen werden können.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
// Ausgabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
agl_int32_t AGL_API AGL_Simotion_CreateAccess(const HandleType NodeHandle, HandleType* const AccessHandle);

// *******************************
// Erzeugt einen Zugriffshandle anhand des Elementpfades, wenn als NodeHandle der Root mitgegegebn wird. Z.B. PLC_1.Blocks.Datenblock_1.ElementX.
// Alternativ kann ein beliebiger Knoten übergeben werden und dann der Zugriff auf diesem Erfolgen. Z.B. Wenn das NodeHandle "PLC_1.Blocks" entspräche, dann wäre der Kindname "Datenblock_1.ElementX"
// Funktionalität vergleichbar mit AGL_Symbolic_CreateAccess.
// *******************************
// Eingabe:
//   NodeHandle - Knoten im Baum
//   ItemPath - Pfad zum gewünschten Element ausgehend vom Startknoten (Hinweis: Maskierungsregeln siehe AGL_Symbolic_EscapeString())
// Ausgabe:
//   AccessHandle - Handle auf das Zugriffsobjekt
//   ErrorPosition  - Im Feherfall die Position im Text an der der Fehler auftrat
agl_int32_t AGL_API AGL_Simotion_CreateAccessByPath(const HandleType ParentNodeHandle, const agl_cstr8_t const ItemPath, HandleType* const AccessHandle, agl_int32_t* const ErrorPosition);

// *******************************
// Lesen der Daten von einer SIMOTION Steuerung
// *******************************
// Eingabe:
//   ConnNr - Verbindungsnummer von AGL_PLCConnect oder AGL_PLCConnectEx
//   Buff - Zeiger auf 1-n SymbolicRW Strukturen mit AccesHandle und Lesepufferinformation
//   Num - Anzahl der SymbolicRW Strukturen
//   Timeout - Maximale Wartezeit auf eine Anfrage
//   UserVal - Benutzerspezifischer Parameter zur Kennung bei z.b. asynchronen Aufrufen.
agl_int32_t AGL_API AGL_Simotion_ReadMixEx( const agl_int32_t ConnNr, SymbolicRW_t* const Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

// *******************************
// Schreiben der Daten auf eine SIMOTION Steuerung
// *******************************
// Eingabe:
//   ConnNr - Verbindungsnummer von AGL_PLCConnect oder AGL_PLCConnectEx
//   Buff - Zeiger auf 1-n SymbolicRW Strukturen mit AccesHandle und Schreibpufferinformation
//   Num - Anzahl der SymbolicRW Strukturen
//   Timeout - Maximale Wartezeit auf eine Anfrage
//   UserVal - Benutzerspezifischer Parameter zur Kennung bei z.b. asynchronen Aufrufen.
agl_int32_t AGL_API AGL_Simotion_WriteMixEx( const agl_int32_t ConnNr, SymbolicRW_t* const Buff, const agl_int32_t Num, const agl_int32_t Timeout, const agl_ptrdiff_t UserVal );

#if defined( __cplusplus )
  }
#endif

#endif  // #if !defined( __AGL_SYMBOLIC_FUNCS__ )
