using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jint;
using LuYao.Toolkit.Resources;
using Newtonsoft.Json.Linq;
using System;

namespace LuYao.Toolkit.Channels.Converts;

public partial class JsonToCsvViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _json;

    [ObservableProperty]
    private string _csv;
    [RelayCommand]
    private void Convert()
    {
        if (string.IsNullOrWhiteSpace(this.Json))
        {
            this.Csv = "JSON 不能为空";
            return;
        }
        if (!Format()) return;
        try
        {
            using (var engine = new Engine())
            {
                var js = AppResources.Channels_Converts_JsonToCsv_JS;
                engine.Evaluate(js);
                engine.SetValue("str", this.Json);
                var value = engine.Evaluate("var t = JSON.parse(str);var opts = {transforms:[ json2csv.transforms.flatten()]};var parser = new json2csv.Parser(opts);return parser.parse(t);");
                var output = value.AsString();
                this.Csv = output;
            }
        }
        catch (Exception e)
        {
            this.Csv = e.Message;
        }
    }
    private bool Format()
    {
        try
        {
            var token = JToken.Parse(this.Json);
            this.Json = token.ToString(Newtonsoft.Json.Formatting.Indented);
            return true;
        }
        catch (Exception e)
        {
            this.Csv = "JSON 格式不正确：" + Environment.NewLine + e.Message;
            return false;
        }
    }
    [RelayCommand]
    private void Demo()
    {
        this.Json = AppResources.Channels_Converts_JsonToCsvDemo_JSON;
        this.Format();
    }
    [RelayCommand]
    private void Clear() => this.Json = String.Empty;
    [RelayCommand]
    private void Copy()
    {
        if (string.IsNullOrWhiteSpace(this.Csv)) return;
        Services.ClipboardService.CopyText(this.Csv);
    }
}