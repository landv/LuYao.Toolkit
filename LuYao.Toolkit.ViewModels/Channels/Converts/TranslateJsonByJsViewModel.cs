using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.Services;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using System.Xml;
using Newtonsoft.Json;
using Jint;
using Jint.Native;
using Jint.Native.Object;
using Jint.Native.Date;
using Jint.Native.Array;

namespace LuYao.Toolkit.Channels.Converts;

public partial class TranslateJsonByJsViewModel : ViewModelBase
{
    private IWatchToken _jsonWatchToken = null;
    private IWatchToken _jsWatchToken = null;

    [ObservableProperty]
    private string _jsonPath;
    [ObservableProperty]
    private string _jsPath;
    [ObservableProperty]
    private string _output = "准备就绪";
    [ObservableProperty]
    private DateTime? _lastBuild;
    private void FileWatchToken_Changed(object sender, FileSystemEventArgs e)
    {
        XTrace.WriteLine("自动重载：{0}", e.FullPath);
        Translate();
    }
    partial void OnJsonPathChanged(string value)
    {
        if (this._jsonWatchToken != null) this._jsonWatchToken.Dispose();
        this._jsonWatchToken = WatchFileService.Watch(value);
        this._jsonWatchToken.Changed += this.FileWatchToken_Changed;
        this.Translate();
    }
    partial void OnJsPathChanged(string value)
    {
        if (this._jsWatchToken != null) this._jsWatchToken.Dispose();
        this._jsWatchToken = WatchFileService.Watch(value);
        this._jsWatchToken.Changed += this.FileWatchToken_Changed;
        this.Translate();
    }
    [RelayCommand]
    private void Translate()
    {
        if (string.IsNullOrWhiteSpace(JsPath))
        {
            Output = "请选择 Js 文件";
            return;
        }

        try
        {
            string json = String.Empty;
            if (!string.IsNullOrWhiteSpace(JsonPath)) json = FileService.ReadAllText(JsonPath);
            var js = FileService.ReadAllText(JsPath);
            using (var e = new Engine())
            {
                if (!string.IsNullOrWhiteSpace(json))
                {
                    JsonConvert.DeserializeObject(json);
                    e.Evaluate($"var model = {json};");
                }
                var result = e.Evaluate(js);
                this.Output = BuildOutput(result);
            }
        }
        catch (Exception e)
        {
            Output = e.Message;
            if (e is Jint.Runtime.JavaScriptException js) Output += Environment.NewLine + js.JavaScriptStackTrace;
        }
        finally
        {
            this.LastBuild = DateTime.Now;
        }
    }
    private string BuildOutput(JsValue value)
    {
        if (value.IsObject())
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var w = new JsonTextWriter(sw)
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };
                Write(value, w);
                return sb.ToString();
            }
        }
        return value.ToString();
    }
    private static void Write(JsValue value, JsonWriter w)
    {
        switch (value)
        {
            case ArrayInstance array:
                w.WriteStartArray();
                foreach (var item in array) Write(item, w);
                w.WriteEndArray();
                break;
            case JsDate date: w.WriteValue(date.ToDateTime()); break;
            case JsNumber number: w.WriteRawValue(number.ToString()); break;
            case JsBigInt bigInt: w.WriteValue(bigInt.ToObject()); break;
            case JsBoolean boolean: w.WriteValue(boolean.ToObject()); break;
            case ObjectInstance instance:
                w.WriteStartObject();
                foreach (var item in instance.GetOwnProperties())
                {
                    w.WritePropertyName(item.Key.ToString());
                    Write(item.Value.Value, w); ;
                }
                w.WriteEndObject();
                break;
            case JsString str: w.WriteValue(str.ToString()); break;
            default: w.WriteRawValue(value.ToString()); break;
        }
    }
    public override void Destroy()
    {
        base.Destroy();
        this._jsonWatchToken?.Dispose();
        this._jsWatchToken?.Dispose();
    }
}
