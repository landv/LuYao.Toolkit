using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.Toolkit.Channels.Gens;

public partial class GenGuidViewModel : ViewModelBase
{
    [INotifyPropertyChanged]
    public partial class GuidFormat
    {
        public GuidFormat(string name, Func<Guid, string> format)
        {
            this.Name = name;
            this.Formater = format;
        }
        public string Name { get; }
        public Func<Guid, string> Formater { get; }
        [ObservableProperty]
        private bool _isSelected;
    }
    private Guid _guid = Guid.Empty;
    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(SelectedIndex))]
    private int _selectedIndex;
    public List<GuidFormat> Formats { get; }
    [ObservableProperty]
    private string _result;
    public GenGuidViewModel()
    {
        this.Formats = new List<GuidFormat>
        {
            new GuidFormat("小写带横线", x => x.ToString("D").ToLowerInvariant()),
            new GuidFormat("大写带横线", x => x.ToString("D").ToUpperInvariant()),
            new GuidFormat("小写无横线", x => x.ToString("N").ToLowerInvariant()),
            new GuidFormat("大写无横线", x => x.ToString("N").ToUpperInvariant()),
            new GuidFormat("注册表格式", x => x.ToString("B")),
            new GuidFormat("[GUID(\"xxxxxxx-xxxx ... xxxx\")]", x => $"[GUID(\"{x.ToString("D")}\")]"),
            new GuidFormat("<GUID(\"xxxxxxx-xxxx ... xxxx\")>", x => $"<GUID(\"{x.ToString("D")}\")>"),
            new GuidFormat("Guid.Parse(\"xxxxxxx-xxxx ... xxxx\")", x => $"Guid.Parse(\"{x.ToString("D")}\")"),
            new GuidFormat("BASE64",x=>Convert.ToBase64String(x.ToByteArray()))
        };
        if (this.SelectedIndex >= this.Formats.Count)
        {
            this.SelectedIndex = 0;
        }
        this.Formats[this.SelectedIndex].IsSelected = true;
        this.Gen();
    }
    [RelayCommand]
    private void Select(GuidFormat fmt)
    {
        for (int i = 0; i < this.Formats.Count; i++)
        {
            var item = this.Formats[i];
            if (item == fmt)
            {
                item.IsSelected = true;
                this.SelectedIndex = i;
                this.Result = item.Formater(this._guid);
            }
            else
            {
                item.IsSelected = false;
            }
        }
    }
    [RelayCommand]
    private void Gen()
    {
        this._guid = Guid.NewGuid();
        var fmt = this.Formats.Find(i => i.IsSelected) ?? this.Formats[0];
        this.Result = fmt.Formater(this._guid);
    }
    [RelayCommand]
    private void Copy() => Services.ClipboardService.CopyText(this.Result);
}
