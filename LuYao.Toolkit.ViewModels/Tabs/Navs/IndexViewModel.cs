using CommunityToolkit.Mvvm.Input;
using NewLife.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LuYao.Toolkit.Tabs.Navs;

public partial class IndexViewModel : ViewModelBase
{
    public IndexViewModel()
    {
        this.Groups = NavGroup.GetAll();
    }

    public IReadOnlyCollection<NavGroup> Groups { get; }

    [RelayCommand]
    private void Open(NavItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Url)) return;
        var builder = new UriBuilder(item.Url);
        var qs = System.Web.HttpUtility.ParseQueryString(builder.Query);
        qs["from"] = "LuYao.Toolkit";
        qs["ver"] = AssemblyX.Entry.Version;
        builder.Query = qs.ToString();
        Start(builder.ToString());
        Services.TongjiService.Tongji(Views.ViewNames.Tabs.Navs.Index + "?jump=" + Uri.EscapeDataString(item.Title));
    }

    public static void Start(string url) // 调用系统默认的浏览器 
    {
        try
        {
            var p = Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            if (url.StartsWith("http")) throw new Exception($"系统默认浏览器未设置好，请设置后重试！[{ex.Message}]");
            throw new Exception($"系统未找到能打开{url}文件的应用程序！[{ex.Message}]");
        }
    }
}
