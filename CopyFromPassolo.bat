@echo off

set datCoVer=v3.0.x

echo %datCoVer% chosen

pushd "%CD%"      
CD /D "%~dp0"

robocopy  \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries\de Passolo\de *.dll
robocopy  \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries\es Passolo\es *.dll
robocopy  \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries\fr Passolo\fr *.dll
robocopy  \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries\it Passolo\it *.dll
robocopy  \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries\ru Passolo\ru *.dll
robocopy  \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries\zh-Hans Passolo\zh-Hans *.dll
robocopy  \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries\pt Passolo\pt *.dll
robocopy  \\iba-fue-dokuv6.iba-ag.local\Passolo\ibaDatCoordinator\%datCoVer%\Binaries\ja Passolo\ja *.dll

popd

:: pause only when file was double clicked 
:: according to: https://stackoverflow.com/questions/5859854/detect-if-bat-file-is-running-via-double-click-or-from-cmd-window
IF %0 == "%~0"  pause