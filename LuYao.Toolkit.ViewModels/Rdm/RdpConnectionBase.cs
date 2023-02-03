using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Rdm;

[INotifyPropertyChanged]
public partial class RdpConnectionBase
{
    [ObservableProperty]
    private Guid id;
    [ObservableProperty]
    private string name;
    [ObservableProperty]
    private string host;
    [ObservableProperty]
    private int port;
    [ObservableProperty]
    private string username;
    [ObservableProperty]
    private Guid _groupId;
    [ObservableProperty]
    private string groupName;
    [ObservableProperty]
    private DateTime _updatedAt;
    public RdpConnectionBase() { }
    public RdpConnectionBase(Entities.RdpConnection e) : this()
    {
        this.Id = e.Id;
        this.Name = e.Name;
        this.Host = e.Host;
        this.Port = e.Port;
        this.Username = e.Username;
        this.GroupId = e.GroupId;
        this.GroupName = e.GroupName;
        this.UpdatedAt = e.UpdatedAt;
    }
}
