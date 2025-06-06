using System;
using System.CommandLine.Invocation;
using DarkLink.Kubernetes.Toolbox.Application;
using LanguageExt.Effects.Traits;
using Microsoft.Extensions.Logging;

namespace DarkLink.Kubernetes.Toolbox.Cli;

public class DependenciesNfsCommandHandler<RT>(
    CommandRuntime<RT> commandRuntime,
    ILogger<DependenciesNfsCommandHandler<RT>> logger,
    DependenciesNfsUseCase<RT> useCase) : ICommandHandler where RT : struct, HasCancel<RT>
{
    public int Invoke(InvocationContext context) => InvokeAsync(context).GetAwaiter().GetResult();

    public Task<int> InvokeAsync(InvocationContext context) =>
    (
        from _ in useCase.Run()
        select unit
    ).RunCommand(commandRuntime, logger);
}
