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
    private List<Profile> _profiles;
    private string ProfileConfigName { get; } = "profiles.yaml";
    
    private FileSystemManager FileSystemManager { get; set; }
    
    private ISerializer Serializer { get; set; }
    private IDeserializer Deserializer { get; set; }

    private ILogger _logger = 
        LoggerFactory.Create(b => b.AddSpectreConsole())
            .CreateLogger("Profile Registry");
    
    public ProfileRegistry(FileSystemManager fileSystemManager)
    {
        this.FileSystemManager = fileSystemManager;

        this._profiles = new List<Profile>();
        
        Serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        Deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        
        this.ReadAndSetupProfiles();
    }

    private void ReadAndSetupProfiles()
    {
        List<ProfileEntry> profileEntries = GetProfileEntries();
        
        foreach (ProfileEntry profileEntry in profileEntries)
        {
            Profile profile;

            if (TryGetProfile(profileEntry.Identifier, out profile))
            {
                this._logger.LogDebug($"Loaded profile {profile.Identifier}");
            }
            else
            {
                this._logger.LogDebug($"Could not load profile {profile.Identifier}");
                continue;
            }
            
            this._profiles.Add(profile);
        }
    }

    private bool TryGetProfile(Guid profileId, out Profile profile)
    {
        try
        {
            profile = ReadProfile(profileId);
            return true;
        }
        catch (Exception e)
        {
            profile = null;
            return false;
        }
    }

    private Profile ReadProfile(Guid identifier)
    {
        string profileContent =
            this.FileSystemManager.ReadFile(EnumFileSystemFolder.Profiles, $"{identifier}/{identifier}.yaml");
        return Deserializer.Deserialize<Profile>(profileContent);
    }

    private void WriteProfile(Profile profile)
    {
        string profileContent = this.Serializer.Serialize(profile);
        this.FileSystemManager.CreateAndWriteFile(EnumFileSystemFolder.Profiles, 
            $"{profile.Identifier}/{profile.Identifier}.yaml",
            profileContent);
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
            return CreateDefaultEmptyProfileEntries();
        }
    }
    
    // find default git ignore
    private List<ProfileEntry> CreateDefaultEmptyProfileEntries()
    {
        List<ProfileEntry> profileEntries = new List<ProfileEntry>();

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
}