using System;

namespace DarkLink.Kubernetes.Toolbox.Domain;

public static class YamlNodeAccessor
{
    public static Validation<YamlNodeAccessFailure, YamlNode> Get(this YamlNode subject, YamlPath path)
    {
        return path.Match(
            @this => subject,
            MapItem,
            ListItem);

        Validation<YamlNodeAccessFailure, YamlNode> MapItem(YamlPath.YamlPathMapItem mapItem)
        {
            return subject.Match(
                map => Get(map.Values[mapItem.Key], mapItem.Next),
                list => Failure<YamlNode.YamlNodeList>(),
                @string => Failure<YamlNode.YamlScalar.YamlNodeString>(),
                @bool => Failure<YamlNode.YamlScalar.YamlNodeBool>(),
                number => Failure<YamlNode.YamlScalar.YamlNodeNumber>(),
                @null => Failure<YamlNode.YamlScalar.YamlNodeNull>());

            YamlNodeAccessFailure Failure<T>() => YamlNodeAccessFailure.UnexpectedType(
                typeof(YamlNode.YamlNodeMap),
                typeof(T),
                path);
        }

        Validation<YamlNodeAccessFailure, YamlNode> ListItem(YamlPath.YamlPathListItem listItem)
        {
            return subject.Match(
                map => Failure<YamlNode.YamlNodeMap>(),
                list => Get(list.Values[listItem.Index.Value], listItem.Next),
                @string => Failure<YamlNode.YamlScalar.YamlNodeString>(),
                @bool => Failure<YamlNode.YamlScalar.YamlNodeBool>(),
                number => Failure<YamlNode.YamlScalar.YamlNodeNumber>(),
                @null => Failure<YamlNode.YamlScalar.YamlNodeNull>());

            YamlNodeAccessFailure Failure<T>() => YamlNodeAccessFailure.UnexpectedType(
                typeof(YamlNode.YamlNodeList),
                typeof(T),
                path);
        }
    }

    public static Validation<YamlNodeAccessFailure, T> Get<T>(this YamlNode subject, YamlPath path)
        where T : YamlNode =>
        subject.Get(path)
            .Bind<T>(x => x is T typedX
                ? typedX
                : YamlNodeAccessFailure.UnexpectedType(typeof(T), x.GetType(), path));
}
