using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Highlighting;
using System.IO;
using System.Xml;

namespace LuYao.Toolkit.Controls.AvalonEdit.Highlighting;

public static class Defines
{
    static Defines()
    {
        var type = typeof(Defines);
        using (Stream s = type.Assembly.GetManifestResourceStream($"{type.Namespace}.Log.xshd"))
        {
            using (XmlReader reader = new XmlTextReader(s))
            {
                Log = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
        }
    }
    public static IHighlightingDefinition Log { get; }
}
