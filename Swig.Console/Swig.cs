using Microsoft.Extensions.Logging;
using Spectre.Console.Extensions.Logging;
using Swig.Console.Configuration;
using Swig.Console.FileSystem;
using Swig.Console.Style.Layouts;

namespace Swig.Console;

public class Swig
{
    private FileSystemManager FileSystemManager { get; set; }
    private ProfileRegistry ProfileRegistry { get; set; }
    public ProfileManager ProfileManager { get; private set; }

    private static Swig _swigInstance;
    
    public SpectreConsoleLoggerConfiguration LoggerConfiguration { get; private set; }
    
    private string[] Args { get; set; }
    
    public Swig(params string[] args)
    {
        #pragma warning disable S3010
        _swigInstance = this;
        #pragma warning restore S3010
        
        this.Args = args;
        
        SpectreConsoleLoggerConfiguration loggerConfiguration = new SpectreConsoleLoggerConfiguration();
        loggerConfiguration.IncludeEventId = true;
        loggerConfiguration.IncludePrefix = true;
        
        loggerConfiguration.LogLevel = args.Contains("--debug") ? LogLevel.Debug : LogLevel.None;
        LoggerConfiguration = loggerConfiguration;
        
        FileSystemManager = new FileSystemManager();
        ProfileRegistry = new ProfileRegistry(this.FileSystemManager);
        ProfileManager = new ProfileManager(this.ProfileRegistry, this.FileSystemManager);
    }

    public void Run()
    {
        new MainLayout().DrawLayout();
    }

    public static Swig Instance
    {
        get
        {
            return _swigInstance;
        }
    }
}