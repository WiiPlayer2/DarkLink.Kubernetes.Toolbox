using DarkLink.Kubernetes.Toolbox.Domain;
using LanguageExt;
using LanguageExt.Effects.Traits;

namespace DarkLink.Kubernetes.Toolbox.Application;

public class DependenciesNfsUseCase<RT> where RT : struct, HasCancel<RT>
{
    public Aff<RT, DependencyTree> Run() => SuccessAff(new DependencyTree());
}
