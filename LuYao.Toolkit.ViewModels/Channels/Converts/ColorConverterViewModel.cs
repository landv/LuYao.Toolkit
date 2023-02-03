using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace LuYao.Toolkit.Channels.Converts;

public partial class ColorConverterViewModel : ViewModelBase
{
    private static readonly ColorConverter converter = new ColorConverter();
    private static Regex RgbColorRegex = new Regex("RGB\\((?<r>[\\d\\s]+),(?<g>[\\d\\s]+),(?<b>[\\d\\s]+)\\)", RegexOptions.IgnoreCase);
    private static Regex ArgbColorRegex = new Regex("ARGB\\((?<a>[\\d\\s]+),(?<r>[\\d\\s]+),(?<g>[\\d\\s]+),(?<b>[\\d\\s]+)\\)", RegexOptions.IgnoreCase);

    [INotifyPropertyChanged]
    public partial class CodeItem
    {
        public CodeItem(string title, Func<Color, string> format)
        {
            this.Title = title;
            this._format = format;
        }
        private Func<Color, string> _format;
        public string Title { get; }
        [ObservableProperty]
        private string _code;
        public void Read(Color color) => this.Code = _format(color);
        [RelayCommand]
        public void Copy() => Services.ClipboardService.CopyText(this.Code);
    }

    public ColorConverterViewModel()
    {
        Items = new List<CodeItem>()
        {
            new CodeItem("HEX",c=> "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2")),
            new CodeItem("RGB",c=>$"{c.R},{c.G},{c.B}"),
            new CodeItem("ARGB",c=>$"{c.A},{c.R},{c.G},{c.B}"),
            new CodeItem("CSS RGB",c=>$"rgb({c.R},{c.G},{c.B})"),
            new CodeItem("CSS ARGB",c=>$"argb({c.A},{c.R},{c.G},{c.B})"),
            new CodeItem("C# RGB",c=>$"Color.FromRgb({c.R},{c.G},{c.B});"),
            new CodeItem("C# ARGB",c=>$"Color.FromArgb({c.A},{c.R},{c.G},{c.B});"),
            new CodeItem("C# Brush RGB",c=>$"new SolidColorBrush(Color.FromRgb({c.R},{c.G},{c.B}));"),
            new CodeItem("C# Brush ARGB",c=>$"new SolidColorBrush(Color.FromArgb({c.A},{c.R},{c.G},{c.B}));"),
        };
        if (!string.IsNullOrWhiteSpace(Input)) this.OnInputChanged(this.Input);
    }

    [ViewStates.WatchViewState(nameof(Input))]
    [ObservableProperty]
    private string _input;
    [ObservableProperty]
    private string _fail;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Hex))]
    private Color _color;
    [ObservableProperty]
    private bool _done = false;
    public string Hex => GetHex();
    private string GetHex()
    {
        if (_color.IsEmpty) return "Transparent";
        var c = _color;
        return "#" + c.A.ToString("X2") + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
    }

    public IReadOnlyCollection<CodeItem> Items { get; }

    partial void OnInputChanged(string value)
    {
        this.Done = false;
        this.Fail = String.Empty;
        this.Color = Color.Empty;
        try
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Trim();
                if (RgbColorRegex.IsMatch(value))
                {
                    value = RgbColorRegex.Replace(value, "$1,$2,$3");
                }
                else if (ArgbColorRegex.IsMatch(value))
                {
                    value = ArgbColorRegex.Replace(value, "$1,$2,$3,$4");
                }
                else if (value.Contains(" "))
                {
                    value = value.Replace(" ", ",");
                }
                var ret = converter.ConvertFromString(value);
                if (ret == null) throw new Exception();
                this.Color = (Color)ret;
                this.Done = true;
                foreach (var item in this.Items) item.Read(this.Color);
            }
        }
        catch (Exception)
        {
            this.Fail = "请输入 16 进制或 RGB 颜色值";
        }
    }
}
