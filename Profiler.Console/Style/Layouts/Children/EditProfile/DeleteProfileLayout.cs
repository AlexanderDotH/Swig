using Profiler.Console.Exceptions;
using Profiler.Console.Style.Models;
using Profiler.Console.Style.Models.Children;
using Spectre.Console;
using Profile = Profiler.Shared.Classes.Profile;

namespace Profiler.Console.Style.Layouts.Children;

public class DeleteProfileLayout : BaseChildLayout
{
    private BaseProfileSelectionModel Model { get; set; }
    private Profile Profile { get; set; }
    
    public DeleteProfileLayout(ILayout parent, Profile profile) : base(parent)
    {
        this.Profile = profile;
        this.Model = new BaseProfileSelectionModel();
    }

    public override void DrawLayout()
    {
        string profileConfirmation = AnsiConsole.Ask<string>($"Please confirm deletion with the profile name [red1]{this.Profile.Name}[/]");

        if (!this.Profile.Name.SequenceEqual(profileConfirmation))
        {
            AnsiConsole.Markup($"[red1]Your entered name does not match[/] {this.Profile.Name}");
            Thread.Sleep(TimeSpan.FromSeconds(3));
            AnsiConsole.Clear();
            DrawLayout();
        }

        Profiler.Instance.ProfileManager.DeleteProfile(this.Profile);
        
        AnsiConsole.Markup($"Successfully deleted profile [red1]{this.Profile.Name}[/]!");
        
        DrawParent();
    }
}