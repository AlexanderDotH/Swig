using Bogus.DataSets;
using DevBase.Typography;
using SmartFormat;
using Spectre.Console;
using Swig.Console.Style.Models.Children.EditProfile;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Style.Layouts.Children.EditProfile;

public class RenameProfileLayout : BaseChildLayout
{
    private Profile Profile { get; set; }
    private RenameProfileModel Model { get; set; }
    
    public RenameProfileLayout(ILayout parent, Profile profile) : base(parent)
    {
        this.Model = new RenameProfileModel();
        this.Profile = profile;
    }

    public override void DrawLayout()
    {
        AnsiConsole.MarkupLine(Smart.Format(this.Model.RenameProfilePromptString, this.Profile));

        AString placeholder = new AString(new Hacker().Noun());
        
        string newName = AnsiConsole.Ask<string>(this.Model.NameProfilePromptString, placeholder.CapitalizeFirst());

        if (Swig.Instance.ProfileManager.DoesProfileExist(newName))
        {
            AnsiConsole.Markup(this.Model.InvalidProfileString);
            AnsiConsole.Clear();
            System.Console.ReadKey();
            DrawLayout();
        }
        
        Swig.Instance.ProfileManager.RenameProfile(this.Profile, newName);
        this.DrawParent();
    }
}