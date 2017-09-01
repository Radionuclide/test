; Script generated by the HM NIS Edit Script Wizard.

;--------------------------------
;Variables

Var ServiceStatus
Var PluginPath
Var ibaDatCoordinatorPath
Var StandAlone

;--------------------------------
;General Interface settings

!define MUI_ICON    "Graphics\ibaInstall.ico"
!define MUI_UNICON  "Graphics\ibaUnInstall.ico"

!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "Graphics\MUIInstallLogo.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP "Graphics\ibaWizard.bmp"

!define MUI_FINISHPAGE_TEXT $(FINISH_TEXT)
!define MUI_FINISHPAGE_NOAUTOCLOSE

!define DATCO_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\ibaDatCoordinator"
!define DATCO_UNINST_ROOT_KEY "HKLM"

; Include files
!include "MUI.nsh"
!include "LogicLib.nsh"
!include "StrFunc.nsh"
!include "FileFunc.nsh"

!addplugindir .

SetCompressor /SOLID lzma

; MUI Settings
!define MUI_ABORTWARNING

; checks, sets vars
Page custom PreInstall
; Welcome page
!define MUI_WELCOMEPAGE_TITLE_3LINES
!insertmacro MUI_PAGE_WELCOME
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!define MUI_FINISHPAGE_TITLE_3LINES
!insertmacro MUI_PAGE_FINISH

; Language files
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "German"

; Reserve files
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS

; Function declarations
!insertmacro GetParent
${StrLoc}

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "ibaDatCoordinator-${PRODUCT_SHORTNAME}-PluginInstaller_v${PRODUCT_VERSION}.exe"
InstallDir "$PROGRAMFILES\iba\ibaDatCoordinator\plugins"
ShowInstDetails show
BrandingText "iba AG"


VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "${PRODUCT_NAME}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments" ""
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "${PRODUCT_PUBLISHER}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalTrademarks" ""
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "� iba AG. All rights reserved"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "${PRODUCT_NAME} installer"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "${PRODUCT_FILE_VERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductVersion" "${PRODUCT_VERSION}"
VIProductVersion "${PRODUCT_FILE_VERSION}"

;--------------------------------
; Initialization functions
Function .onInit
  SetShellVarContext all
    ;In case of a silent install call the PreInstall function directly
  IfSilent +1 +2
    Call PreInstall
FunctionEnd
  
Section "MainSection" SEC01

  DetailPrint "Path to plugins is : $PluginPath"
 
  ${If} $StandAlone == "1"
    Call CloseStandAlone
  ${Else}
    Call StopService
  ${Endif}  
  
    Call CopyFiles
    Pop $0
  
  ${If} $0 == "error"
    MessageBox MB_ICONSTOP $(FAILED_COPY)
    Abort
  ${EndIf}

  ${If} $StandAlone == "1"
    nsSCMEx::RunAsNonElevatedUser /NOUNLOAD "$INSTDIR\..\ibaDatCoordinator.exe" '"$INSTDIR\..\ibaDatCoordinator.exe"' "$INSTDIR"
  ${Else}
    nsSCMEx::Start /NOUNLOAD "ibaDatCoordinatorService"
  ${Endif}  
SectionEnd

Section -Post
SectionEnd

;--------------------------------
; PreInstall function that checks requirements and previous installs
; Remark : This is done in separate "dummy" page because initialization functions
;          don't support translated error messages

Function PreInstall
  ;Check version of installed ibaDatco
  ReadRegStr $0 ${DATCO_UNINST_ROOT_KEY} "${DATCO_UNINST_KEY}" "DisplayVersion"
  ${If} $0 != ""
    DetailPrint "ibaDatCoordinator $0 is installed"
    
    ;Retrieve version number from string
    Push $0
    Call GetVersionNr
    Pop $R0
    IntCmp $R0 2000000 okVersion oldVersion okVersion

    oldVersion:
    MessageBox MB_ICONSTOP $(UPGRADE_REQUIRED)
    Abort

    okVersion:

  ${Else}
    MessageBox MB_ICONSTOP $(NO_DATCO) 
    Abort
  ${EndIf}

  StrCpy $StandAlone "0"
  ClearErrors
  ReadRegStr $0 HKLM "System\Currentcontrolset\services\ibaDatcoordinatorService" "imagepath"
  ${If} $0 == ""

	  ClearErrors
	  ReadRegStr $ibaDatCoordinatorPath "${DATCO_UNINST_ROOT_KEY}" "${DATCO_UNINST_KEY}" "InstallDir"
	  IfErrors nodatcoordinator coordinatorpresent

	nodatcoordinator:
	  MessageBox MB_ICONSTOP $(NO_DATCO_FOUND)
	  Abort
	  
	coordinatorpresent:
	 ; could also be client: 
	 ReadRegStr $1 "${DATCO_UNINST_ROOT_KEY}" "${DATCO_UNINST_KEY}" "Server"
	 ${If} $1 == "2"
	   MessageBox MB_ICONSTOP $(DATCO_CLIENTONLY) 
	   Abort
	 ${EndIf}
	 
	 StrCpy $ibaDatCoordinatorPath "0"
  ${Else} 
    ${GetParent} $0 $1
	StrCpy $ibaDatCoordinatorPath $1
  ${EndIf}

  StrCpy $PluginPath "$ibaDatCoordinatorPath\Plugins"

FunctionEnd

Function CloseStandAlone
  FindWindow $0 "" "ibaDatCoordinatorClientCloseForm"
  
  ${Unless} $0 == 0
	MessageBox MB_YESNO $(STOP_DATCO_STANDALONE_REQ) IDYES standalonestop IDNO standalonenostop
standalonestop:
    SendMessage $0 0x8140 0 0
	Sleep 1000
    FindWindow $0 "" "ibaDatCoordinatorClientCloseForm"
  ${EndUnless}
  Return
  
standalonenostop:  
  MessageBox MB_ICONSTOP $(DATCO_RUNNING_FAILURE)
  Abort
FunctionEnd

Function StopService
  Call GetServiceStatus
  Pop $ServiceStatus
  DetailPrint "ibaDatCoordinator service status is $ServiceStatus"
  ;Sleep 1000

  StrCmp $ServiceStatus '1:stopped' serviceend  ; check on running

  MessageBox MB_YESNO $(STOP_DATCO_SERVICE_REQ) IDYES servicestop IDNO servicenostop

servicestop:
  ;Stop service
  DetailPrint "Stopping DatCoordinator service..."
  nsSCMEx::Stop /NOUNLOAD "ibaDatCoordinatorService"

  POP $0
  DetailPrint "status = $0"
  Sleep 1000

  Call GetServiceStatus
  Pop $ServiceStatus
  DetailPrint "ibaDatCoordinator service status is $ServiceStatus"
  StrCmp $ServiceStatus '1:stopped' serviceend  ; check on running
  
servicenoStop:
  MessageBox MB_ICONSTOP $(DATCO_RUNNING_FAILURE)
  Abort
  
serviceend:

FunctionEnd

Function GetServiceStatus


/*
  !define SERVICE_STOPPED                0x00000001
  !define SERVICE_START_PENDING          0x00000002
  !define SERVICE_STOP_PENDING           0x00000003
  !define SERVICE_RUNNING                0x00000004
  !define SERVICE_CONTINUE_PENDING       0x00000005
  !define SERVICE_PAUSE_PENDING          0x00000006
  !define SERVICE_PAUSED                 0x00000007
*/

  ClearErrors


  nsSCMEx::QueryStatus  "ibaDatCoordinatorService"
  Pop $8 ; return error/success
  Pop $9 ; return service status

  StrCmp $8 'error' lbl_error

  ${Switch} $9
    ${Case} 1
      Strcpy $R0 '1:stopped'
      ${Break}
    ${Case} 2
      Strcpy $R0 '2:start pending'
      ${Break}
    ${Case} 3
      Strcpy $R0 '3:stop pending'
      ${Break}
    ${Case} 4
      Strcpy $R0 '4:running'
      ${Break}
    ${Case} 5
      Strcpy $R0 '5:continue pending'
      ${Break}
    ${Case} 6
      Strcpy $R0 '6:pause pending'
      ${Break}
    ${Case} 7
      Strcpy $R0 '7:paused'
      ${Break}
    ${Default}
      Goto lbl_error
  ${EndSwitch}

  Goto lbl_done

  lbl_error:
    Strcpy $R0 'error on getting service status'
  lbl_done:

  Push $R0

FunctionEnd

;--------------------------------
; Parse the version number
; x.y.z            --> x*1000000 + y*10000 + z*100 + 99
; x.y.z BETA       --> x*1000000 + y*10000 + z*100 + 0
; x.y.z BETAa      --> x*1000000 + y*10000 + z*100 + a

Function GetVersionNr
;  $R5 : version string
;  $0  : version number
;  $1  : element string
;  $2  : character counter
;  $3  : multiplicator
;  $4  : temp character

  Exch $R5
  Push $0
  Push $1
  Push $2
  Push $3
  Push $4

  StrCpy $0 "0"
  StrCpy $1 ""
  StrCpy $2 "-1"
  StrCpy $3 "1000000"

  loop:
  IntOp $2 $2 + 1
  StrCpy $4 $R5 1 $2   ;Copy next character from version string
  StrCmp $4 "" dotFound
  StrCmp $4 " " dotFound
  StrCmp $4 "." dotFound
  StrCpy $1 $1$4       ;Add character to element string
  Goto loop

  dotFound:
  IntOp $1 $1 * $3     ;Multiply value with its weight
  IntOp $0 $0 + $1     ;Add weighted value to version nr
  IntOp $3 $3 / 100    ;Decrease weight of next value
  StrCpy $1 ""         ;Initialize element string
  StrCmp $4 "." loop

  ${StrLoc} $2 $R5 "BETA" "<"
  ${If} $2 != ""
    ;Get BETA number
    ${If} $2 > 0
      IntOp $2 $2 * -1
      StrCpy $1 $R5 10 $2  ;Copy BETA number to $1
      IntOp $0 $0 + $1  ;Add BETA number to version number
    ${EndIf}
  ${Else}
    IntOp $0 $0 + 99  ;Add 99 to version number if it is not a BETA version
  ${EndIf}

  Pop $4
  Pop $3
  Pop $2
  Pop $1
  StrCpy $R5 $0
  Pop $0
  Exch $R5

FunctionEnd

LangString UPGRADE_REQUIRED                  ${LANG_ENGLISH} "ibaDatCoordinator v2.0.0 or higher is required for the plugin.$\r$\nPlease contact iba-AG for an upgrade."
LangString NO_DATCO                          ${LANG_ENGLISH} "ibaDatCoordinator is not installed."
LangString NO_DATCO_FOUND                    ${LANG_ENGLISH} "No DatCoordinator found, please install the iba DatCoordinator first"
LangString STOP_DATCO_STANDALONE_REQ         ${LANG_ENGLISH} "The ibaDatCoordinator program needs to be stopped to install the Plugin DLLs. Do you want to stop the ibaDatCoordinator now?"
LangString FINISH_TEXT                       ${LANG_ENGLISH} "The Plugin DLL(s) are now installed on your computer"
LangString FAILED_COPY                       ${LANG_ENGLISH} "Failed to copy plugin files"
LangString DATCO_RUNNING_FAILURE             ${LANG_ENGLISH} "The plugin dlls cannot be installed when the ibaDatCoordinator service is running!"
LangString STOP_DATCO_SERVICE_REQ            ${LANG_ENGLISH} "The ibaDatCoordinator Service needs to be stopped to install the Plugin DLLs. Do you want to stop the ibaDatCoordinator service now?"
LangString DATCO_CLIENTONLY					 ${LANG_ENGLISH} "ibaDatCoordinator is installed as client only. The plugin needs to be installed on the remote server side only!"

LangString UPGRADE_REQUIRED                  ${LANG_GERMAN} "ibaDatCoordinator v2.0.0 oder h�her ist f�r das Plugin erforderlich.$\r$\nBitte kontaktieren Sie iba AG f�r ein Upgrade."
LangString NO_DATCO                          ${LANG_GERMAN} "ibaDatCoordinator ist nicht installiert."
LangString NO_DATCO_FOUND                    ${LANG_GERMAN} "Kein DatCoordinator gefunden, bitte installieren Sie zuerst ibaDatCoordinator."
LangString STOP_DATCO_STANDALONE_REQ         ${LANG_GERMAN} "Der ibaDatCoordinator-Programm muss angehalten werden, um die Plugin-DLLs installieren zu k�nnen. Wollen Sie ibaDatCoordinator jetzt anhalten?"
LangString FINISH_TEXT                       ${LANG_GERMAN} "Die Plugin-DLL(s) sind nun auf Ihrem Computer installiert."
LangString FAILED_COPY                       ${LANG_GERMAN} "Kopieren der Plugin-Dateien fehlgeschlagen"
LangString DATCO_RUNNING_FAILURE             ${LANG_GERMAN} "Die Plugin-DLLs k�nnen nicht installiert werden, wenn der ibaDatCoordinator-Dienst l�uft!"
LangString STOP_DATCO_SERVICE_REQ            ${LANG_GERMAN} "Der ibaDatCoordinator-Dienst muss angehalten werden, um die Plugin-DLLs installieren zu k�nnen. Wollen Sie den ibaDatCoordinator-Dienst jetzt anhalten?"
LangString DATCO_CLIENTONLY					 ${LANG_GERMAN} "ibaDatCoordinator ist nur als Client installiert. Das Plugin muss nur auf dem Remote Server installiert werden!"

