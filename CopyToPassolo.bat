@echo off
@ECHO OFF

set datCoVer=v3.0.x

echo %datCoVer% chosen

pushd "%CD%"      
CD /D "%~dp0"

robocopy ibaDatCoordinatorService\bin\Release \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries *.dll
robocopy ibaDatCoordinatorService\bin\Release \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries *.exe
robocopy ibaDatCoordinatorStatus\bin\Release \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries *.dll
robocopy ibaDatCoordinatorStatus\bin\Release \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries *.exe
robocopy AM-OSPC-plugin\bin\Release \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries *.dll
robocopy S7-writer-plugin\bin\Release \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries *.dll
robocopy Alunorf-sinec-h1-plugin\bin\Release \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries *.dll

popd

pause
exit
