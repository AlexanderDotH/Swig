using Profiler.Console.Style.Models.Children;
using Spectre.Console;

namespace Profiler.Console.Style.Layouts.Children;

public class LoadProfileLayout : BaseChildLayout
{
    private BaseProfileSelectionModel Model { get; set; }

    public LoadProfileLayout(ILayout parent) : base(parent)
    {
        Model = new BaseProfileSelectionModel();
    }

    public override void DrawLayout()
    {
        SelectionPrompt<string> profileSelectionPrompt = new SelectionPrompt<string>()
            .Title("Please choose a [mediumturquoise]profile[/]")
            .AddChoices(this.Model.GetChoices());

        string choice = AnsiConsole.Prompt(profileSelectionPrompt);
        
        DrawParent();
    }
}