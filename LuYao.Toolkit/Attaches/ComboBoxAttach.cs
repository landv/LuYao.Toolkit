using NewLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LuYao.Toolkit.Attaches;

public static class ComboBoxAttach
{

    public static readonly DependencyProperty BindEnumProperty =
        DependencyProperty.RegisterAttached("BindEnum", typeof(Type), typeof(ComboBoxAttach), new PropertyMetadata(null, OnBindEnumChanged));
    public static void SetBindEnum(DependencyObject element, Type value)
    {
        element.SetValue(BindEnumProperty, value);
    }

    public static Type GetBindEnum(DependencyObject element)
    {
        return (Type)element.GetValue(BindEnumProperty);
    }
    private static void OnBindEnumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ComboBox combo && e.NewValue is Type type && type.IsEnum)
        {
            var values = Enum.GetValues(type);
            combo.DisplayMemberPath = "Display";
            combo.SelectedValuePath = "Value";
            combo.Items.Clear();
            foreach (Enum item in values)
            {
                var desc = item.GetDescription();
                if (string.IsNullOrWhiteSpace(desc)) desc = item.ToString();
                var dto = new
                {
                    Display = desc,
                    Value = item
                };
                combo.Items.Add(dto);
            }
        }
    }
}
