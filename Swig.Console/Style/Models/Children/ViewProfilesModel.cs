using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Extensions.Logging;
using Swig.Shared.Utils;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Style.Models.Children;

public class ViewProfilesModel : ILayoutModel
{
    private readonly ILogger _logger = 
        new SpectreConsoleLogger("ViewProfilesModel", Swig.Instance.LoggerConfiguration);

    public Table GetProfileTable()
    {
        List<Profile> profiles = Swig.Instance.ProfileManager.Profiles;

        if (!profiles.Any())
        {
            _logger.LogError($"Cannot find any profiles (Entries present {profiles.Count})");
            return null;
        }
        
        Table table = new Table();
        table.Border = TableBorder.Square;
        
        table.AddColumn(new TableColumn("Number"));
        table.AddColumn(new TableColumn("Name"));
        table.AddColumn(new TableColumn("Content"));

        for (int i = 0; i < profiles.Count; i++)
        {
            Profile currentProfile = profiles[i];

            AnsiConsole.MarkupLine($"Loaded profile {currentProfile.Name}");
            
            Markup currentNumber = new Markup(i.ToString());
            Markup currentName = new Markup(currentProfile.Name);
            Table currentTable = GetContentTable(currentProfile);
            
            _logger.LogDebug($"Added Table entry No: {currentNumber}, Name: {currentName}, Object: {currentTable}");
            
            table.AddRow(currentNumber, currentName, currentTable);
        }

        return table;
    }

    private Table GetContentTable(Profile profile)
    {
        Table contentTable = new Table();

        FileInfo gitConfigPathInfo = new FileInfo(profile.GitConfigFile);

        if (!gitConfigPathInfo.Exists)
        {
            return new Table()
                .AddColumns("Error")
                .AddRow(new Markup("[red1]Cannot load gitconfig[/]"));
        }
        
        contentTable.BorderColor(new Color(95, 0, 255));
            
        contentTable.AddColumn("Key");
        contentTable.AddColumn("Value");

        List<KeyValuePair<string, string>> gitContent = GitUtils.GetGitContent(gitConfigPathInfo);
            
        foreach (var (key, value) in gitContent)
            contentTable.AddRow(key, value);

        return contentTable;
    }
}