using Profiler.Shared.Utils;
using Spectre.Console;

namespace Profiler.Console.Style.Layouts.Children;

public class CreateProfileLayout : BaseChildLayout
{
    public CreateProfileLayout(ILayout parent) : base(parent) { }

    public override void DrawLayout()
    {
        string profileName = AnsiConsole.Ask<string>("How do you want to call [mediumturquoise]you[/] new profile? :cat_with_wry_smile: ", "Work");

        if (Profiler.Instance.ProfileManager.DoesProfileExist(profileName))
        {
            AnsiConsole.Markup("[red1]Please provide a non duplicated profile names[/]");
            AnsiConsole.Clear();
            System.Console.ReadKey();
            DrawLayout();
        }
        
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
        
        DrawParent();
    }
}