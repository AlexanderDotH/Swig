#!/bin/bash

PROJECT_FILE="../Profiler.sln"
OUTPUT_DIR="bin"
APP_NAME="Profiler"

set -e

echo "Cleaning previous build..."
rm -rf $OUTPUT_DIR

echo "Restoring dependencies..."
dotnet restore $PROJECT_FILE

echo "Publishing..."
dotnet publish $PROJECT_FILE -c Release -r osx-x64 /p:PublishReadyToRun=true /p:PublishTrimmed=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o $APP_NAME

echo "Build complete!"
