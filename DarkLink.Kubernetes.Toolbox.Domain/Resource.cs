using System;

namespace DarkLink.Kubernetes.Toolbox.Domain;

public record Resource(YamlNode Yaml)
{
    public ResourceMetadata Metadata { get; init; }
}