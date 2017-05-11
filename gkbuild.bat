::
:: Copyright (c) 2013 Halsign Corporation.  All rights reserved
::
:: gkbuild.bat [-b|-i|-c] [-d|-r]
::
::       -b / -i / -c              build only / and install / clean
::       -d / -r                   debug / release
::


@echo off

SET WORK_DIR=%CD%
SET WORK_DRV=%WORK_DIR:~0,2%

SET BUILD_TARGET="release"

cmd /C "%MSVS_PATH_CONSOLE%\VC\vcvarsall.bat x86_amd64 && %WORK_DRV% && CD %WORK_DIR% && devenv /clean debug XenAdmin.sln"
cmd /C "%MSVS_PATH_CONSOLE%\VC\vcvarsall.bat x86_amd64 && %WORK_DRV% && CD %WORK_DIR% && devenv /clean Release XenAdmin.sln"
echo clean finish! /p:Configuration=Release /p:OutputPath=bin\Release /p:VisualStudioVersion=15.0
cmd /C "%MSVS_PATH_CONSOLE%\VC\vcvarsall.bat x86_amd64 && %WORK_DRV% && CD %WORK_DIR% && MSBuild.exe XenAdmin.sln /t:Rebuild /p:Configuration=Release /p:OutputPath=bin\Release /p:Platform="Mixed Platforms" /p:VisualStudioVersion=15.0"
echo build successful
echo.

echo update build info
for /f "tokens=2 delims==" %%a in ('wmic path win32_operatingsystem get LocalDateTime /value') do set t=%%a
set buildinfo=%t:~0,4%%t:~4,2%%t:~6,2%
echo %buildinfo% > .\XenAdmin\bin\Release\BuildInfo









