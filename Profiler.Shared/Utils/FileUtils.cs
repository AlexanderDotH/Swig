namespace Profiler.Shared.Utils;

public static class FileUtils
{
    public static DirectoryInfo CombineDirectories(DirectoryInfo directory1, string folderName)
    {
        if (folderName.Equals("/"))
            return directory1;
        
        string combinded = Path.Combine(directory1.FullName, folderName);
        return new DirectoryInfo(combinded);
    }

    public static FileInfo CombineFile(DirectoryInfo directory, string fileName)
    {
        string combinded = Path.Combine(directory.FullName, fileName);
        return new FileInfo(combinded);
    }
}