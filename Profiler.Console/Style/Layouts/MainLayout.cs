using Profiler.Console.Style.Layouts.Children;
using Spectre.Console;

namespace Profiler.Console.Style.Layouts;

public class MainLayout : ILayout
{
    public void DrawLayout()
    {
        SelectionPrompt<string> actionSelectonPrompt = new SelectionPrompt<string>()
            .Title("What do [mediumturquoise]you[/] want to do?")
            .AddChoices("View", "Load", "Create", "Edit");

        string choice = AnsiConsole.Prompt(actionSelectonPrompt);

        switch (choice)
        {
            case "View":
            {
                new ViewProfilesLayout(this).DrawLayout();
                break;
            }
            case "Load":
            {
                new LoadProfileLayout(this).DrawLayout();
                break;
            }
            case "Create":
            {
                new CreateProfileLayout(this).DrawLayout();
                break;
            }
            case "Edit":
            {
                new EditProfileSelectionLayout(this).DrawLayout();
                break;
            }
        }
    }
}