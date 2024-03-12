@echo off

set PROJECT_FILE="../Swig.sln"
set OUTPUT_DIR="bin"
set APP_NAME="Swig"

echo "Cleaning previous build..."
del %OUTPUT_DIR&

echo "Restoring dependencies..."
dotnet restore %PROJECT_FILE%

echo "Publishing..."
dotnet publish %PROJECT_FILE% -c Release -r osx-x64 /p:PublishReadyToRun=true /p:PublishTrimmed=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o %APP_NAME%\OSX
dotnet publish %PROJECT_FILE% -c Release -r win-x64 /p:PublishReadyToRun=true /p:PublishTrimmed=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o %APP_NAME%\Windows
dotnet publish %PROJECT_FILE% -c Release -r win-x86 /p:PublishReadyToRun=true /p:PublishTrimmed=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o %APP_NAME%\Windows
dotnet publish %PROJECT_FILE% -c Release -r linux-x64 /p:PublishReadyToRun=true /p:PublishTrimmed=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o %APP_NAME%\Linux

echo "Build complete!"
