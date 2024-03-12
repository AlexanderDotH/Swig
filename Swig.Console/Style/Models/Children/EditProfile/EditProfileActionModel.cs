using System.Runtime.InteropServices;

namespace Swig.Console.Style.Models.Children.EditProfile;

public class EditProfileActionModel : ILayoutModel
{
    public readonly string EditActionPromptString = "What do you want to do with [mediumturquoise]{Name}[/]?";
    public readonly string RenameActionString = "Rename";
    public readonly string DeleteActionString = "Delete";
    
    public string GetOsSpecificOpenLabel()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return "Open in finder";
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "Open in explorer";
        
        return "Open in file view";
    }
}