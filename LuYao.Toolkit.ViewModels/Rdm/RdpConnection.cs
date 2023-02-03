using CommunityToolkit.Mvvm.ComponentModel;
using LuYao.Toolkit.Rdm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Rdm;

public partial class RdpConnection : RdpConnectionBase, IRdpConnection
{
    public RdpConnection() { }
    public RdpConnection(Entities.RdpConnection e) : base(e)
    {
        this.DesktopSize = (DesktopSize)e.DesktopSize;
        this.Password = e.Password;
        this.Domain = e.Domain;
        this.ConnectToConsole = e.ConnectToConsole;
        this.DisplayWidth = e.DisplayWidth;
        this.DisplayHeight = e.DisplayHeight;
        this.AutoExpand = e.AutoExpand;
        this.SmartSizing = e.SmartSizing;
        this.ColorDepth = (ColorDepth)e.ColorDepth;
        this.AudioSetting = (AudioRedirection)e.AudioSetting;
        this.KeyboardSetting = (KeyboardRedirection)e.KeyboardSetting;
        this.RedirectDisks = e.RedirectDisks;
        this.RedirectPorts = e.RedirectPorts;
        this.RedirectPrinters = e.RedirectPrinters;
        this.RedirectSmartCards = e.RedirectSmartCards;
        this.BitmapCaching = e.BitmapCaching;
        this.AllowWallpaper = e.AllowWallpaper;
        this.AllowThemes = e.AllowThemes;
        this.AllowContents = e.AllowContents;
        this.AllowAnimation = e.AllowAnimation;
        this.AuthenticationLevel = (AuthenticationLevel)e.AuthenticationLevel;
        this.EnableCredSspSupport = e.EnableCredSspSupport;
        this.Remark = e.Remark;
        this.GroupId = e.GroupId;
    }

    [ObservableProperty]
    private DesktopSize desktopSize;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string domain;

    [ObservableProperty]
    private bool connectToConsole;

    [ObservableProperty]
    private int displayWidth;

    [ObservableProperty]
    private int displayHeight;

    [ObservableProperty]
    private bool autoExpand;

    [ObservableProperty]
    private bool smartSizing;

    [ObservableProperty]
    private ColorDepth colorDepth;

    [ObservableProperty]
    private AudioRedirection audioSetting;

    [ObservableProperty]
    private KeyboardRedirection keyboardSetting;

    [ObservableProperty]
    private bool redirectDisks;

    [ObservableProperty]
    private bool redirectPorts;

    [ObservableProperty]
    private bool redirectPrinters;

    [ObservableProperty]
    private bool redirectSmartCards;

    [ObservableProperty]
    private bool bitmapCaching;

    [ObservableProperty]
    private bool allowWallpaper;

    [ObservableProperty]
    private bool allowThemes;

    [ObservableProperty]
    private bool allowContents;

    [ObservableProperty]
    private bool allowAnimation;

    [ObservableProperty]
    private AuthenticationLevel authenticationLevel;

    [ObservableProperty]
    private bool enableCredSspSupport;

    [ObservableProperty]
    private string remark;

    [ObservableProperty]
    private Guid groupId;
    public void Write(Entities.RdpConnection e)
    {
        e.DesktopSize = (int)this.DesktopSize;
        e.Password = this.Password;
        e.Domain = this.Domain;
        e.ConnectToConsole = this.ConnectToConsole;
        e.DisplayWidth = this.DisplayWidth;
        e.DisplayHeight = this.DisplayHeight;
        e.AutoExpand = this.AutoExpand;
        e.SmartSizing = this.SmartSizing;
        e.ColorDepth = (int)this.ColorDepth;
        e.AudioSetting = (int)this.AudioSetting;
        e.KeyboardSetting = (int)this.KeyboardSetting;
        e.RedirectDisks = this.RedirectDisks;
        e.RedirectPorts = this.RedirectPorts;
        e.RedirectPrinters = this.RedirectPrinters;
        e.RedirectSmartCards = this.RedirectSmartCards;
        e.BitmapCaching = this.BitmapCaching;
        e.AllowWallpaper = this.AllowWallpaper;
        e.AllowThemes = this.AllowThemes;
        e.AllowContents = this.AllowContents;
        e.AllowAnimation = this.AllowAnimation;
        e.AuthenticationLevel = (int)this.AuthenticationLevel;
        e.EnableCredSspSupport = this.EnableCredSspSupport;
        e.Name = this.Name;
        e.Host = this.Host;
        e.Port = this.Port;
        e.Username = this.Username;
        e.Remark = this.Remark;
        e.GroupId = this.GroupId;
    }
}