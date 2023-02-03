using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jint;
using LuYao.Toolkit.Resources;
using Newtonsoft.Json.Linq;
using System;

namespace LuYao.Toolkit.Channels.Converts;

public partial class JsonToCSharpViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _json;
    [ObservableProperty]
    private string _cSharp;

    [RelayCommand]
    private void Convert()
    {
        if (string.IsNullOrWhiteSpace(this.Json))
        {
            this.CSharp = "JSON 不能为空";
            return;
        }
        if (!Format()) return;
        try
        {
            using (var engine = new Engine())
            {
                var js = AppResources.Channels_Converts_JsonToCSharp_JS;
                engine.Evaluate(js);
                engine.SetValue("str", this.Json);
                var value = engine.Evaluate(@"var t = JSON.parse(str), e = JSON2CSharp.convert(t);e = e.replace(/<br\/>/g, '\n'); return e;");
                var output = value.AsString();
                output = System.Web.HttpUtility.HtmlDecode(output);
                this.CSharp = output;
            }
        }
        catch (Exception e)
        {
            this.CSharp = e.Message;
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
            this.CSharp = "JSON 格式不正确：" + Environment.NewLine + e.Message;
            return false;
        }
    }
    [RelayCommand]
    private void Demo()
    {
        this.Json = AppResources.Channels_Converts_JsonToCSharpDemo_JSON;
        this.Format();
    }
    [RelayCommand]
    private void Clear() => this.Json = String.Empty;
    [RelayCommand]
    private void Copy()
    {
        if (string.IsNullOrWhiteSpace(this.CSharp)) return;
        Services.ClipboardService.CopyText(this.CSharp);
    }
}
