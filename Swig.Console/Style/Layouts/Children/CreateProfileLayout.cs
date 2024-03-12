using System.Runtime.InteropServices;
using Bogus.DataSets;
using DevBase.Typography;
using SmartFormat;
using Spectre.Console;
using Swig.Console.Style.Models.Children;
using Swig.Shared.Utils;

namespace Swig.Console.Style.Layouts.Children;

public class CreateProfileLayout : BaseChildLayout
{
    private CreateProfileModel Model { get; set; }

    public CreateProfileLayout(ILayout parent) : base(parent)
    {
        this.Model = new CreateProfileModel();
    }

    public override void DrawLayout()
    {
        string profilePromptLabel = Smart.Format(this.Model.ProfileNamePromptString,
            Swig.Instance.AreEmojisAllowed);

        AString placeholder = new AString(new Hacker().Noun());
        
        string profileName = AnsiConsole.Ask<string>(profilePromptLabel, placeholder.CapitalizeFirst());

        if (Swig.Instance.ProfileManager.DoesProfileExist(profileName))
        {
            AnsiConsole.Markup(this.Model.InvalidNameString);
            System.Console.ReadKey();
            AnsiConsole.Clear();
            DrawLayout();
        }
        
        bool importFromDisk = AnsiConsole.Confirm(this.Model.ImportGitConfigPrompt, false);

        string configPath = null;

        if (importFromDisk)
        {
            configPath = AnsiConsole.Ask<string>(this.Model.ProvidePathPrompt, 
                GitUtils.IsGlobalConfigAvailable() ? GitUtils.GetGlobalGitConfigPath() : null);
        }

        Swig.Instance.ProfileManager.CreateNewProfile(profileName, configPath);
        
        DrawParent();
    }
}