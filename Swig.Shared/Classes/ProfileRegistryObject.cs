using Swig.Shared.Serializable;

namespace Swig.Shared.Classes;

public class ProfileRegistryObject
{
    public ProfileEntry Selected { get; set; } = new ProfileEntry();
    public List<ProfileEntry> ProfileEntries { get; set; } = new List<ProfileEntry>();
    public double Version { get; set; } = 1.1;
}