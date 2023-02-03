using HandyControl.Data;
using HandyControl.Tools;
using LuYao.Toolkit.Behaviors;
using System;
using System.Windows;

namespace LuYao.Toolkit.Themes;

internal class ThemeManager
{
    public static readonly ThemeManager Current = new ThemeManager();
    public static event EventHandler<ThemeMode> ThemeChanged;
    private static void OnThemeChanged(object sender,ThemeMode theme)
    {
        ThemeChanged?.Invoke(sender, theme);
    }
    private ThemeMode _theme = ThemeMode.Light;
    public ThemeMode Theme
    {
        get { return _theme; }
        set
        {
            if (_theme == value) return;
            _theme = value;
            SkinType skin = SkinType.Default;
            switch (_theme)
            {
                case ThemeMode.Light: skin = SkinType.Default; break;
                case ThemeMode.Dark: skin = SkinType.Dark; break;
            }
            var dics = Application.Current.Resources.MergedDictionaries;
            var skins0 = dics[0];
            skins0.MergedDictionaries.Clear();
            skins0.MergedDictionaries.Add(ResourceHelper.GetSkin(skin));
            skins0.MergedDictionaries.Add(ResourceHelper.GetSkin(typeof(App).Assembly, "Themes", skin));

            var skins1 = dics[1];
            skins1.MergedDictionaries.Clear();
            skins1.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
            });
            skins1.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/LuYao.Toolkit;component/Themes/Theme.xaml")
            });

            foreach (Window win in Application.Current.Windows) win.OnApplyTemplate();
            OnThemeChanged(this, _theme);
        }
    }
}
