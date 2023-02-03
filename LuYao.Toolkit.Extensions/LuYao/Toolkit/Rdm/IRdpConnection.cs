using System;

namespace LuYao.Toolkit.Rdm;

public interface IRdpConnection
{
    string Name { get; }
    //常规
    String Host { get; }
    Int32 Port { get; }
    String Domain { get; }
    String Username { get; }
    String Password { get; }
    Boolean ConnectToConsole { get; }

    //显示
    Int32 DisplayWidth { get; }
    Int32 DisplayHeight { get; }
    Boolean AutoExpand { get; }
    Boolean SmartSizing { get; }
    ColorDepth ColorDepth { get; }

    //本地资源
    AudioRedirection AudioSetting { get; }
    KeyboardRedirection KeyboardSetting { get; }
    Boolean RedirectDisks { get; }
    Boolean RedirectPorts { get; }
    Boolean RedirectPrinters { get; }
    Boolean RedirectSmartCards { get; }

    //体验
    Boolean BitmapCaching { get; }
    Boolean AllowWallpaper { get; }
    Boolean AllowThemes { get; }
    Boolean AllowContents { get; }
    Boolean AllowAnimation { get; }

    //高级
    AuthenticationLevel AuthenticationLevel { get; }
    Boolean EnableCredSspSupport { get; }
}
