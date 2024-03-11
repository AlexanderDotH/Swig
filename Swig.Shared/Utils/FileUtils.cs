namespace Swig.Shared.Utils;

public class FileUtils
{
    protected FileUtils() { }
    
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

    public static string ReadContent(string filePath) => ReadContent(new FileInfo(filePath));
    
    public static string ReadContent(FileInfo fileInfo)
    {
        if (!fileInfo.Exists)
            throw new FileNotFoundException($"Cannot find file {fileInfo.FullName}");

        return File.ReadAllText(fileInfo.FullName);
    }
}