using NewLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LuYao.Toolkit.Helpers;

public static class ComboBoxHelper
{
    public static void BindEnum<T, TEnum>(T combo) where TEnum : Enum where T : ComboBox
    {
        var values = Enum.GetValues(typeof(TEnum));
        combo.DisplayMemberPath = "Display";
        combo.SelectedValuePath = "Value";
        combo.Items.Clear();
        foreach (TEnum item in values)
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
    public static void BindEnum<TEnum>(this ComboBox combo) where TEnum : Enum
    {
        BindEnum<ComboBox, TEnum>(combo);
    }
    public static void BindEnum<TEnum>(this HandyControl.Controls.ComboBox combo) where TEnum : Enum
    {
        BindEnum<HandyControl.Controls.ComboBox, TEnum>(combo);
    }
}