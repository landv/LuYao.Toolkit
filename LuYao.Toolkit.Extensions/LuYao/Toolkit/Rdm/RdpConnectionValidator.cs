using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Rdm;

public class RdpConnectionValidator : AbstractValidator<IRdpConnection>
{
	public static RdpConnectionValidator Instance { get; } = new RdpConnectionValidator();
	public RdpConnectionValidator()
    {
        RuleFor(i => i.Name).NotEmpty().WithName("连接名称");
        RuleFor(i => i.Host).NotEmpty().WithName("主机");
        RuleFor(i => i.Port).GreaterThan(0).LessThan(65535).WithName("端口");
    }
}
