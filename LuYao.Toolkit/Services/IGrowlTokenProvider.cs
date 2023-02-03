using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Services;

public interface IGrowlTokenProvider
{
    string GrowlToken { get; }
}
