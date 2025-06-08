namespace DarkLink.Kubernetes.Toolbox.Cli;

public static class Options
{
    public static System.CommandLine.Option<string[]> StorageClasses { get; } = new(
        ["--storage-class", "-s"],
        () => []);
}
