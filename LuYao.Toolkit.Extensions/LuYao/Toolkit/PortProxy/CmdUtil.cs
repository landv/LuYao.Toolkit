using LuYao.Toolkit.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LuYao.Toolkit.PortProxy;

public static class CmdUtil
{
    private static Regex GetRegex(string fromType, string toType)
    {
        return new Regex($@"{fromType}[^:]*:[^\n]+?{toType}[^:]*:\r\n\r\n.+?\r\n--------------- ----------  --------------- ----------\r\n(.+?)\r\n\r\n", RegexOptions.Singleline);
    }

    private static readonly Dictionary<string, Regex> RegexList = new Dictionary<string, Regex>()
    {
        ["ipv4 to ipv4"] = GetRegex("ipv4", "ipv4"),
        ["ipv4 to ipv6"] = GetRegex("ipv4", "ipv6"),
        ["ipv6 to ipv4"] = GetRegex("ipv6", "ipv4"),
        ["ipv6 to ipv6"] = GetRegex("ipv6", "ipv6"),
    };

    private static readonly Regex LineRegex = new(@"^(.*?)\s{1,}(.*?)\s{1,}(.*?)\s{1,}(.*?)$");

    public static PortProxyRule[] GetProxies()
    {
        var output = CmdRunner.Execute("netsh interface portproxy show all");
        var types = new[]
        {
                new ProxyType("ipv4", "ipv4"),
                new ProxyType("ipv4", "ipv6"),
                new ProxyType("ipv6", "ipv4"),
                new ProxyType("ipv6", "ipv6"),
            };

        var list = new List<PortProxyRule>();
        foreach (var type in types)
        {
            var regex = RegexList[$"{type.From} to {type.To}"];
            var settings = output.ExtractFirst(regex);
            var lines = settings?.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            if (lines is not null)
            {
                foreach (var line in lines)
                {
                    if (line.TryResolve(LineRegex, out var parts))
                    {
                        var realListenPort = parts[2].First();
                        var realConnectPort = parts[4].First();

                        _ = int.TryParse(realListenPort, out var listenPort);
                        _ = int.TryParse(realConnectPort, out var connectPort);

                        list.Add(new PortProxyRule
                        {
                            Type = type.Type,
                            ListenOn = parts[1].First(),
                            ListenPort = listenPort.ToString(),
                            ConnectTo = parts[3].First(),
                            ConnectPort = connectPort.ToString(),
                        });
                    }
                }
            }
        }

        return list.ToArray();
    }

    public static string GenAddOrUpdateProxyCommand(PortProxyRule rule)
    {
        return $"netsh interface portproxy add {rule.Type} listenaddress={rule.ListenOn} listenport={rule.ListenPort} connectaddress={rule.ConnectTo} connectport={rule.ConnectPort}";
        //CmdRunner.Run($"netsh interface portproxy add {rule.Type} listenaddress={rule.ListenOn} listenport={rule.ListenPort} connectaddress={rule.ConnectTo} connectport={rule.ConnectPort}");
    }

    public static string GenDeleteProxyCommand(PortProxyRule rule)
    {
        //CmdRunner.Run($"netsh interface portproxy delete {rule.Type} listenaddress={rule.ListenOn} listenport={rule.ListenPort}");
        return $"netsh interface portproxy delete {rule.Type} listenaddress={rule.ListenOn} listenport={rule.ListenPort}";
    }
}
