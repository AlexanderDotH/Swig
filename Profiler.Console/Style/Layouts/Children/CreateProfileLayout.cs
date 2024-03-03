using Profiler.Shared.Utils;
using Spectre.Console;
using Profile = Profiler.Shared.Classes.Profile;

namespace Profiler.Console.Style.Layouts.Children;

public class CreateProfileLayout : ILayout
{
    private ILayout Parent { get; set; }
    
    public CreateProfileLayout(ILayout parent)
    {
        this.Parent = parent;
    }

    public void DrawLayout()
    {
        string profileName = AnsiConsole.Ask<string>("How do you want to call [mediumturquoise]you[/] new profile? :cat_with_wry_smile: ", "Work");

        bool importFromDisk = AnsiConsole.Confirm(
            $"Do you want to import the git config [mediumturquoise]from disk[/]?", false);

        string configPath = null;

        if (importFromDisk)
        {
            configPath = AnsiConsole.Ask<string>(
                "Pleas provide a valid [mediumturquoise].gitconfig[/]", 
                GitUtils.IsGlobalConfigAvailable() ? GitUtils.GetGlobalGitConfigPath() : null);
        }

        Profiler.Instance.ProfileManager.CreateNewProfile(profileName, new FileInfo(configPath));
        
        AnsiConsole.Clear();
        this.Parent.DrawLayout();
    }
}