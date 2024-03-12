using System.Text;
using Spectre.Console;
using Swig.Console.Helper;
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
        if (Swig.Instance.RequiresSetup)
            new UserSetupLayout(this).DrawLayout();
        
        SelectionPrompt<string> actionSelectonPrompt = new SelectionPrompt<string>()
            .Title(this.Model.GetTitle())
            .AddChoices(
                this.Model.ViewString, 
                this.Model.LoadString, 
                this.Model.CreateString, 
                this.Model.EditString, 
                this.Model.RestoreString, 
                this.Model.ExitString);

        string choice = AnsiConsole.Prompt(actionSelectonPrompt);

        ChoiceHelper.Choice(choice, this.Model.ViewString, () => new ViewProfilesLayout(this).DrawLayout());
        ChoiceHelper.Choice(choice, this.Model.LoadString, () => new LoadProfileLayout(this).DrawLayout());
        ChoiceHelper.Choice(choice, this.Model.CreateString, () => new CreateProfileLayout(this).DrawLayout());
        ChoiceHelper.Choice(choice, this.Model.EditString, () => new EditProfileSelectionLayout(this).DrawLayout());
        ChoiceHelper.Choice(choice, this.Model.RestoreString, () => new RestoreBackupLayout(this).DrawLayout());
        ChoiceHelper.Choice(choice, this.Model.ExitString, () => Environment.Exit(0));
    }
}