using System;

namespace LuYao.Toolkit.Views;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class ViewNameAttribute : Attribute
{
    public string Name { get; }

    public ViewNameAttribute(string name)
    {
        Name = name;
    }
}
