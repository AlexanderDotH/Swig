using Profiler.Console.Style.Layouts.Children;
using Spectre.Console;

namespace Profiler.Console.Style.Layouts;

public class MainLayout : ILayout
{
    public void DrawLayout()
    {
        SelectionPrompt<string> actionSelectonPrompt = new SelectionPrompt<string>()
            .Title("What do [mediumturquoise]you[/] want to do?")
            .AddChoices("Load", "Create", "Edit", "Delete");

        string choice = AnsiConsole.Prompt(actionSelectonPrompt);

        switch (choice)
        {
            case "Create":
            {
                new CreateProfileLayout(this).DrawLayout();
                break;
            }
        }
    }
}