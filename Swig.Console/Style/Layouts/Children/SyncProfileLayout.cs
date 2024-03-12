using Spectre.Console;
using Swig.Console.Style.Models.Children;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Style.Layouts.Children;

public class SyncProfileLayout : BaseChildLayout
{
    private Profile Profile { get; set; }
    private SyncProfileModel Model { get; set; }
   
    public SyncProfileLayout(ILayout parent, Profile profile) : base(parent)
    {
        this.Model = new SyncProfileModel();
        this.Profile = profile;
    }

    public override void DrawLayout()
    {
        bool confirmSync =
            AnsiConsole.Confirm(this.Model.SyncPromptString, true);

        if (!confirmSync)
            return;

        if (this.Model.SyncProfile(this.Profile))
        {
            AnsiConsole.MarkupLine(this.Model.SuccessProfileSyncString);
        }
        else
        {
            AnsiConsole.MarkupLine(this.Model.ErrorProfileSyncString);
        }
    }
}