using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Security.Cryptography;
using LuYao.Toolkit.Services;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Files;

public partial class HashFileViewModel : ViewModelBase, IFileDragDropTarget
{
    public enum HashStatus { Pendding, Running, Success, Failure }
    private static IDictionary<string, IHashAlgorithmItem> Factory { get; } = new SortedDictionary<string, IHashAlgorithmItem>();
    public static HashAlgorithm Create(string name)
    {
        if (Factory.TryGetValue(name, out var algorithm)) return algorithm.Create();
        return null;
    }



    public interface IHashAlgorithmItem
    {
        string Name { get; }
        bool IsSelected { get; set; }
        HashAlgorithm Create();
    }
    private partial class HashAlgorithmItem<T> : ViewModelBase, IHashAlgorithmItem where T : HashAlgorithm
    {
        [ObservableProperty]
        [ViewStates.WatchViewState(nameof(IsSelected))]
        private bool _isSelected = false;
        public HashAlgorithmItem(string name, Func<T> factory)
        {
            this.Name = name;
            Factory[name] = this;
            _factory = factory;
        }
        private readonly Func<T> _factory;
        public string Name { get; private set; }
        public HashAlgorithm Create() => _factory.Invoke();
    }
    public partial class HashTask : ViewModelBase
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Name))]
        private string _fullName;
        public string Name => Path.GetFileName(this.FullName);
        [ObservableProperty]
        private HashStatus _status;
        [ObservableProperty]
        private long _length;
        [ObservableProperty]
        private long _current;
        [ObservableProperty]
        private TimeSpan _cost;
        [ObservableProperty]
        private string _algorithm;
        [ObservableProperty]
        private string _result;
        [ObservableProperty]
        private long _speed;

        [RelayCommand]
        private void Copy()
        {
            if (string.IsNullOrWhiteSpace(this.Result)) return;
            ClipboardService.CopyText(this.Result);
        }
        public void Update(Stopwatch stopwatch)
        {
            var sec = stopwatch.ElapsedMilliseconds / 1000f;
            if (sec <= 0)
            {
                this.Speed = 0;
                return;
            }
            this.Speed = Convert.ToInt64(this.Current / sec);
        }
    }
    public ObservableCollection<IHashAlgorithmItem> HashAlgorithms { get; }
    public ObservableCollection<HashTask> Tasks { get; } = new ObservableCollection<HashTask>();
    public HashFileViewModel()
    {
        this.HashAlgorithms = new ObservableCollection<IHashAlgorithmItem>
        {
                new HashAlgorithmItem<Crc32>("CRC32",()=>new Crc32()),
                new HashAlgorithmItem<MD5>("MD5",()=>MD5.Create()),
                new HashAlgorithmItem<SHA1>("SHA1",()=>SHA1.Create()),
                new HashAlgorithmItem<SHA256>("SHA256",()=>SHA256.Create()),
                new HashAlgorithmItem<SHA384>("SHA384",()=>SHA384.Create()),
        };
        if (this.HashAlgorithms.All(i => i.IsSelected == false)) this.HashAlgorithms[1].IsSelected = true;
    }
    private Task _task = Task.CompletedTask;
    private void LoadFiles(string[] files)
    {
        if (this.HashAlgorithms.All(i => i.IsSelected == false)) this.HashAlgorithms[0].IsSelected = true;
        var names = this.HashAlgorithms.Where(i => i.IsSelected).Select(i => i.Name).ToList();

        foreach (var file in files)
        {
            var info = new FileInfo(file);
            if (info.Exists == false) continue;
            foreach (var item in this.HashAlgorithms)
            {
                if (item.IsSelected == false) continue;
                this.Tasks.Add(new HashTask
                {
                    FullName = info.FullName,
                    Status = HashStatus.Pendding,
                    Length = info.Length,
                    Current = 0,
                    Cost = TimeSpan.Zero,
                    Algorithm = item.Name,
                    Result = String.Empty
                });
            }
        }
        if (_task.IsCompleted) _task = Run();
    }

    [RelayCommand]
    private void Open()
    {
        var dialog = FileDialogService.CreateOpenFileDialog();
        dialog.Multiselect = true;
        dialog.Title = "选择需要校验的文件";
        if (dialog.ShowDialog())
        {
            var files = dialog.FileNames;
            if (files is { Length: > 0 }) this.LoadFiles(files);
        }
    }

    [RelayCommand]
    private void Clear()
    {
        var forRemove = this.Tasks.Where(i => i.Status != HashStatus.Running).ToList();
        if (forRemove.Count > 0)
        {
            foreach (var item in forRemove)
            {
                if (item.Status == HashStatus.Running) continue;
                this.Tasks.Remove(item);
            }
        }
    }
    public void OnFilesDropped(string group, string[] filepaths)
    {
        LoadFiles(filepaths);
    }
    private async Task Run()
    {
        var st = new Stopwatch();
        while (true)
        {
            var item = this.Tasks.FirstOrDefault(i => i.Status == HashStatus.Pendding);
            if (item == null) break;
            var runAt = DateTime.Now;
            try
            {
                item.IsBusy = true;
                item.Status = HashStatus.Running;
                using (HashAlgorithm a = Create(item.Algorithm))
                {
                    using var fs = File.OpenRead(item.FullName);
                    var buffer = new byte[1024 * 1024];
                    int read;
                    long total = 0;
                    var start = DateTime.Now;
                    var last = 0;
                    st.Restart();

                    while ((read = await fs.ReadAsync(buffer)) > 0)
                    {
                        a.TransformBlock(buffer, 0, read, buffer, 0);
                        total += read;
                        var now = Convert.ToInt32(1d * total / fs.Length * 1000);
                        if (last != now)
                        {
                            item.Current = total;
                            last = now;
                            item.Update(st);
                        }
                    }

                    a.TransformFinalBlock(buffer, 0, read);
                    st.Stop();
                    item.Update(st);

                    item.Result = BitConverter.ToString(a.Hash)
                        .ToLowerInvariant()
                        .Replace("-", string.Empty);

                    item.Current = total;
                }
                item.Status = HashStatus.Success;
            }
            catch (Exception e)
            {
                XTrace.WriteException(e);
                item.Result = e.Message;
                item.Status = HashStatus.Failure;
            }
            finally
            {
                item.Cost = DateTime.Now - runAt;
                item.IsBusy = false;
            }
        }
    }
}
