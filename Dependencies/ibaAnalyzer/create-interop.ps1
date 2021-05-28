
$basefolder="C:\Program Files (x86)\Microsoft SDKs\Windows\v1*"

$staticprops= @('/product:ibaAnalyzer', '/copyright:"© iba AG. All rights reserved."', "/machine:agnostic")

$tlpim=(dir $basefolder -Filter TlbImp.exe -Recurse) | Select-Object -Last 1
$regtypeLib = Get-ItemPropertyValue 'Registry::HKEY_CLASSES_ROOT\Interface\{963F47A3-DD3F-4204-93B6-978228717D9A}\TypeLib' -name "(default)",Version

$k = "Registry::HKEY_CLASSES_ROOT\TypeLib\$($regtypeLib[0])\$($regtypeLib[1])\0\win32"
# $k
$analyzer = Get-ItemPropertyValue $k -name "(default)"

$fv = (gi "$analyzer").VersionInfo.FileVersion
$pv = [Version]::new($fv).ToString(3)

"Create Interop.ibaAnalyzer.dll for ibaAnalyzer v$pv"
""

$args = @($analyzer, '/out:Interop.ibaAnalyzer.dll', '/namespace:IbaAnalyzer', "/productversion:$pv", "/asmversion:$fv") + $staticprops
# $args

# "Running: " + $tlpim.Name + " " + $args -join " "

& $tlpim.FullName $args 

""
"Copy Interop.ibaAnalyzer.dll to Interop.ibaAnalyzer-$pv.dll"
Copy-Item Interop.ibaAnalyzer.dll Interop.ibaAnalyzer-$pv.dll -Force

#pause when called by Run with PS (might be a little bit fragile)
if ($MyInvocation.InvocationName -eq "&")
{
    Write-Host "Press <any> key to exit" -foregroundcolor "Yellow" 
#    pause
    cmd /c Pause | Out-Null
}
