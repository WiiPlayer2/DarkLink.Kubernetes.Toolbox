using DarkLink.Kubernetes.Toolbox.Domain;
using LanguageExt;
using LanguageExt.Effects.Traits;

namespace DarkLink.Kubernetes.Toolbox.Application;

public class DependenciesNfsUseCase<RT>(IKubernetesClient<RT> kubernetesClient) where RT : struct, HasCancel<RT>
{
    public Aff<RT, DependencyTree> Run(Seq<StorageClassName> relevantStorageClasses) =>
        from pods in kubernetesClient.GetPods()
        from pvcs in kubernetesClient.GetPersistentVolumeClaims()
        let relevantStorageClassesOption = relevantStorageClasses.IsEmpty
            ? None
            : Some(relevantStorageClasses)
        let dependencyTree = DependencyTreeBuilder.Build(pods, pvcs, relevantStorageClassesOption)
        select dependencyTree;
}
