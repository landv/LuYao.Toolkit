using System;

namespace LuYao.Toolkit.Channels;

public partial class Channel
{
    public static EncodingsChannel Encodings { get; } = new EncodingsChannel();
    public class EncodingsChannel : Channel
    {
        public FunctionItem StringZipper { get; }
        public FunctionItem UrlEncode { get; }
        public FunctionItem HtmlEncode { get; }
        public FunctionItem Ascii85Encode { get; }
        public FunctionItem Base64Encode { get; }
        public FunctionItem Base62Encode { get; }
        public FunctionItem Base16Encode { get; }

        public EncodingsChannel() : base(nameof(Encodings), "编码互转", Icons.Altimeter)
        {
            this.StringZipper = new FunctionItem(this, Guid.Parse("C20499BE2E874AB193B14E59B97A84B4"), nameof(StringZipper))
            {
                Title = "文本压缩",
                Icon = Icons.Package,
                Description = "将字符串压缩后再进行编码，以减少带宽占用。",
                View = Views.ViewNames.Channels.Encodings.StringZipper,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "WenBenYaSuo", "WBYS" },
            };

            this.UrlEncode = new FunctionItem(this, Guid.Parse("6206D4C38FAC4750848595D22DE565E1"), nameof(UrlEncode))
            {
                Title = "URL 编码",
                Icon = Icons.Web,
                Description = "为了让包含中文的URL可以使用，您可以使用本工具对中文进行UrlEncode编码。",
                View = Views.ViewNames.Channels.Encodings.UrlEncode,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "URLBianMa", "URLBM" },
            };

            this.HtmlEncode = new FunctionItem(this, Guid.Parse("8ADFDB01B0C742408E405999EEA813B7"), nameof(HtmlEncode))
            {
                Title = "HTML 编码",
                Icon = Icons.LanguageHtml5,
                Description = "对html字符串进行HtmlEncode编码与HtmlDecode解码。",
                View = Views.ViewNames.Channels.Encodings.HtmlEncode,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "HTMLBianMa", "HTMLBM" },
            };

            this.Ascii85Encode = new FunctionItem(this, Guid.Parse("BAE9969570D64055AAE5B357BF42E804"), nameof(Ascii85Encode))
            {
                Title = "ASCII85 编码",
                Icon = Icons.Altimeter,
                Description = "ASCII85 编码解码，可以指定字符串编码。",
                View = Views.ViewNames.Channels.Encodings.Ascii85Encode,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "ASCII85BianMa", "ASCII85BM" },
            };

            this.Base64Encode = new FunctionItem(this, Guid.Parse("ED00FB68A01E458C9122EE976D0036AA"), nameof(Base64Encode))
            {
                Title = "BASE64 编码",
                Icon = Icons.Altimeter,
                Description = "Base64 编码解码，可以指定字符串编码。",
                View = Views.ViewNames.Channels.Encodings.Base64Encode,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "Base64BianMa", "Base64BM" },
            };

            this.Base62Encode = new FunctionItem(this, Guid.Parse("3B78E3A4A1E749CBA151B24C147ECAC2"), nameof(Base62Encode))
            {
                Title = "BASE62 编码",
                Icon = Icons.Altimeter,
                Description = "Base62 编码解码，可以指定字符串编码。",
                View = Views.ViewNames.Channels.Encodings.Base62Encode,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "Base62BianMa", "Base62BM" },
            };

            this.Base16Encode = new FunctionItem(this, Guid.Parse("B018FD5E66374039A5A37BB3F57F3630"), nameof(Base16Encode))
            {
                Title = "BASE16 编码",
                Icon = Icons.Altimeter,
                Description = "Base16 编码解码，可以指定字符串编码。",
                View = Views.ViewNames.Channels.Encodings.Base16Encode,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "Base16BianMa", "Base16BM" },
            };

            this.Items = new[]
            {
                StringZipper,
                UrlEncode,
                HtmlEncode,
                Ascii85Encode,
                Base64Encode,
                Base62Encode,
                Base16Encode,
            };
        }
    }
}
