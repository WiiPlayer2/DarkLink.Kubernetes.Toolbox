using System;

namespace DarkLink.Kubernetes.Toolbox.Domain;

public record PersistentVolumeClaimV2(YamlNode Values)
{
    private static readonly YamlPath STORAGE_CLASS_NAME_PATH = YamlPath.MapItem(YamlMapKey.From("spec"), YamlPath.MapItem(YamlMapKey.From("storageClassName"), YamlPath.This()));

    public string StorageClassName
    {
        get => Values
            .Get<YamlNode.YamlScalar.YamlNodeString>(STORAGE_CLASS_NAME_PATH)
            .IfFail(() => throw new NotImplementedException())
            .Value;
        init => Values = Values.Set(STORAGE_CLASS_NAME_PATH, YamlNode.String(value))
            .IfFail(() => throw new NotImplementedException());
    }
}
