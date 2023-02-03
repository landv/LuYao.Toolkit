using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace LuYao.Toolkit.Channels.Converts;

public partial class IndentXmlViewModel : ViewModelBase, IFileDragDropTarget
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
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str);
            var sb = new StringBuilder();
            using (var w = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true, IndentChars = "\t" }))
            {
                doc.Save(w);
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
        if (string.IsNullOrWhiteSpace(Content)) return;
        try
        {
            var str = this.Content;
            str = StringZipper.Unzip(str);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str);
            var sb = new StringBuilder();
            using (var w = XmlWriter.Create(sb, new XmlWriterSettings { Indent = false, IndentChars = "\t" }))
            {
                doc.Save(w);
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
        dialog.Title = "保存 XML 文件";
        dialog.Filter = "XML|*.xml|所有文件|*.*";
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
        if (group == "XML")
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
