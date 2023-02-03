using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Services.Dialogs;
using System;
using System.IO;
using System.Text;

namespace LuYao.Toolkit.Dialogs;

public partial class PreviewFileAsStringDialogViewModel : ViewModelBase, IDialogAware
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FileName))]
    private Events.PreviewFileAsStringEventPayload _payload;

    public string FileName => Path.GetFileName(this.Payload.FullName);

    [ObservableProperty]
    private string _content;

    public const int Limit = 1000;


    public string Title => "预览文件";

    public event Action<IDialogResult> RequestClose;

    [RelayCommand]
    private void Close()
    {
        this.RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }

    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
    }

    private void ReadConten()
    {
        using (var sr = new StreamReader(this.Payload.FullName, this.Payload.Encoding))
        {
            var sb = new StringBuilder();
            while (!sr.EndOfStream)
            {
                if (sb.Length >= Limit) break;
                var line = sr.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (line.Length <= Limit)
                    {
                        sb.AppendLine(line);
                    }
                    else
                    {
                        sb.Append(line.Substring(0, Limit));
                        sb.Append("...");
                        sb.AppendLine();
                        break;
                    }
                }
                else
                {
                    sb.Append(line);
                }
            }
            this.Content = sb.ToString();
        }
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        if (parameters.TryGetValue<Events.PreviewFileAsStringEventPayload>(nameof(Payload), out var payload))
        {
            this.Payload = payload;
            this.ReadConten();
        }
        else
        {
            throw new Exception("Payload 为空");
        }
    }
}
