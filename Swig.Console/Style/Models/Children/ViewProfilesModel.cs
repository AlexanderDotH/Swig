using Spectre.Console;
using Swig.Shared.Utils;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Style.Models.Children;

public class ViewProfilesModel : ILayoutModel
{
    public Table GetProfileTable()
    {
        List<Profile> profiles = Swig.Instance.ProfileManager.Profiles;

        if (!profiles.Any())
            return null;
        
        Table table = new Table();
        table.Border = TableBorder.Square;
        
        table.AddColumn(new TableColumn("Number"));
        table.AddColumn(new TableColumn("Name"));
        table.AddColumn(new TableColumn("Content"));

        for (int i = 0; i < profiles.Count; i++)
        {
            Profile currentProfile = profiles[i];

            Markup currentNumber = new Markup(i.ToString());
            Markup currentName = new Markup(currentProfile.Name);
            Table currentTable = GetContentTable(currentProfile);
            
            table.AddRow(currentNumber, currentName, currentTable);
        }

        return table;
    }

    private Table GetContentTable(Profile profile)
    {
        Table contentTable = new Table();
        contentTable.BorderColor(new Color(95, 0, 255));
            
        contentTable.AddColumn("Key");
        contentTable.AddColumn("Value");

        List<KeyValuePair<string, string>> gitContent = GitUtils.GetGitContent(profile.GitConfigFile);
            
        foreach (var (key, value) in gitContent)
            contentTable.AddRow(key, value);

        return contentTable;
    }
}