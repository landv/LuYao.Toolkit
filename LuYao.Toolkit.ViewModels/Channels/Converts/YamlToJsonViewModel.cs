using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using System.IO;
using YamlDotNet.Serialization;

namespace LuYao.Toolkit.Channels.Converts;

public partial class YamlToJsonViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(YamlToJsonCommand))]
    [NotifyCanExecuteChangedFor(nameof(JsonToYamlCommand))]
    private string input;

    [ObservableProperty]
    private string output;

    private bool CanExecute() => !string.IsNullOrWhiteSpace(this.Input);

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private void YamlToJson()
    {
        using var r = new StringReader(this.Input);
        var deserializer = new DeserializerBuilder().Build();
        var yamlObject = deserializer.Deserialize(r);
        this.Output = JsonConvert.SerializeObject(yamlObject, Formatting.Indented);
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private void JsonToYaml()
    {
        var serializer = new SerializerBuilder().Build();
        var dyn = JsonConvert.DeserializeObject<ExpandoObject>(this.Input, new ExpandoObjectConverter());
        this.Output = serializer.Serialize(dyn);
    }

    [RelayCommand]
    private void Clear()
    {
        this.Output = this.Input = string.Empty;
    }

    [RelayCommand]
    private void Copy()
    {
        Services.ClipboardService.CopyText(this.Output);
    }
}
