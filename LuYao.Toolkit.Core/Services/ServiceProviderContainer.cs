using System;

namespace LuYao.Toolkit.Services;

public static class ServiceProviderContainer
{
    public static IServiceProvider Provider { get; private set; } = new NullServiceProvider();
    public static void SetProvider(IServiceProvider provider)
    {
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }
}
