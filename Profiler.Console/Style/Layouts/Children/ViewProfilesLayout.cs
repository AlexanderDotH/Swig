using Profiler.Console.Style.Models.Children;
using Spectre.Console;

namespace Profiler.Console.Style.Layouts.Children;

public class ViewProfilesLayout : BaseChildLayout
{
    private ViewProfilesModel Model { get; set; }

    public ViewProfilesLayout(ILayout parent) : base(parent)
    {
        this.Model = new ViewProfilesModel();
    }

    public override void DrawLayout()
    {
        Layout leftLayout = new Layout("View Profiles");
        leftLayout.Name = "Profiles";

        leftLayout.Update(this.Model.GetProfileTable());
        
        AnsiConsole.Write(leftLayout);

        System.Console.ReadKey();
        
        DrawParent();
    }
}