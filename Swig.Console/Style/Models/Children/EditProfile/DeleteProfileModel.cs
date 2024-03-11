namespace Swig.Console.Style.Models.Children.EditProfile;

public class DeleteProfileModel : BaseProfileSelectionModel
{
    public string ConfirmDeletionString => "Please confirm deletion with the profile name [red1]{Name}[/]";
    public string NameMismatchString => "[red1]Your entered name does not match[/] {Name}";
    public string DeletedProfileString => "Successfully deleted profile [red1]{Name}[/]!";
}