; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "ibaDatCoordinator S7 Writer Plugin"

!define PRODUCT_VERSION "2.1.1"
!define PRODUCT_FILE_VERSION "2.1.1.0"

!include "PluginInstaller.nsh"
 
Function CopyFiles
  ClearErrors
  DetailPrint "Copying plugin files"

  SetOutPath "$ibaDatCoordinatorPath"
  File "..\S7-writer\AGLink\AGLink40.dll"

  SetOutPath "$PluginPath"
  File "..\S7-writer-plugin\bin\Release\S7-writer.dll"
  File "..\S7-writer-plugin\bin\Release\S7-writer-plugin.dll"

  IfErrors copyError copyOk
copyError:
  Push "error"
  Return
copyOk:
  Push "success"
FunctionEnd