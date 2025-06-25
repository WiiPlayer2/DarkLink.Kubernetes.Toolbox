using System;

namespace DarkLink.Kubernetes.Toolbox.Domain;

public record ResourceMetadata(YamlNode Yaml) : IComparable<ResourceMetadata>
{
    private static readonly YamlPath PATH_NAME = YamlPath.MapItem(YamlMapKey.From("name"), YamlPath.This());

    private static readonly YamlPath PATH_NAMESPACE = YamlPath.MapItem(YamlMapKey.From("namespace"), YamlPath.This());
    
    public ResourceMetadata(ResourceName Name, ResourceNamespace Namespace) : this(YamlNode.Map(Map<YamlMapKey, YamlNode>()))
    {
        this.Name = Name;
        this.Namespace = Namespace;
    }

    public int CompareTo(ResourceMetadata? other)
    {
        if (other is null) return 1;
        
        var namespaceComparison = Namespace.CompareTo(other.Namespace);
        if (namespaceComparison != 0) return namespaceComparison;

        return Name.CompareTo(other.Name);
    }

    public ResourceName Name
    {
        get => Yaml.Get<YamlNode.YamlScalar.YamlNodeString>(PATH_NAME)
            .Map(x => ResourceName.From(x.Value))
            .GetOrThrow();
        init => Yaml = Yaml.Set(PATH_NAME, YamlNode.String(value.Value))
            .GetOrThrow();
    }

    public ResourceNamespace Namespace
    {
        get => Yaml.Get<YamlNode.YamlScalar.YamlNodeString>(PATH_NAMESPACE)
            .Map(x => ResourceNamespace.From(x.Value))
            .GetOrThrow();
        init => Yaml = Yaml.Set(PATH_NAMESPACE, YamlNode.String(value.Value))
            .GetOrThrow();
    }
}