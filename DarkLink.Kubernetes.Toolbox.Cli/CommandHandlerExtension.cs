using LanguageExt;
using LanguageExt.Effects.Traits;
using Microsoft.Extensions.Logging;

namespace DarkLink.Kubernetes.Toolbox.Cli;

internal static class CommandHandlerExtension
{
    public static Task<int> RunCommand<RT>(this Aff<RT, Unit> aff, CommandRuntime<RT> commandRuntime, ILogger logger)
        where RT : struct, HasCancel<RT> =>
        aff.Run(commandRuntime.Runtime)
            .Map(fin => fin.Match(
                _ => 0,
                error =>
                {
                    logger.LogError(error.ToException(), "Command failed");
                    return -1;
                }))
            .AsTask();
}