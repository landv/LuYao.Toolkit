using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Security;
using LuYao.Toolkit.ViewStates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LuYao.Toolkit.Channels.Gens;

public partial class GenPasswordViewModel : ViewModelBase
{
    public GenPasswordViewModel() => Gen();

    [ObservableProperty]
    [WatchViewState(nameof(UseUpperCase))]
    private bool useUpperCase = true;
    [ObservableProperty]
    [WatchViewState(nameof(UseNumber))]
    private bool useNumber = true;
    [ObservableProperty]
    [WatchViewState(nameof(UseSymbols))]
    private bool useSymbols = false;
    [ObservableProperty]
    [WatchViewState(nameof(Length))]
    private int length = 10;
    [ObservableProperty]
    [WatchViewState(nameof(EasyToRead))]
    private bool easyToRead = true;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Score))]
    [NotifyCanExecuteChangedFor(nameof(GenCommand))]
    private string result = string.Empty;
    public PasswordScore Score => PasswordAdvisor.CheckStrength(this.result);
    private StringBuilder builder = new StringBuilder();
    private static readonly Random Rnd = new Random();
    private static char[] Fallibility = new char[] { '0', 'o', 'O' };
    [RelayCommand]
    private void Gen()
    {
        builder.Clear();
        var pool = new List<char> { };
        pool.AddRange("abcdefghijklmnopqrstuvwxyz");
        if (useUpperCase) pool.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        if (useNumber) pool.AddRange("0123456789");
        if (useSymbols) pool.AddRange("!#$%&*@^");
        if (easyToRead) pool.RemoveAll(x => Array.IndexOf(Fallibility, x) >= 0);

        for (int i = 0; i < length; i++)
        {
            var next = Rnd.Next(0, pool.Count);
            builder.Append(pool[next]);
        }

        Result = builder.ToString();
    }
    [RelayCommand(CanExecute = nameof(CanCopy))]
    private void Copy() => Services.ClipboardService.CopyText(this.Result);
    private bool CanCopy => !string.IsNullOrWhiteSpace(this.Result);
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        switch (e.PropertyName)
        {
            case nameof(UseUpperCase):
            case nameof(UseNumber):
            case nameof(UseSymbols):
            case nameof(Length):
            case nameof(EasyToRead):
                this.Gen();
                break;
        }
    }
}
