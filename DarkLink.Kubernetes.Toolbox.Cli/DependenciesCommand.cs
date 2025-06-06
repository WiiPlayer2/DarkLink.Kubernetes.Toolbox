using System.CommandLine;

namespace DarkLink.Kubernetes.Toolbox.Cli;

public class DependenciesCommand : Command
{
    public DependenciesCommand() : base("deps")
    {
        Add(new NfsCommand());
    }

    public class NfsCommand : Command
    {
        public NfsCommand() : base("nfs") { }
    }
}