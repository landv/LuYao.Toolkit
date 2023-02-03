using FluentValidation;
using LuYao.Toolkit.Validators;

namespace LuYao.Toolkit.Channels.Networks.PortProxy;
public class PortProxyItemValidator : AbstractValidator<PortProxyItem>
{
    public static PortProxyItemValidator Instance { get; } = new PortProxyItemValidator();
    public PortProxyItemValidator()
    {
        RuleFor(i => i.ListenOn)
            .NotEmpty()
            .WithName("监听地址");

        RuleFor(i => i.ListenPort)
            .NotEmpty()
            .NetworkPort()
            .WithName("监听端口");

        RuleFor(i => i.ConnectTo)
            .NotEmpty()
            .IPAddress()
            .NotEqual(i => i.ListenOn)
            .WithName("转发地址");

        RuleFor(i => i.ConnectPort)
            .NotEmpty()
            .NetworkPort()
            .WithName("转发端口");
    }
}
