namespace DarkLink.Kubernetes.Toolbox.Domain;

public record PersistentVolumeClaim(
    ResourceMetadata Metadata,
    StorageClassName StorageClassName,
    PersistentVolumeClaimResources Resources
    ) : Resource(Metadata)
{
    [Obsolete]
    public PersistentVolumeClaim(ResourceMetadata metadata, StorageClassName storageClassName)
        : this(metadata, storageClassName, new(new(new(1, DataSizeUnit.Gibibyte)))) { }
    
    public Option<ResourceName> VolumeName { get; init; }
    
    public Option<VolumeMode> VolumeMode { get; init; }
    
    public Arr<VolumeAccessMode> AccessModes { get; init; }
}