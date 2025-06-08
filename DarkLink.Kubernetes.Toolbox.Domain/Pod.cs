using FunicularSwitch.Generators;
using Vogen;

namespace DarkLink.Kubernetes.Toolbox.Domain;

[UnionType(CaseOrder = CaseOrder.AsDeclared)]
public abstract partial record PodVolume
{
    public record PersistentVolumeClaim_(ResourceName Name) : PodVolume;

    public record Nfs_(Hostname Server) : PodVolume;
}

[ValueObject<string>]
public partial record Hostname;

public record Pod(ResourceMetadata Metadata) : Resource(Metadata), IComparable<Pod>
{
    public IReadOnlyCollection<PodVolume> Volumes { get; init; } = [];

    public int CompareTo(Pod? other) => Metadata.CompareTo(other?.Metadata);
}
