using Vogen;

namespace DarkLink.Kubernetes.Toolbox.Domain;

public record PersistentVolumeClaim(ResourceMetadata Metadata, StorageClassName StorageClassName) : Resource(Metadata);

[ValueObject<string>]
public partial record StorageClassName;

[ValueObject<string>]
public partial record ResourceName;

[ValueObject<string>]
public partial record ResourceNamespace;

public record Resource(ResourceMetadata Metadata);

public record ResourceMetadata(ResourceName Name, ResourceNamespace Namespace) : IComparable<ResourceMetadata>
{
    public int CompareTo(ResourceMetadata? other)
    {
        if (other is null) return 1;
        
        var namespaceComparison = Namespace.CompareTo(other.Namespace);
        if (namespaceComparison != 0) return namespaceComparison;

        return Name.CompareTo(other.Name);
    }
}