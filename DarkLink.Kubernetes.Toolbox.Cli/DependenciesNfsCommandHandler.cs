using System;
using System.CommandLine.Invocation;

namespace DarkLink.Kubernetes.Toolbox.Cli;

public class DependenciesNfsCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context) => InvokeAsync(context).GetAwaiter().GetResult();

    public Task<int> InvokeAsync(InvocationContext context) => throw new NotImplementedException();
}
