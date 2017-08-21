; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "ibaDatCoordinator Roh Plugin Installer"
!define PRODUCT_VERSION "2.0.3"
!define PRODUCT_FILE_VERSION "2.0.3.0"
!define PRODUCT_PUBLISHER "iba AG"
!define PRODUCT_WEB_SITE "http://www.iba-ag.com"

;--------------------------------
;Variables

Var ServiceStatus
Var PluginPath
Var ibaDatCoordinatorPath

!AddPluginDir .

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

SetCompressor /SOLID lzma

; MUI Settings
!define MUI_ABORTWARNING


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
OutFile "ibaRohPluginInstaller_v${PRODUCT_VERSION}.exe"
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

Section "MainSection" SEC01

  ;Check version of installed ibaDatco
  ReadRegStr $0 ${DATCO_UNINST_ROOT_KEY} "${DATCO_UNINST_KEY}" "DisplayVersion"
  ${If} $0 != ""
    DetailPrint "ibaDatCoordinator $0 is installed"
    
    ;Retrieve version number from string
    Push $0
    Call GetVersionNr
    Pop $R0
    IntCmp $R0 1220000 okVersion oldVersion okVersion

    oldVersion:
    MessageBox MB_ICONSTOP $(UPGRADE_REQUIRED)
    Abort

    okVersion:

  ${Else}
    MessageBox MB_ICONSTOP $(NO_DATCO) 
    Abort
  ${EndIf}

  ClearErrors
  ReadRegStr $0 HKLM "System\Currentcontrolset\services\ibaDatcoordinatorService" "imagepath"
  IfErrors datcorunsstandalone datcorunsasservice
  
datcorunsstandalone:
;DatCoordinator not running as a service
  ClearErrors
  ReadRegStr $0 "${DATCO_UNINST_ROOT_KEY}" "${DATCO_UNINST_KEY}" "InstallDir"
  IfErrors nodatcoordinator coordinatorpresent
nodatcoordinator:
  MessageBox MB_ICONSTOP $(NO_DATCO_FOUND)
  Abort
coordinatorpresent:
  StrCpy $ibaDatCoordinatorPath $0
  StrCpy $PluginPath "$ibaDatCoordinatorPath\Plugins"
  DetailPrint "Path to plugins is : $PluginPath"

  FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
  ${If} $0 == 0
        Goto standalonecopy
  ${EndIf}

  MessageBox MB_YESNO $(STOP_DATCO_REQ) IDYES standalonestop IDNO standalonenostop
standalonestop:
  ;stop statusform
  FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
  SendMessage $0 0x8140 0 0
  ${Unless} $0 == 0
    Sleep 1000
    FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
  ${EndUnless}
standalonecopy:
  ClearErrors
  DetailPrint "Copying plugin files"
  SetOutPath "$PluginPath"
  File "..\Dependencies\msvcr100.dll"
  File "..\Dependencies\msvcp100.dll"
  File "..\RohPlugin\bin\Release\Alunorf_roh_plugin.dll"
  File "..\RohPlugin\bin\Release\RohWriter.dll"
  IfErrors standalonecopyError standalonecopyOk
standalonecopyError:
  MessageBox MB_ICONSTOP $(FAILED_COPY)
  Abort
standalonecopyOk:
  Goto end
standalonenostop:
  MessageBox MB_ICONSTOP $(DATCO_RUNNING_FAILURE)
  Abort
datcorunsasservice:
  DetailPrint "Imagepath to ibaDatCoordinatorService is : $0"
  ${GetParent} $0 $1
  StrCpy $ibaDatCoordinatorPath $1
  StrCpy $PluginPath "$ibaDatCoordinatorPath\Plugins"
  DetailPrint "Path to plugins is : $PluginPath"

  Call GetServiceStatus
  Pop $ServiceStatus
  DetailPrint "ibaDatCoordinator service status is $ServiceStatus"
  ;Sleep 1000

  StrCmp $ServiceStatus '1:stopped' servicecopy  ; check on running

  MessageBox MB_YESNO $(STOP_DATCO_SERVICE_REQ) IDYES servicestop IDNO servicenostop

servicestop:
  ;stop statusform
  FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
  SendMessage $0 0x8140 0 0
  ${Unless} $0 == 0
    Sleep 1000
    FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
  ${EndUnless}

  ;Stop service
  DetailPrint "Stopping DatCoordinator service..."
  nsSCMEx::Stop /NOUNLOAD "ibaDatCoordinatorService"

  POP $0
  DetailPrint "status = $0"
  Sleep 1000

  Call GetServiceStatus
  Pop $ServiceStatus
  DetailPrint "ibaDatCoordinator service status is $ServiceStatus"
  ;Sleep 1000

servicecopy:
  ClearErrors
  DetailPrint "Copying plugin files"
  SetOutPath "$PluginPath"
  File "..\Dependencies\msvcr100.dll"
  File "..\Dependencies\msvcp100.dll"
  File "..\RohPlugin\bin\Release\Alunorf_roh_plugin.dll"
  File "..\RohPlugin\bin\Release\RohWriter.dll"
  IfErrors servicecopyError servicecopyOk

servicecopyError:
  MessageBox MB_ICONSTOP $(FAILED_COPY)
  Abort
servicecopyOk:
  Goto end
servicenostop:
  MessageBox MB_ICONSTOP $(DATCO_RUNNING_FAILURE)
  Abort
end:

SectionEnd

Section -Post
SectionEnd

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

LangString UPGRADE_REQUIRED                  ${LANG_ENGLISH} "ibaDatCoordinator v1.22.0 or higher is required for plugins.$\r$\nPlease contact iba-AG for an upgrade."
LangString NO_DATCO                          ${LANG_ENGLISH} "ibaDatCoordinator is not installed."
LangString NO_DATCO_FOUND                    ${LANG_ENGLISH} "No DatCoordinator found, please install the iba DatCoordinator first"
LangString STOP_DATCO_REQ                    ${LANG_ENGLISH} "The ibaDatCoordinator Service needs to be stopped to install the Plugin DLLs. Do you want to stop the ibaDatCoordinator now?"
LangString FINISH_TEXT                       ${LANG_ENGLISH} "The Plugin DLL(s) are now installed on your computer"
LangString FAILED_COPY                       ${LANG_ENGLISH} "Failed to copy plugin files"
LangString DATCO_RUNNING_FAILURE             ${LANG_ENGLISH} "The plugin dlls cannot be installed when the ibaDatCoordinator service is running!"
LangString STOP_DATCO_SERVICE_REQ            ${LANG_ENGLISH} "The ibaDatCoordinator Service needs to be stopped to install the Plugin DLLs. Do you want to stop the ibaDatCoordinator service now?"

LangString UPGRADE_REQUIRED                  ${LANG_GERMAN} "ibaDatCoordinator v1.22.0 or higher is required for plugins.$\r$\nPlease contact iba-AG for an upgrade."
LangString NO_DATCO                          ${LANG_GERMAN} "ibaDatCoordinator is not installed."
LangString NO_DATCO_FOUND                    ${LANG_GERMAN} "No DatCoordinator found, please install the iba DatCoordinator first"
LangString STOP_DATCO_REQ                    ${LANG_GERMAN} "The ibaDatCoordinator Service needs to be stopped to install the Plugin DLLs. Do you want to stop the ibaDatCoordinator now?"
LangString FINISH_TEXT                       ${LANG_GERMAN} "The Plugin DLL(s) are now installed on your computer"
LangString FAILED_COPY                       ${LANG_GERMAN} "Failed to copy plugin files"
LangString DATCO_RUNNING_FAILURE             ${LANG_GERMAN} "The plugin dlls cannot be installed when the ibaDatCoordinator service is running!"
LangString STOP_DATCO_SERVICE_REQ            ${LANG_GERMAN} "The ibaDatCoordinator Service needs to be stopped to install the Plugin DLLs. Do you want to stop the ibaDatCoordinator service now?"