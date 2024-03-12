#!/bin/bash

PROJECT_FILE="../Swig.sln"
OUTPUT_DIR="bin"
APP_NAME="Swig"

set -e

echo "Cleaning previous build..."
rm -rf $OUTPUT_DIR

echo "Restoring dependencies..."
dotnet restore $PROJECT_FILE

echo "Publishing..."
dotnet publish $PROJECT_FILE -c Release -r osx-x64 /p:PublishReadyToRun=true /p:PublishTrimmed=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o $APP_NAME/OSX
dotnet publish $PROJECT_FILE -c Release -r win-x64 /p:PublishReadyToRun=true /p:PublishTrimmed=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o $APP_NAME/Windows
dotnet publish $PROJECT_FILE -c Release -r win-x86 /p:PublishReadyToRun=true /p:PublishTrimmed=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o $APP_NAME/Windows
dotnet publish $PROJECT_FILE -c Release -r linux-x64 /p:PublishReadyToRun=true /p:PublishTrimmed=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o $APP_NAME/Linux

echo "Build complete!"
