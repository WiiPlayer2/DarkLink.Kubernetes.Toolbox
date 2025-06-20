using LanguageExt.UnitTesting;

namespace DarkLink.Kubernetes.Toolbox.Tests;

[TestClass]
public class YamlNodeAccessorTest
{
    [TestMethod]
    public void Get_WithThis_ReturnsNode()
    {
        // Arrange
        var path = YamlPath.This();
        var subject = YamlNode.String("henlo");

        // Act
        var result = subject.Get(path);

        // Assert
        result.ShouldBeSuccess(x => x.Should().Be(subject));
    }
    
    [TestMethod]
    public void Get_WithMapItemOnMap_ReturnsMapItem()
    {
        // Arrange
        var path = YamlPath.MapItem(YamlMapKey.From("key"), YamlPath.This());
        var expected = YamlNode.String("henlo");
        var subject = YamlNode.Map(Map((YamlMapKey.From("key"), expected)));

        // Act
        var result = subject.Get(path);

        // Assert
        result.ShouldBeSuccess(x => x.Should().Be(expected));
    }
    
    [TestMethod]
    public void Get_WithListItemOnList_ReturnsListItem()
    {
        // Arrange
        var path = YamlPath.ListItem(YamlListIndex.From(0), YamlPath.This());
        var expected = YamlNode.String("henlo");
        var subject = YamlNode.List(List(expected));

        // Act
        var result = subject.Get(path);

        // Assert
        result.ShouldBeSuccess(x => x.Should().Be(expected));
    }
    
    [TestMethod]
    public void Get_WithMultiLevelPathOnNestedValue_ReturnsNestedValue()
    {
        // Arrange
        var path = YamlPath.ListItem(YamlListIndex.From(0),
            YamlPath.MapItem(YamlMapKey.From("key"),
                YamlPath.ListItem(YamlListIndex.From(0), 
                    YamlPath.This())));
        var expected = YamlNode.String("henlo");
        var subject = YamlNode.List(List(
            YamlNode.Map(Map((YamlMapKey.From("key"),
                YamlNode.List(List(expected)))))));

        // Act
        var result = subject.Get(path);

        // Assert
        result.ShouldBeSuccess(x => x.Should().Be(expected));
    }
    
    [TestMethod]
    public void GetT_WithMultiLevelPathOnNestedValue_ReturnsNestedValue()
    {
        // Arrange
        var path = YamlPath.ListItem(YamlListIndex.From(0),
            YamlPath.MapItem(YamlMapKey.From("key"),
                YamlPath.ListItem(YamlListIndex.From(0), 
                    YamlPath.This())));
        var expected = YamlNode.String("henlo");
        var subject = YamlNode.List(List(
            YamlNode.Map(Map((YamlMapKey.From("key"),
                YamlNode.List(List(expected)))))));

        // Act
        var result = subject.Get<YamlNode.YamlScalar.YamlNodeString>(path);

        // Assert
        result.ShouldBeSuccess(x => x.Should().Be(expected));
    }
}
