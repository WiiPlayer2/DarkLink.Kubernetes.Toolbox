namespace DarkLink.Kubernetes.Toolbox.Domain;

public static class DependencyTreeBuilder
{
    public static DependencyTree Build(Seq<Pod> pods, Seq<PersistentVolumeClaim> pvcs)
    {
        return new();
    }
}
