using Spectre.Console;
using Swig.Console.Helper;
using Swig.Console.Style.Models.Children;
using Swig.Console.Style.Models.Children.EditProfile;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Style.Layouts.Children.EditProfile;

public class EditProfileSelectionLayout : BaseChildLayout
{
    private EditProfileSelectionModel Model { get; set; }
    
    public EditProfileSelectionLayout(ILayout parent) : base(parent)
    {
        this.Model = new EditProfileSelectionModel();
    }

    public override void DrawLayout()
    {
        SelectionPrompt<string> profileSelectionPrompt = new SelectionPrompt<string>()
            .Title(this.Model.ProfileSelectionPrompt)
            .AddChoices(this.FormattedBackLabel)
            .AddChoices(this.Model.GetChoices());

        string choice = AnsiConsole.Prompt(profileSelectionPrompt);

        ChoiceHelper.Choice(choice, this.FormattedBackLabel, () => this.DrawParent());
        
        Profile selectedProfile = Swig.Instance.ProfileManager.GetProfileByName(choice);
        new EditProfileActionLayout(this, selectedProfile).DrawLayout();
    }
}