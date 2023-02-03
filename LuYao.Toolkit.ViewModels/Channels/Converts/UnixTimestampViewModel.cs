using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Converts;
public enum UnixTimestampUnit
{
    Secound,
    Millisecond
}
public partial class UnixTimestampViewModel : ViewModelBase
{
    public UnixTimestampViewModel()
    {
        this._toTimestampInput = DateTime.Now;
        switch (this.Unit)
        {
            case UnixTimestampUnit.Secound:
                this.ToTimeInput = DateTimeOffset.Now.ToUnixTimeSeconds();
                break;
            case UnixTimestampUnit.Millisecond:
                this.ToTimeInput = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                break;
            default: throw new ArgumentOutOfRangeException();
        }
        this.Update();
        this.RunUpdate();
    }

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Unit))]
    private UnixTimestampUnit _unit = UnixTimestampUnit.Secound;

    [ObservableProperty]
    private long _current;

    [ObservableProperty]
    private bool _isRunning = true;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(AutoUpdate))]
    private bool _autoUpdate = true;

    [RelayCommand]
    private void Copy()
    {
        var i = this.Current;
        if (i <= 0) return;
        var txt = i.ToString();
        Services.ClipboardService.CopyText(txt);
    }
    [RelayCommand]
    private void Start() => this.AutoUpdate = true;
    [RelayCommand]
    private void Stop() => this.AutoUpdate = false;
    [RelayCommand]
    private void Update()
    {
        switch (this.Unit)
        {
            case UnixTimestampUnit.Secound:
                this.Current = DateTimeOffset.Now.ToUnixTimeSeconds();
                break;
            case UnixTimestampUnit.Millisecond:
                this.Current = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    private async void RunUpdate()
    {
        var sleep = TimeSpan.FromMilliseconds(100);
        while (IsRunning)
        {
            if (this.AutoUpdate) Update();
            await Task.Delay(sleep);
        }
    }
    public override void Destroy()
    {
        base.Destroy();
        if (this.IsRunning) this.IsRunning = false;
    }

    [ObservableProperty]
    private long _toTimeInput;
    [ObservableProperty]
    private string _toTimeOutput;
    [RelayCommand]
    private void ToTime()
    {
        DateTimeOffset offset;
        switch (this.Unit)
        {
            case UnixTimestampUnit.Secound:
                offset = DateTimeOffset.FromUnixTimeSeconds(this.ToTimeInput);
                break;
            case UnixTimestampUnit.Millisecond:
                offset = DateTimeOffset.FromUnixTimeMilliseconds(this.ToTimeInput);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
        var timezone = TimeZoneInfo.Local;
        var d = TimeZoneInfo.ConvertTimeFromUtc(offset.DateTime, timezone);
        this.ToTimeOutput = d.ToString("yyyy-MM-dd HH:mm:ss");
    }

    [ObservableProperty]
    private DateTime _toTimestampInput;
    [ObservableProperty]
    private long? _toTimestampOutput;
    [RelayCommand]
    private void ToTimestamp()
    {
        var offset = new DateTimeOffset(this.ToTimestampInput, TimeZoneInfo.Local.BaseUtcOffset);
        switch (this.Unit)
        {
            case UnixTimestampUnit.Secound:
                this.ToTimestampOutput = offset.ToUnixTimeSeconds();
                break;
            case UnixTimestampUnit.Millisecond:
                this.ToTimestampOutput = offset.ToUnixTimeMilliseconds();
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }
}
