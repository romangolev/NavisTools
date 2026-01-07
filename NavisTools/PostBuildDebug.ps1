param(
    [string]$TargetDir,
    [string]$ProjectDir,
    [string]$TargetName,
    [string]$NavisVersion
)

$pluginPath = "$env:ProgramFiles\Autodesk\Navisworks Manage $NavisVersion\Plugins\$TargetName"

# Copy DLLs to plugin directory
Write-Host "Copying DLLs to: $pluginPath"
$null = New-Item -ItemType Directory -Path $pluginPath -Force
Copy-Item -Path "$TargetDir*.dll" -Destination $pluginPath -Force

# Copy XAML files to en-US subdirectory
$enUsPath = "$pluginPath\en-US"
Write-Host "Copying XAML files to: $enUsPath"
$null = New-Item -ItemType Directory -Path $enUsPath -Force
Copy-Item -Path "$ProjectDir*.xaml" -Destination $enUsPath -Force

# Copy .name files to en-US subdirectory
Write-Host "Copying .name files to: $enUsPath"
Copy-Item -Path "$ProjectDir*.name" -Destination $enUsPath -Force

# Copy icon files to Images subdirectory
$imagesPath = "$pluginPath\Images"
Write-Host "Copying icon files to: $imagesPath"
$null = New-Item -ItemType Directory -Path $imagesPath -Force
Copy-Item -Path "$ProjectDir\Images\*.ico" -Destination $imagesPath -Force

Write-Host "Post-build deployment completed successfully!"
