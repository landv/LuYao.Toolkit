using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.Channels;
using Prism.Events;
using System;
using System.Collections.Generic;

namespace LuYao.Toolkit.Tabs.Explorer;

public partial class IndexViewModel : ViewModelBase
{
    public IndexViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        this.Selected = Channels[0];
    }
    private IEventAggregator _eventAggregator;
    public IReadOnlyList<Channel> Channels => Channel.Channels;
    [ObservableProperty]
    private Channel _selected;

    [RelayCommand]
    void OpenItem(FunctionItem item)
    {
        if (item == null) return;
        _eventAggregator.GetEvent<Events.OpenFunctionItemEvent>().Publish(new Events.OpenFunctionItemEventPayload(item)
        {
            IsNewSession = true
        });
    }
}
