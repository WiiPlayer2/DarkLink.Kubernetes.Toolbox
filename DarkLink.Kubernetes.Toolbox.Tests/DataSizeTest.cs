namespace DarkLink.Kubernetes.Toolbox.Tests;

[TestClass]
public class DataSizeTest
{
    [DataRow(0U, DataSizeUnit.Gibibyte, "0Gi")]
    [DataRow(1U, DataSizeUnit.Gibibyte, "1Gi")]
    [DataRow(10U, DataSizeUnit.Gibibyte, "10Gi")]
    [DataRow(512U, DataSizeUnit.Gibibyte, "512Gi")]
    [DataRow(99999999U, DataSizeUnit.Gibibyte, "99999999Gi")]
    [DataRow(0U, DataSizeUnit.Mibibyte, "0Mi")]
    [DataRow(1U, DataSizeUnit.Mibibyte, "1Mi")]
    [DataRow(10U, DataSizeUnit.Mibibyte, "10Mi")]
    [DataRow(512U, DataSizeUnit.Mibibyte, "512Mi")]
    [DataRow(99999999U, DataSizeUnit.Mibibyte, "99999999Mi")]
    [TestMethod]
    public void ToString_WithGivenValues_ReturnsExpectedString(uint count, DataSizeUnit unit, string expected)
    {
        // Arrange
        var subject = new DataSize(count, unit);

        // Act
        var result = subject.ToString();

        // Assert
        result.Should().Be(expected);
    }
}
