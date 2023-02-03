using System;

namespace LuYao.Toolkit.Services;

public static class NotifyService
{
    public static void QuickTip(string msg) => ServiceProviderContainer.Provider.NotifyQuickTip(msg);
    public static void Success(string msg) => ServiceProviderContainer.Provider.NotifySuccess(msg);
    public static void Info(string msg) => ServiceProviderContainer.Provider.NotifyInfo(msg);
    public static void Warning(string msg) => ServiceProviderContainer.Provider.NotifyWarning(msg);
    public static void Warning(Exception e) => ServiceProviderContainer.Provider.NotifyWarning(e.Message);
    public static void Fail(string msg) => ServiceProviderContainer.Provider.NotifyFail(msg);
    public static void Fail(Exception e) => ServiceProviderContainer.Provider.NotifyFail(e.Message);
    public static void Clear() => ServiceProviderContainer.Provider.NotifyClear();
}
