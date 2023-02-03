using LuYao.IO.Updating;
using NewLife.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Update;

public class UpdateConfig : Config<UpdateConfig>
{
    public UpdateConfig() => NextCheckUpdate = DateTime.Now;
    public static string Endpoint => "https://luyao.coderbusy.com/update";
    public DateTime NextCheckUpdate { get; set; }
    public string GetDataUrl()
    {
        return $"{Endpoint.Trim('/')}/data.xml?t={DateTimeOffset.Now.ToUnixTimeSeconds()}";
    }
    public async Task<UpdatePackage> GetLastVersion()
    {
        using (var http = new HttpClient())
        {
            var xml = await http.GetStringAsync(GetDataUrl());
            return UpdatePackageHelper.Deserialize(xml);
        }
    }
    public void ResetNextCheckTime()
    {
        NextCheckUpdate = DateTime.Now.AddDays(1);
    }
}
