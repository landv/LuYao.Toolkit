using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace LuYao.Toolkit.Controls.AvalonEdit;

public class TruncateLongLinesElementGenerator : VisualLineElementGenerator
{
    const int maxLength = 256;
    const string ellipsis = "......";
    const int charactersAfterEllipsis = 50;

    public override int GetFirstInterestedOffset(int startOffset)
    {
        DocumentLine line = CurrentContext.VisualLine.LastDocumentLine;
        if (line.Length > maxLength)
        {
            int ellipsisOffset = line.Offset + maxLength - charactersAfterEllipsis - ellipsis.Length;
            if (startOffset <= ellipsisOffset)
                return ellipsisOffset;
        }
        return -1;
    }

    public override VisualLineElement ConstructElement(int offset)
    {
        var fmt = new FormattedTextElement(ellipsis, CurrentContext.VisualLine.LastDocumentLine.EndOffset - offset - charactersAfterEllipsis);
        return fmt;
    }
}
