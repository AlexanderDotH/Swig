using Spectre.Console;
using Profile = Profiler.Shared.Classes.Profile;

namespace Profiler.Console.Style.Models.Children;

public class LoadProfileModel : ILayoutModel
{
    public string[] GetChoices()
    {
        List<Profile> profiles = Profiler.Instance.ProfileManager.Profiles;
        return profiles.Select(p => p.Name).ToArray();
    }
}