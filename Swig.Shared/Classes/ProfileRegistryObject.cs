using Swig.Shared.Serializable;

namespace Swig.Shared.Classes;

public class ProfileRegistryObject
{
    public Guid Selected { get; set; }
    public List<ProfileEntry> Entries { get; set; }
    
    public bool RequiresSetup { get; set; }
    public bool AreEmojisAllowed { get; set; }
    public byte Version { get; set; } = 2;
    
    public ProfileRegistryObject()
    {
        Selected = Guid.Empty;
        Entries = new List<ProfileEntry>();
        RequiresSetup = true;
        AreEmojisAllowed = false;
    }
}