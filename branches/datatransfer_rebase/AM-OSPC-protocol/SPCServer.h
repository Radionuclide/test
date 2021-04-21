/* this ALWAYS GENERATED file contains the definitions for the interfaces */


/* File created by MIDL compiler version 5.01.0164 */
/* at Mon Jul 07 11:03:10 2003
 */
/* Compiler settings for SPCServer.idl:
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

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __SPCServer_h__
#define __SPCServer_h__

#ifdef __cplusplus
extern "C"{
#endif 

/* Forward Declarations */ 

#ifndef __ISPCProcesManager_FWD_DEFINED__
#define __ISPCProcesManager_FWD_DEFINED__
typedef interface ISPCProcesManager ISPCProcesManager;
#endif 	/* __ISPCProcesManager_FWD_DEFINED__ */


#ifndef __ISPCProcesManagerPolling_FWD_DEFINED__
#define __ISPCProcesManagerPolling_FWD_DEFINED__
typedef interface ISPCProcesManagerPolling ISPCProcesManagerPolling;
#endif 	/* __ISPCProcesManagerPolling_FWD_DEFINED__ */


#ifndef __IDataProcessing_FWD_DEFINED__
#define __IDataProcessing_FWD_DEFINED__
typedef interface IDataProcessing IDataProcessing;
#endif 	/* __IDataProcessing_FWD_DEFINED__ */


#ifndef __IDataProcessingEx_FWD_DEFINED__
#define __IDataProcessingEx_FWD_DEFINED__
typedef interface IDataProcessingEx IDataProcessingEx;
#endif 	/* __IDataProcessingEx_FWD_DEFINED__ */


#ifndef __IDiagnose_FWD_DEFINED__
#define __IDiagnose_FWD_DEFINED__
typedef interface IDiagnose IDiagnose;
#endif 	/* __IDiagnose_FWD_DEFINED__ */


#ifndef __ISPCVersion_FWD_DEFINED__
#define __ISPCVersion_FWD_DEFINED__
typedef interface ISPCVersion ISPCVersion;
#endif 	/* __ISPCVersion_FWD_DEFINED__ */


#ifndef __IProcManShortcut_FWD_DEFINED__
#define __IProcManShortcut_FWD_DEFINED__
typedef interface IProcManShortcut IProcManShortcut;
#endif 	/* __IProcManShortcut_FWD_DEFINED__ */


#ifndef __IBackup_FWD_DEFINED__
#define __IBackup_FWD_DEFINED__
typedef interface IBackup IBackup;
#endif 	/* __IBackup_FWD_DEFINED__ */


#ifndef __ISPCProces_FWD_DEFINED__
#define __ISPCProces_FWD_DEFINED__
typedef interface ISPCProces ISPCProces;
#endif 	/* __ISPCProces_FWD_DEFINED__ */


#ifndef __ISPCProductieOpmerking_FWD_DEFINED__
#define __ISPCProductieOpmerking_FWD_DEFINED__
typedef interface ISPCProductieOpmerking ISPCProductieOpmerking;
#endif 	/* __ISPCProductieOpmerking_FWD_DEFINED__ */


#ifndef __ISPCToepassing_FWD_DEFINED__
#define __ISPCToepassing_FWD_DEFINED__
typedef interface ISPCToepassing ISPCToepassing;
#endif 	/* __ISPCToepassing_FWD_DEFINED__ */


#ifndef __ISPCOnderwerp_FWD_DEFINED__
#define __ISPCOnderwerp_FWD_DEFINED__
typedef interface ISPCOnderwerp ISPCOnderwerp;
#endif 	/* __ISPCOnderwerp_FWD_DEFINED__ */


#ifndef __ISPCModel_FWD_DEFINED__
#define __ISPCModel_FWD_DEFINED__
typedef interface ISPCModel ISPCModel;
#endif 	/* __ISPCModel_FWD_DEFINED__ */


#ifndef __ISPCOverzicht_FWD_DEFINED__
#define __ISPCOverzicht_FWD_DEFINED__
typedef interface ISPCOverzicht ISPCOverzicht;
#endif 	/* __ISPCOverzicht_FWD_DEFINED__ */


#ifndef __ISPCKaart_FWD_DEFINED__
#define __ISPCKaart_FWD_DEFINED__
typedef interface ISPCKaart ISPCKaart;
#endif 	/* __ISPCKaart_FWD_DEFINED__ */


#ifndef __ISPCKaartPolling_FWD_DEFINED__
#define __ISPCKaartPolling_FWD_DEFINED__
typedef interface ISPCKaartPolling ISPCKaartPolling;
#endif 	/* __ISPCKaartPolling_FWD_DEFINED__ */


#ifndef __SPCProcesManager_FWD_DEFINED__
#define __SPCProcesManager_FWD_DEFINED__

#ifdef __cplusplus
typedef class SPCProcesManager SPCProcesManager;
#else
typedef struct SPCProcesManager SPCProcesManager;
#endif /* __cplusplus */

#endif 	/* __SPCProcesManager_FWD_DEFINED__ */


#ifndef __SPCProces_FWD_DEFINED__
#define __SPCProces_FWD_DEFINED__

#ifdef __cplusplus
typedef class SPCProces SPCProces;
#else
typedef struct SPCProces SPCProces;
#endif /* __cplusplus */

#endif 	/* __SPCProces_FWD_DEFINED__ */


#ifndef __SPCToepassing_FWD_DEFINED__
#define __SPCToepassing_FWD_DEFINED__

#ifdef __cplusplus
typedef class SPCToepassing SPCToepassing;
#else
typedef struct SPCToepassing SPCToepassing;
#endif /* __cplusplus */

#endif 	/* __SPCToepassing_FWD_DEFINED__ */


#ifndef __SPCOnderwerp_FWD_DEFINED__
#define __SPCOnderwerp_FWD_DEFINED__

#ifdef __cplusplus
typedef class SPCOnderwerp SPCOnderwerp;
#else
typedef struct SPCOnderwerp SPCOnderwerp;
#endif /* __cplusplus */

#endif 	/* __SPCOnderwerp_FWD_DEFINED__ */


#ifndef __SPCModel_FWD_DEFINED__
#define __SPCModel_FWD_DEFINED__

#ifdef __cplusplus
typedef class SPCModel SPCModel;
#else
typedef struct SPCModel SPCModel;
#endif /* __cplusplus */

#endif 	/* __SPCModel_FWD_DEFINED__ */


#ifndef __SPCOverzicht_FWD_DEFINED__
#define __SPCOverzicht_FWD_DEFINED__

#ifdef __cplusplus
typedef class SPCOverzicht SPCOverzicht;
#else
typedef struct SPCOverzicht SPCOverzicht;
#endif /* __cplusplus */

#endif 	/* __SPCOverzicht_FWD_DEFINED__ */


#ifndef __SPCKaart_FWD_DEFINED__
#define __SPCKaart_FWD_DEFINED__

#ifdef __cplusplus
typedef class SPCKaart SPCKaart;
#else
typedef struct SPCKaart SPCKaart;
#endif /* __cplusplus */

#endif 	/* __SPCKaart_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"
#include "spcServerStructures.h"

void __RPC_FAR * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void __RPC_FAR * ); 

/* interface __MIDL_itf_SPCServer_0000 */
/* [local] */ 







#define	SPC_MAJOR	( 3 )

#define	SPC_COMPATIBILITY	( 3 )



extern RPC_IF_HANDLE __MIDL_itf_SPCServer_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_SPCServer_0000_v0_0_s_ifspec;

#ifndef __ISPCProcesManager_INTERFACE_DEFINED__
#define __ISPCProcesManager_INTERFACE_DEFINED__

/* interface ISPCProcesManager */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_ISPCProcesManager;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("E9D7FCC0-D663-11D0-90FE-AA0004007F05")
    ISPCProcesManager : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetProcesList( 
            /* [out] */ struct SPC_PROCESLIST __RPC_FAR *pProcesList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetPrivilege( 
            /* [in] */ BSTR strUserName,
            /* [in] */ BSTR workstation,
            /* [in] */ struct SPC_PROCESINFO procesInfo,
            /* [out] */ enum SPC_USERLEVEL __RPC_FAR *pUserLevel,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetProces( 
            /* [in] */ struct SPC_PROCESINFO procesInfo,
            /* [out] */ ISPCProces __RPC_FAR *__RPC_FAR *pProces) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetProcesInfo( 
            /* [in] */ int procesId,
            /* [out] */ struct SPC_PROCESINFO __RPC_FAR *pProcesInfo) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditProces( 
            /* [in] */ struct SPC_PROCESINFO procesInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CreateProces( 
            /* [in] */ struct SPC_PROCESINFO procesInfo,
            /* [in] */ struct SPC_VERANTWOORDELIJKE initVerantw,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveProces( 
            /* [in] */ struct SPC_PROCESINFO procesInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetVerantwList( 
            /* [out] */ struct SPC_VERANTWOORDELIJKELIST __RPC_FAR *pVerantwList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddVerantw( 
            /* [out][in] */ struct SPC_VERANTWOORDELIJKE __RPC_FAR *pVerantw,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditVerantw( 
            /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveVerantw( 
            /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCommunicationModuleList( 
            /* [out] */ struct SPC_COMMUNICATION_MODULE_LIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetPhysicalDestinationList( 
            /* [out] */ struct SPC_FYSISCHE_BESTEMMINGLIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddPhysicalDestination( 
            /* [out][in] */ struct SPC_FYSISCHE_BESTEMMING __RPC_FAR *pBestemming,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditPhysicalDestination( 
            /* [in] */ struct SPC_FYSISCHE_BESTEMMING bestemming,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemovePhysicalDestination( 
            /* [in] */ int idPhysicalDestination,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddLogicalDestination( 
            /* [out][in] */ struct SPC_LOGISCHE_BESTEMMING __RPC_FAR *pBestemming,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditLogicalDestination( 
            /* [in] */ struct SPC_LOGISCHE_BESTEMMING bestemming,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveLogicalDestination( 
            /* [in] */ int idLogicalDestination,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetLogicalDestinationList( 
            /* [out] */ struct SPC_LOGISCHE_BESTEMMINGLIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE IAm( 
            /* [in] */ BSTR computername,
            /* [in] */ BSTR username) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetServerTime( 
            /* [out] */ double __RPC_FAR *pServerTimeStamp) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISPCProcesManagerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISPCProcesManager __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISPCProcesManager __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetProcesList )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [out] */ struct SPC_PROCESLIST __RPC_FAR *pProcesList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetPrivilege )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ BSTR strUserName,
            /* [in] */ BSTR workstation,
            /* [in] */ struct SPC_PROCESINFO procesInfo,
            /* [out] */ enum SPC_USERLEVEL __RPC_FAR *pUserLevel,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetProces )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ struct SPC_PROCESINFO procesInfo,
            /* [out] */ ISPCProces __RPC_FAR *__RPC_FAR *pProces);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetProcesInfo )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ int procesId,
            /* [out] */ struct SPC_PROCESINFO __RPC_FAR *pProcesInfo);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditProces )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ struct SPC_PROCESINFO procesInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *CreateProces )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ struct SPC_PROCESINFO procesInfo,
            /* [in] */ struct SPC_VERANTWOORDELIJKE initVerantw,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveProces )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ struct SPC_PROCESINFO procesInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetVerantwList )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [out] */ struct SPC_VERANTWOORDELIJKELIST __RPC_FAR *pVerantwList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddVerantw )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [out][in] */ struct SPC_VERANTWOORDELIJKE __RPC_FAR *pVerantw,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditVerantw )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveVerantw )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetCommunicationModuleList )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [out] */ struct SPC_COMMUNICATION_MODULE_LIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetPhysicalDestinationList )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [out] */ struct SPC_FYSISCHE_BESTEMMINGLIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddPhysicalDestination )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [out][in] */ struct SPC_FYSISCHE_BESTEMMING __RPC_FAR *pBestemming,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditPhysicalDestination )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ struct SPC_FYSISCHE_BESTEMMING bestemming,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemovePhysicalDestination )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ int idPhysicalDestination,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddLogicalDestination )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [out][in] */ struct SPC_LOGISCHE_BESTEMMING __RPC_FAR *pBestemming,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditLogicalDestination )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ struct SPC_LOGISCHE_BESTEMMING bestemming,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveLogicalDestination )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ int idLogicalDestination,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetLogicalDestinationList )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [out] */ struct SPC_LOGISCHE_BESTEMMINGLIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *IAm )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [in] */ BSTR computername,
            /* [in] */ BSTR username);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetServerTime )( 
            ISPCProcesManager __RPC_FAR * This,
            /* [out] */ double __RPC_FAR *pServerTimeStamp);
        
        END_INTERFACE
    } ISPCProcesManagerVtbl;

    interface ISPCProcesManager
    {
        CONST_VTBL struct ISPCProcesManagerVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISPCProcesManager_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISPCProcesManager_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISPCProcesManager_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISPCProcesManager_GetProcesList(This,pProcesList)	\
    (This)->lpVtbl -> GetProcesList(This,pProcesList)

#define ISPCProcesManager_GetPrivilege(This,strUserName,workstation,procesInfo,pUserLevel,pError)	\
    (This)->lpVtbl -> GetPrivilege(This,strUserName,workstation,procesInfo,pUserLevel,pError)

#define ISPCProcesManager_GetProces(This,procesInfo,pProces)	\
    (This)->lpVtbl -> GetProces(This,procesInfo,pProces)

#define ISPCProcesManager_GetProcesInfo(This,procesId,pProcesInfo)	\
    (This)->lpVtbl -> GetProcesInfo(This,procesId,pProcesInfo)

#define ISPCProcesManager_EditProces(This,procesInfo,pError)	\
    (This)->lpVtbl -> EditProces(This,procesInfo,pError)

#define ISPCProcesManager_CreateProces(This,procesInfo,initVerantw,pError)	\
    (This)->lpVtbl -> CreateProces(This,procesInfo,initVerantw,pError)

#define ISPCProcesManager_RemoveProces(This,procesInfo,pError)	\
    (This)->lpVtbl -> RemoveProces(This,procesInfo,pError)

#define ISPCProcesManager_GetVerantwList(This,pVerantwList)	\
    (This)->lpVtbl -> GetVerantwList(This,pVerantwList)

#define ISPCProcesManager_AddVerantw(This,pVerantw,pError)	\
    (This)->lpVtbl -> AddVerantw(This,pVerantw,pError)

#define ISPCProcesManager_EditVerantw(This,verantw,pError)	\
    (This)->lpVtbl -> EditVerantw(This,verantw,pError)

#define ISPCProcesManager_RemoveVerantw(This,verantw,pError)	\
    (This)->lpVtbl -> RemoveVerantw(This,verantw,pError)

#define ISPCProcesManager_GetCommunicationModuleList(This,pList)	\
    (This)->lpVtbl -> GetCommunicationModuleList(This,pList)

#define ISPCProcesManager_GetPhysicalDestinationList(This,pList)	\
    (This)->lpVtbl -> GetPhysicalDestinationList(This,pList)

#define ISPCProcesManager_AddPhysicalDestination(This,pBestemming,pError)	\
    (This)->lpVtbl -> AddPhysicalDestination(This,pBestemming,pError)

#define ISPCProcesManager_EditPhysicalDestination(This,bestemming,pError)	\
    (This)->lpVtbl -> EditPhysicalDestination(This,bestemming,pError)

#define ISPCProcesManager_RemovePhysicalDestination(This,idPhysicalDestination,pError)	\
    (This)->lpVtbl -> RemovePhysicalDestination(This,idPhysicalDestination,pError)

#define ISPCProcesManager_AddLogicalDestination(This,pBestemming,pError)	\
    (This)->lpVtbl -> AddLogicalDestination(This,pBestemming,pError)

#define ISPCProcesManager_EditLogicalDestination(This,bestemming,pError)	\
    (This)->lpVtbl -> EditLogicalDestination(This,bestemming,pError)

#define ISPCProcesManager_RemoveLogicalDestination(This,idLogicalDestination,pError)	\
    (This)->lpVtbl -> RemoveLogicalDestination(This,idLogicalDestination,pError)

#define ISPCProcesManager_GetLogicalDestinationList(This,pList)	\
    (This)->lpVtbl -> GetLogicalDestinationList(This,pList)

#define ISPCProcesManager_IAm(This,computername,username)	\
    (This)->lpVtbl -> IAm(This,computername,username)

#define ISPCProcesManager_GetServerTime(This,pServerTimeStamp)	\
    (This)->lpVtbl -> GetServerTime(This,pServerTimeStamp)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE ISPCProcesManager_GetProcesList_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [out] */ struct SPC_PROCESLIST __RPC_FAR *pProcesList);


void __RPC_STUB ISPCProcesManager_GetProcesList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_GetPrivilege_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ BSTR strUserName,
    /* [in] */ BSTR workstation,
    /* [in] */ struct SPC_PROCESINFO procesInfo,
    /* [out] */ enum SPC_USERLEVEL __RPC_FAR *pUserLevel,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_GetPrivilege_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_GetProces_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ struct SPC_PROCESINFO procesInfo,
    /* [out] */ ISPCProces __RPC_FAR *__RPC_FAR *pProces);


void __RPC_STUB ISPCProcesManager_GetProces_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_GetProcesInfo_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ int procesId,
    /* [out] */ struct SPC_PROCESINFO __RPC_FAR *pProcesInfo);


void __RPC_STUB ISPCProcesManager_GetProcesInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_EditProces_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ struct SPC_PROCESINFO procesInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_EditProces_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_CreateProces_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ struct SPC_PROCESINFO procesInfo,
    /* [in] */ struct SPC_VERANTWOORDELIJKE initVerantw,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_CreateProces_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_RemoveProces_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ struct SPC_PROCESINFO procesInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_RemoveProces_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_GetVerantwList_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [out] */ struct SPC_VERANTWOORDELIJKELIST __RPC_FAR *pVerantwList);


void __RPC_STUB ISPCProcesManager_GetVerantwList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_AddVerantw_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [out][in] */ struct SPC_VERANTWOORDELIJKE __RPC_FAR *pVerantw,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_AddVerantw_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_EditVerantw_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_EditVerantw_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_RemoveVerantw_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_RemoveVerantw_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_GetCommunicationModuleList_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [out] */ struct SPC_COMMUNICATION_MODULE_LIST __RPC_FAR *pList);


void __RPC_STUB ISPCProcesManager_GetCommunicationModuleList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_GetPhysicalDestinationList_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [out] */ struct SPC_FYSISCHE_BESTEMMINGLIST __RPC_FAR *pList);


void __RPC_STUB ISPCProcesManager_GetPhysicalDestinationList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_AddPhysicalDestination_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [out][in] */ struct SPC_FYSISCHE_BESTEMMING __RPC_FAR *pBestemming,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_AddPhysicalDestination_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_EditPhysicalDestination_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ struct SPC_FYSISCHE_BESTEMMING bestemming,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_EditPhysicalDestination_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_RemovePhysicalDestination_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ int idPhysicalDestination,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_RemovePhysicalDestination_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_AddLogicalDestination_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [out][in] */ struct SPC_LOGISCHE_BESTEMMING __RPC_FAR *pBestemming,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_AddLogicalDestination_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_EditLogicalDestination_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ struct SPC_LOGISCHE_BESTEMMING bestemming,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_EditLogicalDestination_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_RemoveLogicalDestination_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ int idLogicalDestination,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProcesManager_RemoveLogicalDestination_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_GetLogicalDestinationList_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [out] */ struct SPC_LOGISCHE_BESTEMMINGLIST __RPC_FAR *pList);


void __RPC_STUB ISPCProcesManager_GetLogicalDestinationList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_IAm_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [in] */ BSTR computername,
    /* [in] */ BSTR username);


void __RPC_STUB ISPCProcesManager_IAm_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProcesManager_GetServerTime_Proxy( 
    ISPCProcesManager __RPC_FAR * This,
    /* [out] */ double __RPC_FAR *pServerTimeStamp);


void __RPC_STUB ISPCProcesManager_GetServerTime_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISPCProcesManager_INTERFACE_DEFINED__ */


#ifndef __ISPCProcesManagerPolling_INTERFACE_DEFINED__
#define __ISPCProcesManagerPolling_INTERFACE_DEFINED__

/* interface ISPCProcesManagerPolling */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_ISPCProcesManagerPolling;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("B6141560-7FD1-11d4-9193-0050DA5E4B9B")
    ISPCProcesManagerPolling : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetChangedObjects( 
            /* [out][in] */ double __RPC_FAR *pServerTimeStamp,
            /* [out] */ struct SPC_CHANGED_OBJECT_LIST __RPC_FAR *pList,
            /* [out] */ int __RPC_FAR *pShutdownDelayInMinutes,
            /* [string][out] */ char __RPC_FAR *__RPC_FAR *pShutdownReason) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISPCProcesManagerPollingVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISPCProcesManagerPolling __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISPCProcesManagerPolling __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISPCProcesManagerPolling __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetChangedObjects )( 
            ISPCProcesManagerPolling __RPC_FAR * This,
            /* [out][in] */ double __RPC_FAR *pServerTimeStamp,
            /* [out] */ struct SPC_CHANGED_OBJECT_LIST __RPC_FAR *pList,
            /* [out] */ int __RPC_FAR *pShutdownDelayInMinutes,
            /* [string][out] */ char __RPC_FAR *__RPC_FAR *pShutdownReason);
        
        END_INTERFACE
    } ISPCProcesManagerPollingVtbl;

    interface ISPCProcesManagerPolling
    {
        CONST_VTBL struct ISPCProcesManagerPollingVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISPCProcesManagerPolling_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISPCProcesManagerPolling_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISPCProcesManagerPolling_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISPCProcesManagerPolling_GetChangedObjects(This,pServerTimeStamp,pList,pShutdownDelayInMinutes,pShutdownReason)	\
    (This)->lpVtbl -> GetChangedObjects(This,pServerTimeStamp,pList,pShutdownDelayInMinutes,pShutdownReason)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE ISPCProcesManagerPolling_GetChangedObjects_Proxy( 
    ISPCProcesManagerPolling __RPC_FAR * This,
    /* [out][in] */ double __RPC_FAR *pServerTimeStamp,
    /* [out] */ struct SPC_CHANGED_OBJECT_LIST __RPC_FAR *pList,
    /* [out] */ int __RPC_FAR *pShutdownDelayInMinutes,
    /* [string][out] */ char __RPC_FAR *__RPC_FAR *pShutdownReason);


void __RPC_STUB ISPCProcesManagerPolling_GetChangedObjects_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISPCProcesManagerPolling_INTERFACE_DEFINED__ */


#ifndef __IDataProcessing_INTERFACE_DEFINED__
#define __IDataProcessing_INTERFACE_DEFINED__

/* interface IDataProcessing */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_IDataProcessing;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("2B1DD4F4-0368-11d1-98EA-00C04FB9742F")
    IDataProcessing : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE AddData( 
            /* [in] */ struct SPC_VARIABELELIST dataList) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDataProcessingVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            IDataProcessing __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            IDataProcessing __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            IDataProcessing __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddData )( 
            IDataProcessing __RPC_FAR * This,
            /* [in] */ struct SPC_VARIABELELIST dataList);
        
        END_INTERFACE
    } IDataProcessingVtbl;

    interface IDataProcessing
    {
        CONST_VTBL struct IDataProcessingVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDataProcessing_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDataProcessing_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDataProcessing_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDataProcessing_AddData(This,dataList)	\
    (This)->lpVtbl -> AddData(This,dataList)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDataProcessing_AddData_Proxy( 
    IDataProcessing __RPC_FAR * This,
    /* [in] */ struct SPC_VARIABELELIST dataList);


void __RPC_STUB IDataProcessing_AddData_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDataProcessing_INTERFACE_DEFINED__ */


#ifndef __IDataProcessingEx_INTERFACE_DEFINED__
#define __IDataProcessingEx_INTERFACE_DEFINED__

/* interface IDataProcessingEx */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_IDataProcessingEx;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("3DAAD340-9AFC-11d5-92D9-0050DA5E4B9B")
    IDataProcessingEx : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE ChangedData( 
            /* [in] */ struct SPC_VARIABELELIST dataList) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDataProcessingExVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            IDataProcessingEx __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            IDataProcessingEx __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            IDataProcessingEx __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *ChangedData )( 
            IDataProcessingEx __RPC_FAR * This,
            /* [in] */ struct SPC_VARIABELELIST dataList);
        
        END_INTERFACE
    } IDataProcessingExVtbl;

    interface IDataProcessingEx
    {
        CONST_VTBL struct IDataProcessingExVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDataProcessingEx_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDataProcessingEx_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDataProcessingEx_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDataProcessingEx_ChangedData(This,dataList)	\
    (This)->lpVtbl -> ChangedData(This,dataList)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDataProcessingEx_ChangedData_Proxy( 
    IDataProcessingEx __RPC_FAR * This,
    /* [in] */ struct SPC_VARIABELELIST dataList);


void __RPC_STUB IDataProcessingEx_ChangedData_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDataProcessingEx_INTERFACE_DEFINED__ */


#ifndef __IDiagnose_INTERFACE_DEFINED__
#define __IDiagnose_INTERFACE_DEFINED__

/* interface IDiagnose */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_IDiagnose;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("BE30D174-4633-11d2-9A28-00C04FB9742F")
    IDiagnose : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetClientCount( 
            /* [out] */ unsigned int __RPC_FAR *pCount) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ForcedShutdown( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetConnectedClients( 
            /* [out] */ struct SPC_CLIENTLIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Shutdown( 
            /* [in] */ DWORD shutdownDelayInMinutes,
            /* [string][in] */ char __RPC_FAR *pShutdownReason) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDiagnoseVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            IDiagnose __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            IDiagnose __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            IDiagnose __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetClientCount )( 
            IDiagnose __RPC_FAR * This,
            /* [out] */ unsigned int __RPC_FAR *pCount);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *ForcedShutdown )( 
            IDiagnose __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetConnectedClients )( 
            IDiagnose __RPC_FAR * This,
            /* [out] */ struct SPC_CLIENTLIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *Shutdown )( 
            IDiagnose __RPC_FAR * This,
            /* [in] */ DWORD shutdownDelayInMinutes,
            /* [string][in] */ char __RPC_FAR *pShutdownReason);
        
        END_INTERFACE
    } IDiagnoseVtbl;

    interface IDiagnose
    {
        CONST_VTBL struct IDiagnoseVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDiagnose_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDiagnose_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDiagnose_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDiagnose_GetClientCount(This,pCount)	\
    (This)->lpVtbl -> GetClientCount(This,pCount)

#define IDiagnose_ForcedShutdown(This)	\
    (This)->lpVtbl -> ForcedShutdown(This)

#define IDiagnose_GetConnectedClients(This,pList)	\
    (This)->lpVtbl -> GetConnectedClients(This,pList)

#define IDiagnose_Shutdown(This,shutdownDelayInMinutes,pShutdownReason)	\
    (This)->lpVtbl -> Shutdown(This,shutdownDelayInMinutes,pShutdownReason)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDiagnose_GetClientCount_Proxy( 
    IDiagnose __RPC_FAR * This,
    /* [out] */ unsigned int __RPC_FAR *pCount);


void __RPC_STUB IDiagnose_GetClientCount_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDiagnose_ForcedShutdown_Proxy( 
    IDiagnose __RPC_FAR * This);


void __RPC_STUB IDiagnose_ForcedShutdown_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDiagnose_GetConnectedClients_Proxy( 
    IDiagnose __RPC_FAR * This,
    /* [out] */ struct SPC_CLIENTLIST __RPC_FAR *pList);


void __RPC_STUB IDiagnose_GetConnectedClients_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDiagnose_Shutdown_Proxy( 
    IDiagnose __RPC_FAR * This,
    /* [in] */ DWORD shutdownDelayInMinutes,
    /* [string][in] */ char __RPC_FAR *pShutdownReason);


void __RPC_STUB IDiagnose_Shutdown_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDiagnose_INTERFACE_DEFINED__ */


#ifndef __ISPCVersion_INTERFACE_DEFINED__
#define __ISPCVersion_INTERFACE_DEFINED__

/* interface ISPCVersion */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_ISPCVersion;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("240C8B00-A806-11d3-8279-0050DA5E4B9B")
    ISPCVersion : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetVersion( 
            /* [out] */ struct SPC_VERSION __RPC_FAR *pServerVersion) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISPCVersionVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISPCVersion __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISPCVersion __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISPCVersion __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetVersion )( 
            ISPCVersion __RPC_FAR * This,
            /* [out] */ struct SPC_VERSION __RPC_FAR *pServerVersion);
        
        END_INTERFACE
    } ISPCVersionVtbl;

    interface ISPCVersion
    {
        CONST_VTBL struct ISPCVersionVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISPCVersion_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISPCVersion_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISPCVersion_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISPCVersion_GetVersion(This,pServerVersion)	\
    (This)->lpVtbl -> GetVersion(This,pServerVersion)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE ISPCVersion_GetVersion_Proxy( 
    ISPCVersion __RPC_FAR * This,
    /* [out] */ struct SPC_VERSION __RPC_FAR *pServerVersion);


void __RPC_STUB ISPCVersion_GetVersion_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISPCVersion_INTERFACE_DEFINED__ */


#ifndef __IProcManShortcut_INTERFACE_DEFINED__
#define __IProcManShortcut_INTERFACE_DEFINED__

/* interface IProcManShortcut */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_IProcManShortcut;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("7EA4B040-6810-11d2-AD32-0060086CE9B1")
    IProcManShortcut : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetOnderwerp( 
            /* [in] */ BSTR procesNaam,
            /* [in] */ BSTR toepNaam,
            /* [in] */ BSTR onderwerpNaam,
            /* [out] */ ISPCOnderwerp __RPC_FAR *__RPC_FAR *pIOnderwerp) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IProcManShortcutVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            IProcManShortcut __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            IProcManShortcut __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            IProcManShortcut __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetOnderwerp )( 
            IProcManShortcut __RPC_FAR * This,
            /* [in] */ BSTR procesNaam,
            /* [in] */ BSTR toepNaam,
            /* [in] */ BSTR onderwerpNaam,
            /* [out] */ ISPCOnderwerp __RPC_FAR *__RPC_FAR *pIOnderwerp);
        
        END_INTERFACE
    } IProcManShortcutVtbl;

    interface IProcManShortcut
    {
        CONST_VTBL struct IProcManShortcutVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IProcManShortcut_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IProcManShortcut_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IProcManShortcut_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IProcManShortcut_GetOnderwerp(This,procesNaam,toepNaam,onderwerpNaam,pIOnderwerp)	\
    (This)->lpVtbl -> GetOnderwerp(This,procesNaam,toepNaam,onderwerpNaam,pIOnderwerp)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IProcManShortcut_GetOnderwerp_Proxy( 
    IProcManShortcut __RPC_FAR * This,
    /* [in] */ BSTR procesNaam,
    /* [in] */ BSTR toepNaam,
    /* [in] */ BSTR onderwerpNaam,
    /* [out] */ ISPCOnderwerp __RPC_FAR *__RPC_FAR *pIOnderwerp);


void __RPC_STUB IProcManShortcut_GetOnderwerp_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IProcManShortcut_INTERFACE_DEFINED__ */


#ifndef __IBackup_INTERFACE_DEFINED__
#define __IBackup_INTERFACE_DEFINED__

/* interface IBackup */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_IBackup;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("920520C0-F338-11d3-910C-0050DA5E4B9B")
    IBackup : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE StartBackup( void) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IBackupVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            IBackup __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            IBackup __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            IBackup __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *StartBackup )( 
            IBackup __RPC_FAR * This);
        
        END_INTERFACE
    } IBackupVtbl;

    interface IBackup
    {
        CONST_VTBL struct IBackupVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IBackup_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IBackup_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IBackup_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IBackup_StartBackup(This)	\
    (This)->lpVtbl -> StartBackup(This)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IBackup_StartBackup_Proxy( 
    IBackup __RPC_FAR * This);


void __RPC_STUB IBackup_StartBackup_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IBackup_INTERFACE_DEFINED__ */


#ifndef __ISPCProces_INTERFACE_DEFINED__
#define __ISPCProces_INTERFACE_DEFINED__

/* interface ISPCProces */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_ISPCProces;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("B3233135-D72B-11D0-9103-AA0004007F05")
    ISPCProces : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetVerantwList( 
            /* [out] */ struct SPC_VERANTWOORDELIJKELIST __RPC_FAR *pVerantwList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddVerantw( 
            /* [out][in] */ struct SPC_VERANTWOORDELIJKE __RPC_FAR *pVerantw,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveVerantw( 
            /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetToepassingList( 
            /* [out] */ struct SPC_TOEPASSINGLIST __RPC_FAR *pToepList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetToepassing( 
            /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
            /* [out] */ ISPCToepassing __RPC_FAR *__RPC_FAR *pToepassing) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetToepassingInfo( 
            /* [in] */ int toepId,
            /* [out] */ struct SPC_TOEPASSINGINFO __RPC_FAR *pToepInfo) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CreateToepassing( 
            /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
            /* [in] */ struct SPC_VERANTWOORDELIJKE initVerantw,
            /* [out] */ ISPCToepassing __RPC_FAR *__RPC_FAR *pToepassing,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditToepassing( 
            /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveToepassing( 
            /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetPrivilege( 
            /* [in] */ BSTR strUserName,
            /* [in] */ BSTR strWorkstation,
            /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
            /* [out] */ enum SPC_USERLEVEL __RPC_FAR *pUserLevel,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetOverzichtList( 
            /* [out] */ struct SPC_OVERZICHTLIST __RPC_FAR *pList,
            /* [in] */ BSTR UID) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCompleteOverzichtList( 
            /* [out] */ struct SPC_OVERZICHTLIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetOverzicht( 
            /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
            /* [out] */ ISPCOverzicht __RPC_FAR *__RPC_FAR *pOverzicht) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetOverzichtInfo( 
            /* [in] */ int overzichtId,
            /* [out] */ struct SPC_OVERZICHTINFO __RPC_FAR *pOverzichtInfo) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CreateOverzicht( 
            /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
            /* [out] */ ISPCOverzicht __RPC_FAR *__RPC_FAR *pOverzicht,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditOverzicht( 
            /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveOverzicht( 
            /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetProcesDataInfoList( 
            /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddProcesDataInfo( 
            /* [out][in] */ struct SPC_PROCESDATAINFO __RPC_FAR *pProcesData,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddProcesDataInfoList( 
            /* [out][in] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pProcesDataList,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditProcesDataInfo( 
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveProcesDataInfo( 
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetAccessInfoList( 
            /* [out] */ struct SPC_ACCESSINFOLIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddAccessInfo( 
            /* [out][in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditAccessInfo( 
            /* [in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveAccessInfo( 
            /* [in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetProcesDataInfoListEx( 
            /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pProcesdataInfoList,
            /* [out] */ struct SPC_ACCESSINFOLIST __RPC_FAR *pAccessInfoList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddProcesDataInfoEx( 
            /* [out][in] */ struct SPC_PROCESDATAINFO __RPC_FAR *pProcesData,
            /* [out][in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditProcesDataInfoEx( 
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [in] */ struct SPC_ACCESSINFO accessInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveProcesDataInfoEx( 
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [in] */ struct SPC_ACCESSINFO accessInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetAlarmLoggingList( 
            /* [in] */ double van,
            /* [in] */ double tot,
            /* [in] */ int maxAantal,
            /* [in] */ DWORD alarmFilter,
            /* [in] */ enum SPC_OBJECT_TYPE objectType,
            /* [in] */ int objectId,
            /* [out] */ struct SPC_ALARMLIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddManualData( 
            /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ChangedManualData( 
            /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetTimeOldestDataRecord( 
            /* [out] */ double __RPC_FAR *pOldestTime) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE NewRemark( 
            /* [in] */ int opmerkingID,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartData,
            /* [string][in] */ char __RPC_FAR *szIngegevenDoor,
            /* [in] */ BOOL internePO) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditRemark( 
            /* [in] */ int opmerkingID,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartData,
            /* [string][in] */ char __RPC_FAR *szLaatstGewijzigdDoor) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveRemark( 
            /* [in] */ int opmerkingID,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartData,
            /* [string][in] */ char __RPC_FAR *szGewistDoor) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetRemark( 
            /* [in] */ int opmerkingSequenceNr,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartData,
            /* [string][out] */ char __RPC_FAR *__RPC_FAR *pUserName,
            /* [out] */ double __RPC_FAR *pTijdVanWijziging) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetVerzamelkaartList( 
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pKaartConfigAsRWOrdered) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CreateVerzamelkaart( 
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartCnfgBaseIn,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartCnfgBaseOut) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditVerzamelkaart( 
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartConfiguratieBase) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveVerzamelkaart( 
            /* [in] */ int idKaart) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISPCProcesVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISPCProces __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISPCProces __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetVerantwList )( 
            ISPCProces __RPC_FAR * This,
            /* [out] */ struct SPC_VERANTWOORDELIJKELIST __RPC_FAR *pVerantwList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddVerantw )( 
            ISPCProces __RPC_FAR * This,
            /* [out][in] */ struct SPC_VERANTWOORDELIJKE __RPC_FAR *pVerantw,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveVerantw )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetToepassingList )( 
            ISPCProces __RPC_FAR * This,
            /* [out] */ struct SPC_TOEPASSINGLIST __RPC_FAR *pToepList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetToepassing )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
            /* [out] */ ISPCToepassing __RPC_FAR *__RPC_FAR *pToepassing);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetToepassingInfo )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ int toepId,
            /* [out] */ struct SPC_TOEPASSINGINFO __RPC_FAR *pToepInfo);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *CreateToepassing )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
            /* [in] */ struct SPC_VERANTWOORDELIJKE initVerantw,
            /* [out] */ ISPCToepassing __RPC_FAR *__RPC_FAR *pToepassing,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditToepassing )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveToepassing )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetPrivilege )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ BSTR strUserName,
            /* [in] */ BSTR strWorkstation,
            /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
            /* [out] */ enum SPC_USERLEVEL __RPC_FAR *pUserLevel,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetOverzichtList )( 
            ISPCProces __RPC_FAR * This,
            /* [out] */ struct SPC_OVERZICHTLIST __RPC_FAR *pList,
            /* [in] */ BSTR UID);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetCompleteOverzichtList )( 
            ISPCProces __RPC_FAR * This,
            /* [out] */ struct SPC_OVERZICHTLIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetOverzicht )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
            /* [out] */ ISPCOverzicht __RPC_FAR *__RPC_FAR *pOverzicht);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetOverzichtInfo )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ int overzichtId,
            /* [out] */ struct SPC_OVERZICHTINFO __RPC_FAR *pOverzichtInfo);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *CreateOverzicht )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
            /* [out] */ ISPCOverzicht __RPC_FAR *__RPC_FAR *pOverzicht,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditOverzicht )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveOverzicht )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetProcesDataInfoList )( 
            ISPCProces __RPC_FAR * This,
            /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddProcesDataInfo )( 
            ISPCProces __RPC_FAR * This,
            /* [out][in] */ struct SPC_PROCESDATAINFO __RPC_FAR *pProcesData,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddProcesDataInfoList )( 
            ISPCProces __RPC_FAR * This,
            /* [out][in] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pProcesDataList,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditProcesDataInfo )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveProcesDataInfo )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetAccessInfoList )( 
            ISPCProces __RPC_FAR * This,
            /* [out] */ struct SPC_ACCESSINFOLIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddAccessInfo )( 
            ISPCProces __RPC_FAR * This,
            /* [out][in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditAccessInfo )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveAccessInfo )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetProcesDataInfoListEx )( 
            ISPCProces __RPC_FAR * This,
            /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pProcesdataInfoList,
            /* [out] */ struct SPC_ACCESSINFOLIST __RPC_FAR *pAccessInfoList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddProcesDataInfoEx )( 
            ISPCProces __RPC_FAR * This,
            /* [out][in] */ struct SPC_PROCESDATAINFO __RPC_FAR *pProcesData,
            /* [out][in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditProcesDataInfoEx )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [in] */ struct SPC_ACCESSINFO accessInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveProcesDataInfoEx )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [in] */ struct SPC_ACCESSINFO accessInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetAlarmLoggingList )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ double van,
            /* [in] */ double tot,
            /* [in] */ int maxAantal,
            /* [in] */ DWORD alarmFilter,
            /* [in] */ enum SPC_OBJECT_TYPE objectType,
            /* [in] */ int objectId,
            /* [out] */ struct SPC_ALARMLIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddManualData )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *ChangedManualData )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetTimeOldestDataRecord )( 
            ISPCProces __RPC_FAR * This,
            /* [out] */ double __RPC_FAR *pOldestTime);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *NewRemark )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ int opmerkingID,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartData,
            /* [string][in] */ char __RPC_FAR *szIngegevenDoor,
            /* [in] */ BOOL internePO);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditRemark )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ int opmerkingID,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartData,
            /* [string][in] */ char __RPC_FAR *szLaatstGewijzigdDoor);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveRemark )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ int opmerkingID,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartData,
            /* [string][in] */ char __RPC_FAR *szGewistDoor);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetRemark )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ int opmerkingSequenceNr,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartData,
            /* [string][out] */ char __RPC_FAR *__RPC_FAR *pUserName,
            /* [out] */ double __RPC_FAR *pTijdVanWijziging);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetVerzamelkaartList )( 
            ISPCProces __RPC_FAR * This,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pKaartConfigAsRWOrdered);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *CreateVerzamelkaart )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartCnfgBaseIn,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartCnfgBaseOut);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditVerzamelkaart )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartConfiguratieBase);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveVerzamelkaart )( 
            ISPCProces __RPC_FAR * This,
            /* [in] */ int idKaart);
        
        END_INTERFACE
    } ISPCProcesVtbl;

    interface ISPCProces
    {
        CONST_VTBL struct ISPCProcesVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISPCProces_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISPCProces_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISPCProces_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISPCProces_GetVerantwList(This,pVerantwList)	\
    (This)->lpVtbl -> GetVerantwList(This,pVerantwList)

#define ISPCProces_AddVerantw(This,pVerantw,pError)	\
    (This)->lpVtbl -> AddVerantw(This,pVerantw,pError)

#define ISPCProces_RemoveVerantw(This,verantw,pError)	\
    (This)->lpVtbl -> RemoveVerantw(This,verantw,pError)

#define ISPCProces_GetToepassingList(This,pToepList)	\
    (This)->lpVtbl -> GetToepassingList(This,pToepList)

#define ISPCProces_GetToepassing(This,toepInfo,pToepassing)	\
    (This)->lpVtbl -> GetToepassing(This,toepInfo,pToepassing)

#define ISPCProces_GetToepassingInfo(This,toepId,pToepInfo)	\
    (This)->lpVtbl -> GetToepassingInfo(This,toepId,pToepInfo)

#define ISPCProces_CreateToepassing(This,toepInfo,initVerantw,pToepassing,pError)	\
    (This)->lpVtbl -> CreateToepassing(This,toepInfo,initVerantw,pToepassing,pError)

#define ISPCProces_EditToepassing(This,toepInfo,pError)	\
    (This)->lpVtbl -> EditToepassing(This,toepInfo,pError)

#define ISPCProces_RemoveToepassing(This,toepInfo,pError)	\
    (This)->lpVtbl -> RemoveToepassing(This,toepInfo,pError)

#define ISPCProces_GetPrivilege(This,strUserName,strWorkstation,toepInfo,pUserLevel,pError)	\
    (This)->lpVtbl -> GetPrivilege(This,strUserName,strWorkstation,toepInfo,pUserLevel,pError)

#define ISPCProces_GetOverzichtList(This,pList,UID)	\
    (This)->lpVtbl -> GetOverzichtList(This,pList,UID)

#define ISPCProces_GetCompleteOverzichtList(This,pList)	\
    (This)->lpVtbl -> GetCompleteOverzichtList(This,pList)

#define ISPCProces_GetOverzicht(This,overzichtInfo,pOverzicht)	\
    (This)->lpVtbl -> GetOverzicht(This,overzichtInfo,pOverzicht)

#define ISPCProces_GetOverzichtInfo(This,overzichtId,pOverzichtInfo)	\
    (This)->lpVtbl -> GetOverzichtInfo(This,overzichtId,pOverzichtInfo)

#define ISPCProces_CreateOverzicht(This,overzichtInfo,pOverzicht,pError)	\
    (This)->lpVtbl -> CreateOverzicht(This,overzichtInfo,pOverzicht,pError)

#define ISPCProces_EditOverzicht(This,overzichtInfo,pError)	\
    (This)->lpVtbl -> EditOverzicht(This,overzichtInfo,pError)

#define ISPCProces_RemoveOverzicht(This,overzichtInfo,pError)	\
    (This)->lpVtbl -> RemoveOverzicht(This,overzichtInfo,pError)

#define ISPCProces_GetProcesDataInfoList(This,pList)	\
    (This)->lpVtbl -> GetProcesDataInfoList(This,pList)

#define ISPCProces_AddProcesDataInfo(This,pProcesData,pError)	\
    (This)->lpVtbl -> AddProcesDataInfo(This,pProcesData,pError)

#define ISPCProces_AddProcesDataInfoList(This,pProcesDataList,pError)	\
    (This)->lpVtbl -> AddProcesDataInfoList(This,pProcesDataList,pError)

#define ISPCProces_EditProcesDataInfo(This,procesData,pError)	\
    (This)->lpVtbl -> EditProcesDataInfo(This,procesData,pError)

#define ISPCProces_RemoveProcesDataInfo(This,procesData,pError)	\
    (This)->lpVtbl -> RemoveProcesDataInfo(This,procesData,pError)

#define ISPCProces_GetAccessInfoList(This,pList)	\
    (This)->lpVtbl -> GetAccessInfoList(This,pList)

#define ISPCProces_AddAccessInfo(This,pAccessInfo,pError)	\
    (This)->lpVtbl -> AddAccessInfo(This,pAccessInfo,pError)

#define ISPCProces_EditAccessInfo(This,pAccessInfo,pError)	\
    (This)->lpVtbl -> EditAccessInfo(This,pAccessInfo,pError)

#define ISPCProces_RemoveAccessInfo(This,pAccessInfo,pError)	\
    (This)->lpVtbl -> RemoveAccessInfo(This,pAccessInfo,pError)

#define ISPCProces_GetProcesDataInfoListEx(This,pProcesdataInfoList,pAccessInfoList)	\
    (This)->lpVtbl -> GetProcesDataInfoListEx(This,pProcesdataInfoList,pAccessInfoList)

#define ISPCProces_AddProcesDataInfoEx(This,pProcesData,pAccessInfo,pError)	\
    (This)->lpVtbl -> AddProcesDataInfoEx(This,pProcesData,pAccessInfo,pError)

#define ISPCProces_EditProcesDataInfoEx(This,procesData,accessInfo,pError)	\
    (This)->lpVtbl -> EditProcesDataInfoEx(This,procesData,accessInfo,pError)

#define ISPCProces_RemoveProcesDataInfoEx(This,procesData,accessInfo,pError)	\
    (This)->lpVtbl -> RemoveProcesDataInfoEx(This,procesData,accessInfo,pError)

#define ISPCProces_GetAlarmLoggingList(This,van,tot,maxAantal,alarmFilter,objectType,objectId,pList)	\
    (This)->lpVtbl -> GetAlarmLoggingList(This,van,tot,maxAantal,alarmFilter,objectType,objectId,pList)

#define ISPCProces_AddManualData(This,recordList)	\
    (This)->lpVtbl -> AddManualData(This,recordList)

#define ISPCProces_ChangedManualData(This,recordList)	\
    (This)->lpVtbl -> ChangedManualData(This,recordList)

#define ISPCProces_GetTimeOldestDataRecord(This,pOldestTime)	\
    (This)->lpVtbl -> GetTimeOldestDataRecord(This,pOldestTime)

#define ISPCProces_NewRemark(This,opmerkingID,arg_GSPCKaartData,szIngegevenDoor,internePO)	\
    (This)->lpVtbl -> NewRemark(This,opmerkingID,arg_GSPCKaartData,szIngegevenDoor,internePO)

#define ISPCProces_EditRemark(This,opmerkingID,arg_GSPCKaartData,szLaatstGewijzigdDoor)	\
    (This)->lpVtbl -> EditRemark(This,opmerkingID,arg_GSPCKaartData,szLaatstGewijzigdDoor)

#define ISPCProces_RemoveRemark(This,opmerkingID,arg_GSPCKaartData,szGewistDoor)	\
    (This)->lpVtbl -> RemoveRemark(This,opmerkingID,arg_GSPCKaartData,szGewistDoor)

#define ISPCProces_GetRemark(This,opmerkingSequenceNr,pArg_GSPCKaartData,pUserName,pTijdVanWijziging)	\
    (This)->lpVtbl -> GetRemark(This,opmerkingSequenceNr,pArg_GSPCKaartData,pUserName,pTijdVanWijziging)

#define ISPCProces_GetVerzamelkaartList(This,pKaartConfigAsRWOrdered)	\
    (This)->lpVtbl -> GetVerzamelkaartList(This,pKaartConfigAsRWOrdered)

#define ISPCProces_CreateVerzamelkaart(This,arg_GSPCKaartCnfgBaseIn,pArg_GSPCKaartCnfgBaseOut)	\
    (This)->lpVtbl -> CreateVerzamelkaart(This,arg_GSPCKaartCnfgBaseIn,pArg_GSPCKaartCnfgBaseOut)

#define ISPCProces_EditVerzamelkaart(This,arg_GSPCKaartConfiguratieBase)	\
    (This)->lpVtbl -> EditVerzamelkaart(This,arg_GSPCKaartConfiguratieBase)

#define ISPCProces_RemoveVerzamelkaart(This,idKaart)	\
    (This)->lpVtbl -> RemoveVerzamelkaart(This,idKaart)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE ISPCProces_GetVerantwList_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out] */ struct SPC_VERANTWOORDELIJKELIST __RPC_FAR *pVerantwList);


void __RPC_STUB ISPCProces_GetVerantwList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_AddVerantw_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out][in] */ struct SPC_VERANTWOORDELIJKE __RPC_FAR *pVerantw,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_AddVerantw_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_RemoveVerantw_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_RemoveVerantw_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetToepassingList_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out] */ struct SPC_TOEPASSINGLIST __RPC_FAR *pToepList);


void __RPC_STUB ISPCProces_GetToepassingList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetToepassing_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
    /* [out] */ ISPCToepassing __RPC_FAR *__RPC_FAR *pToepassing);


void __RPC_STUB ISPCProces_GetToepassing_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetToepassingInfo_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ int toepId,
    /* [out] */ struct SPC_TOEPASSINGINFO __RPC_FAR *pToepInfo);


void __RPC_STUB ISPCProces_GetToepassingInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_CreateToepassing_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
    /* [in] */ struct SPC_VERANTWOORDELIJKE initVerantw,
    /* [out] */ ISPCToepassing __RPC_FAR *__RPC_FAR *pToepassing,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_CreateToepassing_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_EditToepassing_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_EditToepassing_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_RemoveToepassing_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_RemoveToepassing_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetPrivilege_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ BSTR strUserName,
    /* [in] */ BSTR strWorkstation,
    /* [in] */ struct SPC_TOEPASSINGINFO toepInfo,
    /* [out] */ enum SPC_USERLEVEL __RPC_FAR *pUserLevel,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_GetPrivilege_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetOverzichtList_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out] */ struct SPC_OVERZICHTLIST __RPC_FAR *pList,
    /* [in] */ BSTR UID);


void __RPC_STUB ISPCProces_GetOverzichtList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetCompleteOverzichtList_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out] */ struct SPC_OVERZICHTLIST __RPC_FAR *pList);


void __RPC_STUB ISPCProces_GetCompleteOverzichtList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetOverzicht_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
    /* [out] */ ISPCOverzicht __RPC_FAR *__RPC_FAR *pOverzicht);


void __RPC_STUB ISPCProces_GetOverzicht_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetOverzichtInfo_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ int overzichtId,
    /* [out] */ struct SPC_OVERZICHTINFO __RPC_FAR *pOverzichtInfo);


void __RPC_STUB ISPCProces_GetOverzichtInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_CreateOverzicht_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
    /* [out] */ ISPCOverzicht __RPC_FAR *__RPC_FAR *pOverzicht,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_CreateOverzicht_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_EditOverzicht_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_EditOverzicht_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_RemoveOverzicht_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_OVERZICHTINFO overzichtInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_RemoveOverzicht_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetProcesDataInfoList_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pList);


void __RPC_STUB ISPCProces_GetProcesDataInfoList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_AddProcesDataInfo_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out][in] */ struct SPC_PROCESDATAINFO __RPC_FAR *pProcesData,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_AddProcesDataInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_AddProcesDataInfoList_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out][in] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pProcesDataList,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_AddProcesDataInfoList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_EditProcesDataInfo_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_PROCESDATAINFO procesData,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_EditProcesDataInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_RemoveProcesDataInfo_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_PROCESDATAINFO procesData,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_RemoveProcesDataInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetAccessInfoList_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out] */ struct SPC_ACCESSINFOLIST __RPC_FAR *pList);


void __RPC_STUB ISPCProces_GetAccessInfoList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_AddAccessInfo_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out][in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_AddAccessInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_EditAccessInfo_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_EditAccessInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_RemoveAccessInfo_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_RemoveAccessInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetProcesDataInfoListEx_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pProcesdataInfoList,
    /* [out] */ struct SPC_ACCESSINFOLIST __RPC_FAR *pAccessInfoList);


void __RPC_STUB ISPCProces_GetProcesDataInfoListEx_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_AddProcesDataInfoEx_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out][in] */ struct SPC_PROCESDATAINFO __RPC_FAR *pProcesData,
    /* [out][in] */ struct SPC_ACCESSINFO __RPC_FAR *pAccessInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_AddProcesDataInfoEx_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_EditProcesDataInfoEx_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_PROCESDATAINFO procesData,
    /* [in] */ struct SPC_ACCESSINFO accessInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_EditProcesDataInfoEx_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_RemoveProcesDataInfoEx_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_PROCESDATAINFO procesData,
    /* [in] */ struct SPC_ACCESSINFO accessInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCProces_RemoveProcesDataInfoEx_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetAlarmLoggingList_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ double van,
    /* [in] */ double tot,
    /* [in] */ int maxAantal,
    /* [in] */ DWORD alarmFilter,
    /* [in] */ enum SPC_OBJECT_TYPE objectType,
    /* [in] */ int objectId,
    /* [out] */ struct SPC_ALARMLIST __RPC_FAR *pList);


void __RPC_STUB ISPCProces_GetAlarmLoggingList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_AddManualData_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList);


void __RPC_STUB ISPCProces_AddManualData_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_ChangedManualData_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList);


void __RPC_STUB ISPCProces_ChangedManualData_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetTimeOldestDataRecord_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out] */ double __RPC_FAR *pOldestTime);


void __RPC_STUB ISPCProces_GetTimeOldestDataRecord_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_NewRemark_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ int opmerkingID,
    /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartData,
    /* [string][in] */ char __RPC_FAR *szIngegevenDoor,
    /* [in] */ BOOL internePO);


void __RPC_STUB ISPCProces_NewRemark_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_EditRemark_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ int opmerkingID,
    /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartData,
    /* [string][in] */ char __RPC_FAR *szLaatstGewijzigdDoor);


void __RPC_STUB ISPCProces_EditRemark_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_RemoveRemark_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ int opmerkingID,
    /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartData,
    /* [string][in] */ char __RPC_FAR *szGewistDoor);


void __RPC_STUB ISPCProces_RemoveRemark_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetRemark_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ int opmerkingSequenceNr,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartData,
    /* [string][out] */ char __RPC_FAR *__RPC_FAR *pUserName,
    /* [out] */ double __RPC_FAR *pTijdVanWijziging);


void __RPC_STUB ISPCProces_GetRemark_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_GetVerzamelkaartList_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pKaartConfigAsRWOrdered);


void __RPC_STUB ISPCProces_GetVerzamelkaartList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_CreateVerzamelkaart_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartCnfgBaseIn,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartCnfgBaseOut);


void __RPC_STUB ISPCProces_CreateVerzamelkaart_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_EditVerzamelkaart_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartConfiguratieBase);


void __RPC_STUB ISPCProces_EditVerzamelkaart_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProces_RemoveVerzamelkaart_Proxy( 
    ISPCProces __RPC_FAR * This,
    /* [in] */ int idKaart);


void __RPC_STUB ISPCProces_RemoveVerzamelkaart_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISPCProces_INTERFACE_DEFINED__ */


#ifndef __ISPCProductieOpmerking_INTERFACE_DEFINED__
#define __ISPCProductieOpmerking_INTERFACE_DEFINED__

/* interface ISPCProductieOpmerking */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_ISPCProductieOpmerking;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("8C87A020-263C-11d4-9140-0050DA5E4B9B")
    ISPCProductieOpmerking : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE NewRemark( 
            /* [in] */ double utcTijd,
            /* [in] */ int kaartId,
            /* [in] */ long kaartSequentieNr,
            /* [in] */ int opmerkingsCode,
            /* [in] */ int oorzaakCode,
            /* [string][in] */ char __RPC_FAR *szVrijeOpmerking,
            /* [out] */ int __RPC_FAR *pOpmerkingID) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditRemark( 
            /* [in] */ int opmerkingsCode,
            /* [in] */ int oorzaakCode,
            /* [string][in] */ char __RPC_FAR *szVrijeOpmerking,
            /* [in] */ int opmerkingID) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveRemark( 
            /* [in] */ int opmerkingID) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetRemark( 
            /* [in] */ int opmerkingID,
            /* [out] */ int __RPC_FAR *pOpmerkingsCode,
            /* [out] */ int __RPC_FAR *pOorzaakCode,
            /* [string][out] */ char __RPC_FAR *__RPC_FAR *pVrijeOpmerking) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCodeLists( 
            /* [out] */ struct SPC_PRODUCTIE_CODE_LIST __RPC_FAR *pOpmerkingsCodeList,
            /* [out] */ struct SPC_PRODUCTIE_CODE_LIST __RPC_FAR *pOorzakenCodeList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddOpmerkingsCode( 
            /* [in] */ int codeID,
            /* [string][in] */ char __RPC_FAR *szOmschrijving) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddOorzakenCode( 
            /* [in] */ int codeID,
            /* [string][in] */ char __RPC_FAR *szOmschrijving) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditOpmerkingsCode( 
            /* [in] */ int codeID,
            /* [string][in] */ char __RPC_FAR *szOmschrijving) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditOorzakenCode( 
            /* [in] */ int codeID,
            /* [string][in] */ char __RPC_FAR *szOmschrijving) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveOpmerkingsCode( 
            /* [in] */ int codeID) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveOorzakenCode( 
            /* [in] */ int codeID) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISPCProductieOpmerkingVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISPCProductieOpmerking __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISPCProductieOpmerking __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *NewRemark )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [in] */ double utcTijd,
            /* [in] */ int kaartId,
            /* [in] */ long kaartSequentieNr,
            /* [in] */ int opmerkingsCode,
            /* [in] */ int oorzaakCode,
            /* [string][in] */ char __RPC_FAR *szVrijeOpmerking,
            /* [out] */ int __RPC_FAR *pOpmerkingID);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditRemark )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [in] */ int opmerkingsCode,
            /* [in] */ int oorzaakCode,
            /* [string][in] */ char __RPC_FAR *szVrijeOpmerking,
            /* [in] */ int opmerkingID);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveRemark )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [in] */ int opmerkingID);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetRemark )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [in] */ int opmerkingID,
            /* [out] */ int __RPC_FAR *pOpmerkingsCode,
            /* [out] */ int __RPC_FAR *pOorzaakCode,
            /* [string][out] */ char __RPC_FAR *__RPC_FAR *pVrijeOpmerking);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetCodeLists )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [out] */ struct SPC_PRODUCTIE_CODE_LIST __RPC_FAR *pOpmerkingsCodeList,
            /* [out] */ struct SPC_PRODUCTIE_CODE_LIST __RPC_FAR *pOorzakenCodeList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddOpmerkingsCode )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [in] */ int codeID,
            /* [string][in] */ char __RPC_FAR *szOmschrijving);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddOorzakenCode )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [in] */ int codeID,
            /* [string][in] */ char __RPC_FAR *szOmschrijving);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditOpmerkingsCode )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [in] */ int codeID,
            /* [string][in] */ char __RPC_FAR *szOmschrijving);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditOorzakenCode )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [in] */ int codeID,
            /* [string][in] */ char __RPC_FAR *szOmschrijving);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveOpmerkingsCode )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [in] */ int codeID);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveOorzakenCode )( 
            ISPCProductieOpmerking __RPC_FAR * This,
            /* [in] */ int codeID);
        
        END_INTERFACE
    } ISPCProductieOpmerkingVtbl;

    interface ISPCProductieOpmerking
    {
        CONST_VTBL struct ISPCProductieOpmerkingVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISPCProductieOpmerking_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISPCProductieOpmerking_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISPCProductieOpmerking_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISPCProductieOpmerking_NewRemark(This,utcTijd,kaartId,kaartSequentieNr,opmerkingsCode,oorzaakCode,szVrijeOpmerking,pOpmerkingID)	\
    (This)->lpVtbl -> NewRemark(This,utcTijd,kaartId,kaartSequentieNr,opmerkingsCode,oorzaakCode,szVrijeOpmerking,pOpmerkingID)

#define ISPCProductieOpmerking_EditRemark(This,opmerkingsCode,oorzaakCode,szVrijeOpmerking,opmerkingID)	\
    (This)->lpVtbl -> EditRemark(This,opmerkingsCode,oorzaakCode,szVrijeOpmerking,opmerkingID)

#define ISPCProductieOpmerking_RemoveRemark(This,opmerkingID)	\
    (This)->lpVtbl -> RemoveRemark(This,opmerkingID)

#define ISPCProductieOpmerking_GetRemark(This,opmerkingID,pOpmerkingsCode,pOorzaakCode,pVrijeOpmerking)	\
    (This)->lpVtbl -> GetRemark(This,opmerkingID,pOpmerkingsCode,pOorzaakCode,pVrijeOpmerking)

#define ISPCProductieOpmerking_GetCodeLists(This,pOpmerkingsCodeList,pOorzakenCodeList)	\
    (This)->lpVtbl -> GetCodeLists(This,pOpmerkingsCodeList,pOorzakenCodeList)

#define ISPCProductieOpmerking_AddOpmerkingsCode(This,codeID,szOmschrijving)	\
    (This)->lpVtbl -> AddOpmerkingsCode(This,codeID,szOmschrijving)

#define ISPCProductieOpmerking_AddOorzakenCode(This,codeID,szOmschrijving)	\
    (This)->lpVtbl -> AddOorzakenCode(This,codeID,szOmschrijving)

#define ISPCProductieOpmerking_EditOpmerkingsCode(This,codeID,szOmschrijving)	\
    (This)->lpVtbl -> EditOpmerkingsCode(This,codeID,szOmschrijving)

#define ISPCProductieOpmerking_EditOorzakenCode(This,codeID,szOmschrijving)	\
    (This)->lpVtbl -> EditOorzakenCode(This,codeID,szOmschrijving)

#define ISPCProductieOpmerking_RemoveOpmerkingsCode(This,codeID)	\
    (This)->lpVtbl -> RemoveOpmerkingsCode(This,codeID)

#define ISPCProductieOpmerking_RemoveOorzakenCode(This,codeID)	\
    (This)->lpVtbl -> RemoveOorzakenCode(This,codeID)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE ISPCProductieOpmerking_NewRemark_Proxy( 
    ISPCProductieOpmerking __RPC_FAR * This,
    /* [in] */ double utcTijd,
    /* [in] */ int kaartId,
    /* [in] */ long kaartSequentieNr,
    /* [in] */ int opmerkingsCode,
    /* [in] */ int oorzaakCode,
    /* [string][in] */ char __RPC_FAR *szVrijeOpmerking,
    /* [out] */ int __RPC_FAR *pOpmerkingID);


void __RPC_STUB ISPCProductieOpmerking_NewRemark_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProductieOpmerking_EditRemark_Proxy( 
    ISPCProductieOpmerking __RPC_FAR * This,
    /* [in] */ int opmerkingsCode,
    /* [in] */ int oorzaakCode,
    /* [string][in] */ char __RPC_FAR *szVrijeOpmerking,
    /* [in] */ int opmerkingID);


void __RPC_STUB ISPCProductieOpmerking_EditRemark_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProductieOpmerking_RemoveRemark_Proxy( 
    ISPCProductieOpmerking __RPC_FAR * This,
    /* [in] */ int opmerkingID);


void __RPC_STUB ISPCProductieOpmerking_RemoveRemark_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProductieOpmerking_GetRemark_Proxy( 
    ISPCProductieOpmerking __RPC_FAR * This,
    /* [in] */ int opmerkingID,
    /* [out] */ int __RPC_FAR *pOpmerkingsCode,
    /* [out] */ int __RPC_FAR *pOorzaakCode,
    /* [string][out] */ char __RPC_FAR *__RPC_FAR *pVrijeOpmerking);


void __RPC_STUB ISPCProductieOpmerking_GetRemark_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProductieOpmerking_GetCodeLists_Proxy( 
    ISPCProductieOpmerking __RPC_FAR * This,
    /* [out] */ struct SPC_PRODUCTIE_CODE_LIST __RPC_FAR *pOpmerkingsCodeList,
    /* [out] */ struct SPC_PRODUCTIE_CODE_LIST __RPC_FAR *pOorzakenCodeList);


void __RPC_STUB ISPCProductieOpmerking_GetCodeLists_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProductieOpmerking_AddOpmerkingsCode_Proxy( 
    ISPCProductieOpmerking __RPC_FAR * This,
    /* [in] */ int codeID,
    /* [string][in] */ char __RPC_FAR *szOmschrijving);


void __RPC_STUB ISPCProductieOpmerking_AddOpmerkingsCode_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProductieOpmerking_AddOorzakenCode_Proxy( 
    ISPCProductieOpmerking __RPC_FAR * This,
    /* [in] */ int codeID,
    /* [string][in] */ char __RPC_FAR *szOmschrijving);


void __RPC_STUB ISPCProductieOpmerking_AddOorzakenCode_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProductieOpmerking_EditOpmerkingsCode_Proxy( 
    ISPCProductieOpmerking __RPC_FAR * This,
    /* [in] */ int codeID,
    /* [string][in] */ char __RPC_FAR *szOmschrijving);


void __RPC_STUB ISPCProductieOpmerking_EditOpmerkingsCode_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProductieOpmerking_EditOorzakenCode_Proxy( 
    ISPCProductieOpmerking __RPC_FAR * This,
    /* [in] */ int codeID,
    /* [string][in] */ char __RPC_FAR *szOmschrijving);


void __RPC_STUB ISPCProductieOpmerking_EditOorzakenCode_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProductieOpmerking_RemoveOpmerkingsCode_Proxy( 
    ISPCProductieOpmerking __RPC_FAR * This,
    /* [in] */ int codeID);


void __RPC_STUB ISPCProductieOpmerking_RemoveOpmerkingsCode_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCProductieOpmerking_RemoveOorzakenCode_Proxy( 
    ISPCProductieOpmerking __RPC_FAR * This,
    /* [in] */ int codeID);


void __RPC_STUB ISPCProductieOpmerking_RemoveOorzakenCode_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISPCProductieOpmerking_INTERFACE_DEFINED__ */


#ifndef __ISPCToepassing_INTERFACE_DEFINED__
#define __ISPCToepassing_INTERFACE_DEFINED__

/* interface ISPCToepassing */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_ISPCToepassing;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("F6783763-E3B2-11D0-9110-AA0004007F05")
    ISPCToepassing : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetVerantwList( 
            /* [out] */ struct SPC_VERANTWOORDELIJKELIST __RPC_FAR *pVerantwList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddVerantw( 
            /* [in] */ struct SPC_VERANTWOORDELIJKE __RPC_FAR *pVerantw,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveVerantw( 
            /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetOnderwerpList( 
            /* [out] */ struct SPC_ONDERWERPLIST __RPC_FAR *pOnderwerpList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetOnderwerp( 
            /* [in] */ struct SPC_ONDERWERPINFO onderwerpInfo,
            /* [out] */ ISPCOnderwerp __RPC_FAR *__RPC_FAR *pIOnderwerp) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetOnderwerpInfo( 
            /* [in] */ int onderwerpId,
            /* [out] */ struct SPC_ONDERWERPINFO __RPC_FAR *pOnderwerpInfo) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CreateOnderwerp( 
            /* [out][in] */ struct SPC_ONDERWERPINFO __RPC_FAR *pOnderwerpInfo,
            /* [out] */ ISPCOnderwerp __RPC_FAR *__RPC_FAR *pOnderwerp,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditOnderwerp( 
            /* [in] */ struct SPC_ONDERWERPINFO onderwerpInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveOnderwerp( 
            /* [in] */ struct SPC_ONDERWERPINFO onderwerpInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISPCToepassingVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISPCToepassing __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISPCToepassing __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISPCToepassing __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetVerantwList )( 
            ISPCToepassing __RPC_FAR * This,
            /* [out] */ struct SPC_VERANTWOORDELIJKELIST __RPC_FAR *pVerantwList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddVerantw )( 
            ISPCToepassing __RPC_FAR * This,
            /* [in] */ struct SPC_VERANTWOORDELIJKE __RPC_FAR *pVerantw,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveVerantw )( 
            ISPCToepassing __RPC_FAR * This,
            /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetOnderwerpList )( 
            ISPCToepassing __RPC_FAR * This,
            /* [out] */ struct SPC_ONDERWERPLIST __RPC_FAR *pOnderwerpList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetOnderwerp )( 
            ISPCToepassing __RPC_FAR * This,
            /* [in] */ struct SPC_ONDERWERPINFO onderwerpInfo,
            /* [out] */ ISPCOnderwerp __RPC_FAR *__RPC_FAR *pIOnderwerp);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetOnderwerpInfo )( 
            ISPCToepassing __RPC_FAR * This,
            /* [in] */ int onderwerpId,
            /* [out] */ struct SPC_ONDERWERPINFO __RPC_FAR *pOnderwerpInfo);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *CreateOnderwerp )( 
            ISPCToepassing __RPC_FAR * This,
            /* [out][in] */ struct SPC_ONDERWERPINFO __RPC_FAR *pOnderwerpInfo,
            /* [out] */ ISPCOnderwerp __RPC_FAR *__RPC_FAR *pOnderwerp,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditOnderwerp )( 
            ISPCToepassing __RPC_FAR * This,
            /* [in] */ struct SPC_ONDERWERPINFO onderwerpInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveOnderwerp )( 
            ISPCToepassing __RPC_FAR * This,
            /* [in] */ struct SPC_ONDERWERPINFO onderwerpInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        END_INTERFACE
    } ISPCToepassingVtbl;

    interface ISPCToepassing
    {
        CONST_VTBL struct ISPCToepassingVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISPCToepassing_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISPCToepassing_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISPCToepassing_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISPCToepassing_GetVerantwList(This,pVerantwList)	\
    (This)->lpVtbl -> GetVerantwList(This,pVerantwList)

#define ISPCToepassing_AddVerantw(This,pVerantw,pError)	\
    (This)->lpVtbl -> AddVerantw(This,pVerantw,pError)

#define ISPCToepassing_RemoveVerantw(This,verantw,pError)	\
    (This)->lpVtbl -> RemoveVerantw(This,verantw,pError)

#define ISPCToepassing_GetOnderwerpList(This,pOnderwerpList)	\
    (This)->lpVtbl -> GetOnderwerpList(This,pOnderwerpList)

#define ISPCToepassing_GetOnderwerp(This,onderwerpInfo,pIOnderwerp)	\
    (This)->lpVtbl -> GetOnderwerp(This,onderwerpInfo,pIOnderwerp)

#define ISPCToepassing_GetOnderwerpInfo(This,onderwerpId,pOnderwerpInfo)	\
    (This)->lpVtbl -> GetOnderwerpInfo(This,onderwerpId,pOnderwerpInfo)

#define ISPCToepassing_CreateOnderwerp(This,pOnderwerpInfo,pOnderwerp,pError)	\
    (This)->lpVtbl -> CreateOnderwerp(This,pOnderwerpInfo,pOnderwerp,pError)

#define ISPCToepassing_EditOnderwerp(This,onderwerpInfo,pError)	\
    (This)->lpVtbl -> EditOnderwerp(This,onderwerpInfo,pError)

#define ISPCToepassing_RemoveOnderwerp(This,onderwerpInfo,pError)	\
    (This)->lpVtbl -> RemoveOnderwerp(This,onderwerpInfo,pError)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE ISPCToepassing_GetVerantwList_Proxy( 
    ISPCToepassing __RPC_FAR * This,
    /* [out] */ struct SPC_VERANTWOORDELIJKELIST __RPC_FAR *pVerantwList);


void __RPC_STUB ISPCToepassing_GetVerantwList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCToepassing_AddVerantw_Proxy( 
    ISPCToepassing __RPC_FAR * This,
    /* [in] */ struct SPC_VERANTWOORDELIJKE __RPC_FAR *pVerantw,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCToepassing_AddVerantw_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCToepassing_RemoveVerantw_Proxy( 
    ISPCToepassing __RPC_FAR * This,
    /* [in] */ struct SPC_VERANTWOORDELIJKE verantw,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCToepassing_RemoveVerantw_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCToepassing_GetOnderwerpList_Proxy( 
    ISPCToepassing __RPC_FAR * This,
    /* [out] */ struct SPC_ONDERWERPLIST __RPC_FAR *pOnderwerpList);


void __RPC_STUB ISPCToepassing_GetOnderwerpList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCToepassing_GetOnderwerp_Proxy( 
    ISPCToepassing __RPC_FAR * This,
    /* [in] */ struct SPC_ONDERWERPINFO onderwerpInfo,
    /* [out] */ ISPCOnderwerp __RPC_FAR *__RPC_FAR *pIOnderwerp);


void __RPC_STUB ISPCToepassing_GetOnderwerp_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCToepassing_GetOnderwerpInfo_Proxy( 
    ISPCToepassing __RPC_FAR * This,
    /* [in] */ int onderwerpId,
    /* [out] */ struct SPC_ONDERWERPINFO __RPC_FAR *pOnderwerpInfo);


void __RPC_STUB ISPCToepassing_GetOnderwerpInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCToepassing_CreateOnderwerp_Proxy( 
    ISPCToepassing __RPC_FAR * This,
    /* [out][in] */ struct SPC_ONDERWERPINFO __RPC_FAR *pOnderwerpInfo,
    /* [out] */ ISPCOnderwerp __RPC_FAR *__RPC_FAR *pOnderwerp,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCToepassing_CreateOnderwerp_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCToepassing_EditOnderwerp_Proxy( 
    ISPCToepassing __RPC_FAR * This,
    /* [in] */ struct SPC_ONDERWERPINFO onderwerpInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCToepassing_EditOnderwerp_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCToepassing_RemoveOnderwerp_Proxy( 
    ISPCToepassing __RPC_FAR * This,
    /* [in] */ struct SPC_ONDERWERPINFO onderwerpInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCToepassing_RemoveOnderwerp_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISPCToepassing_INTERFACE_DEFINED__ */


#ifndef __ISPCOnderwerp_INTERFACE_DEFINED__
#define __ISPCOnderwerp_INTERFACE_DEFINED__

/* interface ISPCOnderwerp */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_ISPCOnderwerp;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("F6783765-E3B2-11D0-9110-AA0004007F05")
    ISPCOnderwerp : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetProcesDataInfoList( 
            /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetPuntGroep( 
            /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddToPuntGroep( 
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveFromPuntGroep( 
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetPuntGroepRelationship( 
            /* [in] */ struct SPC_PUNTGROEP_RELATIONSHIP relation,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetPuntGroepRelationship( 
            /* [out] */ struct SPC_PUNTGROEP_RELATIONSHIP __RPC_FAR *pRelation) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetGebeurtenisGroep( 
            /* [out] */ struct SPC_GEBEURTENISGROEP __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddToGebeurtenisGroep( 
            /* [in] */ int IDProcesDataInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveFromGebeurtenisGroep( 
            /* [in] */ int IDProcesDataInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ModelExists( 
            /* [in] */ enum MODELCATEGORY category,
            /* [out] */ BOOL __RPC_FAR *pReturn) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetModel( 
            /* [in] */ enum MODELCATEGORY category,
            /* [out] */ ISPCModel __RPC_FAR *__RPC_FAR *pModel) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CreateStudyModel( 
            /* [in] */ struct SPC_MODELINFO modelInfo,
            /* [out] */ ISPCModel __RPC_FAR *__RPC_FAR *pModel,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetModelInfo( 
            /* [in] */ enum MODELCATEGORY category,
            /* [out] */ struct SPC_MODELINFO __RPC_FAR *pInfo) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditModel( 
            /* [in] */ enum MODELCATEGORY category,
            /* [in] */ struct SPC_MODELINFO info,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveModel( 
            /* [in] */ enum MODELCATEGORY category,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CopyStudyToProduction( 
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CopyProductionToStudy( 
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetLastManualData( 
            /* [in] */ int aantal,
            /* [out] */ struct SPC_MANUELE_DATA_RECORDLIST __RPC_FAR *pList,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CheckManualData( 
            /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList,
            /* [out] */ struct SPC_MANUELE_DATA_ERRORLIST __RPC_FAR *pErrorList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ClearManualDataCheckList( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetOperatorList( 
            /* [out] */ struct SPC_OPERATORLIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddOperator( 
            /* [in] */ int IDVerantw,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveOperator( 
            /* [in] */ int IDVerantw,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetDescription( 
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pDataInfo) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetDataSelection( 
            /* [in] */ double from,
            /* [in] */ double to,
            /* [in] */ int maxRecords,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCDataOrderedIn,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCDataOrderedOut,
            /* [out][in] */ DWORD __RPC_FAR *pdwNext) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISPCOnderwerpVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISPCOnderwerp __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISPCOnderwerp __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetProcesDataInfoList )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetPuntGroep )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddToPuntGroep )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveFromPuntGroep )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ struct SPC_PROCESDATAINFO procesData,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetPuntGroepRelationship )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ struct SPC_PUNTGROEP_RELATIONSHIP relation,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetPuntGroepRelationship )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [out] */ struct SPC_PUNTGROEP_RELATIONSHIP __RPC_FAR *pRelation);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetGebeurtenisGroep )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [out] */ struct SPC_GEBEURTENISGROEP __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddToGebeurtenisGroep )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ int IDProcesDataInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveFromGebeurtenisGroep )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ int IDProcesDataInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *ModelExists )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ enum MODELCATEGORY category,
            /* [out] */ BOOL __RPC_FAR *pReturn);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetModel )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ enum MODELCATEGORY category,
            /* [out] */ ISPCModel __RPC_FAR *__RPC_FAR *pModel);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *CreateStudyModel )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ struct SPC_MODELINFO modelInfo,
            /* [out] */ ISPCModel __RPC_FAR *__RPC_FAR *pModel,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetModelInfo )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ enum MODELCATEGORY category,
            /* [out] */ struct SPC_MODELINFO __RPC_FAR *pInfo);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditModel )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ enum MODELCATEGORY category,
            /* [in] */ struct SPC_MODELINFO info,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveModel )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ enum MODELCATEGORY category,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *CopyStudyToProduction )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *CopyProductionToStudy )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetLastManualData )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ int aantal,
            /* [out] */ struct SPC_MANUELE_DATA_RECORDLIST __RPC_FAR *pList,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *CheckManualData )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList,
            /* [out] */ struct SPC_MANUELE_DATA_ERRORLIST __RPC_FAR *pErrorList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *ClearManualDataCheckList )( 
            ISPCOnderwerp __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetOperatorList )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [out] */ struct SPC_OPERATORLIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddOperator )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ int IDVerantw,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveOperator )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ int IDVerantw,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetDescription )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pDataInfo);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetDataSelection )( 
            ISPCOnderwerp __RPC_FAR * This,
            /* [in] */ double from,
            /* [in] */ double to,
            /* [in] */ int maxRecords,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCDataOrderedIn,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCDataOrderedOut,
            /* [out][in] */ DWORD __RPC_FAR *pdwNext);
        
        END_INTERFACE
    } ISPCOnderwerpVtbl;

    interface ISPCOnderwerp
    {
        CONST_VTBL struct ISPCOnderwerpVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISPCOnderwerp_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISPCOnderwerp_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISPCOnderwerp_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISPCOnderwerp_GetProcesDataInfoList(This,pList)	\
    (This)->lpVtbl -> GetProcesDataInfoList(This,pList)

#define ISPCOnderwerp_GetPuntGroep(This,pList)	\
    (This)->lpVtbl -> GetPuntGroep(This,pList)

#define ISPCOnderwerp_AddToPuntGroep(This,procesData,pError)	\
    (This)->lpVtbl -> AddToPuntGroep(This,procesData,pError)

#define ISPCOnderwerp_RemoveFromPuntGroep(This,procesData,pError)	\
    (This)->lpVtbl -> RemoveFromPuntGroep(This,procesData,pError)

#define ISPCOnderwerp_SetPuntGroepRelationship(This,relation,pError)	\
    (This)->lpVtbl -> SetPuntGroepRelationship(This,relation,pError)

#define ISPCOnderwerp_GetPuntGroepRelationship(This,pRelation)	\
    (This)->lpVtbl -> GetPuntGroepRelationship(This,pRelation)

#define ISPCOnderwerp_GetGebeurtenisGroep(This,pList)	\
    (This)->lpVtbl -> GetGebeurtenisGroep(This,pList)

#define ISPCOnderwerp_AddToGebeurtenisGroep(This,IDProcesDataInfo,pError)	\
    (This)->lpVtbl -> AddToGebeurtenisGroep(This,IDProcesDataInfo,pError)

#define ISPCOnderwerp_RemoveFromGebeurtenisGroep(This,IDProcesDataInfo,pError)	\
    (This)->lpVtbl -> RemoveFromGebeurtenisGroep(This,IDProcesDataInfo,pError)

#define ISPCOnderwerp_ModelExists(This,category,pReturn)	\
    (This)->lpVtbl -> ModelExists(This,category,pReturn)

#define ISPCOnderwerp_GetModel(This,category,pModel)	\
    (This)->lpVtbl -> GetModel(This,category,pModel)

#define ISPCOnderwerp_CreateStudyModel(This,modelInfo,pModel,pError)	\
    (This)->lpVtbl -> CreateStudyModel(This,modelInfo,pModel,pError)

#define ISPCOnderwerp_GetModelInfo(This,category,pInfo)	\
    (This)->lpVtbl -> GetModelInfo(This,category,pInfo)

#define ISPCOnderwerp_EditModel(This,category,info,pError)	\
    (This)->lpVtbl -> EditModel(This,category,info,pError)

#define ISPCOnderwerp_RemoveModel(This,category,pError)	\
    (This)->lpVtbl -> RemoveModel(This,category,pError)

#define ISPCOnderwerp_CopyStudyToProduction(This,pError)	\
    (This)->lpVtbl -> CopyStudyToProduction(This,pError)

#define ISPCOnderwerp_CopyProductionToStudy(This,pError)	\
    (This)->lpVtbl -> CopyProductionToStudy(This,pError)

#define ISPCOnderwerp_GetLastManualData(This,aantal,pList,pError)	\
    (This)->lpVtbl -> GetLastManualData(This,aantal,pList,pError)

#define ISPCOnderwerp_CheckManualData(This,recordList,pErrorList)	\
    (This)->lpVtbl -> CheckManualData(This,recordList,pErrorList)

#define ISPCOnderwerp_ClearManualDataCheckList(This)	\
    (This)->lpVtbl -> ClearManualDataCheckList(This)

#define ISPCOnderwerp_GetOperatorList(This,pList)	\
    (This)->lpVtbl -> GetOperatorList(This,pList)

#define ISPCOnderwerp_AddOperator(This,IDVerantw,pError)	\
    (This)->lpVtbl -> AddOperator(This,IDVerantw,pError)

#define ISPCOnderwerp_RemoveOperator(This,IDVerantw,pError)	\
    (This)->lpVtbl -> RemoveOperator(This,IDVerantw,pError)

#define ISPCOnderwerp_GetDescription(This,pDataInfo)	\
    (This)->lpVtbl -> GetDescription(This,pDataInfo)

#define ISPCOnderwerp_GetDataSelection(This,from,to,maxRecords,arg_GSPCDataOrderedIn,pArg_GSPCDataOrderedOut,pdwNext)	\
    (This)->lpVtbl -> GetDataSelection(This,from,to,maxRecords,arg_GSPCDataOrderedIn,pArg_GSPCDataOrderedOut,pdwNext)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE ISPCOnderwerp_GetProcesDataInfoList_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pList);


void __RPC_STUB ISPCOnderwerp_GetProcesDataInfoList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_GetPuntGroep_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [out] */ struct SPC_PROCESDATAINFOLIST __RPC_FAR *pList);


void __RPC_STUB ISPCOnderwerp_GetPuntGroep_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_AddToPuntGroep_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ struct SPC_PROCESDATAINFO procesData,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_AddToPuntGroep_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_RemoveFromPuntGroep_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ struct SPC_PROCESDATAINFO procesData,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_RemoveFromPuntGroep_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_SetPuntGroepRelationship_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ struct SPC_PUNTGROEP_RELATIONSHIP relation,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_SetPuntGroepRelationship_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_GetPuntGroepRelationship_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [out] */ struct SPC_PUNTGROEP_RELATIONSHIP __RPC_FAR *pRelation);


void __RPC_STUB ISPCOnderwerp_GetPuntGroepRelationship_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_GetGebeurtenisGroep_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [out] */ struct SPC_GEBEURTENISGROEP __RPC_FAR *pList);


void __RPC_STUB ISPCOnderwerp_GetGebeurtenisGroep_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_AddToGebeurtenisGroep_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ int IDProcesDataInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_AddToGebeurtenisGroep_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_RemoveFromGebeurtenisGroep_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ int IDProcesDataInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_RemoveFromGebeurtenisGroep_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_ModelExists_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ enum MODELCATEGORY category,
    /* [out] */ BOOL __RPC_FAR *pReturn);


void __RPC_STUB ISPCOnderwerp_ModelExists_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_GetModel_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ enum MODELCATEGORY category,
    /* [out] */ ISPCModel __RPC_FAR *__RPC_FAR *pModel);


void __RPC_STUB ISPCOnderwerp_GetModel_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_CreateStudyModel_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ struct SPC_MODELINFO modelInfo,
    /* [out] */ ISPCModel __RPC_FAR *__RPC_FAR *pModel,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_CreateStudyModel_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_GetModelInfo_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ enum MODELCATEGORY category,
    /* [out] */ struct SPC_MODELINFO __RPC_FAR *pInfo);


void __RPC_STUB ISPCOnderwerp_GetModelInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_EditModel_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ enum MODELCATEGORY category,
    /* [in] */ struct SPC_MODELINFO info,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_EditModel_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_RemoveModel_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ enum MODELCATEGORY category,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_RemoveModel_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_CopyStudyToProduction_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_CopyStudyToProduction_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_CopyProductionToStudy_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_CopyProductionToStudy_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_GetLastManualData_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ int aantal,
    /* [out] */ struct SPC_MANUELE_DATA_RECORDLIST __RPC_FAR *pList,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_GetLastManualData_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_CheckManualData_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList,
    /* [out] */ struct SPC_MANUELE_DATA_ERRORLIST __RPC_FAR *pErrorList);


void __RPC_STUB ISPCOnderwerp_CheckManualData_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_ClearManualDataCheckList_Proxy( 
    ISPCOnderwerp __RPC_FAR * This);


void __RPC_STUB ISPCOnderwerp_ClearManualDataCheckList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_GetOperatorList_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [out] */ struct SPC_OPERATORLIST __RPC_FAR *pList);


void __RPC_STUB ISPCOnderwerp_GetOperatorList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_AddOperator_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ int IDVerantw,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_AddOperator_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_RemoveOperator_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ int IDVerantw,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOnderwerp_RemoveOperator_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_GetDescription_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pDataInfo);


void __RPC_STUB ISPCOnderwerp_GetDescription_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOnderwerp_GetDataSelection_Proxy( 
    ISPCOnderwerp __RPC_FAR * This,
    /* [in] */ double from,
    /* [in] */ double to,
    /* [in] */ int maxRecords,
    /* [in] */ struct SPC_BINARY_DATA arg_GSPCDataOrderedIn,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCDataOrderedOut,
    /* [out][in] */ DWORD __RPC_FAR *pdwNext);


void __RPC_STUB ISPCOnderwerp_GetDataSelection_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISPCOnderwerp_INTERFACE_DEFINED__ */


#ifndef __ISPCModel_INTERFACE_DEFINED__
#define __ISPCModel_INTERFACE_DEFINED__

/* interface ISPCModel */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_ISPCModel;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("8BF56DEE-3BE3-11D1-991F-00C04FB9742F")
    ISPCModel : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE SetModelSettings( 
            /* [in] */ struct SPC_MODELSETTINGS setting) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetModelSettings( 
            /* [out] */ struct SPC_MODELSETTINGS __RPC_FAR *pSetting) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetModelSettingsEx( 
            /* [in] */ struct SPC_MODELSETTINGS_EX setting) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetModelSettingsEx( 
            /* [out] */ struct SPC_MODELSETTINGS_EX __RPC_FAR *pSetting) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetHoofdVariabele( 
            /* [in] */ struct SPC_HOOFDVARIABELE hoofdVar) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetHoofdVariabele( 
            /* [out] */ struct SPC_HOOFDVARIABELE __RPC_FAR *pHoofdVar) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditHoofdVariabele( 
            /* [in] */ struct SPC_HOOFDVARIABELE hoofdVar) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetDimension( 
            /* [in] */ int dimensionNr,
            /* [out] */ enum DIMENSIONTYPE __RPC_FAR *pDimensionType,
            /* [out] */ struct SPC_INTERVAL_DIMENSION __RPC_FAR *pIntervalDimension,
            /* [out] */ struct SPC_CLASS_DIMENSION __RPC_FAR *pClassDimension) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddIntervalDimension( 
            /* [in] */ int dimensionNr,
            /* [in] */ struct SPC_INTERVAL_DIMENSION intervalDimension) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddClassDimension( 
            /* [in] */ int dimensionNr,
            /* [in] */ struct SPC_CLASS_DIMENSION classDimension) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditIntervalDimension( 
            /* [in] */ int dimensionNr,
            /* [in] */ struct SPC_INTERVAL_DIMENSION intervalDimension) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditClassDimension( 
            /* [in] */ int dimensionNr,
            /* [in] */ struct SPC_CLASS_DIMENSION classDimension) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveDimension( 
            /* [in] */ int dimensionNr) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetGebeurtenisList( 
            /* [out] */ struct SPC_GEBEURTENISINMODELLIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EditGebeurtenis( 
            /* [in] */ struct SPC_GEBEURTENISINMODEL gebeurtenis,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetKaartList( 
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pKaartConfigAsRWOrdered) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CreateKaart( 
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartCnfgBaseIn,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartCnfgBaseOut) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveKaart( 
            /* [in] */ int idKaart) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetHistoryEntries( 
            /* [out] */ UINT __RPC_FAR *pEntries) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCurrentIntersection( 
            /* [in] */ enum SPC_MODEL_ACCESSOR_TYPE type,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup,
            /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
            /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetHistoryIntersection( 
            /* [in] */ UINT pos,
            /* [in] */ enum SPC_MODEL_ACCESSOR_TYPE type,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup,
            /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
            /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCurrentHistogram( 
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ VARIANT __RPC_FAR *pXFitting,
            /* [out] */ VARIANT __RPC_FAR *pYFitting,
            /* [out] */ struct SPCHistographSetup __RPC_FAR *pSetup,
            /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
            /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetHistoryHistogram( 
            /* [in] */ UINT pos,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ VARIANT __RPC_FAR *pXFitting,
            /* [out] */ VARIANT __RPC_FAR *pYFitting,
            /* [out] */ struct SPCHistographSetup __RPC_FAR *pSetup,
            /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
            /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetMaxSelection( 
            /* [out] */ struct SPC_MODEL_SELECTION __RPC_FAR *pSelection) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetSelectionList( 
            /* [out] */ struct SPC_MODEL_SELECTION_LIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetSelectionList( 
            /* [in] */ struct SPC_MODEL_SELECTION_LIST list) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCapabilityTable( 
            /* [out] */ struct SPC_MODEL_CAPABILITY_TABLE __RPC_FAR *pTable) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetHistorySelectionList( 
            /* [in] */ UINT pos,
            /* [out] */ struct SPC_MODEL_SELECTION_LIST __RPC_FAR *pList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetHistoryCapabilityTable( 
            /* [in] */ UINT pos,
            /* [out] */ struct SPC_MODEL_CAPABILITY_TABLE __RPC_FAR *pTable) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetIsLearning( 
            /* [in] */ BOOL isLearning) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Clear( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Reset( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Open( 
            /* [in] */ BSTR fileName) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Save( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SaveAs( 
            /* [in] */ BSTR filename) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetModelConfiguration( 
            /* [out] */ BSTR __RPC_FAR *pModelConfig) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetBinaryModel( 
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pModel,
            /* [out] */ int __RPC_FAR *pModelLoadVersion) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ReplaceModel( 
            /* [in] */ struct SPC_BINARY_DATA model) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE BlockModelConfiguration( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE FreeModelConfiguration( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCurrentShiftXY( 
            /* [in] */ int EventId,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCurrentMultiplierXY( 
            /* [in] */ int EventId,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetHistoryShiftXY( 
            /* [in] */ UINT pos,
            /* [in] */ int EventId,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetHistoryMultiplierXY( 
            /* [in] */ UINT pos,
            /* [in] */ int EventId,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCurrentRfmXY( 
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCurrentRfsXY( 
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetHistoryRfmXY( 
            /* [in] */ UINT pos,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetHistoryRfsXY( 
            /* [in] */ UINT pos,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetConfiguration( 
            /* [in] */ struct SPC_BINARY_DATA arg_SPCModelConfiguration) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetModelData( 
            /* [in] */ struct SPC_BINARY_DATA arg_SPCModelData) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetConfiguration( 
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *arg_pSPCModelConfiguration) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetModelData( 
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *arg_pSPCModelData) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISPCModelVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISPCModel __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISPCModel __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetModelSettings )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ struct SPC_MODELSETTINGS setting);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetModelSettings )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ struct SPC_MODELSETTINGS __RPC_FAR *pSetting);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetModelSettingsEx )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ struct SPC_MODELSETTINGS_EX setting);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetModelSettingsEx )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ struct SPC_MODELSETTINGS_EX __RPC_FAR *pSetting);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetHoofdVariabele )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ struct SPC_HOOFDVARIABELE hoofdVar);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetHoofdVariabele )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ struct SPC_HOOFDVARIABELE __RPC_FAR *pHoofdVar);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditHoofdVariabele )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ struct SPC_HOOFDVARIABELE hoofdVar);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetDimension )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ int dimensionNr,
            /* [out] */ enum DIMENSIONTYPE __RPC_FAR *pDimensionType,
            /* [out] */ struct SPC_INTERVAL_DIMENSION __RPC_FAR *pIntervalDimension,
            /* [out] */ struct SPC_CLASS_DIMENSION __RPC_FAR *pClassDimension);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddIntervalDimension )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ int dimensionNr,
            /* [in] */ struct SPC_INTERVAL_DIMENSION intervalDimension);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddClassDimension )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ int dimensionNr,
            /* [in] */ struct SPC_CLASS_DIMENSION classDimension);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditIntervalDimension )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ int dimensionNr,
            /* [in] */ struct SPC_INTERVAL_DIMENSION intervalDimension);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditClassDimension )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ int dimensionNr,
            /* [in] */ struct SPC_CLASS_DIMENSION classDimension);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveDimension )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ int dimensionNr);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetGebeurtenisList )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ struct SPC_GEBEURTENISINMODELLIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *EditGebeurtenis )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ struct SPC_GEBEURTENISINMODEL gebeurtenis,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetKaartList )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pKaartConfigAsRWOrdered);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *CreateKaart )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartCnfgBaseIn,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartCnfgBaseOut);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveKaart )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ int idKaart);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetHistoryEntries )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ UINT __RPC_FAR *pEntries);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetCurrentIntersection )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ enum SPC_MODEL_ACCESSOR_TYPE type,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup,
            /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
            /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetHistoryIntersection )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ UINT pos,
            /* [in] */ enum SPC_MODEL_ACCESSOR_TYPE type,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup,
            /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
            /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetCurrentHistogram )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ VARIANT __RPC_FAR *pXFitting,
            /* [out] */ VARIANT __RPC_FAR *pYFitting,
            /* [out] */ struct SPCHistographSetup __RPC_FAR *pSetup,
            /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
            /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetHistoryHistogram )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ UINT pos,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ VARIANT __RPC_FAR *pXFitting,
            /* [out] */ VARIANT __RPC_FAR *pYFitting,
            /* [out] */ struct SPCHistographSetup __RPC_FAR *pSetup,
            /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
            /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetMaxSelection )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ struct SPC_MODEL_SELECTION __RPC_FAR *pSelection);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetSelectionList )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ struct SPC_MODEL_SELECTION_LIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetSelectionList )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ struct SPC_MODEL_SELECTION_LIST list);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetCapabilityTable )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ struct SPC_MODEL_CAPABILITY_TABLE __RPC_FAR *pTable);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetHistorySelectionList )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ UINT pos,
            /* [out] */ struct SPC_MODEL_SELECTION_LIST __RPC_FAR *pList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetHistoryCapabilityTable )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ UINT pos,
            /* [out] */ struct SPC_MODEL_CAPABILITY_TABLE __RPC_FAR *pTable);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetIsLearning )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ BOOL isLearning);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *Clear )( 
            ISPCModel __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *Reset )( 
            ISPCModel __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *Open )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ BSTR fileName);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *Save )( 
            ISPCModel __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SaveAs )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ BSTR filename);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetModelConfiguration )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ BSTR __RPC_FAR *pModelConfig);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetBinaryModel )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pModel,
            /* [out] */ int __RPC_FAR *pModelLoadVersion);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *ReplaceModel )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ struct SPC_BINARY_DATA model);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *BlockModelConfiguration )( 
            ISPCModel __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *FreeModelConfiguration )( 
            ISPCModel __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetCurrentShiftXY )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ int EventId,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetCurrentMultiplierXY )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ int EventId,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetHistoryShiftXY )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ UINT pos,
            /* [in] */ int EventId,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetHistoryMultiplierXY )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ UINT pos,
            /* [in] */ int EventId,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetCurrentRfmXY )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetCurrentRfsXY )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetHistoryRfmXY )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ UINT pos,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetHistoryRfsXY )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ UINT pos,
            /* [out] */ VARIANT __RPC_FAR *pXValues,
            /* [out] */ VARIANT __RPC_FAR *pYValues,
            /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetConfiguration )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ struct SPC_BINARY_DATA arg_SPCModelConfiguration);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetModelData )( 
            ISPCModel __RPC_FAR * This,
            /* [in] */ struct SPC_BINARY_DATA arg_SPCModelData);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetConfiguration )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *arg_pSPCModelConfiguration);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetModelData )( 
            ISPCModel __RPC_FAR * This,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *arg_pSPCModelData);
        
        END_INTERFACE
    } ISPCModelVtbl;

    interface ISPCModel
    {
        CONST_VTBL struct ISPCModelVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISPCModel_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISPCModel_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISPCModel_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISPCModel_SetModelSettings(This,setting)	\
    (This)->lpVtbl -> SetModelSettings(This,setting)

#define ISPCModel_GetModelSettings(This,pSetting)	\
    (This)->lpVtbl -> GetModelSettings(This,pSetting)

#define ISPCModel_SetModelSettingsEx(This,setting)	\
    (This)->lpVtbl -> SetModelSettingsEx(This,setting)

#define ISPCModel_GetModelSettingsEx(This,pSetting)	\
    (This)->lpVtbl -> GetModelSettingsEx(This,pSetting)

#define ISPCModel_SetHoofdVariabele(This,hoofdVar)	\
    (This)->lpVtbl -> SetHoofdVariabele(This,hoofdVar)

#define ISPCModel_GetHoofdVariabele(This,pHoofdVar)	\
    (This)->lpVtbl -> GetHoofdVariabele(This,pHoofdVar)

#define ISPCModel_EditHoofdVariabele(This,hoofdVar)	\
    (This)->lpVtbl -> EditHoofdVariabele(This,hoofdVar)

#define ISPCModel_GetDimension(This,dimensionNr,pDimensionType,pIntervalDimension,pClassDimension)	\
    (This)->lpVtbl -> GetDimension(This,dimensionNr,pDimensionType,pIntervalDimension,pClassDimension)

#define ISPCModel_AddIntervalDimension(This,dimensionNr,intervalDimension)	\
    (This)->lpVtbl -> AddIntervalDimension(This,dimensionNr,intervalDimension)

#define ISPCModel_AddClassDimension(This,dimensionNr,classDimension)	\
    (This)->lpVtbl -> AddClassDimension(This,dimensionNr,classDimension)

#define ISPCModel_EditIntervalDimension(This,dimensionNr,intervalDimension)	\
    (This)->lpVtbl -> EditIntervalDimension(This,dimensionNr,intervalDimension)

#define ISPCModel_EditClassDimension(This,dimensionNr,classDimension)	\
    (This)->lpVtbl -> EditClassDimension(This,dimensionNr,classDimension)

#define ISPCModel_RemoveDimension(This,dimensionNr)	\
    (This)->lpVtbl -> RemoveDimension(This,dimensionNr)

#define ISPCModel_GetGebeurtenisList(This,pList)	\
    (This)->lpVtbl -> GetGebeurtenisList(This,pList)

#define ISPCModel_EditGebeurtenis(This,gebeurtenis,pError)	\
    (This)->lpVtbl -> EditGebeurtenis(This,gebeurtenis,pError)

#define ISPCModel_GetKaartList(This,pKaartConfigAsRWOrdered)	\
    (This)->lpVtbl -> GetKaartList(This,pKaartConfigAsRWOrdered)

#define ISPCModel_CreateKaart(This,arg_GSPCKaartCnfgBaseIn,pArg_GSPCKaartCnfgBaseOut)	\
    (This)->lpVtbl -> CreateKaart(This,arg_GSPCKaartCnfgBaseIn,pArg_GSPCKaartCnfgBaseOut)

#define ISPCModel_RemoveKaart(This,idKaart)	\
    (This)->lpVtbl -> RemoveKaart(This,idKaart)

#define ISPCModel_GetHistoryEntries(This,pEntries)	\
    (This)->lpVtbl -> GetHistoryEntries(This,pEntries)

#define ISPCModel_GetCurrentIntersection(This,type,pXValues,pYValues,pSetup,dim,dimensionItems)	\
    (This)->lpVtbl -> GetCurrentIntersection(This,type,pXValues,pYValues,pSetup,dim,dimensionItems)

#define ISPCModel_GetHistoryIntersection(This,pos,type,pXValues,pYValues,pSetup,dim,dimensionItems)	\
    (This)->lpVtbl -> GetHistoryIntersection(This,pos,type,pXValues,pYValues,pSetup,dim,dimensionItems)

#define ISPCModel_GetCurrentHistogram(This,pXValues,pYValues,pXFitting,pYFitting,pSetup,dim,dimensionItems)	\
    (This)->lpVtbl -> GetCurrentHistogram(This,pXValues,pYValues,pXFitting,pYFitting,pSetup,dim,dimensionItems)

#define ISPCModel_GetHistoryHistogram(This,pos,pXValues,pYValues,pXFitting,pYFitting,pSetup,dim,dimensionItems)	\
    (This)->lpVtbl -> GetHistoryHistogram(This,pos,pXValues,pYValues,pXFitting,pYFitting,pSetup,dim,dimensionItems)

#define ISPCModel_GetMaxSelection(This,pSelection)	\
    (This)->lpVtbl -> GetMaxSelection(This,pSelection)

#define ISPCModel_GetSelectionList(This,pList)	\
    (This)->lpVtbl -> GetSelectionList(This,pList)

#define ISPCModel_SetSelectionList(This,list)	\
    (This)->lpVtbl -> SetSelectionList(This,list)

#define ISPCModel_GetCapabilityTable(This,pTable)	\
    (This)->lpVtbl -> GetCapabilityTable(This,pTable)

#define ISPCModel_GetHistorySelectionList(This,pos,pList)	\
    (This)->lpVtbl -> GetHistorySelectionList(This,pos,pList)

#define ISPCModel_GetHistoryCapabilityTable(This,pos,pTable)	\
    (This)->lpVtbl -> GetHistoryCapabilityTable(This,pos,pTable)

#define ISPCModel_SetIsLearning(This,isLearning)	\
    (This)->lpVtbl -> SetIsLearning(This,isLearning)

#define ISPCModel_Clear(This)	\
    (This)->lpVtbl -> Clear(This)

#define ISPCModel_Reset(This)	\
    (This)->lpVtbl -> Reset(This)

#define ISPCModel_Open(This,fileName)	\
    (This)->lpVtbl -> Open(This,fileName)

#define ISPCModel_Save(This)	\
    (This)->lpVtbl -> Save(This)

#define ISPCModel_SaveAs(This,filename)	\
    (This)->lpVtbl -> SaveAs(This,filename)

#define ISPCModel_GetModelConfiguration(This,pModelConfig)	\
    (This)->lpVtbl -> GetModelConfiguration(This,pModelConfig)

#define ISPCModel_GetBinaryModel(This,pModel,pModelLoadVersion)	\
    (This)->lpVtbl -> GetBinaryModel(This,pModel,pModelLoadVersion)

#define ISPCModel_ReplaceModel(This,model)	\
    (This)->lpVtbl -> ReplaceModel(This,model)

#define ISPCModel_BlockModelConfiguration(This)	\
    (This)->lpVtbl -> BlockModelConfiguration(This)

#define ISPCModel_FreeModelConfiguration(This)	\
    (This)->lpVtbl -> FreeModelConfiguration(This)

#define ISPCModel_GetCurrentShiftXY(This,EventId,pXValues,pYValues,pSetup)	\
    (This)->lpVtbl -> GetCurrentShiftXY(This,EventId,pXValues,pYValues,pSetup)

#define ISPCModel_GetCurrentMultiplierXY(This,EventId,pXValues,pYValues,pSetup)	\
    (This)->lpVtbl -> GetCurrentMultiplierXY(This,EventId,pXValues,pYValues,pSetup)

#define ISPCModel_GetHistoryShiftXY(This,pos,EventId,pXValues,pYValues,pSetup)	\
    (This)->lpVtbl -> GetHistoryShiftXY(This,pos,EventId,pXValues,pYValues,pSetup)

#define ISPCModel_GetHistoryMultiplierXY(This,pos,EventId,pXValues,pYValues,pSetup)	\
    (This)->lpVtbl -> GetHistoryMultiplierXY(This,pos,EventId,pXValues,pYValues,pSetup)

#define ISPCModel_GetCurrentRfmXY(This,pXValues,pYValues,pSetup)	\
    (This)->lpVtbl -> GetCurrentRfmXY(This,pXValues,pYValues,pSetup)

#define ISPCModel_GetCurrentRfsXY(This,pXValues,pYValues,pSetup)	\
    (This)->lpVtbl -> GetCurrentRfsXY(This,pXValues,pYValues,pSetup)

#define ISPCModel_GetHistoryRfmXY(This,pos,pXValues,pYValues,pSetup)	\
    (This)->lpVtbl -> GetHistoryRfmXY(This,pos,pXValues,pYValues,pSetup)

#define ISPCModel_GetHistoryRfsXY(This,pos,pXValues,pYValues,pSetup)	\
    (This)->lpVtbl -> GetHistoryRfsXY(This,pos,pXValues,pYValues,pSetup)

#define ISPCModel_SetConfiguration(This,arg_SPCModelConfiguration)	\
    (This)->lpVtbl -> SetConfiguration(This,arg_SPCModelConfiguration)

#define ISPCModel_SetModelData(This,arg_SPCModelData)	\
    (This)->lpVtbl -> SetModelData(This,arg_SPCModelData)

#define ISPCModel_GetConfiguration(This,arg_pSPCModelConfiguration)	\
    (This)->lpVtbl -> GetConfiguration(This,arg_pSPCModelConfiguration)

#define ISPCModel_GetModelData(This,arg_pSPCModelData)	\
    (This)->lpVtbl -> GetModelData(This,arg_pSPCModelData)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE ISPCModel_SetModelSettings_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ struct SPC_MODELSETTINGS setting);


void __RPC_STUB ISPCModel_SetModelSettings_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetModelSettings_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ struct SPC_MODELSETTINGS __RPC_FAR *pSetting);


void __RPC_STUB ISPCModel_GetModelSettings_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_SetModelSettingsEx_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ struct SPC_MODELSETTINGS_EX setting);


void __RPC_STUB ISPCModel_SetModelSettingsEx_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetModelSettingsEx_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ struct SPC_MODELSETTINGS_EX __RPC_FAR *pSetting);


void __RPC_STUB ISPCModel_GetModelSettingsEx_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_SetHoofdVariabele_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ struct SPC_HOOFDVARIABELE hoofdVar);


void __RPC_STUB ISPCModel_SetHoofdVariabele_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetHoofdVariabele_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ struct SPC_HOOFDVARIABELE __RPC_FAR *pHoofdVar);


void __RPC_STUB ISPCModel_GetHoofdVariabele_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_EditHoofdVariabele_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ struct SPC_HOOFDVARIABELE hoofdVar);


void __RPC_STUB ISPCModel_EditHoofdVariabele_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetDimension_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ int dimensionNr,
    /* [out] */ enum DIMENSIONTYPE __RPC_FAR *pDimensionType,
    /* [out] */ struct SPC_INTERVAL_DIMENSION __RPC_FAR *pIntervalDimension,
    /* [out] */ struct SPC_CLASS_DIMENSION __RPC_FAR *pClassDimension);


void __RPC_STUB ISPCModel_GetDimension_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_AddIntervalDimension_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ int dimensionNr,
    /* [in] */ struct SPC_INTERVAL_DIMENSION intervalDimension);


void __RPC_STUB ISPCModel_AddIntervalDimension_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_AddClassDimension_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ int dimensionNr,
    /* [in] */ struct SPC_CLASS_DIMENSION classDimension);


void __RPC_STUB ISPCModel_AddClassDimension_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_EditIntervalDimension_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ int dimensionNr,
    /* [in] */ struct SPC_INTERVAL_DIMENSION intervalDimension);


void __RPC_STUB ISPCModel_EditIntervalDimension_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_EditClassDimension_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ int dimensionNr,
    /* [in] */ struct SPC_CLASS_DIMENSION classDimension);


void __RPC_STUB ISPCModel_EditClassDimension_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_RemoveDimension_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ int dimensionNr);


void __RPC_STUB ISPCModel_RemoveDimension_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetGebeurtenisList_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ struct SPC_GEBEURTENISINMODELLIST __RPC_FAR *pList);


void __RPC_STUB ISPCModel_GetGebeurtenisList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_EditGebeurtenis_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ struct SPC_GEBEURTENISINMODEL gebeurtenis,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCModel_EditGebeurtenis_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetKaartList_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pKaartConfigAsRWOrdered);


void __RPC_STUB ISPCModel_GetKaartList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_CreateKaart_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartCnfgBaseIn,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartCnfgBaseOut);


void __RPC_STUB ISPCModel_CreateKaart_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_RemoveKaart_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ int idKaart);


void __RPC_STUB ISPCModel_RemoveKaart_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetHistoryEntries_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ UINT __RPC_FAR *pEntries);


void __RPC_STUB ISPCModel_GetHistoryEntries_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetCurrentIntersection_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ enum SPC_MODEL_ACCESSOR_TYPE type,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup,
    /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
    /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]);


void __RPC_STUB ISPCModel_GetCurrentIntersection_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetHistoryIntersection_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ UINT pos,
    /* [in] */ enum SPC_MODEL_ACCESSOR_TYPE type,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup,
    /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
    /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]);


void __RPC_STUB ISPCModel_GetHistoryIntersection_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetCurrentHistogram_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ VARIANT __RPC_FAR *pXFitting,
    /* [out] */ VARIANT __RPC_FAR *pYFitting,
    /* [out] */ struct SPCHistographSetup __RPC_FAR *pSetup,
    /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
    /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]);


void __RPC_STUB ISPCModel_GetCurrentHistogram_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetHistoryHistogram_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ UINT pos,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ VARIANT __RPC_FAR *pXFitting,
    /* [out] */ VARIANT __RPC_FAR *pYFitting,
    /* [out] */ struct SPCHistographSetup __RPC_FAR *pSetup,
    /* [length_is][out][in] */ unsigned int __RPC_FAR dim[ 4 ],
    /* [length_is][out] */ struct SPC_DIMENSION_ITEMS __RPC_FAR dimensionItems[ 4 ]);


void __RPC_STUB ISPCModel_GetHistoryHistogram_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetMaxSelection_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ struct SPC_MODEL_SELECTION __RPC_FAR *pSelection);


void __RPC_STUB ISPCModel_GetMaxSelection_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetSelectionList_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ struct SPC_MODEL_SELECTION_LIST __RPC_FAR *pList);


void __RPC_STUB ISPCModel_GetSelectionList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_SetSelectionList_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ struct SPC_MODEL_SELECTION_LIST list);


void __RPC_STUB ISPCModel_SetSelectionList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetCapabilityTable_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ struct SPC_MODEL_CAPABILITY_TABLE __RPC_FAR *pTable);


void __RPC_STUB ISPCModel_GetCapabilityTable_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetHistorySelectionList_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ UINT pos,
    /* [out] */ struct SPC_MODEL_SELECTION_LIST __RPC_FAR *pList);


void __RPC_STUB ISPCModel_GetHistorySelectionList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetHistoryCapabilityTable_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ UINT pos,
    /* [out] */ struct SPC_MODEL_CAPABILITY_TABLE __RPC_FAR *pTable);


void __RPC_STUB ISPCModel_GetHistoryCapabilityTable_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_SetIsLearning_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ BOOL isLearning);


void __RPC_STUB ISPCModel_SetIsLearning_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_Clear_Proxy( 
    ISPCModel __RPC_FAR * This);


void __RPC_STUB ISPCModel_Clear_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_Reset_Proxy( 
    ISPCModel __RPC_FAR * This);


void __RPC_STUB ISPCModel_Reset_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_Open_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ BSTR fileName);


void __RPC_STUB ISPCModel_Open_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_Save_Proxy( 
    ISPCModel __RPC_FAR * This);


void __RPC_STUB ISPCModel_Save_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_SaveAs_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ BSTR filename);


void __RPC_STUB ISPCModel_SaveAs_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetModelConfiguration_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ BSTR __RPC_FAR *pModelConfig);


void __RPC_STUB ISPCModel_GetModelConfiguration_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetBinaryModel_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pModel,
    /* [out] */ int __RPC_FAR *pModelLoadVersion);


void __RPC_STUB ISPCModel_GetBinaryModel_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_ReplaceModel_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ struct SPC_BINARY_DATA model);


void __RPC_STUB ISPCModel_ReplaceModel_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_BlockModelConfiguration_Proxy( 
    ISPCModel __RPC_FAR * This);


void __RPC_STUB ISPCModel_BlockModelConfiguration_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_FreeModelConfiguration_Proxy( 
    ISPCModel __RPC_FAR * This);


void __RPC_STUB ISPCModel_FreeModelConfiguration_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetCurrentShiftXY_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ int EventId,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);


void __RPC_STUB ISPCModel_GetCurrentShiftXY_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetCurrentMultiplierXY_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ int EventId,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);


void __RPC_STUB ISPCModel_GetCurrentMultiplierXY_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetHistoryShiftXY_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ UINT pos,
    /* [in] */ int EventId,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);


void __RPC_STUB ISPCModel_GetHistoryShiftXY_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetHistoryMultiplierXY_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ UINT pos,
    /* [in] */ int EventId,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);


void __RPC_STUB ISPCModel_GetHistoryMultiplierXY_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetCurrentRfmXY_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);


void __RPC_STUB ISPCModel_GetCurrentRfmXY_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetCurrentRfsXY_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);


void __RPC_STUB ISPCModel_GetCurrentRfsXY_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetHistoryRfmXY_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ UINT pos,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);


void __RPC_STUB ISPCModel_GetHistoryRfmXY_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetHistoryRfsXY_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ UINT pos,
    /* [out] */ VARIANT __RPC_FAR *pXValues,
    /* [out] */ VARIANT __RPC_FAR *pYValues,
    /* [out] */ struct SPCXYSetup __RPC_FAR *pSetup);


void __RPC_STUB ISPCModel_GetHistoryRfsXY_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_SetConfiguration_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ struct SPC_BINARY_DATA arg_SPCModelConfiguration);


void __RPC_STUB ISPCModel_SetConfiguration_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_SetModelData_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [in] */ struct SPC_BINARY_DATA arg_SPCModelData);


void __RPC_STUB ISPCModel_SetModelData_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetConfiguration_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *arg_pSPCModelConfiguration);


void __RPC_STUB ISPCModel_GetConfiguration_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCModel_GetModelData_Proxy( 
    ISPCModel __RPC_FAR * This,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *arg_pSPCModelData);


void __RPC_STUB ISPCModel_GetModelData_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISPCModel_INTERFACE_DEFINED__ */


#ifndef __ISPCOverzicht_INTERFACE_DEFINED__
#define __ISPCOverzicht_INTERFACE_DEFINED__

/* interface ISPCOverzicht */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_ISPCOverzicht;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("F6783767-E3B2-11D0-9110-AA0004007F05")
    ISPCOverzicht : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetKaartList( 
            /* [out] */ struct SPC_KAARTLIST __RPC_FAR *pKaartList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddKaart( 
            /* [in] */ struct SPC_KAARTINFO kaartInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveKaart( 
            /* [in] */ struct SPC_KAARTINFO kaartInfo,
            /* [out] */ BSTR __RPC_FAR *pError) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetOverzichtKaartList( 
            /* [out] */ struct SPC_KAART_IN_OVERZICHTLIST __RPC_FAR *pKaartList) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddOverzichtKaart( 
            /* [in] */ struct SPC_KAART_IN_OVERZICHT kaartInfo) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RemoveOverzichtKaart( 
            /* [in] */ struct SPC_KAART_IN_OVERZICHT kaartInfo) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISPCOverzichtVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISPCOverzicht __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISPCOverzicht __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISPCOverzicht __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetKaartList )( 
            ISPCOverzicht __RPC_FAR * This,
            /* [out] */ struct SPC_KAARTLIST __RPC_FAR *pKaartList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddKaart )( 
            ISPCOverzicht __RPC_FAR * This,
            /* [in] */ struct SPC_KAARTINFO kaartInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveKaart )( 
            ISPCOverzicht __RPC_FAR * This,
            /* [in] */ struct SPC_KAARTINFO kaartInfo,
            /* [out] */ BSTR __RPC_FAR *pError);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetOverzichtKaartList )( 
            ISPCOverzicht __RPC_FAR * This,
            /* [out] */ struct SPC_KAART_IN_OVERZICHTLIST __RPC_FAR *pKaartList);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AddOverzichtKaart )( 
            ISPCOverzicht __RPC_FAR * This,
            /* [in] */ struct SPC_KAART_IN_OVERZICHT kaartInfo);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoveOverzichtKaart )( 
            ISPCOverzicht __RPC_FAR * This,
            /* [in] */ struct SPC_KAART_IN_OVERZICHT kaartInfo);
        
        END_INTERFACE
    } ISPCOverzichtVtbl;

    interface ISPCOverzicht
    {
        CONST_VTBL struct ISPCOverzichtVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISPCOverzicht_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISPCOverzicht_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISPCOverzicht_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISPCOverzicht_GetKaartList(This,pKaartList)	\
    (This)->lpVtbl -> GetKaartList(This,pKaartList)

#define ISPCOverzicht_AddKaart(This,kaartInfo,pError)	\
    (This)->lpVtbl -> AddKaart(This,kaartInfo,pError)

#define ISPCOverzicht_RemoveKaart(This,kaartInfo,pError)	\
    (This)->lpVtbl -> RemoveKaart(This,kaartInfo,pError)

#define ISPCOverzicht_GetOverzichtKaartList(This,pKaartList)	\
    (This)->lpVtbl -> GetOverzichtKaartList(This,pKaartList)

#define ISPCOverzicht_AddOverzichtKaart(This,kaartInfo)	\
    (This)->lpVtbl -> AddOverzichtKaart(This,kaartInfo)

#define ISPCOverzicht_RemoveOverzichtKaart(This,kaartInfo)	\
    (This)->lpVtbl -> RemoveOverzichtKaart(This,kaartInfo)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE ISPCOverzicht_GetKaartList_Proxy( 
    ISPCOverzicht __RPC_FAR * This,
    /* [out] */ struct SPC_KAARTLIST __RPC_FAR *pKaartList);


void __RPC_STUB ISPCOverzicht_GetKaartList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOverzicht_AddKaart_Proxy( 
    ISPCOverzicht __RPC_FAR * This,
    /* [in] */ struct SPC_KAARTINFO kaartInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOverzicht_AddKaart_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOverzicht_RemoveKaart_Proxy( 
    ISPCOverzicht __RPC_FAR * This,
    /* [in] */ struct SPC_KAARTINFO kaartInfo,
    /* [out] */ BSTR __RPC_FAR *pError);


void __RPC_STUB ISPCOverzicht_RemoveKaart_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOverzicht_GetOverzichtKaartList_Proxy( 
    ISPCOverzicht __RPC_FAR * This,
    /* [out] */ struct SPC_KAART_IN_OVERZICHTLIST __RPC_FAR *pKaartList);


void __RPC_STUB ISPCOverzicht_GetOverzichtKaartList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOverzicht_AddOverzichtKaart_Proxy( 
    ISPCOverzicht __RPC_FAR * This,
    /* [in] */ struct SPC_KAART_IN_OVERZICHT kaartInfo);


void __RPC_STUB ISPCOverzicht_AddOverzichtKaart_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCOverzicht_RemoveOverzichtKaart_Proxy( 
    ISPCOverzicht __RPC_FAR * This,
    /* [in] */ struct SPC_KAART_IN_OVERZICHT kaartInfo);


void __RPC_STUB ISPCOverzicht_RemoveOverzichtKaart_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISPCOverzicht_INTERFACE_DEFINED__ */


#ifndef __ISPCKaart_INTERFACE_DEFINED__
#define __ISPCKaart_INTERFACE_DEFINED__

/* interface ISPCKaart */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_ISPCKaart;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("F6783769-E3B2-11D0-9110-AA0004007F05")
    ISPCKaart : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE ConnectTo( 
            /* [in] */ int kaartId,
            /* [in] */ BSTR computername,
            /* [in] */ BSTR username) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetKaartConfiguratie( 
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartConfiguratieBas) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetKaartConfiguratie( 
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartConfiguratieBas) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCurrentSequenceNumber( 
            /* [out] */ unsigned long __RPC_FAR *pSequence) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetKaartHistory( 
            /* [out] */ struct SPC_KAART_BINARY_DATA __RPC_FAR *pKaartBinData) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetKaartHistoryLast( 
            /* [in] */ unsigned long aantal,
            /* [out] */ struct SPC_KAART_BINARY_DATA __RPC_FAR *pKaartBinData,
            /* [out] */ double __RPC_FAR *pServerTimeStamp) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetHistoryEntries( 
            /* [out] */ UINT __RPC_FAR *pEntries) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SaveExternalConnectionInfo( 
            /* [in] */ BSTR linkName,
            /* [in] */ struct SPC_BINARY_DATA binData) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE LoadExternalConnectionInfo( 
            /* [in] */ BSTR linkName,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pBinData) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE PreCalculateManualData( 
            /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList,
            /* [out] */ struct SPC_MANUELE_PRECALCULATED_DATALIST __RPC_FAR *pCalculatedData) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISPCKaartVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISPCKaart __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISPCKaart __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISPCKaart __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *ConnectTo )( 
            ISPCKaart __RPC_FAR * This,
            /* [in] */ int kaartId,
            /* [in] */ BSTR computername,
            /* [in] */ BSTR username);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetKaartConfiguratie )( 
            ISPCKaart __RPC_FAR * This,
            /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartConfiguratieBas);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetKaartConfiguratie )( 
            ISPCKaart __RPC_FAR * This,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartConfiguratieBas);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetCurrentSequenceNumber )( 
            ISPCKaart __RPC_FAR * This,
            /* [out] */ unsigned long __RPC_FAR *pSequence);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetKaartHistory )( 
            ISPCKaart __RPC_FAR * This,
            /* [out] */ struct SPC_KAART_BINARY_DATA __RPC_FAR *pKaartBinData);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetKaartHistoryLast )( 
            ISPCKaart __RPC_FAR * This,
            /* [in] */ unsigned long aantal,
            /* [out] */ struct SPC_KAART_BINARY_DATA __RPC_FAR *pKaartBinData,
            /* [out] */ double __RPC_FAR *pServerTimeStamp);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetHistoryEntries )( 
            ISPCKaart __RPC_FAR * This,
            /* [out] */ UINT __RPC_FAR *pEntries);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SaveExternalConnectionInfo )( 
            ISPCKaart __RPC_FAR * This,
            /* [in] */ BSTR linkName,
            /* [in] */ struct SPC_BINARY_DATA binData);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *LoadExternalConnectionInfo )( 
            ISPCKaart __RPC_FAR * This,
            /* [in] */ BSTR linkName,
            /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pBinData);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *PreCalculateManualData )( 
            ISPCKaart __RPC_FAR * This,
            /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList,
            /* [out] */ struct SPC_MANUELE_PRECALCULATED_DATALIST __RPC_FAR *pCalculatedData);
        
        END_INTERFACE
    } ISPCKaartVtbl;

    interface ISPCKaart
    {
        CONST_VTBL struct ISPCKaartVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISPCKaart_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISPCKaart_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISPCKaart_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISPCKaart_ConnectTo(This,kaartId,computername,username)	\
    (This)->lpVtbl -> ConnectTo(This,kaartId,computername,username)

#define ISPCKaart_SetKaartConfiguratie(This,arg_GSPCKaartConfiguratieBas)	\
    (This)->lpVtbl -> SetKaartConfiguratie(This,arg_GSPCKaartConfiguratieBas)

#define ISPCKaart_GetKaartConfiguratie(This,pArg_GSPCKaartConfiguratieBas)	\
    (This)->lpVtbl -> GetKaartConfiguratie(This,pArg_GSPCKaartConfiguratieBas)

#define ISPCKaart_GetCurrentSequenceNumber(This,pSequence)	\
    (This)->lpVtbl -> GetCurrentSequenceNumber(This,pSequence)

#define ISPCKaart_GetKaartHistory(This,pKaartBinData)	\
    (This)->lpVtbl -> GetKaartHistory(This,pKaartBinData)

#define ISPCKaart_GetKaartHistoryLast(This,aantal,pKaartBinData,pServerTimeStamp)	\
    (This)->lpVtbl -> GetKaartHistoryLast(This,aantal,pKaartBinData,pServerTimeStamp)

#define ISPCKaart_GetHistoryEntries(This,pEntries)	\
    (This)->lpVtbl -> GetHistoryEntries(This,pEntries)

#define ISPCKaart_SaveExternalConnectionInfo(This,linkName,binData)	\
    (This)->lpVtbl -> SaveExternalConnectionInfo(This,linkName,binData)

#define ISPCKaart_LoadExternalConnectionInfo(This,linkName,pBinData)	\
    (This)->lpVtbl -> LoadExternalConnectionInfo(This,linkName,pBinData)

#define ISPCKaart_PreCalculateManualData(This,recordList,pCalculatedData)	\
    (This)->lpVtbl -> PreCalculateManualData(This,recordList,pCalculatedData)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE ISPCKaart_ConnectTo_Proxy( 
    ISPCKaart __RPC_FAR * This,
    /* [in] */ int kaartId,
    /* [in] */ BSTR computername,
    /* [in] */ BSTR username);


void __RPC_STUB ISPCKaart_ConnectTo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCKaart_SetKaartConfiguratie_Proxy( 
    ISPCKaart __RPC_FAR * This,
    /* [in] */ struct SPC_BINARY_DATA arg_GSPCKaartConfiguratieBas);


void __RPC_STUB ISPCKaart_SetKaartConfiguratie_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCKaart_GetKaartConfiguratie_Proxy( 
    ISPCKaart __RPC_FAR * This,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pArg_GSPCKaartConfiguratieBas);


void __RPC_STUB ISPCKaart_GetKaartConfiguratie_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCKaart_GetCurrentSequenceNumber_Proxy( 
    ISPCKaart __RPC_FAR * This,
    /* [out] */ unsigned long __RPC_FAR *pSequence);


void __RPC_STUB ISPCKaart_GetCurrentSequenceNumber_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCKaart_GetKaartHistory_Proxy( 
    ISPCKaart __RPC_FAR * This,
    /* [out] */ struct SPC_KAART_BINARY_DATA __RPC_FAR *pKaartBinData);


void __RPC_STUB ISPCKaart_GetKaartHistory_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCKaart_GetKaartHistoryLast_Proxy( 
    ISPCKaart __RPC_FAR * This,
    /* [in] */ unsigned long aantal,
    /* [out] */ struct SPC_KAART_BINARY_DATA __RPC_FAR *pKaartBinData,
    /* [out] */ double __RPC_FAR *pServerTimeStamp);


void __RPC_STUB ISPCKaart_GetKaartHistoryLast_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCKaart_GetHistoryEntries_Proxy( 
    ISPCKaart __RPC_FAR * This,
    /* [out] */ UINT __RPC_FAR *pEntries);


void __RPC_STUB ISPCKaart_GetHistoryEntries_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCKaart_SaveExternalConnectionInfo_Proxy( 
    ISPCKaart __RPC_FAR * This,
    /* [in] */ BSTR linkName,
    /* [in] */ struct SPC_BINARY_DATA binData);


void __RPC_STUB ISPCKaart_SaveExternalConnectionInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCKaart_LoadExternalConnectionInfo_Proxy( 
    ISPCKaart __RPC_FAR * This,
    /* [in] */ BSTR linkName,
    /* [out] */ struct SPC_BINARY_DATA __RPC_FAR *pBinData);


void __RPC_STUB ISPCKaart_LoadExternalConnectionInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE ISPCKaart_PreCalculateManualData_Proxy( 
    ISPCKaart __RPC_FAR * This,
    /* [in] */ struct SPC_MANUELE_DATA_RECORDLIST recordList,
    /* [out] */ struct SPC_MANUELE_PRECALCULATED_DATALIST __RPC_FAR *pCalculatedData);


void __RPC_STUB ISPCKaart_PreCalculateManualData_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISPCKaart_INTERFACE_DEFINED__ */


#ifndef __ISPCKaartPolling_INTERFACE_DEFINED__
#define __ISPCKaartPolling_INTERFACE_DEFINED__

/* interface ISPCKaartPolling */
/* [object][unique][helpstring][uuid] */ 


EXTERN_C const IID IID_ISPCKaartPolling;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("B6141561-7FD1-11d4-9193-0050DA5E4B9B")
    ISPCKaartPolling : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetPollingData( 
            /* [in] */ int lastClientChartSeqNr,
            /* [out][in] */ double __RPC_FAR *pServerTimeStamp,
            /* [out] */ struct SPC_KAART_POLLING __RPC_FAR *pData) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISPCKaartPollingVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISPCKaartPolling __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISPCKaartPolling __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISPCKaartPolling __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetPollingData )( 
            ISPCKaartPolling __RPC_FAR * This,
            /* [in] */ int lastClientChartSeqNr,
            /* [out][in] */ double __RPC_FAR *pServerTimeStamp,
            /* [out] */ struct SPC_KAART_POLLING __RPC_FAR *pData);
        
        END_INTERFACE
    } ISPCKaartPollingVtbl;

    interface ISPCKaartPolling
    {
        CONST_VTBL struct ISPCKaartPollingVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISPCKaartPolling_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISPCKaartPolling_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISPCKaartPolling_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISPCKaartPolling_GetPollingData(This,lastClientChartSeqNr,pServerTimeStamp,pData)	\
    (This)->lpVtbl -> GetPollingData(This,lastClientChartSeqNr,pServerTimeStamp,pData)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE ISPCKaartPolling_GetPollingData_Proxy( 
    ISPCKaartPolling __RPC_FAR * This,
    /* [in] */ int lastClientChartSeqNr,
    /* [out][in] */ double __RPC_FAR *pServerTimeStamp,
    /* [out] */ struct SPC_KAART_POLLING __RPC_FAR *pData);


void __RPC_STUB ISPCKaartPolling_GetPollingData_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISPCKaartPolling_INTERFACE_DEFINED__ */



#ifndef __SPCSERVERLib_LIBRARY_DEFINED__
#define __SPCSERVERLib_LIBRARY_DEFINED__

/* library SPCSERVERLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_SPCSERVERLib;

EXTERN_C const CLSID CLSID_SPCProcesManager;

#ifdef __cplusplus

class DECLSPEC_UUID("E9D7FCC1-D663-11D0-90FE-AA0004007F05")
SPCProcesManager;
#endif

EXTERN_C const CLSID CLSID_SPCProces;

#ifdef __cplusplus

class DECLSPEC_UUID("B3233136-D72B-11D0-9103-AA0004007F05")
SPCProces;
#endif

EXTERN_C const CLSID CLSID_SPCToepassing;

#ifdef __cplusplus

class DECLSPEC_UUID("F6783764-E3B2-11D0-9110-AA0004007F05")
SPCToepassing;
#endif

EXTERN_C const CLSID CLSID_SPCOnderwerp;

#ifdef __cplusplus

class DECLSPEC_UUID("F6783766-E3B2-11D0-9110-AA0004007F05")
SPCOnderwerp;
#endif

EXTERN_C const CLSID CLSID_SPCModel;

#ifdef __cplusplus

class DECLSPEC_UUID("8BF56DEF-3BE3-11D1-991F-00C04FB9742F")
SPCModel;
#endif

EXTERN_C const CLSID CLSID_SPCOverzicht;

#ifdef __cplusplus

class DECLSPEC_UUID("F6783768-E3B2-11D0-9110-AA0004007F05")
SPCOverzicht;
#endif

EXTERN_C const CLSID CLSID_SPCKaart;

#ifdef __cplusplus

class DECLSPEC_UUID("F678376A-E3B2-11D0-9110-AA0004007F05")
SPCKaart;
#endif
#endif /* __SPCSERVERLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long __RPC_FAR *, unsigned long            , BSTR __RPC_FAR * ); 
unsigned char __RPC_FAR * __RPC_USER  BSTR_UserMarshal(  unsigned long __RPC_FAR *, unsigned char __RPC_FAR *, BSTR __RPC_FAR * ); 
unsigned char __RPC_FAR * __RPC_USER  BSTR_UserUnmarshal(unsigned long __RPC_FAR *, unsigned char __RPC_FAR *, BSTR __RPC_FAR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long __RPC_FAR *, BSTR __RPC_FAR * ); 

unsigned long             __RPC_USER  VARIANT_UserSize(     unsigned long __RPC_FAR *, unsigned long            , VARIANT __RPC_FAR * ); 
unsigned char __RPC_FAR * __RPC_USER  VARIANT_UserMarshal(  unsigned long __RPC_FAR *, unsigned char __RPC_FAR *, VARIANT __RPC_FAR * ); 
unsigned char __RPC_FAR * __RPC_USER  VARIANT_UserUnmarshal(unsigned long __RPC_FAR *, unsigned char __RPC_FAR *, VARIANT __RPC_FAR * ); 
void                      __RPC_USER  VARIANT_UserFree(     unsigned long __RPC_FAR *, VARIANT __RPC_FAR * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif
