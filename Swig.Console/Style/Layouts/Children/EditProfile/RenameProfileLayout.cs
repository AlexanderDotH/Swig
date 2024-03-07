using Bogus.DataSets;
using Spectre.Console;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Style.Layouts.Children.EditProfile;

public class RenameProfileLayout : BaseChildLayout
{
    private Profile Profile { get; set; }
    
    public RenameProfileLayout(ILayout parent, Profile profile) : base(parent)
    {
        this.Profile = profile;
    }

    public override void DrawLayout()
    {
        AnsiConsole.MarkupLine($"How should we call [mediumturquoise]{this.Profile.Name}[/]?");

        string newName = AnsiConsole.Ask<string>("Please provide a creative but silly name", new Hacker().Noun());

        if (Swig.Instance.ProfileManager.DoesProfileExist(newName))
        {
            AnsiConsole.Markup("[red1]Please provide a non duplicated profile names[/]");
            AnsiConsole.Clear();
            System.Console.ReadKey();
            DrawLayout();
        }
        
        Swig.Instance.ProfileManager.RenameProfile(this.Profile, newName);
        
        this.DrawParent();
    }
}