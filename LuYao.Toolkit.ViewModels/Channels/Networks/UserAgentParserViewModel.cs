using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;
using UAParser;

namespace LuYao.Toolkit.Channels.Networks;

public partial class UserAgentParserViewModel : ViewModelBase
{
    private Parser _parser;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ParseCommand))]
    private string _input;

    [ObservableProperty()]
    private OS _OS;

    [ObservableProperty]
    private Device _device;

    [ObservableProperty]
    private UserAgent _UA;

    [ObservableProperty]
    private string _output;

    private bool CanParse() => !string.IsNullOrWhiteSpace(this.Input);

    [RelayCommand(CanExecute = nameof(CanParse))]
    private void Parse()
    {
        if (_parser == null) _parser = Parser.GetDefault();
        var sb = new StringBuilder();
        var client = _parser.Parse(this.Input);
        this.OS = client.OS;
        this.Device = client.Device;
        this.UA = client.UA;

        sb.AppendLine($"========== {this.Device} ==========");
        sb.AppendLine($"{nameof(Device.IsSpider)}：{this.Device.IsSpider}");
        sb.AppendLine($"{nameof(Device.Brand)}：{this.Device.Brand}");
        sb.AppendLine($"{nameof(Device.Family)}：{this.Device.Family}");
        sb.AppendLine($"{nameof(Device.Model)}：{this.Device.Model}");
        sb.AppendLine();

        sb.AppendLine($"========== {this.OS} ==========");
        sb.AppendLine($"{nameof(OS.Family)}：{this.OS.Family}");
        sb.AppendLine($"{nameof(OS.Major)}：{this.OS.Major}");
        sb.AppendLine($"{nameof(OS.Minor)}：{this.OS.Minor}");
        sb.AppendLine($"{nameof(OS.Patch)}：{this.OS.Patch}");
        sb.AppendLine($"{nameof(OS.PatchMinor)}：{this.OS.PatchMinor}");
        sb.AppendLine();


        sb.AppendLine($"========== {this.UA} ==========");
        sb.AppendLine($"{nameof(UserAgent.Family)}：{this.UA.Family}");
        sb.AppendLine($"{nameof(UserAgent.Major)}：{this.UA.Major}");
        sb.AppendLine($"{nameof(UserAgent.Minor)}：{this.UA.Minor}");
        sb.AppendLine($"{nameof(UserAgent.Patch)}：{this.UA.Patch}");

        this.Output = sb.ToString();
    }
}
