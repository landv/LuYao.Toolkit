namespace LuYao.Toolkit.Services;

public static class MessageBoxService
{

    public static bool Confirm(string message, string title) => ServiceProviderContainer.Provider.MessageBoxConfirm(message, title);
    public static bool Confirm(string message) => Confirm(message, "确认");
    public static void Alert(string message, string title) => ServiceProviderContainer.Provider.MessageBoxAlert(message, title);
    public static void Alert(string message) => Alert(message, "提示");
}
