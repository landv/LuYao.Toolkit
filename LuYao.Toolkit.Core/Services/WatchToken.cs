using NewLife.Threading;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace LuYao.Toolkit.Services;

public class WatchToken : IWatchToken
{
    public WatchToken(FileSystemWatcher watcher, string fullName,int delay)
    {
        Watcher = watcher ?? throw new ArgumentNullException(nameof(watcher));
        watcher.Created += Watcher_Changed;
        watcher.Changed += Watcher_Changed;
        watcher.Deleted += Watcher_Changed;
        watcher.Renamed += Watcher_Changed;
        watcher.EnableRaisingEvents = true;
        _timer = new TimerX(Raise, null, int.MaxValue, int.MaxValue);
        _delay = delay;
        FullName = fullName;
    }
    private int _delay;
    private readonly ConcurrentQueue<FileSystemEventArgs> _queue = new ConcurrentQueue<FileSystemEventArgs>();
    private readonly TimerX _timer;


    private void Watcher_Changed(object sender, FileSystemEventArgs e)
    {
        _queue.Enqueue(e);
        _timer.SetNext(_delay);
    }

    private void Raise(object state)
    {
        FileSystemEventArgs e = null;
        while (_queue.TryDequeue(out var current))
        {
            if (e != null && e.FullPath != current.FullPath)
            {
                Changed?.Invoke(this, e);
            }
            if (e == null)
            {
                e = current;
            }
            else
            {
                var type = e.ChangeType | current.ChangeType;
                e = new FileSystemEventArgs(type, Path.GetDirectoryName(e.FullPath) ?? string.Empty, e.Name);
            }
        }
        if (e != null)
        {
            Changed?.Invoke(this, e);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    ~WatchToken()
    {
        Dispose(false);
    }
    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        this.Watcher.EnableRaisingEvents = false;
        this.Watcher.Created -= this.Watcher_Changed;
        this.Watcher.Changed -= this.Watcher_Changed;
        this.Watcher.Deleted -= this.Watcher_Changed;
        this.Watcher.Renamed -= this.Watcher_Changed;
        this._timer.Dispose();
        this.Watcher.Dispose();
        _disposed = true;
        if (disposing) GC.SuppressFinalize(this);
    }

    private bool _disposed = false;

    public FileSystemWatcher Watcher { get; }

    public string FullName { get; }

    public event EventHandler<FileSystemEventArgs> Changed;
}
