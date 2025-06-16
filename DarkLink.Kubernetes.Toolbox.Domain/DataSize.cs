using System;

namespace DarkLink.Kubernetes.Toolbox.Domain;

public record DataSize(uint Count, DataSizeUnit Unit);