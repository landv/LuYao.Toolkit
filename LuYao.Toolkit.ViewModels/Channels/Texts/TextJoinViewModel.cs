using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LuYao.Toolkit.Channels.Texts;

public partial class TextJoinViewModel : ViewModelBase
{
    /// <summary>
    /// 分割方式
    /// </summary>
    public enum SplitBy
    {
        [Description("换行")]
        NewLine,
        [Description("逗号(,)")]
        Dot,
        [Description("分号(;)")]
        Semicolon,
        [Description("分隔符(|)")]
        Separator,
        [Description("制表符(\\t)")]
        Tab
    }
    /// <summary>
    /// 串联方式
    /// </summary>
    public enum JoinBy
    {
        [Description("换行")]
        NewLine,
        [Description("逗号(,)")]
        Dot,
        [Description("分号(;)")]
        Semicolon,
        [Description("分隔符(|)")]
        Separator,
        [Description("制表符(\\t)")]
        Tab,
        [Description("无")]
        None
    }
    public enum EscapeType
    {
        [Description("不包装")]
        None,
        [Description("包裹 ' 转义为 \\'")]
        斜杠单引号,
        [Description("包裹 ' 转义为 ''")]
        重复单引号,
        [Description("包裹 \" 转义为 \\\"")]
        斜杠双引号,
        [Description("包裹 \" 转义为 \"\"")]
        重复双引号
    }
    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Split))]
    private SplitBy _split = SplitBy.NewLine;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Escape))]
    private EscapeType _escape = EscapeType.None;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Join))]
    private JoinBy _join = JoinBy.NewLine;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Trim))]
    private bool _trim = true;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Distinct))]
    private bool _distinct = true;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Sort))]
    private bool _sort = true;

    [ObservableProperty]
    private string _input;

    [ObservableProperty]
    private string _output;

    [RelayCommand]
    private void Execute()
    {
        this.Output = string.Empty;
        if (string.IsNullOrEmpty(this.Input)) return;

        var split = new char[] { '\n', '\r' };
        switch (this.Split)
        {
            case SplitBy.Dot:
                split = new char[] { ',' };
                break;
            case SplitBy.Semicolon:
                split = new char[] { ';' };
                break;
            case SplitBy.Separator:
                split = new char[] { '|' };
                break;
            case SplitBy.Tab:
                split = new char[] { '\t' };
                break;
            case SplitBy.NewLine:
            default:
                break;
        }

        var separator = string.Empty;
        switch (this.Join)
        {
            case JoinBy.Dot:
                separator = ",";
                break;
            case JoinBy.Semicolon:
                separator = ";";
                break;
            case JoinBy.Separator:
                separator = "|";
                break;
            case JoinBy.Tab:
                separator = "\t";
                break;
            case JoinBy.NewLine:
                separator = Environment.NewLine;
                break;
            case JoinBy.None:
                break;
            default:
                break;
        }

        IEnumerable<string> items = this.Input.Split(split, StringSplitOptions.RemoveEmptyEntries);
        if (this.Trim) { items = items.Select(i => i.Trim()); }
        switch (this.Escape)
        {
            case EscapeType.None:
                break;
            case EscapeType.斜杠单引号:
                items = items.Select(i => $"'{i.Replace("'", "\\'")}'");
                break;
            case EscapeType.重复单引号:
                items = items.Select(i => $"'{i.Replace("'", "''")}'");
                break;
            case EscapeType.斜杠双引号:
                items = items.Select(i => $"\"{i.Replace("\"", "\\\"")}\"");
                break;
            case EscapeType.重复双引号:
                items = items.Select(i => $"\"{i.Replace("\"", "\"\"")}\"");
                break;
        }
        if (this.Distinct) { items = items.Distinct(); }
        if (this.Sort) { items = items.OrderBy(i => i); }

        this.Output = string.Join(separator, items);
    }
}
