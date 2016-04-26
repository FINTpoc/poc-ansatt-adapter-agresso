REM Script for å generere C# Class-filer ut fra XSD-filer til Ansatt-felleskomponent.

REM Starte Developer Command Prompt for VS2015:
REM %comspec% /k "%VS140COMNTOOLS%\VsDevCmd.bat"

set "inputdir=D:\GitHub\FINT\event-modell-poc\src\main\resources\"
set "inputfiles=%inputdir%event.xsd"

xsd.exe %inputfiles% /outputdir:. /classes