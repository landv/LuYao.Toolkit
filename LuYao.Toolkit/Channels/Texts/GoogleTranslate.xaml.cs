using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows;
using System;
using System.Windows.Controls;
using System.IO;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Linq;
using NewLife.Log;
using NewLife;
using LuYao.Toolkit.Services;
using LuYao.Toolkit.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace LuYao.Toolkit.Channels.Texts
{
    /// <summary>
    /// GoogleTranslate.xaml 的交互逻辑
    /// </summary>
    [Views.ViewName(Views.ViewNames.Channels.Texts.GoogleTranslate)]
    public partial class GoogleTranslate : UserControl
    {
        public GoogleTranslate()
        {
            InitializeComponent();
        }
        private void FixHostButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                TongjiService.Tongji(Views.ViewNames.Channels.Texts.GoogleTranslate, new
                {
                    Action = "FixHost"
                });
                this.FixHostButton.IsEnabled = false;
                var ip = FixHost(out var file);
                using (var p = new Process())
                {
                    p.StartInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Normal,
                        CreateNoWindow = false,
                        FileName = "cmd.exe",
                        Arguments = $"/C xcopy /Y /R \"{file}\" \"%WinDir%\\system32\\drivers\\etc\\hosts\"",
                        Verb = "runas",
                        UseShellExecute = true
                    };
                    if (p.Start())
                    {
                        p.WaitForExit();
                        NotifyService.Success($"修复成功，服务器地址为：{ip}。");
                    }
                }
            }
            catch (Exception exception)
            {
                XTrace.WriteException(exception);
                if (exception is Win32Exception)
                {
                    MessageBoxService.Alert("操作失败，请在弹出是否允许时，选择“是”。" + Environment.NewLine + exception.Message);
                }
                else if (exception is UnauthorizedAccessException)
                {
                    var copy = false;
                    if (!string.IsNullOrWhiteSpace(fixedHost))
                    {
                        ClipboardService.CopyText(fixedHost);
                        copy = true;
                    }
                    MessageBoxService.Alert(
                        "操作失败，请以管理员身份启动程序（程序图标上点鼠标右键，选择：以管理员身份运行）。"
                        + Environment.NewLine
                        + "如果依旧无法修复，尝试手动修改 hosts 文件。"
                        + (copy ? "修复内容已经复制到剪切板。" : string.Empty)
                        + Environment.NewLine
                        + exception.Message
                    );
                }
                else
                {
                    MessageBoxService.Alert("操作失败：" + Environment.NewLine + exception.Message);
                }
            }
            finally
            {
                this.FixHostButton.IsEnabled = true;
            }
        }

        private string fixedHost = string.Empty;
        private record HostItem(string Host, string IPAddress);
        private string FixHost(out string file)
        {
            var ip = GoogleService.ResolveServerAddress();

            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "drivers", "etc");
            var fn = Path.Combine(dir, "hosts");
            FileInfo info = new FileInfo(fn);
            var lines = new List<string>();
            if (info.Exists) lines.AddRange(File.ReadAllLines(fn));
            //File.ReadAllLines(fn)
            foreach (var d in new string[] { "translate.google.com", "translate.googleapis.com" })
            {
                var keep = new HostItem(d, ip);
                Keep(lines, keep);
            }
            var output = lines.Join(Environment.NewLine);
            this.OutputTextEditor.Text = fixedHost = output;
            XTrace.WriteLine(output);
            file = TempHelper.GetTempFileName("host.txt");
            File.WriteAllLines(file, lines, Encoding.UTF8);
            return ip;
        }

        private static void Keep(List<string> lines, HostItem keep)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                var str = lines[i];
                if (string.IsNullOrWhiteSpace(str)) continue;//跳过空格
                if (str.Trim().StartsWith("#")) continue;//跳过注释
                var parts = str.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length <= 1) continue;
                var host = parts[1];
                if (string.Equals(host, keep.Host, StringComparison.OrdinalIgnoreCase))
                {
                    lines.RemoveAt(i);
                    i--;
                }
            }
            lines.Add($"{keep.IPAddress}    {keep.Host}");
        }
    }
}
