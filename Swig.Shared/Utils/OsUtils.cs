using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Swig.Shared.Utils;

public class OsUtils
{
    protected OsUtils() { }
    
    public static void OpenFileExplorerAt(DirectoryInfo directoryInfo)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.Arguments = directoryInfo.FullName;
            processStartInfo.UseShellExecute = true;
            processStartInfo.FileName = "/usr/bin/open";
            
            Process.Start(processStartInfo);
        } 
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.Arguments = $"\"{directoryInfo.FullName}\"";
            processStartInfo.UseShellExecute = true;
            processStartInfo.FileName = "C:\\Windows\\explorer.exe";
            
            Process.Start(processStartInfo);
        } 
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.Arguments = directoryInfo.FullName;
            processStartInfo.UseShellExecute = true;
            processStartInfo.FileName = "/usr/bin/xdg-open";
            
            Process.Start(processStartInfo);
        }
    }
}