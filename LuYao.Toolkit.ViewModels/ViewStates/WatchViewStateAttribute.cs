using System;

namespace LuYao.Toolkit.ViewStates;

[AttributeUsage(AttributeTargets.Field)]
public class WatchViewStateAttribute : Attribute
{
    public string Property { get; }
    public WatchViewStateAttribute(string property)
    {
        if (string.IsNullOrWhiteSpace(property)) throw new ArgumentOutOfRangeException(nameof(property));
        Property = property;
    }
}
