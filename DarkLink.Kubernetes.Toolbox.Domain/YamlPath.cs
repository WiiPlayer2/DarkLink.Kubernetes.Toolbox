using System;
using FunicularSwitch.Generators;

namespace DarkLink.Kubernetes.Toolbox.Domain;

[UnionType(CaseOrder = CaseOrder.AsDeclared)]
public abstract partial record YamlPath
{
    public record YamlPathThis : YamlPath;
    
    public record YamlPathMapItem(YamlMapKey Key, YamlPath Next) : YamlPath;
    
    public record YamlPathListItem(YamlListIndex Index, YamlPath Next) : YamlPath;
}
