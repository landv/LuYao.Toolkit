using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Regions;

namespace LuYao.Toolkit.UI;

public partial class MasterDetailViewModelBase : ViewModelBase, IMasterDetailViewModel
{
    [ObservableProperty]
    private bool _isShowDetail = false;

    protected NavigationParameters CreateNavigationParameters()
    {
        var p = new NavigationParameters
        {
            { "MasterDetailViewModel", this }
        };
        return p;
    }
    partial void OnIsShowDetailChanged(bool value)
    {
        if (value) OnShowDetail();
        else OnHideDetail();
    }
    protected virtual void OnShowDetail() { }
    protected virtual void OnHideDetail() { }
}
