using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageMagick;
using LuYao.Toolkit.Drawing;
using LuYao.Toolkit.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace LuYao.Toolkit.Channels.Images;

public partial class ImageToIconViewModel : ViewModelBase
{
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    private string fileName;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConvertCommand)), ViewStates.WatchViewState(nameof(X16))] private bool _x16 = true;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConvertCommand)), ViewStates.WatchViewState(nameof(X24))] private bool _x24 = true;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConvertCommand)), ViewStates.WatchViewState(nameof(X32))] private bool _x32 = true;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConvertCommand)), ViewStates.WatchViewState(nameof(X48))] private bool _x48 = true;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConvertCommand)), ViewStates.WatchViewState(nameof(X64))] private bool _x64 = true;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConvertCommand)), ViewStates.WatchViewState(nameof(X96))] private bool _x96 = true;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConvertCommand)), ViewStates.WatchViewState(nameof(X128))] private bool _x128 = true;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConvertCommand)), ViewStates.WatchViewState(nameof(X256))] private bool _x256 = true;

    private bool CanConvert()
    {
        if (string.IsNullOrWhiteSpace(this.FileName)) return false;
        var width = GetWidth();
        if (width.Count <= 0) return false;
        return true;
    }

    private List<int> GetWidth()
    {
        var ret = new List<int>();

        if (this.X16) ret.Add(16);
        if (this.X24) ret.Add(24);
        if (this.X32) ret.Add(32);
        if (this.X48) ret.Add(48);
        if (this.X64) ret.Add(64);
        if (this.X96) ret.Add(96);
        if (this.X128) ret.Add(128);
        if (this.X256) ret.Add(256);

        return ret;
    }

    [RelayCommand(CanExecute = nameof(CanConvert))]
    private void Convert()
    {
        var widths = GetWidth();
        var dialog = Services.FileDialogService.CreateSaveFileDialog();
        dialog.Title = "保存图标";
        dialog.Filter = "图标|*.ico";
        dialog.FileName = Path.GetFileNameWithoutExtension(this.FileName) + ".ico";
        dialog.InitialDirectory = Path.GetDirectoryName(this.FileName);
        dialog.DefaultExt = ".ico";
        if (dialog.ShowDialog())
        {
            widths.Reverse();
            var files = new List<string>();
            foreach (var w in widths)
            {
                var tmp = TempHelper.GetTempFileName() + ".png";
                using (var img = new MagickImage(this.FileName))
                {
                    img.Strip();
                    img.Resize(w, w);
                    img.BackgroundColor = MagickColors.Transparent;
                    img.Format = MagickFormat.Png32;
                    img.Write(tmp);
                }
                files.Add(tmp);
            }
            if (files.Count > 0)
            {
                var images = files.Select(Image.FromFile).ToList();
                using (var fs = File.OpenWrite(dialog.FileName)) IconFactory.SavePngsAsIcon(images, fs);
                images.ForEach(i => i.Dispose());
                files.ForEach(File.Delete);
                Services.NotifyService.Success("图标转换成功");
            }
        }
    }
}
