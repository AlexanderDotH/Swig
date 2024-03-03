using Profiler.Console.Style.Models;
using Spectre.Console;
using Profile = Profiler.Shared.Classes.Profile;

namespace Profiler.Console.Style.Layouts;

public class ViewProfilesLayout : ILayout
{
    private ViewProfilesModel Model { get; set; }
    private ILayout Parent { get; set; }

    public ViewProfilesLayout(ILayout parent)
    {
        this.Parent = parent;
        this.Model = new ViewProfilesModel();
    }
    
    public void DrawLayout()
    {
        Layout leftLayout = new Layout("Left");
        leftLayout.Name = "Profiles";

        leftLayout.Update(this.Model.GetProfileTable());
        
        Layout rightLayout = new Layout("Right");

        Panel guidePanel = new Panel("[lightseagreen]You can choose between one of the options below and select what you want.[/]");
        guidePanel.Header("How to use this thing?", Justify.Center);

        rightLayout.Update(guidePanel);
        
        Layout rootLayout =
            new Layout("Root")
                .SplitColumns(leftLayout, rightLayout);
        
        AnsiConsole.Write(rootLayout);

        System.Console.ReadKey();
        AnsiConsole.Clear();
        
        this.Parent.DrawLayout();
    }
}