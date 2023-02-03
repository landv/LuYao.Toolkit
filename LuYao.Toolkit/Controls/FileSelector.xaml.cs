using LuYao.Toolkit.IO;
using LuYao.Toolkit.Services;
using NewLife;
using NewLife.Log;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ookii.Dialogs.Wpf;

namespace LuYao.Toolkit.Controls;

/// <summary>
/// FileSelector.xaml 的交互逻辑
/// </summary>
public partial class FileSelector : UserControl
{
    public FileSelector()
    {
        InitializeComponent();
    }
    public FileType FileType { get; set; } = FileType.All;

    public string FileTypeDescription { get; set; }
    public string CustomerExtensions { get; set; }
    public string DialogTitle { get; set; }

    public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(nameof(FilePath), typeof(string), typeof(FileSelector), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string FilePath
    {
        get => (string)GetValue(FilePathProperty);
        set => SetValue(FilePathProperty, value);
    }


    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(FileSelector), new FrameworkPropertyMetadata("可将文件拖拽至此", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new VistaOpenFileDialog { Multiselect = false, Title = DialogTitle };

        dialog.Filter = this.FileType.BuildFilter(this.CustomerExtensions);

        if (dialog.ShowDialog() == true)
        {
            SelectFile(dialog.FileName);
        }
    }

    private void SelectFile(string file)
    {
        FilePath = file;
    }

    private void OpenFolder_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FilePath)) return;

        var arg = $"/select,\"{FilePath.Replace("\"", "\"\"")}\"";
        try
        {
            Process.Start("explorer", arg);
        }
        catch (Exception exception)
        {
            XTrace.WriteLine("打开文件失败，路径：{0}", FilePath);
            XTrace.WriteException(exception);
        }
    }

    private void CopyFullName_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FilePath)) return;

        ClipboardService.CopyText(FilePath);
    }

    private void CopyFileName_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FilePath)) return;

        var fn = System.IO.Path.GetFileName(FilePath);
        if (string.IsNullOrWhiteSpace(fn)) return;

        ClipboardService.CopyText(fn);
    }

    private void FileNameTextBox_PreviewDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(DataFormats.FileDrop) is string[] files)
        {
            var file = files.First();
            SelectFile(file);
        }
    }

    private void FileNameTextBox_PreviewDragOver(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.None;
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] files)
            {
                var file = files.First();
                if (File.Exists(file))
                {
                    if (this.FileType.TryGetExtensions(this.CustomerExtensions, out var extensions))
                    {
                        var ext = Path.GetExtension(file) ?? string.Empty;
                        if (string.IsNullOrWhiteSpace(ext) || extensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                        {
                            e.Effects = DragDropEffects.Copy;
                        }
                    }
                    else
                    {
                        e.Effects = DragDropEffects.Copy;
                    }
                }
            }
        }

        e.Handled = true;
    }
}
