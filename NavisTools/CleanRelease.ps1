param(
    [string]$NavisVersion
)

$bundlePath = "$env:ProgramData\Autodesk\ApplicationPlugins\NavisTools.bundle"
$contentPath = "$bundlePath\Contents\$NavisVersion"

# Remove version-specific content directory if it exists
if (Test-Path $contentPath) {
    Write-Host "Cleaning release bundle directory: $contentPath"
    try {
        Remove-Item -Path $contentPath -Recurse -Force -ErrorAction Stop
        Write-Host "Successfully cleaned release bundle directory."
    }
    catch {
        Write-Warning "Failed to clean release bundle directory: $_"
        exit 1
    }
}
else {
    Write-Host "Release bundle directory does not exist, nothing to clean: $contentPath"
}

Write-Host "Clean completed successfully!"
