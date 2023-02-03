using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Text;
using System;

namespace LuYao.Toolkit.Channels.Converts;

public partial class IndentJsonViewModel : ViewModelBase, IFileDragDropTarget
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _content;

    [ObservableProperty]
    private string _fileName;

    [RelayCommand]
    private void Beautify()
    {
        if (string.IsNullOrWhiteSpace(Content)) return;
        try
        {
            var str = this.Content;
            str = StringZipper.Unzip(str);
            var sb = new StringBuilder();
            using (var sr = new StringReader(str))
            using (var sw = new StringWriter(sb))
            {
                JsonReader r = new JsonTextReader(sr);
                JsonWriter w = new JsonTextWriter(sw) { Formatting = Formatting.Indented };
                while (r.Read()) w.WriteToken(r);
            }
            this.Content = sb.ToString();
        }
        catch (System.Exception e)
        {
            Services.NotifyService.Warning(e);
        }
    }

    [RelayCommand]
    private void Uglify()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Content)) return;
            var str = this.Content;
            str = StringZipper.Unzip(str);
            var sb = new StringBuilder();
            using (var sr = new StringReader(str))
            using (var sw = new StringWriter(sb))
            {
                JsonReader r = new JsonTextReader(sr);
                JsonWriter w = new JsonTextWriter(sw) { Formatting = Formatting.None };
                while (r.Read()) w.WriteToken(r);
            }
            this.Content = sb.ToString();
        }
        catch (System.Exception e)
        {
            Services.NotifyService.Warning(e);
        }
    }

    [RelayCommand]
    private void Copy()
    {
        if (string.IsNullOrWhiteSpace(Content)) return;
        Services.ClipboardService.CopyText(Content);
    }

    [RelayCommand]
    private void Paste()
    {
        var text = Services.ClipboardService.GetText();
        if (string.IsNullOrWhiteSpace(text)) return;
        Content = text;
    }

    [RelayCommand]
    private void Escape()
    {
        if (string.IsNullOrWhiteSpace(this.Content)) return;
        this.Content = this.Content.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    [RelayCommand]
    private void Unescape()
    {
        if (string.IsNullOrWhiteSpace(this.Content)) return;
        this.Content = this.Content.Replace("\\\\", "\\").Replace("\\\"", "\"");
    }

    [RelayCommand]
    private void Clear()
    {
        this.Content = string.Empty;
    }
    private bool CanSave() => !string.IsNullOrWhiteSpace(this.Content);

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(this.Content)) return;
        var dialog = Services.FileDialogService.CreateSaveFileDialog();
        dialog.FileName = this.FileName;
        dialog.Title = "保存 JSON 文件";
        dialog.Filter = "JSON|*.json|所有文件|*.*";
        if (!string.IsNullOrWhiteSpace(dialog.FileName))
        {
            dialog.InitialDirectory = Path.GetDirectoryName(dialog.FileName);
        }
        else
        {
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
        if (dialog.ShowDialog())
        {
            File.WriteAllText(dialog.FileName, this.Content, Encoding.UTF8);
            Services.NotifyService.Success("保存成功");
        }
    }
    public void OnFilesDropped(string group, string[] filepaths)
    {
        if (group == "Json")
        {
            if (filepaths == null || filepaths.Length == 0) return;
            var fn = filepaths[0];
            var encoding = Services.FileService.GetEncoding(fn);
            if (encoding == null) throw new System.Exception("编码识别失败，可能不是文本文件。");
            this.Content = File.ReadAllText(fn, encoding);
            this.FileName = fn;
        }
    }
}
