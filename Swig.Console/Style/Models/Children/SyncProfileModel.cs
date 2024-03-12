using Microsoft.Extensions.Logging;
using Spectre.Console.Extensions.Logging;
using Swig.Shared.Classes;
using Swig.Shared.Utils;

namespace Swig.Console.Style.Models.Children;

public class SyncProfileModel : ILayoutModel
{
    public readonly string SyncPromptString =
        "Your current gitconfig has been [mediumturquoise]changed[/], do you want to sync it with your profile?";
    
    public readonly string SuccessProfileSyncString = "[mediumturquoise]Successfully[/] synced your profile config";
    public readonly string ErrorProfileSyncString = "[red1]Cannot sync your profile config[/]";
     
    private readonly ILogger _logger = 
        new SpectreInlineLogger("SyncProfileModel", Swig.Instance.LoggerConfiguration);
    
    public bool SyncProfile(Profile profile)
    {
        try
        {
            string gitConfigPath = GitUtils.GetOriginGitConfigPath();
            Swig.Instance.ProfileManager.SyncGitConfig(gitConfigPath, profile);

            _logger.LogInformation("Synced profile {}!", profile.Name);
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot sync profile");
            return false;
        }
    }
    
}