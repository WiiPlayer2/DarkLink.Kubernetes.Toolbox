using System;

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
}