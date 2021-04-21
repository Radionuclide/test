@echo off

IF EXIST undoprintregistrychanges.reg exit /B 0

::create intermediate reg dumps
copy NUL .\dump_keyPrinters.reg
reg export "HKEY_USERS\.DEFAULT\Printers" .\dump_keyPrinters.reg /y
copy NUL .\dump_keyWindows.reg
reg export "HKEY_USERS\.DEFAULT\Software\Microsoft\Windows NT\CurrentVersion\Windows" .\dump_keyWindows.reg /y
copy NUL .\dump_keyDevices.reg
reg export "HKEY_USERS\.DEFAULT\Software\Microsoft\Windows NT\CurrentVersion\Devices" .\dump_keyDevices.reg /y
copy NUL .\dump_keyPrinterPorts.reg
reg export "HKEY_USERS\.DEFAULT\Software\Microsoft\Windows NT\CurrentVersion\PrinterPorts" .\dump_keyPrinterPorts.reg /y

::Concatenate reg dumps
copy dump_keyPrinters.reg+dump_keyWindows.reg+dump_keyDevices.reg+dump_keyPrinterPorts.reg undoprintregistrychanges.reg

::delete intermediate reg dums
del dump_keyPrinters.reg dump_keyWindows.reg dump_keyDevices.reg dump_keyPrinterPorts.reg

@echo on
