# install-deps.ps1
# Script to install .NET dependencies for the TodoMauiApp project

$ErrorActionPreference = "Stop"
Write-Host "Installing dependencies for TodoMauiApp..."

# Function to check if dotnet is available
function Test-DotNet {
    try {
        $dotnetVersion = dotnet --version
        Write-Host "Found .NET SDK version: $dotnetVersion"
        return $true
    }
    catch {
        Write-Warning "dotnet command not found in PATH"
        return $false
    }
}

# If dotnet is not in PATH, try to add it
if (-not (Test-DotNet)) {
    $possiblePaths = @(
        "$env:ProgramFiles\dotnet",
        "${env:ProgramFiles(x86)}\dotnet",
        "$env:USERPROFILE\.dotnet",
        "$HOME\.dotnet",
        "C:\Users\pzgam\AppData\Local\Microsoft\dotnet"
    )
    
    foreach ($path in $possiblePaths) {
        if (Test-Path "$path\dotnet.exe") {
            Write-Host "Adding $path to PATH"
            $env:PATH = "$path;$env:PATH"
            
            # For persistence, also update user PATH
            [Environment]::SetEnvironmentVariable("PATH", "$path;" + [Environment]::GetEnvironmentVariable("PATH", "User"), "User")
            
            if (Test-DotNet) {
                break
            }
        }
    }
}

# If we still don't have dotnet, offer to install it
if (-not (Test-DotNet)) {
    Write-Warning "Could not find dotnet in any standard location."
    $install = Read-Host "Would you like to download and install .NET SDK? (y/n)"
    
    if ($install -eq "y") {
        $installScript = "$env:TEMP\dotnet-install.ps1"
        Write-Host "Downloading .NET installer..."
        Invoke-WebRequest -Uri "https://dot.net/v1/dotnet-install.ps1" -OutFile $installScript
        
        Write-Host "Installing .NET 7.0 SDK for MAUI support..."
        & $installScript -Channel 7.0
        
        Write-Host "Installing .NET 8.0 SDK..."
        & $installScript -Channel 8.0
        
        # Add to PATH for current session
        $env:PATH = "$env:ProgramFiles\dotnet;$env:PATH"
        
        if (Test-DotNet) {
            Write-Host ".NET SDK installed successfully."
        }
        else {
            Write-Error "Failed to install .NET SDK. Please install manually from https://dotnet.microsoft.com/download"
            exit 1
        }
    }
    else {
        Write-Error "Cannot continue without .NET SDK. Please install manually from https://dotnet.microsoft.com/download"
        exit 1
    }
}

# Install required .NET workloads for MAUI
Write-Host "Installing required .NET workloads for MAUI..."
dotnet workload install maui --skip-manifest-update

# Restore NuGet packages
Write-Host "Restoring NuGet packages..."
dotnet restore TodoCodexPoc.sln

Write-Host "Dependencies installed successfully!" 