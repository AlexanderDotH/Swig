namespace Profiler.Shared.Classes;

public class Profile
{
    public string Name { private set; get; }
    public Guid Identifier { private set; get; }
    public FileInfo GitConfigInfo { private set; get; }
}