namespace Profiler.Console.Configuration;

public class ProfileManager
{
    private ProfileRegistry ProfileRegistry { get; set; }
    private static ProfileManager _profileManager;

    public ProfileManager(ProfileRegistry profileRegistry)
    {
        this.ProfileRegistry = profileRegistry;
    }

    public void CreateNewProfile(string profileName, string gitConfigPath)
    {
        
    }
}