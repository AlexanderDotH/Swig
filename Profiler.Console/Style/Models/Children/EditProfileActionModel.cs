using System.Runtime.InteropServices;

namespace Profiler.Console.Style.Models.Children;

public class EditProfileActionModel : ILayoutModel
{
    public string GetOsSpecificOpenLabel()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "Open in finder";
        } 
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "Open in explorer";
        }
        
        return "Open in file view";
    }
}