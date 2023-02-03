using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Encodings;

public partial class StringZipperViewModel : EncodingViewModelBase
{
    public IReadOnlyList<string> Encoders { get; } = new string[] {
        StringZipper.Base16.Identifier,
        StringZipper.Base62.Identifier,
        StringZipper.Base64.Identifier,
        StringZipper.Ascii85.Identifier
    };
    public IReadOnlyCollection<string> Compressors { get; } = new[]
    {
        StringZipper.LzString.Identifier,
        StringZipper.GZip.Identifier,
        StringZipper.Deflate.Identifier,
        StringZipper.Br.Identifier,
    };

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Encoder))]
    private string _encoder = "ascii85";

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Compressor))]
    private string _compressor = "gzip";

    [ObservableProperty]
    private string _report;

    [RelayCommand]
    private void Zip()
    {
        try
        {
            if (!StringZipper.TryGetComponent<StringZipper.ICompressor>(this.Compressor, out var compressor))
                throw new Exception($"压缩算法未找到：{this.Compressor}");

            if (!StringZipper.TryGetComponent<StringZipper.IEncoder>(this.Encoder, out var encoder))
                throw new Exception($"编码格式未找到：{this.Encoder}");

            this.Output = StringZipper.Zip(this.Input, compressor, encoder);
            this.Report = this.GetReport(this.Input, this.Output);
        }
        catch (Exception e)
        {
            this.Output = e.Message;
            this.Report = String.Empty;
        }
    }

    [RelayCommand]
    private void Unzip()
    {
        try
        {
            this.Output = StringZipper.Unzip(this.Input);
            this.Report = this.GetReport(this.Output, this.Input);
        }
        catch (Exception e)
        {
            this.Output = e.Message;
            this.Report = String.Empty;
        }
    }

    protected override void Clear()
    {
        base.Clear();
        this.Report = String.Empty;
    }
    private string GetReport(string org, string zip)
    {
        var orgCount = System.Text.Encoding.UTF8.GetByteCount(org ?? String.Empty);
        var zipCount = System.Text.Encoding.UTF8.GetByteCount(zip ?? String.Empty);
        return $"压缩前:{orgCount} 压缩后:{zipCount} 压缩率:{1d * zipCount / orgCount:0.00%}";
    }
}