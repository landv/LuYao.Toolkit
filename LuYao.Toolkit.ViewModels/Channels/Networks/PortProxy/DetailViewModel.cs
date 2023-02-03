using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using LuYao.Toolkit.Entities;
using LuYao.Toolkit.Events;
using LuYao.Toolkit.PortProxy;
using LuYao.Toolkit.UI;
using NewLife;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Networks.PortProxy;

public partial class DetailViewModel : DetailViewModelBase
{
    private IEventAggregator _eventAggregator;

    public DetailViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        _eventAggregator.GetEvent<EntityDeletedEvent<PortProxyRule>>().Subscribe(this.OnDelete);
    }

    private void OnDelete(PortProxyRule obj)
    {
        if (this.IsNew) return;
        if (obj.Id != this.Detail.Id) return;
        this.MasterDetailViewModel.IsShowDetail = false;
    }

    public override bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public override void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        if (navigationContext.Parameters.TryGetValue<Int32>("Id", out var id))
        {
            this.Id = id;
        }
        else
        {
            this.Id = 0;
        }
        this.Reload();
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNew))]
    [NotifyCanExecuteChangedFor(nameof(RefreshCommand))]
    private int _id;
    public bool IsNew => Id == default;

    [ObservableProperty]
    private PortProxyItem _detail;

    [ObservableProperty]
    private IReadOnlyList<string> _groups = new List<string>();

    [ObservableProperty]
    private IReadOnlyList<string> _connectTos = new List<string>();

    public IReadOnlyDictionary<string, string> Types { get; } = new SortedDictionary<string, string>
    {
        {"","(自动)" },
        {"v4tov4","v4tov4" },
        {"v4tov6","v4tov6" },
        {"v6tov4","v6tov4" },
        {"v6tov6","v6tov6" }
    };

    private void Reload()
    {
        this.Detail = new PortProxyItem
        {
            ListenOn = "*",
            IsEnabled = true,
            Type = string.Empty
        };
        if (!this.IsNew)
        {
            var e = PortProxyRule.FindByKey(this.Id);
            if (e == null) throw new Exception($"Id 为 {this.Id} 的数据不存在");
            this.Detail.Read(e);
        }

        this.Groups = PortProxyRule.GetGroups();
        this.ConnectTos = PortProxyRule.GetConnectTos();
    }
    private bool IsIPv6(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip)) return false;
        if (IPAddress.TryParse(ip, out var addr) == false) return false;
        return addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;
    }

    private string GetPassType(string listenOn, string connectTo)
    {
        var from = IsIPv6(listenOn) ? "v6" : "v4";
        var to = IsIPv6(connectTo) ? "v6" : "v4";
        return $"{from}to{to}";
    }

    private bool CanRefresh()
    {
        return this.IsNew == false;
    }

    [RelayCommand(CanExecute = nameof(CanRefresh))]
    private void Refresh()
    {
        this.Reload();
    }

    [RelayCommand]
    private void Save()
    {
        PortProxyItemValidator.Instance.ValidateAndThrow(this.Detail);
        var isEnabled = true;
        this.Detail.Type = GetPassType(this.Detail.ListenOn, this.Detail.ConnectTo);
        var cmd = new List<string>();
        PortProxyRule e;
        if (this.IsNew)
        {
            e = new PortProxyRule { };
        }
        else
        {
            e = PortProxyRule.FindByKeyForEdit(this.Id);
            if (e == null) throw new Exception($"Id 为 {this.Id} 的数据不存在");
            cmd.Add(CmdUtil.GenDeleteProxyCommand(e));
            var sysProxies = CmdUtil.GetProxies();
            var find = false;
            foreach (var item in sysProxies)
            {
                if (item.EqualsWithKeys(e))
                {
                    find = true;
                    break;
                }
            }
            if (find == false)
            {
                isEnabled = false;
            }
        }
        this.Detail.Write(e);
        e.Save();
        cmd.Add(CmdUtil.GenAddOrUpdateProxyCommand(e));
        if (isEnabled && cmd.Count > 0) CmdRunner.Run(cmd);
        if (this.IsNew)
        {
            this._eventAggregator.GetEvent<EntityCreatedEvent<PortProxyRule>>().Publish(e);
            this.Id = e.Id;
        }
        else
        {
            this._eventAggregator.GetEvent<EntityUpdatedEvent<PortProxyRule>>().Publish(e);
        }
        this.Reload();
    }
}
