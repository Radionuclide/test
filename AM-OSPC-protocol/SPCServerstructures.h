/* this ALWAYS GENERATED file contains the definitions for the interfaces */


/* File created by MIDL compiler version 5.01.0164 */
/* at Mon Jul 07 11:03:11 2003
 */
/* Compiler settings for SPCServerstructures.idl:
    Oicf (OptLev=i2), W1, Zp8, env=Win32, ms_ext, c_ext
    error checks: allocation ref bounds_check enum stub_data 
*/
//@@MIDL_FILE_HEADING(  )


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 440
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __SPCServerstructures_h__
#define __SPCServerstructures_h__

#ifdef __cplusplus
extern "C"{
#endif 

/* Forward Declarations */ 

/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

void __RPC_FAR * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void __RPC_FAR * ); 

/* interface __MIDL_itf_SPCServerstructures_0000 */
/* [local] */ 

#define	MAX_KENMERK	( 51 )


enum NOTIFY_TYPE
    {	INVALID_NOTIFY_TYPE	= 0,
	ISPCPROCESMANAGER_NOTIFY	= 1,
	ISPCKAART_NOTIFY	= 2,
	IBACKUP_NOTIFY	= 4,
	ALL_NOTIFIES	= 255
    };
struct  SPC_VERSION
    {
    DWORD major;
    DWORD compatibility;
    DWORD minor;
    };

enum SPC_USERLEVEL
    {	INVALID_USERLEVEL	= 0,
	PROCES	= 1,
	TOEPASSING	= PROCES + 1,
	OPERATOR	= TOEPASSING + 1,
	USER	= OPERATOR + 1
    };

enum SPC_OBJECT_TYPE
    {	INVALID_OBJ_TYPE	= 0,
	OBJ_PROCES	= 1,
	OBJ_TOEPASSING	= OBJ_PROCES + 1,
	OBJ_ONDERWERP	= OBJ_TOEPASSING + 1,
	OBJ_MODEL	= OBJ_ONDERWERP + 1,
	OBJ_KAART	= OBJ_MODEL + 1,
	OBJ_OVERZICHT	= OBJ_KAART + 1,
	OBJ_VERANTWOORDELIJKE	= OBJ_OVERZICHT + 1,
	OBJ_COMMUNICATIEMODULE	= OBJ_VERANTWOORDELIJKE + 1,
	OBJ_FYSISCHE_BESTEMMING	= OBJ_COMMUNICATIEMODULE + 1,
	OBJ_LOGISCHE_BESTEMMING	= OBJ_FYSISCHE_BESTEMMING + 1,
	OBJ_MODEL_CREATE	= OBJ_LOGISCHE_BESTEMMING + 1,
	OBJ_MODEL_HOOFDVAR	= OBJ_MODEL_CREATE + 1,
	OBJ_MODEL_MODELSETTINGS	= OBJ_MODEL_HOOFDVAR + 1,
	OBJ_MODEL_MODELSETTINGSEX	= OBJ_MODEL_MODELSETTINGS + 1,
	OBJ_MODEL_DIMENSION	= OBJ_MODEL_MODELSETTINGSEX + 1,
	OBJ_MODEL_EVENT	= OBJ_MODEL_DIMENSION + 1,
	OBJ_KAART_CREATE	= OBJ_MODEL_EVENT + 1,
	OBJ_KAART_EDIT	= OBJ_KAART_CREATE + 1,
	OBJ_KAART_REMOVE	= OBJ_KAART_EDIT + 1,
	OBJ_VERZAMELKAART_CREATE	= OBJ_KAART_REMOVE + 1,
	OBJ_VERZAMELKAART_EDIT	= OBJ_VERZAMELKAART_CREATE + 1,
	OBJ_VERZAMELKAART_REMOVE	= OBJ_VERZAMELKAART_EDIT + 1,
	OBJ_MODEL_CONFIG	= OBJ_VERZAMELKAART_REMOVE + 1,
	OBJ_MODEL_DATA	= OBJ_MODEL_CONFIG + 1,
	OBJ_WATCHDOG	= 255
    };
struct  SPC_VERANTWOORDELIJKE
    {
    int Id;
    BSTR naam;
    BSTR UID;
    };
struct  SPC_VERANTWOORDELIJKELIST
    {
    int size;
    /* [size_is] */ struct SPC_VERANTWOORDELIJKE __RPC_FAR *pVerantw;
    };

enum SPC_VARIABELE_TYPE
    {	INVALID_VARTYPE	= 0,
	VARIABELE	= 1,
	KENMERK	= VARIABELE + 1,
	EVENT	= KENMERK + 1
    };
struct  SPC_VARIABELE
    {
    BSTR procesNaam;
    BSTR varNaam;
    BSTR productIdentiteit;
    double tijd;
    enum SPC_VARIABELE_TYPE type;
    double waarde;
    BOOL status;
    BSTR varKenmerk;
    };
struct  SPC_VARIABELELIST
    {
    int size;
    /* [size_is] */ struct SPC_VARIABELE __RPC_FAR *pVar;
    };
struct  SPC_PROCESDATA
    {
    int IDprocesDataInfo;
    BSTR productIdentiteit;
    double tijd;
    double waarde;
    BOOL status;
    BSTR varKenmerk;
    };
struct  SPC_PROCESDATALIST
    {
    int size;
    /* [size_is] */ struct SPC_PROCESDATA __RPC_FAR *pData;
    };
struct  SPC_KENMERK
    {
    DWORD IDKenmerk;
    char szKenmerk[ 51 ];
    };
struct  SPC_KENMERKLIST
    {
    int size;
    /* [size_is] */ struct SPC_KENMERK __RPC_FAR *pKenmerk;
    };
struct  SPC_MANUELE_DATA
    {
    int IDprocesDataInfo;
    double waarde;
    BOOL status;
    BSTR varKenmerk;
    };
struct  SPC_MANUELE_DATA_RECORD
    {
    BSTR productIdentiteit;
    double tijd;
    int size;
    /* [size_is] */ struct SPC_MANUELE_DATA __RPC_FAR *pData;
    };
struct  SPC_MANUELE_DATA_RECORDLIST
    {
    int size;
    /* [size_is] */ struct SPC_MANUELE_DATA_RECORD __RPC_FAR *pRecord;
    };
struct  SPC_MANUELE_DATA_ERROR
    {
    int recordIndex;
    int IDprocesDataInfo;
    };
struct  SPC_MANUELE_DATA_ERRORLIST
    {
    int size;
    /* [size_is] */ struct SPC_MANUELE_DATA_ERROR __RPC_FAR *pErrorList;
    };
struct  SPC_MANUELE_PRECALCULATED_DATA
    {
    double waarde;
    double target;
    double bovenTolerantie;
    double onderTolerantie;
    double bovenGrens;
    double onderGrens;
    };
struct  SPC_MANUELE_PRECALCULATED_DATALIST
    {
    int size;
    /* [size_is] */ struct SPC_MANUELE_PRECALCULATED_DATA __RPC_FAR *pData;
    };
struct  SPC_PROCESINFO
    {
    int Id;
    BSTR naam;
    BSTR omschrijving;
    DWORD recordsInCircFile;
    DWORD defaultMaxAantalOnvolledigePuntgroepen;
    BSTR progID;
    };
struct  SPC_PROCESLIST
    {
    int size;
    /* [size_is] */ struct SPC_PROCESINFO __RPC_FAR *pProcesInfo;
    };
struct  SPC_COMMUNICATION_MODULE
    {
    int Id;
    BSTR naam;
    };
struct  SPC_COMMUNICATION_MODULE_LIST
    {
    int size;
    /* [size_is] */ struct SPC_COMMUNICATION_MODULE __RPC_FAR *pCommunicationModuleInfo;
    };
struct  SPC_FYSISCHE_BESTEMMING
    {
    int Id;
    int IdCommunicatiemodule;
    BSTR bestemming;
    BSTR omschrijving;
    };
struct  SPC_FYSISCHE_BESTEMMINGLIST
    {
    int size;
    /* [size_is] */ struct SPC_FYSISCHE_BESTEMMING __RPC_FAR *pFysischeBestemming;
    };
struct  SPC_LOGISCHE_BESTEMMING
    {
    int Id;
    BSTR naam;
    BSTR omschrijving;
    int size;
    /* [size_is] */ int __RPC_FAR *pIdFysischeBestemming;
    };
struct  SPC_LOGISCHE_BESTEMMINGLIST
    {
    int size;
    /* [size_is] */ struct SPC_LOGISCHE_BESTEMMING __RPC_FAR *pLogischeBestemming;
    };
struct  SPC_CHANGED_OBJECT
    {
    enum SPC_OBJECT_TYPE type;
    int objectId;
    int parentId;
    };
struct  SPC_CHANGED_OBJECT_LIST
    {
    int size;
    /* [size_is] */ struct SPC_CHANGED_OBJECT __RPC_FAR *pList;
    };
struct  SPC_PROCESDATAINFO
    {
    int Id;
    BSTR naam;
    enum SPC_VARIABELE_TYPE type;
    BSTR eenheid;
    double minimumGrens;
    double maximumGrens;
    double resolutie;
    BSTR omschrijving;
    };
struct  SPC_PROCESDATAINFOLIST
    {
    int size;
    /* [size_is] */ struct SPC_PROCESDATAINFO __RPC_FAR *pProcesDataInfo;
    };
struct  SPC_TOEPASSINGINFO
    {
    int Id;
    BSTR naam;
    BSTR omschrijving;
    };
struct  SPC_TOEPASSINGLIST
    {
    int size;
    /* [size_is] */ struct SPC_TOEPASSINGINFO __RPC_FAR *pToepInfo;
    };
struct  SPC_OVERZICHTINFO
    {
    int Id;
    BSTR naam;
    BSTR omschrijving;
    BSTR creator;
    BOOL prive;
    };
struct  SPC_OVERZICHTLIST
    {
    int size;
    /* [size_is] */ struct SPC_OVERZICHTINFO __RPC_FAR *pOverzichtInfo;
    };

enum SPC_BERICHTTYPE
    {	INVALID_BERICHTTYPE	= 0,
	BERICHT_MET_DATA	= 1,
	BERICHT_ZONDER_DATA	= BERICHT_MET_DATA + 1,
	CYCLISCH	= BERICHT_ZONDER_DATA + 1,
	MANUELE_INVOER	= CYCLISCH + 1
    };
struct  SPC_ACCESSINFO
    {
    int Id;
    BSTR naam;
    BSTR omschrijving;
    enum SPC_BERICHTTYPE berichtType;
    BSTR ODBCDataSource;
    BSTR SQLQuery;
    unsigned long refreshRate;
    int size;
    /* [size_is] */ int __RPC_FAR *pIDProcesDataInfo;
    };
struct  SPC_ACCESSINFOLIST
    {
    int size;
    /* [size_is] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo;
    };
struct  SPC_CLEANUP_PROCESDATA
    {
    int IDProces;
    double tijd;
    BSTR fileNaam;
    };
struct  SPC_ONDERWERPINFO
    {
    int Id;
    BSTR naam;
    BSTR omschrijving;
    BOOL manueleInvoer;
    BOOL testInvoerViaSubgroep;
    DWORD maxAantalOnvolledigePuntgroepen;
    BOOL tracingBijOnvolledigePuntgroep;
    };
struct  SPC_ONDERWERPLIST
    {
    int size;
    /* [size_is] */ struct SPC_ONDERWERPINFO __RPC_FAR *pOnderwerpInfo;
    };
struct  SPC_PUNTGROEP_RELATIONSHIP
    {
    BOOL checkOnProductId;
    UINT timeSpan;
    };
struct  SPC_GEBEURTENISGROEP
    {
    int size;
    /* [size_is] */ int __RPC_FAR *pIDProcesDataInfo;
    };
struct  SPC_GEBEURTENISINMODEL
    {
    int IDProcesDataInfo;
    UINT horizon;
    BOOL stabiliteit;
    BOOL proportioneel;
    BOOL actief;
    };
struct  SPC_GEBEURTENISINMODELLIST
    {
    int size;
    /* [size_is] */ struct SPC_GEBEURTENISINMODEL __RPC_FAR *pGebeurtenisInModel;
    };

enum MODELCATEGORY
    {	INVALID_MODEL	= 0,
	STUDY_MODEL	= INVALID_MODEL + 1,
	PRODUCTION_MODEL	= STUDY_MODEL + 1,
	FICTIEF_MODEL	= PRODUCTION_MODEL + 1
    };

enum MODELTYPE
    {	INVALID_TYPE	= 0,
	NORMAL_MONO	= INVALID_TYPE + 1,
	GAMMA_MONO	= NORMAL_MONO + 1,
	SIGMA	= GAMMA_MONO + 1,
	EWMA	= SIGMA + 1,
	MULTI	= EWMA + 1,
	BINOMIAL	= MULTI + 1,
	POISSON	= BINOMIAL + 1,
	SUBGROUP	= POISSON + 1,
	XMR	= SUBGROUP + 1
    };
struct  SPC_MODELINFO
    {
    int Id;
    enum MODELTYPE type;
    BSTR creator;
    BSTR omschrijving;
    BOOL isLearning;
    };
struct  SPC_BINARY_DATA
    {
    unsigned long originalSize;
    unsigned long size;
    /* [full][size_is] */ unsigned char __RPC_FAR *pBinData;
    };

enum TRANSFORMATIEFORMULE
    {	INVALID_FORMULA	= 0,
	X	= INVALID_FORMULA + 1,
	X2	= X + 1,
	LNX	= X2 + 1,
	SQRTX	= LNX + 1
    };
struct  SPC_MODELSETTINGS
    {
    double dempingsfactor;
    BOOL steekproefcorrectie;
    BOOL historyFile;
    UINT archiveringsperiode;
    UINT archiveringsBufferSize;
    };
struct  SPC_TRANSFORMATIEFORMULESETTINGS
    {
    UINT varNr;
    enum TRANSFORMATIEFORMULE transformatieformule;
    double transformatieCoefA;
    double transformatieCoefB;
    };
struct  SPC_MODELSETTINGS_EX
    {
    BOOL histogram;
    UINT aantalStaven;
    double minimumInHistogram;
    double maximumInHistogram;
    double alfa;
    int size;
    /* [size_is] */ struct SPC_TRANSFORMATIEFORMULESETTINGS __RPC_FAR *pTransformatieformule;
    };
struct  SPC_HOOFDVARIABELE
    {
    BSTR formule;
    double minimumGrens;
    double maximumGrens;
    double resolutie;
    BSTR eenheid;
    };

enum DIMENSIONTYPE
    {	INVALID_DIMENSION	= 0,
	INTERVAL_DIMENSION	= INVALID_DIMENSION + 1,
	CLASS_DIMENSION	= INTERVAL_DIMENSION + 1
    };
struct  SPC_INTERVAL_DIMENSION
    {
    BSTR naam;
    struct SPC_HOOFDVARIABELE hoofdVar;
    int size;
    /* [size_is] */ double __RPC_FAR *pSteunpunt;
    };
struct  SPC_CLASS
    {
    BSTR naam;
    int size;
    /* [size_is] */ BSTR __RPC_FAR *pAttribute;
    };
struct  SPC_CLASS_DIMENSION
    {
    BSTR naam;
    BSTR formule;
    int size;
    /* [size_is] */ struct SPC_CLASS __RPC_FAR *pClass;
    };

enum SPC_MODEL_ACCESSOR_TYPE
    {	INVALID_ACCESSOR_TYPE	= 0,
	NEQ	= INVALID_ACCESSOR_TYPE + 1,
	POP	= NEQ + 1,
	VAR	= POP + 1,
	MEAN	= VAR + 1,
	FAST_MEAN	= MEAN + 1
    };
struct  SPCXYSetup
    {
    BSTR titleHeader;
    BSTR titleFooter;
    BSTR titleX;
    BSTR titleY;
    BOOL isClass;
    UINT size;
    /* [size_is] */ BSTR __RPC_FAR *pClassNames;
    };
struct  SPCHistographSetup
    {
    BSTR titleHeader;
    BSTR titleFooter;
    BSTR kurtosis;
    BOOL kurtosisRed;
    BSTR skewness;
    BOOL skewnessRed;
    double totalcounts;
    };
struct  SPC_DIMENSION_ITEMS
    {
    BSTR dimensionName;
    int size;
    /* [size_is] */ BSTR __RPC_FAR *pDimItems;
    };
struct  SPC_MODEL_SELECTION_ITEM
    {
    enum DIMENSIONTYPE type;
    BSTR axName;
    int size;
    /* [size_is] */ BSTR __RPC_FAR *pClassNames;
    double lowerValue;
    double upperValue;
    };
struct  SPC_MODEL_SELECTION
    {
    BSTR selectionName;
    double lower_tolerance;
    double upper_tolerance;
    double target;
    int numberOfDimensions;
    /* [size_is] */ struct SPC_MODEL_SELECTION_ITEM __RPC_FAR *pItems;
    };
struct  SPC_MODEL_SELECTION_LIST
    {
    int rows;
    /* [size_is] */ struct SPC_MODEL_SELECTION __RPC_FAR *pSelections;
    };
struct  SPC_MODEL_CAPABILITY_TABLE_ITEM
    {
    BSTR selectionName;
    double target;
    double lower_pp;
    double pp;
    double upper_pp;
    double lower_ppk;
    double ppk;
    double upper_ppk;
    double lower_ppm;
    double ppm;
    double upper_ppm;
    };
struct  SPC_MODEL_CAPABILITY_TABLE
    {
    double m_last_update;
    int size;
    /* [size_is] */ struct SPC_MODEL_CAPABILITY_TABLE_ITEM __RPC_FAR *pItem;
    };

enum KAARTTYPE
    {	INVALID_KAART	= 0,
	SIMPLE	= INVALID_KAART + 1,
	X_KAART	= SIMPLE + 1,
	S_KAART	= X_KAART + 1,
	X_SUBGROUP	= S_KAART + 1,
	R_SUBGROUP	= X_SUBGROUP + 1,
	SUBGROUP_KAART	= R_SUBGROUP + 1,
	XMR_KAART	= SUBGROUP_KAART + 1,
	EWMA_KAART	= XMR_KAART + 1,
	CUSUM	= EWMA_KAART + 1,
	SUBSHEWHART_KAART	= CUSUM + 1,
	BAR_KAART	= SUBSHEWHART_KAART + 1,
	ELLIPS	= BAR_KAART + 1,
	T2_KAART	= ELLIPS + 1
    };
struct  SPC_KAARTINFO
    {
    int Id;
    BSTR naam;
    enum KAARTTYPE type;
    BSTR omschrijving;
    int idProces;
    int idToepassing;
    int idOnderwerp;
    int idModel;
    };
struct  SPC_KAARTLIST
    {
    int size;
    /* [size_is] */ struct SPC_KAARTINFO __RPC_FAR *pKaartInfo;
    };
struct  SPC_KAARTSETTINGS
    {
    BSTR yLabel;
    double yMin;
    double yMax;
    UINT maxPoints;
    BSTR formule;
    BOOL frozen;
    };

enum SPC_KAART_VALUE_TYPE
    {	INVALID_VALUE	= 0,
	NO_VALUE	= INVALID_VALUE + 1,
	VALUE_IS_CONSTANT	= NO_VALUE + 1,
	VALUE_IS_PROCESDATA	= VALUE_IS_CONSTANT + 1
    };
struct  SPC_KAARTSETTINGS_EX
    {
    UINT steekproefgrootte;
    BOOL correlatie;
    BOOL kaarthistoriek;
    UINT grootteHistoriek;
    enum SPC_KAART_VALUE_TYPE bovenRegelgrensType;
    BSTR bovenRegelgrensFormule;
    enum SPC_KAART_VALUE_TYPE onderRegelgrensType;
    BSTR onderRegelgrensFormule;
    enum SPC_KAART_VALUE_TYPE targetType;
    BSTR targetFormule;
    enum SPC_KAART_VALUE_TYPE bovengrensType;
    BSTR bovengrensFormule;
    enum SPC_KAART_VALUE_TYPE ondergrensType;
    BSTR ondergrensFormule;
    char szURLWerkvoorSchrift[ 256 ];
    };
struct  SPC_KAART_IN_OVERZICHT
    {
    int IdKaart;
    BOOL grafischeKaart;
    BSTR naam;
    int idOnderwerp;
    };
struct  SPC_KAART_IN_OVERZICHTLIST
    {
    int size;
    /* [size_is] */ struct SPC_KAART_IN_OVERZICHT __RPC_FAR *pList;
    };
struct  SPC_KAART_BINARY_DATA
    {
    BOOL compressed;
    unsigned long originalSize;
    unsigned long size;
    /* [size_is] */ unsigned char __RPC_FAR *pBinData;
    };
struct  SPC_KAART_POLLING
    {
    struct SPC_KAART_BINARY_DATA newPointsBinData;
    struct SPC_KAART_BINARY_DATA changedPointsBinData;
    BOOL ChangedSetup;
    };
struct  SPC_ALARMBESTEMMING
    {
    int Id;
    BSTR computernaam;
    int IDCommunicatiemodule;
    BSTR omschrijving;
    };
struct  SPC_ALARMBESTEMMINGLIST
    {
    int size;
    /* [size_is] */ struct SPC_ALARMBESTEMMING __RPC_FAR *pAlarmBestemming;
    };

enum SPC_ALARMTYPE
    {	INVALID_ALARMTYPE	= 0,
	BOVENALARM	= INVALID_ALARMTYPE + 1,
	ONDERALARM	= BOVENALARM + 1,
	BOVENALARM_TOLERANTIE	= ONDERALARM + 1,
	ONDERALARM_TOLERANTIE	= BOVENALARM_TOLERANTIE + 1,
	WE_LEVEL_HIGH	= ONDERALARM_TOLERANTIE + 1,
	WE_LEVEL_LOW	= WE_LEVEL_HIGH + 1,
	WE_TREND_POSITIVE	= WE_LEVEL_LOW + 1,
	WE_TREND_NEGATIVE	= WE_TREND_POSITIVE + 1
    };

enum SPC_ALARMFILTERTYPE
    {	INVALID_ALARMFILTER	= 0,
	HORIZONFILTER	= INVALID_ALARMFILTER + 1,
	PERIODEFILTER	= HORIZONFILTER + 1
    };
struct  SPC_ALARMINFO
    {
    int Id;
    enum SPC_ALARMTYPE alarmType;
    BOOL alarmIsActief;
    enum SPC_ALARMFILTERTYPE filterType;
    int aantalAlarmPunten;
    int horizon;
    int periode;
    BOOL alarmIsExtern;
    int IDAlarmBestemming;
    int procesComputerAlarmId;
    double alfa;
    int horizonExternAlarm;
    };
struct  SPC_ALARMINFOLIST
    {
    int size;
    /* [size_is] */ struct SPC_ALARMINFO __RPC_FAR *pAlarmInfo;
    };

enum SPC_ALARM_TYPE
    {	INVALID_ALARM_TYPE	= 0,
	EXTERNALARM	= 1,
	LOKAAL	= 2,
	BOVENGRENSALARM	= 4,
	ONDERGRENSALARM	= 8,
	REGELGRENSALARM	= 16,
	TOLERANTIEGRENSALARM	= 32,
	GEWISTE_PRODUCTIEOPMERKING	= 64,
	PRODUCTIEOPMERKING	= 128,
	NO_FILTER	= -1
    };
struct  SPC_ALARM
    {
    double tijd;
    int IDKaart;
    unsigned long sequence;
    double waarde;
    double grenswaarde;
    DWORD IDProductieOpmerking;
    BYTE alarmType;
    char productID[ 30 ];
    };
struct  SPC_ALARM_EX
    {
    struct SPC_ALARM alarm;
    BSTR kaartNaam;
    };
struct  SPC_ALARMLIST
    {
    int size;
    /* [size_is] */ struct SPC_ALARM_EX __RPC_FAR *pAlarm;
    };
struct  SPC_OPERATORLIST
    {
    int size;
    /* [size_is] */ int __RPC_FAR *pVerantwIDList;
    };
struct  SPC_CLIENT
    {
    BSTR userName;
    BSTR computerName;
    };
struct  SPC_CLIENTLIST
    {
    int size;
    /* [size_is] */ struct SPC_CLIENT __RPC_FAR *pClient;
    };
struct  SPC_PRODUCTIE_CODE
    {
    int codeID;
    char omschrijving[ 256 ];
    };
struct  SPC_PRODUCTIE_CODE_LIST
    {
    int size;
    /* [size_is] */ struct SPC_PRODUCTIE_CODE __RPC_FAR *pList;
    };


extern RPC_IF_HANDLE __MIDL_itf_SPCServerstructures_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_SPCServerstructures_0000_v0_0_s_ifspec;

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif
