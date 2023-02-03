using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.Events;
using Prism.Events;
using Prism.Services.Dialogs;
using System;

namespace LuYao.Toolkit.Dialogs;

public partial class ToolkitSettingsDialogViewModel : ViewModelBase, IDialogAware
{
    private IEventAggregator eventAggregator;

    public ToolkitSettingsDialogViewModel(IEventAggregator eventAggregator)
    {
        this.eventAggregator = eventAggregator;
    }

    public string Title => "设置";

    public event Action<IDialogResult> RequestClose;

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        ToolkitConfig.Current = null;
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        this.Config = ToolkitConfig.Current;
    }

    [ObservableProperty]
    private ToolkitConfig _config;

    [RelayCommand]
    private void Save()
    {
        this.Config.Save();
        eventAggregator.GetEvent<ThemeChangedEvent>().Publish(this.Config.Theme);
        this.RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }
}
