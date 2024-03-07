using LibGit2Sharp;

namespace Profiler.Shared.Utils;

public class GitUtils
{
    public static string GetGlobalGitConfigPath()
    {
        IEnumerable<string> configs = GlobalSettings.GetConfigSearchPaths(ConfigurationLevel.Global);
        
        string basePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        if (configs.Any())
            basePath = configs.FirstOrDefault();
        
        return Path.Combine(basePath, ".gitconfig");
    }

    public static string GetOriginGitConfigPath()
    {
        string basePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(basePath, ".gitconfig");
    }
    
    public static bool SetGlobalGitConfigPath(FileInfo fileInfo)
    {
        string globalPath = GetGlobalGitConfigPath();

        try
        {
            fileInfo.CopyTo(globalPath, true);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public static bool IsGlobalConfigAvailable()
    {
        IEnumerable<string> configs = GlobalSettings.GetConfigSearchPaths(ConfigurationLevel.Global);
        return configs.Any();
    }
}