namespace Swig.Console.Style.Models.Children;

public class RestoreBackupModel : ILayoutModel
{
    public readonly string SuccessRestoreString = "[mediumturquoise]Successfully[/] restored backup!";
    public readonly string ErrorRestoreString = 
        "[red1]Cannot[/] restored backup because there is no backup copy {0:choose(True|False)::sad_but_relieved_face:|}.";
}