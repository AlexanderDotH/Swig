using Spectre.Console;
using Profile = Profiler.Shared.Classes.Profile;

namespace Swig.Console.Style.Models.Children;

public class ViewProfilesModel : ILayoutModel
{
    public Table GetProfileTable()
    {
        List<Profile> profiles = Profiler.Instance.ProfileManager.Profiles;
        
        Table table = new Table();
        table.BorderStyle = Spectre.Console.Style.Plain;

        table.AddColumn(new TableColumn("Number"));
        table.AddColumn(new TableColumn("Name"));
        table.AddColumn(new TableColumn("Identifier"));
        table.AddColumn(new TableColumn("Git config path"));

        for (int i = 0; i < profiles.Count; i++)
        {
            Profile currentProfile = profiles[i];
            table.AddRow($"{i}", currentProfile.Name, $"{currentProfile.Identifier}", currentProfile.GitConfigFile);
        }

        return table;
    }
}