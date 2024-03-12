using SmartFormat;
using Spectre.Console;
using Swig.Console.Style.Models;
using Swig.Console.Style.Models.Children;

namespace Swig.Console.Style.Layouts.Children;

public class UserSetupLayout : BaseChildLayout
{
    private UserSetupModel Model { get; set; }

    public UserSetupLayout(ILayout parent) : base(parent)
    {
        this.Model = new UserSetupModel();
    }

    public override void DrawLayout()
    {
        AnsiConsole.MarkupLine(this.Model.WelcomePromptString);

        string canBeSeenLabel = Smart.Format(this.Model.CanBeSeenString, DateTime.Now.Month == 6);
        
        bool canbeSeen = AnsiConsole.Confirm(canBeSeenLabel, false);
        bool confirm = AnsiConsole.Confirm(canbeSeen ? this.Model.ConfirmCanBeSeenString : this.Model.ConfirmCanNotBeSeenString, canbeSeen);
            
        if (confirm)
        {
            Swig.Instance.AreEmojisAllowed = true;
            Swig.Instance.RequiresSetup = false;
        }
        else
        {
            Swig.Instance.AreEmojisAllowed = false;
            Swig.Instance.RequiresSetup = false;
        }
        
        this.DrawParent();
    }
}