using System;

namespace LuYao.Toolkit.Services;

public static class TongjiService
{
    public static void Tongji(string url)
    {
        ServiceProviderContainer.Provider.Tongji(url);
    }
    public static void Tongji(string url, object args)
    {
        var qs = System.Web.HttpUtility.ParseQueryString(string.Empty);
        if (args != null)
        {
            var type = args.GetType();
            foreach (var o in type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                if (o.CanRead == false) continue;
                var value = o.GetValue(args);
                if (value != null) qs[o.Name] = value.ToString();
            }
        }
        ServiceProviderContainer.Provider.Tongji($"{url}?{qs}");
    }
    public static void Tongji(Type type)
    {
        ServiceProviderContainer.Provider.Tongji(type.FullName);
    }
}
