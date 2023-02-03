using System.Windows.Input;

namespace LuYao.Toolkit;

public static class LuYaoCommands
{
    static LuYaoCommands()
    {
        CloseDetailCommand = new RoutedCommand(nameof(CloseDetailCommand), typeof(LuYaoCommands));
    }
    public static RoutedCommand CloseDetailCommand { get; private set; }
}
