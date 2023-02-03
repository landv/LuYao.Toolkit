using CommunityToolkit.Mvvm.ComponentModel;
using LuYao.Toolkit.ViewStates;
using Prism.Navigation;
using System;
using System.Collections.Concurrent;

namespace LuYao.Toolkit;

public partial class ViewModelBase : ObservableObject, IDestructible, IViewStateHost
{
    private static readonly ConcurrentDictionary<Type, int> _count = new ConcurrentDictionary<Type, int>();
    private int instanceId;
    public ViewModelBase()
    {
        this.instanceId = _count.AddOrUpdate(this.GetType(), 1, (t, x) => x + 1);
        this.ViewState = new ViewStateBag(this);
    }
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy = false;
    public bool IsNotBusy => _isBusy == false;

    int IViewStateHost.InstanceId => this.instanceId;

    protected ViewStateBag ViewState { get; }
    public virtual void Destroy()
    {
        _count.AddOrUpdate(this.GetType(), 0, (t, x) => x - 1);
    }

    protected IDisposable Busy()
    {
        this.IsBusy = true;
        return new DisposeAction(() => this.IsBusy = false);
    }
}
