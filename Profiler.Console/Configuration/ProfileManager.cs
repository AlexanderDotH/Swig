using System.Reflection;
using Profiler.Console.Exceptions;
using Profiler.Console.FileSystem;
using Profiler.Shared.Classes;
using Profiler.Shared.Enums;
using Profiler.Shared.Serializable;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Profiler.Console.Configuration;

public class ProfileManager
{
    private List<Profile> _profiles;

    private ProfileRegistry ProfileRegistry { get; set; }
    private FileSystemManager FileSystemManager { get; set; }

    private ISerializer Serializer { get; set; }
    private IDeserializer Deserializer { get; set; }

    public ProfileManager(ProfileRegistry profileRegistry, FileSystemManager fileSystemManager)
    {
        this._profiles = new List<Profile>();

        this.ProfileRegistry = profileRegistry;
        this.FileSystemManager = fileSystemManager;

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
        foreach (ProfileEntry profileEntry in this.ProfileRegistry.ProfileEntries)
        {
            Profile profile;

            if (!TryGetProfile(profileEntry.Identifier, out profile))
                continue;

            this._profiles.Add(profile);
        }
    }

    public bool DoesProfileExist(string profileName)
    {
        return this._profiles.Any(p => p.Name.Equals(profileName));
    }

    public Profile GetProfileByName(string profileName)
    {
        return this._profiles.First(p => p.Name.Equals(profileName));
    }

    public void DeleteProfile(Profile profile)
    {
        FileInfo gitConfigFileInfo = new FileInfo(profile.GitConfigFile);
        
        if (gitConfigFileInfo.Exists)
            gitConfigFileInfo.Delete();
        
        if (!this.FileSystemManager.DeleteFile(
                EnumFileSystemFolder.Profiles,
                profile.Identifier.ToString(),
                $"{profile.Identifier}.yaml"))
            throw new ProfileException("Cannot delete profile xaml");
        
        if (!this.FileSystemManager.DeleteDirectory(
                EnumFileSystemFolder.Profiles,
                profile.Identifier.ToString()))
            throw new ProfileException("Cannot delete profile directory");

        this._profiles.Remove(profile);
        
        this.ProfileRegistry.RemoveAndWriteProfile(profile);
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

        this.WriteProfile(profile, true);
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

    private DirectoryInfo PrepareWorkspace(Profile profile)
    {
        return this.FileSystemManager.CreateDirectory(EnumFileSystemFolder.Profiles, profile.Identifier.ToString());
    }

    private Profile ReadProfile(Guid identifier)
    {
        string profileContent =
            this.FileSystemManager.ReadFile(EnumFileSystemFolder.Profiles, $"{identifier}/{identifier}.yaml");
        return Deserializer.Deserialize<Profile>(profileContent);
    }

    private void WriteProfile(Profile profile, bool registerProfile = true)
    {
        string profileContent = this.Serializer.Serialize(profile);

        DirectoryInfo workSpace = this.PrepareWorkspace(profile);

        this.FileSystemManager.CreateAndWriteFile(
            EnumFileSystemFolder.Profiles,
            workSpace.Name,
            $"{profile.Identifier}.yaml",
            profileContent);

        if (registerProfile)
        {
            this._profiles.Add(profile);
            this.ProfileRegistry.AddAndWriteProfile(profile);
        }
    }

    public List<Profile> Profiles
    {
        get => this._profiles;
    }
}