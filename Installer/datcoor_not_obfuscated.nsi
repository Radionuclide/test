; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_VERSION "1.22.0"
!define PRODUCT_FILE_VERSION "1.22.0.0"
;!define DO_UNINSTALLER_SIGNING use this to generate and use a signed uninstaller. This also requires SIGN_TOOL, SIGN_CERT, SIGN_PASS and optional SIGN_TIMESTAMP_URL
;!define UNINSTALLER_ONLY use this to generate the uninstaller

!ifdef DO_UNINSTALLER_SIGNING
;Section to generate uninstaller and sign it.
;This actually calls the same script again with the UNINSTALLER_ONLY define set.
;After the special installer is created it is run and the uninstaller is signed

!verbose push
!verbose 4

!echo "Creating installer with unsigned uninstaller"
!system '"${NSISDIR}\makensis" /V2 /DUNINSTALLER_ONLY /D"PRODUCT_VERSION=${PRODUCT_VERSION}" /D"PRODUCT_FILE_VERSION=${PRODUCT_FILE_VERSION}" ${__FILE__}' = 0

!echo "Executing installer with unsigned uninstaller"
!system "..\InstallFiles\uninstaller_only.exe" = 2
!delfile "..\InstallFiles\uninstaller_only.exe"

!echo "Signing uninstaller"
!ifdef SIGN_TIMESTAMP_URL
!system '"${SIGN_TOOL}" sign /f "${SIGN_CERT}" /p ${SIGN_PASS} /t ${SIGN_TIMESTAMP_URL} ..\InstallFiles\uninst.exe' = 0
!else
!system '"${SIGN_TOOL}" sign /f "${SIGN_CERT}" /p ${SIGN_PASS} ..\InstallFiles\uninst.exe' = 0
!endif

!verbose pop

!endif
!define PRODUCT_NAME "ibaDatCoordinator"
!define PRODUCT_PUBLISHER "iba ag"
!define PRODUCT_WEB_SITE "http://www.iba-ag.com"
;!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\ibaPda.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

; .NET Framework
; English
!define BASE_URL http://download.microsoft.com/download
!define URL_DOTNET "${BASE_URL}/5/6/7/567758a3-759e-473e-bf8f-52154438565a/dotnetfx.exe"

!include "MUI.nsh"
!include "WinMessages.nsh"
;!define LOGICLIB_STRCMP
!include "LogicLib.nsh"
!include "sections.nsh"
!include "StrFunc.nsh"

!include WordFunc.nsh
!insertmacro VersionCompare

!ifdef UNINSTALLER_ONLY
SetCompress off
!else
SetCompressor /SOLID lzma
!endif

;--------------------------------
;General Interface settings

!define MUI_ICON    "Graphics\ibaInstall.ico"
!define MUI_UNICON  "Graphics\ibaUnInstall.ico"

;--------------------------------
;Interface settings

!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "Graphics\MUIInstallLogo.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP "Graphics\ibaWizard.bmp"
!define MUI_ABORTWARNING
!define MUI_COMPONENTSPAGE_NODESC
!define MUI_WELCOMEFINISHPAGE_INI "welcome.ini"
;!define MUI_COMPONENTSPAGE_SMALLDESC
;!define MUI_FINISHPAGE_NOAUTOCLOSE

;--------------------------------
; Language Selection Dialog Settings

;Remember the installer language
!define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_LANGDLL_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"

Function OnEnd
  ReadRegStr $0 ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Server"
  ${If} $0 == "1"
    Exec '"$INSTDIR\ibaDatCoordinator.exe" /service'
    FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
    ${While} $0 == 0
      Sleep 500
      FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
    ${EndWhile}
    SendMessage $0 0x8141 0 0
  ${Else}
    Exec '"$INSTDIR\ibaDatCoordinator.exe"'
  ${EndIf}
FunctionEnd

;--------------------------------
; Install pages

Page custom PreInstall
!define MUI_WELCOMEPAGE_TITLE_3LINES
;!define MUI_PAGE_CUSTOMFUNCTION_PRE "DisableBackButton"
!insertmacro MUI_PAGE_WELCOME
Page custom InstalltypeSelect
Page custom ServiceAccountPage
;!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_TITLE_3LINES
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_FUNCTION "OnEnd"
!insertmacro MUI_PAGE_FINISH

!ifndef DO_UNINSTALLER_SIGNING
;--------------------------------
; Uninstall pages

!define MUI_WELCOMEPAGE_TITLE_3LINES
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!define MUI_FINISHPAGE_TITLE_3LINES
!insertmacro MUI_UNPAGE_FINISH
!endif

;--------------------------------
; Languages

!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "German"
!insertmacro MUI_LANGUAGE "French"

;--------------------------------
; General

Name "${PRODUCT_NAME} v${PRODUCT_VERSION}"
!ifdef UNINSTALLER_ONLY
OutFile "..\InstallFiles\uninstaller_only.exe"
!else
OutFile "ibaDatCoordinatorInstall_v${PRODUCT_VERSION}.exe"
!endif
InstallDir "$PROGRAMFILES\iba\ibaDatCoordinator"
;InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails hide
ShowUnInstDetails hide
BrandingText "iba AG"

VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "${PRODUCT_NAME}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments" ""
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "${PRODUCT_PUBLISHER}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalTrademarks" ""
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "Copyright ?2006 iba AG"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "${PRODUCT_NAME} installer"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "${PRODUCT_VERSION}"
VIProductVersion "${PRODUCT_FILE_VERSION}"

;--------------------------------
; Reserve Files

ReserveFile "serviceorstandalone.ini"
;ReserveFile "welcome.ini"
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
!insertmacro MUI_RESERVEFILE_LANGDLL

${StrLoc}

VAR WinVer
VAR PrevUninstall
Var ServiceUserName
Var ServicePassword


InstType "service"
InstType "standalone"

;--------------------------------
; Initialization functions
Function .onInit
  SetShellVarContext all
  
!ifdef UNINSTALLER_ONLY

  WriteUninstaller "$EXEDIR\..\InstallFiles\uninst.exe"
  Quit
  
!else
  
  !insertmacro MUI_INSTALLOPTIONS_EXTRACT "serviceorstandalone.ini"
  ${If} $LANGUAGE == 1031       ;German
    !insertmacro MUI_INSTALLOPTIONS_EXTRACT_AS "ServiceAccount_de.ini" "ServiceAccount.ini"
  ${ElseIf} $LANGUAGE == 1036   ;French
    !insertmacro MUI_INSTALLOPTIONS_EXTRACT_AS "ServiceAccount_fr.ini" "ServiceAccount.ini"
  ${Else}
    !insertmacro MUI_INSTALLOPTIONS_EXTRACT_AS "ServiceAccount_en.ini" "ServiceAccount.ini"
  ${EndIf}
  ;!insertmacro MUI_INSTALLOPTIONS_EXTRACT "welcome.ini"
  ;Display language selection dialog
  ;!insertmacro MUI_LANGDLL_DISPLAY
    ;In case of a silent install call the PreInstall function directly
  IfSilent +1 +3
    Call PreInstall
	SetCurInstType 1
	
!endif
FunctionEnd

!ifndef DO_UNINSTALLER_SIGNING
Function un.onInit
  ;!insertmacro MUI_UNGETLANGUAGE
FunctionEnd
!endif

;--------------------------------
; PreInstall function that checks requirements and previous installs
; Remark : This is done in separate "dummy" page because initialization functions
;          don't support translated error messages

Function PreInstall
  Call CheckRequirements
  Call CheckPreviousVersions
FunctionEnd

Function InstalltypeSelect
  !insertmacro MUI_INSTALLOPTIONS_WRITE "serviceorstandalone.ini" "Field 1" "Text" "$(TEXT_INSTALLSERVICE)"
  !insertmacro MUI_INSTALLOPTIONS_WRITE "serviceorstandalone.ini" "Field 2" "Text" "$(TEXT_INSTALLSTANDALONE)"
  !insertmacro MUI_HEADER_TEXT "$(TEXT_SERVICEORSTANDALONE_TITLE)" "$(TEXT_SERVICEORSTANDALONE_SUBTITLE)"
  ReadRegStr $0 ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Server"
  ${If} $0 == "0"
    !insertmacro MUI_INSTALLOPTIONS_WRITE "serviceorstandalone.ini" "Field 1" "State" "0"
    !insertmacro MUI_INSTALLOPTIONS_WRITE "serviceorstandalone.ini" "Field 2" "State" "1"
  ${Else}
    !insertmacro MUI_INSTALLOPTIONS_WRITE "serviceorstandalone.ini" "Field 1" "State" "1"
    !insertmacro MUI_INSTALLOPTIONS_WRITE "serviceorstandalone.ini" "Field 2" "State" "0"
  ${EndIf}
  
  
  
  !insertmacro MUI_INSTALLOPTIONS_DISPLAY "serviceorstandalone.ini"
  !insertmacro MUI_INSTALLOPTIONS_READ $0 "serviceorstandalone.ini" "Field 1" "State"
  ${If} $0 != "1"
    SetCurInstType 0
  ${Else}
    SetCurInstType 1
  ${EndIf}
FunctionEnd

;--------------------------------
; Requirements check

Function CheckRequirements
  ;Check that operating system is supported
  Call GetWindowsVersion
  Pop $WinVer
  StrCmp $WinVer "2000" OSisOk +1
  StrCmp $WinVer "XP" OSisOK +1
  StrCmp $WinVer "2003" OSisOK +1
  StrCmp $WinVer "2008" OSisOK +1
  StrCmp $WinVer "win7" OSisOK +1
  StrCmp $WinVer "win8" OSisOK +1
  StrCmp $WinVer "win8.1" OSisOK +1
    MessageBox MB_OK|MB_ICONSTOP $(TEXT_OS_NOT_SUPPORTED)
    Quit
  OSisOk:

  ;Check user has administrator rights
  UserInfo::GetName
  Pop $0
  UserInfo::GetAccountType
  Pop $1
  StrCmp $1 "Admin" adminOK +1
    MessageBox MB_OK|MB_ICONSTOP $(TEXT_NOT_ADMINISTRATOR)
    Quit
  adminOK:

  ;Check if required .NET framework version is installed
  Call GetDotNETVersion
  Pop $0
  ${If} $0 == "not found"
       MessageBox MB_OK|MB_ICONSTOP $(TEXT_FRAMEWORK_MISSING)
       Quit
  ${EndIf}

FunctionEnd

;--------------------------------
; Check if previous version of ibaDatCoordinator are installed

Function CheckPreviousVersions

  StrCpy $PrevUninstall ""
  StrCpy $ServiceUserName ""
  StrCpy $ServicePassword ""

  ;Check if previous install found
  ReadRegStr $0 ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion"
  ${If} $0 != ""

    ;Retrieve version number from string
    Push $0
    Call GetVersionNr
    Pop $R0
    Push "${PRODUCT_VERSION}"
    Call GetVersionNr
    Pop $R1
    IntCmp $R0 $R1 sameVersion olderVersion newerVersion

    sameVersion:
    ;Same version is already installed --> ask to re-install
    MessageBox MB_YESNO|MB_ICONSTOP $(TEXT_ALREADY_INSTALLED) /SD IDYES IDYES upgrade IDNO quit

    olderVersion:
    ;Older version is installed --> ask to upgrade
    MessageBox MB_YESNO|MB_ICONQUESTION $(TEXT_OLDER_INSTALLED) /SD IDYES IDYES upgrade IDNO quit

    newerVersion:
    ;Newer version is installed --> quit (manual uninstall is needed)
    MessageBox MB_OK|MB_ICONSTOP $(TEXT_NEWER_INSTALLED)
    Quit
  ${Else}
    Goto end
  ${EndIf}


  quit:
  Quit

  upgrade:
  ReadRegStr $INSTDIR ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "InstallDir"
  ReadRegStr $0 ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString"
  ReadRegStr $ServiceUserName ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Service1" ;Retrieve user name
  ReadRegStr $ServicePassword ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Service2" ;Retrieve password
  StrCpy $PrevUninstall '"$0" /S _?=$INSTDIR'  ;add silent parameter switch to uninstall string

  end:
FunctionEnd

;--------------------------------
; Uninstall previous version section

Section -PreInstall
  ${If} $PrevUninstall != ""
    ;Uninstall previous version
    DetailPrint $(TEXT_PREV_UNINSTALLING)
    nsExec::Exec $PrevUninstall
  ${EndIf}
SectionEnd

Section $(DESC_DATCOOR_NOSERVICE) DATCOOR_NOSERVICE
  SectionIn 1
  SetOverwrite on
  
  ;Copy server files
  SetOutPath "$INSTDIR"
  File "..\Dependencies\ibaFilesLiteInstall.exe"
  File "..\Dependencies\ibaLogger.dll"
  File "..\Dependencies\Eyefinder.dll"
  File "..\Dependencies\DotNetMagic2005.DLL"
  File "..\Dependencies\DotNetMagic.DLL"
  File "..\Dependencies\msvcr100.dll"
  File "..\Dependencies\msvcp100.dll"  
  File "..\Dependencies\ICSharpCode.TextEditor.dll"
  File "..\Dependencies\ICSharpCode.SharpZipLib.dll"
  File "..\Dependencies\msvcr100.dll"
  File "..\Dependencies\msvcp100.dll"  
;HD-stuff
  File "..\Dependencies\hdClient.dll"
  File "..\Dependencies\hdClientInterfaces.dll"
  File "..\Dependencies\hdCommon.dll"
  File "..\Dependencies\DevExpress.XtraEditors.v6.3.dll"
  File "..\Dependencies\DevExpress.XtraGrid.v6.3.dll"
  File "..\Dependencies\DevExpress.Data.v6.3.dll"
  File "..\Dependencies\DevExpress.Utils.v6.3.dll"
  
  File "..\ibaDatCoordinator\bin\Release\Interop.ibaFilesLiteLib.dll"
  File "..\ibaDatCoordinator\bin\Release\Interop.IbaAnalyzer.dll"
  File "..\ibaDatCoordinator\bin\Release\ibaDatCoordinator.exe"

  File "..\ibaDatCoordinator\bin\Release\DatCoUtil.dll"
  File "..\ibaDatCoordinator\Resources\default.ico"
  File "..\DatCoordinatorPlugins\bin\Release\DatCoordinatorPlugins.dll"
  File "readme.htm"
  SetOutPath "$INSTDIR\de"
  File "..\Passolo\de\ibaDatCoordinator.resources.dll"
  SetOutPath "$INSTDIR\fr"
  File "..\Passolo\fr\ibaDatCoordinator.resources.dll"
  ;Install ibaFiles
  DetailPrint $(TEXT_IBAFILES_INSTALL)
  nsExec::Exec '"$INSTDIR\ibaFilesLiteInstall.exe" /S'

  SetOutPath "$INSTDIR"
  ;Create uninstall shortcut
  CreateDirectory "$SMPROGRAMS\iba\ibaDatCoordinator"
  CreateShortCut "$SMPROGRAMS\iba\ibaDatCoordinator\ibaDatCoordinator.lnk" "$INSTDIR\ibaDatCoordinator.exe" "" "$INSTDIR\default.ico"
  CreateDirectory "%LOCALAPPDATA%\iba\ibaDatCoordinator"
  CreateShortCut "$SMPROGRAMS\iba\ibaDatCoordinator\$(TEXT_LOG_FILES).lnk" "$LOCALAPPDATA\iba\ibaDatCoordinator"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Server" "0"
SectionEnd

Section $(DESC_DATCOOR_SERVICE) DATCOOR_SERVICE
  SectionIn 2
  SetOverwrite on
  ;Copy server files
  SetOutPath "$INSTDIR"
  File "..\Dependencies\ibaFilesLiteInstall.exe"
  File "..\Dependencies\ibaLogger.dll"
  File "..\Dependencies\Eyefinder.dll"
  File "..\Dependencies\DotNetMagic2005.DLL"
  File "..\Dependencies\DotNetMagic.DLL"
  File "..\Dependencies\ICSharpCode.TextEditor.dll"
  File "..\Dependencies\ICSharpCode.SharpZipLib.dll"
  File "..\Dependencies\msvcr100.dll"
  File "..\Dependencies\msvcp100.dll"  
;HD-stuff
  File "..\Dependencies\hdClient.dll"
  File "..\Dependencies\hdClientInterfaces.dll"
  File "..\Dependencies\hdCommon.dll"
  File "..\Dependencies\DevExpress.XtraEditors.v6.3.dll"
  File "..\Dependencies\DevExpress.XtraGrid.v6.3.dll"
  File "..\Dependencies\DevExpress.Data.v6.3.dll"
  File "..\Dependencies\DevExpress.Utils.v6.3.dll"
  File "..\ibaDatCoordinator\bin\Release\Interop.ibaFilesLiteLib.dll"
  File "..\ibaDatCoordinator\bin\Release\Interop.IbaAnalyzer.dll"
  File "..\ibaDatCoordinator\bin\Release\ibaDatCoordinator.exe"
  
  File "..\ibaDatCoordinator\bin\Release\Interop.ibaFilesLiteLib.dll"
  File "..\ibaDatCoordinator\bin\Release\Interop.IbaAnalyzer.dll"
  File "..\ibaDatCoordinator\bin\Release\ibaDatCoordinator.exe"

  File "..\ibaDatCoordinator\bin\Release\DatCoUtil.dll"
  File "..\ibaDatCoordinator\Resources\running.ico"
  File "..\DatCoordinatorPlugins\bin\Release\DatCoordinatorPlugins.dll"
  File "..\ibaDatCoordinatorService\bin\Release\ibaDatCoordinatorService.exe"
  File "readme.htm"
  File "Copy_Printer_Settings_To_System_Account.bat"
  File "createundoregfile.bat"

  SetOutPath "$INSTDIR\de"
  File "..\Passolo\de\ibaDatCoordinator.resources.dll"
  SetOutPath "$INSTDIR\fr"
  File "..\Passolo\fr\ibaDatCoordinator.resources.dll"

  ;Install ibaFiles
  DetailPrint $(TEXT_IBAFILES_INSTALL)
  nsExec::Exec '"$INSTDIR\ibaFilesLiteInstall.exe" /S'
  
  DetailPrint $(TEXT_SERVICE_INSTALL)
  !insertmacro MUI_INSTALLOPTIONS_READ $R0 "ServiceAccount.ini" "Field 2" "State" ;local system
  ${If} $R0 == "1"
    StrCpy $ServiceUserName ""
    StrCpy $ServicePassword ""
  ${Else}
    !insertmacro MUI_INSTALLOPTIONS_READ $ServiceUserName "ServiceAccount.ini" "Field 5" "State" ;user name
    !insertmacro MUI_INSTALLOPTIONS_READ $ServicePassword "ServiceAccount.ini" "Field 7" "State" ;password
  ${EndIf}
  nsSCMEx::Install /NOUNLOAD "ibaDatCoordinatorService" "iba DatCoordinator Service" 0x010 2 "$INSTDIR\ibaDatCoordinatorService.exe" "" "" $ServiceUserName $ServicePassword
  Pop $R0
  ${If} $R0 != "success"
    Pop $R0
    MessageBox MB_OK|MB_ICONSTOP "Error installing service, error code $R0" 
  ${EndIf}
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Server" "1"
  ;Save username and password
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Service1" $ServiceUserName
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Service2" $ServicePassword

  ;Add serverstatus to autorun
  WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Run" "ibaDatCoordinator service status" "$INSTDIR\ibaDatCoordinator.exe /service"

  ;printer stuff
  SetOutPath "$INSTDIR"
  nsExec::Exec '"$INSTDIR\createundoregfile.bat"'
  nsExec::Exec '"$INSTDIR\Copy_Printer_Settings_To_System_Account.bat"'
  Delete "$INSTDIR\createundoregfile.bat"
  nsSCMEx::Stop /NOUNLOAD "Spooler"
  nsSCMEx::Start /NOUNLOAD "Spooler"

  ;shortcut
  CreateDirectory "$SMPROGRAMS\iba\ibaDatCoordinator"
  CreateShortCut "$SMPROGRAMS\iba\ibaDatCoordinator\ibaDatCoordinator Service Status.lnk" "$INSTDIR\ibaDatCoordinator.exe" "/service" "$INSTDIR\running.ico"
  CreateDirectory "$APPDATA\iba\ibaDatCoordinator"
  CreateShortCut "$SMPROGRAMS\iba\ibaDatCoordinator\$(TEXT_LOG_FILES).lnk" "$APPDATA\iba\ibaDatCoordinator"
  ;Start service
   nsSCMEx::Start /NOUNLOAD "ibaDatCoordinatorService"
SectionEnd

Section -Post

!ifdef DO_UNINSTALLER_SIGNING
  SetOutPath $INSTDIR
  File "..\InstallFiles\uninst.exe"
!else
  WriteUninstaller "$INSTDIR\uninst.exe"
!endif

  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\ibaDatCoordinator.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "HelpLink" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "InstallDir" "$INSTDIR"
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoModify" 1
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoRepair" 1
  CreateDirectory "$SMPROGRAMS\iba\ibaDatCoordinator"
  CreateShortCut "$SMPROGRAMS\iba\ibaDatCoordinator\$(TEXT_UNINSTALL).lnk" "$INSTDIR\uninst.exe"

  IfSilent +1 +2
    Exec '"$INSTDIR\ibaDatCoordinator.exe" /service'

SectionEnd


!ifndef DO_UNINSTALLER_SIGNING
;--------------------------------
; Uninstall section

Section Uninstall

  SetShellVarContext all

  ReadRegStr $0 ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Server"
  ${If} $0 == "1"
    Call un.UninstallService
  ${EndIf}

  Call un.UninstallTasks

  ;Delete uninstaller
  Delete "$INSTDIR\uninst.exe"

  ;Delete shortcuts
  RMDir /r "$SMPROGRAMS\iba\ibaDatCoordinator"
  RMDir "$SMPROGRAMS\iba" ;it will only be removed when it is empty

  ;Delete "Add/Remove programs" registry keys
  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  SetAutoClose true
SectionEnd


Function un.UninstallTasks
  ClearErrors
  Delete "$INSTDIR\ibaDatCoordinator.exe"
  IfErrors 0 +2
    Call un.stillRunning
  Delete "$INSTDIR\ibaLogger.dll"
  Delete "$INSTDIR\Eyefinder.dll"
  Delete "$INSTDIR\DatCoUtil.dll"
  Delete "$INSTDIR\DatCoordinatorPlugins.dll"
  Delete "$INSTDIR\DotNetMagic.dll"
  Delete "$INSTDIR\DotNetMagic2005.dll"
  Delete "$INSTDIR\hdClient.dll"
  Delete "$INSTDIR\hdClientInterfaces.dll"
  Delete "$INSTDIR\hdCommon.dll"
  Delete "$INSTDIR\DevExpress.XtraEditors.v6.3.dll"
  Delete "$INSTDIR\DevExpress.XtraGrid.v6.3.dll"
  Delete "$INSTDIR\DevExpress.Data.v6.3.dll"
  Delete "$INSTDIR\DevExpress.Utils.v6.3.dll"
  
  Delete "$INSTDIR\Interop.ibaFilesLiteLib.dll"
  Delete "$INSTDIR\Interop.IbaAnalyzer.dll"
  Delete "$INSTDIR\ICSharpCode.TextEditor.dll"
  Delete "$INSTDIR\ICSharpCode.SharpZipLib.dll"
  Delete "$INSTDIR\msvcr100.dll"
  Delete "$INSTDIR\msvcp100.dll"
  Delete "$INSTDIR\ibaFilesLiteInstall.exe"
  Delete "$INSTDIR\default.ico"
  Delete "$INSTDIR\readme.htm"
  Delete "$INSTDIR\Copy_Printer_Settings_To_System_Account.bat"
  Delete "$INSTDIR\createundoregfile.bat"
  Delete "$INSTDIR\de\ibaDatCoordinator.resources.dll"
  RMDir "$INSTDIR\de"
  Delete "$INSTDIR\fr\ibaDatCoordinator.resources.dll"
  RMDir "$INSTDIR\fr"
  ;Remove install dir
  RMDir "$INSTDIR"
FunctionEnd

Function un.UninstallService
  ;uninstall the service

  ;stop statusform
  FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
  SendMessage $0 0x8140 0 0
  ${Unless} $0 == 0
    Sleep 1000
    FindWindow $0 "" "ibaDatCoordinatorStatusCloseForm"
  ${EndUnless}

  ;Stop service
  DetailPrint $(TEXT_SERVICE_STOP)
  nsSCMEx::Stop /NOUNLOAD "ibaDatCoordinatorService"

  ;Delete service
  DetailPrint $(TEXT_SERVICE_REMOVE)
  nsSCMEx::Remove /NOUNLOAD "ibaDatCoordinatorService"

  Delete "$INSTDIR\ibaDatCoordinatorService.exe"
  IfErrors 0 +2
    Call un.waitAndDelete

  Delete "$INSTDIR\running.ico"

  ;Remove server status from autorun
  DeleteRegValue HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Run" "ibaDatCoordinator service status"
FunctionEnd

Function un.WaitAndDelete
  Sleep 1000
  Delete "$INSTDIR\ibaDatCoordinatorService.exe"
  IfErrors -2 0
FunctionEnd

Function un.StillRunning
  MessageBox MB_ICONSTOP|MB_OK $(TEXT_STILL_RUNNING)
  Abort
FunctionEnd

!endif ;DO_UNINSTALLER_SIGNING

;--------------------------------
; Description of sections

;!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
;  !insertmacro MUI_DESCRIPTION_TEXT ${SECDOTNET}       $(DESC_LONGDOTNET)
;  !insertmacro MUI_DESCRIPTION_TEXT ${DATCOOR_NOSERVICE}   $(DESC_DatCoor_NoService2)
;  !insertmacro MUI_DESCRIPTION_TEXT ${DATCOOR_SERVICE}   $(DESC_DatCoor_Service2)
;!insertmacro MUI_FUNCTION_DESCRIPTION_END


;--------------------------------
; Usefull functions

;--------------------------------
; GetDotNETVersion
;
; Usage:
;   Call GetDotNETVersion
;   Pop $0
;   $0 = "not found" when no .NET framework is installed
;      = "VERSION" with VERSION the version of the .NET framework installed


Function GetDotNETVersion
  Push $0
  Push $1

  System::Call "mscoree::GetCORVersion(w .r0, i ${NSIS_MAX_STRLEN}, *i) i .r1"
  StrCmp $1 0 +2
    StrCpy $0 "not found"

  Pop $1
  Exch $0
FunctionEnd


;--------------------------------
; GetWindowsVersion
;
; Based on Yazno's function, http://yazno.tripod.com/powerpimpit/
; Updated by Joost Verburg
;
; Returns on top of stack
;
; Windows Version (95, 98, ME, NT x.x, 2000, XP, 2003)
; or
; '' (Unknown Windows Version)
;
; Usage:
;   Call GetWindowsVersion
;   Pop $R0
;   ; at this point $R0 is "NT 4.0" or whatnot

Function GetWindowsVersion

  Push $R0
  Push $R1

  ClearErrors

  ReadRegStr $R0 HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion" CurrentVersion

  IfErrors 0 lbl_winnt

   ; we are not NT
  ReadRegStr $R0 HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion" VersionNumber

  StrCpy $R1 $R0 1
  StrCmp $R1 '4' 0 lbl_error

  StrCpy $R1 $R0 3

  StrCmp $R1 '4.0' lbl_win32_95
  StrCmp $R1 '4.9' lbl_win32_ME lbl_win32_98

  lbl_win32_95:
    StrCpy $R0 '95'
  Goto lbl_done

  lbl_win32_98:
    StrCpy $R0 '98'
  Goto lbl_done

  lbl_win32_ME:
    StrCpy $R0 'ME'
  Goto lbl_done

  lbl_winnt:

  StrCpy $R1 $R0 1

  StrCmp $R1 '3' lbl_winnt_x
  StrCmp $R1 '4' lbl_winnt_x

  StrCpy $R1 $R0 3

  StrCmp $R1 '5.0' lbl_winnt_2000
  StrCmp $R1 '5.1' lbl_winnt_XP
  StrCmp $R1 '5.2' lbl_winnt_2003
  StrCmp $R1 '6.0' lbl_winnt_2008
  StrCmp $R1 '6.1' lbl_winnt_7
  StrCmp $R1 '6.2' lbl_winnt_8
  StrCmp $R1 '6.3' lbl_winnt_8_1 lbl_error
  
  lbl_winnt_x:
    StrCpy $R0 "NT $R0" 6
  Goto lbl_done

  lbl_winnt_2000:
    Strcpy $R0 '2000'
  Goto lbl_done

  lbl_winnt_XP:
    Strcpy $R0 'XP'
  Goto lbl_done

  lbl_winnt_2003:
    Strcpy $R0 '2003'
  Goto lbl_done

  lbl_winnt_2008:
    Strcpy $R0 '2008'
  Goto lbl_done
  
  lbl_winnt_7:
    Strcpy $R0 'win7'
  Goto lbl_done

  lbl_winnt_8:
    Strcpy $R0 'Win8'
  Goto lbl_done
  
  lbl_winnt_8_1:
    Strcpy $R0 'Win8.1'
  Goto lbl_done
  
  lbl_error:
    Strcpy $R0 ''
  lbl_done:

  Pop $R1
  Exch $R0

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


;--------------------------------
; Service account page

Function ServiceAccountPage
  ;Check if install as service is selected
  ${Unless} ${SectionIsSelected} ${DATCOOR_SERVICE}
    Abort
  ${EndUnless}
  !insertmacro MUI_HEADER_TEXT "$(TEXT_SERVICEACCOUNT_TITLE)" "$(TEXT_SERVICEACCOUNT_SUBTITLE)"
  ${If} $ServiceUserName == ""
    !insertmacro MUI_INSTALLOPTIONS_WRITE "ServiceAccount.ini" "Field 2" "State" "1"
    !insertmacro MUI_INSTALLOPTIONS_WRITE "ServiceAccount.ini" "Field 3" "State" "0"
  ${Else}
    !insertmacro MUI_INSTALLOPTIONS_WRITE "ServiceAccount.ini" "Field 2" "State" "0"
    !insertmacro MUI_INSTALLOPTIONS_WRITE "ServiceAccount.ini" "Field 3" "State" "1"
  ${EndIf}
  !insertmacro MUI_INSTALLOPTIONS_WRITE "ServiceAccount.ini" "Field 5" "State" $ServiceUserName
  !insertmacro MUI_INSTALLOPTIONS_WRITE "ServiceAccount.ini" "Field 7" "State" $ServicePassword
  !insertmacro MUI_INSTALLOPTIONS_DISPLAY "ServiceAccount.ini"

FunctionEnd


;--------------------------------
; String resources

LangString TEXT_SERVICEACCOUNT_TITLE      ${LANG_ENGLISH} "Select user account"
LangString TEXT_SERVICEACCOUNT_SUBTITLE   ${LANG_ENGLISH} "Select the user account the ibaDatCoordinator service should use."
LangString DESC_DATCOOR_NOSERVICE         ${LANG_ENGLISH} "ibaDatCoordinator"
LangString DESC_DATCOOR_SERVICE           ${LANG_ENGLISH} "ibaDatCoordinator Service"
LangString TEXT_OS_NOT_SUPPORTED          ${LANG_ENGLISH} "This operating system is not supported."
LangString TEXT_NOT_ADMINISTRATOR         ${LANG_ENGLISH} "You do not have sufficient privileges to complete this installation for all users of the machine.  Log on as an administrator and then retry this installation."
LangString TEXT_FRAMEWORK_MISSING         ${LANG_ENGLISH} "The .NET framework version 2.0 is not installed. Please install this before running the ibaDatCoordinator installer. The .NET framework can be found on the distribution cd (dotnetfx.exe) or it can be downloaded from http://www.microsoft.com"
LangString TEXT_UNINSTALL                 ${LANG_ENGLISH} "Uninstall ${PRODUCT_NAME}"
LangString TEXT_ALREADY_INSTALLED         ${LANG_ENGLISH} "${PRODUCT_NAME} v${PRODUCT_VERSION} is already installed. Do you wish to reinstall?"
LangString TEXT_OLDER_INSTALLED           ${LANG_ENGLISH} "An older version of ${PRODUCT_NAME} is installed. Do you wish to upgrade to version ${PRODUCT_VERSION}?"
LangString TEXT_NEWER_INSTALLED           ${LANG_ENGLISH} "A newer version of ${PRODUCT_NAME} is installed."
LangString TEXT_PREV_UNINSTALLING         ${LANG_ENGLISH} "Removing older version of ${PRODUCT_NAME}"
LangString TEXT_STILL_RUNNING             ${LANG_ENGLISH} "Cannot delete the application you are trying to remove. Perhaps it is still running. If so, please close it and try again."
LangString DESC_REMAINING                 ${LANG_ENGLISH} " (%d %s%s remaining)"
LangString DESC_PROGRESS                  ${LANG_ENGLISH} "%dkB (%d%%) of %dkB @ %d.%01dkB/s" ;"%d.%01dkB/s" ;"%dkB (%d%%) of %dkB @ %d.%01dkB/s"
LangString DESC_PLURAL                    ${LANG_ENGLISH} "s"
LangString DESC_HOUR                      ${LANG_ENGLISH} "hour"
LangString DESC_MINUTE                    ${LANG_ENGLISH} "minute"
LangString DESC_SECOND                    ${LANG_ENGLISH} "second"
LangString DESC_CONNECTING                ${LANG_ENGLISH} "Connecting..."
LangString DESC_DOWNLOADING               ${LANG_ENGLISH} "Downloading %s"
LangString DESC_INSTALLING                ${LANG_ENGLISH} "Installing"
LangString TEXT_SERVICE_INSTALL           ${LANG_ENGLISH} "Installing service"
LangString TEXT_SERVICE_STOP              ${LANG_ENGLISH} "Stopping service"
LangString TEXT_SERVICE_REMOVE            ${LANG_ENGLISH} "Deleting service"
LangString TEXT_SERVICEORSTANDALONE_TITLE ${LANG_ENGLISH} "Choose Installation Type"
LangString TEXT_SERVICEORSTANDALONE_SUBTITLE ${LANG_ENGLISH} "Choose whether or not ibaDatCoordinator is installed as a service"
LangString TEXT_INSTALLSERVICE            ${LANG_ENGLISH} "Install ibaDatCoordinator as a service"
LangString TEXT_INSTALLSTANDALONE         ${LANG_ENGLISH} "Install ibaDatCoordinator as stand alone executable"
LangString TEXT_LOG_FILES                 ${LANG_ENGLISH} "log files"
LangString TEXT_IBAFILES_INSTALL          ${LANG_ENGLISH} "Installing ibaFiles"

LangString TEXT_SERVICEACCOUNT_TITLE      ${LANG_GERMAN}  "Benutzerkonto w�hlen"
LangString TEXT_SERVICEACCOUNT_SUBTITLE   ${LANG_GERMAN}  "W�hlen Sie das Benutzerkonto f�r den Server-Dienst aus."
LangString DESC_DATCOOR_NOSERVICE         ${LANG_GERMAN} "ibaDatCoordinator"
LangString DESC_DATCOOR_SERVICE           ${LANG_GERMAN} "ibaDatCoordinator Dienst"
LangString TEXT_OS_NOT_SUPPORTED          ${LANG_GERMAN} "Das Betriebssystem ist nicht unterst�tzt."
LangString TEXT_NOT_ADMINISTRATOR         ${LANG_GERMAN} "Sie besitzen keine ausreichenden Berechtigungen, um diese Installation f�r alle Benutzer dieses Computers auszuf�hren. Melden Sie sich als Administrator an, und wiederholen Sie diese Installation."
LangString TEXT_FRAMEWORK_MISSING         ${LANG_GERMAN} "Das .NET Framework, Version 2.0, ist nicht installiert. Bitte installieren Sie dies zun�chst, bevor Sie mit der Installation von ibaDatCoordinator beginnen. Das .NET Framework finden Sie auf der Programm-CD (dotnetfx.exe) oder als Download via http://www.microsoft.com."
LangString TEXT_UNINSTALL                 ${LANG_GERMAN} "${PRODUCT_NAME} deinstallieren"
LangString TEXT_ALREADY_INSTALLED         ${LANG_GERMAN} "${PRODUCT_NAME} v${PRODUCT_VERSION} ist bereits installiert. Wollen Sie die Neuinstallation trotzdem durchf�hren?"
LangString TEXT_OLDER_INSTALLED           ${LANG_GERMAN} "Eine �ltere Version von ${PRODUCT_NAME} ist bereits installiert. Wollen Sie ein upgrade auf die neuere Version ${PRODUCT_VERSION} durchf�hren?"
LangString TEXT_NEWER_INSTALLED           ${LANG_GERMAN} "Eine neuere Version von ${PRODUCT_NAME} ist bereits installiert."
LangString TEXT_PREV_UNINSTALLING         ${LANG_GERMAN} "�ltere Version von ${PRODUCT_NAME} wird entfernt..."
LangString TEXT_STILL_RUNNING             ${LANG_GERMAN} "Die Anwendung, die Sie entfernen wollen kann nicht gel�scht werden. Eventuell wird sie noch ausgef�hrt. Wenn dem so ist, beenden Sie bitte die Anwendung und versuchen Sie es erneut."
LangString DESC_REMAINING                 ${LANG_GERMAN} " (%d %s%s)"
LangString DESC_PROGRESS                  ${LANG_GERMAN} "%dkB (%d%%) of %dkB @ %d.%01dkB/s" ;"%d.%01dkB/s" ;"%dkB (%d%%) of %dkB @ %d.%01dkB/s"
LangString DESC_PLURAL                    ${LANG_GERMAN} "n"
LangString DESC_HOUR                      ${LANG_GERMAN} "Stunde"
LangString DESC_MINUTE                    ${LANG_GERMAN} "Minute"
LangString DESC_SECOND                    ${LANG_GERMAN} "Sekunde"
LangString DESC_CONNECTING                ${LANG_GERMAN} "Verbinden..."
LangString DESC_DOWNLOADING               ${LANG_GERMAN} "%s wird heruntergeladen"
LangString DESC_INSTALLING                ${LANG_GERMAN} "wird installiert"
LangString TEXT_SERVICE_INSTALL           ${LANG_GERMAN} "Dienst wird installiert"
LangString TEXT_SERVICE_STOP              ${LANG_GERMAN} "Dienst wird angehalten"
LangString TEXT_SERVICE_REMOVE            ${LANG_GERMAN} "Dienst wird gel�scht"
LangString TEXT_SERVICEORSTANDALONE_TITLE ${LANG_GERMAN} "W�hlen Sie die Installationsart"
LangString TEXT_SERVICEORSTANDALONE_SUBTITLE ${LANG_GERMAN} "W�hlen Sie ob der ibaDatCoordinator als Dienst installiert werden soll"
LangString TEXT_INSTALLSERVICE            ${LANG_GERMAN} "ibaDatCoordinator als Dienst installieren"
LangString TEXT_INSTALLSTANDALONE         ${LANG_GERMAN} "ibaDatCoordinator nur als Programm zu installieren"
LangString TEXT_LOG_FILES                 ${LANG_GERMAN} "Log Dateien"
LangString TEXT_IBAFILES_INSTALL          ${LANG_GERMAN}  "ibaFiles wird installiert"

LangString TEXT_SERVICEACCOUNT_TITLE      ${LANG_FRENCH}  "Choisir le compte d'utilisateur"
LangString TEXT_SERVICEACCOUNT_SUBTITLE   ${LANG_FRENCH}  "Choisir le compte d'utilisateur employ� par le service de serveur."
LangString DESC_DATCOOR_NOSERVICE         ${LANG_FRENCH} "ibaDatCoordinator"
LangString DESC_DATCOOR_SERVICE           ${LANG_FRENCH} "Service ibaDatCoordinator"
LangString TEXT_OS_NOT_SUPPORTED          ${LANG_FRENCH} "Le syst�me d'exploitation n'est pas support?"
LangString TEXT_NOT_ADMINISTRATOR         ${LANG_FRENCH} "Vous n'avez pas assez de privil�ges pour effectuer cette installation pour tous les utilisateurs de cet ordinateur. Connectez-vous en tant qu'administrateur et r�essayez cette installation."
LangString TEXT_FRAMEWORK_MISSING         ${LANG_FRENCH} "Le .NET framework, version 2.0, n'est pas install? Installez-le avant de commencer l'installation d'ibaDatCoordinator. Le .NET framework est sur le CD programme (dotnetfx.exe) ou vous pouvez le t�l�charger via http://www.microsoft.com."
LangString TEXT_UNINSTALL                 ${LANG_FRENCH} "D�sinstaller ${PRODUCT_NAME}"
LangString TEXT_ALREADY_INSTALLED         ${LANG_FRENCH} "${PRODUCT_NAME} v${PRODUCT_VERSION} est d�j?install? Souhaitez-vous proc�der ?une r�installation?"
LangString TEXT_OLDER_INSTALLED           ${LANG_FRENCH} "Une ancienne version de ${PRODUCT_NAME} est d�j?install�e. Souhaitez-vous une mise ?niveau ?la nouvelle version ${PRODUCT_VERSION}?"
LangString TEXT_NEWER_INSTALLED           ${LANG_FRENCH} "Une version plus r�cente de ${PRODUCT_NAME} est d�j?install�e."
LangString TEXT_PREV_UNINSTALLING         ${LANG_FRENCH} "Suppression de l'ancienne version de ${PRODUCT_NAME}."
LangString TEXT_STILL_RUNNING             ${LANG_FRENCH} "L'application que vous voulez supprimer ne peut pas �tre effac�e. Elle est �ventuellement en cours d'ex�cution. Dans ce cas, fermez l'application et r�essayez."
LangString DESC_REMAINING                 ${LANG_FRENCH} " (%d %s%s restant)"
LangString DESC_PROGRESS                  ${LANG_FRENCH} "%dkB (%d%%) of %dkB @ %d.%01dkB/s" ;"%d.%01dkB/s" ;"%dkB (%d%%) of %dkB @ %d.%01dkB/s"
LangString DESC_PLURAL                    ${LANG_FRENCH} "s"
LangString DESC_HOUR                      ${LANG_FRENCH} "Heure"
LangString DESC_MINUTE                    ${LANG_FRENCH} "Minute"
LangString DESC_SECOND                    ${LANG_FRENCH} "Seconde"
LangString DESC_CONNECTING                ${LANG_FRENCH} "Connexion..."
LangString DESC_DOWNLOADING               ${LANG_FRENCH} "T�l�chargement de %s"
LangString DESC_INSTALLING                ${LANG_FRENCH} "Installation"
LangString TEXT_SERVICE_INSTALL           ${LANG_FRENCH} "Installation du service"
LangString TEXT_SERVICE_STOP              ${LANG_FRENCH} "Arr�t du service"
LangString TEXT_SERVICE_REMOVE            ${LANG_FRENCH} "Suppression du service"
LangString TEXT_SERVICEORSTANDALONE_TITLE ${LANG_FRENCH} "Choisir Type d'Installlation"
LangString TEXT_SERVICEORSTANDALONE_SUBTITLE ${LANG_FRENCH} "Choisir si l'ibaDatCoordinator est install?comme service"
LangString TEXT_INSTALLSERVICE            ${LANG_FRENCH} "Installer  l'ibaDatCoordinator comme service"
LangString TEXT_INSTALLSTANDALONE         ${LANG_FRENCH} "Installer l'ibaDatCoordinator comme ex�cutable autonome"
LangString TEXT_LOG_FILES                 ${LANG_FRENCH} "fichiers log"
LangString TEXT_IBAFILES_INSTALL          ${LANG_FRENCH}  "Installation de ibaFiles"