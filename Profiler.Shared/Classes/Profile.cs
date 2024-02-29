namespace Profiler.Shared.Classes;

public class Profile
{
    public string Name { set; get; }
    public Guid Identifier { set; get; }
    public FileInfo GitConfigFile { set; get; }
}