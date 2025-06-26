using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Net.Mime;
using CliWrap;
using DarkLink.Kubernetes.Toolbox.Application;
using DarkLink.Kubernetes.Toolbox.Cli;
using DarkLink.Kubernetes.Toolbox.Demo;
using DarkLink.Kubernetes.Toolbox.KubernetesClient;
using LanguageExt.Effects.Traits;
using LanguageExt.Sys.Live;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

return await new CommandLineBuilder(new RootCommand()
    {
        new DependenciesCommand(),
    })
    .UseDefaults()
    .AddMiddleware(RunAuxCliCommand)
    .UseHost(
        args => new HostBuilder().ConfigureDefaults(args),
        hostBuilder => UseCommandHandlers<Runtime>(hostBuilder)
            .ConfigureServices(services => services.AddSingleton(new CommandRuntime<Runtime>(Runtime.New())))
            .ConfigureServices(ConfigureServices<Runtime>)
            .ConfigureLogging(loggingBuilder => loggingBuilder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("DarkLink", LogLevel.Debug)
                .AddConsole()))
    .Build()
    .InvokeAsync(args);

IHostBuilder UseCommandHandlers<RT>(IHostBuilder hostBuilder) where RT : struct, HasCancel<RT> => hostBuilder
    .UseCommandHandler<DependenciesCommand.NfsCommand, DependenciesNfsCommandHandler<RT>>();

async Task RunAuxCliCommand(InvocationContext context, Func<InvocationContext, Task> next)
{
    if (context.ParseResult.Errors.Any() && context.ParseResult.Tokens.Any() && context.ParseResult.CommandResult.Command is RootCommand)
    {
        var tokens = context.ParseResult.Tokens;
        var tool = tokens.First().Value;
        var args = tokens
            .Skip(1)
            .Select(x => x.Value)
            .ToArray();

        var cliResult = await Cli.Wrap($"k8s-toolbox-{tool}")
            .WithArguments(args)
            .WithValidation(CommandResultValidation.None)
            .ExecuteAsync(context.GetCancellationToken());
        
        context.ExitCode = cliResult.ExitCode;
    }
    else
    {
        await next(context);
    }
}

void ConfigureServices<RT>(HostBuilderContext hostContext, IServiceCollection services)
    where RT : struct, HasCancel<RT>
{
#if DEBUG
    services.AddDemoServices<RT>();
#endif

    services.AddKubernetesClient<RT>();
    
    services.AddApplicationServices<RT>();
}