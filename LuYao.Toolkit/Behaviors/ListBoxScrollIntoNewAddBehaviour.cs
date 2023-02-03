using Microsoft.Xaml.Behaviors;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace LuYao.Toolkit.Behaviors;
public class ListBoxScrollIntoNewAddBehaviour : Behavior<ListBox>
{
    protected override void OnAttached()
    {
        if (this.AssociatedObject.Items is INotifyCollectionChanged notify)
        {
            notify.CollectionChanged += Notify_CollectionChanged;
        }
    }

    private void Notify_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems.Count > 0)
        {
            var item = e.NewItems[0];
            this.AssociatedObject.ScrollIntoView(item);
        }
    }

    protected override void OnDetaching()
    {
        if (this.AssociatedObject.Items is INotifyCollectionChanged notify)
        {
            notify.CollectionChanged -= Notify_CollectionChanged;
        }
    }
}