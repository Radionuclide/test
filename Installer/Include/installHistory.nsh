!include "LogicLib.nsh"

!define InstallHistoryFileName "$INSTDIR\..\InstallHistory.txt"

!macro WriteToInstallHistory MSG
  !define UID2 ${__LINE__}

  ;Prepare time variables
  Push $0 ;Second
  Push $1 ;Minute
  Push $2 ;Hour
  Push $3 ;Day of week
  Push $4 ;Year
  Push $5 ;Month
  Push $6 ;Day

  ;Call GetLocalTime API from Kernel32.dll
  System::Call '*(&i2, &i2, &i2, &i2, &i2, &i2, &i2, &i2) i .r0'
  System::Call 'kernel32::GetLocalTime(i) i(r0)'
  System::Call '*$0(&i2, &i2, &i2, &i2, &i2, &i2, &i2, &i2)i \
  (.r4, .r5, .r3, .r6, .r2, .r1, .r0,)'

  ;Month: convert to 2 digits format
  IntCmp $5 9 0 0 +2
  StrCpy $5 '0$5'

  ;Day: convert to 2 digits format
  IntCmp $6 9 0 0 +2
  StrCpy $6 '0$6'
  
  ;Hour: convert to 2 digits format
  IntCmp $2 9 0 0 +2
  StrCpy $2 '0$2'

  ;Minute: convert to 2 digits format
  IntCmp $1 9 0 0 +2
  StrCpy $1 '0$1'

  ;Second: convert to 2 digits format
  IntCmp $0 9 0 0 +2
  StrCpy $0 '0$0'

  ;Save register $R0 because it will be used as file handle
  Push $R0
  
  ;Open file in mode append
  FileOpen $R0 "${InstallHistoryFileName}" a
  ${If} $R0 == ""
    Goto end_${UID2}
  ${EndIf}
  
  ;Seek to end of file
  FileSeek $R0 0 END
  
  ;Write message to file
  FileWrite $R0 "$4-$5-$6 $2:$1:$0 : ${MSG}$\r$\n"
  
  ;Close file
  FileClose $R0

  end_${UID2}:
  
  ;Restore register $R0
  Pop $R0
  
  ;Restore time variables
  Pop $6
  Pop $5
  Pop $4
  Pop $3
  Pop $2
  Pop $1
  Pop $0
  
  !undef UID2
!macroend
