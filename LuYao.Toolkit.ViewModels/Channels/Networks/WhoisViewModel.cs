using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Networks;

public partial class WhoisViewModel : ViewModelBase
{

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(QueryCommand))]
    private string _input;
    private bool CanQuery => !string.IsNullOrWhiteSpace(this.Input);




    public ObservableCollection<WhoisInfomation> Items { get; } = new ObservableCollection<WhoisInfomation>();

    [ObservableProperty]
    private WhoisInfomation _current;

    [RelayCommand(CanExecute = nameof(CanQuery))]
    private async Task Query()
    {
        this.Current = null;
        this.Items.Clear();
        var input = this.Input;
        var list = new List<Task<WhoisInfomation>>();
        foreach (var server in AllServers)
        {
            list.Add(Query(server, input));
        }
        await Task.WhenAll(list);
        foreach (var item in list)
        {
            if (item.IsCompletedSuccessfully())
            {
                var server = item.Result;
                if (server != null && !string.IsNullOrWhiteSpace(server.Result))
                {
                    this.Items.Add(item.Result);
                }
            }
        }
        if (this.Items.Count > 0) this.Current = this.Items[0];
    }
    private static readonly string[] AllServers = new string[] {
        "whois.verisign-grs.com",
        "whois.markmonitor.com",
        "whois.sfn.cn"
    };
    private static async Task<string> GetWhoisInformation(string whoisServer, string url)
    {
        StringBuilder stringBuilderResult = new StringBuilder();
        using TcpClient tcpClinetWhois = new TcpClient();
        await tcpClinetWhois.ConnectAsync(whoisServer, 43);
        NetworkStream networkStreamWhois = tcpClinetWhois.GetStream();
        BufferedStream bufferedStreamWhois = new BufferedStream(networkStreamWhois);
        StreamWriter streamWriter = new StreamWriter(bufferedStreamWhois);

        await streamWriter.WriteLineAsync(url);
        await streamWriter.FlushAsync();

        StreamReader streamReaderReceive = new StreamReader(bufferedStreamWhois) { };

        while (!streamReaderReceive.EndOfStream)
        {
            var line = await streamReaderReceive.ReadLineAsync();
            stringBuilderResult.AppendLine(line.Trim());
        }

        return stringBuilderResult.ToString();
    }
    private static async Task<WhoisInfomation> Query(string server, string domain)
    {
        try
        {
            var ret = await GetWhoisInformation(server, domain);
            return new WhoisInfomation { Server = server, Result = ret };
        }
        catch (Exception)
        {
            return null;
        }
    }

    public class WhoisInfomation
    {
        public string Server { get; set; }
        public string Result { get; set; }
    }
}
