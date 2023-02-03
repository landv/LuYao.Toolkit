namespace LuYao.Toolkit.Services;
public static class ClipboardService
{
    public static void CopyText(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        ServiceProviderContainer.Provider.CopyTextToClipboard(text);
    }
    public static string GetText() => ServiceProviderContainer.Provider.GetClipboardText();
    public static string GetImage() => ServiceProviderContainer.Provider.GetClipboardImage();
}
