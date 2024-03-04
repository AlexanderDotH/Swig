using Microsoft.Extensions.Logging;
using Profiler.Console.FileSystem;
using Profiler.Shared.Classes;
using Profiler.Shared.Enums;
using Profiler.Shared.Serializable;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Profiler.Console.Configuration;

public class ProfileRegistry
{
    private List<ProfileEntry> _profileEntries;
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

        this._profileEntries = GetProfileEntries();
    }
    
    private List<ProfileEntry> GetProfileEntries()
    {
        if (!this.FileSystemManager.IsFilePresent(EnumFileSystemFolder.Root, this.ProfileConfigName))
            return new List<ProfileEntry>();
        
        try
        {
            string fileContent = this.FileSystemManager.ReadFile(EnumFileSystemFolder.Root, this.ProfileConfigName);
            return this.Deserializer.Deserialize<List<ProfileEntry>>(fileContent);
        }
        catch (Exception e)
        {
            return UpdateRegistryOnDisk(new List<ProfileEntry>());
        }
    }
    
    public void AddAndWriteProfile(Profile profile)
    {
        ProfileEntry profileEntry = new ProfileEntry()
        {
            Identifier = profile.Identifier
        };
        
        this._profileEntries.Add(profileEntry);
        
        this._profileEntries = UpdateRegistryOnDisk(this._profileEntries);
    }
    
    public void RemoveAndWriteProfile(Profile profile)
    {
        this._profileEntries.RemoveAll(p => p.Identifier.Equals(profile.Identifier));
        this._profileEntries = UpdateRegistryOnDisk(this._profileEntries);
    }
    
    private List<ProfileEntry> UpdateRegistryOnDisk(List<ProfileEntry> profileEntries)
    {
        try
        {
            string serialized = this.Serializer.Serialize(profileEntries);
            this.FileSystemManager.CreateAndWriteFile(EnumFileSystemFolder.Root, ProfileConfigName, serialized);
            return profileEntries;
        }
        catch (Exception e)
        {
            // TODO: Output (Cannot create default profile)
            return profileEntries;
        }
    }

    public List<ProfileEntry> ProfileEntries
    {
        get => this._profileEntries;
    }
}