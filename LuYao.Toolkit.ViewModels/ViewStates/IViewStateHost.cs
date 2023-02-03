using System.ComponentModel;

namespace LuYao.Toolkit.ViewStates
{
    public interface IViewStateHost : INotifyPropertyChanged
    {
        public int InstanceId { get; }
    }
}
