#!/bin/bash
# install-deps.sh
# Script to install .NET dependencies for the TodoMauiApp project on Linux/macOS

set -e
echo "Installing dependencies for TodoMauiApp..."

# Function to check if dotnet is available
check_dotnet() {
    if command -v dotnet &> /dev/null; then
        DOTNET_VERSION=$(dotnet --version)
        echo "Found .NET SDK version: $DOTNET_VERSION"
        return 0
    else
        echo "Warning: dotnet command not found in PATH"
        return 1
    fi
}

# If dotnet is not in PATH, try to add it
if ! check_dotnet; then
    POSSIBLE_PATHS=(
        "/usr/local/share/dotnet"
        "/usr/share/dotnet"
        "$HOME/.dotnet"
    )
    
    for path in "${POSSIBLE_PATHS[@]}"; do
        if [ -f "$path/dotnet" ]; then
            echo "Adding $path to PATH"
            export PATH="$path:$PATH"
            
            # For persistence, also update user profile
            echo "export PATH=\"$path:\$PATH\"" >> ~/.bashrc
            
            if check_dotnet; then
                break
            fi
        fi
    done
fi

# If we still don't have dotnet, offer to install it
if ! check_dotnet; then
    echo "Warning: Could not find dotnet in any standard location."
    read -p "Would you like to download and install .NET SDK? (y/n) " INSTALL
    
    if [ "$INSTALL" = "y" ]; then
        INSTALL_SCRIPT="/tmp/dotnet-install.sh"
        echo "Downloading .NET installer..."
        curl -sSL https://dot.net/v1/dotnet-install.sh -o $INSTALL_SCRIPT
        chmod +x $INSTALL_SCRIPT
        
        echo "Installing .NET 7.0 SDK for MAUI support..."
        $INSTALL_SCRIPT --channel 7.0
        
        echo "Installing .NET 8.0 SDK..."
        $INSTALL_SCRIPT --channel 8.0
        
        # Add to PATH for current session
        export PATH="$HOME/.dotnet:$PATH"
        
        if check_dotnet; then
            echo ".NET SDK installed successfully."
        else
            echo "Error: Failed to install .NET SDK. Please install manually from https://dotnet.microsoft.com/download"
            exit 1
        fi
    else
        echo "Error: Cannot continue without .NET SDK. Please install manually from https://dotnet.microsoft.com/download"
        exit 1
    fi
fi

# Install required .NET workloads for MAUI
echo "Installing required .NET workloads for MAUI..."
dotnet workload install maui --skip-manifest-update

# Restore NuGet packages
echo "Restoring NuGet packages..."
dotnet restore TodoCodexPoc.sln

echo "Dependencies installed successfully!" 