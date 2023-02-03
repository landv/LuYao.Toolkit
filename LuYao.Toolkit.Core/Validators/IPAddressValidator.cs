using FluentValidation;
using FluentValidation.Validators;
using System.Net;

namespace LuYao.Toolkit.Validators;

public class IPAddressValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "IPAddressValidator";

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        if (value == null) return true;
        if (IPAddress.TryParse(value, out IPAddress ipAddress)) return true;
        return false;
    }
}
