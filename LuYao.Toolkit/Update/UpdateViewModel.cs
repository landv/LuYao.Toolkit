using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.IO.Updating;
using LuYao.Toolkit.IO;
using LuYao.Toolkit.Services;
using NewLife.Log;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Update;

public partial class UpdateViewModel : ViewModelBase
{
    [INotifyPropertyChanged]
    public partial class UpdateLog
    {
        [ObservableProperty]
        private string _message;
    }
    public class ReplaceTask
    {
        public string Source { get; set; }
        public string Target { get; set; }
    }
    public UpdateViewModel(UpdatePackage package)
    {
        this.Package = package ?? throw new ArgumentNullException(nameof(package));
        this.Execute();
    }
    private async void Execute()
    {
        try
        {
            await UpdateAsync();
            this.Status = UpdateStatus.Success;
        }
        catch (Exception e)
        {
            XTrace.WriteLine("更新失败");
            XTrace.WriteException(e);
            this.Status = UpdateStatus.Fail;
            NotifyService.Fail(e);
        }
    }
    public UpdatePackage Package { get; }
    public ObservableCollection<UpdateLog> Logs { get; } = new ObservableCollection<UpdateLog>();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RestartCommand))]
    private UpdateStatus _status;

    [ObservableProperty]
    private long _total;

    [ObservableProperty]
    private long _current;

    private Queue<ReplaceTask> _replaceTasks;

    private bool CanRestart => this.Status == UpdateStatus.Success;

    [RelayCommand(CanExecute = nameof(CanRestart))]
    private void Restart()
    {
        if (_replaceTasks != null)
        {
            var log = new UpdateLog { Message = "替换文件" };
            Logs.Add(log);
            while (_replaceTasks.Count > 0)
            {
                var task = _replaceTasks.Dequeue();
                if (string.IsNullOrWhiteSpace(task.Target)) continue;
                var dir = Path.GetDirectoryName(task.Target);
                var name = Path.GetFileName(task.Target);
                log.Message = $"替换文件：{name}";
                if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (File.Exists(task.Target))
                {
                    var del = task.Target + ".del";
                    if (File.Exists(del)) File.Delete(del);
                    File.Move(task.Target, del);
                }

                File.Copy(task.Source, task.Target, true);
            }
        }

        System.Windows.Forms.Application.Restart();
        System.Windows.Application.Current.Shutdown();
    }
    private async Task UpdateAsync()
    {
        Status = UpdateStatus.Checking;
        Logs.Clear();
        var replace = new Queue<ReplaceTask>();
        var checkLog = new UpdateLog { Message = "文件对比..." };
        Logs.Add(checkLog);
        var root = AppDomain.CurrentDomain.BaseDirectory;
        var queue = new Queue<UpdateFilePackage>();
        await CheckFiles(checkLog, root, queue);

        checkLog.Message = $"对比完毕，待更新：{queue.Count}";

        Status = UpdateStatus.Updating;
        this.Total = queue.Count > 0 ? queue.Sum(i => i.FileSize) : 0;
        this.Current = 0;
        using var http = new HttpClient();
        while (queue.Count > 0)
        {
            var file = queue.Dequeue();
            var fileLog = new UpdateLog { Message = $"处理文件：{file.FilePath}" };
            Logs.Add(fileLog);
            await Task.Delay(100);
            var url = (string.IsNullOrWhiteSpace(Package.BaseUrl) ? UpdateConfig.Endpoint : Package.BaseUrl) + "/" + file.Url;
            var tmp = TempHelper.GetTempFileName(file.FileHash + ".tmp");
            if (!File.Exists(tmp) || await UpdatePackageHelper.Hash(tmp) != file.FileHash)
            {
                fileLog.Message = $"下载文件：{file.FilePath}";
                using (var ms = await http.GetStreamAsync(url))
                {
                    using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
                    {
                        using (var fs = File.OpenWrite(tmp))
                        {
                            fs.SetLength(0);
                            var buffer = new byte[4096];
                            int read;
                            while ((read = await gzip.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fs.WriteAsync(buffer, 0, read);
                                Current += read;
                            }
                        }
                    }
                }
            }
            else
            {
                Current += file.FileSize;
            }
            fileLog.Message = $"下载完毕：{file.FilePath}";
            replace.Enqueue(new ReplaceTask
            {
                Source = tmp,
                Target = Path.Combine(root, file.FilePath)
            });
        }
        Logs.Add(new UpdateLog { Message = "更新下载完毕" });
        _replaceTasks = replace;
    }

    private async Task CheckFiles(UpdateLog checkLog, string root, Queue<UpdateFilePackage> queue)
    {
        foreach (var file in Package.UpdateFilePackages)
        {
            checkLog.Message = $"对比文件：{file.FilePath}";
            var fileName = Path.Combine(root, file.FilePath);
            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                queue.Enqueue(file);
            }
            else if (fileInfo.Length != file.FileSize)
            {
                queue.Enqueue(file);
            }
            else if (!string.IsNullOrWhiteSpace(file.FileVersion))
            {
                //文件版本相同的情况下，就不用对比哈希值了。
                var version = FileVersionInfo.GetVersionInfo(fileName);
                if (version.FileVersion != file.FileVersion)
                {
                    queue.Enqueue(file);
                }
            }
            else if (file.FileHash != await UpdatePackageHelper.Hash(fileName))
            {
                queue.Enqueue(file);
            }
        }
    }
}
