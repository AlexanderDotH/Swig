using Spectre.Console;

namespace Profiler.Console.Style.Layouts;

public class MainLayout : ILayout
{
    public void DrawLayout()
    {
        // Layout leftLayout = new Layout("Left");
        // Layout rightLayout = new Layout("Right");
        //
        // Layout rootLayout =
        //     new Layout("Root")
        //         .SplitColumns(leftLayout, rightLayout);

        SelectionPrompt<string> actionSelectonPrompt = new SelectionPrompt<string>()
            .Title("What do [mediumturquoise]you[/] want to do?")
            .AddChoices("Load", "Create", "Edit", "Delete");

        string choice = AnsiConsole.Prompt(actionSelectonPrompt);
        
        //AnsiConsole.Write(rootLayout);
    }
}