using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Rdm;

public class RdpGroupValidator : AbstractValidator<RdpGroup>
{
    public static RdpGroupValidator Instance { get; } = new RdpGroupValidator();
    public RdpGroupValidator()
    {
        this.RuleFor(i => i.Name).NotEmpty().WithName("分组名称");
    }
}
