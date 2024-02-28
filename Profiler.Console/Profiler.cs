using Profiler.Console.Configuration;
using Profiler.Console.FileSystem;
using Profiler.Console.Style.Layouts;

namespace Profiler.Console;

public class Profiler
{
    private FileSystemManager FileSystemManager { get; set; }
    private ProfileRegistry ProfileRegistry { get; set; }

    private string[] Args { get; set; }
    
    public Profiler()
    {
        FileSystemManager = new FileSystemManager();
        ProfileRegistry = new ProfileRegistry(this.FileSystemManager);
    }

    public void Run(params string[] args)
    {
        this.Args = args;
        new ListLayout().DrawLayout();
    }
}