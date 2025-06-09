{ lib
, buildDotnetModule

, kubectl
, frp
}:
with lib;
buildDotnetModule rec {
  pname = "k8s-toolbox";
  version = "0.1";

  src = fileset.toSource {
    root = ./.;
    fileset = fileset.fromSource (sources.cleanSource ./.);
  };

  projectFile = "DarkLink.Kubernetes.Toolbox.Cli/DarkLink.Kubernetes.Toolbox.Cli.csproj";
  nugetDeps = ./deps.json; # nix build .#k8s-toolbox.passthru.fetch-deps
  dotnetInstallFlags = "-f net8.0";

  # makeWrapperArgs = [
  #   "--suffix"
  #   "PATH"
  #   ":"
  #   (lib.makeBinPath [
  #     kubectl
  #   ])
  # ];

  meta = with lib; {
    homepage = "https://github.com/WiiPlayer2/DarkLink.Kubernetes.Toolbox";
    description = "My personal toolbox of kubernetes tools";
    license = licenses.mit;
  };
}
