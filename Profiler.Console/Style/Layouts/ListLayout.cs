using Spectre.Console;

namespace Profiler.Console.Style.Layouts;

public class ListLayout : ILayout
{
    public void DrawLayout()
    {
        Layout leftLayout = new Layout("Left");
        leftLayout.Name = "Profiles";

        Table table = new Table();
        table.BorderStyle = Spectre.Console.Style.Plain;
        
        table.AddColumn(new TableColumn("Name"));
        table.AddColumn(new TableColumn("Created at"));

        table.AddRow("Profile 1", "Today");
        table.AddRow("Profile 2", "Today");
        table.AddRow("Profile 3", "Today");
        table.AddRow("Profile 4", "Today");

        leftLayout.Update(table);
        
        Layout rightLayout = new Layout("Right");

        Panel guidePanel = new Panel("[lightseagreen]You can choose between one of the options below and select what you want.[/]");
        guidePanel.Header("How to use this thing?", Justify.Center);

        rightLayout.Update(guidePanel);
        
        Layout rootLayout =
            new Layout("Root")
                .SplitColumns(leftLayout, rightLayout);
        
        AnsiConsole.Write(rootLayout);
    }
}