using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Encodings;

public partial class Base62EncodeViewModel : EncodingViewModelBase
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
            var encoding = this.GetEncoding();
            var bytes = encoding.GetBytes(this.Input);
            this.Output = StringZipper.Base62.Encode(bytes);
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
            var encoding = this.GetEncoding();
            try
            {
                var bytes = StringZipper.Base62.Decode(this.Input);
                this.Output = encoding.GetString(bytes);
            }
            catch (Exception e)
            {
                this.Output = e.Message;
            }
        }
    }
}
