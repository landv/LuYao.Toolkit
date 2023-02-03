using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;

namespace LuYao.Toolkit.Channels.Networks;

public partial class UrlAnalyzerViewModel : ViewModelBase
{
    public class KeyValueItem
    {
        public KeyValueItem(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
        public string Key { get; }
        public string Value { get; }
    }
    [ObservableProperty]
    private string _url;
    [ObservableProperty]
    private bool _isAbsoluteUri;
    [ObservableProperty]
    private IReadOnlyList<KeyValueItem> _queryString;
    [ObservableProperty]
    private Uri _uri;
    [ObservableProperty]
    private string _output;
    [RelayCommand]
    private void Clear()
    {
        this.Output = string.Empty;
        this.QueryString = default;
        this.IsAbsoluteUri = default;
        this.Uri = default;
    }
    [RelayCommand]
    private void Analyze()
    {
        this.Clear();
        if (string.IsNullOrWhiteSpace(this.Url))
        {
            this.Output = "URL为空";
            return;
        }
        if (Uri.TryCreate(this.Url, UriKind.RelativeOrAbsolute, out var uri))
        {
            IsAbsoluteUri = uri.IsAbsoluteUri;
            if (!this.IsAbsoluteUri)
            {
                this.Output = "仅支持绝对路径";
                return;
            }
            this.Uri = uri;
            if (!string.IsNullOrWhiteSpace(uri.Query))
            {
                var nv = System.Web.HttpUtility.ParseQueryString(uri.Query);
                if (nv.Count > 0)
                {
                    var list = new List<KeyValueItem>();
                    foreach (var key in nv.AllKeys)
                    {
                        foreach (var value in nv.GetValues(key))
                        {
                            list.Add(new KeyValueItem(key, value));
                        }
                    }
                    this.QueryString = list;
                }
            }
        }
        else
        {
            this.Output = "URL解析失败";
        }
    }
}
