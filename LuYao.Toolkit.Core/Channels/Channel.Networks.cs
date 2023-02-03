using System;

namespace LuYao.Toolkit.Channels;

public partial class Channel
{
    public static NetworksChannel Networks { get; } = new NetworksChannel();
    public class NetworksChannel : Channel
    {
        public FunctionItem IPLookup { get; }
        public FunctionItem Ping { get; }
        public FunctionItem Whois { get; }
        public FunctionItem UserAgentParser { get; }
        public FunctionItem UrlAnalyzer { get; }
        public FunctionItem RemoteDesktopManager { get; }
        public FunctionItem TrafficMonitor { get; }
        public FunctionItem HttpProxyChecker { get; }
        public FunctionItem PortProxy { get; }

        public NetworksChannel() : base(nameof(Networks), "网络工具", Icons.Magnet)
        {
            this.IPLookup = new FunctionItem(this, Guid.Parse("AC449F65DF5D49D086604AAC55E3101D"), nameof(IPLookup))
            {
                Title = "IP 查询",
                Icon = Icons.IpNetwork,
                Description = "本工具可以获取当前的IP地址，输入IP可以查询对应的归属地、地理位置信息。可以精确到运营商和国家、省市级别。",
                View = Views.ViewNames.Channels.Networks.IPLookup,
                IsNew = false,
                UseNetwork = true,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "IPChaXun", "IPCX" },
            };

            this.Ping = new FunctionItem(this, Guid.Parse("11561511804445D6948691190488EF7E"), nameof(Ping))
            {
                Title = "Ping 检测",
                Icon = Icons.Speedometer,
                Description = "通过该工具可以 Ping 服务器以检测服务器响应速度。",
                View = Views.ViewNames.Channels.Networks.Ping,
                IsNew = false,
                UseNetwork = true,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "PINGJianCe", "PINGJC" },
            };

            this.Whois = new FunctionItem(this, Guid.Parse("CD05842B0AAF487DA23EBD4CEE84EABC"), nameof(Whois))
            {
                Title = "Whois 信息查询",
                Icon = Icons.CardAccountDetailsOutline,
                Description = "域名注册信息查询",
                View = Views.ViewNames.Channels.Networks.Whois,
                IsNew = false,
                UseNetwork = true,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "WhoisXinXiChaXun", "WhoisXXCX" },
            };

            this.UserAgentParser = new FunctionItem(this, Guid.Parse("6514BB9F8DD34418A5167F6C89473469"), nameof(UserAgentParser))
            {
                Title = "User Agent 解析",
                Icon = Icons.Web,
                Description = "通过UA分析出浏览器名称、浏览器版本号、浏览器渲染引擎、浏览器操作系统等信息。",
                View = Views.ViewNames.Channels.Networks.UserAgentParser,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "UserAgentJieXi", "UserAgentJieXi", "UAJX" },
            };

            this.UrlAnalyzer = new FunctionItem(this, Guid.Parse("2E9F578A754E459F9216A670AEE26DE0"), nameof(UrlAnalyzer))
            {
                Title = "URL 分析器",
                Icon = Icons.Abacus,
                Description = "获取一个 URL 地址的详细信息",
                View = Views.ViewNames.Channels.Networks.UrlAnalyzer,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "URLFenXiQi", "URLFXQ" },
            };

            this.RemoteDesktopManager = new FunctionItem(this, Guid.Parse("0B8C2033F49D4C28A1D488AE256EBD2D"), nameof(RemoteDesktopManager))
            {
                Title = "远程桌面",
                Icon = Icons.RemoteDesktop,
                Description = "简易远程桌面管理工具",
                View = Views.ViewNames.Channels.Networks.RemoteDesktopManager,
                IsNew = false,
                UseNetwork = true,
                IsDocument = false,
                Multiboxing = false,
                Keywords = new string[] { "YuanChengZhuoMian", "YCZM", "RDP", "RDO", "RDM" },
            };

            this.TrafficMonitor = new FunctionItem(this, Guid.Parse("A16B2AD7913146FAB522B5147AE0222E"), nameof(TrafficMonitor))
            {
                Title = "流量监控",
                Icon = Icons.Wan,
                Description = "查看程序的网络流量",
                View = Views.ViewNames.Channels.Networks.TrafficMonitor,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = false,
                Keywords = new string[] { "LiuLiangJianKong", "LLJK" },
            };

            this.HttpProxyChecker = new FunctionItem(this, Guid.Parse("3DC849233B1E4FF1A8ADA09ECB30933B"), nameof(HttpProxyChecker))
            {
                Title = "HTTP 代理检测",
                Icon = Icons.WebCheck,
                Description = "测试 HTTP 代理服务器是否正常工作",
                View = Views.ViewNames.Channels.Networks.HttpProxyChecker,
                IsNew = false,
                UseNetwork = true,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "HTTPDaiLiJianCe", "HTTPDLJC", "HttpProxyChecker" },
            };

            this.PortProxy = new FunctionItem(this, Guid.Parse("D980814A28E94C7E930A3021EDD0D14C"), nameof(PortProxy))
            {
                Title = "端口转发",
                Icon = Icons.Trackpad,
                Description = "端口转发配置工具，基于 netsh 。",
                View = Views.ViewNames.Channels.Networks.PortProxy.Index,
                IsNew = true,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = false,
                Keywords = new string[] { "DuanKouZhuanFa", "DKZF", "PORT", "PROXY", "NETSH" },
            };

            this.Items = new[]
            {
                IPLookup,
                Ping,
                Whois,
                UserAgentParser,
                UrlAnalyzer,
                RemoteDesktopManager,
                TrafficMonitor,
                HttpProxyChecker,
                PortProxy,
            };
        }
    }
}
