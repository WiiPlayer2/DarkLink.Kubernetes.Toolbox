namespace DarkLink.Kubernetes.Toolbox.Domain;

public static class DependencyTreeBuilder
{
    public static DependencyTree Build(Seq<Pod> pods, Seq<PersistentVolumeClaim> pvcs, Option<Seq<StorageClassName>> relevantStorageClassses = default)
    {
        return new(
            pods.ToDictionary(
                    x => x,
                    x => new PodDependency(pvcs
                        .Where(y => relevantStorageClassses.Match(classes => classes.Contains(y.StorageClassName), () => true))
                        .Where(y => x.Volumes
                            .OfType<PodVolume.PersistentVolumeClaim_>()
                            .Any(z => z.Name == y.Metadata.Name && x.Metadata.Namespace == y.Metadata.Namespace))
                        .Select(NfsDependency.Pvc)
                        .ToSeq()))
                .Where(x => x.Value.Dependencies.Any())
                .ToMap());
    }
}
