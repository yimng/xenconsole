::
:: Copyright (c) 2013 Halsign Corporation  All rights reserved
::
:: gkbuild.bat [-b|-i|-c] [-d|-r]
::
::       -b / -i / -c              build only / and install / clean
::       -d / -r                   debug / release
::


@echo off

echo.

if not "%1" == "-c" goto release

:CLEAN
echo Halsign Console: Clean build

copy /y .\gkclean.bat ..\..\release
cd ..\..\release
call .\gkclean.bat
del /q .\gkclean.bat
cd ..\src\release

copy /y .\gkclean.bat ..\..\debug
cd ..\..\debug
call .\gkclean.bat
del /q .\gkclean.bat
cd ..\src\release

echo Clean OK
goto end

:release

if "%2" == "-d" goto install_debug

SET BUILD_TARGET="Release"
goto install

:install_debug
SET BUILD_TARGET="Debug"
goto install

:install
echo Halsign Console: install begins

xcopy /e /s /y /exclude:.\exclude.txt .\*.* ..\..\%BUILD_TARGET%\

echo Halsign Console: install OK.

:end
