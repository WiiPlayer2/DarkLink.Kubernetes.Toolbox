using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using DarkLink.Kubernetes.Toolbox.Cli;
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
        hostBuilder => hostBuilder
            .ConfigureServices(ConfigureServices<Runtime>)
            .UseCommandHandler<DependenciesCommand.NfsCommand, DependenciesNfsCommandHandler>()
            .ConfigureLogging(loggingBuilder => loggingBuilder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("DarkLink", LogLevel.Debug)
                .AddConsole()))
    .Build()
    .InvokeAsync(args);

void ConfigureServices<RT>(HostBuilderContext hostContext, IServiceCollection services)
    where RT : struct, HasCancel<RT>
{
}