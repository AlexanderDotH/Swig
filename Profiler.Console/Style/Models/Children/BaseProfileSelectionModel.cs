using Profiler.Shared.Classes;

namespace Profiler.Console.Style.Models.Children;

public class BaseProfileSelectionModel : ILayoutModel
{
    public string[] GetChoices()
    {
        List<Profile> profiles = Profiler.Instance.ProfileManager.Profiles;
        return profiles.Select(p => p.Name).ToArray();
    }
}