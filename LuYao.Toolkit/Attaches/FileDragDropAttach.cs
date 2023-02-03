using System;
using System.Windows;

namespace LuYao.Toolkit.Attaches;

public static class FileDragDropAttach
{
    public static readonly DependencyProperty GroupProperty =
        DependencyProperty.Register("Group", typeof(string), typeof(FrameworkElement), new PropertyMetadata(string.Empty, OnGroupChanged)
        {
        });

    public static void SetGroup(DependencyObject element, string value)
    {
        element.SetValue(GroupProperty, value);
    }
    public static string GetGroup(DependencyObject obj)
    {
        return (string)obj.GetValue(GroupProperty);
    }

    private static void OnGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement element)
        {
            var g = Convert.ToString(e.NewValue ?? string.Empty);
            if (string.IsNullOrWhiteSpace(g))
            {
                element.AllowDrop = false;
                element.PreviewDragOver -= Element_PreviewDragOver;
                element.Drop -= Element_Drop;
            }
            else
            {
                element.AllowDrop = true;
                element.PreviewDragOver += Element_PreviewDragOver;
                element.Drop += Element_Drop;
            }
        }
    }

    private static void Element_Drop(object sender, DragEventArgs e)
    {
        if (sender is FrameworkElement element && element.DataContext is IFileDragDropTarget target)
        {
            var group = (string)element.GetValue(GroupProperty);
            if (string.IsNullOrWhiteSpace(group)) return;

            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            if (e.Data.GetData(DataFormats.FileDrop) is string[] files) target.OnFilesDropped(group, files);
        }
    }

    private static void Element_PreviewDragOver(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }
}
