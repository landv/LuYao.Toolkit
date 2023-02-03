using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Services;

internal class NullServiceProvider : IServiceProvider
{
    public void CopyTextToClipboard(string text)
    {
        throw new NotImplementedException();
    }

    public IOpenFileDialog CreateOpenFileDialog()
    {
        throw new NotImplementedException();
    }

    public ISaveFileDialog CreateSaveFileDialog()
    {
        throw new NotImplementedException();
    }

    public string GetClipboardImage()
    {
        throw new NotImplementedException();
    }

    public string GetClipboardText()
    {
        throw new NotImplementedException();
    }

    public void MessageBoxAlert(string message, string title)
    {
        throw new NotImplementedException();
    }

    public bool MessageBoxConfirm(string message, string title)
    {
        throw new NotImplementedException();
    }

    public void NotifyClear()
    {
        throw new NotImplementedException();
    }

    public void NotifyFail(string msg)
    {
        throw new NotImplementedException();
    }

    public void NotifyInfo(string msg)
    {
        throw new NotImplementedException();
    }

    public void NotifyQuickTip(string msg)
    {
        throw new NotImplementedException();
    }

    public void NotifySuccess(string msg)
    {
        throw new NotImplementedException();
    }

    public void NotifyWarning(string msg)
    {
        throw new NotImplementedException();
    }

    public Task PlaySound(string url)
    {
        throw new NotImplementedException();
    }

    public void Tongji(string url)
    {
    }
}
