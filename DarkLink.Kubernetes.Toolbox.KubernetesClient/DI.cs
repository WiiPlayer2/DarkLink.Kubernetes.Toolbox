using DarkLink.Kubernetes.Toolbox.Application;
using k8s;
using LanguageExt.Effects.Traits;
using Microsoft.Extensions.DependencyInjection;

namespace DarkLink.Kubernetes.Toolbox.KubernetesClient;

public static class DI
{
    public static void AddKubernetesClient<RT>(this IServiceCollection services) where RT : struct, HasCancel<RT>
    {
        services.AddSingleton<IKubernetesClient<RT>, KubernetesClient<RT>>();
        services.AddSingleton<k8s.Kubernetes>(sp => new k8s.Kubernetes(sp.GetRequiredService<KubernetesClientConfiguration>()));
        services.AddSingleton(KubernetesClientConfiguration.BuildDefaultConfig());
    }
}
