using Profiler.Console.Configuration;
using Profiler.Console.FileSystem;
using Profiler.Console.Style.Layouts;

namespace Profiler.Console;

public class Profiler
{
    public FileSystemManager FileSystemManager { get; private set; }
    public ProfileRegistry ProfileRegistry { get; private set; }
    public ProfileManager ProfileManager { get; private set; }

    private static Profiler _profilerInstance;
    
    private string[] Args { get; set; }
    
    public Profiler()
    {
        FileSystemManager = new FileSystemManager();
        ProfileRegistry = new ProfileRegistry(this.FileSystemManager);
        ProfileManager = new ProfileManager(this.ProfileRegistry, this.FileSystemManager);
    }

    public void Run(params string[] args)
    {
        this.Args = args;
        
        _profilerInstance = this;
        
        new MainLayout().DrawLayout();
    }

    public static Profiler Instance
    {
        get
        {
            return _profilerInstance;
        }
    }
}