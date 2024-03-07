using System.Linq.Expressions;
using System.Text;
using Profiler.Shared.Enums;

namespace Profiler.Shared.Exceptions;

public class FileSystemException : Exception
{
    public FileSystemException(EnumFileSystemExceptionType fileSystemExceptionType) : base(GetMessage(fileSystemExceptionType)) { }

    private static string GetMessage(EnumFileSystemExceptionType fileSystemExceptionType)
    {
        switch (fileSystemExceptionType)
        {
            case EnumFileSystemExceptionType.FolderNotFound:
                return "Cannot find folder";
            case EnumFileSystemExceptionType.CannotWriteFile:
                return "Cannot write file content to file";
            case EnumFileSystemExceptionType.CannotReadFile:
                return "Cannot read file content";
        }

        return string.Empty;
    }
}