@echo *
@echo ********************
@echo Deleting old files
@echo ********************
@echo *

del /q InstallFiles
rmdir /s /q InstallFiles
timeout /t 2 > NUL
mkdir InstallFiles
mkdir InstallFiles\CRT
mkdir InstallFiles\de
mkdir InstallFiles\fr
mkdir InstallFiles\es
mkdir InstallFiles\ru
mkdir InstallFiles\zh-Hans
mkdir InstallFiles\it
mkdir InstallFiles\ja
mkdir InstallFiles\pt

timeout /t 2 > NUL

@echo *
@echo ********************
@echo Copy ibaDatCoordinator files
@echo ********************
@echo *

robocopy ibaDatCoordinator\bin\release InstallFiles *.dll /S /NJH /NJS /NFL /NDL
robocopy ibaDatCoordinator\bin\release InstallFiles *.exe /S /NJH /NJS /NFL /NDL

xcopy dependencies\CRT\*.dll InstallFiles\CRT /S /E /Y
rem Copy hdclientfiles because it is required for the obfuscation and hd plugin
robocopy Dependencies InstallFiles hdclientfiles*.dll /S /NJH /NJS /NFL /NDL
copy ibaDatCoordinatorService\bin\release\ibaDatCoordinatorService.exe InstallFiles
copy ibaDatCoordinatorStatus\bin\release\ibaDatCoordinatorStatus.exe InstallFiles

rem pause




