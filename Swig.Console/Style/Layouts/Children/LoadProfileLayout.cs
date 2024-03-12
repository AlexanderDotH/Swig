using SmartFormat;
using Spectre.Console;
using Swig.Console.Helper;
using Swig.Console.Style.Models.Children;
using Swig.Shared.Utils;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Style.Layouts.Children;

public class LoadProfileLayout : BaseChildLayout
{
    private LoadProfileModel Model { get; set; }

    public LoadProfileLayout(ILayout parent) : base(parent)
    {
        Model = new LoadProfileModel();
    }

    public override void DrawLayout()
    {
        SelectionPrompt<string> profileSelectionPrompt = new SelectionPrompt<string>()
            .Title(this.Model.ChooseProfilePromptString)
            .AddChoices(this.FormattedBackLabel)
            .AddChoices(this.Model.GetChoices());

        string choice = AnsiConsole.Prompt(profileSelectionPrompt);
        
        ChoiceHelper.Choice(choice, this.FormattedBackLabel, () => this.DrawParent());

        string gitConfigPath = GitUtils.GetGlobalGitConfigPath();
        Profile currentProfile = Swig.Instance.ProfileManager.Current;
        
        if (Swig.Instance.ProfileManager.HasChanged(gitConfigPath, currentProfile))
            new SyncProfileLayout(this, currentProfile).DrawLayout();

        Profile profile = null;
        
        if (this.Model.LoadProfile(choice, out profile))
        {
            AnsiConsole.MarkupLine(Smart.Format(this.Model.SuccessProfileLoadString, profile));
        }
        else
        {
            AnsiConsole.MarkupLine(Smart.Format(this.Model.ErrorProfileLoadString, choice));
        }
        
        System.Console.ReadKey();
        
        DrawParent();
    }
}