using Swig.Shared.Classes;

namespace Swig.Console.Style.Models.Children;

public class BaseProfileSelectionModel : ILayoutModel
{
    public string[] GetChoices()
    {
        List<Profile> profiles = Swig.Instance.ProfileManager.Profiles;
        return profiles.Select(p => p.Name).ToArray();
    }
}