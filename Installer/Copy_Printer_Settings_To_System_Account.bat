@echo off

::copy all the printer settings from the current user to the system account
reg copy "HKEY_CURRENT_USER\Printers" "HKEY_USERS\.DEFAULT\Printers" /s /f
reg copy "HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Windows" "HKEY_USERS\.DEFAULT\Software\Microsoft\Windows NT\CurrentVersion\Windows" /f
reg copy "HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Devices" "HKEY_USERS\.DEFAULT\Software\Microsoft\Windows NT\CurrentVersion\Devices" /f
reg copy "HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\PrinterPorts" "HKEY_USERS\.DEFAULT\Software\Microsoft\Windows NT\CurrentVersion\PrinterPorts" /f


@echo on