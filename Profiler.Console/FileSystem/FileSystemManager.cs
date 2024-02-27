using Profiler.Shared.Enums;
using Profiler.Shared.Exceptions;
using Profiler.Shared.Utils;

namespace Profiler.Console.FileSystem;

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

    private DirectoryInfo CreateSubDirectory(DirectoryInfo directory) => 
        Directory.CreateDirectory(directory.FullName);
    
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
        String userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        String configFolderName = ".config";
        
        return new DirectoryInfo(Path.Combine(userProfile, configFolderName));
    }
}