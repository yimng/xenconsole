;
; Copyright (c) 2008-2010 Halsign, Inc.  All rights reserved.
;

!define PRODUCER_NAME      "cldmind"
!define PRODUCT_SUITE      "cldmind"
!define PRODUCT_NAME       "cldmind Console"
!define PRODUCT_DIS_VER    "6.0"
!define PRODUCT_VERSION    "6.0.0"
!define PRODUCT_PUBLISHER  "cldmind Corporation."
!define PRODUCT_WEB_SITE   "http://www.cldmind.com"
!define PRODUCT_CODE_KEY   "ProductCode"
!define PRODUCT_CODE       "cldmind Console"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_CODE}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"
!define BINARY_DIR         "BinDir"
!define DATA_DIR           "DataDir"
!define DEBUG_MODE         "Debug"
!define AUTO_UI_LANG       "AutoUILang"

!define APP_NAME "HalsignConsole.exe"
!define APP_MAIN_NAME "ConsoleMain.exe"
!define PROCESS_NAME "ConsoleMain.exe"

!define MIN_FRA_MAJOR "3"
!define MIN_FRA_MINOR "5"
!define MIN_FRA_BUILD "*"

; MUI 1.67 compatible ------
!include "MUI.nsh"
!include "FileFunc.nsh"
!include "Registry.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "install.ico"
!define MUI_UNICON "uninstall.ico"
!define MUI_WELCOMEFINISHPAGE_BITMAP "install.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "uninstall.bmp"
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "header-install.bmp"
!define MUI_HEADERIMAGE_UNBITMAP "header-uninstall.bmp"
!define MUI_CUSTOMFUNCTION_ABORT "UserAbort"
!define MUI_CUSTOMFUNCTION_UNABORT "un.AbortFunction"
RequestExecutionLevel admin

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; License page
!define MUI_LICENSEPAGE_RADIOBUTTONS
!insertmacro MUI_PAGE_LICENSE "$(gkLicenseData)"
; Directory page
!insertmacro MUI_PAGE_DIRECTORY
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

; Language files
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "SimpChinese"

!macro SetUILanguage UN
Function ${UN}SetUILanguage
  Push $R0
  Push $R1
  Push $R2
  ; Call GetUserDefaultUILanguage (available on Windows Me, 2000 and later)
  ; $R0 = GetUserDefaultUILanguage()
  System::Call 'kernel32::GetUserDefaultUILanguage() i.r10'
  StrCpy $LANGUAGE $R0
  Pop $R2
  Pop $R1
  Pop $R0
FunctionEnd
!macroend
!insertmacro SetUILanguage ""
!insertmacro SetUILanguage "un."

; License files
LicenseLangString gkLicenseData ${LANG_ENGLISH} ".\en\EULA"
LicenseLangString gkLicenseData ${LANG_SIMPCHINESE} ".\zh-CN\EULA"
LicenseData $(gkLicenseData)

LangString message_exist ${LANG_ENGLISH} "${PRODUCT_NAME} $0 is detected on your computer.$\r$\n\
  To install this release, please manually uninstall the previous one first."
LangString message_exist ${LANG_SIMPCHINESE} "该计算机已经安装了 ${PRODUCT_NAME} $0.$\r$\n\
  在重新安装之前，您必须先卸载掉上一次的安装。"
LangString check_before_install ${LANG_ENGLISH} "${PRODUCT_NAME} is running on your computer and can not be installed, please close ${PRODUCT_NAME} and try again."
LangString check_before_install ${LANG_SIMPCHINESE} "安装程序检测到 ${PRODUCT_NAME} 正在运行，请尝试关闭 ${PRODUCT_NAME} 并重新安装。"
LangString check_before_uninstall ${LANG_ENGLISH} "${PRODUCT_NAME} is running on your computer and can not be removed, please close ${PRODUCT_NAME} and try again."
LangString check_before_uninstall ${LANG_SIMPCHINESE} "安装程序检测到 ${PRODUCT_NAME} 正在运行，请尝试关闭 ${PRODUCT_NAME} 并重新卸载。"
LangString message_os_low ${LANG_ENGLISH} "Windows version is not supported by ${PRODUCT_NAME}."
LangString message_os_low ${LANG_SIMPCHINESE} "您的操作系统不支持安装 ${PRODUCT_NAME}。"
LangString message_no_sp ${LANG_ENGLISH} "Please install Service Pack 2 or later before ${PRODUCT_NAME} setup."
LangString message_no_sp ${LANG_SIMPCHINESE} "安装 ${PRODUCT_NAME} 之前，请先安装该操作系统的 Service Pack 2 或以后版本。"
LangString message_no_netframwork ${LANG_ENGLISH} "Cannot install this software. \
                                                   This software requires Windows .NET Framework \
                                                   ${MIN_FRA_MAJOR}.${MIN_FRA_MINOR} or higher.$\n\
                                                   No version of Windows .NET Framework is installed. \
                                                   Please download and install Windows .NET Framwork ${MIN_FRA_MAJOR}.${MIN_FRA_MINOR}."
LangString message_no_netframwork ${LANG_SIMPCHINESE} "无法安装该软件。\
                                                   该软件需要安装 Windows .NET Framework \
                                                   ${MIN_FRA_MAJOR}.${MIN_FRA_MINOR} 或更高版本。$\n\
                                                   该计算机没有安装 Windows .NET Framework。\
                                                   请下载并安装 Windows .NET Framwork ${MIN_FRA_MAJOR}.${MIN_FRA_MINOR}。"
LangString message_low_netframwork ${LANG_ENGLISH} "Cannot install this software. \
                                                    This software requires Windows Framework version \
                                                    ${MIN_FRA_MAJOR}.${MIN_FRA_MINOR} or higher.$\n\
                                                    The highest version on this computer is $R8. \
                                                    Please download and install Windows .NET Framwork ${MIN_FRA_MAJOR}.${MIN_FRA_MINOR}."
LangString message_low_netframwork ${LANG_SIMPCHINESE} "无法安装该软件。\
                                                    该软件需要安装 Windows .NET Framework \
                                                    ${MIN_FRA_MAJOR}.${MIN_FRA_MINOR} 或更高版本。$\n\
                                                    当前计算机安装的最高版本为： $R8。\
                                                    请下载并安装 Windows .NET Framwork ${MIN_FRA_MAJOR}.${MIN_FRA_MINOR}。"
LangString message_company ${LANG_ENGLISH} "Halsign"
LangString message_company ${LANG_SIMPCHINESE} "红山"
LangString message_product ${LANG_ENGLISH} "Halsign Console"
LangString message_product ${LANG_SIMPCHINESE} "Halsign Console"
LangString message_uninstall ${LANG_ENGLISH} "Uninstall"
LangString message_uninstall ${LANG_SIMPCHINESE} "卸载"


; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_DIS_VER}"
OutFile "CloudConsole.exe"
InstallDir "$PROGRAMFILES\${PRODUCT_NAME}"
ShowInstDetails hide
ShowUnInstDetails hide
SpaceTexts
BrandingText ${PRODUCT_WEB_SITE}

Section "Install components..."
  setShellVarContext all
  LogSet on
  SetOutPath "$INSTDIR\en"
  File /r "en\EULA"
  SetOutPath "$INSTDIR\zh-CN"
  File /r "zh-CN\ConsoleMain.resources.dll"
  File /r "zh-CN\XenModel.resources.dll"
  File /r "zh-CN\XenOvf.resources.dll"
  File /r "zh-CN\XenOvfTransport.resources.dll"
  File /r "zh-CN\EULA"
  SetOutPath "$INSTDIR\Help"
  ;File /r "Help\${PRODUCT_CODE}.chm"
  SetOutPath "$INSTDIR\Schemas"
  File /r "Schemas\CIM_OperatingSystem.xml"
  File /r "Schemas\CIM_ResourceAllocationSettingData.xml"
  File /r "Schemas\CIM_ResourceAllocationSettingData.xsd"
  File /r "Schemas\CIM_VirtualSystemSettingData.xml"
  File /r "Schemas\CIM_VirtualSystemSettingData.xsd"
  File /r "Schemas\common.xsd"
  File /r "Schemas\DSP8023.xsd"
  File /r "Schemas\DSP8027.xsd"
  File /r "Schemas\secext-1.0.xsd"
  File /r "Schemas\wss-utility-1.0.xsd"
  File /r "Schemas\xenc-schema.xsd"
  File /r "Schemas\xml.xsd"
  File /r "Schemas\xmldsig-core-schema.xsd"
  SetOutPath "$INSTDIR"
  SetOverwrite ifdiff
  File /r "default.ico"
  File /r ${APP_NAME}
  File /r ${APP_MAIN_NAME}
  File /r "CommandLib.dll"
  File /r "ConsoleLib.dll"
  File /r "CookComputing.XmlRpcV2.dll"
  File /r "DiffieHellman.dll"
  File /r "DiscUtils.dll"
  File /r "HalsignLib.dll"
  File /r "ICSharpCode.SharpZipLib.dll"
  File /r "Ionic.Zip.dll"
  File /r "log4net.dll"
  File /r "Microsoft.ReportViewer.Common.dll"
  File /r "Microsoft.ReportViewer.ProcessingObjectModel.dll"
  File /r "Microsoft.ReportViewer.WinForms.dll"
  File /r "MSTSCLib.dll"
  File /r "Tamir.SharpSSH.dll"
  File /r "XenCenterVNC.dll"
  File /r "XenModel.dll"
  File /r "XenOvf.dll"
  File /r "XenOvfTransport.dll"
  File /r "Org.Mentalis.Security.dll"
  File /r "putty.exe"

  File /r "${APP_MAIN_NAME}.config"
  File /r "reports.xml"  
  File /r "Halsign.cer"
  File /r "halsign_host_upgrade.py"
  File /r "BuildInfo"
  File /r "uninstall.ico"
  File /r "x86\"
  WriteUninstaller "$INSTDIR\uninst.exe"

  WriteIniStr "$INSTDIR\${PRODUCT_SUITE}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  SetShellVarContext all
  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\$(message_uninstall) ${PRODUCT_NAME}.lnk" "$INSTDIR\uninst.exe" "uninstall" "$INSTDIR\uninstall.ico"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME} ${PRODUCT_DIS_VER}.lnk" "$INSTDIR\${APP_NAME}"
  CreateShortCut "$desktop\${PRODUCT_NAME} ${PRODUCT_DIS_VER}.lnk" "$INSTDIR\${APP_NAME}"
  
  WriteRegStr "HKCR" ".vhd" "" "${PRODUCT_CODE}.vhd"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.vhd" "" "${PRODUCT_SUITE} Virtual Appliance"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.vhd\DefaultIcon" "" "$INSTDIR\default.ico"
  
  WriteRegStr "HKCR" ".ovf" "" "${PRODUCT_CODE}.ovf"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.ovf" "" "${PRODUCT_SUITE} Virtual Appliance"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.ovf\DefaultIcon" "" "$INSTDIR\default.ico"
  
  WriteRegStr "HKCR" ".tva" "" "${PRODUCT_CODE}.tva"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.tva" "" "${PRODUCT_SUITE} Virtual Appliance"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.tva\DefaultIcon" "" "$INSTDIR\default.ico"
  WriteRegStr "HKCR" ".va" "" "${PRODUCT_CODE}.va"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.va" "" "${PRODUCT_SUITE} Virtual Appliance"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.va\DefaultIcon" "" "$INSTDIR\default.ico"
  WriteRegStr "HKCR" ".tslic" "" "${PRODUCT_CODE}.tslic"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.tslic" "" "${PRODUCT_SUITE} License File"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.tslic\DefaultIcon" "" "$INSTDIR\default.ico"
  WriteRegStr "HKCR" ".tsupdate" "" "${PRODUCT_CODE}.tsupdate"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.tsupdate" "" "${PRODUCT_SUITE} Update File"
  WriteRegStr "HKCR" "${PRODUCT_CODE}.tsupdate\DefaultIcon" "" "$INSTDIR\default.ico"

  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_DIS_VER}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLUpdateInfo" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "HelpLink" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoModify" 0x00000001
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoRepair" 0x00000001

  LogSet off
SectionEnd

Section -AdditionalIcons
SectionEnd

Section -Post
SectionEnd

Function AbortIfBadFramework
  ;Save the variables in case something else is using them
  Push $0
  Push $1
  Push $2
  Push $3
  Push $4
  Push $R1
  Push $R2
  Push $R3
  Push $R4
  Push $R5
  Push $R6
  Push $R7
  Push $R8

  StrCpy $R5 "0"
  StrCpy $R6 "0"
  StrCpy $R7 "0"
  StrCpy $R8 "0.0.0"
  StrCpy $0 0

loop:
  ;Get each sub key under "SOFTWARE\Microsoft\NET Framework Setup\NDP"
  EnumRegKey $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP" $0
  StrCmp $1 "" done ;jump to end if no more registry keys
  IntOp $0 $0 + 1
  StrCpy $2 $1 1 ;Cut off the first character
  StrCpy $3 $1 "" 1 ;Remainder of string

  ;Loop if first character is not a 'v'
  StrCmpS $2 "v" start_parse loop

  ;Parse the string
start_parse:
  StrCpy $R1 ""
  StrCpy $R2 ""
  StrCpy $R3 ""
  StrCpy $R4 $3

  StrCpy $4 1

parse:
  StrCmp $3 "" parse_done ;If string is empty, we are finished
  StrCpy $2 $3 1 ;Cut off the first character
  StrCpy $3 $3 "" 1 ;Remainder of string
  StrCmp $2 "." is_dot not_dot ;Move to next part if it's a dot

is_dot:
  IntOp $4 $4 + 1 ; Move to the next section
  goto parse ;Carry on parsing

not_dot:
  IntCmp $4 1 major_ver
  IntCmp $4 2 minor_ver
  IntCmp $4 3 build_ver
  IntCmp $4 4 parse_done

major_ver:
  StrCpy $R1 $R1$2
  goto parse ;Carry on parsing

minor_ver:
  StrCpy $R2 $R2$2
  goto parse ;Carry on parsing

build_ver:
  StrCpy $R3 $R3$2
  goto parse ;Carry on parsing

parse_done:
  IntCmp $R1 $R5 this_major_same loop this_major_more
  this_major_more:
  StrCpy $R5 $R1
  StrCpy $R6 $R2
  StrCpy $R7 $R3
  StrCpy $R8 $R4

  goto loop

this_major_same:
  IntCmp $R2 $R6 this_minor_same loop this_minor_more
this_minor_more:
  StrCpy $R6 $R2
  StrCpy $R7 R3
  StrCpy $R8 $R4
  goto loop

this_minor_same:
  IntCmp R3 $R7 loop loop this_build_more
this_build_more:
  StrCpy $R7 $R3
  StrCpy $R8 $R4
  goto loop

done:
  ;Have we got the framework we need?
  IntCmp $R5 ${MIN_FRA_MAJOR} max_major_same fail end
max_major_same:
  IntCmp $R6 ${MIN_FRA_MINOR} max_minor_same fail end
max_minor_same:
  IntCmp $R7 ${MIN_FRA_BUILD} end fail end

fail:
  StrCmp $R8 "0.0.0" no_framework
  goto wrong_framework

no_framework:
  MessageBox MB_OK|MB_ICONSTOP $(message_no_netframwork)
  Abort
wrong_framework:
  MessageBox MB_OK|MB_ICONSTOP $(message_low_netframwork)
  Abort
end:
  ;Pop the variables we pushed earlier
  Pop $R8
  Pop $R7
  Pop $R6
  Pop $R5
  Pop $R4
  Pop $R3
  Pop $R2
  Pop $R1
  Pop $4
  Pop $3
  Pop $2
  Pop $1
  Pop $0
FunctionEnd

Function .onInit
  LockedList::FindProcess ${PROCESS_NAME}
	Pop $R0
    ${If} $R0 != ``
       MessageBox MB_ICONSTOP|MB_OK $(check_before_install)
	   Quit
    ${EndIf} 
  BringToFront
  ; Check if already running
  ; If so don't open another but bring to front
  System::Call "kernel32::CreateMutexA(i 0, i 0, t '$(^Name)') i .r0 ?e"
  Pop $0
  StrCmp $0 0 launch
  StrLen $0 "$(^Name)"
  IntOp $0 $0 + 1
loop:
  FindWindow $1 '#32770' '' 0 $1
  IntCmp $1 0 +5
  System::Call "user32::GetWindowText(i r1, t .r2, i r0) i."
  StrCmp $2 "$(^Name)" 0 loop
  System::Call "user32::ShowWindow(i r1,i 9) i."         ; If minimized then maximize
  System::Call "user32::SetForegroundWindow(i r1) i."    ; Bring to front
  Abort
launch:
  ReadRegStr $0 HKLM "Software\${PRODUCER_NAME}\${PRODUCT_CODE}" "Version"
  StrCmp $0 "" end 0
  ReadRegStr $INSTDIR HKLM "Software\${PRODUCER_NAME}\${PRODUCT_CODE}" "${BINARY_DIR}"
;  MessageBox MB_ICONINFORMATION|MB_OK ${message_exist}
;  Abort
end:

  Call SetUILanguage

  ;Get Windows OS version
  ReadRegStr $0 HKLM "Software\Microsoft\Windows NT\CurrentVersion" "CurrentVersion"
  LogText "Windows Current version : $0"
  strCmp $0 "5.0" canInstall 0
  strCmp $0 "5.1" canInstall 0
  strCmp $0 "5.2" canInstall 0
  strCmp $0 "6.0" Install 0
  strCmp $0 "6.1" Install 0
  strCmp $0 "6.2" Install 0
  strCmp $0 "6.3" Install 0
  MessageBox MB_ICONSTOP|MB_OK $(message_os_low)
  Abort
canInstall:
  ReadRegStr $1 HKLM "Software\Microsoft\Windows NT\CurrentVersion" "CSDVersion"
  LogText "$1"
  strCmp $1 "" cannot 0
  strCmp $1 "Service Pack 1" cannot Install ; Install after service pack 2
cannot:
  MessageBox MB_ICONSTOP|MB_OK $(message_no_sp)
  Abort
Install:

  Call AbortIfBadFramework
FunctionEnd

Function "UserAbort"
FunctionEnd

Function "un.AbortFunction"
FunctionEnd

Function .onInstSuccess
  ClearErrors
  WriteRegStr HKLM "Software\${PRODUCER_NAME}" "${PRODUCT_CODE_KEY}" "${PRODUCT_CODE}"
  WriteRegStr HKLM "Software\${PRODUCER_NAME}\${PRODUCT_CODE}" "Company" $(message_company)
  WriteRegStr HKLM "Software\${PRODUCER_NAME}\${PRODUCT_CODE}" "Product" $(message_product)
  WriteRegStr HKLM "Software\${PRODUCER_NAME}\${PRODUCT_CODE}" "Version" "${PRODUCT_VERSION}"
  WriteRegStr HKLM "Software\${PRODUCER_NAME}\${PRODUCT_CODE}" "${BINARY_DIR}" "$INSTDIR\"
  WriteRegDWORD HKLM "Software\${PRODUCER_NAME}\${PRODUCT_CODE}" "${DEBUG_MODE}" 0
  WriteRegDWORD HKLM "Software\${PRODUCER_NAME}\${PRODUCT_CODE}" "${AUTO_UI_LANG}" 0
  
  ;Push $R0
  ;ReadRegStr $R0 HKLM "SYSTEM\CurrentControlSet\Control\Session Manager\Environment" PROCESSOR_ARCHITECTURE
  ;SetOutPath "$INSTDIR"
  ;${If} $R0 == 'x86'  
	;ExecWait '"$SYSDIR\rundll32.exe" "$INSTDIR\JITComVCTK_S.dll" DllRegisterServer' 
  ;${Else}
	;ExecWait '"$SYSDIR\rundll32.exe" "$INSTDIR\JITComVCTK64_S.dll" DllRegisterServer'  
  ;${EndIf}   
FunctionEnd

Function un.onInit
  LockedList::FindProcess ${PROCESS_NAME}
	Pop $R0
    ${If} $R0 != ``
       MessageBox MB_ICONSTOP|MB_OK $(check_before_uninstall)
	   Quit
    ${EndIf} 
  BringToFront
  ; Check if already running
  ; If so don't open another but bring to front
  System::Call "kernel32::CreateMutexA(i 0, i 0, t '$(^Name)') i .r0 ?e"
  Pop $0
  StrCmp $0 0 launch
  StrLen $0 "$(^Name)"
  IntOp $0 $0 + 1
loop:
  FindWindow $1 '#32770' '' 0 $1
  IntCmp $1 0 +5
  System::Call "user32::GetWindowText(i r1, t .r2, i r0) i."
  StrCmp $2 "$(^Name)" 0 loop
  System::Call "user32::ShowWindow(i r1,i 9) i."         ; If minimized then maximize
  System::Call "user32::SetForegroundWindow(i r1) i."    ; Bring to front
  Abort
launch:
  Call un.SetUILanguage
FunctionEnd

Section Uninstall
  setShellVarContext all
  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegValue HKLM "Software\${PRODUCER_NAME}" "${PRODUCT_CODE_KEY}"
  DeleteRegKey HKLM "Software\${PRODUCER_NAME}\${PRODUCT_CODE}"
  DeleteRegKey HKLM "Software\${PRODUCER_NAME}"
  
  ;Push $R0
  ;ReadRegStr $R0 HKLM "SYSTEM\CurrentControlSet\Control\Session Manager\Environment" PROCESSOR_ARCHITECTURE
  ;SetOutPath "$INSTDIR"
  ;${If} $R0 == 'x86'  
	;ExecWait '"$SYSDIR\rundll32.exe" "$INSTDIR\JITComVCTK_S.dll" DllUnregisterServer' 
  ;${Else}
	;ExecWait '"$SYSDIR\rundll32.exe" "$INSTDIR\JITComVCTK64_S.dll" DllUnregisterServer'  
  ;${EndIf} 
  
  Delete "$desktop\${PRODUCT_NAME} ${PRODUCT_DIS_VER}.lnk"
  RMDir /r /REBOOTOK "$SMPROGRAMS\${PRODUCT_NAME}"
  Delete /REBOOTOK "$INSTDIR\${PRODUCT_SUITE}.url"
  Delete /REBOOTOK "$INSTDIR\${APP_NAME}"
  Delete /REBOOTOK "$INSTDIR\${APP_MAIN_NAME}"
  Delete /REBOOTOK "$INSTDIR\uninst.exe"
  Delete /REBOOTOK "$INSTDIR\uninstall.ico"
  Delete /REBOOTOK "$INSTDIR\install.log"

  RMDir /r /REBOOTOK "$INSTDIR"    
SectionEnd
