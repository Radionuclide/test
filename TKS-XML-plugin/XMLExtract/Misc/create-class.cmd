:: creates a class based on xsd-file provided by tks.
:: the changes shown in ..\Erzeugung\Messreihe-iba-mod.diff must be integrated:
:: - add and update xsiSchemaLocation to latest version (eg. REL-2_4)
:: - add WerteList to Raster1D
:: - change WerteX serialization on Raster1D
:: - remove unneeded private fields for autoimplemented properties: einheitField, bandlaufrichtungField, messgroesseField
:: - add "ShouldSerializeStatistic" function
:: - add enum comments for special values which contains a [System.Xml.Serialization.XmlEnumAttribute("...")]  attribute

:: set xsd=C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\NETFX 4.0 Tools\xsd.exe
::"%xsd%" Messreihe.xsd /c /n:XmlExtract

:: use xsd2code.exe (3.6.x.x, https://xsd2code.codeplex.com/) istead of xsd.exe since there are less changes needed.

set xsd2code=xsd2code\Xsd2Code.exe
"%xsd2code%" MessreiheReduziert.xsd XmlExtract Messreihe-xsd2code.cs /cu "System.Xml" /pl Net35 /dbg- /is- /xa+ /ap+ /sc+
