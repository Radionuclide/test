; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "ibaDatCoordinator ArcelorMittal OSPC Plugin"  ;will be also used as output file (blanks will be replaced by dashes)
                                                                    ;e.g. "ibaDatCoordinator-ArcelorMittal-OSPC-PluginInstaller_vX.Y.Z.exe"
!define PRODUCT_SHORTNAME "AM-OSPC" ;(optional) will result in "ibaDatCoordinator-${PRODUCT_SHORTNAME}-PluginInstaller_vX.Y.Z.exe"
                                    ;e.g. "ibaDatCoordinator-AM-OSPC-PluginInstaller_vX.Y.Z.exe"
!define PRODUCT_VERSION "2.4.0 BETA13"
!define PRODUCT_FILE_VERSION "2.4.0.0"

;!define PRODUCT_PUBLISHER "iba AG"                     ;(optional) default to "iba AG"
;!define PRODUCT_WEB_SITE "http://www.iba-ag.com"       ;(optional) default to "http://www.iba-ag.com"

!include "PluginInstaller.nsh"
 
Function CopyFiles
  ClearErrors
  DetailPrint "Copying plugin files"
  SetOutPath "$PluginPath"
  File "..\Dependencies\msvcr100.dll"
  File "..\Dependencies\msvcp100.dll"
  Delete "$PluginPath\Sidmar-OSPC-Plugin.dll" ;remove old version of plugin with different name
  File "..\AM-OSPC-plugin\bin\Release\AM-OSPC-plugin.dll"
  File "..\AM-OSPC-plugin\bin\Release\AM-OSPC-protocol.dll"
  File "..\AM-OSPC-protocol\SPCServerps.dll"
  IfErrors copyError copyOk
copyError:
  Push "error"
  Return
copyOk:
  RegDLL "$PluginPath\SPCServerps.dll"
  Push "success"
FunctionEnd
