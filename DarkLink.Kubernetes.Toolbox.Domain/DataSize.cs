using System;
using System.Text.RegularExpressions;

namespace DarkLink.Kubernetes.Toolbox.Domain;

public record DataSize(uint Count, DataSizeUnit Unit)
{
    public override string ToString()
    {
        return $"{Count}{GetSuffix()}";
        
        string GetSuffix() => Unit switch
        {
            DataSizeUnit.Mibibyte => "Mi",
            DataSizeUnit.Gibibyte => "Gi",
        };
    }

    public static DataSize Parse(string s)
    {
        var regex = new Regex(@"(?<count>\d+)(?<unit>[GM]i)");
        var match = regex.Match(s);
        var count = uint.Parse(match.Groups["count"].Value);
        var unit = match.Groups["unit"].Value switch
        {
            "Mi" => DataSizeUnit.Mibibyte,
            "Gi" => DataSizeUnit.Gibibyte,
        };
        var value = new DataSize(count, unit);
        return value;
    }
}