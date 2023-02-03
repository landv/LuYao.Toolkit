using System;
using System.IO;

namespace LuYao.Toolkit.Services;

public static class WatchFileService
{
    public static IWatchToken Watch(string file, NotifyFilters filter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.Size, int delay = 500)
    {
        if (string.IsNullOrWhiteSpace(file)) throw new ArgumentNullException(nameof(file));

        var watch = new FileSystemWatcher
        {
            NotifyFilter = filter,
            Path = Path.GetDirectoryName(file),
            Filter = Path.GetFileName(file),
        };

        return new WatchToken(watch, file, delay);
    }
}
