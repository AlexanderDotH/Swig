using Microsoft.Extensions.Logging;
using Spectre.Console.Extensions.Logging;
using Swig.Console.Helper;
using Swig.Shared.Classes;
using Swig.Shared.Serializable;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Swig.Console.Configuration.Registry;

public class RegistryMigration
{
    private readonly ILogger _logger = 
        new SpectreInlineLogger("RegistryMigration", Swig.Instance.LoggerConfiguration);

    private IDeserializer Deserializer { get; set; }
    
    private ProfileRegistryObject DefaultRegistry { get; set; }
    
    public RegistryMigration()
    {
        this.DefaultRegistry = new ProfileRegistryObject();

        Deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
    }
    
    public ProfileRegistryObject MigrateFromOtherVersion(string content)
    {
        YamlHelper yamlHelper = new YamlHelper(content);
        bool hasVersionTag = yamlHelper.HasField("version");

        if (hasVersionTag)
        {
            _logger.LogInformation("Found version tag");
            
            byte versionTag = yamlHelper.GetByte("version");
            _logger.LogInformation("Found registry version: {version}", versionTag);

            if (this.DefaultRegistry.Version != versionTag)
            {
                _logger.LogDebug("Detected version mismatch");
                return Migrate(versionTag, this.DefaultRegistry.Version, content);
            }
        }
        else
        {
            _logger.LogDebug("Version tag not present");

            try
            {
                _logger.LogDebug("Try migrating from legacy build");
                List<ProfileEntry> profileEntries = this.Deserializer.Deserialize<List<ProfileEntry>>(content);
                
                _logger.LogDebug("Legacy version detected");
                
                ProfileRegistryObject registryObject = new ProfileRegistryObject()
                {
                    Entries = profileEntries,
                    Selected = Guid.Empty
                };

                return registryObject;
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "Tried to parse legacy version, but failed");
            }
        }

        _logger.LogDebug("Loaded registry");
        
        return this.Deserializer.Deserialize<ProfileRegistryObject>(content);
    }
    
    private ProfileRegistryObject Migrate(int versionTag, int registryVersion, string content)
    {
        if (versionTag == 1 && this.DefaultRegistry.Version == 2)
        {
            _logger.LogDebug("Migrating from {} to {}", versionTag, registryVersion);
            
            IDeserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            try
            {
                ProfileRegistryObject deserialized = deserializer.Deserialize<ProfileRegistryObject>(content);

                deserialized.RequiresSetup = true;
                deserialized.AreEmojisAllowed = false;

                _logger.LogInformation("Migrated from {} to {}!", versionTag, registryVersion);

                return deserialized;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Cannot deserialize registry");
            }
        }

        return null;
    }
}