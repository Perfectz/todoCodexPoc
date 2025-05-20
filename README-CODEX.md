# Codex Setup Guide for TodoMauiApp

This document explains how to set up the TodoMauiApp project in a Codex environment using the provided setup scripts.

## Prerequisites

- A Codex environment or container
- Offline .NET SDK packages (optional, will download if not available)

## Setup Process

### 1. Copy Offline .NET SDK Packages (Optional)

If you want to use offline packages:

- For Windows: Place .NET SDK `.zip` files in `/tmp/dotnet/` 
- For Linux/macOS: Place .NET SDK `.tar.gz` files in `/tmp/dotnet/`

### 2. Run the Setup Script

Based on your environment, run one of the following scripts:

#### Windows

```powershell
./setup-dotnet.ps1
```

#### Linux/macOS

```bash
chmod +x ./setup-dotnet.sh
./setup-dotnet.sh
```

### 3. Configure Codex

In your Codex configuration, specify the setup script path:

```json
{
  "setup": {
    "script": "setup-dotnet.ps1"  // Or "setup-dotnet.sh" for Linux/macOS
  }
}
```

## What the Scripts Do

1. **setup-dotnet.ps1 / setup-dotnet.sh**
   - Installs .NET SDK from offline packages if available
   - Falls back to downloading the SDK if offline packages aren't found
   - Adds .NET to the PATH
   - Runs project-specific setup (install-deps.ps1 / install-deps.sh)
   - Installs MAUI workload
   - Restores NuGet packages

2. **install-deps.ps1 / install-deps.sh**
   - Checks if .NET SDK is available
   - Tries to find .NET in various standard locations
   - Offers to install .NET SDK if not found
   - Installs MAUI workload
   - Restores NuGet packages for the solution

## Troubleshooting

If you encounter issues:

1. Verify the scripts have execute permissions
2. Check for error messages in the output
3. Ensure the offline packages are correctly placed (if using offline installation)
4. Make sure your container has internet access (if using online installation)

## Manual Installation

If the scripts fail, you can manually install .NET SDK:

1. Download from https://dotnet.microsoft.com/download
2. Install MAUI workload: `dotnet workload install maui`
3. Restore packages: `dotnet restore TodoCodexPoc.sln` 