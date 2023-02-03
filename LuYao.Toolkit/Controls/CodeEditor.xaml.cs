using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Search;
using LuYao.Toolkit.Controls.Utils;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LuYao.Toolkit.Controls
{
    /// <summary>
    /// CodeEditor.xaml 的交互逻辑
    /// </summary>
    public partial class CodeEditor : UserControl
    {
        private SearchPanel _searchPanel = null;
        private EditorContext _editorContext = null;
        private FoldingManager _foldingManager = null;

        public CodeEditor()
        {
            InitializeComponent();
            this._searchPanel = SearchPanel.Install(this.MainEditor);
            this.MainEditor.TextChanged += MainEditor_TextChanged;
            this.InitCommands();
            var cfg = ToolkitConfig.Current;
            this.MainEditor.ShowLineNumbers = cfg.CodeEditorShowLineNumbers;
            this.MainEditor.WordWrap = cfg.CodeEditorWordWrap;

            WeakEventManager<ToolkitConfig, EventArgs>.AddHandler(null, nameof(ToolkitConfig.Saved), this.ToolkitConfig_Saved);
        }

        private void ToolkitConfig_Saved(object sender, EventArgs e)
        {
            SetWordWrap(this.WordWrap);
            SetShowLineNumbers(this.ShowLineNumbers);
        }

        private void MainEditor_TextChanged(object sender, EventArgs e)
        {
            this.Text = this.MainEditor.Text;
            if (_foldingManager == null) return;
            if (_editorContext?.FoldingStrategy != null) _editorContext.FoldingStrategy.UpdateFoldings(_foldingManager, this.MainEditor.Document);
        }


        private Highlighting _syntaxHighlighting;

        public Highlighting SyntaxHighlighting
        {
            get { return _syntaxHighlighting; }
            set
            {
                if (_syntaxHighlighting != value)
                {
                    _syntaxHighlighting = value;
                    this.MainEditor.SyntaxHighlighting = null;
                    this._editorContext = null;
                    if (this._foldingManager != null)
                    {
                        FoldingManager.Uninstall(this._foldingManager);
                        this._foldingManager = null;
                    }
                    switch (value)
                    {
                        case Highlighting.CSharp:
                            this.MainEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
                            this._editorContext = new EditorContext(Highlighting.CSharp);
                            break;
                        case Highlighting.Json:
                            this.MainEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(nameof(Highlighting.Json));
                            this._editorContext = new EditorContext(Highlighting.Json, new JsonFoldingStrategy());
                            break;
                        case Highlighting.Log:
                            this.MainEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(nameof(Highlighting.Log));
                            this._editorContext = new EditorContext(Highlighting.Log);
                            break;
                        case Highlighting.Xml:
                            this.MainEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("XML");
                            this._editorContext = new EditorContext(Highlighting.Xml, new XmlFoldingStrategy());
                            break;
                    }
                    if (this._editorContext != null && this._editorContext.FoldingStrategy != null)
                    {
                        this._foldingManager = FoldingManager.Install(this.MainEditor.TextArea);
                    }
                }
            }
        }


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty
            = DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(CodeEditor),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged)
            );

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var txt = e.NewValue as string;
            if (d is CodeEditor editor && editor.MainEditor.Document != null && editor.MainEditor.Text != txt)
            {
                editor.MainEditor.Document.Text = txt ?? string.Empty;
                if (editor.IsReadOnly && editor.IsAutoToEnd)
                {
                    editor.MainEditor.ScrollToEnd();
                    if (editor.MainEditor.WordWrap) editor.MainEditor.ScrollToEnd();
                }
            }
        }

        public static DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(CodeEditor), new FrameworkPropertyMetadata(Boxes.Box(false), OnIsReadOnlyChanged));

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, Boxes.Box(value));
        }

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CodeEditor editor) editor.MainEditor.IsReadOnly = (bool)e.NewValue;
        }

        public bool? WordWrap
        {
            get { return (bool?)GetValue(WordWrapProperty); }
            set { SetValue(WordWrapProperty, value); }
        }

        public static readonly DependencyProperty WordWrapProperty = DependencyProperty.Register("WordWrap", typeof(bool?), typeof(CodeEditor), new FrameworkPropertyMetadata(null, OnWordWrapChanged));

        private static void OnWordWrapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CodeEditor editor)
            {
                editor.SetWordWrap(e.NewValue as bool?);
            }
        }

        public bool? ShowLineNumbers
        {
            get { return (bool?)GetValue(ShowLineNumbersProperty); }
            set { SetValue(ShowLineNumbersProperty, value); }
        }

        public static readonly DependencyProperty ShowLineNumbersProperty = DependencyProperty.Register("ShowLineNumbers", typeof(bool?), typeof(CodeEditor), new FrameworkPropertyMetadata(null, OnShowLineNumbersChanged));

        private static void OnShowLineNumbersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CodeEditor editor)
            {
                editor.SetShowLineNumbers(e.NewValue as bool?);
            }
        }

        private void SetWordWrap(bool? v)
        {
            if (v != null)
            {
                this.MainEditor.WordWrap = v.Value;
            }
            else
            {
                this.MainEditor.WordWrap = ToolkitConfig.Current.CodeEditorWordWrap;
            }
        }

        private void SetShowLineNumbers(bool? v)
        {
            if (v != null)
            {
                this.MainEditor.ShowLineNumbers = v.Value;
            }
            else
            {
                this.MainEditor.ShowLineNumbers = ToolkitConfig.Current.CodeEditorShowLineNumbers;
            }
        }

        private bool _isAutoToEnd;

        public bool IsAutoToEnd
        {
            get => _isAutoToEnd;
            set => _isAutoToEnd = value;
        }

    }
}
