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
                GetMapItem,
                list => Failure<YamlNode.YamlNodeList>(),
                @string => Failure<YamlNode.YamlScalar.YamlNodeString>(),
                @bool => Failure<YamlNode.YamlScalar.YamlNodeBool>(),
                number => Failure<YamlNode.YamlScalar.YamlNodeNumber>(),
                @null => Failure<YamlNode.YamlScalar.YamlNodeNull>());

            Validation<YamlNodeAccessFailure, YamlNode> GetMapItem(YamlNode.YamlNodeMap map) =>
                map.Values.ContainsKey(mapItem.Key)
                    ? Get(map.Values[mapItem.Key], mapItem.Next)
                    : YamlNodeAccessFailure.OutOfRange(path);
            
            YamlNodeAccessFailure Failure<T>() => YamlNodeAccessFailure.UnexpectedType(
                typeof(YamlNode.YamlNodeMap),
                typeof(T),
                path);
        }

        Validation<YamlNodeAccessFailure, YamlNode> ListItem(YamlPath.YamlPathListItem listItem)
        {
            return subject.Match(
                map => Failure<YamlNode.YamlNodeMap>(),
                GetListItem,
                @string => Failure<YamlNode.YamlScalar.YamlNodeString>(),
                @bool => Failure<YamlNode.YamlScalar.YamlNodeBool>(),
                number => Failure<YamlNode.YamlScalar.YamlNodeNumber>(),
                @null => Failure<YamlNode.YamlScalar.YamlNodeNull>());

            Validation<YamlNodeAccessFailure, YamlNode> GetListItem(YamlNode.YamlNodeList list) =>
                listItem.Index.Value >= 0 && listItem.Index.Value < list.Values.Count
                    ? Get(list.Values[listItem.Index.Value], listItem.Next)
                    : YamlNodeAccessFailure.OutOfRange(path);
            
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
