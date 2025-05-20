#!/bin/bash
# setup-dotnet.sh
# Script to set up .NET environment for Codex in Linux/macOS containers

set -e
echo "Starting .NET environment setup for Codex..."

# Create necessary directories
TEMP_DIR="/tmp/dotnet"
INSTALL_DIR="$HOME/.dotnet"
mkdir -p $INSTALL_DIR

# If offline packages exist, install from them
if [ -d "$TEMP_DIR" ]; then
    echo "Installing .NET SDK from offline packages in $TEMP_DIR"
    for package in $TEMP_DIR/*.tar.gz; do
        if [ -f "$package" ]; then
            echo "Extracting $package..."
            tar -xzf "$package" -C $INSTALL_DIR
        fi
    done
else
    # Fall back to online installer
    echo "No offline packages found, using online installer..."
    INSTALL_SCRIPT="/tmp/dotnet-install.sh"
    
    # Download the installation script
    curl -sSL https://dot.net/v1/dotnet-install.sh -o $INSTALL_SCRIPT
    chmod +x $INSTALL_SCRIPT
    
    # Install .NET 7.0 SDK (for MAUI) and .NET 8.0 SDK (latest)
    $INSTALL_SCRIPT --channel 7.0 --install-dir $INSTALL_DIR
    $INSTALL_SCRIPT --channel 8.0 --install-dir $INSTALL_DIR
fi

# Add .NET to the PATH
export PATH="$INSTALL_DIR:$PATH"
echo "export PATH=\"$INSTALL_DIR:\$PATH\"" >> ~/.bashrc

# Run our local dependencies setup script if it exists
if [ -f "./install-deps.sh" ]; then
    echo "Running project-specific dependency setup..."
    bash ./install-deps.sh
else
    echo "No project-specific install-deps.sh found."
fi

# Install MAUI workload if not already installed
echo "Checking and installing MAUI workload..."
dotnet workload install maui --skip-manifest-update

# Restore packages for the solution
echo "Restoring packages for the solution..."
dotnet restore *.sln

echo ".NET environment setup completed successfully." 