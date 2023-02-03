using Prism.Regions;
using System;

namespace LuYao.Toolkit.UI;

public abstract partial class DetailViewModelBase : ViewModelBase, INavigationAware
{
    private IMasterDetailViewModel _masterDetailViewModel;
    public abstract bool IsNavigationTarget(NavigationContext navigationContext);

    public abstract void OnNavigatedFrom(NavigationContext navigationContext);

    public virtual void OnNavigatedTo(NavigationContext navigationContext)
    {
        _masterDetailViewModel = null;
        if (navigationContext.Parameters.TryGetValue<IMasterDetailViewModel>("MasterDetailViewModel", out var vm)) _masterDetailViewModel = vm;
    }
    protected IMasterDetailViewModel MasterDetailViewModel => _masterDetailViewModel ?? throw new ArgumentNullException();
}
