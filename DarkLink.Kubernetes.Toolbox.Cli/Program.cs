using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using DarkLink.Kubernetes.Toolbox.Application;
using DarkLink.Kubernetes.Toolbox.Cli;
using DarkLink.Kubernetes.Toolbox.Demo;
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

void ConfigureServices<RT>(HostBuilderContext hostContext, IServiceCollection services)
    where RT : struct, HasCancel<RT>
{
#if DEBUG
    services.AddDemoServices<RT>();
#endif
    
    services.AddApplicationServices<RT>();
}