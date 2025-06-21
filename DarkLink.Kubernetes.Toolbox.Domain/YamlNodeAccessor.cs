using System;

namespace DarkLink.Kubernetes.Toolbox.Domain;

public static class YamlNodeAccessor
{
    public static Validation<YamlNodeAccessFailure, YamlNode> Get(this YamlNode subject, YamlPath path) =>
        subject.GetInternal(path, x => x);
        
    private static Validation<YamlNodeAccessFailure, YamlNode> GetInternal(this YamlNode subject, YamlPath path, Func<YamlPath, YamlPath> buildFailurePath)
    {
        var failurePath = buildFailurePath(YamlPath.This());
        
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
                    ? GetInternal(map.Values[mapItem.Key], mapItem.Next, x => buildFailurePath(mapItem with {Next = YamlPath.This(),}))
                    : YamlNodeAccessFailure.OutOfRange(failurePath);
            
            YamlNodeAccessFailure Failure<T>() => YamlNodeAccessFailure.UnexpectedType(
                typeof(YamlNode.YamlNodeMap),
                typeof(T),
                failurePath);
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
                    ? GetInternal(list.Values[listItem.Index.Value], listItem.Next, x => buildFailurePath(listItem with {Next = YamlPath.This(),}))
                    : YamlNodeAccessFailure.OutOfRange(failurePath);
            
            YamlNodeAccessFailure Failure<T>() => YamlNodeAccessFailure.UnexpectedType(
                typeof(YamlNode.YamlNodeList),
                typeof(T),
                failurePath);
        }
    }

    public static Validation<YamlNodeAccessFailure, T> Get<T>(this YamlNode subject, YamlPath path)
        where T : YamlNode =>
        subject.Get(path)
            .Bind<T>(x => x is T typedX
                ? typedX
                : YamlNodeAccessFailure.UnexpectedType(typeof(T), x.GetType(), path));

    public static Validation<YamlNodeAccessFailure, YamlNode> Set(this YamlNode subject, YamlPath path, YamlNode value) =>
        subject.SetInternal(path, value, x => x);
    
    public static Validation<YamlNodeAccessFailure, YamlNode> SetInternal(this YamlNode subject, YamlPath path, YamlNode value, Func<YamlPath, YamlPath> buildFailurePath)
    {
        var failurePath = buildFailurePath(YamlPath.This());
        
        return path.Match(
            _ => value,
            MapItem,
            ListItem);

        Validation<YamlNodeAccessFailure, YamlNode> MapItem(YamlPath.YamlPathMapItem mapItem)
        {
            return subject.Match(
                SetMapItem,
                nodeList => throw new NotImplementedException(),
                nodeString => throw new NotImplementedException(),
                b => throw new NotImplementedException(),
                number => throw new NotImplementedException(),
                @null => throw new NotImplementedException());

            Validation<YamlNodeAccessFailure, YamlNode> SetMapItem(YamlNode.YamlNodeMap map) =>
                map.SetInternal(mapItem.Next, value, x => buildFailurePath(mapItem with {Next = YamlPath.This(),}))
                    .Map(updatedNode =>
                        (YamlNode) (map with
                        {
                            Values = map.Values.ContainsKey(mapItem.Key)
                                ? map.Values.SetItem(mapItem.Key, updatedNode)
                                : map.Values.Add(mapItem.Key, updatedNode),
                        }));
        }

        Validation<YamlNodeAccessFailure, YamlNode> ListItem(YamlPath.YamlPathListItem listItem) => throw new NotImplementedException();
    }
}
