using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.IO.Updating;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Update;

public partial class FindNewVersionDialogViewModel : ViewModelBase, IDialogAware
{
    public string Title => "发现新版本";

    public event Action<IDialogResult> RequestClose;
    protected virtual void OnRequestClose(IDialogResult obj)
    {
        RequestClose?.Invoke(obj);
    }

    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        var pkg = parameters.GetValue<UpdatePackage>("UpdatePackage");
        this.Description = pkg.Description;
    }

    [ObservableProperty]
    private string _description;

    [RelayCommand]
    private void Update()
    {
        OnRequestClose(new DialogResult(ButtonResult.Yes));
    }

    [RelayCommand]
    private void Ignore()
    {
        OnRequestClose(new DialogResult(ButtonResult.Ignore));
    }
}
