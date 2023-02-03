using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;

namespace LuYao.Toolkit.Channels.Converts;

public partial class PostmanConverterViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UrlToPostmanCommand))]
    [NotifyCanExecuteChangedFor(nameof(PostmanToUrlCommand))]
    [NotifyCanExecuteChangedFor(nameof(JsonToPostmanCommand))]
    [NotifyCanExecuteChangedFor(nameof(PostmanToJsonCommand))]
    private string input;

    [ObservableProperty]
    private string output;

    private bool CanUrlToPostman()
    {
        if (string.IsNullOrWhiteSpace(this.Input)) return false;
        return true;
    }
    private bool CanPostmanToUrl()
    {
        if (string.IsNullOrWhiteSpace(this.Input)) return false;

        return true;
    }
    private bool CanJsonToPostman()
    {
        if (string.IsNullOrWhiteSpace(this.Input)) return false;
        try
        {
            JObject.Parse(this.Input);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    [RelayCommand(CanExecute = nameof(CanUrlToPostman))]
    private void UrlToPostman()
    {
        var qs = System.Web.HttpUtility.ParseQueryString(this.Input ?? string.Empty);
        var sb = new StringBuilder();
        for (int i = 0; i < qs.Count; i++)
        {
            var key = qs.GetKey(i);
            var value = qs.Get(i);
            sb.AppendFormat("{0}:{1}", key, value);
            sb.AppendLine();
        }
        this.Output = sb.ToString();
    }

    [RelayCommand(CanExecute = nameof(CanPostmanToUrl))]
    private void PostmanToUrl()
    {
        var items = (this.Input ?? String.Empty).Split('\n');
        var qs = System.Web.HttpUtility.ParseQueryString(string.Empty);
        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item)) continue;
            if (item.IndexOf(':') < 0) continue;
            var pairs = item.Split(':');
            var key = pairs[0].Trim();
            if (string.IsNullOrWhiteSpace(key)) continue;
            var value = string.Join(':', pairs, 1, pairs.Length - 1).Trim();
            qs.Add(key, value);
        }
        this.Output = qs.ToString();
    }
    [RelayCommand(CanExecute = nameof(CanJsonToPostman))]
    private void JsonToPostman()
    {
        var obj = JObject.Parse(this.Input);
        var sb = new StringBuilder();
        foreach (var p in obj.Properties())
        {
            sb.AppendFormat("{0}:{1}", p.Name, p.Value);
            sb.AppendLine();
        }
        this.Output = sb.ToString();
    }
    private bool CanPostmanToJson()
    {
        if (string.IsNullOrWhiteSpace(this.Input)) return false;
        return true;
    }

    [RelayCommand(CanExecute = nameof(CanPostmanToJson))]
    private void PostmanToJson()
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        using (var json = new JsonTextWriter(sw))
        {
            json.Formatting = Formatting.Indented;
            json.WriteStartObject();
            var items = (this.Input ?? String.Empty).Split('\n');
            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                if (item.IndexOf(':') < 0) continue;
                var pairs = item.Split(':');
                var key = pairs[0].Trim();
                if (string.IsNullOrWhiteSpace(key)) continue;
                var value = string.Join(':', pairs, 1, pairs.Length - 1).Trim();
                json.WritePropertyName(key);
                json.WriteValue(value);
            }
            json.WriteEndObject();
        }
        this.Output = sb.ToString();
    }
    [RelayCommand]
    private void Clear()
    {
        this.Input = string.Empty;
        this.Output = string.Empty;
    }
}
