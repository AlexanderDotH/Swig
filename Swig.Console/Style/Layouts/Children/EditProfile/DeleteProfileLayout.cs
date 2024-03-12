using SmartFormat;
using Spectre.Console;
using Swig.Console.Style.Models.Children.EditProfile;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Style.Layouts.Children.EditProfile;

public class DeleteProfileLayout : BaseChildLayout
{
    private DeleteProfileModel Model { get; set; }
    private Profile Profile { get; set; }
    
    public DeleteProfileLayout(ILayout parent, Profile profile) : base(parent)
    {
        this.Profile = profile;
        this.Model = new DeleteProfileModel();
    }

    public override void DrawLayout()
    {
        string profileConfirmation = AnsiConsole.Ask<string>(Smart.Format(this.Model.ConfirmDeletionString, this.Profile));

        if (!this.Profile.Name.SequenceEqual(profileConfirmation))
        {
            AnsiConsole.Markup(Smart.Format(this.Model.NameMismatchString, this.Profile));
            System.Console.ReadKey();
            AnsiConsole.Clear();
            DrawLayout();
        }

        Swig.Instance.ProfileManager.DeleteProfile(this.Profile);
        
        AnsiConsole.Markup(Smart.Format(this.Model.DeletedProfileString, this.Profile));
        System.Console.ReadKey();
        
        if (this.Parent is EditProfileActionLayout layout)
            layout.DrawParent();
    }
}