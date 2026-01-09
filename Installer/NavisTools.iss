; Inno Setup Script for NavisTools
; Version is passed via command line parameter /DMyAppVersion="x.y.z"

#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif

#define MyAppName "NavisTools"
#define MyAppPublisher "NavisTools"
#define MyAppURL "https://github.com/romangolev/NavisTools"
#define BundleSourceDir "..\output\NavisTools.bundle"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
AppId={{A3B5C8D9-1E2F-4A5B-8C9D-0E1F2A3B4C5D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
; Don't show directory selection page as we're installing to a fixed location
DisableDirPage=yes
DisableProgramGroupPage=yes
LicenseFile=..\LICENSE
OutputDir=..\output\installer
OutputBaseFilename=NavisTools-Setup-v{#MyAppVersion}
Compression=lzma2/max
SolidCompression=yes
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
; Require admin rights to write to ProgramData
PrivilegesRequired=admin
; Installer and uninstaller icons
SetupIconFile=..\NavisTools\Images\nt_48.ico
UninstallDisplayIcon={commonappdata}\Autodesk\ApplicationPlugins\NavisTools.bundle\Contents\nt_48.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
; Copy the entire bundle to ProgramData
Source: "{#BundleSourceDir}\*"; DestDir: "{commonappdata}\Autodesk\ApplicationPlugins\NavisTools.bundle"; Flags: ignoreversion recursesubdirs createallsubdirs

[Registry]
; Register the application in registry
Root: HKLM; Subkey: "Software\Autodesk\Navisworks\NavisTools"; Flags: uninsdeletekeyifempty
Root: HKLM; Subkey: "Software\Autodesk\Navisworks\NavisTools"; ValueType: string; ValueName: "InstallPath"; ValueData: "{commonappdata}\Autodesk\ApplicationPlugins\NavisTools.bundle"; Flags: uninsdeletevalue
Root: HKLM; Subkey: "Software\Autodesk\Navisworks\NavisTools"; ValueType: string; ValueName: "Version"; ValueData: "{#MyAppVersion}"; Flags: uninsdeletevalue
Root: HKLM; Subkey: "Software\Autodesk\Navisworks\NavisTools"; ValueType: string; ValueName: "Publisher"; ValueData: "{#MyAppPublisher}"; Flags: uninsdeletevalue

[Icons]
; Don't create Start Menu shortcuts as this is a plugin

[Code]
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    // Log successful installation
    Log('NavisTools installed successfully to: ' + ExpandConstant('{commonappdata}\Autodesk\ApplicationPlugins\NavisTools.bundle'));
  end;
end;

function InitializeUninstall(): Boolean;
begin
  Result := True;
  if MsgBox('Are you sure you want to remove NavisTools and all of its components?', 
            mbConfirmation, MB_YESNO) = IDYES then
  begin
    Result := True;
  end
  else
  begin
    Result := False;
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usPostUninstall then
  begin
    // Clean up any remaining files or registry entries
    Log('NavisTools uninstalled successfully');
  end;
end;
