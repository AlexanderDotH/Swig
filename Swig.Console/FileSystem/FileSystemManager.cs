using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Extensions.Logging;
using Swig.Shared.Enums;
using Swig.Shared.Exceptions;
using Swig.Shared.Utils;

namespace Swig.Console.FileSystem;

public class FileSystemManager
{
    private String WorkingFolder { get; } = "Swig";
    public DirectoryInfo WorkingDirectory { get; private set; }
    
    private readonly ILogger _logger = 
        new SpectreConsoleLogger("FileSystemManager", Swig.Instance.LoggerConfiguration);
    
    public FileSystemManager()
    {
        WorkingDirectory = SetupAndConfigureFs(GetBaseDirectoryInfo(), WorkingFolder);
        
        _logger.LogInformation($"Working directory is set up {WorkingDirectory.FullName}");
        _logger.LogDebug("Loaded ctor");
    }
    
    private DirectoryInfo SetupAndConfigureFs(DirectoryInfo baseDirectory, string workingFolder)
    {
        DirectoryInfo workingDirectoryInfo = FileUtils.CombineDirectories(baseDirectory, workingFolder);

        if (!workingDirectoryInfo.Exists)
            workingDirectoryInfo.Create();

        return workingDirectoryInfo;
    }

    public FileInfo CreateAndWriteFile(EnumFileSystemFolder fileSystemFolder, string fileName, string buffer)
    {
        DirectoryInfo folderPath = CreateSubDirectory(GetFolder(fileSystemFolder));
        FileInfo fileInfo = FileUtils.CombineFile(folderPath, fileName);
        
        File.WriteAllText(fileInfo.FullName, buffer);

        return fileInfo;
    }
    
    public FileInfo CreateAndWriteFile(EnumFileSystemFolder fileSystemFolder, string directoryName, string fileName, string buffer)
    {
        DirectoryInfo folderPath = CreateDirectory(fileSystemFolder, directoryName);
        FileInfo destination = FileUtils.CombineFile(folderPath, fileName);
        
        File.WriteAllText(destination.FullName, buffer);

        return destination;
    }
    
    public bool DeleteFile(EnumFileSystemFolder fileSystemFolder, string directoryName, string fileName)
    {
        DirectoryInfo folderPath = FileUtils.CombineDirectories(GetFolder(fileSystemFolder), directoryName);
        FileInfo destination = FileUtils.CombineFile(folderPath, fileName);

        try
        {
            destination.Delete();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Cannot delete file: {destination.FullName}");
        }

        return false;
    }
    
    public bool DeleteDirectory(EnumFileSystemFolder fileSystemFolder, string directoryName)
    {
        DirectoryInfo folderPath = FileUtils.CombineDirectories(GetFolder(fileSystemFolder), directoryName);

        try
        {
            folderPath.Delete();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Cannot delete directory: {folderPath.FullName}");
        }

        return false;
    }
    
    public DirectoryInfo CreateDirectory(EnumFileSystemFolder fileSystemFolder, string directoryName)
    {
        DirectoryInfo newDirectory = FileUtils.CombineDirectories(
            CreateSubDirectory(GetFolder(fileSystemFolder)), directoryName);

        return CreateSubDirectory(newDirectory);
    }

    public FileInfo CopyTo(EnumFileSystemFolder fileSystemFolder, FileInfo source, string directoryName, string fileName)
    {
        DirectoryInfo folderPath = CreateDirectory(fileSystemFolder, directoryName);
        FileInfo destination = FileUtils.CombineFile(folderPath, fileName);

        if (source.Exists)
            return source.CopyTo(destination.FullName);

        return destination;
    }
    
    public bool IsFilePresent(EnumFileSystemFolder fileSystemFolder, string fileName)
    {
        DirectoryInfo folderPath = GetFolder(fileSystemFolder);
        FileInfo fileInfo = FileUtils.CombineFile(folderPath, fileName);

        return fileInfo.Exists;
    }

    public string ReadFile(EnumFileSystemFolder fileSystemFolder, string directoryName, string fileName)
    {
        DirectoryInfo folderPath = CreateDirectory(fileSystemFolder, directoryName);
        FileInfo fileInfo = FileUtils.CombineFile(folderPath, fileName);

        if (!fileInfo.Exists)
            return string.Empty;

        try
        {
            return File.ReadAllText(fileInfo.FullName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not read from file: {fileInfo.FullName}");
            throw new FileSystemException(EnumFileSystemExceptionType.CannotReadFile);
        }
    }
    
    public string ReadFile(EnumFileSystemFolder fileSystemFolder, string fileName)
    {
        DirectoryInfo folderPath = GetFolder(fileSystemFolder);
        FileInfo fileInfo = FileUtils.CombineFile(folderPath, fileName);

        if (!fileInfo.Exists)
            return string.Empty;

        try
        {
            return File.ReadAllText(fileInfo.FullName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not read from file: {fileInfo.FullName}");
            throw new FileSystemException(EnumFileSystemExceptionType.CannotReadFile);
        }
    }

    private DirectoryInfo CreateSubDirectory(DirectoryInfo directory)
    {
        if (!directory.Exists)
            return Directory.CreateDirectory(directory.FullName);

        return directory;
    }
    
    private DirectoryInfo GetFolder(EnumFileSystemFolder fileSystemFolder)
    {
        switch (fileSystemFolder)
        {
            case EnumFileSystemFolder.Root:
                return FileUtils.CombineDirectories(WorkingDirectory, "/");
            case EnumFileSystemFolder.Profiles:
                return FileUtils.CombineDirectories(WorkingDirectory, "Profiles");
        }

        _logger.LogWarning($"Could not find folder type {fileSystemFolder.ToString()}");
        throw new FileSystemException(EnumFileSystemExceptionType.FolderNotFound);
    }
    
    private DirectoryInfo GetBaseDirectoryInfo()
    {
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string configFolderName = ".config";
        
        return new DirectoryInfo(Path.Combine(userProfile, configFolderName));
    }
}