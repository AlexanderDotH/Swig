using System.Reflection;
using Profiler.Console.FileSystem;
using Profiler.Shared.Classes;
using Profiler.Shared.Enums;

namespace Profiler.Console.Configuration;

public class ProfileManager
{
    private ProfileRegistry ProfileRegistry { get; set; }
    private FileSystemManager FileSystemManager { get; set; }
    
    public ProfileManager(ProfileRegistry profileRegistry, FileSystemManager fileSystemManager)
    {
        this.ProfileRegistry = profileRegistry;
        this.FileSystemManager = fileSystemManager;
    }
}