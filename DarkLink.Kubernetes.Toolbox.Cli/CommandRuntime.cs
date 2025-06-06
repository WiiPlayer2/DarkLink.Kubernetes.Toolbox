using System;
using LanguageExt.Effects.Traits;

namespace DarkLink.Kubernetes.Toolbox.Cli;

public record CommandRuntime<RT>(RT Runtime) where RT : struct, HasCancel<RT>;
