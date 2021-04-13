; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "ibaDatCoordinator TKS Xml Plugin"

!define PRODUCT_VERSION "2.4.0"
!define PRODUCT_FILE_VERSION "2.4.0.0"

!define DATCO_MIN 2040000
!define DATCO_MIN_STR "2.4.0"

!include "PluginInstaller.nsh"

Function CopyFiles
  ClearErrors
  DetailPrint "Copying plugin files"
  SetOutPath "$PluginPath"
  File "..\TKS-XML-plugin\bin\Release\TKS-XML-plugin.dll"
  IfErrors copyError copyOk
copyError:
  Push "error"
  Return
copyOk:
  Push "success"
FunctionEnd
