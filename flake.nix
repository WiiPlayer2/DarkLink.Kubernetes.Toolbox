{
  description = "A very basic flake";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
    flake-parts.url = "github:hercules-ci/flake-parts";
  };

  outputs = { flake-parts, ... } @ inputs:
    flake-parts.lib.mkFlake
    { inherit inputs; }
    (
      {
        systems = [
          "x86_64-linux"
          "aarch64-linux"
          "i686-linux"
          "x86_64-darwin"
          "aarch64-darwin"
        ];

        flake = {
          overlays.default =
            final: prev:
            {
              k8s-toolbox = final.pkgs.callPackage ./package.nix {};
            };
        };

        perSystem =
          { pkgs, ... }:
          {
            packages = rec {
              k8s-toolbox = pkgs.callPackage ./package.nix {};
              default = k8s-toolbox;
            };
          };
      }
    );
}
