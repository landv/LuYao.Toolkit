using NewLife.Reflection;
using NewLife.Xml;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Windows.Forms;
using System.Collections.Generic;

namespace LuYao.Toolkit.Services;

public static class TongjiServiceProvider
{
    public class TongjiConfig : XmlConfig<TongjiConfig>
    {
        public int LastVisitTime { get; set; }
        public string Detail { get; set; }
        public CookieContainer GetCookie()
        {
            if (string.IsNullOrWhiteSpace(Detail)) return new CookieContainer();
            var cookie = new CookieContainer();
            var collection = new CookieCollection();
            var list = JsonConvert.DeserializeObject<CookieInfo[]>(this.Detail);
            foreach (var item in list)
            {
                collection.Add(item.ToCookie());
            }
            cookie.Add(collection);
            return cookie;
        }
        public void SetCookie(CookieContainer cookie)
        {
            if (cookie == null) throw new ArgumentNullException(nameof(cookie));
            var collection = cookie.GetAllCookies();
            var json = JsonConvert.SerializeObject(collection.Select(i => new CookieInfo(i)));
            this.Detail = json;
        }
    }
    private static CookieContainer Cookie { get; }
    static TongjiServiceProvider()
    {
        Cookie = TongjiConfig.Current.GetCookie();
        HttpClient = new HttpClient(new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = Cookie
        }, true);
        HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(GetUserAgent());
        HttpClient.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        HttpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("zh-CN,zh;q=0.9,en;q=0.8,fr;q=0.7,pt;q=0.6,so;q=0.5,de;q=0.4,en-US;q=0.3,ko;q=0.2,ja;q=0.1,zh-TW;q=0.1,und;q=0.1,is;q=0.1");

#if DEBUG
        _fail = int.MaxValue;
#endif
    }
    public class CookieInfo
    {
        public CookieInfo() { }
        public CookieInfo(Cookie cookie)
        {
            this.Name = cookie.Name;
            this.Value = cookie.Value;
            this.Path = cookie.Path;
            this.Domain = cookie.Domain;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public string Domain { get; set; }
        public Cookie ToCookie() => new Cookie(this.Name, this.Value, this.Path, this.Domain);
    }
    private static void SaveCookie()
    {
        var cfg = TongjiConfig.Current;
        cfg.SetCookie(Cookie);
        cfg.SaveAsync();
    }
    private static int _fail = 0;
    private static int _lastPageVisitTime = 0;
    private static HttpClient HttpClient { get; }
    private static string GetUserAgent() => $"Mozilla/5.0 (Windows NT {Environment.OSVersion.Version.Major}.{Environment.OSVersion.Version.Minor}; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/{AssemblyX.Entry.Version} Safari/537.36";
    private static string _prev = string.Empty;

    public static async void Tongji(string func)
    {
        if (_fail >= 10) return;
        var link = $"https://luyao.coderbusy.com/app/{func}";
        if (link == _prev) return;
        int currentPageVisitTime = GetSecondsSinceEpoch(DateTime.Now);
        int lastPageVisitTime = _lastPageVisitTime;
        var info = new Info();
        var cfg = TongjiConfig.Current;
        if (cfg.LastVisitTime > 0) info.LastVisitTime = cfg.LastVisitTime;
        cfg.LastVisitTime = currentPageVisitTime;
        _lastPageVisitTime = currentPageVisitTime;
        info.SourceType = GetSourceType(currentPageVisitTime, lastPageVisitTime);
        info.IsNewVisit = (info.SourceType == 4) ? 0 : 1;
        info.Data["u"] = link;
        if (!string.IsNullOrWhiteSpace(_prev)) info.Data["su"] = _prev;
        _prev = link;
        var screen = Screen.PrimaryScreen;
        info.Data["ds"] = $"{screen.Bounds.Width}x{screen.Bounds.Height}";
        info.Data["cl"] = $"{screen.BitsPerPixel}-bit";
        var target = $"http://hm.baidu.com/hm.gif?{info.ToQueryString()}";
        try
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, target))
            {
                request.Headers.Referrer = new Uri(link);
                using (var response = await HttpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using var _ = await response.Content.ReadAsStreamAsync();
                        SaveCookie();
                    }
                }
            }
            _fail = 0;
        }
        catch (Exception)
        {
            System.Threading.Interlocked.Increment(ref _fail);
        }
    }
    #region MyRegion

    private const string Version = "wap-1-0.1";
    private const int VisitDuration = 1800;
    private static String GetRandomNumber()
    {
        Random RandomClass = new Random();
        return RandomClass.Next(0x7fffffff).ToString();
    }

    private static int GetSecondsSinceEpoch(DateTime time)
    {
        return (int)(time - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
    }

    private static int GetSourceType(int currentPageVisitTime, int lastPageVisitTime)
    {
        if (currentPageVisitTime - lastPageVisitTime > VisitDuration)
        {
            return 1;
        }
        else
        {
            return 4;
        }
    }
    private class Info
    {
        public int? LastVisitTime { get; set; }
        public int SourceType { get; set; }
        public int IsNewVisit { get; set; }
        public SortedDictionary<string, string> Data { get; } = new SortedDictionary<string, string>();
        public string ToQueryString()
        {
            var nv = System.Web.HttpUtility.ParseQueryString("si=0d558ebb1f987e8cd4ce83298458d9c6&et=0");
            nv["nv"] = this.IsNewVisit.ToString();
            nv["st"] = this.SourceType.ToString();
            nv["v"] = Version;
            nv["rnd"] = GetRandomNumber();
            if (LastVisitTime != null) nv["lt"] = LastVisitTime.ToString();
            foreach (var item in Data) nv[item.Key] = item.Value;
            return nv.ToString();
        }
    }
    #endregion
}
