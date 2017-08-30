; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "ibaDatCoordinator TKS Xml Plugin Installer"
!define PRODUCT_SHORTNAME "TksXml"

!define PRODUCT_VERSION "2.0.0"
!define PRODUCT_FILE_VERSION "2.0.0.0"
!define PRODUCT_PUBLISHER "iba AG"
!define PRODUCT_WEB_SITE "http://www.iba-ag.com"

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