using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Net;
using NewLife;
using NewLife.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LuYao.Toolkit.Channels.Networks;

public partial class IPLookupViewModel : ViewModelBase
{
    [ObservableObject]
    public partial class IPLookupProvider
    {
        public IPLookupProvider(string name, Func<string, CancellationToken, Task<string>> handler)
        {
            this.Name = name;
            this.handler = handler;
        }
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string result = string.Empty;
        [ObservableProperty]
        private string _IP = string.Empty;
        private Func<string, CancellationToken, Task<string>> handler;
        public async Task Execute(string ip, CancellationToken cancellationToken)
        {
            this.IP = string.Empty;
            this.Result = string.Empty;
            if (string.IsNullOrWhiteSpace(ip)) return;
            try
            {
                this.Result = "查询中";
                this.IP = ip;
                var result = await this.handler(ip, cancellationToken);
                if (this.IP == ip) Result = result;
            }
            catch (Exception ex)
            {
                if (this.IP == ip) Result = $"查询异常:{ex.Message}";
            }
        }
    }

    public IReadOnlyCollection<IPLookupProvider> Providers { get; }
    public IPLookupViewModel()
    {
        this.Providers = new List<IPLookupProvider>
        {
                new IPLookupProvider("码农很忙",IPLookupPHandler),
                new IPLookupProvider("淘宝", TaoBaoQueryHandler),
                new IPLookupProvider("百度地图", BaiduLbsQueryHandler),
                new IPLookupProvider("腾讯地图", TencentLbsQueryHandler),
                new IPLookupProvider("高德地图", AmapQueryHandler),
                new IPLookupProvider("太平洋电脑网", PcOnlineQueryHandler)
        };
    }


    private string _ipAddress;
    public string IPAddress
    {
        get => this._ipAddress;
        set
        {
            if (SetProperty(ref this._ipAddress, value)) this.QueryCommand.NotifyCanExecuteChanged();
        }
    }
    private CancellationTokenSource CancellationTokenSource;
    [RelayCommand(CanExecute = nameof(CanQuery))]
    private async Task Query()
    {
        if (string.IsNullOrWhiteSpace(this.IPAddress)) return;
        if (this.CancellationTokenSource != null)
        {
            this.CancellationTokenSource.Cancel();
            this.CancellationTokenSource = null;
        }
        using (this.Busy())
        {
            var tasks = new List<Task>();
            this.CancellationTokenSource = new CancellationTokenSource();
            foreach (var provider in this.Providers) tasks.Add(provider.Execute(this.IPAddress, this.CancellationTokenSource.Token));
            await Task.WhenAll(tasks);
        }
    }
    private bool CanQuery()
    {
        if (string.IsNullOrWhiteSpace(this.IPAddress)) return false;
        string[] splitValues = this.IPAddress.Split('.');
        if (splitValues.Length != 4) return false;
        if (!System.Net.IPAddress.TryParse(this.IPAddress, out _)) return false;
        return true;
    }
    private static HttpClient HttpClient = new HttpClient();
    [RelayCommand]
    private async Task GetLocale()
    {
        using (var response = await HttpClient.GetAsync("https://ipv4.gdt.qq.com/get_client_ip"))
        {
            var str = await response.Content.ReadAsStringAsync();
            if (System.Net.IPAddress.TryParse(str, out var ip))
            {
                this.IPAddress = ip.ToString();
                await Query();
            }
        }
    }

    #region 太平洋电脑网

    private async Task<string> PcOnlineQueryHandler(string ip, CancellationToken cancellationToken)
    {
        var url = $"http://whois.pconline.com.cn/ip.jsp?ip={ip}";
        var str = await HttpClient.GetStringAsync(url, cancellationToken);
        return str.Trim();
    }

    #endregion

    #region 淘宝

    public class TaoBaoResponse
    {
        [XmlElement("code")] public int Code { get; set; }

        [XmlElement("data")] public TaoBaoData Data { get; set; }
        [XmlElement("msg")] public string Message { get; set; }

        public class TaoBaoData
        {
            [XmlElement("country")] public string Country { get; set; }

            [XmlElement("country_id")] public string CountryId { get; set; }

            [XmlElement("area")] public string Area { get; set; }

            [XmlElement("area_id")] public string AreaId { get; set; }

            [XmlElement("region")] public string Region { get; set; }

            [XmlElement("region_id")] public string RegionId { get; set; }

            [XmlElement("city")] public string City { get; set; }

            [XmlElement("city_id")] public string CityId { get; set; }

            [XmlElement("county")] public string County { get; set; }

            [XmlElement("county_id")] public string CountyId { get; set; }

            [XmlElement("isp")] public string Isp { get; set; }

            [XmlElement("isp_id")] public string IspId { get; set; }

            [XmlElement("ip")] public string Ip { get; set; }
        }
    }

    private async Task<string> TaoBaoQueryHandler(string ip, CancellationToken cancellationToken)
    {
        var url = $"http://ip.taobao.com/outGetIpInfo?ip={ip}&accessKey=alibaba-inc";
        var json = await HttpClient.GetStringAsync(url, cancellationToken);
        json = json
            .Replace("xx", string.Empty)
            .Replace("XX", string.Empty);

        var r = json.ToJsonEntity<TaoBaoResponse>();
        if (r.Code == 0)
        {
            var data = r.Data;
            var list = new List<string>
                    {
                        data.Country,
                        data.Region,
                        data.City,
                        data.Isp
                    };
            list.RemoveAll(string.IsNullOrWhiteSpace);
            return list.Join(" ");
        }

        return r.Message;
    }

    #endregion

    #region 高德地图

    public class AmapResponse
    {
        [XmlElement("status")] public int Status { get; set; }

        [XmlElement("info")] public string Info { get; set; }

        [XmlElement("infocode")] public string Infocode { get; set; }

        [XmlElement("country")] public string Country { get; set; }

        [XmlElement("province")] public string Province { get; set; }

        [XmlElement("city")] public string City { get; set; }

        [XmlElement("district")] public string District { get; set; }

        [XmlElement("isp")] public string Isp { get; set; }

        [XmlElement("location")] public string Location { get; set; }

        [XmlElement("ip")] public string Ip { get; set; }

        public override string ToString()
        {
            if (Status != 1)
            {
                return Info;
            }

            var list = new List<string>
                    {
                        Country,
                        Province,
                        City,
                        District,
                        Isp
                    };
            list.RemoveAll(string.IsNullOrWhiteSpace);
            return list.Join(" ");
        }
    }

    private async Task<string> AmapQueryHandler(string ip, CancellationToken cancellationToken)
    {
        var key = "0f48c54461148ea1e670b676cbd1700b";
        var url = $"https://restapi.amap.com/v5/ip?key={key}&ip={ip}&type=4";
        var json = await HttpClient.GetStringAsync(url, cancellationToken);
        var obj = json.ToJsonEntity<AmapResponse>();
        return obj.ToString();
    }

    #endregion

    #region 百度地图

    public class BaiduResponse
    {
        [XmlElement("address")] public string Address { get; set; }

        [XmlElement("status")] public int Status { get; set; }

        [XmlElement("message")] public string Message { get; set; }
    }

    private async Task<string> BaiduLbsQueryHandler(string ip, CancellationToken cancellationToken)
    {
        const string ak = "yBaSv2qHR4r540yBDWGPpC1bLZYK17ni";
        var url = $"http://api.map.baidu.com/location/ip?ak={ak}&ip={ip}";
        var json = await HttpClient.GetStringAsync(url, cancellationToken);
        var obj = json.ToJsonEntity<BaiduResponse>();
        if (obj.Status != 0)
        {
            return obj.Message;
        }

        return obj.Address;
    }

    #endregion

    #region 腾讯地图

    public partial class TencentLbsResponse
    {
        public class Location2
        {
            [XmlElement("lat")] public double Lat { get; set; }

            [XmlElement("lng")] public double Lng { get; set; }
        }
    }

    public partial class TencentLbsResponse
    {
        public class AdInfo
        {
            [XmlElement("nation")] public string Nation { get; set; }

            [XmlElement("province")] public string Province { get; set; }

            [XmlElement("city")] public string City { get; set; }

            [XmlElement("district")] public string District { get; set; }

            [XmlElement("adcode")] public int AdCode { get; set; }
        }
    }

    public partial class TencentLbsResponse
    {
        public class Result2
        {
            [XmlElement("ip")] public string Ip { get; set; }

            [XmlElement("location")] public Location2 Location { get; set; }

            [XmlElement("ad_info")] public AdInfo AdInfo { get; set; }
        }
    }

    public partial class TencentLbsResponse
    {
        [XmlElement("status")] public int Status { get; set; }

        [XmlElement("message")] public string Message { get; set; }

        [XmlElement("result")] public Result2 Result { get; set; }

        public override string ToString()
        {
            if (Status != 0)
            {
                return Message;
            }

            var list = new List<string>();
            if (Result != null && Result.AdInfo != null)
            {
                var info = Result.AdInfo;
                list.Add(info.Nation);
                list.Add(info.Province);
                list.Add(info.City);
                list.Add(info.District);
                list.RemoveAll(string.IsNullOrWhiteSpace);
            }

            return list.Join(" ");
        }
    }

    private async Task<string> TencentLbsQueryHandler(string ip, CancellationToken cancellationToken)
    {
        var key = "TAOBZ-YQ3KU-R4NVU-BT4IA-2P2MF-RDBVJ";
        var url = $"https://apis.map.qq.com/ws/location/v1/ip?ip={ip}&key={key}";
        var json = await HttpClient.GetStringAsync(url, cancellationToken);
        var e = json.ToJsonEntity<TencentLbsResponse>();
        return e.ToString();
    }

    #endregion

    private static Task<string> IPLookupPHandler(string ip, CancellationToken cancellationToken)
    {
        return Task.FromResult(IPLookup.Instance.Search(ip));
    }
}
