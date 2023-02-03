using LuYao.Toolkit.Themes;
using NewLife.Xml;
using System;

namespace LuYao.Toolkit;

public class ToolkitConfig : XmlConfig<ToolkitConfig>
{
    public ThemeMode Theme { get; set; } = ThemeMode.Light;
    public bool CheckForUpdatesOnStartup { get; set; } = true;
    public bool CodeEditorShowLineNumbers { get; set; } = true;
    public bool CodeEditorWordWrap { get; set; } = false;

    public override void Save()
    {
        base.Save();
        Saved?.Invoke(null, EventArgs.Empty);
    }

    public static event EventHandler<EventArgs> Saved;
}
