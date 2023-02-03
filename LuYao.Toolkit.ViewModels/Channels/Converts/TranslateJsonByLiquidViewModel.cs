using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fluid;
using LuYao.Toolkit.Services;
using Microsoft.Extensions.FileProviders;
using NewLife.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;

namespace LuYao.Toolkit.Channels.Converts;

public partial class TranslateJsonByLiquidViewModel : ViewModelBase
{
    private TemplateOptions _templateOptions = new TemplateOptions();
    private FluidParser _fluidParser = new FluidParser();
    private IWatchToken _jsonWatchToken = null;
    private IWatchToken _liquidWatchToken = null;
    [ObservableProperty]
    private string _jsonPath;
    [ObservableProperty]
    private string _liquidPath;
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
    partial void OnLiquidPathChanged(string value)
    {
        if (this._liquidWatchToken != null) this._liquidWatchToken.Dispose();
        this._liquidWatchToken = WatchFileService.Watch(value);
        this._liquidWatchToken.Changed += this.FileWatchToken_Changed;
        this.Translate();
    }
    [RelayCommand]
    private void Translate()
    {
        if (string.IsNullOrWhiteSpace(LiquidPath))
        {
            Output = "请选择 Liquid 文件";
            return;
        }
        var model = new Dictionary<string, object>();
        if (!string.IsNullOrWhiteSpace(this.JsonPath))
        {
            try
            {
                var json = Services.FileService.ReadAllText(this.JsonPath);
                JsonConvert.PopulateObject(json, model);
            }
            catch (Exception e)
            {
                this.Output = $"模型解析失败: {e.Message}";
                return;
            }
        }
        try
        {
            var str = FileService.ReadAllText(this.LiquidPath);
            var template = _fluidParser.Parse(str);
            var dir = Path.GetDirectoryName(this.LiquidPath);
            if (this._templateOptions.FileProvider is PhysicalFileProvider provider)
            {
                if (provider.Root != dir)
                {
                    this._templateOptions.FileProvider = new PhysicalFileProvider(dir);
                }
            }
            else
            {
                this._templateOptions.FileProvider = new PhysicalFileProvider(dir);
            }
            var context = new TemplateContext(model, this._templateOptions, true) { };
            Output = template.Render(context, HtmlEncoder.Default);
        }
        catch (ParseException e)
        {
            Output = e.Message;
        }
        catch (Exception e)
        {
            Output = e.Message;
        }
    }
}
