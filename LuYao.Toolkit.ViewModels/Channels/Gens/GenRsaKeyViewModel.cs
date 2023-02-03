using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace LuYao.Toolkit.Channels.Gens;
public partial class GenRsaKeyViewModel : ViewModelBase
{
    public IReadOnlyCollection<int> KeySizes { get; } = new SortedSet<int> { 512, 1024, 2048, 4096 };

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(KeySize))]
    private int _keySize = 1024;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(KeyFormat))]
    private RsaKeyFormat _keyFormat = RsaKeyFormat.PKCS8;

    [ObservableProperty]
    private string _privateKey;
    [ObservableProperty]
    private string _publicKey;
    [RelayCommand]
    private void Gen()
    {
        using (var rsa = RSA.Create(this.KeySize))
        {
            var publicKey = rsa.ExportRSAPublicKey();
            this.PublicKey = Convert.ToBase64String(publicKey);
            switch (this.KeyFormat)
            {
                case RsaKeyFormat.PKCS8:
                    this.PrivateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());
                    break;
                case RsaKeyFormat.PKCS1:
                    this.PrivateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                    break;
                case RsaKeyFormat.XML:
                    this.PrivateKey = rsa.ToXmlString(true);
                    this.PublicKey = rsa.ToXmlString(false);
                    break;
                default:
                    this.PrivateKey = String.Empty;
                    break;
            }
        }
    }
    [RelayCommand]
    private void Copy(string text)
    {
        Services.ClipboardService.CopyText(text);
    }
}
