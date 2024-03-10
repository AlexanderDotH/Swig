using System.Globalization;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Extensions.Logging;
using Swig.Console.FileSystem;
using Swig.Console.Helper;
using Swig.Shared.Classes;
using Swig.Shared.Enums;
using Swig.Shared.Serializable;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Profile = Swig.Shared.Classes.Profile;

namespace Swig.Console.Configuration;

public class ProfileRegistry
{
    private ProfileRegistryObject Registry { get; set; }
    private string ProfileConfigName { get; } = "profiles.yaml";
    
    private FileSystemManager FileSystemManager { get; set; }
    
    private ISerializer Serializer { get; set; }
    private IDeserializer Deserializer { get; set; }

    private readonly ILogger _logger = 
        new SpectreConsoleLogger("Profile Registry", Swig.Instance.LoggerConfiguration);
    
    public ProfileRegistry(FileSystemManager fileSystemManager)
    {
        this.FileSystemManager = fileSystemManager;

        Serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        Deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        this.Registry = 
            GetProfileEntries();
        
        this.Registry.Entries?.ForEach(e => 
            _logger.LogDebug($"Loaded profile id: {e.Identifier}"));
        
        _logger.LogDebug("Loaded ctor");
    }
    
    private ProfileRegistryObject GetProfileEntries()
    {
        _logger.LogDebug("Begin loading registry");
        
        if (!this.FileSystemManager.IsFilePresent(EnumFileSystemFolder.Root, this.ProfileConfigName))
        {
            _logger.LogWarning($"Registry file is not present and created an new empty one");
            _logger.LogDebug("Loaded registry");
            return this.UpdateRegistryOnDisk(new ProfileRegistryObject());
        }
        
        try
        {
            string fileContent = this.FileSystemManager.ReadFile(EnumFileSystemFolder.Root, this.ProfileConfigName);
            MigrateFromOtherVersion(fileContent);

            _logger.LogDebug("Loaded registry");
            return this.Deserializer.Deserialize<ProfileRegistryObject>(fileContent);
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to parse registry file");
            AnsiConsole.WriteException(e);

            _logger.LogDebug("Loaded registry, it will use a new registry object for now");
            return new ProfileRegistryObject();
        }
    }

    private ProfileRegistryObject MigrateFromOtherVersion(string content)
    {
        double version = -1.0d;

        YamlHelper yamlHelper = new YamlHelper(content);
        
        if (yamlHelper.HasField("version"))
        {
            
            _logger.LogDebug("Found version tag");
        }

        return null;
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
        get => Registry.Entries;
    }

    public Guid Selected
    {
        get => this.Registry.Selected;
    }
}