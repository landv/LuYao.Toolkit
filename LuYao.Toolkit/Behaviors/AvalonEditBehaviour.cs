//using ICSharpCode.AvalonEdit;
//using ICSharpCode.AvalonEdit.Document;
//using ICSharpCode.AvalonEdit.Highlighting;
//using ICSharpCode.AvalonEdit.Highlighting.Xshd;
//using ICSharpCode.AvalonEdit.Rendering;
//using LuYao.Toolkit.Controls.AvalonEdit.Highlighting;
//using LuYao.Toolkit.Themes;
//using Microsoft.Xaml.Behaviors;
//using System;
//using System.IO;
//using System.Windows;
//using System.Xml;

//namespace LuYao.Toolkit.Behaviors;

//public class AvalonEditBehaviour : Behavior<TextEditor>
//{
//    private class TruncateLongLines : VisualLineElementGenerator
//    {

//        const int maxLength = 256;
//        const string ellipsis = "......";
//        const int charactersAfterEllipsis = 50;

//        public override int GetFirstInterestedOffset(int startOffset)
//        {
//            DocumentLine line = CurrentContext.VisualLine.LastDocumentLine;
//            if (line.Length > maxLength)
//            {
//                int ellipsisOffset = line.Offset + maxLength - charactersAfterEllipsis - ellipsis.Length;
//                if (startOffset <= ellipsisOffset)
//                    return ellipsisOffset;
//            }
//            return -1;
//        }

//        public override VisualLineElement ConstructElement(int offset)
//        {
//            var fmt = new FormattedTextElement(ellipsis, CurrentContext.VisualLine.LastDocumentLine.EndOffset - offset - charactersAfterEllipsis);
//            return fmt;
//        }
//    }

//    public static readonly DependencyProperty CodeTextProperty =
//        DependencyProperty.Register("CodeText", typeof(string), typeof(AvalonEditBehaviour),
//        new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CodeTextChangedCallback));

//    public string CodeText
//    {
//        get { return (string)GetValue(CodeTextProperty); }
//        set { SetValue(CodeTextProperty, value); }
//    }

//    public Boolean IsTruncateable
//    {
//        get { return (Boolean)GetValue(IsTruncateableProperty); }
//        set { SetValue(IsTruncateableProperty, value); }
//    }

//    // Using a DependencyProperty as the backing store for IsTruncateable.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty IsTruncateableProperty =
//        DependencyProperty.Register("IsTruncateable", typeof(Boolean), typeof(AvalonEditBehaviour), new FrameworkPropertyMetadata(false));



//    public Boolean AutoToEnd
//    {
//        get { return (Boolean)GetValue(AutoToEndProperty); }
//        set { SetValue(AutoToEndProperty, value); }
//    }

//    // Using a DependencyProperty as the backing store for AutoToEnd.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty AutoToEndProperty =
//        DependencyProperty.Register("AutoToEnd", typeof(Boolean), typeof(AvalonEditBehaviour), new PropertyMetadata(false));

//    private static void CodeTextChangedCallback(
//        DependencyObject dependencyObject,
//        DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
//    {
//        var behavior = dependencyObject as AvalonEditBehaviour;
//        if (behavior.AssociatedObject != null)
//        {
//            var editor = behavior.AssociatedObject;
//            if (editor.Document != null)
//            {
//                var caretOffset = editor.CaretOffset;
//                var next = dependencyPropertyChangedEventArgs.NewValue.ToString();
//                if (next != editor.Text)
//                {
//                    editor.Document.Text = next;
//                    if (behavior.AutoToEnd)
//                    {
//                        editor.ScrollToEnd();
//                        if (editor.WordWrap) editor.ScrollToEnd();
//                    }
//                    else
//                    {
//                        if (caretOffset <= editor.Document.Text.Length) editor.CaretOffset = caretOffset;
//                    }
//                }
//            }
//        }
//    }
//    protected override void OnAttached()
//    {
//        base.OnAttached();
//        if (AssociatedObject != null)
//        {
//            var editor = AssociatedObject;
//            editor.Options.EnableHyperlinks = false;
//            editor.Options.EnableEmailHyperlinks = false;
//            if (this.IsTruncateable) editor.TextArea.TextView.ElementGenerators.Add(new TruncateLongLines());
//            editor.TextChanged += AssociatedObjectOnTextChanged;
//            ThemeManager.ThemeChanged += AvalonEditBehaviour_OnThemeChanged;
//        }
//    }

//    private void AvalonEditBehaviour_OnThemeChanged(object sender, ThemeMode e)
//    {
//        if (this.AssociatedObject != null)
//        {
//            var editor = this.AssociatedObject;
//            if (editor != null)
//            {
//                var txt = editor.Text;
//                editor.Clear();
//                var highlighting = editor.SyntaxHighlighting;
//                if (highlighting != null)
//                {
//                    editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(highlighting.Name);
//                }
//                editor.Text = txt;
//            }
//        }
//    }

//    protected override void OnDetaching()
//    {
//        base.OnDetaching();
//        if (AssociatedObject != null)
//        {
//            AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
//            ThemeManager.ThemeChanged -= AvalonEditBehaviour_OnThemeChanged;
//        }
//    }
//    private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
//    {
//        if (sender is TextEditor textEditor)
//        {
//            if (textEditor.Document != null)
//            {
//                CodeText = textEditor.Document.Text;
//            }
//        }
//    }
//    public static void RegisterHighlighting()
//    {
//        HighlightingManager.Instance.RegisterHighlighting("C#", new[] { ".cs" }, "CSharp-Mode");
//        HighlightingManager.Instance.RegisterHighlighting("Json", new[] { ".json" }, "Json-Mode");
//        HighlightingManager.Instance.RegisterHighlighting("XML", new[] { ".xml", ".baml" }, "XML-Mode");
//    }
//}

//static class ExtensionMethods
//{
//    public static void RegisterHighlighting(
//        this HighlightingManager manager,
//        string name,
//        string[] extensions,
//        string resourceName)
//    {
//        switch (ToolkitConfig.Current.Theme)
//        {
//            case ThemeMode.Light: resourceName += "-Default"; break;
//            case ThemeMode.Dark: resourceName += "-Dark"; break;
//        }

//        resourceName += ".xshd";

//        Stream resourceStream = typeof(AvalonEditBehaviour).Assembly
//            .GetManifestResourceStream(typeof(Defines), resourceName);

//        if (resourceStream != null)
//        {
//            using (resourceStream)
//            using (XmlTextReader reader = new XmlTextReader(resourceStream)) manager.RegisterHighlighting(name, extensions, HighlightingLoader.Load(reader, manager));
//        }
//    }
//}