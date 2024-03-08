using Spectre.Console;
using Swig.Shared.Utils;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Style.Layouts.Children;

public class SyncProfileLayout : BaseChildLayout
{
    private Profile Profile { get; set; }
    
    public SyncProfileLayout(ILayout parent, Profile profile) : base(parent)
    {
        this.Profile = profile;
    }

    public override void DrawLayout()
    {
        bool confirmSync =
            AnsiConsole.Confirm("Your current gitconfig has been [mediumturquoise]changed[/], do you want to sync it with your profile?", true);

        if (!confirmSync)
        {
            return;
        }
        
        try
        {
            string gitConfigPath = GitUtils.GetOriginGitConfigPath();
            Swig.Instance.ProfileManager.SyncGitConfig(gitConfigPath, this.Profile);
            
            AnsiConsole.MarkupLine("[mediumturquoise]Successfully[/] synced your profile config");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine("[red1]Cannot sync your profile config[/]");
        }
    }
}