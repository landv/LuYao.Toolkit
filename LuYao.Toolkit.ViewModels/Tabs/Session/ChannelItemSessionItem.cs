using CommunityToolkit.Mvvm.ComponentModel;
using LuYao.Toolkit.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.Toolkit.Tabs.Session;

[INotifyPropertyChanged]
public partial class ChannelItemSessionItem
{
    public ChannelItemSessionItem(FunctionItem item)
    {
        Item = item ?? throw new ArgumentNullException(nameof(item));
    }
    public FunctionItem Item { get; }
    [ObservableProperty]
    private int _isFavorite;
    [ObservableProperty]
    private DateTime _lastClick;
}
