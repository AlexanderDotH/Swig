using Profiler.Shared.Utils;
using Spectre.Console;
using Swig.Console.Style.Models.Children;
using Profile = Profiler.Shared.Classes.Profile;

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
        
        SelectionPrompt<string> editOptionSelectionPrompt = new SelectionPrompt<string>()
            .Title("Please pick a [mediumturquoise]profile[/]")
            .AddChoices(":backhand_index_pointing_left: Go back", "Rename", openLabel, "Delete");

        string choice = AnsiConsole.Prompt(editOptionSelectionPrompt);

        if (choice.SequenceEqual(openLabel))
        {
            DirectoryInfo profileFolder = Profiler.Instance.ProfileManager.PrepareWorkspace(this.Profile);
            OsUtils.OpenFileExplorerAt(profileFolder);
            DrawLayout();
        }
        
        switch (choice)
        {
            case ":backhand_index_pointing_left: Go back":
            {
                this.DrawParent();
                break;
            }
            case "Rename":
            {   
                new RenameProfileLayout(this, this.Profile).DrawLayout();
                break;
            }
            case "Delete":
            {   
                new DeleteProfileLayout(this, this.Profile).DrawLayout();
                break;
            }
        }
        
        
    }
}