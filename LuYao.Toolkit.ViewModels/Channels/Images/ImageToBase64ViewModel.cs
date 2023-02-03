using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageMagick;
using LuYao.Toolkit.IO;
using LuYao.Toolkit.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace LuYao.Toolkit.Channels.Images;

public partial class ImageToBase64ViewModel : ViewModelBase, IFileDragDropTarget
{
    public record OutputFormat(string Title, Func<IMagickFormatInfo, string, string> Format);
    public partial class OutputLine
    {
        public OutputLine(string title, string content)
        {
            this.Title = title;
            this.Content = content;
        }
        public string Title { get; }
        public string Content { get; }
        [RelayCommand]
        private void Copy()
        {
            ClipboardService.CopyText(this.Content);
        }
    }

    private static OutputFormat[] Formats = new OutputFormat[]
    {
        new OutputFormat("Base64：",static(fmt,str) => $"data:{fmt.MimeType};base64,{str}"),
        new OutputFormat("CSS：",static(fmt,str) => $"background-image: url(data:{fmt.MimeType};base64,{str});"),
        new OutputFormat("HTML：",static(fmt,str) => $"<img src=\"data:{fmt.MimeType};base64,{str}\" />"),
    };

    public void OnFilesDropped(string group, string[] filepaths)
    {
        if (filepaths is { Length: > 0 }) SetImage(filepaths[0]);
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ClearCommand))]
    private string _origin = string.Empty;

    partial void OnOriginChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            this.Preview = null;
        }
        else if (File.Exists(value))
        {
            var tmp = TempHelper.GetTempFileName();
            File.Copy(value, tmp, true);
            this.Preview = tmp;
        }
        else
        {
            this.Preview = value;
        }
    }

    [ObservableProperty]
    private string _preview = null;


    public ObservableCollection<OutputLine> Outputs { get; } = new ObservableCollection<OutputLine>();

    [RelayCommand]
    private void OpenFile()
    {
        var dialog = FileDialogService.CreateOpenFileDialog();
        dialog.Title = "打开图片";
        dialog.Filter = "图片文件|*.jpg;*.png;*.bmp;*.gif;*.webp|所有文件|*.*";
        dialog.Multiselect = false;
        if (!dialog.ShowDialog()) return;
        var fn = dialog.FileName;
        SetImage(fn);
    }
    private void SetImage(string filename)
    {
        this.Outputs.Clear();
        if (string.IsNullOrWhiteSpace(filename)) return;
        if (!File.Exists(filename)) throw new Exception("原图不存在！");
        var bytes = File.ReadAllBytes(filename);
        var fmt = MagickFormatInfo.Create(bytes);
        if (fmt == null) throw new Exception("原图加载失败，可能是格式不正确！");
        this.Origin = filename;
        var data = File.ReadAllBytes(this.Origin);
        var str = Convert.ToBase64String(data);
        foreach (var outputFormat in Formats) this.Outputs.Add(new OutputLine(outputFormat.Title, outputFormat.Format(fmt, str)));
    }

    private bool CanClear() => !string.IsNullOrWhiteSpace(this.Origin);
    [RelayCommand(CanExecute = nameof(CanClear))]
    private void Clear()
    {
        this.Origin = string.Empty;
        this.Outputs.Clear();
    }

    [RelayCommand]
    private void Paste()
    {
        var img = ClipboardService.GetImage();
        if (string.IsNullOrWhiteSpace(img)) return;
        this.SetImage(img);
    }
}