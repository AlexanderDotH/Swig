namespace Swig.Console.Style.Models.Children.EditProfile;

public class DeleteProfileModel : BaseProfileSelectionModel
{
    public readonly string ConfirmDeletionString = "Please confirm deletion with the profile name [red1]{Name}[/]";
    public readonly string NameMismatchString = "[red1]Your entered name does not match \"{Name}\"[/]";
    public readonly string DeletedProfileString = "Successfully deleted profile [red1]{Name}[/]!";
}