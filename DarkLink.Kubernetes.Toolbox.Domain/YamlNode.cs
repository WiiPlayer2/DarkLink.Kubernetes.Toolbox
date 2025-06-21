using System.Globalization;
using FunicularSwitch.Generators;

namespace DarkLink.Kubernetes.Toolbox.Domain;

[UnionType(CaseOrder = CaseOrder.AsDeclared)]
public abstract partial record YamlNode
{
    public record YamlNodeMap(Map<YamlMapKey, YamlNode> Values) : YamlNode
    {
        public override string ToString() => Values.ToString();
    }
    
    public record YamlNodeList(Lst<YamlNode> Values) : YamlNode
    {
        public override string ToString() => Values.ToString();
    }

    public abstract record YamlScalar : YamlNode
    {
        public record YamlNodeString(string Value) : YamlScalar
        {
            public override string ToString() => $"'{Value}'";
        }

        public record YamlNodeBool(bool Value) : YamlScalar
        {
            public override string ToString() => Value.ToString();
        }

        public record YamlNodeNumber(double Value) : YamlScalar
        {
            public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
        }

        public record YamlNodeNull : YamlScalar
        {
            public override string ToString() => "<null>";
        }
    }
}

public record PersistentVolumeClaimV2(YamlNode.YamlNodeMap Values)
{
    public string StorageClassName
    {
        get => Values
            .Get<YamlNode.YamlScalar.YamlNodeString>(YamlPath.MapItem(YamlMapKey.From("spec"), YamlPath.MapItem(YamlMapKey.From("storageClassName"), YamlPath.This())))
            .IfFail(() => throw new NotImplementedException())
            .Value;
        init => Values = Values with
        {
            Values = Values.Values.SetItem(YamlMapKey.From("spec"), YamlNode.String(value)),
        };
    }
}