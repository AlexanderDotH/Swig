using Profiler.Console.Style.Layouts.Children;
using Spectre.Console;

namespace Profiler.Console.Style.Layouts;

public class MainLayout : ILayout
{
    public void DrawLayout()
    {
        SelectionPrompt<string> actionSelectonPrompt = new SelectionPrompt<string>()
            .Title("[dodgerblue1]W[/][blueviolet]e[/][slateblue3_1]l[/][royalblue1]c[/][lightslateblue]o[/][mediumpurple]m[/][slateblue1]e[/] to Profiler, what do [mediumturquoise]you[/] want to do?")
            .AddChoices("View", "Load", "Create", "Edit", "Restore", "Exit");

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
            case "Restore":
            {
                if (Profiler.Instance.ProfileManager.RestoreBackup())
                {
                    AnsiConsole.MarkupLine("[mediumturquoise]Successfully[/] restored backup!");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red1]Cannot[/] restored backup because there is no backup copy :sad_but_relieved_face:.");
                }
                
                AnsiConsole.Clear();
                System.Console.ReadKey();
                DrawLayout();
                break;
            }
            case "Exit":
            {
                Environment.Exit(0);
                break;
            }
        }
    }
}