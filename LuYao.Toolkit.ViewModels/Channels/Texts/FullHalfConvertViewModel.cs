using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Text;

namespace LuYao.Toolkit.Channels.Texts;

public partial class FullHalfConvertViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _input = string.Empty;
    [ObservableProperty]
    private string _output = string.Empty;
    [RelayCommand]
    private void ToFull()
    {
        if (string.IsNullOrWhiteSpace(this.Input))
        {
            this.Output = string.Empty;
            return;
        }
        var str = this.Input;
        var len = str.Length;
        var sb = new StringBuilder();
        for (int i = 0; i < len; i++)
        {
            int t = str[i];
            if (t == 32)
            {
                t = 12288;
            }
            else if (t >= 33 && t <= 126)
            {
                t += 65248;
            }
            else if (t == '．')
            {
                t = '。';
            }
            sb.Append((char)t);
        }
        this.Output = sb.ToString();
    }
    [RelayCommand]
    private void ToHalf()
    {
        if (string.IsNullOrWhiteSpace(this.Input))
        {
            this.Output = string.Empty;
            return;
        }
        var str = this.Input;
        var len = str.Length;
        var sb = new StringBuilder();
        for (int i = 0; i < len; i++)
        {
            int t = str[i];
            if (t == 12288)
            {
                t = 32;
            }
            else if (t >= 65281 && t <= 65374)
            {
                t -= 65248;
            }
            else if (t == '。')
            {
                t = '．';
            }
            sb.Append((char)t);
        }
        this.Output = sb.ToString();
    }
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
