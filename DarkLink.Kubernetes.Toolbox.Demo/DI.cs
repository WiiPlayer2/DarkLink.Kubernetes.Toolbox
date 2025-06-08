using DarkLink.Kubernetes.Toolbox.Application;
using LanguageExt.Effects.Traits;
using Microsoft.Extensions.DependencyInjection;

namespace DarkLink.Kubernetes.Toolbox.Demo;

public static class DI
{
    public static void AddDemoServices<RT>(this IServiceCollection services) where RT : struct, HasCancel<RT>
    {
        services.AddSingleton<IKubernetesClient<RT>, DemoKubernetesClient<RT>>();
    }
}
