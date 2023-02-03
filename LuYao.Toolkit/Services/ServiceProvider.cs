using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using LuYao.Toolkit.IO;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LuYao.Toolkit.Services;

internal class ServiceProvider : IServiceProvider
{
    public static HttpClient HttpClient { get; } = new HttpClient();
    private class OpenFileDialog : IOpenFileDialog
    {
        private Ookii.Dialogs.Wpf.VistaOpenFileDialog _dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog { };

        public string FileName { get => _dialog.FileName; set => _dialog.FileName = value; }
        public string Title { get => _dialog.Title; set => _dialog.Title = value; }
        public string Filter { get => _dialog.Filter; set => _dialog.Filter = value; }
        public bool Multiselect { get => _dialog.Multiselect; set => _dialog.Multiselect = value; }
        public string[] FileNames => _dialog.FileNames;
        public bool ShowDialog() => _dialog.ShowDialog() == true;
    }
    private class SaveFileDialog : ISaveFileDialog
    {
        private Ookii.Dialogs.Wpf.VistaSaveFileDialog _dialog = new Ookii.Dialogs.Wpf.VistaSaveFileDialog { };

        public string Title { get => _dialog.Title; set => _dialog.Title = value; }
        public string InitialDirectory { get => _dialog.InitialDirectory; set => _dialog.InitialDirectory = value; }
        public string Filter { get => _dialog.Filter; set => _dialog.Filter = value; }
        public bool AddExtension { get => _dialog.AddExtension; set => _dialog.AddExtension = value; }
        public string FileName { get => _dialog.FileName; set => _dialog.FileName = value; }
        public string DefaultExt { get => _dialog.DefaultExt; set => _dialog.DefaultExt = value; }

        public bool ShowDialog() => _dialog.ShowDialog() == true;
    }
    public void CopyTextToClipboard(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        System.Windows.Clipboard.SetText(text);
        NotifyQuickTip("复制成功");
    }

    public string GetClipboardText()
    {
        return System.Windows.Clipboard.GetText();
    }
    public IOpenFileDialog CreateOpenFileDialog() => new OpenFileDialog();
    public ISaveFileDialog CreateSaveFileDialog() => new SaveFileDialog();

    public void MessageBoxAlert(string message, string title)
    {
        MessageBox.Show(new MessageBoxInfo { Message = message, Caption = title, Button = System.Windows.MessageBoxButton.OK });
    }

    public bool MessageBoxConfirm(string message, string title)
    {
        return MessageBox.Show(new MessageBoxInfo { Message = message, Caption = "确认", Button = System.Windows.MessageBoxButton.YesNo }) == System.Windows.MessageBoxResult.Yes;
    }
    private string GetGrowlToken()
    {
        if (WindowHelper.GetActiveWindow() is IGrowlTokenProvider provider) return provider.GrowlToken;
        return nameof(MainWindow);
    }

    public void NotifyClear() => Growl.Clear(GetGrowlToken());

    public void NotifyFail(string msg) => Growl.Fatal(msg, GetGrowlToken());

    public void NotifyInfo(string msg) => Growl.Info(msg, GetGrowlToken());

    public void NotifyQuickTip(string msg) => Growl.Success(new GrowlInfo { Token = GetGrowlToken(), Message = msg, WaitTime = 1 });

    public void NotifySuccess(string msg) => Growl.Success(msg, GetGrowlToken());

    public void NotifyWarning(string msg) => Growl.Warning(msg, GetGrowlToken());

    public void Tongji(string url)
    {
        TongjiServiceProvider.Tongji(url);
    }

    public string GetClipboardImage()
    {
        if (System.Windows.Clipboard.ContainsImage())
        {
            var img = System.Windows.Clipboard.GetImage();
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(img));
            var tmp = TempHelper.GetTempFileName();
            using (var fs = File.OpenWrite(tmp)) encoder.Save(fs);
            return tmp;
        }
        return string.Empty;
    }

    private static string GetHash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        using (var sha1 = SHA1.Create()) bytes = sha1.ComputeHash(bytes);
        return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
    }

    [DllImport("winmm.dll")]
    public static extern uint mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hWndCallback);

    public async Task PlaySound(string url)
    {
        var dir = Path.Combine(TempHelper.Root, "Sounds");
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        var fn = Path.Combine(dir, GetHash(url) + ".mp3");
        if (!File.Exists(fn))
        {
            using var ms = await HttpClient.GetStreamAsync(url);
            using var fs = File.OpenWrite(fn);
            await ms.CopyToAsync(fs);
        }
        mciSendString(@"close temp_alias", null, 0, IntPtr.Zero);
        mciSendString($@"open ""{fn}"" alias temp_alias", null, 0, IntPtr.Zero);
        mciSendString("play temp_alias", null, 0, IntPtr.Zero);
    }
}
