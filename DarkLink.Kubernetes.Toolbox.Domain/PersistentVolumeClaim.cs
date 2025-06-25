namespace DarkLink.Kubernetes.Toolbox.Domain;

public record PersistentVolumeClaim(YamlNode Yaml) : Resource(Yaml)
{
    private static readonly YamlPath PATH_STORAGE_CLASS_NAME = YamlPath.MapItem(YamlMapKey.From("spec"), YamlPath.MapItem(YamlMapKey.From("storageClassName"), YamlPath.This())); 
    
    [Obsolete]
    public PersistentVolumeClaim(ResourceMetadata metadata, StorageClassName storageClassName)
        : this(metadata, storageClassName, new(new(new(1, DataSizeUnit.Gibibyte)))) { }

    [Obsolete]
    public PersistentVolumeClaim(ResourceMetadata Metadata,
        StorageClassName StorageClassName,
        PersistentVolumeClaimResources Resources) : this(YamlNode.Map(Map<YamlMapKey, YamlNode>()))
    {
        this.Metadata = Metadata;
        this.StorageClassName = StorageClassName;
        this.Resources = Resources;
    }

    public Option<ResourceName> VolumeName { get; init; }
    
    public Option<VolumeMode> VolumeMode { get; init; }
    
    public Arr<VolumeAccessMode> AccessModes { get; init; }

    public StorageClassName StorageClassName
    {
        get => Yaml.Get<YamlNode.YamlScalar.YamlNodeString>(PATH_STORAGE_CLASS_NAME)
            .Map(x => StorageClassName.From(x.Value))
            .GetOrThrow();
        init => Yaml = Yaml.Set(PATH_STORAGE_CLASS_NAME, new YamlNode.YamlScalar.YamlNodeString(value.Value))
            .GetOrThrow();
    }

    public PersistentVolumeClaimResources Resources { get; init; }
}