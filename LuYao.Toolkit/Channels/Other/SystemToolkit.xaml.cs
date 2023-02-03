using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LuYao.Toolkit.Channels.Other;

/// <summary>
/// SystemToolkit.xaml 的交互逻辑
/// </summary>
[Views.ViewName(Views.ViewNames.Channels.Other.SystemToolkit)]
public partial class SystemToolkit : UserControl
{
    public SystemToolkit()
    {
        InitializeComponent();
        //https://github.com/BlackINT3/OpenArk/blob/cf3d548b962f0f5cfb764c626f0c71b37f1ac06b/src/OpenArk/utilities/utilities.cpp
        this.AddHandler(Button.ClickEvent, new RoutedEventHandler(this.OnButtonClick));
    }


    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is Button btn)
        {
            switch (btn.Name)
            {
                case nameof(Cmd): ShellRun("cmd.exe", "/k cd /d %userprofile%"); break;
                case nameof(Wsl): ShellRun("wsl.exe", ""); break;
                case nameof(PowerShell): ShellRun("powershell.exe", ""); break;
                case nameof(taskmgr): ShellRun("taskmgr.exe", ""); break;
                case nameof(regedit): ShellRun("regedit.exe", ""); break;
                case nameof(services): ShellRun("services.msc", ""); break;
                case nameof(pcname): ShellRun("SystemPropertiesComputerName.exe", ""); break;
                case nameof(env): ShellRun("SystemPropertiesAdvanced.exe", ""); break;
                case nameof(program): ShellRun("control.exe", "appwiz.cpl"); break;
                case nameof(calc): ShellRun("calc.exe", ""); break;
                case nameof(systeminfo): ShellRun("cmd.exe", "/c systeminfo |more & pause"); break;
                case nameof(version): ShellRun("winver.exe", ""); break;
                case nameof(deskicon): ShellRun("rundll32.exe", "shell32.dll,Control_RunDLL desk.cpl,,0"); break;
                case nameof(tasksch): ShellRun("taskschd.msc", "/s"); break;
                case nameof(devmgr): ShellRun("devmgmt.msc", ""); break;
                case nameof(disks): ShellRun("diskmgmt.msc", ""); break;
                case nameof(datetime): ShellRun("control.exe", "date/time"); break;
                case nameof(wallpaper):
                    if (Environment.OSVersion.Version.Major <= 5)
                    {
                        ShellRun("rundll32.exe", "shell32.dll,Control_RunDLL desk.cpl,,0");
                    }
                    else
                    {
                        ShellRun("control.exe", "/name Microsoft.Personalization /page pageWallpaper");
                    }
                    break;
                case nameof(credential): ShellRun("control.exe", "/name Microsoft.CredentialManager"); break;
                case nameof(uac): ShellRun("UserAccountControlSettings.exe", ""); break;
                case nameof(users): ShellRun("lusrmgr.msc", ""); break;
                case nameof(secpolicy): ShellRun("secpol.msc", ""); break;
                case nameof(gpedit): ShellRun("gpedit.msc", ""); break;
                case nameof(eventlog): ShellRun("eventvwr.msc", ""); break;
                case nameof(performance): ShellRun("perfmon.exe", ""); break;
                case nameof(perfsettings): ShellRun("SystemPropertiesPerformance.exe", ""); break;
                case nameof(resmon): ShellRun("resmon.exe", ""); break;
                case nameof(powercfg): ShellRun("control.exe", "powercfg.cpl,,3"); break;
                case nameof(certmgr): ShellRun("certmgr.msc", ""); break;
                case nameof(hosts): ShellRun("notepad.exe", System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "drivers", "etc", "hosts")); break;
                case nameof(proxy): ShellRun("rundll32.exe", "shell32.dll,Control_RunDLL inetcpl.cpl,,4"); break;
                case nameof(firewall): ShellRun("control.exe", "firewall.cpl"); break;
                case nameof(ipv6): ShellRun("cmd.exe", "/k ipconfig|findstr /i ipv6"); break;
                case nameof(ipv4): ShellRun("cmd.exe", "/k ipconfig|findstr /i ipv4"); break;
                case nameof(route): ShellRun("cmd.exe", "/k route print"); break;
                case nameof(netconnections): ShellRun("control.exe", "ncpa.cpl"); break;
                case nameof(share): ShellRun("fsmgmt.msc", ""); break;
            }
        }
    }
    private static void ShellRun(string cmdline, string param)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.CreateNoWindow = false;
        startInfo.UseShellExecute = true;
        startInfo.FileName = cmdline;
        startInfo.Arguments = param;

        Process.Start(startInfo);
    }
}
