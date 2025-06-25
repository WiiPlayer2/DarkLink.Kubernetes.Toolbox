using DarkLink.Kubernetes.Toolbox.Application;
using DarkLink.Kubernetes.Toolbox.Domain;
using k8s;
using LanguageExt;
using LanguageExt.Effects.Traits;

namespace DarkLink.Kubernetes.Toolbox.KubernetesClient;

public class KubernetesClient<RT>(k8s.Kubernetes k8s) : IKubernetesClient<RT> where RT : struct, HasCancel<RT>
{
    public Aff<RT, Seq<Pod>> GetPods() =>
        from pods in Aff((RT rt) => k8s.ListPodForAllNamespacesAsync(cancellationToken: rt.CancellationToken).ToValue())
        let mappedPods = pods.Items.Select(x => new Pod(new ResourceMetadata(ResourceName.From(x.Metadata.Name), ResourceNamespace.From(x.Metadata.NamespaceProperty)))
            {
                Volumes = (x.Spec.Volumes ?? [])
                    .Select(x => x.PersistentVolumeClaim is not null
                        ? PodVolume.PersistentVolumeClaim(ResourceName.From(x.PersistentVolumeClaim.ClaimName))
                        : x.Nfs is not null
                            ? PodVolume.Nfs(Hostname.From(x.Nfs.Server))
                            : null)
                    .Where(x => x is not null)
                    .ToArr(),
                Status = new PodStatus(PodStatusPhase.From(x.Status.Phase)),
            })
            .ToSeq()
        select mappedPods;

    public Aff<RT, Seq<PersistentVolumeClaim>> GetPersistentVolumeClaims() =>
        from pvcs in Aff((RT rt) => k8s.ListPersistentVolumeClaimForAllNamespacesAsync(cancellationToken: rt.CancellationToken).ToValue())
        let mappedPvcs = pvcs.Items
            .Select(x => new PersistentVolumeClaim(new(ResourceName.From(x.Metadata.Name), ResourceNamespace.From(x.Metadata.NamespaceProperty)), StorageClassName.From(x.Spec.StorageClassName)))
            .ToSeq()
        select mappedPvcs;
}
