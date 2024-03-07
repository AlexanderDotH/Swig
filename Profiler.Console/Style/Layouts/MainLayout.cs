using LibGit2Sharp;
using Profiler.Console.Style.Layouts.Children;
using Spectre.Console;

namespace Profiler.Console.Style.Layouts;

public class MainLayout : ILayout
{
    public void DrawLayout()
    {
        SelectionPrompt<string> actionSelectonPrompt = new SelectionPrompt<string>()
            .Title("What do [mediumturquoise]you[/] want to do?")
            .AddChoices(":glasses: View", ":file_cabinet: Load", ":high_voltage: Create", ":pencil: Edit", ":fast_reverse_button:  Restore", ":door: Exit");

        string choice = AnsiConsole.Prompt(actionSelectonPrompt);

        switch (choice)
        {
            case ":glasses: View":
            {
                new ViewProfilesLayout(this).DrawLayout();
                break;
            }
            case ":file_cabinet: Load":
            {
                new LoadProfileLayout(this).DrawLayout();
                break;
            }
            case ":high_voltage: Create":
            {
                new CreateProfileLayout(this).DrawLayout();
                break;
            }
            case ":pencil: Edit":
            {
                new EditProfileSelectionLayout(this).DrawLayout();
                break;
            }
            case ":fast_reverse_button:  Restore":
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
            case ":door: Exit":
            {
                Environment.Exit(0);
                break;
            }
        }
    }
}