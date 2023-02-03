using System;

namespace LuYao.Toolkit.Channels;

public partial class Channel
{
    public static OtherChannel Other { get; } = new OtherChannel();
    public class OtherChannel : Channel
    {
        public FunctionItem SystemToolkit { get; }

        public OtherChannel() : base(nameof(Other), "其他工具", Icons.Puzzle)
        {
            this.SystemToolkit = new FunctionItem(this, Guid.Parse("D8F1C1B5B3674EB6AE1BDB05EF15BC16"), nameof(SystemToolkit))
            {
                Title = "系统工具",
                Icon = Icons.MicrosoftWindows,
                Description = "Windows 系统工具",
                View = Views.ViewNames.Channels.Other.SystemToolkit,
                IsNew = true,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = false,
                Keywords = new string[] { "XiTongGongJu", "XTGJ" },
            };

            this.Items = new[]
            {
                SystemToolkit,
            };
        }
    }
}
