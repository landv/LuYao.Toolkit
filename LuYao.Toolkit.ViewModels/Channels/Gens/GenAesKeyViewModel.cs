using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LuYao.Toolkit.Channels.Gens;

public partial class GenAesKeyViewModel : ViewModelBase
{
    public GenAesKeyViewModel()
    {
        this.LoadKeys();
        if (this.Keys.Count > 0) Gen(this.Keys.First());
    }

    [ObservableProperty]
    private string _result;
    public IReadOnlyCollection<int> Keys { get; private set; }
    private void LoadKeys()
    {
        var keys = new SortedSet<int>();
        using (var aes = Aes.Create())
        {
            foreach (var size in aes.LegalKeySizes)
            {
                for (int i = size.MinSize; i <= size.MaxSize; i += size.SkipSize)
                {
                    if (keys.Contains(i))
                    {
                        continue;
                    }
                    keys.Add(i);
                }
            }
        }
        Keys = keys;
    }
    [RelayCommand]
    private void Gen(object len) => this.Result = GenerateAesKey(Convert.ToInt32(len));
    [RelayCommand]
    private void Copy()
    {
        Services.ClipboardService.CopyText(this.Result);
    }
    private static string GenerateAesKey(int len)
    {
        using (var aes = Aes.Create())
        {
            aes.KeySize = len;
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);
        }
    }
}
