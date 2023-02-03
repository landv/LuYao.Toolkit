using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.Toolkit.Channels.Encodings;

public partial class UrlEncodeViewModel : EncodingViewModelBase
{
    [RelayCommand]
    private void Encode()
    {
        if (string.IsNullOrWhiteSpace(this.Input))
        {
            this.Output = string.Empty;
        }
        else
        {
            this.Output = Uri.EscapeDataString(this.Input);
        }
    }

    [RelayCommand]
    private void Decode()
    {
        if (string.IsNullOrWhiteSpace(this.Input))
        {
            this.Output = string.Empty;
        }
        else
        {
            this.Output = Uri.UnescapeDataString(this.Input);
        }
    }
}
