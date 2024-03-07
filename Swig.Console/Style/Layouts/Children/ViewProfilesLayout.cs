using Spectre.Console;
using Swig.Console.Style.Models.Children;

namespace Swig.Console.Style.Layouts.Children;

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

        Table profiles = this.Model.GetProfileTable();

        if (profiles == null)
        {
            leftLayout.Update(new Markup("It's pretty empty here, please come back later [red]:red_heart:[/]"));
        }
        else
        {
            leftLayout.Update(profiles);
        }
        
        AnsiConsole.Write(leftLayout);
        System.Console.ReadKey();
        DrawParent();
    }
}