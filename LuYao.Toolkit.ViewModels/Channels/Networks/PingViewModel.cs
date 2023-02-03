using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Networks;

public partial class PingViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PingCommand))]
    private string _input;


    [ObservableProperty]
    private string _output;
    private bool CanPing => !string.IsNullOrWhiteSpace(this.Input);

    [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanPing))]
    private async Task Ping()
    {
        var options = new PingOptions { DontFragment = true };
        const string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        const int timeout = 1000 * 5;
        var input = this.Input;
        this.Output = String.Empty;
        var sb = new StringBuilder();
        string ip = string.Empty;
        var total = 0;
        var times = new List<long>();
        using (var sender = new Ping())
        {
            for (int i = 0; i < 4; i++)
            {
                if (i > 0) await Task.Delay(TimeSpan.FromSeconds(1));
                try
                {
                    if (string.IsNullOrWhiteSpace(ip))
                    {
                        ip = await GetIPAddress(input);
                        if (ip != input)
                        {
                            sb.AppendLine($"正在 Ping {input} [{ip}] 具有 {buffer.Length} 字节的数据：");
                        }
                        else
                        {
                            sb.AppendLine($"正在 Ping {input} 具有 {buffer.Length} 字节的数据：");
                        }
                    }
                    var reply = await sender.SendPingAsync(ip, timeout, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        sb.AppendLine($"来自 {reply.Address} 的回复： 字节={reply.Buffer.Length} 时间={reply.RoundtripTime}ms TTL={reply.Options.Ttl}");
                        times.Add(reply.RoundtripTime);
                    }
                    else
                    {
                        sb.AppendLine(reply.Status.ToString());
                    }
                }
                catch (Exception e)
                {
                    sb.AppendLine(GetMessage(e));
                    break;
                }
                finally
                {
                    this.Output = sb.ToString();
                    total++;
                }
            }
        }
        if (!string.IsNullOrWhiteSpace(ip))
        {
            sb.AppendLine();
            sb.AppendFormat("{0} 的 Ping 统计信息:", ip);
            sb.AppendLine();
            sb.AppendFormat(
                "    数据包: 已发送 = {0}，已接收 = {1}，丢失 = {2} ({3:0.##%} 丢失)，",
                total,
                times.Count,
                total - times.Count,
                1d * (total - times.Count) / total
             );
            sb.AppendLine();
            if (times.Count > 0)
            {
                sb.AppendLine("往返行程的估计时间(以毫秒为单位):");
                sb.AppendFormat("    最短 = {0}ms，最长 = {1}ms，平均 = {2}ms", times.Min(), times.Max(), times.Average());
                sb.AppendLine();
            }
        }
        this.Output = sb.ToString();
    }
    private async Task<string> GetIPAddress(string host)
    {
        foreach (var item in await Dns.GetHostAddressesAsync(host)) return item.ToString();
        return "127.0.0.1";
    }
    private static string GetMessage(Exception e)
    {
        while (e.InnerException != null) e = e.InnerException;
        return e.Message;
    }
}
