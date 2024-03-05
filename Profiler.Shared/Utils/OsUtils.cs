using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Profiler.Shared.Utils;

public class OsUtils
{
    public static void OpenFileExplorerAt(DirectoryInfo directoryInfo)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.Arguments = directoryInfo.FullName;
            processStartInfo.UseShellExecute = true;
            processStartInfo.FileName = "open";
            
            Process.Start(processStartInfo);
        } 
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start($"explorer.exe {directoryInfo.FullName}");
        } 
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start($"xdg-open {directoryInfo.FullName}");
        }
    }
}