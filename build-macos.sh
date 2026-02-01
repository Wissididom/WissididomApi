# https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
dotnet publish -c Release -r osx-x64 --self-contained
dotnet publish -c Release -r osx-arm64 --self-contained
