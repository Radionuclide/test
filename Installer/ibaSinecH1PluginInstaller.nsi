; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "ibaDatCoordinator Sinec-H1 Plugin Installer"
!define PRODUCT_VERSION "1.3"
!define PRODUCT_PUBLISHER "iba AG"
!define PRODUCT_WEB_SITE "http://www.iba-ag.com"

;--------------------------------
;Variables

Var ServiceStatus
Var PluginPath
Var ibaDatCoordinatorPath

;--------------------------------
;General Interface settings

!define MUI_ICON    "Graphics\ibaInstall.ico"
!define MUI_UNICON  "Graphics\ibaUnInstall.ico"

!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "Graphics\MUIInstallLogo.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP "Graphics\ibaWizard.bmp"

!define MUI_FINISHPAGE_TEXT "The Plugin DLL(s) are now installed on your computer"
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
OutFile "ibaSinecH1PluginInstaller.exe"
InstallDir "$PROGRAMFILES\iba\ibaDatCoordinator\plugins"
ShowInstDetails show
BrandingText "iba AG"

Section "MainSection" SEC01

  ;Check version of installed ibaPDA
  ReadRegStr $0 ${DATCO_UNINST_ROOT_KEY} "${DATCO_UNINST_KEY}" "DisplayVersion"
  ${If} $0 != ""
    DetailPrint "ibaDatCoordinator $0 is installed"
    
    ;Retrieve version number from string
    Push $0
    Call GetVersionNr
    Pop $R0
    IntCmp $R0 1070000 okVersion oldVersion okVersion

    oldVersion:
    MessageBox MB_ICONSTOP "ibaDatCoordinator v1.7.0 or higher is required for plugins.$\r$\nPlease contact iba-AG for an upgrade."
    Abort

    okVersion:

  ${Else}
    MessageBox MB_ICONSTOP "ibaDatCoordinator is not installed."
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
  MessageBox MB_ICONSTOP "No DatCoordinator found, please install the iba DatCoordinator first"
  Abort
coordinatorpresent:
  StrCpy $ibaDatCoordinatorPath $0
  StrCpy $PluginPath "$ibaDatCoordinatorPath\Plugins"
  DetailPrint "Path to plugins is : $PluginPath"

  FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
  ${If} $0 == 0
        Goto standalonecopy
  ${EndIf}

  MessageBox MB_YESNO "The ibaDatCoordinator Service needs to be stopped to install the Plugin DLLs. Do you want to stop the ibaDatCoordinator now?" IDYES standalonestop IDNO standalonenostop
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
  File "..\Alunorf-sinec-h1-plugin\bin\Release\Alunorf-sinec-h1-plugin.dll"
  ;File "..\Alunorf-sinec-h1-plugin\bin\Release\DatCoordinatorPlugins.dll"
  File "..\Alunorf-sinec-h1-plugin\bin\Release\H1Protocol.dll"
  File "..\Dependencies\Wmknt.dll"
  SetOutPath "$PluginPath\de"
  File "..\Alunorf-sinec-h1-plugin\bin\Release\de\Alunorf-sinec-h1-plugin.resources.dll"
  ;not copying ibaFiles as this should already be included with the datcoordinator
  ;copy H1 driver
  SetOutPath "$PluginPath\H1Driver"
  File "..\INAT\H1Prot.inf"
  File "..\INAT\H1Prot.sys"
  File "..\INAT\viewnetworks.cmd"
  IfErrors standalonecopyError standalonecopyOk
standalonecopyError:
  MessageBox MB_ICONSTOP "Failed to copy plugin files"
  Abort
standalonecopyOk:
  ;ask to install the H1driver
  Exec '$PluginPath\H1Driver\viewnetworks.cmd'
  Sleep 3000
  MessageBox MB_OK $(DESC_H1INSTALL)
  ;uncomment if you want to restart ibaDatCoordinator
  ;Exec '"$ibaDatCoordinatorPath\ibaDatCoordinator.exe"'
  Goto end
standalonenostop:
  MessageBox MB_ICONSTOP "The plugin dlls cannot be installed while the ibaDatCoordinator is running!"
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

  MessageBox MB_YESNO "The ibaDatCoordinator Service needs to be stopped to install the Plugin DLLs. Do you want to stop the ibaDatCoordinator service now?" IDYES servicestop IDNO servicenostop

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
  DetailPrint "ibaPDA service status is $ServiceStatus"
  ;Sleep 1000

servicecopy:
  ClearErrors
  DetailPrint "Copying plugin files"
  SetOutPath "$PluginPath"
  File "..\Alunorf-sinec-h1-plugin\bin\Release\Alunorf-sinec-h1-plugin.dll"
  File "..\Alunorf-sinec-h1-plugin\bin\Release\DatCoordinatorPlugins.dll"
  File "..\Alunorf-sinec-h1-plugin\bin\Release\H1Protocol.dll"
  File "..\Dependencies\Wmknt.dll"
  ;not copying ibaFiles as this should already be included with the datcoordinator
  ;CopyFiles "$EXEDIR\pluginFiles\*.*" "$PluginPath"

  ;german resources for the plugin
  SetOutPath "$PluginPath\de"
  File "..\Passolo\de\Alunorf-sinec-h1-plugin.resources.dll"

  ;H1 driver
  SetOutPath "$PluginPath\H1Driver"
  File "..\INAT\H1Prot.inf"
  File "..\INAT\H1Prot.sys"
  File "..\INAT\viewnetworks.cmd"
  IfErrors servicecopyError servicecopyOk

servicecopyError:
  MessageBox MB_ICONSTOP "Failed to copy plugin files"
  Abort
servicecopyOk:
  ;ask to install the H1driver
  Exec '$PluginPath\H1Driver\viewnetworks.cmd'
  Sleep 3000
  MessageBox MB_OK $(DESC_H1INSTALL)

  ;uncomment if you want to restart ibaDatCoordinator
  ;  Exec '"$ibaDatCoordinatorPath\ibaDatCoordinator.exe" /service'
  ;  FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
  ;  ${While} $0 == 0
  ;    Sleep 500
  ;    FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
  ;  ${EndWhile}
  ;  SendMessage $0 0x8141 0 0

  ;Call GetServiceStatus
  ;Pop $ServiceStatus
  ;DetailPrint "ibaDatCoordinatorservice status is $ServiceStatus"

  Goto end
  
servicenostop:
  MessageBox MB_ICONSTOP "The plugin dlls cannot be installed when the ibaDatCoordinator service is running!"
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

LangString DESC_H1INSTALL  ${LANG_ENGLISH} "Installation of the PC-H1 protocol driver:$\r$\n$\t- Open the network settings in your system$\r$\n$\t- Choose minimal one Local Area Connection$\r$\n$\t- Press Properties$\r$\n$\t- If installed, disable 'Qos Packet Scheduler'$\r$\n$\t- Press Install$\r$\n$\t- Add a (new) protocol$\r$\n$\t- Press the button 'Have Disc'$\r$\n$\t- You will find the drivers under$\r$\n$\t$\t $PluginPath\H1Driver$\r$\n$\t- Select the *.inf file$\r$\n$\t- Confirm with 'Open' and 'OK'$\r$\n$\t- Select 'INAT H1 Iso Protocol'$\r$\n$\t- Confirm with 'OK'$\r$\n$\t- The PC-H1 protocol driver is linked now to ALL of your network cards, disable the INAT H1 Iso Protocol on the cards that shouldn't have the protocol enabled.$\r$\n$\t- Close all network windows$\r$\n$\r$\nAttention:$\r$\n$\tWhen the PC-H1 protocol driver is installed, the PC has to be rebooted"
LangString DESC_H1INSTALL  ${LANG_GERMAN} "Installation des PC-H1 Protokoll Treibers:$\r$\n$\t- �ber die Systemsteuerung die Netzwerkverbindungen �ffnen$\r$\n$\t- W�hlen Sie mindestens eine LAN Verbindung aus$\r$\n$\t- Eigenschaften$\r$\n$\t- Wenn 'Qos-Paketplaner' installiert ist, bitte deaktivieren$\r$\n$\t- Installieren$\r$\n$\t- 'Protokoll hinzuf�gen' ausw�hlen$\r$\n$\t- W�hlen Sie 'Datentr�ger'$\r$\n$\t- �ber 'Durchsuchen' folgenden Pfad ausw�hlen:$\r$\n$\t$\t $PluginPath\H1Driver$\r$\n$\t- Die *.inf Datei anw�hlen$\r$\n$\t- Mit '�ffnen' und 'OK' best�tigen$\r$\n$\t- 'INAT H1 Iso Protocol' ausw�hlen$\r$\n$\t- Mit 'OK' best�tigen$\r$\n$\t- Der PC-H1 Protokolltreiber ist nun mit ALLEN Ihren Netzwerkkarten verbunden. Deaktivieren Sie das INAT H1 Iso Protokoll auf den Karten, auf denen es nicht aktiv sein soll.$\r$\n$\t- Alle Netzwerkeinstellungen schliessen$\r$\n$\r$\nBitte beachten Sie:$\r$\n$\tNach der Installation des PC-H1 Protokoll Treibers muss der PC neu gestartet werden"

