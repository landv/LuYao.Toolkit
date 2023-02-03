using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace LuYao.Toolkit.Services;

public static class GoogleService
{
    private static string[] GoogleIPAddress = new string[] { "108.177.122.90", "142.250.0.90", "142.250.10.90", "142.250.100.90", "142.250.101.90", "142.250.105.90", "142.250.107.90", "142.250.11.90", "142.250.110.90", "142.250.111.90", "142.250.112.90", "142.250.12.90", "142.250.125.90", "142.250.126.90", "142.250.128.90", "142.250.136.90", "142.250.185.174", "142.250.185.238", "142.250.189.206", "142.250.203.142", "142.250.218.14", "142.250.27.90", "142.250.28.90", "142.250.30.90", "142.250.31.90", "142.250.4.90", "142.250.8.90", "142.250.9.90", "142.250.96.90", "142.250.97.90", "142.250.98.90", "142.251.10.138", "142.251.116.101", "142.251.40.174", "142.251.5.90", "142.251.9.90", "172.217.0.46", "172.217.13.142", "172.217.16.46", "172.217.192.90", "172.217.195.90", "172.217.203.90", "172.217.204.90", "172.217.214.90", "172.217.215.90", "172.217.222.90", "172.217.31.142", "172.253.112.90", "172.253.114.90", "172.253.115.90", "172.253.116.90", "172.253.122.90", "172.253.123.90", "172.253.124.90", "172.253.126.90", "172.253.62.90", "216.58.209.174", "216.58.214.14", "216.58.220.142" };

    public static string ResolveServerAddress()
    {
        var tasks = GoogleIPAddress.Select(str => Task.Run(() => Ping(str))).ToList();
        while (tasks.Count > 0)
        {
            var idx = Task.WaitAny(tasks.ToArray());
            if (idx < 0) throw new Exception("解析 IP 地址没有成功");
            var first = tasks[idx];
            tasks.RemoveAt(idx);
            if (first.IsCompletedSuccessfully())
            {
                var ret = first.Result;
                if (!string.IsNullOrWhiteSpace(ret)) return ret;
            }
        }
        //有些电脑会禁止 ping ，所以在 ping 全部失效时，使用随机地址。
        return GoogleIPAddress[Random.Shared.Next(GoogleIPAddress.Length)];
    }

    private static async Task<string> Ping(string ip)
    {
        var options = new PingOptions { DontFragment = true };
        const string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        const int timeout = 1000 * 10;
        using (var sender = new Ping())
        {
            var reply = await sender.SendPingAsync(ip, timeout, buffer, options);
            if (reply.Status == IPStatus.Success) return ip;
            return string.Empty;
        }
    }

    private static IPAddress RemoteAddress = null;

    public static void RefreshRemoteAddress()
    {
        var ip = ResolveServerAddress();
        if (string.IsNullOrWhiteSpace(ip)) throw new Exception("解析 IP 地址没有成功");
        RemoteAddress = IPAddress.Parse(ip);
    }

    public static HttpClient CreateHttpClient()
    {
        if (RemoteAddress == null) RefreshRemoteAddress();
        var handler = new SocketsHttpHandler
        {
            ConnectCallback = ConnectCallback,
            UseProxy = false
        };
        return new HttpClient(handler, true);
    }

    private static async ValueTask<Stream> ConnectCallback(SocketsHttpConnectionContext context, CancellationToken cancellationToken)
    {
        Socket s = new Socket(SocketType.Stream, ProtocolType.Tcp);
        try
        {
            await s.ConnectAsync(new IPEndPoint(RemoteAddress, context.DnsEndPoint.Port), cancellationToken);
            return new NetworkStream(s, true);
        }
        catch
        {
            s.Dispose();
            throw;
        }
    }
    public class TranslateResult
    {
        public string SourceLanguage { get; set; } = string.Empty;
        public List<string> Lines { get; } = new List<string>();
    }

    private const string Endpoint = "https://translate.googleapis.com/translate_a/t";
    private const string Query = "anno=3&client=te_lib&format=html&v=1.0&key=AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw&logld=vTE_20220717&tc=1&sr=1&mode=1";
    private static readonly Regex RegexBlock = new Regex("<b>(.+?)</b>", RegexOptions.Compiled);
    private static HttpClient DefaultHttpClient;
    private static object Locker = new object();
    public static async Task<TranslateResult> Translate(string from, string to, IReadOnlyList<string> lines)
    {
        if (DefaultHttpClient == null)
        {
            lock (Locker)
            {
                if (DefaultHttpClient == null)
                {
                    DefaultHttpClient = CreateHttpClient();
                }
            }
        }
        return await Translate(DefaultHttpClient, from, to, lines);
    }
    public static async Task<TranslateResult> Translate(HttpClient http, string from, string to, IReadOnlyList<string> lines)
    {
        var ret = new TranslateResult();
        var builder = new UriBuilder(Endpoint) { Port = -1 };
        var qs = HttpUtility.ParseQueryString(Query);
        var data = string.Join(string.Empty, lines);
        var tk = GetTK(data);
        qs["tk"] = tk;
        qs["sl"] = from;
        qs["tl"] = to;
        builder.Query = qs.ToString();
        var doc = new HtmlAgilityPack.HtmlDocument();
        using (var request = new HttpRequestMessage(HttpMethod.Post, builder.Uri.ToString()))
        {
            using (var post = new FormUrlEncodedContent(lines.Select(i => new KeyValuePair<string, string>("q", i))))
            {
                request.Content = post;
                using (var response = await http.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();
                    //var ret = new List<string>();
                    var items = JArray.Parse(json);
                    foreach (var item in items)
                    {
                        var line = string.Empty;
                        if (item.Type == JTokenType.String)
                        {
                            line = item.Value<string>();
                        }
                        else if (item.Type == JTokenType.Array)
                        {
                            line = item.Value<string>(0);
                            if (string.IsNullOrWhiteSpace(ret.SourceLanguage))
                            {
                                var lang = item.Value<string>(1);
                                if (!string.IsNullOrWhiteSpace(lang)) ret.SourceLanguage = lang;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            //去掉原文对照
                            var matches = RegexBlock.Matches(line);
                            if (matches.Count > 0) line = string.Join(string.Empty, matches.Cast<Match>().Select(f => f.Groups[1].Value));
                        }

                        if (line.StartsWith("<"))
                        {
                            doc.LoadHtml(line);
                            line = doc.DocumentNode.InnerText;
                        }
                        else
                        {
                            line = HttpUtility.HtmlDecode(line);
                        }

                        ret.Lines.Add(line);
                    }
                    return ret;
                }
            }
        }
    }
    private static (int First, int Last) Key = (460609, 461153391);
    private static long Mask(long a, string b)
    {
        for (int i = 0; i < b.Length - 2; i += 3)
        {
            int c = b[i + 2];
            int d = 'a' <= c ? (int)c - 87 : (int)c - 48;
            d = '+' == b[i + 1] ? (int)((uint)a >> d) : (int)(a << d);
            a = '+' == b[i] ? a + d & 4294967295 : a ^ d;
        }
        return a;
    }
    private static string GetTK(string s)
    {
        SortedDictionary<int, int> d = new SortedDictionary<int, int>();
        int e = 0;
        for (int f = 0; f < s.Length; f++)
        {
            int g = s[f];
            if (128 > g)
            {
                d[e++] = g;
            }
            else
            {
                if (2048 > g)
                {
                    d[e++] = g >> 6 | 192;
                    d[e++] = g & 63 | 128;
                }
                else if (55296 == (g & 64512) && f + 1 < s.Length && 56320 == (s[f + 1] & 64512))
                {
                    g = 65536 + ((g & 1023) << 10) + (s[++f] & 1023);
                    d[e++] = g >> 18 | 240;
                    d[e++] = g >> 12 & 63 | 128;
                    d[e++] = g >> 6 & 63 | 128;
                    d[e++] = g & 63 | 128;
                }
                else
                {
                    d[e++] = g >> 12 | 224;
                    d[e++] = g >> 6 & 63 | 128;
                    d[e++] = g & 63 | 128;
                }
            }
        }

        long a = Key.First;
        for (e = 0; e < d.Count; e++)
        {
            a += d[e];
            a = Mask(a, "+-a^+6");
        }
        a = Mask(a, "+-3^+b+-f");
        a ^= Key.Last;
        if (0 > a) { a = (a & 2147483647) + 2147483648; }
        var z = a % 1000000;
        return $"{z}.{z ^ Key.First}";
    }
}
