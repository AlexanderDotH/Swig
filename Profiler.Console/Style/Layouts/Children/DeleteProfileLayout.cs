using Profiler.Console.Exceptions;
using Profiler.Console.Style.Models;
using Profiler.Console.Style.Models.Children;
using Spectre.Console;
using Profile = Profiler.Shared.Classes.Profile;

namespace Profiler.Console.Style.Layouts.Children;

public class DeleteProfileLayout : BaseChildLayout
{
    private BaseProfileSelectionModel Model { get; set; }
    
    public DeleteProfileLayout(ILayout parent) : base(parent)
    {
        this.Model = new BaseProfileSelectionModel();
    }

    public override void DrawLayout()
    {
        SelectionPrompt<string> profileSelectionPrompt = new SelectionPrompt<string>()
            .Title("Which profile should be [red1]deleted[/]?")
            .AddChoices(this.Model.GetChoices());

        string profileName = AnsiConsole.Prompt(profileSelectionPrompt);
        
        string profileConfirmation = AnsiConsole.Ask<string>($"Please confirm deletion with the profile name [red1]{profileName}[/]");

        if (!profileName.SequenceEqual(profileConfirmation))
        {
            AnsiConsole.Markup($"[red1]Your entered name does not match[/] {profileName}");
            Thread.Sleep(TimeSpan.FromSeconds(3));
            AnsiConsole.Clear();
            DrawLayout();
        }

        Profile profile = Profiler.Instance.ProfileManager.GetProfileByName(profileName);

        if (profile == null)
            throw new ProfileException($"Cannot find selected profile {profileName}");

        Profiler.Instance.ProfileManager.DeleteProfile(profile);
        
        AnsiConsole.Markup($"Successfully deleted profile [red1]{profileName}[/]!");
        
        DrawParent();
    }
}