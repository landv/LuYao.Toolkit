using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Rdm;

public enum ColorDepth
{
    [Description("256色")]
    Depth8Bit = 8,
    [Description("增强色(15位)")]
    Depth15Bit = 0xF,
    [Description("增强色(16位)")]
    Depth16Bit = 0x10,
    [Description("真彩色(24位)")]
    Depth24Bit = 24,
    [Description("最高质量(32位)")]
    Depth32Bit = 0x20
}

