using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Youdao;

namespace LuYao.Toolkit.Channels.Texts;

public partial class YoudaoDictionaryViewModel : ViewModelBase
{
    private static HttpClient _http = new HttpClient();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(QueryCommand))]
    private string _input;

    [ObservableProperty]
    private YoudaoWord _result;

    private bool CanQuery() => !string.IsNullOrWhiteSpace(this.Input);

    [RelayCommand(CanExecute = nameof(CanQuery))]
    private async Task Query()
    {
        using (this.Busy())
        {
            if (string.IsNullOrWhiteSpace(this.Input)) throw new ArgumentNullException("英文单词不能为空");
            this.Result = null;
            this.Result = await YoudaoDictionary.QueryAsync(_http, this.Input);
        }
    }

    [RelayCommand]
    private async void Play(YoudaoPhonetic phonetic)
    {
        if (phonetic == null) return;
        if (string.IsNullOrWhiteSpace(phonetic.Source)) return;
        await Services.SoundService.Play(phonetic.Source);
    }
}
