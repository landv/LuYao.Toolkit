using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace LuYao.Toolkit.Channels;

public partial class FunctionItem
{
    internal static readonly Dictionary<Guid, FunctionItem> _maps = new Dictionary<Guid, FunctionItem>();
    internal static readonly NameValueCollection _keywords = new NameValueCollection();
    public FunctionItem(Channel channel, Guid id, string name)
    {
        lock (_maps) _maps[id] = this;
        Channel = channel;
        Id = id;
        Name = name;
    }
    public Channel Channel { get; }
    public Guid Id { get; }
    public string Name { get; }
    public string Title { get; internal set; }
    public string Icon { get; internal set; }
    public string Description { get; internal set; }
    public string View { get; internal set; }
    public bool IsNew { get; internal set; }
    public bool UseNetwork { get; internal set; }
    public bool IsDocument { get; internal set; }
    public bool Multiboxing { get; internal set; } = true;
    public IReadOnlyCollection<string> Keywords { get; internal set; }
    public static IReadOnlyList<FunctionItem> Search(string keyword, int limit)
    {
        if (string.IsNullOrWhiteSpace(keyword)) return Array.Empty<FunctionItem>();
        if (_keywords.Count == 0)
        {
            lock (_keywords)
            {
                if (_keywords.Count == 0)
                {
                    foreach (var item in _maps.Values)
                    {
                        var id = item.Id.ToString();
                        _keywords.Add(item.Title, id);
                        if (item.Keywords == null || item.Keywords.Count == 0) continue;
                        foreach (var k in item.Keywords)
                        {
                            _keywords.Add(k, id);
                        }
                    }
                }
            }
        }
        var keys = new List<string>();
        foreach (var key in _keywords.AllKeys)
        {
            if (key.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) < 0) continue;
            keys.Add(key);
        }
        if (keys.Count <= 0) return Array.Empty<FunctionItem>();
        keys.Sort(static (x, y) => x.Length - y.Length);
        var ret = new List<FunctionItem>(keys.Count);
        var set = new SortedSet<Guid>();
        foreach (var key in keys)
        {
            if (ret.Count >= limit) break;
            foreach (var id in _keywords.GetValues(key))
            {
                if (Channel.TryGetItem(Guid.Parse(id), out var item))
                {
                    if (set.Contains(item.Id)) continue;
                    ret.Add(item);
                    set.Add(item.Id);
                }
            }
        }
        return ret;
    }
}
