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
    
    [TestMethod]
    public void GetT_WithGettingDifferentTypeThenProvided_ReturnsUnexpectedTypeFailure()
    {
        // Arrange
        var path = YamlPath.This();
        var expected = YamlNodeAccessFailure.UnexpectedType(
            typeof(YamlNode.YamlScalar.YamlNodeString),
            typeof(YamlNode.YamlScalar.YamlNodeBool),
            path);
        var subject = YamlNode.Bool(true);

        // Act
        var result = subject.Get<YamlNode.YamlScalar.YamlNodeString>(path);

        // Assert
        result.ShouldBeFail(x => x.Should().BeEquivalentTo([expected]));
    }
    
    [DynamicData(nameof(NonMapNodes))]
    [TestMethod]
    public void Get_WithMapItemOnNonMap_ReturnsUnexpectedTypeFailure(YamlNode subject)
    {
        // Arrange
        var path = YamlPath.MapItem(YamlMapKey.From("key"), YamlPath.This());
        var expected = YamlNodeAccessFailure.UnexpectedType(
            typeof(YamlNode.YamlNodeMap),
            subject.GetType(),
            path);

        // Act
        var result = subject.Get(path);

        // Assert
        result.ShouldBeFail(x => x.Should().BeEquivalentTo([expected]));
    }
    
    [DynamicData(nameof(NonListNodes))]
    [TestMethod]
    public void Get_WithListItemOnNonList_ReturnsUnexpectedTypeFailure(YamlNode subject)
    {
        // Arrange
        var path = YamlPath.ListItem(YamlListIndex.From(0), YamlPath.This());
        var expected = YamlNodeAccessFailure.UnexpectedType(
            typeof(YamlNode.YamlNodeList),
            subject.GetType(),
            path);

        // Act
        var result = subject.Get(path);

        // Assert
        result.ShouldBeFail(x => x.Should().BeEquivalentTo([expected]));
    }

    [TestMethod]
    public void Get_WithOutOfRangeListItem_ReturnsOutOfRangeFailure()
    {
        // Arrange
        var path = YamlPath.ListItem(YamlListIndex.From(1), YamlPath.This());
        var expected = YamlNodeAccessFailure.OutOfRange(path);
        var subject = YamlNode.List(List(YamlNode.String("henlo")));

        // Act
        var result = subject.Get(path);

        // Assert
        result.ShouldBeFail(x => x.Should().BeEquivalentTo([expected]));
    }

    [TestMethod]
    public void Get_WithOutOfRangeMapItem_ReturnsOutOfRangeFailure()
    {
        // Arrange
        var path = YamlPath.MapItem(YamlMapKey.From("key"), YamlPath.This());
        var expected = YamlNodeAccessFailure.OutOfRange(path);
        var subject = YamlNode.Map(Map((YamlMapKey.From("otherKey"), YamlNode.String("henlo"))));

        // Act
        var result = subject.Get(path);

        // Assert
        result.ShouldBeFail(x => x.Should().BeEquivalentTo([expected]));
    }

    private static IEnumerable<object[]> NonMapNodes
    {
        get
        {
            yield return [YamlNode.List(List<YamlNode>())];
            yield return [YamlNode.String(string.Empty)];
            yield return [YamlNode.Bool(default)];
            yield return [YamlNode.Number(default)];
            yield return [YamlNode.Null()];
        }
    }

    private static IEnumerable<object[]> NonListNodes
    {
        get
        {
            yield return [YamlNode.Map(Map<YamlMapKey, YamlNode>())];
            yield return [YamlNode.String(string.Empty)];
            yield return [YamlNode.Bool(default)];
            yield return [YamlNode.Number(default)];
            yield return [YamlNode.Null()];
        }
    }
}
