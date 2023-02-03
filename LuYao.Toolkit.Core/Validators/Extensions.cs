using FluentValidation;
using System;

namespace LuYao.Toolkit.Validators;
public static class Extensions
{
    public static IRuleBuilderOptions<T, String> IPAddress<T>(this IRuleBuilder<T, String> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new IPAddressValidator<T>());
    }
    public static IRuleBuilderOptions<T, String> NetworkPort<T>(this IRuleBuilder<T, String> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new NetworkPortValidator<T>());
    }
}
