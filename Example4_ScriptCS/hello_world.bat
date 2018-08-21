@ECHO OFF
CD /d "%~dp0"
ECHO.
ECHO ====================
ECHO %~dpnxf0
ECHO ====================
CALL script_cs_init.bat
set CURDIR=%CD%\
ECHO CURDIR=%CURDIR%
set SCS="%CURDIR%packages\scriptcs.0.17.1\tools\scriptcs.exe"

%SCS% -install Newtonsoft.Json
SET LOGLVL=
REM SET LOGLVL=-loglevel debug
ECHO ON
%SCS% "%CURDIR%hello_world.csx" %LOGLVL% -- hello cruel world!
pause