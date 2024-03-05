using Profiler.Console.Style.Models.Children;
using Spectre.Console;
using Profile = Profiler.Shared.Classes.Profile;

namespace Profiler.Console.Style.Layouts.Children;

public class EditProfileSelectionLayout : BaseChildLayout
{
    private BaseProfileSelectionModel Model { get; set; }
    
    public EditProfileSelectionLayout(ILayout parent) : base(parent)
    {
        this.Model = new BaseProfileSelectionModel();
    }

    public override void DrawLayout()
    {
        SelectionPrompt<string> profileSelectionPrompt = new SelectionPrompt<string>()
            .Title("Please pick a [mediumturquoise]profile[/]")
            .AddChoices(":backhand_index_pointing_left: Go back")
            .AddChoices(this.Model.GetChoices());

        string choice = AnsiConsole.Prompt(profileSelectionPrompt);
        
        switch (choice)
        {
            case ":backhand_index_pointing_left: Go back":
            {
                this.DrawParent();
                break;
            }
            default:
            {
                Profile selectedProfile = Profiler.Instance.ProfileManager.GetProfileByName(choice);
                new EditProfileActionLayout(this, selectedProfile).DrawLayout();
                break;
            }
        }
    }
}