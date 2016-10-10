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
echo clean finish! /p:Configuration=Release /p:OutputPath=bin\Release /p:VisualStudioVersion=10.0
cmd /C "%MSVS_PATH_CONSOLE%\VC\vcvarsall.bat x86_amd64 && %WORK_DRV% && CD %WORK_DIR% && MSBuild.exe XenAdmin.sln /t:Rebuild /p:Configuration=Release /p:OutputPath=bin\Release /p:Platform="Mixed Platforms" /p:VisualStudioVersion=10.0"
echo build successful
echo.

echo update build info
for /f "tokens=2 delims==" %%a in ('wmic path win32_operatingsystem get LocalDateTime /value') do set t=%%a
set buildinfo=%t:~0,4%%t:~4,2%%t:~6,2%
echo %buildinfo% > .\XenAdmin\bin\Release\BuildInfo

echo obfuscate code
dotNET_Reactor.Console.exe -file ".\XenAdmin\bin\Release\ConsoleMain.exe" -embed 1 -satellite_assemblies ".\XenAdmin\bin\Release\CommandLib.dll/.\XenAdmin\bin\Release\CookComputing.XmlRpcV2.dll/.\XenAdmin\bin\Release\DiscUtils.dll/.\XenAdmin\bin\Release\DiffieHellman.dll/.\XenAdmin\bin\Release\Tamir.SharpSSH.dll/.\XenAdmin\bin\Release\Org.Mentalis.Security.dll/.\XenAdmin\bin\Release\HalsignLib.dll/.\XenAdmin\bin\Release\ICSharpCode.SharpZipLib.dll/.\XenAdmin\bin\Release\Ionic.Zip.dll/.\XenAdmin\bin\Release\log4net.dll/.\XenAdmin\bin\Release\Microsoft.ReportViewer.Common.dll/.\XenAdmin\bin\Release\Microsoft.ReportViewer.ProcessingObjectModel.dll/.\XenAdmin\bin\Release\Microsoft.ReportViewer.WinForms.dll/.\XenAdmin\bin\Release\MSTSCLib.dll/.\XenAdmin\bin\Release\ConsoleLib.dll/.\XenAdmin\bin\Release\ConsoleVNC.dll/.\XenAdmin\bin\Release\XenModel.dll/.\XenAdmin\bin\Release\XenOvf.dll/.\XenAdmin\bin\Release\XenOvfTransport.dll/.\XenAdmin\bin\Release\Microsoft.VisualBasic.PowerPacks.Vs.dll" -necrobit 1 -obfuscation 1 -stringencryption 1 -targetfile ".\release"

cmd /C %WORK_DRV% && CD %WORK_DIR% 

xcopy /e /i /y .\XenAdmin\bin\Release\Help ..\..\release\vconsole\build-60000-new\release\Help
xcopy /e /i /y .\XenAdmin\bin\Release\zh-CN ..\..\release\vconsole\build-60000-new\release\zh-CN
xcopy /e /i /y .\XenAdmin\bin\Release\Schemas ..\..\release\vconsole\build-60000-new\release\Schemas
xcopy /e /s /y /exclude:.\release\exclude.txt .\release\*.* ..\..\release\vconsole\build-60000-new\release

copy /y .\XenAdmin\bin\Release\*.cer ..\..\release\vconsole\build-60000-new\release
copy /y .\XenAdmin\bin\Release\*.py ..\..\release\vconsole\build-60000-new\release
copy /y .\XenAdmin\bin\Release\BuildInfo ..\..\release\vconsole\build-60000-new\release
copy /y .\XenAdmin\bin\Release\ConsoleMain.exe.config ..\..\release\vconsole\build-60000-new\release
copy /y .\XenAdmin\bin\Release\HalsignConsole.exe ..\..\release\vconsole\build-60000-new\release
copy /y .\release\ConsoleMain.exe ..\..\release\vconsole\build-60000-new\release

cd ..\..\release\vconsole\build-60000-new\release
call gkbuild.bat







