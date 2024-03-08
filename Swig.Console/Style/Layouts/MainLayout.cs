using Spectre.Console;
using Swig.Console.Style.Layouts.Children;
using Swig.Console.Style.Layouts.Children.EditProfile;
using Swig.Console.Style.Models;

namespace Swig.Console.Style.Layouts;

public class MainLayout : ILayout
{
    private MainLayoutModel Model { get; set; }
    
    public MainLayout()
    {
        this.Model = new MainLayoutModel();
    }
    
    public void DrawLayout()
    {
        
        SelectionPrompt<string> actionSelectonPrompt = new SelectionPrompt<string>()
            .Title(this.Model.GetTitle())
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
                if (Swig.Instance.ProfileManager.RestoreBackup())
                {
                    AnsiConsole.MarkupLine("[mediumturquoise]Successfully[/] restored backup!");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red1]Cannot[/] restored backup because there is no backup copy :sad_but_relieved_face:.");
                }
                
                System.Console.ReadKey();
                AnsiConsole.Clear();
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