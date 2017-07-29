/*******************************************************************************

 Projekt        : Neue Version der AGLink-Bibliothek

 Dateiname      : AGL_SymbolicTypes.h

 Beschreibung   : Definition der TIA Symbolik Datentypen

 Copyright      : (c) 1998-2017
                  DELTALOGIC Automatisierungstechnik GmbH
                  Stuttgarter Str. 3
                  73525 Schwäbisch Gmünd
                  Web : http://www.deltalogic.de
                  Tel.: +49-7171-916120
                  Fax : +49-7171-916220

 Erstellt       : 27.07.2015 JBa

 Geändert       : 05.01.2017 JBa

 *******************************************************************************/

#if !defined( __AGL_SYMBOLIC_TYPES__ )
#define __AGL_SYMBOLIC_TYPES__

/*******************************************************************************

 Strukturen für die 1200, 1500 Symbolik-Funktionen

 *******************************************************************************/

#if defined( _MSC_VER ) && _MSC_VER > 600
#pragma pack( push, 1 )
#elif !defined( _UCC )
#pragma pack( 1 )
#endif


enum 
{
  MAX_NAME_SIZE = 256,
  MAX_STRING8_LENGTH = 254,
  MAX_STRING16_LENGTH = 16382,
  MAX_COMMENT_LENGTH = 32768
};

struct SymbolicRW_t
{
  HandleType AccessHandle;
  void* Buffer; //Byte Datenbuffer
  agl_long_t BufferLen; //Datenbufferlaenge in Bytes
  agl_int32_t Result; // Symbol Fehlercode
  agl_int32_t SError; // Siemens Fehlercode
};


struct NodeType_t
{
  enum enum_t
  {
    nt_UNDEFINED,
    nt_SCHEMA,
    nt_INSTANCE,
    
// backward compatibility - please use above constants with prefix
#if defined(AGL_USE_LEGACY_TYPES)
    UNDEFINED = nt_UNDEFINED,
    SCHEMA    = nt_SCHEMA,
    INSTANCE  = nt_INSTANCE
#endif
  };
};

struct AccessType_t
{
  enum enum_t
  {
    at_DEFAULT, // 
    
    //possible with 300/400 and with PUT/GET active 1200,1500 -> ueber 0x32
    at_S7_CLASSIC,
    
    //possible with 1200,1500
    at_S7_NEXT, // S7_NEXT/S7_PLUS, - TIA, OPC UA, WINCC...

    at_SINUMERIK, //Sinumerik 840D PL/SL

// backward compatibility - please use above constants with prefix
#if defined(AGL_USE_LEGACY_TYPES)
    DEFAULT     = at_DEFAULT, 
    S7_CLASSIC  = at_S7_CLASSIC,
    S7_NEXT     = at_S7_NEXT,
    SINUMERIK   = at_SINUMERIK
#endif
  };
};

struct SystemTypeState_t
{
  enum enum_t
  {
    sts_UNDEFINED = -1,
    sts_VALID,
    sts_INVALID,
    sts_S7_DATABLOCK_NOT_COMPILED,
    sts_S7_INVALID_TYPE,
    sts_S7_NOT_SUPPORTED_TYPE,

// backward compatibility - please use above constants with prefix    
#if defined(AGL_USE_LEGACY_TYPES)
    UNDEFINED                 = sts_UNDEFINED,
    VALID                     = sts_VALID,
    INVALID                   = sts_INVALID,
    S7_DATABLOCK_NOT_COMPILED = sts_S7_DATABLOCK_NOT_COMPILED,
    S7_INVALID_TYPE           = sts_S7_INVALID_TYPE,
    S7_NOT_SUPPORTED_TYPE     = sts_S7_NOT_SUPPORTED_TYPE
#endif
  };
};

struct HierarchyType_t
{
  enum enum_t
  {
    ht_UNDEFINED = -1,
    ht_ITEM = 0,
    ht_STRUCTURE,
    ht_ARRAY,

// backward compatibility - please use above constants with prefix
#if defined(AGL_USE_LEGACY_TYPES)
    UNDEFINED = ht_UNDEFINED,
    ITEM      = ht_ITEM,
    STRUCTURE = ht_STRUCTURE,
    ARRAY     = ht_ARRAY
#endif
  };
};

struct SegmentType_t
{
  enum enum_t
  {
    se_UNDEFINED = -1,
    se_ROOT = 0,
    se_FIELD,
    se_INDEX,

// backward compatibility - please use above constants with prefix
#if defined(AGL_USE_LEGACY_TYPES)
    UNDEFINED = se_UNDEFINED,
    ROOT      = se_ROOT,
    FIELD     = se_FIELD,
    INDEX     = se_INDEX
#endif
  };
};

struct PermissionType_t
{
  enum enum_t
  {
    pt_UNDEFINED = -1,
    pt_NONE = 0, // niemals zugreifbar z.B. direkt auf Struct oder Struct-Array
    pt_DISABLED = 1, // z.B. wenn DB nicht kompiliert ist, oder unsupported, invalid
    pt_READ = 2,
    pt_WRITE = 3,
    pt_READ_WRITE = 4,

// backward compatibility - please use above constants with prefix
#if defined(AGL_USE_LEGACY_TYPES)
    UNDEFINED   = pt_UNDEFINED,
    NONE        = pt_NONE,  // conflicting with #define in system include file
    DISABLED    = pt_DISABLED,
    READ        = pt_READ,  // conflicting with #define in system include file
    WRITE       = pt_WRITE, // conflicting with #define in system include file
    READ_WRITE  = pt_READ_WRITE
#endif
  };
};

struct SystemType_t
{
  enum enum_t
  {
    st_UNDEFINED                  =        -1,
    st_UNKNOWN                    =         0,
    
    //----
    //Siemens S7 300/400, 1200, 1500
    //----
    st_S7_UNKNOWN                 =      1000,
    //real types, values
    st_S7_Any                     =      1001,
    st_S7_AOM_AID                 =      1002,
    st_S7_AOM_IDENT               =      1003,
    st_S7_Array                   =      1004,
    st_S7_Block_DB                =      1005,
    st_S7_Block_FB                =      1006,
    st_S7_Block_FC                =      1007,
    st_S7_Block_OB                =      1008,
    st_S7_Block_SDB               =      1009,
    st_S7_Block_SFB               =      1010,
    st_S7_Block_SFC               =      1011,
    st_S7_Bool                    =      1012,
    st_S7_Byte                    =      1013,
    st_S7_Char                    =      1014,
    st_S7_CONN_ANY                =      1015,
    st_S7_CONN_OUC                =      1016,
    st_S7_CONN_PRG                =      1017,
    st_S7_CONN_R_ID               =      1018,
    st_S7_Counter                 =      1019,
    st_S7_CRef                    =      1020,
    st_S7_Date                    =      1021,
    st_S7_Date_And_Time           =      1022,
    st_S7_DB_ANY                  =      1023,
    st_S7_DB_DYN                  =      1024,
    st_S7_DB_WWW                  =      1025,
    st_S7_DInt                    =      1026,
    st_S7_DTL                     =      1027,
    st_S7_DWord                   =      1028,
    st_S7_ErrorStruct             =      1029,
    st_S7_EVENT_ANY               =      1030,
    st_S7_EVENT_ATT               =      1031,
    st_S7_EVENT_HWINT             =      1032,
    st_S7_EVENT_SWINT             =      1033,
    st_S7_FBTRef                  =      1034,
    st_S7_HW_ANY                  =      1035,
    st_S7_HW_DEVICE               =      1036,
    st_S7_HW_DPMASTER             =      1037,
    st_S7_HW_DPSLAVE              =      1038,
    st_S7_HW_HSC                  =      1039,
    st_S7_HW_IEPORT               =      1040,
    st_S7_HW_INTERFACE            =      1041,
    st_S7_HW_IO                   =      1042,
    st_S7_HW_IOSYSTEM             =      1043,
    st_S7_HW_MODULE               =      1044,
    st_S7_HW_NR                   =      1045,
    st_S7_HW_PTO                  =      1046,
    st_S7_HW_PWM                  =      1047,
    st_S7_HW_SUBMODULE            =      1048,
    st_S7_IEC_Counter             =      1049,
    st_S7_IEC_DCounter            =      1050,
    st_S7_IEC_LCounter            =      1051,
    st_S7_IEC_LTimer              =      1052,
    st_S7_IEC_SCounter            =      1053,
    st_S7_IEC_Timer               =      1054,
    st_S7_IEC_UCounter            =      1055,
    st_S7_IEC_UDCounter           =      1056,
    st_S7_IEC_ULCounter           =      1057,
    st_S7_IEC_USCounter           =      1058,
    st_S7_INSTANCE                =      1059,
    st_S7_Int                     =      1060,
    st_S7_LDT                     =      1061,
    st_S7_LInt                    =      1062,
    st_S7_LReal                   =      1063,
    st_S7_LTime                   =      1064,
    st_S7_LTOD                    =      1065,
    st_S7_LWord                   =      1066,
    st_S7_NRef                    =      1067,
    st_S7_OB_ANY                  =      1068,
    st_S7_OB_ATT                  =      1069,
    st_S7_OB_COMM                 =      1070,
    st_S7_OB_CYCLIC               =      1071,
    st_S7_OB_DELAY                =      1072,
    st_S7_OB_DIAG                 =      1073,
    st_S7_OB_HWINT                =      1074,
    st_S7_OB_PCYCLE               =      1075,
    st_S7_OB_STARTUP              =      1076,
    st_S7_OB_TIMEERROR            =      1077,
    st_S7_OB_TOD                  =      1078,
    st_S7_PIP                     =      1079,
    st_S7_Pointer                 =      1080,
    st_S7_PORT                    =      1081,
    st_S7_Real                    =      1082,
    st_S7_Remote                  =      1083,
    st_S7_RTM                     =      1084,
    st_S7_S5Time                  =      1085,
    st_S7_SInt                    =      1086,
    st_S7_String                  =      1087,
    st_S7_Struct                  =      1088,
    st_S7_Time                    =      1089,
    st_S7_Time_Of_Day             =      1090,
    st_S7_Timer                   =      1091,
    st_S7_UDInt                   =      1092,
    st_S7_UInt                    =      1093,
    st_S7_ULInt                   =      1094,
    st_S7_USInt                   =      1095,
    st_S7_Variant                 =      1096,
    st_S7_Void                    =      1097,
    st_S7_VRef                    =      1098,
    st_S7_WChar                   =      1099,
    st_S7_Word                    =      1100,
    st_S7_WString                 =      1101,
    //Organisation
    st_Group                      =      1102,
    st_S7_Datablock_Group         =      1103,
    st_S7_UDT_Group               =      1104,
    st_S7_Tag_Group               =      1105,
    st_S7_Datablock               =      1106,
    st_S7_PLC                     =      1107,
    st_S7_Tag_Table               =      1108,
    st_S7_UDT                     =      1109,
    st_S7_UDT_Instance            =      1110,
    //Tags
    st_S7_Input                   =      1111, // I
    st_S7_Output                  =      1112, // Q
    st_S7_Memory                  =      1113, // M,Merker aka Flag
    //Timer und Counter sind auch Types
    st_S7_FB                      =      1114,
    st_S7_FB_Instance             =      1115,
    st_S7_CONDITIONS              =      1116,
    st_S7_TADDR_Param             =      1117,
    st_S7_TCON_Param              =      1118,
    st_S7_TDiag_Status            =      1119,
    st_S7_DiagnosticDetail        =      1120,
    st_S7_AssocValue_0            =      1121,
    st_S7_VAREF                   =      1122,
    st_S7_IP_V4                   =      1123,
    st_S7_TCON_IP_v4              =      1124,
    st_S7_Event_ID                =      1125,
    st_S7_SI_classic              =      1126,
    st_S7_SI_none                 =      1127,
    st_S7_SI_Delay                =      1128,
    st_S7_SI_Cyclic               =      1129,
    st_S7_SI_HWInterrupt          =      1130,
    st_S7_SI_Submodule            =      1131,
    st_S7_SI_IORedundancyError    =      1132,
    st_S7_SI_CPURedundancyError   =      1133,
    st_S7_SI_DiagnostigInterrupt  =      1134,
    st_S7_SI_PlugPullModule       =      1135,
    st_S7_SI_AccessError          =      1136,
    st_S7_SI_StationFailure       =      1137,
    st_S7_SI_Startup              =      1138,
    st_S7_SI_ProgIOAccesError     =      1139,
    st_S7_TCON_Configured         =      1140,
    st_S7_TCON_IP_RFC             =      1141,
    st_S7_TSelector               =      1142,
    st_S7_GEOADDR                 =      1143,
    st_S7_ArrayDBHeader           =      1144,
    st_S7_DBHeader                =      1145,
    st_S7_GeneralDBHeader         =      1146,

    st_S7_SI_ProgramCycle         =      1147,
    st_S7_SI_TimeOfDay            =      1148,
    st_S7_SI_SynchCycle           =      1149,
    st_S7_SI_TimeError            =      1150,
    st_S7_SI_Servo                =      1151,
    st_S7_SI_Ipo                  =      1152,
    st_S7_IF_CONF_Header          =      1153,
    st_S7_IF_CONF_NOS             =      1154,
    st_S7_IF_CONF_APN             =      1155,
    st_S7_IF_CONF_Login           =      1156,
    st_S7_IF_CONF_TCS_Name        =      1157,
    st_S7_IF_CONF_TCS_IP_v4       =      1158,
    st_S7_IF_CONF_GPRS_Mode       =      1159,
    st_S7_IF_CONF_SMS_Provider    =      1160,
    st_S7_IF_CONF_Pin             =      1161,
    st_S7_IF_CONF_TC_Timeouts     =      1162,
    st_S7_IF_CONF_WakeupList      =      1163,
    st_S7_IF_CONF_PrefProvider    =      1164,
    st_S7_IF_CONF_DNS             =      1165,
    st_S7_IF_CONF_NTP             =      1166,
    st_S7_IF_CONF_GPRS_User       =      1167,
    st_S7_IF_CONF_GPRS_UserList   =      1168,
    st_S7_IF_CONF_TS_Name         =      1169,
    st_S7_IF_CONF_TS_IF_v4        =      1170,
    st_S7_TCON_Phone              =      1171,
    st_S7_TCON_WDC                =      1172,
    
    //---
    //Siemens Sinumerik 840D PL/SL
    //---
    st_NCK840D_UNKNOWN            =      2000,

// backward compatibility - please use above constants with prefix

#if defined(AGL_USE_LEGACY_TYPES)
    UNDEFINED                     =      st_UNDEFINED,
    UNKNOWN                       =      st_UNKNOWN,
           
    //---- 
    //Siemens S7 300/400 1200 1500      =      //Siemens S7 300/400 1200 1500
    //----
    S7_UNKNOWN                    =      st_S7_UNKNOWN,
    
    S7_Any                        =      st_S7_Any,
    S7_AOM_AID                    =      st_S7_AOM_AID,
    S7_AOM_IDENT                  =      st_S7_AOM_IDENT,
    S7_Array                      =      st_S7_Array,
    S7_Block_DB                   =      st_S7_Block_DB,
    S7_Block_FB                   =      st_S7_Block_FB,
    S7_Block_FC                   =      st_S7_Block_FC,
    S7_Block_OB                   =      st_S7_Block_OB,
    S7_Block_SDB                  =      st_S7_Block_SDB,
    S7_Block_SFB                  =      st_S7_Block_SFB,
    S7_Block_SFC                  =      st_S7_Block_SFC,
    S7_Bool                       =      st_S7_Bool,
    S7_Byte                       =      st_S7_Byte,
    S7_Char                       =      st_S7_Char,
    S7_CONN_ANY                   =      st_S7_CONN_ANY,
    S7_CONN_OUC                   =      st_S7_CONN_OUC,
    S7_CONN_PRG                   =      st_S7_CONN_PRG,
    S7_CONN_R_ID                  =      st_S7_CONN_R_ID,
    S7_Counter                    =      st_S7_Counter,
    S7_CRef                       =      st_S7_CRef,
    S7_Date                       =      st_S7_Date,
    S7_Date_And_Time              =      st_S7_Date_And_Time,
    S7_DB_ANY                     =      st_S7_DB_ANY,
    S7_DB_DYN                     =      st_S7_DB_DYN,
    S7_DB_WWW                     =      st_S7_DB_WWW,
    S7_DInt                       =      st_S7_DInt,
    S7_DTL                        =      st_S7_DTL,
    S7_DWord                      =      st_S7_DWord,
    S7_ErrorStruct                =      st_S7_ErrorStruct,
    S7_EVENT_ANY                  =      st_S7_EVENT_ANY,
    S7_EVENT_ATT                  =      st_S7_EVENT_ATT,
    S7_EVENT_HWINT                =      st_S7_EVENT_HWINT,
    S7_EVENT_SWINT                =      st_S7_EVENT_SWINT,
    S7_FBTRef                     =      st_S7_FBTRef,
    S7_HW_ANY                     =      st_S7_HW_ANY,
    S7_HW_DEVICE                  =      st_S7_HW_DEVICE,
    S7_HW_DPMASTER                =      st_S7_HW_DPMASTER,
    S7_HW_DPSLAVE                 =      st_S7_HW_DPSLAVE,
    S7_HW_HSC                     =      st_S7_HW_HSC,
    S7_HW_IEPORT                  =      st_S7_HW_IEPORT,
    S7_HW_INTERFACE               =      st_S7_HW_INTERFACE,
    S7_HW_IO                      =      st_S7_HW_IO,
    S7_HW_IOSYSTEM                =      st_S7_HW_IOSYSTEM,
    S7_HW_MODULE                  =      st_S7_HW_MODULE,
    S7_HW_NR                      =      st_S7_HW_NR,
    S7_HW_PTO                     =      st_S7_HW_PTO,
    S7_HW_PWM                     =      st_S7_HW_PWM,
    S7_HW_SUBMODULE               =      st_S7_HW_SUBMODULE,
    S7_IEC_Counter                =      st_S7_IEC_Counter,
    S7_IEC_DCounter               =      st_S7_IEC_DCounter,
    S7_IEC_LCounter               =      st_S7_IEC_LCounter,
    S7_IEC_LTimer                 =      st_S7_IEC_LTimer,
    S7_IEC_SCounter               =      st_S7_IEC_SCounter,
    S7_IEC_Timer                  =      st_S7_IEC_Timer,
    S7_IEC_UCounter               =      st_S7_IEC_UCounter,
    S7_IEC_UDCounter              =      st_S7_IEC_UDCounter,
    S7_IEC_ULCounter              =      st_S7_IEC_ULCounter,
    S7_IEC_USCounter              =      st_S7_IEC_USCounter,
    S7_INSTANCE                   =      st_S7_INSTANCE,
    S7_Int                        =      st_S7_Int,
    S7_LDT                        =      st_S7_LDT,
    S7_LInt                       =      st_S7_LInt,
    S7_LReal                      =      st_S7_LReal,
    S7_LTime                      =      st_S7_LTime,
    S7_LTOD                       =      st_S7_LTOD,
    S7_LWord                      =      st_S7_LWord,
    S7_NRef                       =      st_S7_NRef,
    S7_OB_ANY                     =      st_S7_OB_ANY,
    S7_OB_ATT                     =      st_S7_OB_ATT,
    S7_OB_COMM                    =      st_S7_OB_COMM,
    S7_OB_CYCLIC                  =      st_S7_OB_CYCLIC,
    S7_OB_DELAY                   =      st_S7_OB_DELAY,
    S7_OB_DIAG                    =      st_S7_OB_DIAG,
    S7_OB_HWINT                   =      st_S7_OB_HWINT,
    S7_OB_PCYCLE                  =      st_S7_OB_PCYCLE,
    S7_OB_STARTUP                 =      st_S7_OB_STARTUP,
    S7_OB_TIMEERROR               =      st_S7_OB_TIMEERROR,
    S7_OB_TOD                     =      st_S7_OB_TOD,
    S7_PIP                        =      st_S7_PIP,
    S7_Pointer                    =      st_S7_Pointer,
    S7_PORT                       =      st_S7_PORT,
    S7_Real                       =      st_S7_Real,
    S7_Remote                     =      st_S7_Remote,
    S7_RTM                        =      st_S7_RTM,
    S7_S5Time                     =      st_S7_S5Time,
    S7_SInt                       =      st_S7_SInt,
    S7_String                     =      st_S7_String,
    S7_Struct                     =      st_S7_Struct,
    S7_Time                       =      st_S7_Time,
    S7_Time_Of_Day                =      st_S7_Time_Of_Day,
    S7_Timer                      =      st_S7_Timer,
    S7_UDInt                      =      st_S7_UDInt,
    S7_UInt                       =      st_S7_UInt,
    S7_ULInt                      =      st_S7_ULInt,
    S7_USInt                      =      st_S7_USInt,
    S7_Variant                    =      st_S7_Variant,
    S7_Void                       =      st_S7_Void,
    S7_VRef                       =      st_S7_VRef,
    S7_WChar                      =      st_S7_WChar,
    S7_Word                       =      st_S7_Word,
    S7_WString                    =      st_S7_WString,
    //Organisation                =      //Organisation,
    Group                         =      st_Group,
    S7_Datablock_Group            =      st_S7_Datablock_Group,
    S7_UDT_Group                  =      st_S7_UDT_Group,
    S7_Tag_Group                  =      st_S7_Tag_Group,
    S7_Datablock                  =      st_S7_Datablock,
    S7_PLC                        =      st_S7_PLC,
    S7_Tag_Table                  =      st_S7_Tag_Table,
    S7_UDT                        =      st_S7_UDT,
    S7_UDT_Instance               =      st_S7_UDT_Instance,
    //Tags
    S7_Input                      =      st_S7_Input, //I
    S7_Output                     =      st_S7_Output, // Q
    S7_Memory                     =      st_S7_Memory, // Merker aka Flag
   
    S7_FB                         =      st_S7_FB,
    S7_FB_Instance                =      st_S7_FB_Instance,
    S7_CONDITIONS                 =      st_S7_CONDITIONS,
    S7_TADDR_Param                =      st_S7_TADDR_Param,
    S7_TCON_Param                 =      st_S7_TCON_Param,
    S7_TCON_Phone                 =      st_S7_TCON_Phone,
    S7_TDiag_Status               =      st_S7_TDiag_Status,
    S7_DiagnosticDetail           =      st_S7_DiagnosticDetail,
    S7_AssocValue_0               =      st_S7_AssocValue_0,
    S7_VAREF                      =      st_S7_VAREF,
    S7_IP_V4                      =      st_S7_IP_V4,                           
    S7_TCON_IP_v4                 =      st_S7_TCON_IP_v4,
    S7_Event_ID                   =      st_S7_Event_ID,
    S7_SI_classic                 =      st_S7_SI_classic,
    S7_SI_none                    =      st_S7_SI_none,
    S7_SI_ProgramCycle            =      st_S7_SI_ProgramCycle,
    S7_SI_TimeOfDay               =      st_S7_SI_TimeOfDay,
    S7_SI_Delay                   =      st_S7_SI_Delay,
    S7_SI_Cyclic                  =      st_S7_SI_Cyclic,
    S7_SI_HWInterrupt             =      st_S7_SI_HWInterrupt,
    S7_SI_Submodule               =      st_S7_SI_Submodule,
    S7_SI_SynchCycle              =      st_S7_SI_SynchCycle,
    S7_SI_IORedundancyError       =      st_S7_SI_IORedundancyError,
    S7_SI_CPURedundancyError      =      st_S7_SI_CPURedundancyError,
    S7_SI_TimeError               =      st_S7_SI_TimeError,
    S7_SI_DiagnostigInterrupt     =      st_S7_SI_DiagnostigInterrupt,
    S7_SI_PlugPullModule          =      st_S7_SI_PlugPullModule,
    S7_SI_AccessError             =      st_S7_SI_AccessError,
    S7_SI_StationFailure          =      st_S7_SI_StationFailure,
    S7_SI_Servo                   =      st_S7_SI_Servo,
    S7_SI_Ipo                     =      st_S7_SI_Ipo,
    S7_SI_Startup                 =      st_S7_SI_Startup,
    S7_SI_ProgIOAccesError        =      st_S7_SI_ProgIOAccesError,
    S7_TCON_Configured            =      st_S7_TCON_Configured,
    S7_TCON_IP_RFC                =      st_S7_TCON_IP_RFC,
    S7_TSelector                  =      st_S7_TSelector,
    S7_GEOADDR                    =      st_S7_GEOADDR,
    S7_ArrayDBHeader              =      st_S7_ArrayDBHeader,
    S7_GeneralDBHeader            =      st_S7_GeneralDBHeader,
    S7_DBHeader                   =      st_S7_DBHeader,
    S7_IF_CONF_Header             =      st_S7_IF_CONF_Header,
    S7_IF_CONF_NOS                =      st_S7_IF_CONF_NOS,
    S7_IF_CONF_APN                =      st_S7_IF_CONF_APN,
    S7_IF_CONF_Login              =      st_S7_IF_CONF_Login,
    S7_IF_CONF_TCS_Name           =      st_S7_IF_CONF_TCS_Name,
    S7_IF_CONF_TCS_IP_v4          =      st_S7_IF_CONF_TCS_IP_v4,
    S7_IF_CONF_GPRS_Mode          =      st_S7_IF_CONF_GPRS_Mode,
    S7_IF_CONF_SMS_Provider       =      st_S7_IF_CONF_SMS_Provider,
    S7_IF_CONF_Pin                =      st_S7_IF_CONF_Pin,
    S7_IF_CONF_TC_Timeouts        =      st_S7_IF_CONF_TC_Timeouts,
    S7_IF_CONF_WakeupList         =      st_S7_IF_CONF_WakeupList,
    S7_IF_CONF_PrefProvider       =      st_S7_IF_CONF_PrefProvider,
    S7_IF_CONF_DNS                =      st_S7_IF_CONF_DNS,
    S7_IF_CONF_NTP                =      st_S7_IF_CONF_NTP,
    S7_IF_CONF_GPRS_User          =      st_S7_IF_CONF_GPRS_User,
    S7_IF_CONF_GPRS_UserList      =      st_S7_IF_CONF_GPRS_UserList,
    S7_IF_CONF_TS_Name            =      st_S7_IF_CONF_TS_Name,
    S7_IF_CONF_TS_IF_v4           =      st_S7_IF_CONF_TS_IF_v4,
    S7_TCON_WDC                   =      st_S7_TCON_WDC,
                
    //--- 
    //Siemens Sinumerik 840D PL/SL
    //---
    NCK840D_UNKNOWN     =      st_NCK840D_UNKNOWN
#endif
  };
};

struct ValueType_t
{
  enum enum_t
  {
    vt_UNDEFINED = -1,

    vt_SYSTEM_SPECIFIC, // z.B. S7_DTL, S7_S5Time, mostly means Byte-Buffer, BCD-Encoded etc.

    vt_UINT8,
    vt_UINT16,
    vt_UINT32,
    vt_UINT64,

    vt_INT8,
    vt_INT16,
    vt_INT32,
    vt_INT64,

    vt_CHAR8,
    vt_CHAR16,
    vt_CHAR32,

    vt_STRING8,
    vt_STRING16,
    vt_STRING32,
        
    vt_FLOAT32,
    vt_FLOAT64,

// backward compatibility - please use above constants with prefix
#if defined(AGL_USE_LEGACY_TYPES)
    UNDEFINED       = vt_UNDEFINED,
    SYSTEM_SPECIFIC = vt_SYSTEM_SPECIFIC,

    UINT8           = vt_UINT8,
    UINT16          = vt_UINT16,
    UINT32          = vt_UINT32,
    UINT64          = vt_UINT64,
    
    INT8            = vt_INT8,
    INT16           = vt_INT16,
    INT32           = vt_INT32,
    INT64           = vt_INT64,
    
    CHAR8           = vt_CHAR8,
    CHAR16          = vt_CHAR16,
    CHAR32          = vt_CHAR32,
    
    STRING8         = vt_STRING8,
    STRING16        = vt_STRING16,
    STRING32        = vt_STRING32,
    
    FLOAT32         = vt_FLOAT32,
    FLOAT64         = vt_FLOAT64
#endif
  };
};

struct TypeState_t
{
  enum enum_t
  {
    ts_UNDEFINED = -1,
    ts_NOT_SUPPORTED,
    ts_VALID,
    ts_INVALID,

// backward compatibility - please use above constants with prefix
#if defined(AGL_USE_LEGACY_TYPES)
    UNDEFINED     = ts_UNDEFINED,
    NOT_SUPPORTED = ts_NOT_SUPPORTED,
    VALID         = ts_VALID,
    INVALID       = ts_INVALID
#endif
  };
};

struct S7PlcFamily_t
{
  enum enum_t
  {
    spf_UNDEFINED = -1,
    spf_S7_300_400,
    spf_S7_1200,
    spf_S7_1500,

// backward compatibility - please use above constant with prefix
#if defined(AGL_USE_LEGACY_TYPES)
    UNDEFINED   = spf_UNDEFINED,
    S7_300_400  = spf_S7_300_400,
    S7_1200     = spf_S7_1200,
    S7_1500     = spf_S7_1500
#endif
  };
};

// Enum fuer die Datenblocktypen
struct DatablockTypes_t
{
  enum enum_t
  {
    dt_UNDEFINED = 0,    // Unbekannter Datenblocktyp/Kein Datenblock
    dt_IDB_of_SDT = 1,   // Instanz-Datenblock eines Systemtyps
    dt_IDB_of_FB = 2,    // Instanz-Datenblock eines Funktionsbausteins
    dt_DB_of_Type = 3,   // Datenblock eines definierten Typs
    dt_Standard_DB = 4,  // Datenblock mit selbst definiertem Inhalt
    dt_Array_DB = 5,     // Array Datenblock
    dt_IDB_of_SFB = 6,   // Instanz-Datenblock eines System-Funktionsbausteins
    dt_IDB_of_FBT = 7    // Instanz-Datenblock eines Technologie-Funktionsbausteins
  };
};

/*******************************************************************************

 Strukturen für die 1200, 1500 Symbolik-Funktionen - Ende

 *******************************************************************************/

#if defined( _MSC_VER ) && _MSC_VER > 600
#pragma pack( pop )
#elif !defined( _UCC )
#pragma pack()
#endif


#endif //_AGL_SYMBOLIC_TYPES__
