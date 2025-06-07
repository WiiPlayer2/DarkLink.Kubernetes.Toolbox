namespace DarkLink.Kubernetes.Toolbox.Domain;

public record Pod(ResourceMetadata Metadata) : Resource(Metadata);
