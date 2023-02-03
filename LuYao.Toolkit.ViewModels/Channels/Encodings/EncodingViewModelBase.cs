using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Encodings;

public partial class EncodingViewModelBase : ViewModelBase
{
    public IReadOnlyCollection<string> Encodings { get; } = new string[] { "UTF-8", "GB2312" };

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Encoding))]
    protected string _encoding = "UTF-8";

    protected Encoding GetEncoding() => System.Text.Encoding.GetEncoding(this.Encoding);

    [ObservableProperty]
    private string _input;
    [ObservableProperty]
    private string _output;

    [RelayCommand]
    protected virtual void Copy()
    {
        Services.ClipboardService.CopyText(this.Output);
    }

    [RelayCommand]
    protected virtual void Clear()
    {
        this.Output = this.Input = String.Empty;
    }
}
