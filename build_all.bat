@rem Set Visual Studio environment variables
@set vscmd=C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\Tools\VsDevCmd.bat
@if not exist "%vscmd%" set vscmd=C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\Tools\VsDevCmd.bat
@if not exist "%vscmd%" set vscmd=C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\Tools\VsDevCmd.bat
@if not exist "%vscmd%" set vscmd=C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\Common7\Tools\VsDevCmd.bat

call "%vscmd%"
set vscmd=

:: exit 1 when called from teamcity 
:: exit /b 1 when called from commandline for testing to avoid closing of cmd window
:: usage: exit %exitparam% 1
set exitparam=
if "%TEAMCITY_VERSION%"=="" set exitparam=/b

@echo *
@echo ********************
@echo Cleaning solutions
@echo ********************
@echo *
msbuild.exe ".\ibaDatCoordinator.sln" /p:Configuration=Release /p:Platform="x86 with plugins" /target:clean /v:q
@rem Explicitly delete all obj subdirectories
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"

@rem Clear NuGet package cache
rem dotnet nuget locals all --clear

@echo *
@echo **********************************
@echo BUILDING ibaDatCoordinator 
@echo **********************************
@echo *
::msbuild.exe ".\ibaDatCoordinator.sln" /p:Configuration=Release /p:Platform="x86" /target:build /restore /v:q
msbuild.exe ".\ibaDatCoordinator.sln" /p:Configuration=Release /p:Platform="x86 with plugins" /target:build /restore /m /v:q
if errorlevel 1 goto ERR

goto :EOF

:ERR
@echo ##teamcity[buildStatus status='FAILURE' text='Building solutions failed']
exit %exitparam% 1