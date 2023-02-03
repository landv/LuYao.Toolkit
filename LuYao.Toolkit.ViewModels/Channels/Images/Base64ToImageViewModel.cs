using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageMagick;
using LuYao.Toolkit.IO;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace LuYao.Toolkit.Channels.Images;

public partial class Base64ToImageViewModel : ViewModelBase
{
    private static Regex DataRegex = new Regex("^data:(?<fmt>[\\w/]+);base64,(?<data>[0-9a-zA-Z/+=]+)$", RegexOptions.Compiled);//表达式对象
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearCommand))]
    private string _input;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _output = string.Empty;

    private IMagickFormatInfo _format = null;

    private bool CanConvert() => !string.IsNullOrWhiteSpace(this.Input);
    [RelayCommand(CanExecute = nameof(CanConvert))]
    private void Convert()
    {
        if (string.IsNullOrWhiteSpace(this.Input))
        {
            this.Output = string.Empty;
            this._format = null;
            return;
        }
        var m = DataRegex.Match(this.Input);
        if (m.Success == false) return;
        var data = m.Groups["data"].Value;
        var bytes = System.Convert.FromBase64String(data);
        var fmt = ImageMagick.MagickFormatInfo.Create(bytes);
        if (fmt == null) throw new System.Exception("图片格式不正确！");
        this._format = fmt;
        var tmp = TempHelper.GetTempFileName();
        File.WriteAllBytes(tmp, bytes);
        this.Output = tmp;
    }

    private bool CanClear() => !string.IsNullOrWhiteSpace(this.Input);
    [RelayCommand(CanExecute = nameof(CanClear))]
    private void Clear()
    {
        this.Input = string.Empty;
        this.Output = string.Empty;
    }

    [RelayCommand]
    private void Paste()
    {
        var txt = Services.ClipboardService.GetText();
        if (string.IsNullOrWhiteSpace(txt)) return;
        this.Input = txt;
        this.Convert();
    }

    private bool CanSave() => !string.IsNullOrWhiteSpace(this.Output);
    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(this.Output) || !File.Exists(this.Output) || this._format == null) return;
        var ext = this._format.Format.ToString().ToLower();
        var dialog = Services.FileDialogService.CreateSaveFileDialog();
        dialog.Filter = $"图片|*.{ext}";
        dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        dialog.AddExtension = true;
        dialog.Title = "保存图片";
        if (!dialog.ShowDialog()) return;
        var fn = dialog.FileName;
        if (string.IsNullOrWhiteSpace(Path.GetExtension(fn))) fn += "." + ext;
        File.Copy(this.Output, fn, true);
        Services.NotifyService.Success("图片保存成功");
    }
}
