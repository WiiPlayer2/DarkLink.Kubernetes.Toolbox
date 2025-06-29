using DarkLink.Kubernetes.Toolbox.Domain;
using FluentAssertions;

namespace DarkLink.Kubernetes.Toolbox.Tests;

[TestClass]
public class DependencyTreeBuilderTest
{
    [TestMethod]
    public void Build_WithoutData_DoesNotReturnNull()
    {
        // Arrange
        var pods = Seq<Pod>();
        var pvcs = Seq<PersistentVolumeClaim>();

        // Act
        var result = DependencyTreeBuilder.Build(pods, pvcs);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void Build_WithSinglePodAndSingleReferencedPersistentVolumeClaim_ReturnsBothInTree()
    {
        // Arrange
        var pods = Seq1(new Pod(new ResourceMetadata(ResourceName.From("pods"), ResourceNamespace.From("ns")))
        {
            Volumes = [
                PodVolume.PersistentVolumeClaim(ResourceName.From("pvc")),
            ],
        });
        var pvcs = Seq1(new PersistentVolumeClaim(new(ResourceName.From("pvc"), ResourceNamespace.From("ns")), StorageClassName.From("nfs")));
        var expected = new DependencyTree(Map((pods.Head, new PodDependency(Seq1(NfsDependency.Pvc(pvcs.Head))))));
        
        // Act
        var result = DependencyTreeBuilder.Build(pods, pvcs);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Build_WithSinglePodReferencingASinglePvc_ReturnsOnlyThisDependency()
    {
        // Arrange
        var pods = Seq1(new Pod(new ResourceMetadata(ResourceName.From("pods"), ResourceNamespace.From("ns")))
        {
            Volumes = [
                PodVolume.PersistentVolumeClaim(ResourceName.From("pvc")),
            ],
        });
        var pvcs = Seq(
            new PersistentVolumeClaim(new(ResourceName.From("pvc"), ResourceNamespace.From("ns")), StorageClassName.From("nfs")),
            new PersistentVolumeClaim(new(ResourceName.From("pvc2"), ResourceNamespace.From("ns")), StorageClassName.From("nfs")));
        var expected = new DependencyTree(Map((pods.Head, new PodDependency(Seq1(NfsDependency.Pvc(pvcs.Head))))));
        
        // Act
        var result = DependencyTreeBuilder.Build(pods, pvcs);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Build_WithSinglePodReferencingASinglePvcInSameNamespace_ReturnsOnlyThisDependency()
    {
        // Arrange
        var pods = Seq1(new Pod(new ResourceMetadata(ResourceName.From("pods"), ResourceNamespace.From("ns")))
        {
            Volumes = [
                PodVolume.PersistentVolumeClaim(ResourceName.From("pvc")),
            ],
        });
        var pvcs = Seq(
            new PersistentVolumeClaim(new(ResourceName.From("pvc"), ResourceNamespace.From("ns")), StorageClassName.From("nfs")),
            new PersistentVolumeClaim(new(ResourceName.From("pvc"), ResourceNamespace.From("ns2")), StorageClassName.From("nfs")));
        var expected = new DependencyTree(Map((pods.Head, new PodDependency(Seq1(NfsDependency.Pvc(pvcs.Head))))));
        
        // Act
        var result = DependencyTreeBuilder.Build(pods, pvcs);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Build_WithOnlyOnePodReferencingPvcs_ReturnsOnlyThisDependency()
    {
        // Arrange
        var pods = Seq(
            new Pod(new ResourceMetadata(ResourceName.From("pod"), ResourceNamespace.From("ns")))
            {
                Volumes =
                [
                    PodVolume.PersistentVolumeClaim(ResourceName.From("pvc")),
                ],
            },
            new Pod(new ResourceMetadata(ResourceName.From("pod2"), ResourceNamespace.From("ns"))));
        var pvcs = Seq1(
            new PersistentVolumeClaim(new(ResourceName.From("pvc"), ResourceNamespace.From("ns")), StorageClassName.From("nfs")));
        var expected = new DependencyTree(Map((pods.Head, new PodDependency(Seq1(NfsDependency.Pvc(pvcs.Head))))));
        
        // Act
        var result = DependencyTreeBuilder.Build(pods, pvcs);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Build_WithSinglePodReferencingOnlyOneRelevantPvc_ReturnsOnlyThisDependency()
    {
        // Arrange
        var pods = Seq(
            new Pod(new ResourceMetadata(ResourceName.From("pod"), ResourceNamespace.From("ns")))
            {
                Volumes =
                [
                    PodVolume.PersistentVolumeClaim(ResourceName.From("pvc")),
                    PodVolume.PersistentVolumeClaim(ResourceName.From("pvc2")),
                ],
            },
            new Pod(new ResourceMetadata(ResourceName.From("pod2"), ResourceNamespace.From("ns"))));
        var pvcs = Seq(
            new PersistentVolumeClaim(new(ResourceName.From("pvc"), ResourceNamespace.From("ns")), StorageClassName.From("nfs")),
            new PersistentVolumeClaim(new(ResourceName.From("pvc2"), ResourceNamespace.From("ns")), StorageClassName.From("not-nfs")));
        var relevantStorageClassses = Some(Seq1(StorageClassName.From("nfs")));
        var expected = new DependencyTree(Map((pods.Head, new PodDependency(Seq1(NfsDependency.Pvc(pvcs.Head))))));
        
        // Act
        var result = DependencyTreeBuilder.Build(pods, pvcs, relevantStorageClassses);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Build_WithSinglePodWithNfsVolume_ReturnsOnlyThisDependency()
    {
        // Arrange
        var pods = Seq(
            new Pod(new ResourceMetadata(ResourceName.From("pod"), ResourceNamespace.From("ns")))
            {
                Volumes =
                [
                    PodVolume.Nfs(Hostname.From("nfs.server")),
                ],
            },
            new Pod(new ResourceMetadata(ResourceName.From("pod2"), ResourceNamespace.From("ns"))));
        var pvcs = Seq<PersistentVolumeClaim>();
        var expected = new DependencyTree(Map((pods.Head, new PodDependency(Seq1(NfsDependency.Mount((PodVolume.Nfs_) pods.Head.Volumes.Head()))))));
        
        // Act
        var result = DependencyTreeBuilder.Build(pods, pvcs);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Build_WithOnlyOneRunningPod_ReturnsOnlyThisDependency()
    {
        // Arrange
        var pods = Seq(
            new Pod(new ResourceMetadata(ResourceName.From("pod"), ResourceNamespace.From("ns")))
            {
                Volumes =
                [
                    PodVolume.Nfs(Hostname.From("nfs.server")),
                ],
                Status = new PodStatus(PodStatusPhase.From("Running")),
            },
            new Pod(new ResourceMetadata(ResourceName.From("pod2"), ResourceNamespace.From("ns")))
            {
                Volumes =
                [
                    PodVolume.Nfs(Hostname.From("nfs.server")),
                ],
                Status = new PodStatus(PodStatusPhase.From("Unknown")),
            });
        var pvcs = Seq<PersistentVolumeClaim>();
        var expected = new DependencyTree(Map((pods.Head, new PodDependency(Seq1(NfsDependency.Mount((PodVolume.Nfs_) pods.Head.Volumes.Head()))))));
        
        // Act
        var result = DependencyTreeBuilder.Build(pods, pvcs);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
