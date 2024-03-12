using SmartFormat;
using Spectre.Console;
using Swig.Console.Helper;
using Swig.Console.Style.Models.Children.EditProfile;
using Swig.Shared.Utils;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Style.Layouts.Children.EditProfile;

public class EditProfileActionLayout : BaseChildLayout
{
    private Profile Profile { get; set; }
    private EditProfileActionModel Model { get; set; }
    
    public EditProfileActionLayout(ILayout parent, Profile profile) : base(parent)
    {
        this.Profile = profile;
        this.Model = new EditProfileActionModel();
    }

    public override void DrawLayout()
    {
        string openLabel = this.Model.GetOsSpecificOpenLabel();
        string backLabel = this.FormattedBackLabel;
        string renameLabel = this.Model.RenameActionString;
        string deleteLabel = this.Model.DeleteActionString;
        
        SelectionPrompt<string> editOptionSelectionPrompt = new SelectionPrompt<string>()
            .Title(Smart.Format(this.Model.EditActionPromptString, this.Profile))
            .AddChoices(backLabel, renameLabel, openLabel, deleteLabel);

        string choice = AnsiConsole.Prompt(editOptionSelectionPrompt);

        // Back
        ChoiceHelper.Choice(choice, backLabel, () => this.DrawParent());
        
        // Open in explorer
        ChoiceHelper.Choice(choice, openLabel, () =>
        {
            DirectoryInfo profileFolder = Swig.Instance.ProfileManager.PrepareWorkspace(this.Profile);
            OsUtils.OpenFileExplorerAt(profileFolder);
            DrawLayout();
        });
        
        // Rename profile
        ChoiceHelper.Choice(choice, renameLabel, () => new RenameProfileLayout(this, this.Profile).DrawLayout());
        
        // Delete profile
        ChoiceHelper.Choice(choice, deleteLabel, () => new DeleteProfileLayout(this, this.Profile).DrawLayout());
    }
}