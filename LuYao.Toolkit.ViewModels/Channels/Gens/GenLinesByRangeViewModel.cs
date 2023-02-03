using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;

namespace LuYao.Toolkit.Channels.Gens;

public partial class GenLinesByRangeViewModel : ViewModelBase
{
    public GenLinesByRangeViewModel() => this.Execute();
    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Min))]
    private int min = 1000;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Max))]
    private int max = 1024;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Template))]
    private string template = "http://www.baidu.com/s?w=${id}";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CopyCommand))]
    private string output;

    partial void OnMinChanged(int value)
    {
        this.Execute();
    }
    partial void OnMaxChanged(int value)
    {
        this.Execute();
    }
    partial void OnTemplateChanged(string value)
    {
        this.Execute();
    }

    void Execute()
    {
        if (string.IsNullOrWhiteSpace(this.Template))
        {
            this.Output = string.Empty;
            return;
        }
        var sb = new StringBuilder();
        for (int i = this.Min; i <= this.Max; i++)
        {
            sb.Append(this.Template.Replace("${id}", i.ToString()));
            sb.AppendLine();
        }
        this.Output = sb.ToString();
    }
    private bool CanCopy => !string.IsNullOrWhiteSpace(Output);
    [RelayCommand(CanExecute = nameof(CanCopy))]
    private void Copy()
    {
        Services.ClipboardService.CopyText(this.Output);
    }
}
