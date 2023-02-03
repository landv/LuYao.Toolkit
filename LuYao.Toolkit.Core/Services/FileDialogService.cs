namespace LuYao.Toolkit.Services;

public static class FileDialogService
{
    public static IOpenFileDialog CreateOpenFileDialog() => ServiceProviderContainer.Provider.CreateOpenFileDialog();
    public static ISaveFileDialog CreateSaveFileDialog() => ServiceProviderContainer.Provider.CreateSaveFileDialog();
}