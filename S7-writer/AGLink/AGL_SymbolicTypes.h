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
  agl_long32_t BufferLen; //Datenbufferlaenge in Bytes
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
    st_UNDEFINED = -1,
    st_UNKNOWN = 0,
    
    //----
    //Siemens S7 300/400, 1200, 1500
    //----
    st_S7_UNKNOWN = 1000,
    //real types, values
    st_S7_Any,
    st_S7_AOM_AID,
    st_S7_AOM_IDENT,
    st_S7_Array,
    st_S7_Block_DB,
    st_S7_Block_FB,
    st_S7_Block_FC,
    st_S7_Block_OB,
    st_S7_Block_SDB,
    st_S7_Block_SFB,
    st_S7_Block_SFC,
    st_S7_Bool,
    st_S7_Byte,
    st_S7_Char,
    st_S7_CONN_ANY,
    st_S7_CONN_OUC,
    st_S7_CONN_PRG,
    st_S7_CONN_R_ID,
    st_S7_Counter,
    st_S7_CRef,
    st_S7_Date,
    st_S7_Date_And_Time,
    st_S7_DB_ANY,
    st_S7_DB_DYN,
    st_S7_DB_WWW,
    st_S7_DInt,
    st_S7_DTL,
    st_S7_DWord,
    st_S7_ErrorStruct,
    st_S7_EVENT_ANY,
    st_S7_EVENT_ATT,
    st_S7_EVENT_HWINT,
    st_S7_EVENT_SWINT,
    st_S7_FBTRef,
    st_S7_HW_ANY,
    st_S7_HW_DEVICE,
    st_S7_HW_DPMASTER,
    st_S7_HW_DPSLAVE,
    st_S7_HW_HSC,
    st_S7_HW_IEPORT,
    st_S7_HW_INTERFACE,
    st_S7_HW_IO,
    st_S7_HW_IOSYSTEM,
    st_S7_HW_MODULE,
    st_S7_HW_NR,
    st_S7_HW_PTO,
    st_S7_HW_PWM,
    st_S7_HW_SUBMODULE,
    st_S7_IEC_Counter,
    st_S7_IEC_DCounter,
    st_S7_IEC_LCounter,
    st_S7_IEC_LTimer,
    st_S7_IEC_SCounter,
    st_S7_IEC_Timer,
    st_S7_IEC_UCounter,
    st_S7_IEC_UDCounter,
    st_S7_IEC_ULCounter,
    st_S7_IEC_USCounter,
    st_S7_INSTANCE,
    st_S7_Int,
    st_S7_LDT,
    st_S7_LInt,
    st_S7_LReal,
    st_S7_LTime,
    st_S7_LTOD,
    st_S7_LWord,
    st_S7_NRef,
    st_S7_OB_ANY,
    st_S7_OB_ATT,
    st_S7_OB_COMM,
    st_S7_OB_CYCLIC,
    st_S7_OB_DELAY,
    st_S7_OB_DIAG,
    st_S7_OB_HWINT,
    st_S7_OB_PCYCLE,
    st_S7_OB_STARTUP,
    st_S7_OB_TIMEERROR,
    st_S7_OB_TOD,
    st_S7_PIP,
    st_S7_Pointer,
    st_S7_PORT,
    st_S7_Real,
    st_S7_Remote,
    st_S7_RTM,
    st_S7_S5Time,
    st_S7_SInt,
    st_S7_String,
    st_S7_Struct,
    st_S7_Time,
    st_S7_Time_Of_Day,
    st_S7_Timer,
    st_S7_UDInt,
    st_S7_UInt,
    st_S7_ULInt,
    st_S7_USInt,
    st_S7_Variant,
    st_S7_Void,
    st_S7_VRef,
    st_S7_WChar,
    st_S7_Word,
    st_S7_WString,
    //Organisation
    st_Group,
    st_S7_Datablock_Group,
    st_S7_UDT_Group,
    st_S7_Tag_Group,
    st_S7_Datablock,
    st_S7_PLC,
    st_S7_Tag_Table,
    st_S7_UDT,
    st_S7_UDT_Instance,
    //Tags
    st_S7_Input, // I
    st_S7_Output, // Q
    st_S7_Memory, // M,Merker aka Flag
    //Timer und Counter sind auch Types
    st_S7_FB,
    st_S7_FB_Instance,
    
    //---
    //Siemens Sinumerik 840D PL/SL
    //---
    st_NCK840D_UNKNOWN = 2000,

// backward compatibility - please use above constants with prefix
#if defined(AGL_USE_LEGACY_TYPES)
    UNDEFINED           =      st_UNDEFINED,
    UNKNOWN             =      st_UNKNOWN,
           
    //---- 
    //Siemens S7 300/400 1200 1500      =      //Siemens S7 300/400 1200 1500
    //----
    S7_UNKNOWN          =      st_S7_UNKNOWN,
    //real types values
    S7_Any              =      st_S7_Any,
    S7_AOM_AID          =      st_S7_AOM_AID,
    S7_AOM_IDENT        =      st_S7_AOM_IDENT,
    S7_Array            =      st_S7_Array,
    S7_Block_DB         =      st_S7_Block_DB,
    S7_Block_FB         =      st_S7_Block_FB,
    S7_Block_FC         =      st_S7_Block_FC,
    S7_Block_OB         =      st_S7_Block_OB,
    S7_Block_SDB        =      st_S7_Block_SDB,
    S7_Block_SFB        =      st_S7_Block_SFB,
    S7_Block_SFC        =      st_S7_Block_SFC,
    S7_Bool             =      st_S7_Bool,
    S7_Byte             =      st_S7_Byte,
    S7_Char             =      st_S7_Char,
    S7_CONN_ANY         =      st_S7_CONN_ANY,
    S7_CONN_OUC         =      st_S7_CONN_OUC,
    S7_CONN_PRG         =      st_S7_CONN_PRG,
    S7_CONN_R_ID        =      st_S7_CONN_R_ID,
    S7_Counter          =      st_S7_Counter,
    S7_CRef             =      st_S7_CRef,
    S7_Date             =      st_S7_Date,
    S7_Date_And_Time    =      st_S7_Date_And_Time,
    S7_DB_ANY           =      st_S7_DB_ANY,
    S7_DB_DYN           =      st_S7_DB_DYN,
    S7_DB_WWW           =      st_S7_DB_WWW,
    S7_DInt             =      st_S7_DInt,
    S7_DTL              =      st_S7_DTL,
    S7_DWord            =      st_S7_DWord,
    S7_ErrorStruct      =      st_S7_ErrorStruct,
    S7_EVENT_ANY        =      st_S7_EVENT_ANY,
    S7_EVENT_ATT        =      st_S7_EVENT_ATT,
    S7_EVENT_HWINT      =      st_S7_EVENT_HWINT,
    S7_EVENT_SWINT      =      st_S7_EVENT_SWINT,
    S7_FBTRef           =      st_S7_FBTRef,
    S7_HW_ANY           =      st_S7_HW_ANY,
    S7_HW_DEVICE        =      st_S7_HW_DEVICE,
    S7_HW_DPMASTER      =      st_S7_HW_DPMASTER,
    S7_HW_DPSLAVE       =      st_S7_HW_DPSLAVE,
    S7_HW_HSC           =      st_S7_HW_HSC,
    S7_HW_IEPORT        =      st_S7_HW_IEPORT,
    S7_HW_INTERFACE     =      st_S7_HW_INTERFACE,
    S7_HW_IO            =      st_S7_HW_IO,
    S7_HW_IOSYSTEM      =      st_S7_HW_IOSYSTEM,
    S7_HW_MODULE        =      st_S7_HW_MODULE,
    S7_HW_NR            =      st_S7_HW_NR,
    S7_HW_PTO           =      st_S7_HW_PTO,
    S7_HW_PWM           =      st_S7_HW_PWM,
    S7_HW_SUBMODULE     =      st_S7_HW_SUBMODULE,
    S7_IEC_Counter      =      st_S7_IEC_Counter,
    S7_IEC_DCounter     =      st_S7_IEC_DCounter,
    S7_IEC_LCounter     =      st_S7_IEC_LCounter,
    S7_IEC_LTimer       =      st_S7_IEC_LTimer,
    S7_IEC_SCounter     =      st_S7_IEC_SCounter,
    S7_IEC_Timer        =      st_S7_IEC_Timer,
    S7_IEC_UCounter     =      st_S7_IEC_UCounter,
    S7_IEC_UDCounter    =      st_S7_IEC_UDCounter,
    S7_IEC_ULCounter    =      st_S7_IEC_ULCounter,
    S7_IEC_USCounter    =      st_S7_IEC_USCounter,
    S7_INSTANCE         =      st_S7_INSTANCE,
    S7_Int              =      st_S7_Int,
    S7_LDT              =      st_S7_LDT,
    S7_LInt             =      st_S7_LInt,
    S7_LReal            =      st_S7_LReal,
    S7_LTime            =      st_S7_LTime,
    S7_LTOD             =      st_S7_LTOD,
    S7_LWord            =      st_S7_LWord,
    S7_NRef             =      st_S7_NRef,
    S7_OB_ANY           =      st_S7_OB_ANY,
    S7_OB_ATT           =      st_S7_OB_ATT,
    S7_OB_COMM          =      st_S7_OB_COMM,
    S7_OB_CYCLIC        =      st_S7_OB_CYCLIC,
    S7_OB_DELAY         =      st_S7_OB_DELAY,
    S7_OB_DIAG          =      st_S7_OB_DIAG,
    S7_OB_HWINT         =      st_S7_OB_HWINT,
    S7_OB_PCYCLE        =      st_S7_OB_PCYCLE,
    S7_OB_STARTUP       =      st_S7_OB_STARTUP,
    S7_OB_TIMEERROR     =      st_S7_OB_TIMEERROR,
    S7_OB_TOD           =      st_S7_OB_TOD,
    S7_PIP              =      st_S7_PIP,
    S7_Pointer          =      st_S7_Pointer,
    S7_PORT             =      st_S7_PORT,
    S7_Real             =      st_S7_Real,
    S7_Remote           =      st_S7_Remote,
    S7_RTM              =      st_S7_RTM,
    S7_S5Time           =      st_S7_S5Time,
    S7_SInt             =      st_S7_SInt,
    S7_String           =      st_S7_String,
    S7_Struct           =      st_S7_Struct,
    S7_Time             =      st_S7_Time,
    S7_Time_Of_Day      =      st_S7_Time_Of_Day,
    S7_Timer            =      st_S7_Timer,
    S7_UDInt            =      st_S7_UDInt,
    S7_UInt             =      st_S7_UInt,
    S7_ULInt            =      st_S7_ULInt,
    S7_USInt            =      st_S7_USInt,
    S7_Variant          =      st_S7_Variant,
    S7_Void             =      st_S7_Void,
    S7_VRef             =      st_S7_VRef,
    S7_WChar            =      st_S7_WChar,
    S7_Word             =      st_S7_Word,
    S7_WString          =      st_S7_WString,
    //Organisation      =      //Organisation,
    Group               =      st_Group,
    S7_Datablock_Group  =      st_S7_Datablock_Group,
    S7_UDT_Group        =      st_S7_UDT_Group,
    S7_Tag_Group        =      st_S7_Tag_Group,
    S7_Datablock        =      st_S7_Datablock,
    S7_PLC              =      st_S7_PLC,
    S7_Tag_Table        =      st_S7_Tag_Table,
    S7_UDT              =      st_S7_UDT,
    S7_UDT_Instance     =      st_S7_UDT_Instance,
    //Tags
    S7_Output           =      st_S7_Output, // Q
    S7_Input            =      st_S7_Input, 
    S7_Memory           =      st_S7_Memory, // MMerker aka Flag
    //Timer und Counter sind auch Types
    S7_FB               =      st_S7_FB,
    S7_FB_Instance      =      st_S7_FB_Instance,
                
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

/*******************************************************************************

 Strukturen für die 1200, 1500 Symbolik-Funktionen - Ende

 *******************************************************************************/

#if defined( _MSC_VER ) && _MSC_VER > 600
#pragma pack( pop )
#elif !defined( _UCC )
#pragma pack()
#endif


#endif //_AGL_SYMBOLIC_TYPES__
