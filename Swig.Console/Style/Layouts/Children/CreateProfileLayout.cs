using Spectre.Console;
using Swig.Shared.Utils;

namespace Swig.Console.Style.Layouts.Children;

public class CreateProfileLayout : BaseChildLayout
{
    public CreateProfileLayout(ILayout parent) : base(parent) { }

    public override void DrawLayout()
    {
        string profileName = AnsiConsole.Ask<string>("How do you want to call [mediumturquoise]you[/] new profile? :cat_with_wry_smile: ", "Work");

        if (Swig.Instance.ProfileManager.DoesProfileExist(profileName))
        {
            AnsiConsole.Markup("[red1]Please provide a non duplicated profile names[/]");
            System.Console.ReadKey();
            AnsiConsole.Clear();
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

        Swig.Instance.ProfileManager.CreateNewProfile(profileName, configPath);
        
        DrawParent();
    }
}