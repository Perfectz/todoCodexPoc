#!/usr/bin/env pwsh
# setup-dotnet.ps1
# Script to set up .NET environment for Codex

$ErrorActionPreference = "Stop"
Write-Host "Starting .NET environment setup for Codex..."

# Create necessary directories
$tempDir = "/tmp/dotnet"
$installDir = "$HOME/.dotnet"

# If offline packages exist, install from them
if (Test-Path $tempDir) {
    Write-Host "Installing .NET SDK from offline packages in $tempDir"
    Get-ChildItem -Path $tempDir -Filter "*.zip" | ForEach-Object {
        # Extract the SDK package
        Write-Host "Extracting $_..."
        Expand-Archive -Path $_.FullName -DestinationPath $installDir -Force
    }
} else {
    # Fall back to online installer
    Write-Host "No offline packages found, using online installer..."
    $installScript = "$env:TEMP/dotnet-install.ps1"
    
    # Download the installation script
    Invoke-WebRequest -Uri "https://dot.net/v1/dotnet-install.ps1" -OutFile $installScript
    
    # Install .NET 7.0 SDK (for MAUI) and .NET 8.0 SDK (latest)
    & $installScript -Channel 7.0 -InstallDir $installDir
    & $installScript -Channel 8.0 -InstallDir $installDir
}

# Add .NET to the PATH
$env:PATH = "$installDir;$env:PATH"

# Run our local dependencies setup script if it exists
if (Test-Path "./install-deps.ps1") {
    Write-Host "Running project-specific dependency setup..."
    & ./install-deps.ps1
} else {
    Write-Host "No project-specific install-deps.ps1 found."
}

# Install MAUI workload if not already installed
Write-Host "Checking and installing MAUI workload..."
& dotnet workload install maui --skip-manifest-update

# Restore packages for the solution
Write-Host "Restoring packages for the solution..."
& dotnet restore *.sln

Write-Host ".NET environment setup completed successfully." 