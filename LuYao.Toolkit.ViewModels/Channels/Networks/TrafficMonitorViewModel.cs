using CommunityToolkit.Mvvm.ComponentModel;
using LuYao.Toolkit.Services;
using NewLife.Caching;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Networks;

public partial class TrafficMonitorViewModel : ViewModelBase, INavigationAware
{
    private static MemoryCache ProcessCache = new MemoryCache();
    private record ProcessInfo(string ProcessName, string FileName, string ModuleName, DateTime? StartTime, string FileDescription);
    private static ProcessInfo GetProcessInfo(int id) => ProcessCache.GetOrAdd<ProcessInfo>(id.ToString(), CreateProcessInfo, 300);

    private static bool TryGet<T>(Process p, Func<Process, T> get, out T value)
    {
        try
        {
            value = get(p);
            return true;
        }
        catch (Exception)
        {
            value = default;
            return false;
        }
    }

    private static ProcessInfo CreateProcessInfo(string arg)
    {
        string processName = string.Empty;
        string fileName = string.Empty;
        string moduleName = string.Empty;
        string fileDescription = string.Empty;
        DateTime? startTime = default;
        using (var p = Process.GetProcessById(Convert.ToInt32(arg)))
        {
            if (TryGet(p, p => p.ProcessName, out var name)) processName = name;
            if (TryGet(p, p => p.StartTime, out var time)) startTime = time;
            if (TryGet(p, p => p.MainModule, out var mainModule) && mainModule != null)
            {
                fileName = mainModule.FileName;
                moduleName = mainModule.ModuleName;
                if (mainModule.FileVersionInfo != null)
                {
                    fileDescription = mainModule.FileVersionInfo.FileDescription;
                }
            }
        }
        return new ProcessInfo(processName, fileName, moduleName, startTime, fileDescription);
    }

    [INotifyPropertyChanged]
    public partial class TrafficReport
    {
        [ObservableProperty]
        private int pid;
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string fileName;
        [ObservableProperty]
        private string moduleName;
        [ObservableProperty]
        private int count;
        [ObservableProperty]
        private DateTime? startTime;
        [ObservableProperty]
        private string fileDescription;
    }
    #region INavigationAware
    public bool IsNavigationTarget(NavigationContext navigationContext) => true;
    public void OnNavigatedFrom(NavigationContext navigationContext) => this._isRunning = false;
    public void OnNavigatedTo(NavigationContext navigationContext) => this._isRunning = true;
    #endregion
    private bool _isRunning = true;

    public TrafficMonitorViewModel() => Run();
    private async void Run()
    {
        while (true)
        {
            if (this._isRunning)
            {
                var count = GetAllTcpConnections()
                    .GroupBy(i => i.owningPid)
                    .ToDictionary(i => i.Key, i => i.Count());

                for (int i = 0; i < this.Reports.Count; i++)
                {
                    var item = this.Reports[i];
                    if (count.TryGetValue(item.Pid, out var value))
                    {
                        item.Count = value;
                        count.Remove(item.Pid);
                    }
                    else
                    {
                        this.Reports.RemoveAt(i);
                        i--;
                    }
                }
                if (count.Count > 0)
                {
                    var forAdd = count.Select(i => new TrafficReport { Pid = i.Key, Count = i.Value }).ToList();
                    foreach (var item in forAdd)
                    {
                        this.Reports.Add(item);
                    }
                    Reload(forAdd);
                }
            }
            await Task.Delay(1000);
        }
    }
    private async void Reload(IReadOnlyList<TrafficReport> items)
    {
        foreach (var item in items)
        {
            var info = await Task.Run(() => GetProcessInfo(item.Pid));
            item.Name = info.ProcessName;
            item.FileName = info.FileName;
            item.StartTime = info.StartTime;
            item.ModuleName = info.ModuleName;
            item.FileDescription = info.FileDescription;
        }
    }

    [ObservableProperty]
    private ObservableCollection<TrafficReport> _reports = new ObservableCollection<TrafficReport>();
}
