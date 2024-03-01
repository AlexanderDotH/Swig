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
    private List<ProfileEntry> _profileEntries;
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
        this._profileEntries = new List<ProfileEntry>();
        
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
    
    public void CreateNewProfile(string profileName, FileInfo gitConfigPath)
    {
        Profile profile = new Profile()
        {
            Name = profileName,
            Identifier = Guid.NewGuid()
        };

        DirectoryInfo workSpace = this.PrepareWorkspace(profile);

        FileInfo newGitConfigPath = this.FileSystemManager.CopyTo(EnumFileSystemFolder.Profiles, gitConfigPath,
            workSpace.Name, ".gitconfig");

        profile.GitConfigFile = newGitConfigPath.FullName;

        WriteProfile(profile, true);
    }

    public void WriteProfile(Profile profile, bool registerProfile = true)
    {
        string profileContent = this.Serializer.Serialize(profile);

        DirectoryInfo workSpace = this.PrepareWorkspace(profile);
        
        this.FileSystemManager.CreateAndWriteFile(
            EnumFileSystemFolder.Profiles, 
            workSpace.Name, 
            $"{profile.Identifier}.yaml",
            profileContent);

        if (registerProfile)
            AddAndWriteProfile(profile);
    }
    
    public DirectoryInfo PrepareWorkspace(Profile profile)
    {
        return this.FileSystemManager.CreateDirectory(EnumFileSystemFolder.Profiles, profile.Identifier.ToString());
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
    
    private void AddAndWriteProfile(Profile profile)
    {
        this._profiles.Add(profile);

        ProfileEntry profileEntry = new ProfileEntry()
        {
            Identifier = profile.Identifier
        };
        
        this._profileEntries.Add(profileEntry);
        
        UpdateRegistryOnDisk(this._profileEntries);
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
}