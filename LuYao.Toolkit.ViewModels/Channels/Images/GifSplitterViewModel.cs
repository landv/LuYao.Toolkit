using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageMagick;
using LuYao.Toolkit.IO;
using NewLife.Log;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Images;

public partial class GifSplitterViewModel : ViewModelBase
{
    public record Frame(int Index, string FileName) : IDisposable
    {
        public void Dispose()
        {
            if (File.Exists(this.FileName)) return;
            File.Delete(this.FileName);
        }
    }
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ClearCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExportCommand))]
    private string _origin;


    [ObservableProperty]
    private ObservableCollection<Frame> _frames = new ObservableCollection<Frame>();

    [RelayCommand]
    private async Task OpenFile()
    {
        var dialog = Services.FileDialogService.CreateOpenFileDialog();
        dialog.Title = "打开 Gif 图片";
        dialog.Filter = "GIF|*.gif";
        dialog.Multiselect = false;
        if (!dialog.ShowDialog()) return;
        var fn = dialog.FileName;
        var ext = MagickFormatInfo.Create(fn);
        if (ext == null) throw new Exception("图片格式识别失败，请确认图片文件是否正确");
        if (ext.IsMultiFrame == false) throw new Exception("选择文件不是动画图片。");
        using var _ = this.Busy();
        this.Origin = fn;
        using (var collection = new MagickImageCollection(fn))
        {
            if (this.Frames.Count > 0) foreach (var frame in this.Frames) frame.Dispose();
            this.Frames.Clear();
            collection.Coalesce();
            for (int i = 0; i < collection.Count; i++)
            {
                IMagickImage<ushort> item = collection[i];
                var tmp = TempHelper.GetTempFileName();
                await Task.Run(() => { item.Write(tmp, MagickFormat.Png); });
                Frames.Add(new Frame(i, tmp));
            }
        }
    }
    private bool CanClear() => !string.IsNullOrWhiteSpace(this.Origin);

    [RelayCommand(CanExecute = nameof(CanClear))]
    private void Clear()
    {
        this.Origin = String.Empty;
        this.Frames.Clear();
    }

    [RelayCommand]
    private void Save(Frame frame)
    {
        var dialog = Services.FileDialogService.CreateSaveFileDialog();
        dialog.Title = "保存图片帧";
        dialog.Filter = "PNG|*.png";
        dialog.AddExtension = true;
        dialog.FileName = Path.GetFileNameWithoutExtension(this.Origin) + "_" + frame.Index + ".png";
        if (!dialog.ShowDialog()) return;
        var fn = dialog.FileName;
        if (string.IsNullOrWhiteSpace(Path.GetExtension(fn))) fn += ".png";
        File.Copy(frame.FileName, fn, true);
        Services.NotifyService.Success("保存成功!");
    }

    private bool CanExport() => !string.IsNullOrWhiteSpace(this.Origin);

    [RelayCommand(CanExecute = nameof(CanExport))]
    private void Export()
    {
        var dialog = Services.FileDialogService.CreateSaveFileDialog();
        dialog.Title = "导出所有帧";
        dialog.Filter = "压缩文件|*.zip";
        dialog.AddExtension = true;
        dialog.FileName = Path.GetFileNameWithoutExtension(this.Origin) + ".zip";
        if (!dialog.ShowDialog()) return;
        var fn = dialog.FileName;
        if (string.IsNullOrWhiteSpace(Path.GetExtension(fn))) fn += ".zip";
        if (File.Exists(fn)) File.Delete(fn);
        using (var zip = ZipFile.Open(fn, ZipArchiveMode.Create))
        {
            foreach (var item in this.Frames)
            {
                zip.CreateEntryFromFile(item.FileName, $"{Path.GetFileNameWithoutExtension(this.Origin)}_{item.Index}.png");
            }
        }
        Services.NotifyService.Success("保存成功!");
    }
}
