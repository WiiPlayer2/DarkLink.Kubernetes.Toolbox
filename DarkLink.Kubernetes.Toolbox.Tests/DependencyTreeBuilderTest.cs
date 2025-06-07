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
}
