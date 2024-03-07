using Profiler.Shared.Enums;
using Profiler.Shared.Exceptions;
using Profiler.Shared.Utils;

namespace Swig.Console.FileSystem;

public class FileSystemManager
{
    private String WorkingFolder { get; } = "Profiler";
    public DirectoryInfo WorkingDirectory { get; private set; }
    
    public FileSystemManager()
    {
        WorkingDirectory = SetupAndConfigureFs(GetBaseDirectoryInfo(), WorkingFolder);
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
        catch (Exception e) { }

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
        catch (Exception e) { }

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
            throw new FileSystemException(EnumFileSystemExceptionType.CannotWriteFile);
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

        throw new FileSystemException(EnumFileSystemExceptionType.FolderNotFound);
    }
    
    private DirectoryInfo GetBaseDirectoryInfo()
    {
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string configFolderName = ".config";
        
        return new DirectoryInfo(Path.Combine(userProfile, configFolderName));
    }
}