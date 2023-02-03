using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.Services;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XCode.Code;
using XCode.DataAccessLayer;

namespace LuYao.Toolkit.Channels.Gens;

public partial class GenXCodeEntityViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _fixModelFile = true;
    [ObservableProperty]
    private bool _chineseFileName = false;
    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(XmlFileName))]
    private string _xmlFileName;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(AutoFillDescription))]
    private bool _autoFillDescription = false;
    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(AutoFillToChinese))]
    private bool _autoFillToChinese = false;

    [ObservableProperty]
    private IList<IDataTable> _tables;
    [ObservableProperty]
    private string _output;

    [RelayCommand]
    private async Task Gen()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(this.XmlFileName)) throw new Exception("XML 文件不能为空");
            if (!File.Exists(this.XmlFileName)) throw new Exception("XML 文件不存在");
            var sb = new StringBuilder();
            using (CreateLogger(sb))
            {
                EntityBuilder.Debug = true;
                ClassBuilder.Debug = true;
                PathHelper.BasePath = Path.GetDirectoryName(this.XmlFileName);
                var options = new BuilderOption { };
                var tables = ClassBuilder.LoadModels(this.XmlFileName, options, out var attrs);
                if (this.AutoFillDescription)
                {
                    foreach (var table in tables)
                    {
                        var cols = table.Columns.Where(i => String.IsNullOrWhiteSpace(i.Description)).ToList();
                        if (cols.Count <= 0) continue;
                        if (this.AutoFillToChinese)
                        {
                            var names = cols.Select(i => i.Name).ToList();
                            var values = await GoogleService.Translate("auto", "zh-cn", names);
                            for (int f = 0; f < values.Lines.Count; f++)
                            {
                                cols[f].Description = values.Lines[f];
                            }
                        }
                        else
                        {
                            foreach (var col in cols)
                            {
                                col.Description = col.Name;
                            }
                        }
                    }
                }
                if (this.FixModelFile) EntityBuilder.FixModelFile(this.XmlFileName, options, attrs, tables);
                if (string.IsNullOrWhiteSpace(options.Output))
                {
                    options.Output = Path.GetDirectoryName(this.XmlFileName);
                }
                else
                {
                    options.Output = Path.Combine(Path.GetDirectoryName(this.XmlFileName), options.Output);
                }
                var i = EntityBuilder.BuildTables(tables, options, this.ChineseFileName);
                sb.AppendLine($"生成成功，共 {i} 个。");
                this.Tables = tables;
                Output = sb.ToString();
            }
        }
        catch (Exception e)
        {
            Tables = null;
            Output = e.Message;
        }
    }
    private IDisposable CreateLogger(StringBuilder sb)
    {
        var logger = XTrace.Log;
        var next = new CompositeLog(logger, new ActionLog((fmt, arg) =>
        {
            if (arg is { Length: > 0 })
            {
                sb.AppendFormat(fmt, arg);
            }
            else
            {
                sb.Append(fmt);
            }
            sb.AppendLine();
        }));
        XTrace.Log = next;
        return new DisposeAction(() => { XTrace.Log = logger; });
    }
}
