using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.Entities;
using LuYao.Toolkit.Events;
using LuYao.Toolkit.PortProxy;
using LuYao.Toolkit.Regions;
using LuYao.Toolkit.UI;
using LuYao.Toolkit.Views;
using Prism.Events;
using Prism.Regions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using XCode;

namespace LuYao.Toolkit.Channels.Networks.PortProxy;
public partial class IndexViewModel : MasterDetailViewModelBase, INavigationAware, IMasterDetailViewModel
{
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;

    public IndexViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
    {
        _regionManager = regionManager ?? throw new System.ArgumentNullException(nameof(regionManager));
        _eventAggregator = eventAggregator ?? throw new System.ArgumentNullException(nameof(eventAggregator));
        _eventAggregator.GetEvent<EntityCreatedEvent<Entities.PortProxyRule>>().Subscribe(this.OnPortProxyRuleAny);
        _eventAggregator.GetEvent<EntityUpdatedEvent<Entities.PortProxyRule>>().Subscribe(this.OnPortProxyRuleAny);
        this.Reload();
    }

    private void OnPortProxyRuleAny(PortProxyRule obj)
    {
        this.Reload();
    }

    [ObservableProperty]
    public ObservableCollection<PortProxyItem> _proxies = new ObservableCollection<PortProxyItem>();

    [ObservableProperty]
    private PortProxyItem _current;

    partial void OnCurrentChanged(PortProxyItem value)
    {
        if (value != null)
        {
            var p = CreateNavigationParameters();
            p.Add("Id", value.Id);
            this.IsShowDetail = true;
            this._regionManager.RequestNavigate(RegionNames.PortProxyDetailRegion, ViewNames.Channels.Networks.PortProxy.Detail, p);
        }
    }
    protected override void OnHideDetail()
    {
        this.Current = null;
    }

    [RelayCommand]
    private void Create()
    {
        this.IsShowDetail = true;
        var p = CreateNavigationParameters();
        this._regionManager.RequestNavigate(RegionNames.PortProxyDetailRegion, ViewNames.Channels.Networks.PortProxy.Detail, p);
    }

    private void Reload()
    {
        var proxies = new ObservableCollection<PortProxyItem>();
        var sysProxies = CmdUtil.GetProxies();
        var dbRules = PortProxyRule.FindAll();
        var groups = new SortedSet<string>();
        foreach (var r in dbRules)
        {
            if (!string.IsNullOrWhiteSpace(r.GroupName)) { groups.Add(r.GroupName); }
            var item = new PortProxyItem();
            item.Read(r);
            var matched = sysProxies.FirstOrDefault(f => f.EqualsWithKeys(r));
            item.IsEnabled = matched != null;
            proxies.Add(item);
        }
        if (groups.Count > 0)
        {
            var cvs = CollectionViewSource.GetDefaultView(proxies);
            foreach (var item in proxies)
            {
                if (string.IsNullOrWhiteSpace(item.GroupName)) item.GroupName = "默认";
            }
            cvs.GroupDescriptions.Add(new PropertyGroupDescription(nameof(PortProxyItem.GroupName)));
        }
        this.Proxies = proxies;
    }

    [RelayCommand]
    private void RefreshProxyList()
    {
        var proxies = CmdUtil.GetProxies();
        var rules = PortProxyRule.FindAll();
        foreach (var proxy in proxies)
        {
            var matchedRule = rules.FirstOrDefault(r => r.EqualsWithKeys(proxy));
            if (matchedRule != null) proxy.Id = matchedRule.Id;
        }

        var pendingAdds = proxies.Where(x => x.IsValid && x.Id == 0).ToList();
        if (pendingAdds.Count > 0) pendingAdds.Save(true);

        this.Reload();
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        this.Reload();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }


    private IList<PortProxyRule> Cast(IEnumerable items)
    {
        var set = new SortedSet<int>();

        foreach (PortProxyItem item in items)
        {
            if (item == null) continue;
            set.Add(item.Id);
        }

        return PortProxyRule.FindAllByIds(set);
    }

    [RelayCommand]
    private void Enable(IEnumerable arg)
    {
        var items = Cast(arg);
        if (items.Count == 0)
        {
            Services.NotifyService.Warning("请先选择需要启用的转发规则！");
            return;
        }
        var cmds = new List<string>();
        foreach (var item in items)
        {
            cmds.Add(CmdUtil.GenAddOrUpdateProxyCommand(item));
        }
        CmdRunner.Run(cmds);
        this.Reload();
    }
    [RelayCommand]
    private void Disable(IEnumerable arg)
    {
        var items = Cast(arg);
        if (items.Count == 0)
        {
            Services.NotifyService.Warning("请先选择需要禁用的转发规则！");
            return;
        }
        var cmds = new List<string>();
        foreach (var item in items)
        {
            cmds.Add(CmdUtil.GenDeleteProxyCommand(item));
        }
        CmdRunner.Run(cmds);
        this.Reload();
    }
    [RelayCommand]
    private void Delete(IEnumerable arg)
    {
        var items = Cast(arg);
        if (items.Count == 0)
        {
            Services.NotifyService.Warning("请先选择需要删除的转发规则！");
            return;
        }
        var cmds = new List<string>();
        foreach (var item in items)
        {
            cmds.Add(CmdUtil.GenDeleteProxyCommand(item));
        }
        CmdRunner.Run(cmds);
        var e = this._eventAggregator.GetEvent<EntityDeletedEvent<Entities.PortProxyRule>>();
        foreach (var item in items)
        {
            item.Delete();
            e.Publish(item);
        }
        this.Reload();
    }
}
