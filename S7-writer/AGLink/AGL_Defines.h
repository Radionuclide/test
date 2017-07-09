/*******************************************************************************

 Projekt        : Neue Version der AGLink-Bibliothek

 Dateiname      : AGL_Defines.H

 Beschreibung   : Definition der Konstanten

 Copyright      : (c) 1998-2017
                  DELTALOGIC Automatisierungstechnik GmbH
                  Stuttgarter Str. 3
                  73525 Schwäbisch Gmünd
                  Web : http://www.deltalogic.de
                  Tel.: +49-7171-916120
                  Fax : +49-7171-916220

 Erstellt       : 23.03.2004  RH

 Geändert       : 04.01.2017  RH

 *******************************************************************************/

#if !defined( __AGL_DEFINES__ )
#define __AGL_DEFINES__

/*******************************************************************************

 Definition der Konstanten

 *******************************************************************************/

#if defined( __TIXI__ )

  #if defined( __MQX__ )
    #define MAX_DEVICES               2           // Maximale Anzahl Devices
    #define MAX_PLCS                  8           // Maximale Anzahl SPSen (Verbindungen) je Device
    #define ZERO_SIZE                 0           // Offene Arrays sind hier unbeliebt
  #else
    #define MAX_DEVICES             256           // Maximale Anzahl Devices
    #define MAX_PLCS                 16           // Maximale Anzahl SPSen (Verbindungen) je Device
    #define ZERO_SIZE                             // Offene Arrays sind hier möglich
  #endif

#elif defined( __WINCE__ ) || defined( AGLINK_SE )

  #define MAX_DEVICES               4             // Maximale Anzahl Devices
  #define MAX_PLCS                  8             // Maximale Anzahl SPSen (Verbindungen) je Device
  #define ZERO_SIZE                               // Offene Arrays sind hier möglich

#elif defined( __SOLARIS__ )

  #define MAX_DEVICES             256             // Maximale Anzahl Devices
  #define MAX_PLCS                  8             // Maximale Anzahl SPSen (Verbindungen) je Device
  #define ZERO_SIZE                 0             // Offene Arrays sind hier möglich - Längenangabe notwendig

#elif defined( __EMBEDDED_LINUX__ )

  #define MAX_DEVICES             256             // Maximale Anzahl Devices
  #define MAX_PLCS                  4             // Maximale Anzahl SPSen (Verbindungen) je Device
  #define ZERO_SIZE                               // Offene Arrays sind hier möglich

#elif defined( __OS9__ )

  #define MAX_DEVICES             256             // Maximale Anzahl Devices
  #define MAX_PLCS                  8             // Maximale Anzahl SPSen (Verbindungen) je Device
  #define ZERO_SIZE                 1             // Offene Arrays sind hier nicht möglich - Längenangabe >0 notwendig

#else

  #define MAX_DEVICES             256             // Maximale Anzahl Devices
  #define MAX_PLCS                 16             // Maximale Anzahl SPSen (Verbindungen) je Device
  #define ZERO_SIZE                               // Offene Arrays sind hier möglich

#endif

//
// Parameterarten für die verschiedenen Verbindungswege
//
#define TYPE_UNKNOWN                -1

#define TYPE_S7CONN_IE              0
#define TYPE_S7_TCPIP               1
#define TYPE_S7_NL                  2
#define TYPE_S7_NLPRO               3
#define TYPE_S7_NLUSB               4
#define TYPE_S7_SOFTING             5
#define TYPE_S7_CIF                 6
#define TYPE_S7_MPI_SER             7
#define TYPE_S7_TS_AT               9
#define TYPE_S7_TS_TAPI             10
#define TYPE_S7_PCCP                11
#define TYPE_S7_PPI                 12
#define TYPE_S5_TCPIP               13
#define TYPE_S5_AS511               14
#define TYPE_RK                     15
#define TYPE_S7_NL50                17
#define TYPE_S7_CIFX                18
#define TYPE_S7_RED_CONN            19
#define TYPE_RFC_1006               20
#define TYPE_S7_TCPIP_TIA           21

#define TYPE_S7_SYMBOLIK_TIA        30
#define TYPE_S7_SYMBOLIK            31

#define MAX_PARA_TYPES              (TYPE_S7_TCPIP_TIA+1)     // Maximale Anzahl Kommunikationswege (nicht unbedingt alle bereits implementiert)


//
// Abfragebits für die verschiedenen Verbindungswege
//
#define MASK_S7CONN_IE              (1<<TYPE_S7CONN_IE)
#define MASK_S7_TCPIP               (1<<TYPE_S7_TCPIP)
#define MASK_S7_TCPIP_TIA           (1<<TYPE_S7_TCPIP_TIA)
#define MASK_S7_NL                  (1<<TYPE_S7_NL)
#define MASK_S7_NL50                (1<<TYPE_S7_NL50)
#define MASK_S7_NLPRO               (1<<TYPE_S7_NLPRO)
#define MASK_S7_NLUSB               (1<<TYPE_S7_NLUSB)
#define MASK_S7_SOFTING             (1<<TYPE_S7_SOFTING)
#define MASK_S7_CIF                 (1<<TYPE_S7_CIF)
#define MASK_S7_CIFX                (1<<TYPE_S7_CIFX)
#define MASK_S7_MPI_SER             (1<<TYPE_S7_MPI_SER)
#define MASK_S7_TS_AT               (1<<TYPE_S7_TS_AT)
#define MASK_S7_TS_TAPI             (1<<TYPE_S7_TS_TAPI)
#define MASK_S7_PCCP                (1<<TYPE_S7_PCCP)
#define MASK_S7_PPI                 (1<<TYPE_S7_PPI)
#define MASK_S5_TCPIP               (1<<TYPE_S5_TCPIP)
#define MASK_S5_AS511               (1<<TYPE_S5_AS511)
#define MASK_RK                     (1<<TYPE_RK)
#define MASK_S7_RED_CONN            (1<<TYPE_S7_RED_CONN)
#define MASK_RFC_1006               (1<<TYPE_RFC_1006)

#define MASK_S7_SYMBOLIK_TIA        (1<<TYPE_S7_SYMBOLIK_TIA)
#define MASK_S7_SYMBOLIK            (1<<TYPE_S7_SYMBOLIK)


//
// Fehler-Macros
//
#define IS_SUCCESS( a )       (((a)&ERROR_CLASS_MASK)==ERROR_CLASS_SUCCESS)
#define GET_ERROR_CLASS( a )  ((a)&ERROR_CLASS_MASK)
#define GET_ERROR_NUMBER( a ) ((a)&ERROR_NUMBER_MASK)

//
// Fehlermasken
//
#define ERROR_CLASS_MASK            0xFFFF0000
#define ERROR_NUMBER_MASK           0x0000FFFF

//
// Fehlerklassen
// Typfestlegung auf int wegen Multiplattform-Kompilierung erforderlich 
//
#define ERROR_CLASS_SUCCESS         ((int)0x00000000)
#define ERROR_CLASS_GLOBAL          ((int)0xFFF00000)
#define ERROR_CLASS_L1              ((int)0xFFF10000)
#define ERROR_CLASS_L2              ((int)0xFFF20000)
#define ERROR_CLASS_L3              ((int)0xFFF30000)
#define ERROR_CLASS_L4              ((int)0xFFF40000)
#define ERROR_CLASS_L5              ((int)0xFFF50000)
#define ERROR_CLASS_L6              ((int)0xFFF60000)
#define ERROR_CLASS_L7              ((int)0xFFF70000)

#define ERROR_CLASS_SYM             ((int)0xFFF80000)

#define ERROR_CLASS_TIA             ((int)0xFFF90000)

#define ERROR_CLASS_SYMBOLIC        ((int)0xFFFA0000)

#define ERROR_CLASS_INTERNAL        ((int)0xFFFF0000)


//
// Fehlermeldungen
//
#define AGL40_SUCCESS                       (ERROR_CLASS_SUCCESS+0)
#define AGL40_PENDING                       (ERROR_CLASS_SUCCESS+1)

#define AGL40_FUNC_NOT_IMPLEMENTED          (ERROR_CLASS_GLOBAL+0)
#define AGL40_PARAMETER_ERROR               (ERROR_CLASS_GLOBAL+1)
#define AGL40_INVALID_DEV_NUMBER            (ERROR_CLASS_GLOBAL+2)
#define AGL40_INVALID_PLC_NUMBER            (ERROR_CLASS_GLOBAL+3)
#define AGL40_INVALID_PARA_TYPE             (ERROR_CLASS_GLOBAL+4)
#define AGL40_OUT_OF_MEMORY                 (ERROR_CLASS_GLOBAL+5)
#define AGL40_DEVICE_OPEN                   (ERROR_CLASS_GLOBAL+6)
#define AGL40_DEVICE_NOT_OPEN               (ERROR_CLASS_GLOBAL+7)
#define AGL40_JOB_REMOVED                   (ERROR_CLASS_GLOBAL+8)
#define AGL40_INVALID_JOB                   (ERROR_CLASS_GLOBAL+9)
#define AGL40_FUNC_NOT_SUPPORTED            (ERROR_CLASS_GLOBAL+10)
#define AGL40_ADAPTER_NOT_INIT              (ERROR_CLASS_GLOBAL+11)
#define AGL40_OUT_OF_CONNECTIONS            (ERROR_CLASS_GLOBAL+12)
#define AGL40_DEVICE_NOT_SUPPORTED          (ERROR_CLASS_GLOBAL+13)
#define AGL40_HARDWARE_NOT_FOUND            (ERROR_CLASS_GLOBAL+14)
#define AGL40_PARA_READ_ERROR               (ERROR_CLASS_GLOBAL+15)
#define AGL40_PARA_WRITE_ERROR              (ERROR_CLASS_GLOBAL+16)
#define AGL40_TIME_EXPIRED                  (ERROR_CLASS_GLOBAL+17)
#define AGL40_WRONG_CHAR_ERROR              (ERROR_CLASS_GLOBAL+18)
#define AGL40_BUFF_TOO_SHORT                (ERROR_CLASS_GLOBAL+19)
#define AGL40_FILE_NOT_FOUND                (ERROR_CLASS_GLOBAL+20)
#define AGL40_CONFIG_ERROR                  (ERROR_CLASS_GLOBAL+21)
#define AGL40_DYN_DLL_ERROR                 (ERROR_CLASS_GLOBAL+22)
#define AGL40_FILE_NOT_CREATED              (ERROR_CLASS_GLOBAL+23)
#define AGL40_NOTIFICATION_ALREADY_IN_USE   (ERROR_CLASS_GLOBAL+24)
#define AGL40_RUNNING_JOB_NOT_REMOVED       (ERROR_CLASS_GLOBAL+25)
#define AGL40_JOB_ALREADY_REMOVED           (ERROR_CLASS_GLOBAL+26)
#define AGL40_WLD_READ_ERROR                (ERROR_CLASS_GLOBAL+27) // Fehler beim Lesen der WLD-Datei
#define AGL40_WLD_WRITE_ERROR               (ERROR_CLASS_GLOBAL+28) // Fehler beim Schreiben der WLD-Datei
#define AGL40_WLD_INVALID_STRUCTURE         (ERROR_CLASS_GLOBAL+29) // Ungültige Struktur in WLD-Datei
#define AGL40_WLD_MULTIPLE_BLOCK            (ERROR_CLASS_GLOBAL+30) // Baustein in WLD-Datei mehrfach vorhanden
#define AGL40_REENTRANCY_ERROR              (ERROR_CLASS_GLOBAL+31) // Funktion ist nicht reentrant
#define AGL40_FUNC_REMOVED                  (ERROR_CLASS_GLOBAL+32) // Funktion zur Codereduzierung entfernt

#define AGL40_NO_QUEUE                      (ERROR_CLASS_L5+0)
#define AGL40_INVALID_PACKET                (ERROR_CLASS_L5+1)
#define AGL40_NOT_CONNECTED                 (ERROR_CLASS_L5+2)
#define AGL40_CONNECTION_CLOSED             (ERROR_CLASS_L5+3)
#define AGL40_TIMEOUT                       (ERROR_CLASS_L5+4)
#define AGL40_WRONG_KONTEXT                 (ERROR_CLASS_L5+5)
#define AGL40_PLC_MEMORY_ERROR              (ERROR_CLASS_L5+6)
#define AGL40_WRONG_OP_STATE                (ERROR_CLASS_L5+7)
#define AGL40_WRONG_ADDRESS                 (ERROR_CLASS_L5+8)
#define AGL40_INVALID_MODE_ERROR            (ERROR_CLASS_L5+9)
#define AGL40_NO_DATA_ERROR                 (ERROR_CLASS_L5+10)
#define AGL40_PLC_PRIORITY_CLASS_ERROR      (ERROR_CLASS_L5+11)
#define AGL40_EMPTY_BLOCK_LIST              (ERROR_CLASS_L5+12)
#define AGL40_PLC_BLOCKSIZE_ERROR           (ERROR_CLASS_L5+13)
#define AGL40_INVALID_BLOCK_NUMBER          (ERROR_CLASS_L5+14)
#define AGL40_PROTECT_ERROR                 (ERROR_CLASS_L5+15)
#define AGL40_UNKNOWN_SZL_ID                (ERROR_CLASS_L5+16)
#define AGL40_UNKNOWN_SZL_INDEX             (ERROR_CLASS_L5+17)
#define AGL40_NO_INFORMATION                (ERROR_CLASS_L5+18)
#define AGL40_UNKNOWN_PLC_ERROR             (ERROR_CLASS_L5+19)
#define AGL40_HARDWARE_ERROR                (ERROR_CLASS_L5+20)
#define AGL40_OBJECT_ACCESS_NOT_ALLOWED     (ERROR_CLASS_L5+21)
#define AGL40_CONTEXT_NOT_SUPPORTED         (ERROR_CLASS_L5+22)
#define AGL40_TYPE_NOT_SUPPORTED            (ERROR_CLASS_L5+23)
#define AGL40_PDU_ERROR                     (ERROR_CLASS_L5+24)
#define AGL40_NO_PLC_START                  (ERROR_CLASS_L5+25)
#define AGL40_NO_PLC_RESUME                 (ERROR_CLASS_L5+26)
#define AGL40_DISCONNECT_REQUEST            (ERROR_CLASS_L5+27)
#define AGL40_PLC_NOT_FOUND                 (ERROR_CLASS_L5+28)
#define AGL40_DATA_TOO_LONG                 (ERROR_CLASS_L5+29)
#define AGL40_PLCFUNC_NOT_SUPPORTED         (ERROR_CLASS_L5+30)
#define AGL40_WRONG_PASSWORD                (ERROR_CLASS_L5+31)
#define AGL40_LEGITIMATION_ALREADY_ENTERED  (ERROR_CLASS_L5+32)
#define AGL40_LEGITIMATION_ALREADY_RELEASED (ERROR_CLASS_L5+33)
#define AGL40_PASSWORD_NOT_REQUIRED         (ERROR_CLASS_L5+34)
#define AGL40_CYCL_VAR_DEF_WRONG            (ERROR_CLASS_L5+35)
#define AGL40_CYCL_JOB_DOES_NOT_EXIST       (ERROR_CLASS_L5+36)
#define AGL40_CYCL_INVALID_JOB_STATE        (ERROR_CLASS_L5+37)
#define AGL40_CYCL_INVALID_CYCLE_TIME       (ERROR_CLASS_L5+38)
#define AGL40_CYCL_NO_ADD_JOB_POSSIBLE      (ERROR_CLASS_L5+39)
#define AGL40_CYCL_JOBFUNC_NOT_POSSIBLE     (ERROR_CLASS_L5+40)
#define AGL40_CYCL_OVERLOAD_ABORT           (ERROR_CLASS_L5+41)
#define AGL40_CYCL_NO_MORE_DATA             (ERROR_CLASS_L5+42)
#define AGL40_WRONG_TIME_FORMAT             (ERROR_CLASS_L5+43)
#define AGL40_UNKNOWN_PI_NAME               (ERROR_CLASS_L5+44)
#define AGL40_FILETRANSFER_ABORTED          (ERROR_CLASS_L5+45)
#define AGL40_PDUSIZE_ERROR                 (ERROR_CLASS_L5+46)
#define AGL40_NO_H_PLC_FOUND                (ERROR_CLASS_L5+47)
#define AGL40_DATA_NOT_CHANGED              (ERROR_CLASS_L5+48)
#define AGL40_DATA_CORRUPT                  (ERROR_CLASS_L5+49)
#define AGL40_WRONG_PARA_SIZE_OR_TYPE       (ERROR_CLASS_L5+50)
#define AGL40_WRONG_PARA_VALUE              (ERROR_CLASS_L5+51)
#define AGL40_PARA_NOT_CHANGEABLE           (ERROR_CLASS_L5+52)
#define AGL40_PARA_READ_ONLY                (ERROR_CLASS_L5+53)
#define AGL40_WRONG_VAR_SIZE                (ERROR_CLASS_L5+54)

#define AGL40_L4_ERROR                      (ERROR_CLASS_L4+0)

#define AGL40_ERROR_READING_DEVINFO         (ERROR_CLASS_L3+0)
#define AGL40_ERROR_READING_BUSPARAS        (ERROR_CLASS_L3+1)
#define AGL40_ERROR_WRITING_BUSPARAS        (ERROR_CLASS_L3+2)
#define AGL40_ERROR_NO_RESOURCES            (ERROR_CLASS_L3+3)
#define AGL40_ERROR_INVALID_DEVICE          (ERROR_CLASS_L3+4)
#define AGL40_ERROR_ADAPTER_NOT_FOUND       (ERROR_CLASS_L3+5)
#define AGL40_ERROR_DRIVER_NOT_FOUND        (ERROR_CLASS_L3+6)

#define AGL40_ERROR_OWN_ADDRESS             (ERROR_CLASS_L2+0)
#define AGL40_ERROR_WRONG_HSA               (ERROR_CLASS_L2+1)
#define AGL40_ERROR_NOT_IN_RING             (ERROR_CLASS_L2+2)
#define AGL40_ERROR_WRONG_PAKET             (ERROR_CLASS_L2+4)
#define AGL40_ERROR_UNKNOWN_ADAP_ERR        (ERROR_CLASS_L2+6)
#define AGL40_ERROR_UNKNOWN_DRIVER_ERR      (ERROR_CLASS_L2+7)
#define AGL40_ERROR_ADAPTER_REMOVED         (ERROR_CLASS_L2+8)
#define AGL40_ERROR_MODEM_REMOVED           (ERROR_CLASS_L2+9)
#define AGL40_NO_DIRECT_PLC                 (ERROR_CLASS_L2+10)
#define AGL40_ERROR_WRONG_MPI_SPEED         (ERROR_CLASS_L2+0x0313)
#define AGL40_ERROR_HSA                     (ERROR_CLASS_L2+0x0314)
#define AGL40_ERROR_DUP_ADDRESS             (ERROR_CLASS_L2+0x0315)
#define AGL40_ERROR_NO_MORE_DEVICE          (ERROR_CLASS_L2+0x031A)
#define AGL40_ERROR_BUS_FAILURE_1           (ERROR_CLASS_L2+0x031C)
#define AGL40_ERROR_BUS_FAILURE_2           (ERROR_CLASS_L2+0x031D)
#define AGL40_ERROR_NO_BUSPARA_TELEGRAMM    (ERROR_CLASS_L2+0x031E)
#define AGL40_ERROR_NO_LEGITIMATION         (ERROR_CLASS_L2+0x0337)

#define AGL40_ERROR_PORT_IN_USE             (ERROR_CLASS_L1+0)
#define AGL40_MODEM_NOT_FOUND               (ERROR_CLASS_L1+1)
#define AGL40_MODEM_NOT_ONHOOK              (ERROR_CLASS_L1+2)
#define AGL40_MODEM_NOT_OFFHOOK             (ERROR_CLASS_L1+3)
#define AGL40_MODEM_ERROR_BASE_INIT         (ERROR_CLASS_L1+4)
#define AGL40_MODEM_ERROR_INIT1             (ERROR_CLASS_L1+5)
#define AGL40_MODEM_ERROR_INIT2             (ERROR_CLASS_L1+6)
#define AGL40_MODEM_ERROR_INIT3             (ERROR_CLASS_L1+7)
#define AGL40_MODEM_ERROR_INIT4             (ERROR_CLASS_L1+8)
#define AGL40_MODEM_ERROR_DIALTYPE          (ERROR_CLASS_L1+9)
#define AGL40_MODEM_ERROR_DIALTONE          (ERROR_CLASS_L1+10)
#define AGL40_MODEM_ERROR_AUTOANSWER        (ERROR_CLASS_L1+11)
#define AGL40_MODEM_REMOVED                 (ERROR_CLASS_L1+12)
#define AGL40_MODEM_NO_CONNECTION           (ERROR_CLASS_L1+13)
#define AGL40_MODEM_INVALID_USER            (ERROR_CLASS_L1+14)
#define AGL40_MODEM_INVALID_PASSWORD        (ERROR_CLASS_L1+15)
#define AGL40_MODEM_CALLBACK_NUMBER_ERR     (ERROR_CLASS_L1+16)
#define AGL40_MODEM_DIALING_ERR             (ERROR_CLASS_L1+17)

//TIA usw. Symbolik

#define AGL40_SYMBOLIC_NOT_APPLICABLE       (ERROR_CLASS_SYMBOLIC+0)
#define AGL40_SYMBOLIC_ACCESS_INVALID_ELEMENT  (ERROR_CLASS_SYMBOLIC+1)
#define AGL40_SYMBOLIC_ACCESS_BUFFER_TOO_SMALL  (ERROR_CLASS_SYMBOLIC+2)
#define AGL40_SYMBOLIC_ACCESS_BUFFER_INVALID_S5TIME_VALUE  (ERROR_CLASS_SYMBOLIC+3)
#define AGL40_SYMBOLIC_ACCESS_BUFFER_INVALID_S5TIME_BASE  (ERROR_CLASS_SYMBOLIC+4)
#define AGL40_SYMBOLIC_ACCESS_BUFFER_INVALID_S7_COUNTER_VALUE (ERROR_CLASS_SYMBOLIC+5)
#define AGL40_SYMBOLIC_PATH_MISSING_EXPANDED_INDEX (ERROR_CLASS_SYMBOLIC+6)
#define AGL40_SYMBOLIC_PATH_INVALID (ERROR_CLASS_SYMBOLIC+7)
#define AGL40_SYMBOLIC_PATH_END_REACHED (ERROR_CLASS_SYMBOLIC+8)
#define AGL40_SYMBOLIC_PATH_UNKNOWN_FIELD (ERROR_CLASS_SYMBOLIC+9)
#define AGL40_SYMBOLIC_UNKNOWN_CHILD (ERROR_CLASS_SYMBOLIC+10)
#define AGL40_SYMBOLIC_ARRAY_IS_NO_VALUE_TYPE (ERROR_CLASS_SYMBOLIC+11)
#define AGL40_SYMBOLIC_ARRAY_INDEX_MISSING (ERROR_CLASS_SYMBOLIC+12)
#define AGL40_SYMBOLIC_ARRAY_INDEX_INVALID (ERROR_CLASS_SYMBOLIC+13)
#define AGL40_SYMBOLIC_ARRAY_INDEX_NOT_ALLOWED (ERROR_CLASS_SYMBOLIC+14)
#define AGL40_SYMBOLIC_ARRAY_RANGE_NOT_ALLOWED (ERROR_CLASS_SYMBOLIC+15)
#define AGL40_SYMBOLIC_ARRAY_RANGE_INVALID (ERROR_CLASS_SYMBOLIC+16)
#define AGL40_SYMBOLIC_UDT_HAS_NO_BASE_TYPE (ERROR_CLASS_SYMBOLIC+17)

//SErrors
#define AGL40_SYMBOLIC_UNKNOWN_ERROR_CODE (ERROR_CLASS_SYMBOLIC+18)
#define AGL40_SYMBOLIC_WRONG_SYMBOL_ID (ERROR_CLASS_SYMBOLIC+19)
#define AGL40_SYMBOLIC_ITEM_ACCESS_DENIED (ERROR_CLASS_SYMBOLIC+20)
#define AGL40_SYMBOLIC_READ_ONLY_ITEM (ERROR_CLASS_SYMBOLIC+21)
#define AGL40_SYMBOLIC_PARTIAL_ERRORS (ERROR_CLASS_SYMBOLIC+22)
#define AGL40_SYMBOLIC_NOT_AN_INSTANCE (ERROR_CLASS_SYMBOLIC+23)
#define AGL40_SYMBOLIC_WRONG_SYMBOL_ID_PATH (ERROR_CLASS_SYMBOLIC+24)
#define AGL40_SYMBOLIC_SYMBOLIC_WRONG_AREA (ERROR_CLASS_SYMBOLIC+25)
#define AGL40_SYMBOLIC_WRONG_VALUE_RANGE (ERROR_CLASS_SYMBOLIC+26)
#define AGL40_SYMBOLIC_PLC_CANT_DELIVER (ERROR_CLASS_SYMBOLIC+27)
#define AGL40_SYMBOLIC_PLC_OBJECT_DOES_NOT_EXISTS (ERROR_CLASS_SYMBOLIC+28)
#define AGL40_SYMBOLIC_PLC_TYPE_INFO_MISSING (ERROR_CLASS_SYMBOLIC+29)

#define AGL40_SYMBOLIC_INVALID_DATABLOCK (ERROR_CLASS_SYMBOLIC+80)
#define AGL40_SYMBOLIC_DATABLOCK_IS_OPTIMIZED (ERROR_CLASS_SYMBOLIC+81)
#define AGL40_SYMBOLIC_INVALID_TAG (ERROR_CLASS_SYMBOLIC+82)
#define AGL40_SYMBOLIC_START_ADDRESS_TOO_BIG (ERROR_CLASS_SYMBOLIC+83)
#define AGL40_SYMBOLIC_NOT_ALLOWED_SYMBOLIC_ACCESS (ERROR_CLASS_SYMBOLIC+84)

//99-300 ist reserviert fuer interne Fehlercodes
#define AGL40_SYMBOLIC_INTERNAL_ERROR (ERROR_CLASS_SYMBOLIC+99)

#define AGL40_SYMBOLIC_INTERNAL_ERROR_0 (ERROR_CLASS_SYMBOLIC+100+0)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_1 (ERROR_CLASS_SYMBOLIC+100+1)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_2 (ERROR_CLASS_SYMBOLIC+100+2)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_3 (ERROR_CLASS_SYMBOLIC+100+3)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_4 (ERROR_CLASS_SYMBOLIC+100+4)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_5 (ERROR_CLASS_SYMBOLIC+100+5)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_6 (ERROR_CLASS_SYMBOLIC+100+6)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_7 (ERROR_CLASS_SYMBOLIC+100+7)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_8 (ERROR_CLASS_SYMBOLIC+100+8)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_9 (ERROR_CLASS_SYMBOLIC+100+9)
#define AGL40_SYMBOLIC_FILE_READ_ERROR (ERROR_CLASS_SYMBOLIC+100+10)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_11 (ERROR_CLASS_SYMBOLIC+100+11)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_12 (ERROR_CLASS_SYMBOLIC+100+12)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_13 (ERROR_CLASS_SYMBOLIC+100+13)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_14 (ERROR_CLASS_SYMBOLIC+100+14)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_15 (ERROR_CLASS_SYMBOLIC+100+15)
#define AGL40_SYMBOLIC_PROJECT_FILE_NOT_FOUND (ERROR_CLASS_SYMBOLIC+100+16)
#define AGL40_SYMBOLIC_PROJECT_FILE_UNSUPPORTED_EXTENSION (ERROR_CLASS_SYMBOLIC+100+17)
#define AGL40_SYMBOLIC_PROJECT_FOLDER_MISSING (ERROR_CLASS_SYMBOLIC+100+18)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_19 (ERROR_CLASS_SYMBOLIC+100+19)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_20 (ERROR_CLASS_SYMBOLIC+100+20)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_21 (ERROR_CLASS_SYMBOLIC+100+21)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_22 (ERROR_CLASS_SYMBOLIC+100+22)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_23 (ERROR_CLASS_SYMBOLIC+100+23)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_24 (ERROR_CLASS_SYMBOLIC+100+24)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_25 (ERROR_CLASS_SYMBOLIC+100+25)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_26 (ERROR_CLASS_SYMBOLIC+100+26)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_27 (ERROR_CLASS_SYMBOLIC+100+27)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_28 (ERROR_CLASS_SYMBOLIC+100+28)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_29 (ERROR_CLASS_SYMBOLIC+100+29)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_30 (ERROR_CLASS_SYMBOLIC+100+30)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_31 (ERROR_CLASS_SYMBOLIC+100+31)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_32 (ERROR_CLASS_SYMBOLIC+100+32)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_33 (ERROR_CLASS_SYMBOLIC+100+33)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_34 (ERROR_CLASS_SYMBOLIC+100+34)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_35 (ERROR_CLASS_SYMBOLIC+100+35)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_36 (ERROR_CLASS_SYMBOLIC+100+36)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_37 (ERROR_CLASS_SYMBOLIC+100+37)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_38 (ERROR_CLASS_SYMBOLIC+100+38)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_39 (ERROR_CLASS_SYMBOLIC+100+39)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_40 (ERROR_CLASS_SYMBOLIC+100+40)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_41 (ERROR_CLASS_SYMBOLIC+100+41)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_42 (ERROR_CLASS_SYMBOLIC+100+42)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_43 (ERROR_CLASS_SYMBOLIC+100+43)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_44 (ERROR_CLASS_SYMBOLIC+100+44)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_45 (ERROR_CLASS_SYMBOLIC+100+45)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_46 (ERROR_CLASS_SYMBOLIC+100+46)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_47 (ERROR_CLASS_SYMBOLIC+100+47)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_48 (ERROR_CLASS_SYMBOLIC+100+48)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_49 (ERROR_CLASS_SYMBOLIC+100+49)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_50 (ERROR_CLASS_SYMBOLIC+100+50)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_51 (ERROR_CLASS_SYMBOLIC+100+51)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_52 (ERROR_CLASS_SYMBOLIC+100+52)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_53 (ERROR_CLASS_SYMBOLIC+100+53)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_54 (ERROR_CLASS_SYMBOLIC+100+54)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_55 (ERROR_CLASS_SYMBOLIC+100+55)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_56 (ERROR_CLASS_SYMBOLIC+100+56)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_57 (ERROR_CLASS_SYMBOLIC+100+57)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_58 (ERROR_CLASS_SYMBOLIC+100+58)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_59 (ERROR_CLASS_SYMBOLIC+100+59)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_60 (ERROR_CLASS_SYMBOLIC+100+60)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_61 (ERROR_CLASS_SYMBOLIC+100+61)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_62 (ERROR_CLASS_SYMBOLIC+100+62)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_63 (ERROR_CLASS_SYMBOLIC+100+63)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_64 (ERROR_CLASS_SYMBOLIC+100+64)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_65 (ERROR_CLASS_SYMBOLIC+100+65)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_66 (ERROR_CLASS_SYMBOLIC+100+66)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_67 (ERROR_CLASS_SYMBOLIC+100+67)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_68 (ERROR_CLASS_SYMBOLIC+100+68)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_69 (ERROR_CLASS_SYMBOLIC+100+69)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_70 (ERROR_CLASS_SYMBOLIC+100+70)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_71 (ERROR_CLASS_SYMBOLIC+100+71)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_72 (ERROR_CLASS_SYMBOLIC+100+72)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_73 (ERROR_CLASS_SYMBOLIC+100+73)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_74 (ERROR_CLASS_SYMBOLIC+100+74)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_75 (ERROR_CLASS_SYMBOLIC+100+75)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_76 (ERROR_CLASS_SYMBOLIC+100+76)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_77 (ERROR_CLASS_SYMBOLIC+100+77)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_78 (ERROR_CLASS_SYMBOLIC+100+78)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_79 (ERROR_CLASS_SYMBOLIC+100+79)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_80 (ERROR_CLASS_SYMBOLIC+100+80)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_81 (ERROR_CLASS_SYMBOLIC+100+81)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_82 (ERROR_CLASS_SYMBOLIC+100+82)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_83 (ERROR_CLASS_SYMBOLIC+100+83)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_84 (ERROR_CLASS_SYMBOLIC+100+84)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_85 (ERROR_CLASS_SYMBOLIC+100+85)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_86 (ERROR_CLASS_SYMBOLIC+100+86)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_87 (ERROR_CLASS_SYMBOLIC+100+87)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_88 (ERROR_CLASS_SYMBOLIC+100+88)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_89 (ERROR_CLASS_SYMBOLIC+100+89)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_90 (ERROR_CLASS_SYMBOLIC+100+90)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_91 (ERROR_CLASS_SYMBOLIC+100+91)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_92 (ERROR_CLASS_SYMBOLIC+100+92)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_93 (ERROR_CLASS_SYMBOLIC+100+93)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_94 (ERROR_CLASS_SYMBOLIC+100+94)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_95 (ERROR_CLASS_SYMBOLIC+100+95)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_96 (ERROR_CLASS_SYMBOLIC+100+96)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_97 (ERROR_CLASS_SYMBOLIC+100+97)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_98 (ERROR_CLASS_SYMBOLIC+100+98)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_99 (ERROR_CLASS_SYMBOLIC+100+99)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_100 (ERROR_CLASS_SYMBOLIC+100+100)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_101 (ERROR_CLASS_SYMBOLIC+100+101)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_102 (ERROR_CLASS_SYMBOLIC+100+102)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_103 (ERROR_CLASS_SYMBOLIC+100+103)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_104 (ERROR_CLASS_SYMBOLIC+100+104)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_105 (ERROR_CLASS_SYMBOLIC+100+105)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_106 (ERROR_CLASS_SYMBOLIC+100+106)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_107 (ERROR_CLASS_SYMBOLIC+100+107)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_108 (ERROR_CLASS_SYMBOLIC+100+108)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_109 (ERROR_CLASS_SYMBOLIC+100+109)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_110 (ERROR_CLASS_SYMBOLIC+100+110)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_111 (ERROR_CLASS_SYMBOLIC+100+111)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_112 (ERROR_CLASS_SYMBOLIC+100+112)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_113 (ERROR_CLASS_SYMBOLIC+100+113)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_114 (ERROR_CLASS_SYMBOLIC+100+114)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_115 (ERROR_CLASS_SYMBOLIC+100+115)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_116 (ERROR_CLASS_SYMBOLIC+100+116)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_117 (ERROR_CLASS_SYMBOLIC+100+117)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_118 (ERROR_CLASS_SYMBOLIC+100+118)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_119 (ERROR_CLASS_SYMBOLIC+100+119)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_120 (ERROR_CLASS_SYMBOLIC+100+120)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_121 (ERROR_CLASS_SYMBOLIC+100+121)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_122 (ERROR_CLASS_SYMBOLIC+100+122)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_123 (ERROR_CLASS_SYMBOLIC+100+123)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_124 (ERROR_CLASS_SYMBOLIC+100+124)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_125 (ERROR_CLASS_SYMBOLIC+100+125)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_126 (ERROR_CLASS_SYMBOLIC+100+126)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_127 (ERROR_CLASS_SYMBOLIC+100+127)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_128 (ERROR_CLASS_SYMBOLIC+100+128)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_129 (ERROR_CLASS_SYMBOLIC+100+129)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_130 (ERROR_CLASS_SYMBOLIC+100+130)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_131 (ERROR_CLASS_SYMBOLIC+100+131)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_132 (ERROR_CLASS_SYMBOLIC+100+132)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_133 (ERROR_CLASS_SYMBOLIC+100+133)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_134 (ERROR_CLASS_SYMBOLIC+100+134)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_135 (ERROR_CLASS_SYMBOLIC+100+135)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_136 (ERROR_CLASS_SYMBOLIC+100+136)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_137 (ERROR_CLASS_SYMBOLIC+100+137)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_138 (ERROR_CLASS_SYMBOLIC+100+138)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_139 (ERROR_CLASS_SYMBOLIC+100+139)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_140 (ERROR_CLASS_SYMBOLIC+100+140)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_141 (ERROR_CLASS_SYMBOLIC+100+141)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_142 (ERROR_CLASS_SYMBOLIC+100+142)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_143 (ERROR_CLASS_SYMBOLIC+100+143)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_144 (ERROR_CLASS_SYMBOLIC+100+144)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_145 (ERROR_CLASS_SYMBOLIC+100+145)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_146 (ERROR_CLASS_SYMBOLIC+100+146)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_147 (ERROR_CLASS_SYMBOLIC+100+147)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_148 (ERROR_CLASS_SYMBOLIC+100+148)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_149 (ERROR_CLASS_SYMBOLIC+100+149)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_150 (ERROR_CLASS_SYMBOLIC+100+150)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_151 (ERROR_CLASS_SYMBOLIC+100+151)
#define AGL40_SYMBOLIC_INTERNAL_ERROR_152 (ERROR_CLASS_SYMBOLIC+100+152)

#define AGL40_SYMBOLIC_INTERNAL_COMMUNICATION_ERROR (ERROR_CLASS_SYMBOLIC+400)
#define AGL40_SYMBOLIC_INTERNAL_COMMUNICATION_ERROR_0 (ERROR_CLASS_SYMBOLIC+400+1)
//#define AGL40_SYMBOLIC_INTERNAL_COMMUNICATION_ERROR_1 (ERROR_CLASS_SYMBOLIC+400+2)
#define AGL40_SYMBOLIC_INTERNAL_COMMUNICATION_ERROR_2 (ERROR_CLASS_SYMBOLIC+400+3)
#define AGL40_SYMBOLIC_WRITE_BOOL_RANGE_NEEDS_MODULO_8_START_AND_COUNT (ERROR_CLASS_SYMBOLIC+400+4)
#define AGL40_SYMBOLIC_WRONG_READ_TYPE (ERROR_CLASS_SYMBOLIC+400+5)
#define AGL40_SYMBOLIC_PLC_AUTO_DETECT_FAILED (ERROR_CLASS_SYMBOLIC+400+6)

#define AGL40_SYMBOLIC_ONLY_SINGLE_VALUES_ALLOWED (ERROR_CLASS_SYMBOLIC+400+7)

#define AGL40_TIA_READ_ERROR (ERROR_CLASS_TIA+0)
#define AGL40_TIA_WRITE_ERROR (ERROR_CLASS_TIA+1)
#define AGL40_TIA_READ_TYPE_ERROR (ERROR_CLASS_TIA+2)
#define AGL40_TIA_WRONG_PLC_TYPE (ERROR_CLASS_TIA+3)

#define AGL40_SYMBOLIC_INTERNAL_ASSERT (ERROR_CLASS_TIA+100)

//
// Definitionen für Lifelist
//
#define LL_PASSIVE                  0x00
#define LL_NONE                     0x10
#define LL_ACTIVE_READY             0x20
#define LL_ACTIVE                   0x30


//
// Verbindungsart-Konstanten
//
#define CONN_PG                     0         // PG-Verbindung verwenden
#define CONN_OP                     1         // OP-Verbindung verwenden
#define CONN_SONST                  2         // Sonstige-Verbindung verwenden
#define CONN_PROJ                   3         // Projektierte Verbindung verwenden
#define CONN_ROUTE                  4         // Verbindung über Routing verwenden

//
// Profil-Konstanten für PPI/MPI/Profibus
//
#define PROFIL_MPI                  0         // MPI-Profil verwenden
#define PROFIL_DP                   1         // DP-Profil verwenden
#define PROFIL_STD                  2         // Standard-Profil verwenden
#define PROFIL_FMS                  3         // FMS/DP-Profil verwenden
#define PROFIL_USER                 4         // Benutzerdefiniertes Profil verwenden
#define PROFIL_PPI                  5         // PPI-Profil verwenden

//
// Job-Zustände
//
#define NOT_INIT                    0         // Noch nicht initialisiert
#define HOLD                        1         // Wartet auf Daten des vorigen Auftrages
#define WAITING                     2         // Wartet auf Bearbeitung
#define RUNNING                     3         // In Bearbeitung
#define FINISHED                    4         // Erfolgreich beendet
#define ABORTED                     5         // Abgebrochen
#define DELETED                     6         // Gelöscht


//
// Betriebszustände der normalen S7-SPSen für GetOpstate und GetOpStateMsg ...
//
#define OPSTATE_STOP                0         // CPU im Stop
#define OPSTATE_START               1         // CPU im Anlauf
#define OPSTATE_RUN                 2         // CPU im Run
#define OPSTATE_UNKNOWN             3         // CPU im ???
//
// ... und zusätzlich für H-CPU-Systeme und GetOpStateMsg
//
#define OPSTATE_RUN_SOLO            8         // CPU im Run (Solobetrieb)
#define OPSTATE_RUN_RED             9         // CPU im Run-R (redundanter Betrieb)
#define OPSTATE_HALT                10        // CPU im Halt
#define OPSTATE_CONNECTING          11        // Ankoppeln
#define OPSTATE_UPDATING            12        // Aufdaten


//
// Erweiterte Betriebszustände der SPS
//
#define OPSTATEEX_UNKNOWN           0         // CPU im ???
#define OPSTATEEX_STOP_UPDATE       1         // CPU im Stop (Update)
#define OPSTATEEX_STOP_CLEARALL     2         // CPU im Stop (Urloeschen)
#define OPSTATEEX_STOP_INIT         3         // CPU im Stop (Eigeninitialisierung)
#define OPSTATEEX_STOP_INTERNAL     4         // CPU im Stop (intern)
#define OPSTATEEX_START_COLD        5         // CPU im Anlauf (Kaltstart)
#define OPSTATEEX_START_WARM        6         // CPU im Anlauf (Warmstart)
#define OPSTATEEX_START_RESUME      7         // CPU im Anlauf (Wiederanlauf)
#define OPSTATEEX_RUN_SOLO          8         // CPU im Run (Solobetrieb)
#define OPSTATEEX_RUN_RED           9         // CPU im Run-R (redundanter Betrieb)
#define OPSTATEEX_HALT              10        // CPU im Halt
#define OPSTATEEX_CONNECTING        11        // Ankoppeln
#define OPSTATEEX_UPDATING          12        // Aufdaten
#define OPSTATEEX_DEFECT            13        // Defekt
#define OPSTATEEX_SELFTEST          14        // Selbsttest
#define OPSTATEEX_NO_POWER          15        // No Power


//
// Datenbereiche für gemischte Anfragen
//
#define AERA_UNKNOWN                agl_uint16_t(-1)
#define AREA_IN                     agl_uint16_t(0)         // Eingänge
#define AREA_OUT                    agl_uint16_t(1)         // Ausgänge
#define AREA_FLAG                   agl_uint16_t(2)         // Merker
#define AREA_DATA                   agl_uint16_t(3)         // Daten
#define AREA_TIMER                  agl_uint16_t(4)         // Zeiten
#define AREA_COUNTER                agl_uint16_t(5)         // Zähler
#define AREA_SFLAG_200              agl_uint16_t(6)         // Sondermerker der 200er
#define AREA_VAR_200                agl_uint16_t(7)         // Variablen der 200er
#define AREA_TIMER_200              agl_uint16_t(8)         // Zeiten 200er (werden automatisch von TYPE_TIMER gewandelt wenn 200er vorliegt)
#define AREA_COUNTER_200            agl_uint16_t(9)         // Zähler 200er (werden automatisch von TYPE_COUNTER gewandelt wenn 200er vorliegt)
#define AREA_PERIPHERIE             agl_uint16_t(10)        // Direktzugriff auf Peripherie (lesen von PEx, schreiben auf PAx, nur Byte+Wort+Doppelwort zulässig)
#define AREA_SFLAG_S5               agl_uint16_t(11)        // Sondermerker der S5 (nur mit AS511)
#define AREA_PLCINFO_200            agl_uint16_t(12)        // Systeminfo der 200er (nur intern verwendet)

#define TYP_UNKNOWN                 agl_uint16_t(-1)
#define TYP_BIT                     agl_uint16_t(0)
#define TYP_BYTE                    agl_uint16_t(1)
#define TYP_WORD                    agl_uint16_t(2)
#define TYP_DWORD                   agl_uint16_t(3)
#define TYP_COUNTER                 agl_uint16_t(4)
#define TYP_TIMER                   agl_uint16_t(5)
#define TYP_COUNTER_200             agl_uint16_t(6)
#define TYP_TIMER_200               agl_uint16_t(7)
#define TYP_HS_COUNTER_200          agl_uint16_t(8)

//
// Definitionen für RK512/3964(R)
//
#define AREA_RK_DATA                'D'       // Datenworte aus DB
#define AREA_RK_EXT_DATA            'X'       // Datenworte aus DX
#define AREA_RK_IN                  'E'       // Eingangsbytes
#define AREA_RK_OUT                 'A'       // Ausgangsbytes
#define AREA_RK_FLAG                'M'       // Merkerbytes
#define AREA_RK_PERIPHERIE          'P'       // Peripheriebytes
#define AREA_RK_COUNTER             'Z'       // Zählerzellen
#define AREA_RK_TIMER               'T'       // Zeitzellen
#define AREA_RK_ABS_ADR             'S'       // Absolute Adressen
#define AREA_RK_SYS_ADR             'B'       // Systemadressen
#define AREA_RK_EXT_PERIPHERIE      'Q'       // Erweiterte Peripherie

//
// Definitionen für PC/CP-Protokollarten
//
#define PCCP_PROTO_UNSUPPORTED     -1
#define PCCP_PROTO_UNKNOWN          0
#define PCCP_PROTO_S7               1
#define PCCP_PROTO_PPI              2
#define PCCP_PROTO_ISO              3
#define PCCP_PROTO_TCPIP            4

#define PPI_MULTIMASTER             0x01
#define PPI_ADVANCED_PPI            0x02
#define NL_USE_AUTOBAUD             0x100


//
// Klassifizierung der verwendeten SPS (Abprüfung mit & da mehrere Bits gesetzt sein können!!)
//
#define PLC_TYPE_UNKNOWN            0x01      // Wir wissen es einfach nicht
#define PLC_TYPE_S5                 0x02      // Es handelt sich um eine S5
#define PLC_TYPE_S5_PG              0x04      // Es handelt sich um einen AS511-Zugriff auf die S5
#define PLC_TYPE_RK                 0x08      // Es handelt sichum RK512 bzw. 3964/3964R
#define PLC_TYPE_S7                 0x10      // Es handelt sich um eine S7
#define PLC_TYPE_S7_200             0x20      // Es handelt sich um eine S7-200
#define PLC_TYPE_S7_300_400         0x40      // Es handelt sich um eine S7-300 oder S7-400
#define PLC_TYPE_S7_1200            0x80      // Es handelt sich um eine S7-1200 oder S7-1500 mit absoluter Kommunikation
#define PLC_TYPE_LOGO               0x100     // Es handelt sich um eine LOGO! 0BA7
#define PLC_TYPE_RFC1006            0x200     // Es handelt sich um eine native RFC1006-Verbindung
#define PLC_TYPE_S7_1200_1500_TIA   0x400     // Es handelt sich um eine S7-1200 oder S7-1500 mit symbolischer Kommunikation
#define PLC_TYPE_IEC                0x1000    // Es handelt sich um eine IEC-Steuerung
#define PLC_TYPE_IEC_ADS            0x2000    // Es handelt sich um eine Twincat-Steuerung
#define PLC_TYPE_IEC_CODESYS        0x4000    // Es handelt sich um eine CoDeSys-Steuerung

#define PLC_TYPE_REMOTE             0x8000    // Fernzugriff mittels TS-Option

//
// Zum Prüfen, ob die Funktionen von der SPS unterstützt werden
//
#define HAS_FUNC_READ_MLFBNR        1         // Funktionen AGL_ReadMLFBNr, AGL_ReadMLFBNrEx
#define HAS_FUNC_READ_PLC_INFO      2         // Funktion AGL_ReadPLCInfo
#define HAS_FUNC_READ_OPSTATE       3         // Funktion AGL_ReadOpState
#define HAS_FUNC_PLC_START_STOP     4         // Funktionen AGL_PLCStart, AGL_PLCStop, AGL_PLCResume
#define HAS_FUNC_READ_DB_COUNT      5         // Funktion AGL_ReadDBCount
#define HAS_FUNC_READ_DB_LIST       6         // Funktion AGL_ReadDBList
#define HAS_FUNC_READ_DB_LEN        7         // Funktion AGL_ReadDBLen
#define HAS_FUNC_READ_DIAGBUFF      8         // Funktionen AGL_ReadDiagBufferEntrys, AGL_ReadDiagBuffer
#define HAS_FUNC_READ_CYCLETIME     9         // Funktion AGL_ReadCycleTime
#define HAS_FUNC_READ_PROTLEVEL     10        // Funktion AGL_ReadProtLevel
#define HAS_FUNC_PLC_CLOCK          11        // Funktionen AGL_GetPLCClock, AGL_SetPLCClock
#define HAS_FUNC_READ_MAXPACKET     12        // Funktion AGL_ReadMaxPacketSize

//
// Konstanten für AGL_InitDiagMsg
//
#define S7_DIAG_MSG_SYSTEM            0x02    // Systemdiagnoseereignisse
#define S7_DIAG_MSG_USER              0x04    // Anwenderdefinierte Diagnoseereignisse
#define S7_DIAG_MSG_CONTROL_TECH      0x08    // Leittechniksammelmeldungen


#define S7_SCAN_MAX_VALUES            80      // Maximale Variablenanzahl bei S7-Scan
#define S7_SCAN_MAX_ADD_VALUES        10      // Maximale Zusatzwerteanzahl bei S7-Scan
#define S7_SCAN_MAX_ADD_VALUE_LEN     4       // Maximale Länge Zusatzwerte bei S7-Scan

#define S7_ALARM_MAX_ADD_VALUES       10      // Maximale Zusatzwerteanzahl bei S7-Alarm
#define S7_ALARM_MAX_ADD_VALUE_LEN    432     // Maximale Länge Zusatzwert bei S7-Alarm

#define S7_USEND_URCV_MAX_VALUES      4       // Maximale Werteanzahl bei USEND/URCV
#define S7_USEND_URCV_MAX_VALUE_LEN   452     // Maximale Länge Wert bei USEND/URCV

//
// Konstanten für die Datentypen der Zusatzwerte bei S7-Alarm
//
#define S7_ALARM_AV_TYPE_ERROR        0x00
#define S7_ALARM_AV_TYPE_BOOLEAN      0x03
#define S7_ALARM_AV_TYPE_BITSTRING    0x04
#define S7_ALARM_AV_TYPE_INTEGER      0x05
#define S7_ALARM_AV_TYPE_FLOAT        0x07
#define S7_ALARM_AV_TYPE_OCTET_STRING 0x09
#define S7_ALARM_AV_TYPE_DATE         0x30
#define S7_ALARM_AV_TYPE_TIME_OF_DAY  0x31
#define S7_ALARM_AV_TYPE_TIME         0x32
#define S7_ALARM_AV_TYPE_S5TIME       0x33

//
// Konstanten für S7_RCV_MSG_STATE
//
#define S7_MSG_REASON_ACK             0x01    // Meldung wurde quittiert
#define S7_MSG_REASON_LOCK            0x02    // Meldung wurde gesperrt
#define S7_MSG_REASON_UNLOCK          0x03    // Meldung wurde freigegeben

//
// Konstanten für die Funktionen AGL_AckMsg, AGL_LockMsg und AGL_UnlockMsg
//
#define S7_MSG_TYPE_SYMB              0x05    // Für symbolbezogene Meldungen (Scan, ...)
#define S7_MSG_TYPE_SFB               0x06    // Für bausteinbezogene Meldungen aus SFBs
#define S7_MSG_TYPE_SFC               0x09    // Für bausteinbezogene Meldungen aus SFCs

//
// Und nun noch ein paar Konstanten für Infofunktionen
//
#define S7_IDENT_PLC_NAME             0x01    // Name des Automatisierungssystems
#define S7_IDENT_CPU_NAME             0x02    // Name der Baugruppe
#define S7_IDENT_PLANT_ID             0x03    // Anlagenkennzeichen der Baugruppe
#define S7_IDENT_COPYRIGHT            0x04    // Urheberrechtseintrag
#define S7_IDENT_SERIALNUMBER_CPU     0x05    // Seriennummer der Baugruppe
#define S7_IDENT_CPU_TYPE_NAME        0x07    // Baugruppen-Typname
#define S7_IDENT_SERIALNUMBER_MMC     0x08    // Seriennummer der Memory Card (nur S7-300)
#define S7_IDENT_OEM_ID               0x0A    // OEM-Kennung einer Baugruppe (nur S7-300)
#define S7_IDENT_LOC_ID               0x0B    // Ortskennzeichen einer Baugruppe


#define S7_LED_SF                     0x01    // SF (Sammelfehler)
#define S7_LED_INTF                   0x02    // INTF (interner Fehler)
#define S7_LED_EXTF                   0x03    // EXTF (externer Fehler)
#define S7_LED_RUN                    0x04    // RUN
#define S7_LED_STOP                   0x05    // STOP
#define S7_LED_FRCE                   0x06    // FRCE (Forcen)
#define S7_LED_CRST                   0x07    // CRST (Neustart)
#define S7_LED_BAF                    0x08    // BAF (Batteriefehler/Überlast, Kurzschluß von Batteriespannung am Bus)
#define S7_LED_USR                    0x09    // USR (anwenderdefiniert)
#define S7_LED_USR1                   0x0A    // USR1 (anwenderdefiniert)
#define S7_LED_BUS1F                  0x0B    // BUS1F (Busfehler Schnittstelle 1)
#define S7_LED_BUS2F                  0x0C    // BUS2F (Busfehler Schnittstelle 2)
#define S7_LED_RDF                    0x0D    // REDF (Redundanzfehler)
#define S7_LED_MSTR                   0x0E    // MSTR (Master)
#define S7_LED_RACK0                  0x0F    // RACK0 (Baugruppenträger-Nr. 0)
#define S7_LED_RACK1                  0x10    // RACK1 (Baugruppenträger-Nr. 1)
#define S7_LED_RACK2                  0x11    // RACK2 (Baugruppenträger-Nr. 2)
#define S7_LED_IFM1F                  0x12    // IFM1F (Schnittstellenfehler Interface-Modul 1)
#define S7_LED_IFM2F                  0x13    // IFM2F (Schnittstellenfehler Interface-Modul 2)
#define S7_LED_BUS3F                  0x14    // BUS3F (Busfehler Schnittstelle 3)
#define S7_LED_MAINT                  0x15    // MAINT (Maintenance / Wartung)

#define S7_LED_ON                     0x0001  // LED ist an
#define S7_LED_BLINK_NORMAL           0x0100  // LED blinkt normal (2 Hz)
#define S7_LED_BLINK_SLOW             0x0200  // LED blinkt langsam (0,5 Hz)


#define S7_OPT_WARMSTART              0x01    // Neustart
#define S7_OPT_COLDSTART              0x02    // Kaltstart
#define S7_OPT_RESUME                 0x04    // Wiederanlauf

#define S7_H_CPU_0                    0x00
#define S7_H_CPU_1                    0x01

//
// Ab hier für Fernwartungsmöglichkeiten
//
#define MDM_INIT_LEN                  64      // Länge eines Init-Eintrages
#define MDM_PHONE_NO_LEN              32      // Länge eines Telefonnummernteiles

#define AGL40_PARITY_NONE             'N'
#define AGL40_PARITY_EVEN             'E'
#define AGL40_PARITY_ODD              'O'

#define AGL40_HANDSHAKE_NONE          0       // NONE
#define AGL40_HANDSHAKE_HW            1       // RTS_CTS
#define AGL40_HANDSHAKE_SW            2       // XOFF_XON

#define BLK_TYPE_OB                   0x08
#define BLK_TYPE_DB                   0x0A
#define BLK_TYPE_SDB                  0x0B
#define BLK_TYPE_FC                   0x0C
#define BLK_TYPE_SFC                  0x0D
#define BLK_TYPE_FB                   0x0E
#define BLK_TYPE_SFB                  0x0F


//
// Beide Bits 0 = Beide Verbindungen verwenden
//
#define S7_RED_CONN_DONT_USE_NONE       0x00
#define S7_RED_CONN_DONT_USE_CONN1      0x01
#define S7_RED_CONN_DONT_USE_CONN2      0x02
#define S7_RED_CONN_DONT_USE_MASK       0x03

//
// Beide Bits 0 = H-CPU-System und dessen aktiven Master verwenden
//
#define S7_RED_CONN_HPLC_SYSTEM         0x00
#define S7_RED_CONN_USE_BOTH_PARALLEL   0x04
#define S7_RED_CONN_SECOND_ON_DEMAND    0x08
#define S7_RED_CONN_CONFIG_MASK         0x0C

#define S7_RED_CONN_NOT_CONNECTED       0x00
#define S7_RED_CONN_CONNECTED           0x01

//
// Flags für das zyklische Lesen von Variablen
//
#define S7_CYCLIC_READ_START_IMMEDIATE  0x01    // Zyklisches Lesen auch sofort starten
#define S7_CYCLIC_READ_ONLY_CHANGED     0x02    // Nur bei Änderungen Werte übermitteln,
                                                // dies funktioniert nur für Datenbytes (max. 255),
                                                // andere Operanden werden ganz normal zyklisch gelesen


/*******************************************************************************

 Konstanten für TCP/IP-Kommunikationsverbindungen

*******************************************************************************/

//
// Konstanten für Strukturelement Flags bei Verbindungstyp TYPE_RFC_1006
//
#define CONNECT_MASK                0x0001        // Maske zum Prüfen der Verbindungsart
#define CONNECT_ACTIVE		          0x0000		    // Aktiver Verbindungsaufbau (Client)
#define CONNECT_PASSIVE		          0x0001		    // Passiver Verbindungsaufbau (Server)

#define PROTO_MASK                  0x0002        // Maske zum Prüfen des Protokolle (future extension)
#define PROTO_TCP                   0x0000        // TCP verwenden
#define PROTO_UDP                   0x0002        // UDP verwenden (future extension)

//
// Konstanten für Strukturelement Flags bei Verbindungstyp TYPE_S7_TCPIP bzw. TYPE_RFC_1006
//
#define PORT_MASK                   0x0004        // Maske zum Prüfen der Portnummer
#define PORT_STANDARD               0x0000        // Standard-Portnummer verwenden
#define PORT_SPECIAL                0x0004        // Spezielle Portnummer verwenden

#define OWN_ADDR_MASK               0x0008        // Maske zum Prüfen der eigene IP-Adresse
#define OWN_ADDR_STANDARD           0x0000        // Keine eigene Adresse angeben
#define OWN_ADDR_SPECIAL            0x0008        // Eigene IP-Adresse angeben (z. B. bei mehreren Netzwerkkarten im PC)

#define OWN_PORT_MASK               0x0010        // Maske zum Prüfen der eigene Portnummer
#define OWN_PORT_STANDARD           0x0000        // Keine eigene Portnummer angeben
#define OWN_PORT_SPECIAL            0x0010        // Eigene Portnummer angeben

#define RFC1006_MASK                0x0020        // For internal use only
#define RFC1006_STANDARD            0x0000        // For internal use only
#define RFC1006_SPECIAL             0x0020        // For internal use only

#define RFC1006_ONLY_ONE_MASK       0x00C0        // Maske zum Prüfen der Anzahl Verbindungen pro gleicher Gegenstelle
#define RFC1006_NOT_ONLY_ONE        0x0000        // Alle Verbindungswünsche sind zulässig
#define RFC1006_ONLY_ONE            0x0040        // Nur eine Verbindung mit gleicher IP-Adresse, LTSAP und RTSAP zulässig
#define RFC1006_REJECT_NEW          0x0080        // Bei Gleichheit die neue Verbindung abweisen, ansonsten die alte Verbindung löschen

#define RFC1006_ACCEPT_MASK         0x0300        // Maske zum Prüfen der Verbindungsoptionen
#define RFC1006_ACCEPT_ALL          0x0000        // Alle Verbindungen annehmen
#define RFC1006_ACCEPT_KNOWN        0x0100        // Nur Verbindungen von benannter Gegenstelle annehmen
#define RFC1006_USE_SOCKET_COMMU    0x0200        // Transparentmodus ohne RFC 1006 für S5-TCP/IP



/*******************************************************************************

 Konstanten für die NCK-Funktionen

*******************************************************************************/

//
// Definition der Enums
//
enum NCK_Area
{
  eNCK_AreaUnknown          = -1,         // ?: Unbekannt
  eNCK_AreaNCK              =  0,         // N: NC Daten
  eNCK_AreaBag              =  1,         // B: Daten Betriebsartengruppe
  eNCK_AreaChannel          =  2,         // C: Kanalzugeordnete Daten
  eNCK_AreaAxis             =  3,         // A: Achsspezifische Grundeinstellungen
  eNCK_AreaTool             =  4,         // T: Wergzeugdaten
  eNCK_AreaFeedDrive        =  5,         // V: Vorschubantrieb
  eNCK_AreaMainDrive        =  6,         // H: Hauptantrieb
  eNCK_AreaMMC              =  7          // M: MMC-Daten
};

enum NCK_Block
{
  eNCK_BlockUnknown         = -1,         // Unbekannt
  eNCK_BlockY               = 0x10,       // Systemdaten
  eNCK_BlockYNCFL           = 0x11,       // NCK-Anweisungsgruppen
  eNCK_BlockFU              = 0x12,       // einstellbare Nullpunktverschiebung
  eNCK_BlockFA              = 0x13,       // aktive Nullpunktverschiebung
  eNCK_BlockTO              = 0x14,       // Schneidendaten, Korrekturdaten
  eNCK_BlockRP              = 0x15,       // Rechenparameter
  eNCK_BlockSE              = 0x16,       // Settingdaten
  eNCK_BlockSGUD            = 0x17,       // GUD, SGUD-Block
  eNCK_BlockLUD             = 0x18,       // LUD - lokale Benutzerdaten
  eNCK_BlockTC              = 0x19,       // Werkzeugträger-Parameter
  eNCK_BlockM               = 0x1A,       // Maschinendaten
  eNCK_BlockWAL             = 0x1C,       // CoordSysWorkAreaLimits
  eNCK_Block0x1B            = 0x1B,       // Unbekannt: z.B. in /nck/drive/techno_function_mask
  eNCK_BlockTISO            = 0x1D,       // ISO Werkzeug Korrekturdaten
  eNCK_BlockDIAG            = 0x1E,       // Diagnosedaten, die nur für entwicklungsinterne Zwecke
  eNCK_BlockCC              = 0x1F,       // Unbekannt
  eNCK_BlockFE              = 0x20,       // externe Nullpunktverschiebung
  eNCK_BlockTD              = 0x21,       // Werkzeugdaten, allgemeine Daten
  eNCK_BlockTS              = 0x22,       // Schneidendaten, Überwachungsdaten
  eNCK_BlockTG              = 0x23,       // Werkzeugdaten, schleifspezifische Daten
  eNCK_BlockTU              = 0x24,       // Werkzeugdaten, anwenderdefinierte Daten
  eNCK_BlockTUE             = 0x25,       // Schneidendaten, anwenderdefinierte Daten
  eNCK_BlockTV              = 0x26,       // Werkzeugdaten, Verzeichnis
  eNCK_BlockTM              = 0x27,       // Magazindaten, allgemeine Daten
  eNCK_BlockTP              = 0x28,       // Magazindaten, Platzdaten
  eNCK_BlockTPM             = 0x29,       // Magazindaten, Mehrfachzuordnung von Platzdaten
  eNCK_BlockTT              = 0x2A,       // Magazindaten, Platztypen
  eNCK_BlockTMV             = 0x2B,       // Magazindaten, Verzeichnis
  eNCK_BlockTMC             = 0x2C,       // Magazindaten, Konfigurationsdaten
  eNCK_BlockMGUD            = 0x2D,       // GD2, GUD2, MGUD-Block
  eNCK_BlockUGUD            = 0x2E,       // GD3, GUD3, UGUD-Block
  eNCK_BlockGUD4            = 0x2F,       // GD4, GUD4-Block
  eNCK_BlockGUD5            = 0x30,       // GD5, GUD5-Block
  eNCK_BlockGUD6            = 0x31,       // GD6, GUD6-Block
  eNCK_BlockGUD7            = 0x32,       // GD7, GUD7-Block
  eNCK_BlockGUD8            = 0x33,       // GD8, GUD8-Block
  eNCK_BlockGUD9            = 0x34,       // GD9, GUD9-Block
  eNCK_BlockPA              = 0x35,       // Schutzbereiche
  eNCK_BlockGD1             = 0x36,       // SGUD-Block GD1
  eNCK_BlockNIB             = 0x37,       // Nibbling
  eNCK_BlockETP             = 0x38,       // Event-Typen
  eNCK_BlockETPD            = 0x39,       // Datenlisten für die Protokollierung
  eNCK_BlockSYNACT          = 0x3A,       // Kanalspezifische Synchronaktionen
  eNCK_BlockDIAGN           = 0x3B,       // Diagnosebaustein
  eNCK_BlockVSYN            = 0x3C,       // NCK-spezifische Anwendervariablen für Synchronaktion
  eNCK_BlockTUS             = 0x3D,       // Überwachungsanwenderdaten
  eNCK_BlockTUM             = 0x3E,       // Magazin-Anwenderdaten
  eNCK_BlockTUP             = 0x3F,       // Magazinplatz-Anwenderdaten
  eNCK_BlockTF              = 0x40,       // Tool/Find
  eNCK_BlockFB              = 0x41,       // Basisframe: einstellbarer Frame, der immer wirkt
  eNCK_BlockSSP2            = 0x42,       // Spindelzustandsdaten bei Spindelumsetzung
  eNCK_BlockPUD             = 0x43,       // PUD - programmglobale Benutzerdaten
  eNCK_BlockTOS             = 0x44,       // Schneidenbezogene ortsabhängige Summenkorrekturen
  eNCK_BlockTOST            = 0x45,       // Schneidenbezogene ortsabhängige Summenkorrekturen
  eNCK_BlockTOE             = 0x46,       // Schneidenbezogene ortsabhängige Summenkorrekturen
  eNCK_BlockTOET            = 0x47,       // Schneidenbezogene ortsabhängige Summenkorrekturen
  eNCK_BlockAD              = 0x48,       // Adapterdaten
  eNCK_BlockTOT             = 0x49,       // Schneidendaten, transformierte Korrekturdaten
  eNCK_BlockAEV             = 0x4A,       // Arbeitskorrekturen: Verzeichnis
  eNCK_BlockYFAFL           = 0x4B,       // NCK-Anweisungsgruppen Fanuc
  eNCK_BlockFS              = 0x4C,       // System-Frame
  eNCK_BlockSD              = 0x4D,       // Servo-Daten
  eNCK_BlockTAD             = 0x4E,       // Applikationsspezifische Daten
  eNCK_BlockTAO             = 0x4F,       // Applikationsspezifische Schneidendaten
  eNCK_BlockTAS             = 0x50,       // Applikationsspezifische Überwachungsdaten
  eNCK_BlockTAM             = 0x51,       // Applikationsspezifische Magazindaten
  eNCK_BlockTAP             = 0x52,       // Applikationsspezifische Magazinplatzdaten
  eNCK_BlockMEM             = 0x53,       // Unbekannt
  eNCK_BlockSALAC           = 0x54,       // Alarm-Ereignisse, ältestes zuerst
  eNCK_BlockAUXFU           = 0x55,       // Aktive Hilfsfunktionen
  eNCK_BlockTDC             = 0x56,       // /Tool/Tools
  eNCK_BlockCP              = 0x57,       // GenericCoupling
  eNCK_BlockMTV             = 0x59,       // Multi-tool data, directory
  eNCK_BlockMTD             = 0x5A,       // Multi-tool data, general data
  eNCK_BlockMTP             = 0x5B,       // Multi-tool data, place data
  eNCK_BlockMTUD            = 0x5C,       // Multi-tool data, user-defined data
  eNCK_BlockMTUP            = 0x5D,       // Multi-tool place user data
  eNCK_BlockMTAD            = 0x5E,       // Application-specific multi-tool data
  eNCK_BlockMTAP            = 0x5F,       // Application-specific multi-tool place data
  eNCK_BlockCCS             = 0x6D,       // Siemens Compilezyklen
  eNCK_BlockSDME            = 0x6E,       // Unbekannt
  eNCK_BlockSPARPI          = 0x6F,       // Programmzeiger bei Unterbrechung
  eNCK_BlockSEGA            = 0x70,       // erweiterte Zustandsdaten im WKS
  eNCK_BlockSEMA            = 0x71,       // erweiterte Zustandsdaten im MKS
  eNCK_BlockSSP             = 0x72,       // Zustandsdaten Spindel
  eNCK_BlockSGA             = 0x73,       // Zustandsdaten im WKS
  eNCK_BlockSMA             = 0x74,       // Zustandsdaten im MKS
  eNCK_BlockSALAL           = 0x75,       // Nck/LastAlarm
  eNCK_BlockSALAP           = 0x76,       // Nck/TopPrioAlarm
  eNCK_BlockSALA            = 0x77,       // Nck/SequencedAlarms
  eNCK_BlockSSYNAC          = 0x78,       // Synchronaktionen
  eNCK_BlockSPARPF          = 0x79,       // Programmzeiger für Satzsuchlauf
  eNCK_BlockSPARPP          = 0x7A,       // Programmzeiger im Automatikbetrieb
  eNCK_BlockSNCF            = 0x7B,       // aktive G-Funktionen
  eNCK_BlockSPARP           = 0x7D,       // Teileprogramminformation
  eNCK_BlockSINF            = 0x7E,       // teileprogrammspezifische Zustandsdaten
  eNCK_BlockS               = 0x7F,       // Zustandsdaten
  eNCK_Block0x80            = 0x80,       // Unbekannt - moeglicherweise Antriebsvariablen
  eNCK_Block0x81            = 0x81,       // Unbekannt
  eNCK_Block0x82            = 0x82,       // Unbekannt - moeglicherweise Antriebsvariablen
  eNCK_Block0x83            = 0x83,       // Unbekannt
  eNCK_Block0x84            = 0x84,       // Unbekannt
  eNCK_Block0x85            = 0x85,       // Unbekannt
  eNCK_BlockO               = 0xFD        // Intern
};

enum NCK_DDEVarFormat
{
  eNCK_Unknown              = 0,          // Wird mal als unbekannt gewertet

  //
  // Zuerst die Typen im Little-Endian-Format auf der NCK ...
  //
  eNCK_LE_Uint8             =  1,         // Unsigned Int 8 Bit
  eNCK_LE_Uint16            =  2,         // Unsigned Int 16 Bit
  eNCK_LE_Int32             =  3,         // Signed Int 32 Bit
  eNCK_LE_Float32           =  4,         // Float 32 Bit
  eNCK_LE_Float64           =  5,         // Float 64 Bit
  eNCK_LE_String            =  6,         // String nullterminiert
  eNCK_LE_Bit               =  7,         // Boolean
  eNCK_LE_Int8              =  8,         // Signed Int 8 Bit
  eNCK_LE_Int16             =  9,         // Signed Int 16 Bit
  eNCK_LE_Uint32            = 10,         // Unsigned Int 32 Bit

  //
  // ... dann die Typen im Big-Endian-Format auf der PLC ...
  // ... diese können direkt mit den AGLink-PLC-Funktionen gelesen werden ...
  // ... nicht mit den AGLink-NCK-Funktionen ...
  //
  eNCK_BE_Uint8             = 11,         // Unsigned Int 8 Bit
  eNCK_BE_Uint16            = 12,         // Unsigned Int 16 Bit
  eNCK_BE_Int32             = 13,         // Signed Int 32 Bit
  eNCK_BE_Float32           = 14,         // Float 32 Bit
  eNCK_BE_Float64           = 15,         // Float 64 Bit
  eNCK_BE_String            = 16,         // String nullterminiert
  eNCK_BE_Bit               = 17,         // Boolean
  eNCK_BE_Int8              = 18,         // Signed Int 8 Bit
  eNCK_BE_Int16             = 19,         // Signed Int 16 Bit
  eNCK_BE_Uint32            = 20          // Unsigned Int 32 Bit

  //
  // ... und eine negative Zahl bedeutet Länge der endian transparenten Datenstruktur
  //
};

//
// Die Formate der NCK sind alle Little-Endian
//
enum NCK_MDBVarFormat
{
  eNCK_MDB_Unknown          = -1,         // Wird mal als unbekannt gewertet
  eNCK_MDB_Bit              =  0,         // Boolean
  eNCK_MDB_Int32            =  1,         // Signed Int 32
  eNCK_MDB_Float64          =  2,         // Float 64 Bit (Double)
  eNCK_MDB_Int8             =  3,         // Char
  eNCK_MDB_String           =  4,         // String nullterminiert
  eNCK_MDB_DateAndTime      =  8,         // Date And Time
  eNCK_MDB_Uint16           = 11,         // Unsinged Int 16 Bit
  eNCK_MDB_Int16            = 12,         // Signed Int 16 Bit
  eNCK_MDB_Uint32           = 13,         // Unsigned Int 32 Bit
  eNCK_MDB_Float32          = 14          // Float 32 Bit
};

//
// Definition fuer AGL_NCK_ExtractNckAlarm
//
#define NCK_ALARM_TEXT_TYPE_UNDEFINED 0
#define NCK_ALARM_TEXT_TYPE_GENERAL_STRING 1
#define NCK_ALARM_TEXT_TYPE_AXIS_SPINDLE_NAME 2
#define NCK_ALARM_TEXT_TYPE_CHANNEL_NAME 3
#define NCK_ALARM_TEXT_TYPE_BLOCK_NAME 4
#define NCK_ALARM_TEXT_TYPE_SYSTEM_ERROR 5
#define NCK_ALARM_TEXT_TYPE_DRIVE_NUMBER 6
#define NCK_ALARM_TEXT_TYPE_INDEX_NUMBER 7

#define NCK_ALARM_CLEARED_UNDEFINED 0
#define NCK_ALARM_CLEARED_ON_POWER_ON 1
#define NCK_ALARM_CLEARED_ON_RESET 2
#define NCK_ALARM_CLEARED_ON_CANCEL 3
#define NCK_ALARM_CLEARED_BY_NCK 4
#define NCK_ALARM_CLEARED_BY_RESET_IN_ALL_CHANNELS_OF_BAGS 5
#define NCK_ALARM_CLEARED_BY_RESET_IN_ALL_CHANNELS_OF_NC 6

//
// Definition fuer AGL_NCK_PI_COPY
//
#define NCK_COPY_PASSIVE_FILESYSTEM_TO_PASSIVE_FILESYSTEM 1 // Passives Dateisystem nach passives Dateisystem
#define NCK_COPY_ACTIVE_FILESYSTEM_TO_PASSIVE_FILESYSTEM 2 // Aktives Dateisystem nach passives Dateisystem
#define NCK_COPY_PASSIVE_FILESYSTEM_TO_ACTIVE_FILESYSTEM 3 // Passives Dateisystem nach aktives Dateisystem
#define NCK_COPY_ACTIVE_FILESYSTEM_TO_ACTIVE_FILESYSTEM 4 // Aktives Dateisystem nach aktives Dateisystem
#define NCK_COPY_PASSIVE_FILESYSTEM_TO_PASSIVE_FILESYSTEM_WITH_OVERWRITE 5 // Passives Dateisystem nach passives Dateisystem, ggf. wird Zieldatei gelöscht (Ab Software $[[SW130000]])
#define NCK_COPY_ACTIVE_FILESYSTEM_TO_PASSIVE_FILESYSTEM_WITH_OVERWRITE 6 // Aktives Dateisystem nach passives Dateisystem, ggf. wird Zieldatei gelöscht (Ab Software $[[SW130000]])

//
// Definition fuer AGL_NCK_PI_IBN_SS
//
#define NCK_IBN_SS_NORMAL 0 // Normal (Power On)
#define NCK_IBN_SS_DELETE_SRAM 1 // S-Ram löschen
#define NCK_IBN_SS_SOFTWARE_UPDATE 2 // Software Update
#define NCK_IBN_SS_NCK_SHUTDOWN 8 // NCK-Shutdown
#define NCK_IBN_SS_PC_SHUTDOWN 9 // PC-Shutdown

//
// Definition fuer AGL_NCK_PI_MMCSEM
//
#define NCK_MMCSEM_TMCRTO 1 // Werkzeug anlegen
#define NCK_MMCSEM_TMDPL 2 // Leerplatzsuche zum Beladen
#define NCK_MMCSEM_TMMVTL 3 // Werkzeug beladen / bewegen
#define NCK_MMCSEM_TMFPBP 4 // Suche Platz
#define NCK_MMCSEM_TMGETT 5 // Suche Werkzeugnummer
#define NCK_MMCSEM_TSEARC 6 // Werkzeug suchen
#define NCK_MMCSEM_MEMSIZE 7 // Synchronisation des Zugriffs auf die BTSS-Variable /N/S/memSize

/*******************************************************************************

 Konstanten für die Antriebs-Funktionen

*******************************************************************************/

//
// Operandentypen für die DATA_RW40_DRIVE-Struktur
//

#define DTYP_UNKNOWN        0
#define DTYP_BIT            1
#define DTYP_BYTE           2
#define DTYP_INT8           3
#define DTYP_WORD           4
#define DTYP_INT16          5
#define DTYP_DWORD          6
#define DTYP_INT32          7
#define DTYP_REAL           8

enum DOpType
{
  eDOT_Unknown              = DTYP_UNKNOWN,   // Operandentype unbekannt
  eDOT_Bit                  = DTYP_BIT,       // Operandentype Bit
  eDOT_Byte                 = DTYP_BYTE,      // Operandentype Byte
  eDOT_Int8                 = DTYP_INT8,      // Operandentype Int8
  eDOT_Word                 = DTYP_WORD,      // Operandentype Word
  eDOT_Int16                = DTYP_INT16,     // Operandentype Int16
  eDOT_DWord                = DTYP_DWORD,     // Operandentype DWord
  eDOT_Int32                = DTYP_INT32,     // Operandentype Int32
  eDOT_Real                 = DTYP_REAL       // Operandentype Real
};


/*******************************************************************************

 Konstanten für die Funktionen mit .WLD Dateien

*******************************************************************************/

//
// Konstanten für Parameter Access bei Funktion AGL_WLD_OpenFile
//
#define WLD_ACCESS_READ      0x001 // Nur-Lese-Zugriff
#define WLD_ACCESS_WRITE     0x002 // Lese- und Schreib-Zugriff
#define WLD_ACCESS_CREATE    0x004 // Datei ggf. erzeugen, impliziert Lese- und Schreib-Zugriff
#define WLD_ACCESS_TRUNCATE  0x008 // Dateiinhalt rücksetzen, impliziert Lese- und Schreib-Zugriff

//
// Zusatzoption zur Behandlung mehrfach vorhandener Bausteine
// Ist keine der nachfolgenden Optionen gesetzt, so führt das Vorkommen
// mehrfach vorhandener Bausteine zum Fehlschlag der Funktion AGL_WLD_OpenFile
//
#define WLD_FLAG_LAST_DUP    0x100 // Mehrfach vorhandener Baustein: letztes Vorkommen gilt
#define WLD_FLAG_FIRST_DUP   0x200 // Mehrfach vorhandener Baustein: erstes Vorkommen gilt


/*******************************************************************************

 Konstanten für die Symbolik-Funktionen

*******************************************************************************/

#if !defined( AGLSYM_FUNC_NOT_ALLOWED ) // Wegen AGLink Version 3.x
//
// Fehlerkonstanten
//
#define AGLSYM_SUCCESS                      AGL40_SUCCESS

#define AGLSYM_FUNC_NOT_ALLOWED             (ERROR_CLASS_SYM+0)
#define AGLSYM_INVALID_HANDLE               (ERROR_CLASS_SYM+1)
#define AGLSYM_OPEN_FAILURE_PRJ             (ERROR_CLASS_SYM+2)
#define AGLSYM_OPEN_FAILURE_PRG             (ERROR_CLASS_SYM+3)
#define AGLSYM_CLOSE_FAILURE                (ERROR_CLASS_SYM+4)
#define AGLSYM_NO_PROGRAM                   (ERROR_CLASS_SYM+5)
#define AGLSYM_PROGRAM_NOTFOUND             (ERROR_CLASS_SYM+6)
#define AGLSYM_PARA_ERR                     (ERROR_CLASS_SYM+7)
#define AGLSYM_EXPIRED                      (ERROR_CLASS_SYM+8)
#define AGLSYM_INVALID_DATA                 (ERROR_CLASS_SYM+10)
#define AGLSYM_NO_SYMBOLTABLE               (ERROR_CLASS_SYM+11)
#define AGLSYM_SYMBOLTABLE_OPEN             (ERROR_CLASS_SYM+12)
#define AGLSYM_NO_SYMBOL                    (ERROR_CLASS_SYM+13)
#define AGLSYM_SYMBOL_READ_ERROR            (ERROR_CLASS_SYM+14)
#define AGLSYM_SYMBOL_NOT_FOUND             (ERROR_CLASS_SYM+15)
#define AGLSYM_OPERAND_NOT_FOUND            (ERROR_CLASS_SYM+16)
#define AGLSYM_SYMBOL_INVALID               (ERROR_CLASS_SYM+17)
#define AGLSYM_OPERAND_INVALID              (ERROR_CLASS_SYM+18)
#define AGLSYM_INVALID_FILTER               (ERROR_CLASS_SYM+19)
#define AGLSYM_NO_DB                        (ERROR_CLASS_SYM+20)
#define AGLSYM_READ_DB_FAILURE              (ERROR_CLASS_SYM+21)
#define AGLSYM_PARSE_DB_FAILURE             (ERROR_CLASS_SYM+22)
#define AGLSYM_SYMBOLDB_OPEN                (ERROR_CLASS_SYM+23)
#define AGLSYM_NO_SYMBOLDB                  (ERROR_CLASS_SYM+24)
#define AGLSYM_NO_DBSYMBOL                  (ERROR_CLASS_SYM+25)
#define AGLSYM_DBSYMBOL_NOT_FOUND           (ERROR_CLASS_SYM+26)
#define AGLSYM_DBSYMBOL_INVALID             (ERROR_CLASS_SYM+27)
#define AGLSYM_DBSYMBOL_NOT_MATCH           (ERROR_CLASS_SYM+28)
#define AGLSYM_CONSTANT_INVALID             (ERROR_CLASS_SYM+30)
#define AGLSYM_CONSTANT_FMT_INVALID         (ERROR_CLASS_SYM+31)
#define AGLSYM_INTERNAL_INIT_FAILED         (ERROR_CLASS_SYM+32)
#define AGLSYM_INVALID_ALM_VERSION          (ERROR_CLASS_SYM+33)
#define AGLSYM_OPEN_FAILURE_ALM             (ERROR_CLASS_SYM+34)
#define AGLSYM_CLOSE_FAILURE_ALM            (ERROR_CLASS_SYM+35)
#define AGLSYM_NO_ALM_DATA                  (ERROR_CLASS_SYM+36)
#define AGLSYM_NO_LANGUAGE                  (ERROR_CLASS_SYM+37)
#define AGLSYM_DBERR_ALM_DATA               (ERROR_CLASS_SYM+38)
#define AGLSYM_INVALID_SIGNAL_NO            (ERROR_CLASS_SYM+39)
#define AGLSYM_INVALID_ADDVAL               (ERROR_CLASS_SYM+40)
#define AGLSYM_INVALID_ADDVAL_NO            (ERROR_CLASS_SYM+41)
#define AGLSYM_INVALID_ELEM_TYPE            (ERROR_CLASS_SYM+42)
#define AGLSYM_ADDVAL_DATA_INVALID          (ERROR_CLASS_SYM+43)
#define AGLSYM_NO_ADDVAL_FORMAT             (ERROR_CLASS_SYM+44)
#define AGLSYM_ADDVAL_FORMAT_INVALID        (ERROR_CLASS_SYM+45)
#define AGLSYM_NO_TEXTLIB                   (ERROR_CLASS_SYM+46)
#define AGLSYM_NO_TEXTENTRY                 (ERROR_CLASS_SYM+47)
#define AGLSYM_NOT_ENOUGH_MEMORY            (ERROR_CLASS_SYM+48)
#define AGLSYM_INVALID_BLKTYPE              (ERROR_CLASS_SYM+49)
#define AGLSYM_INVALID_LIST_HANDLE          (ERROR_CLASS_SYM+50)
#define AGLSYM_NO_BLK                       (ERROR_CLASS_SYM+51)
#define AGLSYM_READ_BLK_FAILURE             (ERROR_CLASS_SYM+52)
#define AGLSYM_PARSE_BLK_FAILURE            (ERROR_CLASS_SYM+53)
#define AGLSYM_NO_DECLARATION               (ERROR_CLASS_SYM+54)
#define AGLSYM_NO_ATTRIBUTES                (ERROR_CLASS_SYM+55)
#define AGLSYM_WRITECPULIST_FAILED          (ERROR_CLASS_SYM+56)

//
// Format-Konstanten für Operanden
//
#define AGLSYM_FORMAT_NONE          -1
#define AGLSYM_FORMAT_BOOL           1
#define AGLSYM_FORMAT_BYTE           2
#define AGLSYM_FORMAT_CHAR           3
#define AGLSYM_FORMAT_WORD           4
#define AGLSYM_FORMAT_INT            5
#define AGLSYM_FORMAT_DWORD          6
#define AGLSYM_FORMAT_DINT           7
#define AGLSYM_FORMAT_REAL           8
#define AGLSYM_FORMAT_DATE           9
#define AGLSYM_FORMAT_TIMEOFDAY     10
#define AGLSYM_FORMAT_TIME          11
#define AGLSYM_FORMAT_S5TIME        12

#define AGLSYM_FORMAT_COUNTER       28
#define AGLSYM_FORMAT_TIMER         29

#define AGLDBSYM_FORMAT_DATEANDTIME 14  // DATE_AND_TIME bei DB-Symbolen, Groesse  8 Bytes
#define AGLDBSYM_FORMAT_STRING      19  // STRING        bei DB-Symbolen, Groesse variabel

#define AGLDBSYM_FORMAT_POINTER     20  // POINTER       bei DB-Symbolen, Groesse  6 Bytes
#define AGLDBSYM_FORMAT_ANY         22  // ANY           bei DB-Symbolen, Groesse 10 Bytes


//
// Konstanten für Textlänge der Daten für die Symbole und DB-Symbole
//
#define AGLSYM_PROG_LEN            128
#define AGLSYM_ABSOP_LEN            40
#define AGLSYM_SYMB_LEN            256
#define AGLSYM_COMMENT_LEN          84

#endif    // #if !defined( AGLSYM_FUNC_NOT_ALLOWED ) // Wegen AGLink Version 3.x

#define AGLSYM_EXCOMMENT_LEN      1024
#define AGLSYM_DTYPE_LEN            16

//
// Konstanten für Elementanzahl und Textlänge der Daten für die Meldungskonfiguration
//
#define AGLSYM_ALARM_TEXT_LEN     256 // 255 + 1
#define AGLSYM_ALARM_ADDTEXT_NUM    9 // Anzahl Zusatztexte (max.)

#define AGLSYM_ALARM_NAME_LEN       28 // 24 + 1 gerundet auf DWORD-Alignement
#define AGLSYM_ALARM_SIGNAL_NUM      8 // Anzahl Siagnale (max.)
#define AGLSYM_ALARM_ADDVALUE_NUM   10 // Anzahl Begeleitwerte (max.) für SCAN-Meldungen

//
// Konstanten für Textlängen bei Deklarationsauflistung
//
#define AGLSYM_ADDRESS_LEN           12
#define AGLSYM_MEMCLASS_LEN          16
#define AGLSYM_DEPTH_LEN             12
#define AGLSYM_NAME_LEN              32
#define AGLSYM_TYPE_LEN              96
#define AGLSYM_INITVAL_LEN           32

//
// Darstellungsart-Konstanten für Konstanten
//
#define AGL_VALUE_LEN               64
#define AGL_VALUEFMT_DEFAULT       -1
#define AGL_VALUEFMT_DEC            0     // <Dezimalwert m. Vorzeichen>    (BOOL/BYTE/WORD/DWORD)
#define AGL_VALUEFMT_BIN            1     // 2#...                          (BOOL/BYTE/WORD/DWORD)
#define AGL_VALUEFMT_HEX            2     // B#16#..., W#16#..., DW#16#...       (BYTE/WORD/DWORD)
#define AGL_VALUEFMT_CHAR           3     // '<1, 2, 3 oder 4 Zeichen>'          (BYTE/WORD/DWORD)
#define AGL_VALUEFMT_BOOL           4     // TRUE bzw. FALSE                                (BOOL)
#define AGL_VALUEFMT_S5TIME         5     // SIMATIC-ZEIT (S5T#...) >=10ms      bcd-kodiert (WORD)
#define AGL_VALUEFMT_COUNTER        6     // SIMATIC-ZÄHLWERT (C#...) 3 stellig bcd-kodiert (WORD)
#define AGL_VALUEFMT_DATE           7     // DATUM (D#...) Tage seit 1.1.1990               (WORD)
#define AGL_VALUEFMT_REAL           8     // <Gleitkommawert>                              (DWORD)
#define AGL_VALUEFMT_TIME           9     // IEC-ZEIT (T#...) vorzeichenbehaftet in ms     (DWORD)
#define AGL_VALUEFMT_TIMEOFDAY      10    // TAGESZEIT (TOD#...) ms seit Mitternacht       (DWORD)
#define AGL_VALUEFMT_POINTER        11    // ZEIGER (P#...)                                (DWORD)


//
// Konstanten für die Befehls- bzw. Operandensprache
//
#define AGLSYM_LANGUAGE_SIMATIC     0 // SIMATIC / deutsch    (Voreinstellung)
#define AGLSYM_LANGUAGE_IEC         1 // IEC     / englisch


//
// Konstanten für die Einstellung "Meldeklasse"
//
#define AGLSYM_MSGCLS_NO_MESSAGE                             0   // keine Meldung
#define AGLSYM_MSGCLS_ALARM_ABOVE                            1   // Alarm - oben
#define AGLSYM_MSGCLS_ALARM_BELOW                            2   // Alarm - unten
#define AGLSYM_MSGCLS_WARNING_ABOVE                          3   // Warnung - oben
#define AGLSYM_MSGCLS_WARNING_BELOW                          4   // Warnung - unten
#define AGLSYM_MSGCLS_TOLERANCE_ABOVE                        5   // Toleranz - oben
#define AGLSYM_MSGCLS_TOLERANCE_BELOW                        6   // Toleranz - unten
#define AGLSYM_MSGCLS_PLC_PROCESS_CONTROL_MESSAGE_FAILURE    7   // AS-Leittechnik Meldung-Störung
#define AGLSYM_MSGCLS_PLC_PROCESS_CONTROL_MESSAGE_ERROR      8   // AS-Leittechnik Meldung-Fehler
#define AGLSYM_MSGCLS_OS_PROCESS_CONTROL_MESSAGE_FAILURE     9   // OS-Leittechnik Meldung-Störung
#define AGLSYM_MSGCLS_PREVENTATIVE_MAINTAINANCE_STANDARD    10   // Vorbeugende Wartung - allgemein
#define AGLSYM_MSGCLS_PROCESS_MESSAGE_WITH_ACKNOWLEDGEMENT  11   // Prozessmeldung - mit Quittierung
#define AGLSYM_MSGCLS_EVENT_MESSAGE_WITHOUT_ACKNOWLEDGEMENT 12   // Betriebsmeldung - ohne Quittierung
#define AGLSYM_MSGCLS_OPERATOR_PROMPT_STANDARD              13   // Bedienanforderung - allgemein
#define AGLSYM_MSGCLS_OPERATING_MESSAGE_STANDARD            14   // Bedienmeldung - allgemein
#define AGLSYM_MSGCLS_STATUS_MESSAGE_PLC                    15   // Statusmeldung - AS
#define AGLSYM_MSGCLS_STATUS_MESSAGE_OS                     16   // Statusmeldung - OS

//
// Konstanten für die Einstellung "Quittiergruppe"
//
#define AGLSYM_ACKGRP_SINGLE_ACKNOWLEDGEMENT    0   //  Einzelquittierung
#define AGLSYM_ACKGRP_1                         1   //  1
#define AGLSYM_ACKGRP_2                         2   //  2
#define AGLSYM_ACKGRP_3                         3   //  3
#define AGLSYM_ACKGRP_4                         4   //  4
#define AGLSYM_ACKGRP_5                         5   //  5
#define AGLSYM_ACKGRP_6                         6   //  6
#define AGLSYM_ACKGRP_7                         7   //  7
#define AGLSYM_ACKGRP_8                         8   //  8
#define AGLSYM_ACKGRP_9                         9   //  9
#define AGLSYM_ACKGRP_10                       10   // 10
#define AGLSYM_ACKGRP_11                       11   // 11
#define AGLSYM_ACKGRP_12                       12   // 12
#define AGLSYM_ACKGRP_13                       13   // 13
#define AGLSYM_ACKGRP_14                       14   // 14
#define AGLSYM_ACKGRP_15                       15   // 15
#define AGLSYM_ACKGRP_16                       16   // 16

//
// Konstanten für Bausteintypen bei Deklarationsauflistung
//
#define AGLSYM_BLKTYPE_DB             0
#define AGLSYM_BLKTYPE_FB             1
#define AGLSYM_BLKTYPE_FC             2
#define AGLSYM_BLKTYPE_OB             3
#define AGLSYM_BLKTYPE_UDT            4
#define AGLSYM_BLKTYPE_SFB            5
#define AGLSYM_BLKTYPE_SFC            6

//
// Konstanten für Speicherart bei Deklarationsauflistung
//
#define AGLSYM_MEMCLASS_NONE          0 // Unbekannt/ungueltig
#define AGLSYM_MEMCLASS_IN            1 // IN
#define AGLSYM_MEMCLASS_OUT           2 // OUT
#define AGLSYM_MEMCLASS_IN_OUT        3 // IN_OUT
#define AGLSYM_MEMCLASS_STAT          4 // STAT
#define AGLSYM_MEMCLASS_TEMP          5 // TEMP
#define AGLSYM_MEMCLASS_RET_VAL       6 // RET_VAL (nur bei FC/SFC)
#define AGLSYM_MEMCLASS_STAT_IN       9 // STAT:IN (nur bei FB-Multiinstanzen)
#define AGLSYM_MEMCLASS_STAT_OUT     10 // STAT:OUT (nur bei FB-Multiinstanzen)
#define AGLSYM_MEMCLASS_STAT_IN_OUT  11 // STAT:IN_OUT (nur bei FB-Multiinstanzen)
#define AGLSYM_MEMCLASS_STAT_STAT    12 // STAT:STAT (nur bei FB-Multiinstanzen)

//
// Konstanten für Datentyp bei Deklarationsauflistung
//
#define AGLSYM_TYPE_NONE              0
#define AGLSYM_TYPE_BOOL              1
#define AGLSYM_TYPE_BYTE              2
#define AGLSYM_TYPE_CHAR              3
#define AGLSYM_TYPE_WORD              4
#define AGLSYM_TYPE_INT               5
#define AGLSYM_TYPE_DWORD             6
#define AGLSYM_TYPE_DINT              7
#define AGLSYM_TYPE_REAL              8
#define AGLSYM_TYPE_DATE              9
#define AGLSYM_TYPE_TIMEOFDAY        10
#define AGLSYM_TYPE_TIME             11
#define AGLSYM_TYPE_S5TIME           12
#define AGLSYM_TYPE_DATEANDTIME      14
#define AGLSYM_TYPE_STRUCT           17
#define AGLSYM_TYPE_UDT              18
#define AGLSYM_TYPE_STRING           19
#define AGLSYM_TYPE_ARRAY            16
#define AGLSYM_TYPE_FB               21
#define AGLSYM_TYPE_SFB              27
#define AGLSYM_TYPE_TIMER            29
#define AGLSYM_TYPE_COUNTER          28
#define AGLSYM_TYPE_BLOCKFB          23
#define AGLSYM_TYPE_BLOCKFC          24
#define AGLSYM_TYPE_BLOCKDB          25
#define AGLSYM_TYPE_BLOCKSDB         26
#define AGLSYM_TYPE_POINTER          20
#define AGLSYM_TYPE_ANY              22
#define AGLSYM_TYPE_EMPTY            41
#define AGLSYM_TYPE_END_STRUCT       42
  
//
// AdditionalInfo = Zusatzdaten wie z.B. ARRAY-Dimensionsangaben, STRING-Dimension, UDT/FB/SFB-Nummer etc.
// Inhalt ist speziell an den jeweiligen Datentyp angepasst, z.B. bei STRING die Dimension,
// bei UDT/FB/SFB die Bausteinnummer, bei ARRAY die Dimensionsangaben wie folgt:
// AdditionalInfo[0] = Anzahl Dimensionen (max. 6 lt. Siemens), dann fuer jede Dimension jeweils die Dimensions-Untergrenze (UG)
// und die Dimensions-Obergrenze (OG), also AdditionalInfo[1] = UG Dimension 1, AdditionalInfo[2] = OG Dimension 1, ...,
// AdditionalInfo[11] = UG Dimension 6, AdditionalInfo[12] = OG Dimension 6
//

// Groesse der Zusatzdaten zum Datentyp
#define AGLSYM_ADDITIONAL_INFO_LEN   14

#endif  // #if !defined( __AGL_DEFINES__ )
