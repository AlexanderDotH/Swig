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
        var mapping = (YamlMappingNode)this._yamlStream.Documents[0].RootNode;
        return mapping.Children.Any(c => 
            ((YamlScalarNode)c.Key).Value.Equals(fieldName));
    }
    
    public YamlNode GetContent(string fieldName)
    {
        var mapping = (YamlMappingNode)this._yamlStream.Documents[0].RootNode;
        
        return mapping.Children.First(c => 
            ((YamlScalarNode)c.Key).Value.Equals(fieldName)).Value;
    }

    public double GetDouble(string fieldName)
    {
        YamlNode versionNode = GetContent("version");

        if (versionNode.NodeType == YamlNodeType.Scalar)
        {
            YamlScalarNode scalarNode = (YamlScalarNode)versionNode;

            double result;
            double.TryParse(scalarNode.Value, out result);
            return result;
        }
        
        throw new E
    }
}