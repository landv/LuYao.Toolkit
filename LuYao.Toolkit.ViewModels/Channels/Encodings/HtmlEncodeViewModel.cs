using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Encodings;

public partial class HtmlEncodeViewModel : EncodingViewModelBase
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
            this.Output = System.Web.HttpUtility.HtmlEncode(this.Input);
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
            this.Output = System.Web.HttpUtility.HtmlDecode(this.Input);
        }
    }
}
