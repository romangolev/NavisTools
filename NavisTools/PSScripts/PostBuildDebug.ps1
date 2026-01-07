param(
    [string]$TargetDir,
    [string]$ProjectDir,
    [string]$TargetName,
    [string]$NavisVersion
)

# Normalize paths by removing trailing slashes and ensuring proper format
$TargetDir = $TargetDir.TrimEnd('\')
$ProjectDir = $ProjectDir.TrimEnd('\')

Write-Host "Parameters received:"
Write-Host "  TargetDir: '$TargetDir'"
Write-Host "  ProjectDir: '$ProjectDir'"
Write-Host "  TargetName: '$TargetName'"
Write-Host "  NavisVersion: '$NavisVersion'"

# Verify directories exist
if (-not (Test-Path $TargetDir)) {
    Write-Error "TargetDir does not exist: $TargetDir"
    exit 1
}
if (-not (Test-Path $ProjectDir)) {
    Write-Error "ProjectDir does not exist: $ProjectDir"
    exit 1
}

$pluginPath = "$env:ProgramFiles\Autodesk\Navisworks Manage $NavisVersion\Plugins\$TargetName"

# Copy DLLs to plugin directory
Write-Host "`nCopying DLLs from: $TargetDir"
Write-Host "Copying DLLs to: $pluginPath"
$null = New-Item -ItemType Directory -Path $pluginPath -Force
$dllFiles = Get-ChildItem -Path $TargetDir -Filter "*.dll" -File
if ($dllFiles.Count -eq 0) {
    Write-Warning "No DLL files found in: $TargetDir"
} else {
    foreach ($file in $dllFiles) {
        Copy-Item -Path $file.FullName -Destination $pluginPath -Force
        Write-Host "  Copied: $($file.Name)"
    }
}

# Copy XAML files to en-US subdirectory
$enUsPath = "$pluginPath\en-US"
Write-Host "`nCopying XAML files to: $enUsPath"
$null = New-Item -ItemType Directory -Path $enUsPath -Force
$xamlFiles = Get-ChildItem -Path $ProjectDir -Filter "*.xaml" -File
foreach ($file in $xamlFiles) {
    Copy-Item -Path $file.FullName -Destination $enUsPath -Force
    Write-Host "  Copied: $($file.Name)"
}

# Copy .name files to en-US subdirectory
Write-Host "`nCopying .name files to: $enUsPath"
$nameFiles = Get-ChildItem -Path $ProjectDir -Filter "*.name" -File
foreach ($file in $nameFiles) {
    Copy-Item -Path $file.FullName -Destination $enUsPath -Force
    Write-Host "  Copied: $($file.Name)"
}

# Copy icon files to Images subdirectory
$imagesPath = "$pluginPath\Images"
$projectImagesPath = Join-Path -Path $ProjectDir -ChildPath "Images"
Write-Host "`nCopying icon files from: $projectImagesPath"
Write-Host "Copying icon files to: $imagesPath"
$null = New-Item -ItemType Directory -Path $imagesPath -Force
if (Test-Path $projectImagesPath) {
    $iconFiles = Get-ChildItem -Path $projectImagesPath -Filter "*.ico" -File
    foreach ($file in $iconFiles) {
        Copy-Item -Path $file.FullName -Destination $imagesPath -Force
        Write-Host "  Copied: $($file.Name)"
    }
} else {
    Write-Warning "Images directory not found: $projectImagesPath"
}

Write-Host "`nPost-build deployment completed successfully!"


