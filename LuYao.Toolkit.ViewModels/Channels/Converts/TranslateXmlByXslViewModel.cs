using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using System.Xml;
using NewLife.Log;

namespace LuYao.Toolkit.Channels.Converts;

public partial class TranslateXmlByXslViewModel : ViewModelBase, IDestructible
{
    private IWatchToken _xmlWatchToken = null;
    private IWatchToken _xslWatchToken = null;

    [ObservableProperty]
    private string _xmlPath;
    [ObservableProperty]
    private string _xslPath;
    [ObservableProperty]
    private string _output = "准备就绪";
    [ObservableProperty]
    private DateTime? _lastBuild;


    private void FileWatchToken_Changed(object sender, FileSystemEventArgs e)
    {
        XTrace.WriteLine("自动重载：{0}", e.FullPath);
        Translate();
    }

    partial void OnXmlPathChanged(string value)
    {
        if (this._xmlWatchToken != null) this._xmlWatchToken.Dispose();
        this._xmlWatchToken = WatchFileService.Watch(value);
        this._xmlWatchToken.Changed += this.FileWatchToken_Changed;
        this.Translate();
    }

    partial void OnXslPathChanged(string value)
    {
        if (this._xslWatchToken != null) this._xslWatchToken.Dispose();
        this._xslWatchToken = WatchFileService.Watch(value);
        this._xslWatchToken.Changed += this.FileWatchToken_Changed;
        this.Translate();
    }

    public override void Destroy()
    {
        base.Destroy();
        this._xmlWatchToken?.Dispose();
        this._xslWatchToken?.Dispose();
    }

    [RelayCommand]
    private void Translate()
    {
        if (string.IsNullOrWhiteSpace(XmlPath))
        {
            Output = "请选择 XML 文件";
            return;
        }
        if (string.IsNullOrWhiteSpace(XslPath))
        {
            Output = "请选择 XSL 文件";
            return;
        }
        try
        {
            var xml = FileService.ReadAllText(XmlPath);
            var xsl = new XslCompiledTransform();
            xsl.Load(this.XslPath);
            using (var sr = File.OpenRead(XmlPath))
            {
                using (var reader = XmlReader.Create(sr))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var writer = XmlWriter.Create(ms, new XmlWriterSettings
                        {
                            Indent = true,
                            Encoding = Encoding.UTF8
                        }))
                        {
                            xsl.Transform(reader, writer);
                        }

                        ms.Seek(0, SeekOrigin.Begin);
                        Output = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }
        catch (Exception e)
        {
            Output = e.Message;
        }
        finally
        {
            this.LastBuild = DateTime.Now;
        }
    }
}