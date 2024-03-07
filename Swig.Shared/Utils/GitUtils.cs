using LibGit2Sharp;

namespace Swig.Shared.Utils;

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

    public static List<KeyValuePair<string, string>> GetGitContent(string gitConfigPath)
    {
        FileInfo gitConfigFileInfo = new FileInfo(gitConfigPath);
        
        Configuration configuration = Configuration.BuildFrom(gitConfigFileInfo.FullName);

        IEnumerator<ConfigurationEntry<string>> entries = configuration
            .Where(t => t.Level.Equals(ConfigurationLevel.Local))
            .GetEnumerator();

        List<KeyValuePair<string, string>> gitEntries = new List<KeyValuePair<string, string>>();
        
        while (entries.MoveNext())
        {
            gitEntries.Add(KeyValuePair.Create(entries.Current.Key, entries.Current.Value));
        }

        return gitEntries;
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