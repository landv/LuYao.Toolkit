using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Converts;

public partial class RsaKeyConvertViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _privateKey;
    [ObservableProperty]
    private string _output;

    private void Do(Func<RSA, String> convert)
    {
        try
        {
            using (var rsa = CreateRsa())
            {
                Output = convert(rsa);
            }
        }
        catch (Exception e)
        {
            Output = e.Message;
        }
    }
    [RelayCommand]
    private void ToPkcs8() => Do(static rsa => Convert.ToBase64String(rsa.ExportPkcs8PrivateKey()));
    [RelayCommand]
    private void ToPkcs1() => Do(static rsa => Convert.ToBase64String(rsa.ExportRSAPrivateKey()));
    [RelayCommand]
    private void ToXml() => Do(static rsa => rsa.ToXmlString(true));
    [RelayCommand]
    private void GetPublickKey() => Do(static rsa => Convert.ToBase64String(rsa.ExportRSAPublicKey()));
    [RelayCommand]
    private void GetPublickKeyXml() => Do(static rsa => rsa.ToXmlString(false));
    [RelayCommand]
    private void CopyOutput() => Services.ClipboardService.CopyText(Output);
    private RSA CreateRsa()
    {
        if (string.IsNullOrWhiteSpace(this.PrivateKey)) throw new Exception("私钥不能为空");
        //<RSAKeyValue>
        var rsa = RSA.Create();
        if (this.PrivateKey.StartsWith("<RSAKeyValue>"))
        {
            rsa.FromXmlString(this.PrivateKey);
        }
        else
        {
            var key = this.PrivateKey;
            if (Regex.IsMatch(key, @"^[-]+BEGIN"))
            {
                key = Regex.Match(key, @"(?<=KEY[-]+[\r\n]+)[\s\S]+(?=[\r\n]+[-]+END)").Value;
                key = Regex.Replace(key, @"[\r\n]+", string.Empty);
            }
            var bytes = Convert.FromBase64String(key);
            if (bytes.Length < 8) throw new Exception("私钥长度过短");
            switch (bytes[7])
            {
                case 0x30://PKCS8
                    rsa.ImportPkcs8PrivateKey(bytes, out _);
                    break;
                case 0x02://PKCS1
                    rsa.ImportRSAPrivateKey(bytes, out _);
                    break;
                default: throw new Exception("仅支持 PKCS#1 和 PKCS#8 格式的 RSA 私钥。");
            }
        }
        return rsa;
    }
}
