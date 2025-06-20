using System;
using FunicularSwitch.Generators;

namespace DarkLink.Kubernetes.Toolbox.Domain;

[UnionType(CaseOrder = CaseOrder.AsDeclared)]
public abstract partial record YamlNodeAccessFailure(YamlPath Path)
{
    public record UnexpectedType_(Type Expected, Type Actual, YamlPath Path) : YamlNodeAccessFailure(Path);
}
