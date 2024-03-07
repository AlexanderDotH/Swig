using Spectre.Console;
using Swig.Console.Style.Models.Children;
using Swig.Shared.Utils;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Style.Layouts.Children;

public class LoadProfileLayout : BaseChildLayout
{
    private BaseProfileSelectionModel Model { get; set; }

    public LoadProfileLayout(ILayout parent) : base(parent)
    {
        Model = new BaseProfileSelectionModel();
    }

    public override void DrawLayout()
    {
        SelectionPrompt<string> profileSelectionPrompt = new SelectionPrompt<string>()
            .Title("Please choose a [mediumturquoise]profile[/]")
            .AddChoices(":backhand_index_pointing_left: Go back")
            .AddChoices(this.Model.GetChoices());

        string choice = AnsiConsole.Prompt(profileSelectionPrompt);
        
        if (choice.SequenceEqual(":backhand_index_pointing_left: Go back"))
            DrawParent();

        Profile profile = Swig.Instance.ProfileManager.GetProfileByName(choice);
        GitUtils.SetGlobalGitConfigPath(new FileInfo(profile.GitConfigFile));
        
        AnsiConsole.MarkupLine($"[mediumturquoise]Successfully[/] loaded profile {profile.Name}!");
        System.Console.ReadKey();
        DrawParent();
    }
}