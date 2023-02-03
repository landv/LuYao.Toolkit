using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace LuYao.Toolkit.Rdm;

[INotifyPropertyChanged]
public partial class RdpGroup
{
    public static RdpGroup All { get; } = new RdpGroup { Id = Guid.Empty, Name = "全部分组", Rank = int.MinValue };
    public static RdpGroup None { get; } = new RdpGroup { Id = Guid.Empty, Name = "无", Rank = int.MaxValue };
    public RdpGroup() { }
    public RdpGroup(Entities.RdpGroup e) : this()
    {
        this.Id = e.Id;
        this.Name = e.Name;
        this.Rank = e.Rank;
    }
    [ObservableProperty]
    private Guid _id;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private int _rank;
}
