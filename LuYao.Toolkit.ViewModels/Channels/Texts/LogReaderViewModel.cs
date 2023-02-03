using CommunityToolkit.Mvvm.ComponentModel;
using LuYao.Toolkit.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Texts;

public partial class LogReaderViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _path;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Output))]
    private List<string> _lines = new List<string>();

    public string Output
    {
        get => string.Join(Environment.NewLine, Lines);
        set => Debug.WriteLine(value);
    }

    private const int MAX_LINES = 200;
    
    partial void OnPathChanged(string value)
    {
        this.Lines.Clear();
        var encoding = File.Exists(value) ? FileService.GetEncoding(value) : Encoding.UTF8;
        StartRead(value, encoding);
    }

    private async void StartRead(string fn, Encoding encoding)
    {
        long lastPosition = 0;
        var info = new FileInfo(fn);
        while (fn == Path)
        {
            info.Refresh();
            if (!info.Exists)
            {
                await Task.Delay(1000);
                continue;
            }
            if (info.Length != lastPosition)
            {
                Read(fn, encoding, lastPosition);
                lastPosition = info.Length;
            }
            else
            {
                await Task.Delay(300);
            }
        }
    }

    private void Read(string fn, Encoding encoding, long position)
    {
        using var fs = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        fs.Seek(position, SeekOrigin.Begin);
        using var sr = new StreamReader(fs, encoding);
        var queue = new Queue<string>(this.Lines);
        while (!sr.EndOfStream)
        {
            var str = sr.ReadLine();
            queue.Enqueue(str);
            while (queue.Count > MAX_LINES) queue.Dequeue();
        }
        this.Lines = queue.ToList();
    }

    public override void Destroy()
    {
        base.Destroy();
        this.Path = String.Empty;
    }
}
