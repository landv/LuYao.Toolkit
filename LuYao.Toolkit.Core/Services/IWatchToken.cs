using System;
using System.IO;

namespace LuYao.Toolkit.Services;

public interface IWatchToken : IDisposable
{
    event EventHandler<FileSystemEventArgs> Changed;
    string FullName { get; }
}
