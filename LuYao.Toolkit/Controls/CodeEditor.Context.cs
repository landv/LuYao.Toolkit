using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace LuYao.Toolkit.Controls;

public partial class CodeEditor
{
    private class EditorContext
    {
        public Highlighting SyntaxHighlighting { get; }
        public IFoldingStrategy FoldingStrategy { get; }

        public EditorContext(Highlighting syntaxHighlighting)
        {
            SyntaxHighlighting = syntaxHighlighting;
        }
        public EditorContext(Highlighting syntaxHighlighting, IFoldingStrategy foldingStrategy) : this(syntaxHighlighting)
        {
            this.FoldingStrategy = foldingStrategy;
        }
    }
    private interface IFoldingStrategy
    {
        void UpdateFoldings(FoldingManager manager, TextDocument document);
    }

    private class XmlFoldingStrategy : global::ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy, IFoldingStrategy { }

    private class JsonFoldingStrategy : AvalonEdit.JsonFoldingStrategy, IFoldingStrategy { }
}
