using HandyControl.Controls;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LuYao.Toolkit.Attaches;

public static class UniformSpacingPanelAttach
{
    static UniformSpacingPanelAttach()
    {
    }
    public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached(
        "Columns",
        typeof(int),
        typeof(UniformSpacingPanel),
        new PropertyMetadata(0, OnColumnsChanged)
        );

    private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UniformSpacingPanel panel)
        {
            FixItemWidth(panel);
            panel.SizeChanged += Panel_SizeChanged;
        }
    }
    public static readonly DependencyProperty LayoutProperty = DependencyProperty.RegisterAttached(
        "Layout",
        typeof(ColLayout),
        typeof(UniformSpacingPanel),
        new PropertyMetadata(null, OnLayoutChanged)
    );

    private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UniformSpacingPanel panel)
        {
            FixItemWidth(panel);
            panel.SizeChanged += Panel_SizeChanged;
        }
    }

    public static void SetColumns(DependencyObject element, int value) => element.SetValue(ColumnsProperty, value);
    public static int GetColumns(DependencyObject element) => (int)element.GetValue(ColumnsProperty);
    public static ColLayout GetLayout(DependencyObject element) => (ColLayout)element.GetValue(LayoutProperty);
    public static void SetLayout(DependencyObject element, ColLayout value) => element.SetValue(LayoutProperty, value);

    private static void Panel_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (sender is UniformSpacingPanel panel) FixItemWidth(panel);
    }
    private static void FixItemWidth(UniformSpacingPanel panel)
    {
        var full = panel.ActualWidth;
        if (full <= 0) full = panel.Width;
        var cols = GetColumns(panel);
        var layout = GetLayout(panel);
        if (layout != null)
        {
            FrameworkElement parent = System.Windows.Window.GetWindow(panel);
            if (parent == null)
            {
                FrameworkElement cursor = panel;
                while (cursor.Parent is FrameworkElement element)
                {
                    cursor = element;
                    parent = cursor;
                }
            }
            if (parent == null) parent = panel;
            var status = ColLayout.GetLayoutStatus(parent.ActualWidth);
            var unit = ColLayout.ColMaxCellCount;
            switch (status)
            {
                case HandyControl.Data.ColLayoutStatus.Xs: unit = layout.Xs; break;
                case HandyControl.Data.ColLayoutStatus.Sm: unit = layout.Sm; break;
                case HandyControl.Data.ColLayoutStatus.Md: unit = layout.Md; break;
                case HandyControl.Data.ColLayoutStatus.Lg: unit = layout.Lg; break;
                case HandyControl.Data.ColLayoutStatus.Xl: unit = layout.Xl; break;
                case HandyControl.Data.ColLayoutStatus.Xxl: unit = layout.Xxl; break;
            }
            if (unit > 0 && unit <= ColLayout.ColMaxCellCount)
            {
                cols = ColLayout.ColMaxCellCount / unit;
            }
        }
        if (cols <= 0) return;
        double spacing = 0;
        if (!double.IsNaN(panel.Spacing))
        {
            spacing = panel.Spacing;
        }
        else if (!double.IsNaN(panel.HorizontalSpacing))
        {
            spacing = panel.HorizontalSpacing;
        }
        var itemWidth = (full - (cols - 1) * spacing) / cols;
        panel.ItemWidth = itemWidth;
    }
}
