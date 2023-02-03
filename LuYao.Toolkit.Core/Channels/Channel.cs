using System;
using System.Collections.Generic;

namespace LuYao.Toolkit.Channels;

public partial class Channel
{
    public static bool TryGetItem(Guid id, out FunctionItem item)
    {
        return FunctionItem._maps.TryGetValue(id, out item);
    }
    static Channel()
    {
        Channels = new Channel[]
        {
            Gens,
            Networks,
            Converts,
            Texts,
            Encodings,
            Files,
            Images,
            CrossBorder,
            Other
        };
    }
    private Channel(string name, string title, string icon)
    {
        Name = name;
        Title = title;
        Icon = icon;
    }

    public string Name { get; }
    public string Title { get; }
    public string Icon { get; }
    public IReadOnlyList<FunctionItem> Items { get; private set; }
    public static IReadOnlyList<Channel> Channels { get; }
}
