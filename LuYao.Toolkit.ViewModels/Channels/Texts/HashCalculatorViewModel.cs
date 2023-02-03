using CommunityToolkit.Mvvm.ComponentModel;
using NewLife.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Texts;

public partial class HashCalculatorViewModel : ViewModelBase
{
    public interface IHashAlgorithmItem
    {
        string Name { get; }
        void Reload(HashCalculatorViewModel vm);
    }
    public partial class HashAlgorithmItem<T> : ViewModelBase, IHashAlgorithmItem where T : HashAlgorithm
    {
        public string Name { get; private set; }

        [ObservableProperty]
        private string _output = string.Empty;

        private readonly Func<string, T> _factory;
        public HashAlgorithmItem(string name, Func<string, T> factory)
        {
            this.Name = name;
            _factory = factory;
        }
        public HashAlgorithm Create(string password) => _factory.Invoke(password);

        public void Reload(HashCalculatorViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Input))
            {
                this.Output = string.Empty;
                return;
            }
            var bytes = Encoding.GetEncoding(vm.Encode).GetBytes(vm.Input);
            using (var hash = this.Create(vm.Password))
            {
                bytes = hash.ComputeHash(bytes);
            }
            var str = BitConverter.ToString(bytes).Replace("-", string.Empty);
            if (vm.ToUpperCase)
            {
                this.Output = str.ToUpperInvariant();
            }
            else
            {
                this.Output = str.ToLowerInvariant();
            }
        }
    }

    public EncodingInfo[] Encodings { get; }

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Encode))]
    private int _encode = Encoding.UTF8.CodePage;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(ToUpperCase))]
    private bool _toUpperCase = false;

    [ObservableProperty]
    private string _input = string.Empty;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Password))]
    private string _password = string.Empty;

    public List<IHashAlgorithmItem> Items { get; }
    public HashCalculatorViewModel()
    {
        Encodings = Encoding.GetEncodings();

        this.Items = new List<IHashAlgorithmItem>
        {
            new HashAlgorithmItem<MD5>("MD5",(pwd)=>MD5.Create()),
            new HashAlgorithmItem<SHA1>("SHA1",(pwd)=>SHA1.Create()),
            new HashAlgorithmItem<SHA256>("SHA256",(pwd)=>SHA256.Create()),
            new HashAlgorithmItem<SHA512>("SHA512",(pwd)=>SHA512.Create()),
            new HashAlgorithmItem<HMACMD5>("HMAC-MD5",(pwd)=>new HMACMD5(Encoding.UTF8.GetBytes(pwd))),
            new HashAlgorithmItem<HMACSHA1>("HMAC-SHA1",(pwd)=>new HMACSHA1(Encoding.UTF8.GetBytes(pwd))),
            new HashAlgorithmItem<HMACSHA256>("HMAC-SHA256",(pwd)=>new HMACSHA256(Encoding.UTF8.GetBytes(pwd))),
            new HashAlgorithmItem<HMACSHA384>("HMAC-SHA384",(pwd)=>new HMACSHA384(Encoding.UTF8.GetBytes(pwd))),
            new HashAlgorithmItem<HMACSHA512>("HMAC-SHA512",(pwd)=>new HMACSHA512(Encoding.UTF8.GetBytes(pwd))),
        };
    }
    private void Reload()
    {
        foreach (var item in this.Items) item.Reload(this);
    }
    partial void OnEncodeChanged(int value) => Reload();
    partial void OnInputChanged(string value) => Reload();
    partial void OnToUpperCaseChanged(bool value) => this.Reload();
    partial void OnPasswordChanged(string value) => this.Reload();
}
