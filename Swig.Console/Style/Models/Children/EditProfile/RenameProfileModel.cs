namespace Swig.Console.Style.Models.Children.EditProfile;

public class RenameProfileModel : ILayoutModel
{
    public readonly string RenameProfilePromptString = "How should we call [mediumturquoise]{Name}[/]?";
    public readonly string NameProfilePromptString = "Please provide a creative but silly name";
    public readonly string InvalidProfileString = "[red1]Please provide a non duplicated profile names[/]";
}