using CommunityToolkit.Mvvm.ComponentModel;
using LuYao.Toolkit.Entities;
using System;
using System.Text.RegularExpressions;

namespace LuYao.Toolkit.Channels.Networks.PortProxy;

public partial class PortProxyItem : ObservableObject
{
    [ObservableProperty]
    private int id;
    [ObservableProperty]
    private string groupName;
    [ObservableProperty]
    private bool isEnabled;
    [ObservableProperty]
    private string type;
    [ObservableProperty]
    private string listenOn;
    [ObservableProperty]
    private string listenPort;
    [ObservableProperty]
    private string connectTo;
    [ObservableProperty]
    private string connectPort;
    [ObservableProperty]
    private string comment;

    public void Read(PortProxyRule rule)
    {
        this.Id = rule.Id;
        this.GroupName = rule.GroupName;
        this.Comment = rule.Comment;
        this.Type = rule.Type;
        this.ListenOn = rule.ListenOn;
        this.ListenPort = rule.ListenPort;
        this.ConnectTo = rule.ConnectTo;
        this.ConnectPort = rule.ConnectPort;

    }

    public void Write(PortProxyRule rule)
    {
        if (rule.Id != this.Id) throw new ArgumentException(nameof(rule));
        rule.GroupName = this.GroupName;
        rule.Comment = this.Comment;
        rule.Type = this.Type;
        rule.ListenOn = this.ListenOn;
        rule.ListenPort = this.ListenPort;
        rule.ConnectTo = this.ConnectTo;
        rule.ConnectPort = this.ConnectPort;
    }
}
