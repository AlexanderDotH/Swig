using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Extensions.Logging;
using Swig.Console.FileSystem;
using Swig.Console.Helper;
using Swig.Shared.Classes;
using Swig.Shared.Enums;
using Swig.Shared.Serializable;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Configuration.Registry;

public class ProfileRegistry
{
    private ProfileRegistryObject Registry { get; set; }
    private string ProfileConfigName { get; } = "profiles.yaml";
    
    private FileSystemManager FileSystemManager { get; set; }
    private RegistryMigration RegistryMigration { get; set; }
    
    private ISerializer Serializer { get; set; }

    private readonly ILogger _logger = 
        new SpectreInlineLogger("Profile Registry", Swig.Instance.LoggerConfiguration);
    
    public ProfileRegistry(FileSystemManager fileSystemManager)
    {
        this.FileSystemManager = fileSystemManager;
        this.RegistryMigration = new RegistryMigration(this);
        
        Serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        this.Registry = 
            GetProfileEntries();
        
        this.Registry.Entries?.ForEach(e => 
            _logger.LogDebug("Found profile id: {identifier}", e.Identifier));
        
        _logger.LogDebug("Loaded ctor");
    }
    
    private ProfileRegistryObject GetProfileEntries()
    {
        _logger.LogDebug("Begin loading registry");
        
        if (!this.FileSystemManager.IsFilePresent(EnumFileSystemFolder.Root, this.ProfileConfigName))
        {
            _logger.LogWarning("Registry file is not present and created an new empty one");
            _logger.LogDebug("Loaded registry");
            return this.UpdateRegistryOnDisk(new ProfileRegistryObject());
        }
        
        try
        {
            string fileContent = this.FileSystemManager.ReadFile(EnumFileSystemFolder.Root, this.ProfileConfigName);
            return this.RegistryMigration.MigrateFromOtherVersion(fileContent);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to parse registry file");
            AnsiConsole.WriteException(e);

            _logger.LogDebug("Loaded registry, it will use a new registry object for now");
            return new ProfileRegistryObject();
        }
    }
    
    public void SetSelectedProfile(Guid identifier)
    {
        this.Registry.Selected = identifier;
        this.Registry = UpdateRegistryOnDisk(this.Registry);
    }
    
    public void AddAndWriteProfile(Profile profile)
    {
        ProfileEntry profileEntry = new ProfileEntry()
        {
            Identifier = profile.Identifier
        };
        
        this.Registry.Entries.Add(profileEntry);
        this.Registry = UpdateRegistryOnDisk(this.Registry);
    }
    
    public void RemoveAndWriteProfile(Profile profile)
    {
        this.Registry.Entries.RemoveAll(p => p.Identifier.Equals(profile.Identifier));
        this.Registry = UpdateRegistryOnDisk(this.Registry);
    }
    
    public ProfileRegistryObject UpdateRegistryOnDisk(ProfileRegistryObject profileRegistry)
    {
        try
        {
            string serialized = this.Serializer.Serialize(profileRegistry);
            this.FileSystemManager.CreateAndWriteFile(EnumFileSystemFolder.Root, ProfileConfigName, serialized);
            return profileRegistry;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot create default config");
            return profileRegistry;
        }
    }

    public List<ProfileEntry> ProfileEntries
    {
        get => Registry.Entries;
    }

    public Guid Selected
    {
        get => this.Registry.Selected;
    }

    public bool AreEmojisAllowed
    {
        get => this.Registry.AreEmojisAllowed;
        set
        {
            this.Registry.AreEmojisAllowed = value;
            this.Registry = UpdateRegistryOnDisk(this.Registry);
        }
    }
    
    public bool RequiresSetup
    {
        get => this.Registry.RequiresSetup;
        set
        {
            this.Registry.RequiresSetup = value;
            this.Registry = UpdateRegistryOnDisk(this.Registry);
        }
    }
}