using System;

namespace DarkLink.Kubernetes.Toolbox.Domain;

public static class YamlNodeAccessor
{
    public static Validation<YamlNodeAccessFailure, YamlNode> Get(this YamlNode subject, YamlPath path) => path.Match(
        @this => subject,
        mapItem => subject.Match(
            map => Get(map.Values[mapItem.Key], mapItem.Next),
            list => throw new NotImplementedException(),
            @string => throw new NotImplementedException(),
            @bool => throw new NotImplementedException(),
            number => throw new NotImplementedException(),
            @null => throw new NotImplementedException()),
        listItem => subject.Match(
            map => throw new NotImplementedException(),
            list => Get(list.Values[listItem.Index.Value], listItem.Next),
            @string => throw new NotImplementedException(),
            @bool => throw new NotImplementedException(),
            number => throw new NotImplementedException(),
            @null => throw new NotImplementedException()));

    public static Validation<YamlNodeAccessFailure, T> Get<T>(this YamlNode subject, YamlPath path)
        where T : YamlNode =>
        subject.Get(path)
            .Bind<T>(x => x is T typedX
                ? typedX
                : throw new NotImplementedException());
}
