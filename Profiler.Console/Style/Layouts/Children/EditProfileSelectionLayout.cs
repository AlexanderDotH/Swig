using Profiler.Console.Style.Models.Children;
using Spectre.Console;

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
            .AddChoices(this.Model.GetChoices());

        string choice = AnsiConsole.Prompt(profileSelectionPrompt);
    }
}