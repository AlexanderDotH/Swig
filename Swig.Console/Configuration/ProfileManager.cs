using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Extensions.Logging;
using Swig.Console.FileSystem;
using Swig.Shared.Enums;
using Swig.Shared.Exceptions;
using Swig.Shared.Serializable;
using Swig.Shared.Utils;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Configuration;

public class ProfileManager
{
    private readonly List<Profile> _profiles;
    public Profile Current { get; private set; }
    
    private ProfileRegistry ProfileRegistry { get; set; }
    private FileSystemManager FileSystemManager { get; set; }

    private ISerializer Serializer { get; set; }
    private IDeserializer Deserializer { get; set; }

    private readonly ILogger _logger = 
        new SpectreInlineLogger("Profile Manager", Swig.Instance.LoggerConfiguration);

    private readonly string _gitConfigName = ".gitconfig";

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
        this.CreateBackupCopy();
        
        _logger.LogDebug("Loaded ctor");
    }

    private void CreateBackupCopy()
    {
        bool backupFilePresent = this.FileSystemManager.IsFilePresent(EnumFileSystemFolder.Root, this._gitConfigName);
        
        if (backupFilePresent)
            return;

        FileInfo gitConfig = new FileInfo(GitUtils.GetGlobalGitConfigPath());
        
        if (!gitConfig.Exists)
            return;

        this.FileSystemManager.CopyTo(EnumFileSystemFolder.Root, gitConfig, "/", this._gitConfigName);
    }

    public bool HasChanged(string gitConfigPath, Profile profile)
    {
        try
        {
            string profileContent = this.FileSystemManager.ReadFile(EnumFileSystemFolder.Profiles, profile.Identifier.ToString(),this._gitConfigName);
            string gitConfigContent = FileUtils.ReadContent(gitConfigPath);

            return !profileContent.SequenceEqual(gitConfigContent);
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public void SyncGitConfig(string gitConfigPath, Profile profile)
    {
        FileInfo gitConfigInfo = new FileInfo(gitConfigPath);

        if (!gitConfigInfo.Exists)
            throw new FileNotFoundException("Cannot find gitconfig");

        FileInfo profileInfo = new FileInfo(profile.GitConfigFile);
        
        if (!profileInfo.Exists)
            throw new FileNotFoundException("Cannot find gitconfig inside the profile folder");

        gitConfigInfo.CopyTo(profileInfo.FullName, true);
    }
    
    public bool RestoreBackup()
    {
        bool backupFilePresent = this.FileSystemManager.IsFilePresent(EnumFileSystemFolder.Root, this._gitConfigName);
        
        if (!backupFilePresent)
            return false;

        string originContent = this.FileSystemManager.ReadFile(EnumFileSystemFolder.Root, this._gitConfigName);
        string originPath = GitUtils.GetOriginGitConfigPath();

        File.WriteAllText(originPath, originContent);

        return true;
    }
    
    private void ReadAndSetupProfiles()
    {
        _logger.LogDebug("Begin loading profiles");
        foreach (ProfileEntry profileEntry in this.ProfileRegistry.ProfileEntries)
        {
            Profile profile;

            if (!TryGetProfile(profileEntry.Identifier, out profile))
            {
                _logger.LogWarning($"Skipped profile {profileEntry.Identifier} could not parse data");
                continue;
            }

            _logger.LogDebug($"Added profile {profile.Name} with id: {profile.Identifier}");
            this._profiles.Add(profile);
        }
        
        this.Current = GetProfileById(this.ProfileRegistry.Selected);
        _logger.LogDebug($"Set current profile {this.ProfileRegistry.Selected}");
    }

    public bool DoesProfileExist(string profileName)
    {
        return this._profiles.Exists(p => p.Name.Equals(profileName));
    }

    public Profile LoadProfile(string profileName)
    {
        if (!DoesProfileExist(profileName))
        {
            _logger.LogWarning($"Profile: {profileName} does not exists");
            throw new ProfileException("Cannot find profile");
        }
        
        Profile profile = Swig.Instance.ProfileManager.GetProfileByName(profileName);
        GitUtils.SetGlobalGitConfigPath(new FileInfo(profile.GitConfigFile));

        Current = profile;
        this.ProfileRegistry.SetSelectedProfile(profile.Identifier);
        
        return profile;
    }
    
    public Profile GetProfileByName(string profileName)
    {
        return this._profiles.First(p => p.Name.Equals(profileName));
    }
    
    private Profile GetProfileById(Guid identifier)
    {
        if (identifier == Guid.Empty)
        {
            _logger.LogWarning($"Ignored empty profile id");
            return null;
        }

        try
        {
            return this._profiles.First(p => p.Identifier.Equals(identifier));
        }
        catch (Exception e)
        {
            return null;
        }
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
        
        _logger.LogDebug($"Deleted profile {profile.Name} with id: {profile.Identifier}");
    }

    public void RenameProfile(Profile profile, string newName)
    {
        Profile p = this._profiles.First(pl => pl.Identifier.Equals(profile.Identifier));
        p.Name = newName;
        
        this.WriteProfile(profile, false);
    }
    
    public void CreateNewProfile(string profileName, string gitConfigPath)
    {
        Profile profile = new Profile()
        {
            Name = profileName,
            Identifier = Guid.NewGuid()
        };

        DirectoryInfo workSpace = this.PrepareWorkspace(profile);

        FileInfo newGitConfigPath = null;
        
        if (gitConfigPath != null)
        {
            newGitConfigPath = this.FileSystemManager.CopyTo(EnumFileSystemFolder.Profiles, new FileInfo(gitConfigPath),
                workSpace.Name, this._gitConfigName);
        }
        else
        {
            newGitConfigPath = this.FileSystemManager.CreateAndWriteFile(EnumFileSystemFolder.Profiles, workSpace.Name, this._gitConfigName, string.Empty);
        }

        profile.GitConfigFile = newGitConfigPath.FullName;

        this.WriteProfile(profile, true);
        _logger.LogDebug($"Created profile {profileName}");
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

    public DirectoryInfo PrepareWorkspace(Profile profile)
    {
        return this.FileSystemManager.CreateDirectory(EnumFileSystemFolder.Profiles, profile.Identifier.ToString());
    }

    private Profile ReadProfile(Guid identifier)
    {
        if (!this.FileSystemManager.IsFilePresent(EnumFileSystemFolder.Profiles, identifier.ToString(),
                $"{identifier}.yaml"))
            throw new ProfileException("Profile does not exist on disk");
        
        string profileContent = this.FileSystemManager.ReadFile(
                EnumFileSystemFolder.Profiles, 
                identifier.ToString(),
                $"{identifier}.yaml");
        
        _logger.LogDebug($"Read profile {identifier}");
        
        return Deserializer.Deserialize<Profile>(profileContent);
    }

    private void WriteProfile(Profile profile, bool registerProfile = true)
    {
        _logger.LogDebug($"Wrote to profile {profile.Identifier}");
        
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