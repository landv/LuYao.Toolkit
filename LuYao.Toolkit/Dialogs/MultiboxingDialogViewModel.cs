using LuYao.Toolkit.Channels;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Dialogs;

public partial class MultiboxingDialogViewModel : ViewModelBase, IDialogAware
{
    private IRegionManager _regionManager;
    public string RegionName { get; }
    public MultiboxingDialogViewModel(IRegionManager regionManager)
    {
        this._regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
        this.RegionName = $"dialog_{Math.Abs(this.GetHashCode())}";
    }

    public string Title { set; get; }

    public event Action<IDialogResult> RequestClose;

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        this._regionManager.Regions.Remove(this.RegionName);
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        var func = parameters.GetValue<FunctionItem>("Function");
        if (func == null) return;
        this.Title = func.Title;
        this._regionManager.RegisterViewWithRegion(this.RegionName, func.View);
    }
    protected virtual void OnRequestClose(IDialogResult obj)
    {
        RequestClose?.Invoke(obj);
    }
}
