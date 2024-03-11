using Swig.Shared.Serializable;

namespace Swig.Shared.Classes;

public class ProfileRegistryObject
{
    public Guid Selected { get; set; }
    public List<ProfileEntry> Entries { get; set; }
    
    #pragma warning disable S1104
    public byte Version = 1;
    #pragma warning restore S1104
    
    public ProfileRegistryObject()
    {
        Selected = Guid.Empty;
        Entries = new List<ProfileEntry>();
    }
}