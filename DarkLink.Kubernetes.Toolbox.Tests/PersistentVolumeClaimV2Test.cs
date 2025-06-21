namespace DarkLink.Kubernetes.Toolbox.Tests;

[TestClass]
public class PersistentVolumeClaimV2Test
{
    [TestMethod]
    public void _()
    {
        // Arrange
        var subject = new PersistentVolumeClaimV2(YamlNode.Map(Map((YamlMapKey.From("spec"), YamlNode.Map(Map((YamlMapKey.From("storageClassName"), YamlNode.Null())))))));
        var expected = new PersistentVolumeClaimV2(YamlNode.Map(Map((YamlMapKey.From("spec"), YamlNode.Map(Map((YamlMapKey.From("storageClassName"), YamlNode.String("testSC"))))))));
            
        // Act
        var result = subject with
        {
            StorageClassName = "testSC",
        };

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
