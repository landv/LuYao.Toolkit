using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.Channels;
using LuYao.Toolkit.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LuYao.Toolkit.Tabs.Session;

public partial class IndexViewModel : ViewModelBase
{
    private readonly IEventAggregator eventAggregator;
    public IndexViewModel(IEventAggregator eventAggregator)
    {
        Sessions = new ObservableCollection<ChannelItemSessionItem>();
        this.eventAggregator = eventAggregator;
        ReloadSession();
        if (this.Sessions.Count > 0) this.Session = this.Sessions[0];
        this.eventAggregator.GetEvent<OpenFunctionItemEvent>().Subscribe(this.OnOpenFunctionItem);
    }

    private void OnOpenFunctionItem(OpenFunctionItemEventPayload payload)
    {
        if (payload.IsMultiboxing) return;
        if (payload.IsNewSession)
        {
            var item = payload.Item;
            var session = Entities.ChannelItemSession.FindById(item.Id);
            if (session == null) session = new Entities.ChannelItemSession
            {
                Id = item.Id,
                CreatedAt = DateTime.Now,
                IsFavorite = 0,
                LastClick = DateTime.Now,
            };
            var idx = -1;
            for (int i = 0; i < this.Sessions.Count; i++)
            {
                var tmp = this.Sessions[i];
                if (tmp.Item.Id == item.Id)
                {
                    idx = i;
                    tmp.LastClick = session.LastClick;
                    this.Session = tmp;
                    break;
                }
            }

            if (idx < 0)
            {
                var add = new ChannelItemSessionItem(item) { IsFavorite = session.IsFavorite, LastClick = session.LastClick };
                this.Sessions.Insert(0, add);
                this.Session = add;
            }
            else if (idx > 0)
            {
                this.Sessions.Move(idx, 0);
            }
        }
    }

    public ObservableCollection<ChannelItemSessionItem> Sessions { get; }
    [ObservableProperty]
    private ChannelItemSessionItem _session;
    private void ReloadSession()
    {
        var list = Entities.ChannelItemSession.FindAll(50);
        //only add
        foreach (var item in list)
        {
            var find = Sessions.FirstOrDefault(i => i.Item.Id == item.Id);
            if (find == null)
            {
                if (Channel.TryGetItem(item.Id, out var channelItem))
                {
                    var session = new ChannelItemSessionItem(channelItem)
                    {
                        IsFavorite = item.IsFavorite,
                        LastClick = item.LastClick
                    };
                    this.Sessions.Add(session);
                }
                else
                {
                    item.Delete();
                }
            }
            else
            {
                find.IsFavorite = item.IsFavorite;
                find.LastClick = item.LastClick;
            }
        }
    }
    private void Open(FunctionItem item, bool isNewSession)
    {
        this.eventAggregator.GetEvent<OpenFunctionItemEvent>()
            .Publish(new OpenFunctionItemEventPayload(item)
            {
                IsNewSession = isNewSession
            });
    }
    [RelayCommand]
    private void Open(FunctionItem item)
    {
        if (item == null) return;
        this.IsSearching = false;
        Open(item, true);
    }
    partial void OnSessionChanged(ChannelItemSessionItem value)
    {
        if (value == null) return;
        Open(value.Item, false);
    }
    [RelayCommand]
    private void OpenNew(ChannelItemSessionItem item)
    {
        if (item == null) return;
        this.eventAggregator.GetEvent<OpenFunctionItemEvent>()
            .Publish(new OpenFunctionItemEventPayload(item.Item) { IsMultiboxing = true });
    }
    [ObservableProperty]
    private bool _isSearching = false;
    [ObservableProperty]
    private string _keyword = string.Empty;
    public ObservableCollection<FunctionItem> Suggestions { get; } = new ObservableCollection<FunctionItem>();
    private void Search(string key)
    {
        if (!this.IsSearching && !string.IsNullOrWhiteSpace(key)) this.IsSearching = true;
        var items = FunctionItem.Search(key, 10);
        this.Suggestions.Clear();
        if (items.Count > 0)
        {
            foreach (var item in items) this.Suggestions.Add(item);
        }
    }
    partial void OnIsSearchingChanged(bool value)
    {
        if (value == false) this.Keyword = string.Empty;
    }
    partial void OnKeywordChanged(string value) => Search(value);
    [RelayCommand]
    private void OpenSearch() => this.IsSearching = true;
    [RelayCommand]
    private void CloseSearch() => this.IsSearching = false;
    [RelayCommand]
    private void Delete(ChannelItemSessionItem item)
    {
        if (item == null) return;
        var check = item == this.Session;
        this.Sessions.Remove(item);
        var find = Entities.ChannelItemSession.FindById(item.Item.Id);
        if (find != null) find.Delete();
        if (check && this.Sessions.Count > 0)
        {
            this.Session = this.Sessions[0];
        }
    }
}
