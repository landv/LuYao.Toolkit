using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.ViewStates;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Texts;

public partial class RegexEvaluatorViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _pattern = string.Empty;
    [ObservableProperty]
    private string _inputText = string.Empty;

    #region Options
    [ObservableProperty]
    [WatchViewState(nameof(IgnoreCase))]
    private bool _ignoreCase = false;

    [ObservableProperty]
    [WatchViewState(nameof(Multiline))]
    private bool _multiline = false;

    [ObservableProperty]
    [WatchViewState(nameof(ExplicitCapture))]
    private bool _explicitCapture = false;

    [ObservableProperty]
    [WatchViewState(nameof(Compiled))]
    private bool _compiled = false;

    [ObservableProperty]
    [WatchViewState(nameof(Singleline))]
    private bool _singleline = false;

    [ObservableProperty]
    [WatchViewState(nameof(IgnorePatternWhitespace))]
    private bool _ignorePatternWhitespace = false;

    [ObservableProperty]
    [WatchViewState(nameof(RightToLeft))]
    private bool _rightToLeft = false;

    [ObservableProperty]
    [WatchViewState(nameof(ECMAScript))]
    private bool _eCMAScript = false;

    [ObservableProperty]
    [WatchViewState(nameof(CultureInvariant))]
    private bool _cultureInvariant = false;

    #endregion

    [ObservableProperty]
    private MatchCollection _matches;
    [ObservableProperty]
    private string _code;
    [ObservableProperty]
    private Exception _exception;

    [RelayCommand]
    private void Clear()
    {
        this.Pattern = this.InputText = this.Code = string.Empty;
        this.Matches = null;
        this.Exception = null;
    }

    [RelayCommand]
    private void Update()
    {
        var options = RegexOptions.None;
        if (this.IgnoreCase) options |= RegexOptions.IgnoreCase;
        if (this.Multiline) options |= RegexOptions.Multiline;
        if (this.ExplicitCapture) options |= RegexOptions.ExplicitCapture;
        if (this.Compiled) options |= RegexOptions.Compiled;
        if (this.Singleline) options |= RegexOptions.Singleline;
        if (this.IgnorePatternWhitespace) options |= RegexOptions.IgnorePatternWhitespace;
        if (this.RightToLeft) options |= RegexOptions.RightToLeft;
        if (this.ECMAScript) options |= RegexOptions.ECMAScript;
        if (this.CultureInvariant) options |= RegexOptions.CultureInvariant;
        try
        {
            Matches = Regex.Matches(this.InputText, this.Pattern, options);
            Exception = null;
            BuildCode();
        }
        catch (Exception e)
        {
            Matches = null;
            Exception = e;
            Code = string.Empty;
        }
    }

    private void BuildCode()
    {
        var options = new List<string>();
        if (this.IgnoreCase) options.Add("RegexOptions.IgnoreCase");
        if (this.Multiline) options.Add("RegexOptions.Multiline");
        if (this.ExplicitCapture) options.Add("RegexOptions.ExplicitCapture");
        if (this.Compiled) options.Add("RegexOptions.Compiled");
        if (this.Singleline) options.Add("RegexOptions.Singleline");
        if (this.IgnorePatternWhitespace) options.Add("RegexOptions.IgnorePatternWhitespace");
        if (this.RightToLeft) options.Add("RegexOptions.RightToLeft");
        if (this.ECMAScript) options.Add("RegexOptions.ECMAScript");
        if (this.CultureInvariant) options.Add("RegexOptions.CultureInvariant");
        if (options.Count == 0) options.Add("RegexOptions.None");
        var sb = new StringBuilder();
        sb.AppendFormat("var pattern = {0};//表达式文本", JsonConvert.SerializeObject(this.Pattern));
        sb.AppendLine();
        sb.AppendFormat("var regex = new Regex({0},{1});//表达式对象", JsonConvert.SerializeObject(this.Pattern), string.Join("|", options));
        this.Code = sb.ToString();
    }

    partial void OnPatternChanged(string value) => this.Update();
    partial void OnInputTextChanged(string value) => this.Update();
    partial void OnIgnoreCaseChanged(bool value) => this.Update();
    partial void OnMultilineChanged(bool value) => this.Update();
    partial void OnExplicitCaptureChanged(bool value) => this.Update();
    partial void OnCompiledChanged(bool value) => this.Update();
    partial void OnSinglelineChanged(bool value) => this.Update();
    partial void OnIgnorePatternWhitespaceChanged(bool value) => this.Update();
    partial void OnRightToLeftChanged(bool value) => this.Update();
    partial void OnECMAScriptChanged(bool value) => this.Update();
    partial void OnCultureInvariantChanged(bool value) => this.Update();
}
