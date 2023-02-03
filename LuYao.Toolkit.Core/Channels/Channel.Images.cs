using System;

namespace LuYao.Toolkit.Channels;

public partial class Channel
{
    public static ImagesChannel Images { get; } = new ImagesChannel();
    public class ImagesChannel : Channel
    {
        public FunctionItem ImageToIcon { get; }
        public FunctionItem GifSplitter { get; }
        public FunctionItem ImageToBase64 { get; }
        public FunctionItem Base64ToImage { get; }

        public ImagesChannel() : base(nameof(Images), "图片处理", Icons.Image)
        {
            this.ImageToIcon = new FunctionItem(this, Guid.Parse("5CF336BBB79548F88231ECFCACC0918F"), nameof(ImageToIcon))
            {
                Title = "图片转图标",
                Icon = Icons.Emoticon,
                Description = "将图片转为 Icon 格式",
                View = Views.ViewNames.Channels.Images.ImageToIcon,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "TuPianZhuanTuBiao", "TPZTB" },
            };

            this.GifSplitter = new FunctionItem(this, Guid.Parse("7D335ED73BCA47C9A57E2E08BC760268"), nameof(GifSplitter))
            {
                Title = "Gif 分割",
                Icon = Icons.FileGifBox,
                Description = "分割gif图片，可用于gif图片的打印，制作翻页动画册子。",
                View = Views.ViewNames.Channels.Images.GifSplitter,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "GifFenGe", "GIFFG" },
            };

            this.ImageToBase64 = new FunctionItem(this, Guid.Parse("43FB2B33182A486E9F75533DF81B363E"), nameof(ImageToBase64))
            {
                Title = "图片转 Base64",
                Icon = Icons.FileImageOutline,
                Description = "图片转换成base64编码工具",
                View = Views.ViewNames.Channels.Images.ImageToBase64,
                IsNew = true,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "TuPianZhuanBase64", "TPZBase64", "DataUrl" },
            };

            this.Base64ToImage = new FunctionItem(this, Guid.Parse("653EC3972B5B4697B2F663D7F36497A8"), nameof(Base64ToImage))
            {
                Title = "Base64 转图片",
                Icon = Icons.FileImageOutline,
                Description = "Base64编码图片还原为图片的工具",
                View = Views.ViewNames.Channels.Images.Base64ToImage,
                IsNew = true,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "Base64ZhuanTuPian", "Base64ZTP", "DataUrl" },
            };

            this.Items = new[]
            {
                ImageToIcon,
                GifSplitter,
                ImageToBase64,
                Base64ToImage,
            };
        }
    }
}
