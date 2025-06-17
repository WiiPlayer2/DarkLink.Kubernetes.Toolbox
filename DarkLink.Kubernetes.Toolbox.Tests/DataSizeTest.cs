namespace DarkLink.Kubernetes.Toolbox.Tests;

[TestClass]
public class DataSizeTest
{
    [DataRow(0U, DataSizeUnit.Gibibyte, "0Gi")]
    [DataRow(1U, DataSizeUnit.Gibibyte, "1Gi")]
    [DataRow(10U, DataSizeUnit.Gibibyte, "10Gi")]
    [DataRow(512U, DataSizeUnit.Gibibyte, "512Gi")]
    [DataRow(99999999U, DataSizeUnit.Gibibyte, "99999999Gi")]
    [DataRow(0U, DataSizeUnit.Mebibyte, "0Mi")]
    [DataRow(1U, DataSizeUnit.Mebibyte, "1Mi")]
    [DataRow(10U, DataSizeUnit.Mebibyte, "10Mi")]
    [DataRow(512U, DataSizeUnit.Mebibyte, "512Mi")]
    [DataRow(99999999U, DataSizeUnit.Mebibyte, "99999999Mi")]
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

    [DataRow(0U, DataSizeUnit.Gibibyte, "0Gi")]
    [DataRow(1U, DataSizeUnit.Gibibyte, "1Gi")]
    [DataRow(10U, DataSizeUnit.Gibibyte, "10Gi")]
    [DataRow(512U, DataSizeUnit.Gibibyte, "512Gi")]
    [DataRow(99999999U, DataSizeUnit.Gibibyte, "99999999Gi")]
    [DataRow(0U, DataSizeUnit.Mebibyte, "0Mi")]
    [DataRow(1U, DataSizeUnit.Mebibyte, "1Mi")]
    [DataRow(10U, DataSizeUnit.Mebibyte, "10Mi")]
    [DataRow(512U, DataSizeUnit.Mebibyte, "512Mi")]
    [DataRow(99999999U, DataSizeUnit.Mebibyte, "99999999Mi")]
    [TestMethod]
    public void Parse_WithGivenString_ReturnsExpectedValues(uint expectedCount, DataSizeUnit expectedUnit, string str)
    {
        // Arrange
        var expected = new DataSize(expectedCount, expectedUnit);

        // Act
        var result = DataSize.Parse(str);

        // Assert
        result.Should().Be(expected);
    }
}
