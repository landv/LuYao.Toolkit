using System;

namespace LuYao.Toolkit.Channels;

public partial class Channel
{
    public static FilesChannel Files { get; } = new FilesChannel();
    public class FilesChannel : Channel
    {
        public FunctionItem DetectFileEncodeing { get; }
        public FunctionItem HashFile { get; }

        public FilesChannel() : base(nameof(Files), "文件处理", Icons.File)
        {
            this.DetectFileEncodeing = new FunctionItem(this, Guid.Parse("5AA7E19C47DE41D7BC7FE0C8FD2EF56F"), nameof(DetectFileEncodeing))
            {
                Title = "编码识别",
                Icon = Icons.TextRecognition,
                Description = "文本文件编码识别",
                View = Views.ViewNames.Channels.Files.DetectFileEncodeing,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "BianMaShiBie", "BMSB", "Encoding" },
            };

            this.HashFile = new FunctionItem(this, Guid.Parse("D7CE1BB56ECA4DBA952B2BE96DD8E967"), nameof(HashFile))
            {
                Title = "文件校验",
                Icon = Icons.HandSaw,
                Description = "获取文件校验值",
                View = Views.ViewNames.Channels.Files.HashFile,
                IsNew = true,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "WenJianJiaoYan", "WJJY", "Hash", "MD5", "SHA1", "SHA256", "SHA512" },
            };

            this.Items = new[]
            {
                DetectFileEncodeing,
                HashFile,
            };
        }
    }
}
