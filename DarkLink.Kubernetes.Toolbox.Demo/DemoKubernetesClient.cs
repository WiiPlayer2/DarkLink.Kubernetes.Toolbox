using DarkLink.Kubernetes.Toolbox.Application;
using DarkLink.Kubernetes.Toolbox.Domain;
using LanguageExt;
using LanguageExt.Effects.Traits;

namespace DarkLink.Kubernetes.Toolbox.Demo;

public class DemoKubernetesClient<RT> : IKubernetesClient<RT> where RT : struct, HasCancel<RT>
{
    public Aff<RT, Seq<Pod>> GetPods() => SuccessAff(Seq1(
        new Pod(new(ResourceName.From("pod1"), ResourceNamespace.From("ns1")))
        {
            Volumes = [
                PodVolume.PersistentVolumeClaim(ResourceName.From("pvc1")), 
            ],
        }));

    public Aff<RT, Seq<PersistentVolumeClaim>> GetPersistentVolumeClaims() => SuccessAff(Seq1(
        new PersistentVolumeClaim(new ResourceMetadata(ResourceName.From("pvc1"), ResourceNamespace.From("ns1")), StorageClassName.From("nfs"))));
}
