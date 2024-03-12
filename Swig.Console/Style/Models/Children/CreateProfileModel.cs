namespace Swig.Console.Style.Models.Children;

public class CreateProfileModel : ILayoutModel
{
    public readonly string ProfileNamePromptString =
        "How do you want to call [mediumturquoise]you[/] new profile? {0:choose(True|False)::cat_with_wry_smile:|} ";

    public readonly string InvalidNameString = "[red1]Please provide a non duplicated profile name[/]";
    public readonly string ImportGitConfigPrompt = "Do you want to import the git config [mediumturquoise]from disk[/]?";
    public readonly string ProvidePathPrompt = "Please provide a valid [mediumturquoise].gitconfig[/]";
}