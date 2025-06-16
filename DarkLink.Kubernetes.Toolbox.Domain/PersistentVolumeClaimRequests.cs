using System;

namespace DarkLink.Kubernetes.Toolbox.Domain;

public record PersistentVolumeClaimRequests(DataSize Storage);