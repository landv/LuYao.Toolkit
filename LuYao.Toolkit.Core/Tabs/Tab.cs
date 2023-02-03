using LuYao.Toolkit.Views;

namespace LuYao.Toolkit.Tabs;

public class Tab
{
    public string Icon { get; }
    public string IconArchived { get; }
    public string Title { get; }
    public string View { get; }
    private Tab(string view, string title, string icon, string archived = null)
    {
        View = view;
        Title = title;
        Icon = IconArchived = icon;
        if (!string.IsNullOrWhiteSpace(archived)) IconArchived = archived;
    }
    public static Tab Explorer { get; } = new Tab(ViewNames.Tabs.Explorer.Index, "浏览", Icons.Apps);
    public static Tab Session { get; } = new Tab(ViewNames.Tabs.Session.Index, "会话", Icons.Chat);
    public static Tab Navs { get; } = new Tab(ViewNames.Tabs.Navs.Index, "导航", Icons.Web);
    public static Tab Rdp { get; } = new Tab(ViewNames.Tabs.Rdp.Index, "远程", Icons.RemoteDesktop);
}
