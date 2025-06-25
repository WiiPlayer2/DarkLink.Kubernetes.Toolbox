namespace DarkLink.Kubernetes.Toolbox.Domain;

public static class ValidationExtension
{
    public static T GetOrThrow<TError, T>(this Validation<TError, T> validation) =>
        validation.IfFail(errors => throw new InvalidOperationException($"Validation failed: {string.Join(", ", errors)}"));
}
