; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "ibaDatCoordinator ArcelorMittal OSPC Plugin Installer"
!define PRODUCT_SHORTNAME "AM-OSPC"
!define PRODUCT_VERSION "2.0.0"
!define PRODUCT_FILE_VERSION "2.0.0.0"
!define PRODUCT_PUBLISHER "iba AG"
!define PRODUCT_WEB_SITE "http://www.iba-ag.com"

!include "PluginInstaller.nsh"
 
Function CopyFiles
  ClearErrors
  DetailPrint "Copying plugin files"
  SetOutPath "$PluginPath"
  File "..\Dependencies\msvcr100.dll"
  File "..\Dependencies\msvcp100.dll"
  File "..\Sidmar-OSPC-plugin\bin\Release\Sidmar-OSPC-plugin.dll"
  File "..\Sidmar-OSPC-plugin\bin\Release\AM-OSPC-protocol.dll"
  File "..\AM-OSPC-protocol\SPCServerps.dll"
  IfErrors copyError copyOk
copyError:
  Push "error"
  Return
copyOk:
  RegDLL "$PluginPath\SPCServerps.dll"
  Push "success"
FunctionEnd
