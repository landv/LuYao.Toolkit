using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Converts;

public enum HexConvertBaseType { [Description("二进制")] X2 = 2, [Description("八进制")] X8 = 8, [Description("十进制")] X10 = 10, [Description("十六进制")] X16 = 16 }

public partial class HexConvertViewModel : ViewModelBase
{
    [INotifyPropertyChanged]
    public partial class Item
    {
        public Item(HexConvertBaseType type) => this.Type = type;
        public HexConvertBaseType Type { get; }
        [ObservableProperty]
        private string output;
        [RelayCommand]
        private void Copy()
        {
            Services.ClipboardService.CopyText(this.Output);
        }
        public void Read(string input, HexConvertBaseType from)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                this.Output = string.Empty;
                return;
            }
            try
            {
                var i = System.Convert.ToInt64(input, (int)from);
                this.Output = System.Convert.ToString(i, (int)this.Type);
            }
            catch (Exception)
            {
                this.Output = "NaN";
            }
        }
    }
    [ObservableProperty]
    private string input;
    [ObservableProperty]
    private HexConvertBaseType type = HexConvertBaseType.X10;
    public IReadOnlyList<Item> Items { get; } = new List<Item>
    {
        new Item(HexConvertBaseType.X2),
        new Item(HexConvertBaseType.X8),
        new Item(HexConvertBaseType.X10),
        new Item(HexConvertBaseType.X16),
    };

    partial void OnInputChanged(string value) => Convert();
    partial void OnTypeChanged(HexConvertBaseType value) => Convert();

    private void Convert()
    {
        var str = this.Input;
        var from = this.Type;
        foreach (var item in this.Items)
        {
            item.Read(str, from);
        }
    }
}
