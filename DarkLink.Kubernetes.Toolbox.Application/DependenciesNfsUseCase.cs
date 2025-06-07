using DarkLink.Kubernetes.Toolbox.Domain;
using LanguageExt;
using LanguageExt.Effects.Traits;

namespace DarkLink.Kubernetes.Toolbox.Application;

public class DependenciesNfsUseCase<RT>(IKubernetesClient<RT> kubernetesClient) where RT : struct, HasCancel<RT>
{
    public Aff<RT, DependencyTree> Run() =>
        from pods in kubernetesClient.GetPods()
        from pvcs in kubernetesClient.GetPersistentVolumeClaims()
        let dependencyTree = DependencyTreeBuilder.Build(pods, pvcs)
        select dependencyTree;
}
