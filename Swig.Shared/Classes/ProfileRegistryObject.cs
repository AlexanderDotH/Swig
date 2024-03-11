using Swig.Shared.Serializable;

namespace Swig.Shared.Classes;

public class ProfileRegistryObject
{
    public Guid Selected { get; set; }
    public List<ProfileEntry> Entries { get; set; }
    public byte Version = 1;

    public ProfileRegistryObject()
    {
        Selected = Guid.Empty;
        Entries = new List<ProfileEntry>();
    }
}