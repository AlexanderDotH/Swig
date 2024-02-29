using LibGit2Sharp;

namespace Profiler.Shared.Utils;

public class GitUtils
{
    public static string GetGlobalGitConfigPath()
    {
        IEnumerable<string> configs = GlobalSettings.GetConfigSearchPaths(ConfigurationLevel.Global);
        
        if (!configs.Any())
            return "Profile Directory/.gitconfig";
        
        string configDir = configs.FirstOrDefault();
        return Path.Combine(configDir, ".gitconfig");
    }

    public static bool IsGlobalConfigAvailable()
    {
        IEnumerable<string> configs = GlobalSettings.GetConfigSearchPaths(ConfigurationLevel.Global);
        return configs.Any();
    }
}