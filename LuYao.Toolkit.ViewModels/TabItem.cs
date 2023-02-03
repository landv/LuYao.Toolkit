using CommunityToolkit.Mvvm.ComponentModel;
using LuYao.Toolkit.Tabs;

namespace LuYao.Toolkit;

public partial class TabItem : ObservableObject
{
    private Tab _tab;
    public Tab Tab => _tab;
    [ObservableProperty]
    private bool _isSelected;

    public TabItem(Tab tab)
    {
        _tab = tab;
    }
    public string Icon => _tab.Icon;
    public string IconArchived => _tab.IconArchived;
    public string Title => _tab.Title;
    public string View => _tab.View;
}
