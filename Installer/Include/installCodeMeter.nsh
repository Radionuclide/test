Var CodeMeterMajor
Var CodeMeterMinor
Var CodeMeterBuild
Var EmbeddedCodeMeterMajor
Var EmbeddedCodeMeterMinor
Var EmbeddedCodeMeterBuild

!macro RunCodeMeterInstaller CODEMETER_INSTALLER_PATH
  !define UniqueID ${__LINE__}
  
  Push $R2
  Push $R3
    
  nsSCMEx::ReadCodeMeterVersion /NOUNLOAD
  Pop $CodeMeterMajor ;Installed major version
  Pop $CodeMeterMinor ;Installed minor version
  Pop $CodeMeterBuild ;Installed build version

  DetailPrint "Installed CodeMeter Runtime version $CodeMeterMajor.$CodeMeterMinor.$CodeMeterBuild"
  GetDLLVersion ${CODEMETER_INSTALLER_PATH} $R2 $R3
  IntOp $EmbeddedCodeMeterMajor $R2 / 0x00010000 ;Local major version
  IntOp $EmbeddedCodeMeterMinor $R2 & 0x0000FFFF ;Local minor version
  IntOp $EmbeddedCodeMeterBuild $R3 / 0x00010000 ;Local build version
  DetailPrint "Embedded CodeMeter Runtime version $EmbeddedCodeMeterMajor.$EmbeddedCodeMeterMinor.$EmbeddedCodeMeterBuild"

  ;Compare installed version with embedded version and install when embedded version is higher
  IntCmp $EmbeddedCodeMeterMajor $CodeMeterMajor  0 skipCodeMeterInstall_${UniqueID} installCodeMeter_${UniqueID}
  IntCmp $EmbeddedCodeMeterMinor $CodeMeterMinor  0 skipCodeMeterInstall_${UniqueID} installCodeMeter_${UniqueID}
  IntCmp $EmbeddedCodeMeterBuild $CodeMeterBuild  skipCodeMeterInstall_${UniqueID} skipCodeMeterInstall_${UniqueID} installCodeMeter_${UniqueID}

  ;Install CodeMeter runtime
  installCodeMeter_${UniqueID}:
  DetailPrint $(TEXT_INSTALL_CODEMETER)
  ${If} $CodeMeterMajor == 0
  ${AndIf} $CodeMeterMinor == 0
  ${AndIf} $CodeMeterBuild == 0
    ;Fresh install
    !insertmacro WriteToInstallHistory "Installing CodeMeter Runtime v$EmbeddedCodeMeterMajor.$EmbeddedCodeMeterMinor.$EmbeddedCodeMeterBuild because there is no CodeMeter Runtime installed."

    ;This will disable the automatic search.
    ;You have to write 1 instead of 0 to disable the automatic search. This is a bug in CodeMeter
    ${If} $Is64Bit == 1
      SetRegView 64
    ${EndIf}
    WriteRegDWORD HKLM "SOFTWARE\WIBU-SYSTEMS\CodeMeter\Server\CurrentVersion\ServerSearchList" "UseBroadcast" 1
    ${If} $Is64Bit == 1
      SetRegView lastused
    ${EndIf}
  ${Else}
    ;If silent install then skip updating CodeMeter runtime because we don't want it to stop any other iba services
    IfSilent skipUpdateBecauseSilent_${UniqueID}

    ;Update of CodeMeter runtime
    !insertmacro WriteToInstallHistory "Updating CodeMeter Runtime v$CodeMeterMajor.$CodeMeterMinor.$CodeMeterBuild to v$EmbeddedCodeMeterMajor.$EmbeddedCodeMeterMinor.$EmbeddedCodeMeterBuild"
  ${EndIf}
  IfSilent installCodeMeterSilent_${UniqueID} installCodeMeterAlmostSilent_${UniqueID}

  installCodeMeterAlmostSilent_${UniqueID}:
  nsExec::Exec '"${CODEMETER_INSTALLER_PATH}" /i /q /nosplash /ComponentArgs "*:/qb /norestart"'
  Goto checkCodeMeterExitCode_${UniqueID}

  installCodeMeterSilent_${UniqueID}:
  nsExec::Exec '"${CODEMETER_INSTALLER_PATH}" /i /q /nosplash /ComponentArgs "*:/q /norestart"'
  Goto checkCodeMeterExitCode_${UniqueID}
  
  checkCodeMeterExitCode_${UniqueID}:
  Pop $R2
  !insertmacro WriteToInstallHistory "CodeMeter Runtime installer exited with error code $R2"
  ${If} $R2 == 3010
  ${OrIf} $R2 == 1306
    !insertmacro WriteToInstallHistory "CodeMeter Runtime installer requires reboot"
    SetRebootFlag true
  ${EndIf}
  Goto installCodeMeterFinished_${UniqueID}
  
  skipUpdateBecauseSilent_${UniqueID}:
  !insertmacro WriteToInstallHistory "Skipping CodeMeter Runtime update from v$CodeMeterMajor.$CodeMeterMinor.$CodeMeterBuild to v$EmbeddedCodeMeterMajor.$EmbeddedCodeMeterMinor.$EmbeddedCodeMeterBuild because the installer is running in silent mode"
  Goto skipCodeMeterInstall_${UniqueID}

  installCodeMeterFinished_${UniqueID}:
  !insertmacro WriteToInstallHistory "Finished installing CodeMeter Runtime"

  skipCodeMeterInstall_${UniqueID}:

  !undef UniqueID
  ClearErrors
  
  Pop $R3
  Pop $R2
!macroend

LangString TEXT_INSTALL_CODEMETER         ${LANG_ENGLISH} "Installing CodeMeter Runtime"
LangString TEXT_INSTALL_CODEMETER         ${LANG_GERMAN}  "CodeMeter Runtime wird installiert"
LangString TEXT_INSTALL_CODEMETER         ${LANG_FRENCH}  "Installation du CodeMeter Runtime"
