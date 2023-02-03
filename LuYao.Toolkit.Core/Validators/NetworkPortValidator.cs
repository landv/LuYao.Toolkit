using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Validators;

public class NetworkPortValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "NetworkPortValidator";

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        if (value == null) return true;
        if (int.TryParse(value, out var port) == false) return false;
        if (port < 1 || port > 65535) return false;
        return true;
    }
}
