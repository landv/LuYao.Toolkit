using System.Threading.Tasks;

namespace LuYao.Toolkit.Services;

public interface IServiceProvider
{
    void CopyTextToClipboard(string text);
    string GetClipboardText();
    string GetClipboardImage();
    IOpenFileDialog CreateOpenFileDialog();
    ISaveFileDialog CreateSaveFileDialog();
    void NotifyQuickTip(string msg);
    void NotifySuccess(string msg);
    void NotifyInfo(string msg);
    void NotifyWarning(string msg);
    void NotifyFail(string msg);
    void NotifyClear();
    bool MessageBoxConfirm(string message, string title);
    void MessageBoxAlert(string message, string title);
    void Tongji(string url);
    Task PlaySound(string url);
}
