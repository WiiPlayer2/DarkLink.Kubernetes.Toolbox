using FunicularSwitch.Generators;

namespace DarkLink.Kubernetes.Toolbox.Domain;

[UnionType(CaseOrder = CaseOrder.AsDeclared)]
public abstract partial record NfsDependency
{
    public record Pvc_(PersistentVolumeClaim PersistentVolumeClaim) : NfsDependency;

    public record Mount_ : NfsDependency;
}

public record PodDependency(Seq<NfsDependency> Dependencies);

public record DependencyTree;
