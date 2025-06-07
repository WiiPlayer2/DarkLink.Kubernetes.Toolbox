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

public record ResourceMetadata(ResourceName Name, ResourceNamespace Namespace);