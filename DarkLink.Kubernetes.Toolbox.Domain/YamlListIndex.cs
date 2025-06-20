using Vogen;
using Validation = Vogen.Validation;

namespace DarkLink.Kubernetes.Toolbox.Domain;

[ValueObject<int>]
public partial record YamlListIndex
{
    private static Validation Validate(int input)
    {
        var isValid = input >= 0;
        return isValid ? Validation.Ok : Validation.Invalid("Index must be greater than or equal to 0");
    }
}
