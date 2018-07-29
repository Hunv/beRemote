REM @echo off
REM copies files for the current architecture from x64- or x86-Folder to this directory

if "%COMPUTERNAME%"=="WIN-XP-01" GOTO procchoose
if "%COMPUTERNAME%"=="WIN-7-01" GOTO procchoose
if "%COMPUTERNAME%"=="KRISS-PC" GOTO End
if "%COMPUTERNAME%"=="C102250" GOTO End

DEL /F "%1\..\..\compiled\msvcr100_old.dll"
MOVE "%1\..\..\compiled\msvcr100.dll" "%1\..\..\compiled\msvcr100_old.dll"

:procchoose

if "%PROCESSOR_ARCHITECTURE%"=="AMD64" GOTO x64
ELSE GOTO x86

:x64
    COPY /Y "%1\x64\SQLite.Interop.dll" "%1\SQLite.Interop.dll"
REM    COPY /Y "%1\x64\System.Data.SQLite.dll" "%1\System.Data.SQLite.dll"
    COPY /Y "%1\x64\msvcr100.dll" "%1\msvcr100.dll"
GOTO End

:x86
    COPY /Y "%1\x86\msvcr100.dll" "%1\msvcr100.dll"
REM    COPY /Y "%1\x86\SQLite.Interop.dll" "%1\SQLite.Interop.dll"
    COPY /Y "%1\x86\System.Data.SQLite.dll" "%1\System.Data.SQLite.dll"


:End