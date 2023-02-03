using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using LuYao.Toolkit.Behaviors;
using LuYao.Toolkit.Controls.AvalonEdit.Highlighting;
using LuYao.Toolkit.Themes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LuYao.Toolkit.Controls.AvalonEdit;

public static class HighlightingHelper
{
    static HighlightingHelper()
    {
        HighlightingManager.Instance.RegisterHighlighting("Log", new[] { ".log" }, Defines.Log);
    }
    public static void RegisterHighlighting()
    {
        HighlightingManager.Instance.RegisterHighlighting("C#", new[] { ".cs" }, "CSharp-Mode");
        HighlightingManager.Instance.RegisterHighlighting("Json", new[] { ".json" }, "Json-Mode");
        HighlightingManager.Instance.RegisterHighlighting("XML", new[] { ".xml", ".baml" }, "XML-Mode");
    }
    private static void RegisterHighlighting(
        this HighlightingManager manager,
        string name,
        string[] extensions,
        string resourceName)
    {
        switch (ToolkitConfig.Current.Theme)
        {
            case ThemeMode.Light: resourceName += "-Default"; break;
            case ThemeMode.Dark: resourceName += "-Dark"; break;
        }

        resourceName += ".xshd";

        Stream resourceStream = typeof(HighlightingHelper).Assembly
            .GetManifestResourceStream(typeof(Defines), resourceName);

        if (resourceStream != null)
        {
            using (resourceStream)
            using (XmlTextReader reader = new XmlTextReader(resourceStream)) manager.RegisterHighlighting(name, extensions, HighlightingLoader.Load(reader, manager));
        }
    }
}
