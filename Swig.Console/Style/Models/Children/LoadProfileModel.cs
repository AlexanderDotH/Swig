using Microsoft.Extensions.Logging;
using Spectre.Console.Extensions.Logging;
using Swig.Shared.Classes;

namespace Swig.Console.Style.Models.Children;

public class LoadProfileModel : BaseProfileSelectionModel
{
    public readonly string ChooseProfilePromptString = "Please choose a [mediumturquoise]profile[/]";
    public readonly string SuccessProfileLoadString = "[mediumturquoise]Successfully[/] loaded profile {Name}!";
    public readonly string ErrorProfileLoadString = "[red1]Cannot load profile {0}, the profiles does not exist[/]";

    private readonly ILogger _logger = 
        new SpectreInlineLogger("LoadProfileModel", Swig.Instance.LoggerConfiguration);
    
    public bool LoadProfile(string choice, out Profile profile)
    {
        try
        {
            profile = Swig.Instance.ProfileManager.LoadProfile(choice);
            return true;
        }
        catch (Exception e)
        {
            profile = null;
            _logger.LogError(e, "Cannot load profile");
            return false;
        }
    }
}