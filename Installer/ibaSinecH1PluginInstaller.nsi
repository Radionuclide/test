; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "ibaDatCoordinator Sinec-H1 Plugin"

!define PRODUCT_VERSION "2.3.0"
!define PRODUCT_FILE_VERSION "2.3.0.0"

!include "PluginInstaller.nsh"
 
Function CopyFiles
  ClearErrors
  DetailPrint "Copying plugin files"
  SetOutPath "$PluginPath"
  File "..\Alunorf-sinec-h1-plugin\bin\Release\Alunorf-sinec-h1-plugin.dll"
  ;File "..\Alunorf-sinec-h1-plugin\bin\Release\DatCoordinatorPlugins.dll"
  File "..\Alunorf-sinec-h1-plugin\bin\Release\H1Protocol.dll"
  File "..\Dependencies\Wmknt.dll"
  File "..\Dependencies\msvcr100.dll"
  File "..\Dependencies\msvcp100.dll"
  SetOutPath "$PluginPath\de"
  File "..\Alunorf-sinec-h1-plugin\bin\Release\de\Alunorf-sinec-h1-plugin.resources.dll"
  ;not copying ibaFiles as this should already be included with the datcoordinator
  ;copy H1 driver
  SetOutPath "$PluginPath\H1Driver"
  File "..\INAT\H1Prot.inf"
  File "..\INAT\H1Prot.sys"
  File "..\INAT\viewnetworks.cmd"
  IfErrors copyError copyOk
copyError:
  Push "error"
  Return
copyOk:
  RegDLL "$PluginPath\SPCServerps.dll"
  Exec '$PluginPath\H1Driver\viewnetworks.cmd'
  Sleep 3000
  MessageBox MB_OK $(DESC_H1INSTALL)
  Push "success"
FunctionEnd

LangString DESC_H1INSTALL  ${LANG_ENGLISH} "Installation of the PC-H1 protocol driver:$\r$\n$\t- Open the network settings in your system$\r$\n$\t- Choose minimal one Local Area Connection$\r$\n$\t- Press Properties$\r$\n$\t- If installed, disable 'Qos Packet Scheduler'$\r$\n$\t- Press Install$\r$\n$\t- Add a (new) protocol$\r$\n$\t- Press the button 'Have Disc'$\r$\n$\t- You will find the drivers under$\r$\n$\t$\t $PluginPath\H1Driver$\r$\n$\t- Select the *.inf file$\r$\n$\t- Confirm with 'Open' and 'OK'$\r$\n$\t- Select 'INAT H1 Iso Protocol'$\r$\n$\t- Confirm with 'OK'$\r$\n$\t- The PC-H1 protocol driver is linked now to ALL of your network cards, disable the INAT H1 Iso Protocol on the cards that shouldn't have the protocol enabled.$\r$\n$\t- Close all network windows$\r$\n$\r$\nAttention:$\r$\n$\tWhen the PC-H1 protocol driver is installed, the PC has to be rebooted"
LangString DESC_H1INSTALL  ${LANG_GERMAN} "Installation des PC-H1 Protokoll Treibers:$\r$\n$\t- �ber die Systemsteuerung die Netzwerkverbindungen �ffnen$\r$\n$\t- W�hlen Sie mindestens eine LAN Verbindung aus$\r$\n$\t- Eigenschaften$\r$\n$\t- Wenn 'Qos-Paketplaner' installiert ist, bitte deaktivieren$\r$\n$\t- Installieren$\r$\n$\t- 'Protokoll hinzuf�gen' ausw�hlen$\r$\n$\t- W�hlen Sie 'Datentr�ger'$\r$\n$\t- �ber 'Durchsuchen' folgenden Pfad ausw�hlen:$\r$\n$\t$\t $PluginPath\H1Driver$\r$\n$\t- Die *.inf Datei anw�hlen$\r$\n$\t- Mit '�ffnen' und 'OK' best�tigen$\r$\n$\t- 'INAT H1 Iso Protocol' ausw�hlen$\r$\n$\t- Mit 'OK' best�tigen$\r$\n$\t- Der PC-H1 Protokolltreiber ist nun mit ALLEN Ihren Netzwerkkarten verbunden. Deaktivieren Sie das INAT H1 Iso Protokoll auf den Karten, auf denen es nicht aktiv sein soll.$\r$\n$\t- Alle Netzwerkeinstellungen schliessen$\r$\n$\r$\nBitte beachten Sie:$\r$\n$\tNach der Installation des PC-H1 Protokoll Treibers muss der PC neu gestartet werden"
