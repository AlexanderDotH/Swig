using Swig.Console.FileSystem;
using Swig.Shared.Classes;
using Swig.Shared.Enums;
using Swig.Shared.Serializable;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Swig.Console.Configuration;

public class ProfileRegistry
{
    private ProfileRegistryObject Registry { get; set; }
    private string ProfileConfigName { get; } = "profiles.yaml";
    
    private FileSystemManager FileSystemManager { get; set; }
    
    private ISerializer Serializer { get; set; }
    private IDeserializer Deserializer { get; set; }

    public ProfileRegistry(FileSystemManager fileSystemManager)
    {
        this.FileSystemManager = fileSystemManager;

        Serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        Deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        this.Registry = GetProfileEntries();
    }
    
    private ProfileRegistryObject GetProfileEntries()
    {
        if (!this.FileSystemManager.IsFilePresent(EnumFileSystemFolder.Root, this.ProfileConfigName))
            return this.UpdateRegistryOnDisk(new ProfileRegistryObject());
        
        try
        {
            string fileContent = this.FileSystemManager.ReadFile(EnumFileSystemFolder.Root, this.ProfileConfigName);
            return this.Deserializer.Deserialize<ProfileRegistryObject>(fileContent);
        }
        catch (Exception e)
        {
            return UpdateRegistryOnDisk(new ProfileRegistryObject());
        }
    }

    public void SetSelectedProfile(Guid identifier)
    {
        this.Registry.Selected.Identifier = identifier;
        this.Registry = UpdateRegistryOnDisk(this.Registry);
    }
    
    public void AddAndWriteProfile(Profile profile)
    {
        ProfileEntry profileEntry = new ProfileEntry()
        {
            Identifier = profile.Identifier
        };
        
        this.Registry.ProfileEntries.Add(profileEntry);
        this.Registry = UpdateRegistryOnDisk(this.Registry);
    }
    
    public void RemoveAndWriteProfile(Profile profile)
    {
        this.Registry.ProfileEntries.RemoveAll(p => p.Identifier.Equals(profile.Identifier));
        this.Registry = UpdateRegistryOnDisk(this.Registry);
    }
    
    private ProfileRegistryObject UpdateRegistryOnDisk(ProfileRegistryObject profileRegistry)
    {
        try
        {
            string serialized = this.Serializer.Serialize(profileRegistry);
            this.FileSystemManager.CreateAndWriteFile(EnumFileSystemFolder.Root, ProfileConfigName, serialized);
            return profileRegistry;
        }
        catch (Exception e)
        {
            // TODO: Output (Cannot create default profile)
            return profileRegistry;
        }
    }

    public List<ProfileEntry> ProfileEntries
    {
        get => Registry.ProfileEntries;
    }

    public ProfileEntry CurrentProfileEntry
    {
        get => this.Registry.Selected;
    }
}