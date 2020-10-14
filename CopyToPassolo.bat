@echo off
@ECHO OFF

set pdaVer=v2.4.x

echo %pdaVer% choosen

pushd "%CD%"      
CD /D "%~dp0"

robocopy ibaDatCoordinatorService\bin\Release v:\ibaDatCoordinator\%pdaVer%\Binaries *.dll
robocopy ibaDatCoordinatorService\bin\Release v:\ibaDatCoordinator\%pdaVer%\Binaries *.exe
robocopy ibaDatCoordinatorService\bin\Release v:\ibaDatCoordinator\%pdaVer%\Binaries *.manifest
robocopy ibaDatCoordinatorStatus\bin\Release v:\ibaDatCoordinator\%pdaVer%\Binaries *.dll
robocopy ibaDatCoordinatorStatus\bin\Release v:\ibaDatCoordinator\%pdaVer%\Binaries *.exe
robocopy ibaDatCoordinatorStatus\bin\Release v:\ibaDatCoordinator\%pdaVer%\Binaries *.manifest

popd

pause
exit
