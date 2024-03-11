using Swig.Shared.Enums;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Swig.Console.Helper;

public class YamlHelper
{
    private YamlStream _yamlStream;
    
    public YamlHelper(string yaml)
    {
        YamlStream yamlStream = new YamlStream();
        yamlStream.Load(new StringReader(yaml));

        this._yamlStream = yamlStream;
    }
    
    public bool HasField(string fieldName)
    {
        object rootNode = GetRootNode();

        if (rootNode is YamlMappingNode mappingNode)
        {
            return mappingNode.Children.Any(c =>
            {
                if (c.Key is YamlScalarNode scalarNode)
                    return scalarNode.Value.Equals(fieldName);

                return false;
            });
        }

        if (rootNode is YamlSequenceNode sequenceNode)
        {
            return sequenceNode.Children.Any(c =>
            {
                if (c is YamlScalarNode scalarNode)
                    return scalarNode.Value.Equals(fieldName);

                return false;
            });
        }

        return false;
    }
    
    public YamlNode GetContent(string fieldName)
    {
        object rootNode = GetRootNode();
        
        if (rootNode is YamlMappingNode mappingNode)
        {
            foreach (KeyValuePair<YamlNode, YamlNode> keyValue in mappingNode)
            {
                if (keyValue.Key is YamlScalarNode scalarKey)
                {
                    if (scalarKey.Value.Equals(fieldName))
                    {
                        return keyValue.Value;
                    }
                }
            }
        }
        
        if (rootNode is YamlSequenceNode sequenceNode)
        {
            return sequenceNode.Children.FirstOrDefault(c =>
            {
                if (c is YamlScalarNode scalarNode)
                    return scalarNode.Value.Equals(fieldName);

                return false;
            });
        }
        
        return null;
    }
    
    public double GetDouble(string fieldName)
    {
        YamlNode versionNode = GetContent(fieldName);

        if (versionNode.NodeType == YamlNodeType.Scalar && 
            versionNode is YamlScalarNode scalarNode)
        {
            return double.Parse(scalarNode.Value);
        }

        throw new YamlException($"Wrong data type for field \"{fieldName}\"");
    }
    
    private dynamic GetRootNode()
    {
        var rootNode = this._yamlStream.Documents[0].RootNode;

        if (rootNode is YamlMappingNode mappingNode)
            return mappingNode;

        if (rootNode is YamlSequenceNode sequenceNode)
            return sequenceNode;
        
        return this._yamlStream.Documents[0].RootNode;
    }
}