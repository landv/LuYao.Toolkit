using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.Channels;
using LuYao.Toolkit.Entities;
using LuYao.Toolkit.Events;
using LuYao.Toolkit.Rdm.Events;
using LuYao.Toolkit.Tabs;
using Prism.Events;
using System;
using System.Collections.Generic;

namespace LuYao.Toolkit;
public partial class MainWindowViewModel : ViewModelBase
{
    private IEventAggregator _eventAggregator;
    public MainWindowViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        var hasSessin = ChannelItemSession.HasSessin();
        Tabs = new List<TabItem> {
            new TabItem(Tab.Session){ IsSelected = hasSessin== true },
            new TabItem(Tab.Explorer){ IsSelected = hasSessin== false },
            new TabItem(Tab.Navs),
            new TabItem(Tab.Rdp)
        };
        _eventAggregator.GetEvent<OpenFunctionItemEvent>().Subscribe(this.OnOpenFunctionItem);
        _eventAggregator.GetEvent<OpenRdpConnectionEvent>().Subscribe(this.OnRdmOpenRdpConnection);
    }

    private void OnRdmOpenRdpConnection(OpenRdpConnectionEventPayload obj) => OpenTab(Tab.Rdp);

    private void OnOpenFunctionItem(OpenFunctionItemEventPayload payload)
    {
        if (payload.IsNewSession)
        {
            var item = payload.Item;
            var session = Entities.ChannelItemSession.FindById(item.Id);
            if (session == null) session = new Entities.ChannelItemSession
            {
                Id = item.Id,
                CreatedAt = DateTime.Now,
                IsFavorite = 0
            };
            session.LastClick = DateTime.Now;
            session.Save();
        }
        for (int i = 0; i < this.Tabs.Count; i++)
        {
            this.Tabs[i].IsSelected = this.Tabs[i].View == Tab.Session.View;
        }
    }

    public List<TabItem> Tabs { get; }
    public string UserName => Environment.UserName;
    [RelayCommand]
    void OpenTab(TabItem item)
    {
        if (item == null) return;
        OpenTab(item.Tab);
    }
    void OpenTab(Tab item)
    {
        if (item == null) return;
        foreach (var tab in this.Tabs)
        {
            tab.IsSelected = tab.View == item.View;
            if (tab.IsSelected) _eventAggregator.GetEvent<Events.OpenTabEvent>().Publish(tab.Tab);
        }
    }
}
