using System;
using System.CommandLine.Invocation;
using DarkLink.Kubernetes.Toolbox.Application;
using DarkLink.Kubernetes.Toolbox.Domain;
using LanguageExt.Effects.Traits;
using Microsoft.Extensions.Logging;
using Array = System.Array;

namespace DarkLink.Kubernetes.Toolbox.Cli;

public class DependenciesNfsCommandHandler<RT>(
    CommandRuntime<RT> commandRuntime,
    ILogger<DependenciesNfsCommandHandler<RT>> logger,
    DependenciesNfsUseCase<RT> useCase) : ICommandHandler where RT : struct, HasCancel<RT>
{
    public int Invoke(InvocationContext context) => InvokeAsync(context).GetAwaiter().GetResult();

    public Task<int> InvokeAsync(InvocationContext context) =>
    (
        from relevantStorageClasses in Eff(() => (context.ParseResult.GetValueForOption(Options.StorageClasses) ?? [])
            .Select(StorageClassName.From)
            .ToSeq())
        from dependencyTree in useCase.Run(relevantStorageClasses)
        from _ in PrintTree(dependencyTree)
        select unit
    ).RunCommand(commandRuntime, logger);

    private Eff<Unit> PrintTree(DependencyTree tree) => Eff(fun(() =>
    {
        foreach (var pod in tree.Pods)
        {
            Console.WriteLine($"{pod.Key.Metadata.Namespace}/{pod.Key.Metadata.Name}");
            for (var i = 0; i < pod.Value.Dependencies.Count; i++)
            {
                if(i == pod.Value.Dependencies.Count - 1)
                    Console.Write('┗');
                else
                    Console.Write('┣');

                var dependencyText = pod.Value.Dependencies[i].Match(
                    pvc => $"[PVC] {pvc.PersistentVolumeClaim.Metadata.Name} ({pvc.PersistentVolumeClaim.StorageClassName})",
                    mount => $"[NFS] {mount.Nfs.Server}");
                Console.WriteLine($" {dependencyText}");
            }
        }
    }));
}
