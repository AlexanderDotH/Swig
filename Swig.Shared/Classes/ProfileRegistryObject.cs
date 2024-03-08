using Swig.Shared.Serializable;

namespace Swig.Shared.Classes;

public class ProfileRegistryObject
{
    public Guid Selected { get; set; } = Guid.Empty;
    public List<ProfileEntry> ProfileEntries { get; set; } = new List<ProfileEntry>();
    public double Version { get; set; } = 1.1;
}