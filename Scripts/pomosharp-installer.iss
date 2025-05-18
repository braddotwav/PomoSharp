#define AppName "PomoSharp"
#define AppExeName "PomoSharp.exe"
#define AppPublisher "braddotwav"
#define AppURL "https://github.com/braddotwav/PomoSharp"
#ifndef AppVersion
  #define AppVersion = '0.0.0.0';
#endif

[Setup]
AppId=0C844A98-17DE-4D9E-B17D-CEE910559528
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
VersionInfoVersion={#AppVersion}
DefaultDirName={commonpf64}\{#AppName}
DisableProgramGroupPage=auto
SolidCompression=yes
PrivilegesRequired=none
WizardStyle=modern
AllowUNCPath=no
AllowNetworkDrive=no
LicenseFile=../LICENSE.txt
OutputDir=/bin/
OutputBaseFilename=pomoSharp_installer_win64
UninstallDisplayIcon={app}\{#AppExeName}
SetupIconFile=../PomoSharp/Resources/Icon.ico
UninstallDisplayName={#AppName}

[Files]
Source: "../PomoSharp\bin\framework-dependant\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";
Name: "startmenu"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: startmenu
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[UninstallDelete]
Type: filesandordirs; Name: "{userappdata}/PomoSharp"






