!include "LogicLib.nsh"

!macro CheckV14Version
  !define UniqueID ${__LINE__}  
  ReadRegStr $0 "HKLM" "SOFTWARE\Microsoft\DevDiv\vc\Servicing\14.0\RuntimeMinimum" "Version"		
  StrCmp $0 "" noversion_${UniqueID}  
      
  Push $1
  Push $2
  Push $3
  Push $4
  Push $5      
	
  StrCpy $3 "-1"
  StrCpy $2 ""
  StrCpy $4 "10000000" ; version is form of 14.0.24212
  StrCpy $5 "0"
  
  charloop_${UniqueID}:
  IntOp $3 $3 + 1
  StrCpy $1 $0 1 $3
  StrCmp $1 "." sepfound_${UniqueID}
  StrCmp $1 "" sepfound_${UniqueID}
  StrCmp $1 " " sepfound_${UniqueID}
  StrCpy $2 $2$1
  Goto charloop_${UniqueID}
  
  sepfound_${UniqueID}:
  IntOp $2 $2 * $4
  IntOp $4 $4 / 100
  IntCmp $4 1000 +1 +1 +2
  StrCpy $4 "1"
  IntOp $5 $5 + $2
  StrCpy $2 ""
  StrCmp $1 "." charloop_${UniqueID}
  
  ; check if minimal version is already installed
  IntCmp $5 140024212 installed_${UniqueID} notinstalled_${UniqueID} installed_${UniqueID}
  
  installed_${UniqueID}:
    Push 1
    Goto end_${UniqueID}
  notinstalled_${UniqueID}:
    Push 0
	
  end_${UniqueID}:
   
  Exch 5
  Pop $1
  Pop $5 
  Pop $4
  Pop $3
  Pop $2       
  Goto quit_${UniqueID}
  
  noversion_${UniqueID}:  
  Push 0
  
  quit_${UniqueID}:
  !undef UniqueID
!macroend

Function SearchLockCb
  Pop $R4 ; process id
  Pop $R5 ; file path (currently ignored)
  Pop $R5 ; description
  
  ; If no description is provided, use the process ID
  ${If} $R5 == ""
    StrCpy $R5 "ID $R4"
  ${EndIf}
  
  ; Add process name to list
  StrCpy $R6 "$R6$\r$\n$R5"

  ; Add process ID to list (so we can kill it later)
  ${If} $R7 == ""
    StrCpy $R7 $R4
  ${Else}
    StrCpy $R7 "$R7,$R4"
  ${EndIf}
  
  Push true ; continue enumeration
FunctionEnd

!macro CleanupIbaSystemFiles_archindep
  ; Check if file was found
  IfErrors cleanup_${UniqueID} 0
  
  ; Check if installed version is older than the one in our installer
  ; If files need to be renewed, try to delete them and see who's holding a lock
  ; Ask user to close application(s) holding the lock
  IntCmp $R1 $R3 0 cleanup_${UniqueID} del_files_${UniqueID} 
  IntCmp $R2 $R4 cleanup_${UniqueID} cleanup_${UniqueID} del_files_${UniqueID}
  
  ; Try to delete old files
  del_files_${UniqueID}:
    ClearErrors
    FindFirst $R2 $R1 "$R0\*.dll" ; $R2: search handle; $R1: filename (w/o full path)
    IfErrors cleanup_${UniqueID} 0 ; Error means no files found

    del_loop_nextfile_${UniqueID}:
      ; Try to delete file and check for errors
      Delete "$R0\$R1"
      IfErrors del_error_${UniqueID} 0 ; Error means file could not be deleted

      ; Get next file if available
      FindNext $R2 $R1 ; $R2: search handle; $R1: filename (w/o full path)
      IfErrors del_files_end_${UniqueID} del_loop_nextfile_${UniqueID}

    del_error_${UniqueID}:
      nsSCMEx::AddModule "$R0\$R1"
      GetFunctionAddress $R3 SearchLockCb
      StrCpy $R6 "" ; Clear register in which we will write list of applications
      StrCpy $R7 "" ; Clear register in which we will write list of process IDs
      nsSCMEx::SilentSearch $R3 ; Do silent search for processes holding lock on file that could not be deleted
      Pop $R4 ; Result of silent search (currently ignored)

      ; Ask user what to do with blocking process
      MessageBox MB_ABORTRETRYIGNORE "$(TEXT_IBASYSTEMFILES_LOCKED_0)$\r$\n$R6$\r$\n$\r$\n$(TEXT_IBASYSTEMFILES_LOCKED_1)" /SD IDABORT IDABORT del_loop_abort_${UniqueID} IDRETRY del_loop_retry_${UniqueID} ;IDIGNORE del_loop_ignore_${UniqueID}
      Goto del_loop_ignore_${UniqueID} ; Since we can't supply a goto for IDIGNORE

    del_loop_abort_${UniqueID}:
      Abort
    del_loop_ignore_${UniqueID}:
      nsSCMEx::KillProcesses $R7
    del_loop_retry_${UniqueID}:
      ; Close current search handle
      FindClose $R2

      ; Try again now that application(s) are closed (or should be)
      Goto del_files_${UniqueID}
  del_files_end_${UniqueID}:
    ; No more files; close search handle
    FindClose $R2
  
  cleanup_${UniqueID}:
    ClearErrors
    Pop $R7
    Pop $R6
    Pop $R5
    Pop $R4
    Pop $R3
    Pop $R2
    Pop $R1
    Pop $R0
    !undef UniqueID
!macroend

!macro CleanupIbaSystemFiles_x86
  !define UniqueID ${__LINE__}
  
  ; Save registers to stack
  Push $R0
  Push $R1
  Push $R2
  Push $R3
  Push $R4
  Push $R5
  Push $R6
  Push $R7

  ; Check version of one file (e.g. ucrtbase.dll)
  StrCpy $R0 "$PROGRAMFILES\iba\System"
  GetDLLVersionLocal "..\InstallFiles\CRT\x86\ucrtbase.dll" $R1 $R2
  ClearErrors
  GetDLLVersion "$R0\ucrtbase.dll" $R3 $R4
  
  !insertmacro CleanupIbaSystemFiles_archindep

!macroend

!macro CleanupIbaSystemFiles_x64
  !define UniqueID ${__LINE__}
  
  ; Save registers to stack
  Push $R0
  Push $R1
  Push $R2
  Push $R3
  Push $R4
  Push $R5
  Push $R6
  Push $R7

  ; Check version of one file (e.g. ucrtbase.dll)
  StrCpy $R0 "$PROGRAMFILES64\iba\System"
  GetDLLVersionLocal "..\InstallFiles\CRT\x64\ucrtbase.dll" $R1 $R2
  ClearErrors
  GetDLLVersion "$R0\ucrtbase.dll" $R3 $R4

  ; 64-bit version of plugin to search for locking processes
  InitPluginsDir
  File /oname=$PLUGINSDIR\LockedList64.dll "..\InstallFiles\LockedList64.dll"  	
  
  !insertmacro CleanupIbaSystemFiles_archindep
  
!macroend

!macro InstallCRT14_x86
  !define UID ${__LINE__}  
  Push $0

  !insertmacro CheckV14Version
  Pop $0
  ${If} $0 == "0"
    ; Check if we installed System files on previous occasion
    !insertmacro CleanupIbaSystemFiles_x86

    StrCpy $0 $OUTDIR
    Push $1
    StrCpy $1 "$PROGRAMFILES\iba\System"
    SetOutPath $1
    SetOverwrite off ; Files have already been deleted by CleanupIbaSystemFiles if necessary
    File "..\InstallFiles\CRT\x86\*.dll"
    SetOverwrite lastused
    SetOutPath $0

    nsSCMEx::ModifyEnvVar /NOUNLOAD "add" "exp" "Path" "$1"
    Pop $1 ; the return value of ModifyEnvVar
    StrCmp $1 "success" pathOk_${UID} pathNotOk_${UID}
  ${EndIf}
 
  Goto exitOk_${UID}
  
  pathOk_${UID}:  
  Pop $1
  Goto exitOk_${UID}
  
  pathNotOk_${UID}:  
  Pop $1
  Goto exitNotOk_${UID}
  
  exitOk_${UID}:
  Pop $0
  Push 1
  Goto exit_${UID}
  
  exitNotOk_${UID}:  
  Pop $0 
  Push 0
  
  exit_${UID}:
  !undef UID
!macroend

!macro InstallCRT14_x64
  !define UID ${__LINE__}
  Push $0

  SetRegView 64
  !insertmacro CheckV14Version
  Pop $0
  ${If} $0 == "0"
    ; Check if we installed System files on previous occasion
    !insertmacro CleanupIbaSystemFiles_x64

    StrCpy $0 $OUTDIR
    Push $1
    StrCpy $1 "$PROGRAMFILES64\iba\System"
    SetOutPath $1
    SetOverwrite off ; Files have already been deleted by CleanupIbaSystemFiles if necessary
    File "..\InstallFiles\CRT\x64\*.dll"
    SetOverwrite lastused
    SetOutPath $0

    nsSCMEx::ModifyEnvVar /NOUNLOAD "add" "exp" "Path" "$1"
    Pop $1 ; the return value of ModifyEnvVar
    StrCmp $1 "success" pathOk_${UID} pathNotOk_${UID}
  ${EndIf}

  Goto exitOk_${UID}

  pathOk_${UID}:
  Pop $1
  Goto exitOk_${UID}

  pathNotOk_${UID}:
  Pop $1
  Goto exitNotOk_${UID}

  exitOk_${UID}:
  Pop $0
  Push 1
  Goto exit_${UID}

  exitNotOk_${UID}:
  Pop $0
  Push 0

  exit_${UID}:
  SetRegView lastused
  !undef UID
!macroend

LangString TEXT_IBASYSTEMFILES_LOCKED_0  ${LANG_ENGLISH} "The following applications need to be closed before installation can proceed:"
LangString TEXT_IBASYSTEMFILES_LOCKED_1  ${LANG_ENGLISH} "Clicking Ignore will force the applications to exit"
LangString TEXT_IBASYSTEMFILES_LOCKED_0  ${LANG_GERMAN}  "The following applications need to be closed before installation can proceed:"
LangString TEXT_IBASYSTEMFILES_LOCKED_1  ${LANG_GERMAN}  "Clicking Ignore will force the applications to exit"
LangString TEXT_IBASYSTEMFILES_LOCKED_0  ${LANG_FRENCH}  "The following applications need to be closed before installation can proceed:"
LangString TEXT_IBASYSTEMFILES_LOCKED_1  ${LANG_FRENCH}  "Clicking Ignore will force the applications to exit"