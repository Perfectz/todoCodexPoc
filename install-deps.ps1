# Install .NET SDK 8.0
./dotnet-install.ps1 -Channel 8.0

# Add dotnet to path
$env:Path = "$env:Path;C:\Users\pzgam\AppData\Local\Microsoft\dotnet"

# Verify installation
dotnet --info

# Restore dependencies
dotnet restore

# Install MAUI workload if needed
dotnet workload install maui 