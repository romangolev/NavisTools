param(
    [string]$TargetName,
    [string]$NavisVersion
)

$pluginPath = "$env:ProgramFiles\Autodesk\Navisworks Manage $NavisVersion\Plugins\$TargetName"

# Remove existing plugin directory if it exists
if (Test-Path $pluginPath) {
    Write-Host "Cleaning existing plugin directory: $pluginPath"
    try {
        Remove-Item -Path $pluginPath -Recurse -Force -ErrorAction Stop
        Write-Host "Successfully cleaned plugin directory."
    }
    catch {
        Write-Warning "Failed to clean plugin directory: $_"
        exit 1
    }
}
else {
    Write-Host "Plugin directory does not exist, nothing to clean: $pluginPath"
}

Write-Host "Clean completed successfully!"
