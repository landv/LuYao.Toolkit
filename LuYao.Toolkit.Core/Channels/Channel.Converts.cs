using System;

namespace LuYao.Toolkit.Channels;

public partial class Channel
{
    public static ConvertsChannel Converts { get; } = new ConvertsChannel();
    public class ConvertsChannel : Channel
    {
        public FunctionItem UnixTimestamp { get; }
        public FunctionItem RsaKeyConvert { get; }
        public FunctionItem IndentJson { get; }
        public FunctionItem IndentXml { get; }
        public FunctionItem HexConvert { get; }
        public FunctionItem TranslateXmlByXsl { get; }
        public FunctionItem TranslateJsonByJs { get; }
        public FunctionItem TranslateJsonByLiquid { get; }
        public FunctionItem ColorConverter { get; }
        public FunctionItem JsonToCSharp { get; }
        public FunctionItem JsonToCsv { get; }
        public FunctionItem PostmanConverter { get; }
        public FunctionItem YamlToJson { get; }

        public ConvertsChannel() : base(nameof(Converts), "格式转换", Icons.CogTransfer)
        {
            this.UnixTimestamp = new FunctionItem(this, Guid.Parse("F9FF43A6C1C64DB4A296241061656868"), nameof(UnixTimestamp))
            {
                Title = "Unix 时间戳转换",
                Icon = Icons.Alarm,
                Description = "Unix 时间戳转换可以把Unix时间转成北京时间。",
                View = Views.ViewNames.Channels.Converters.UnixTimestamp,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "UnixShiJianChuoZhuanHuan", "UnixSJCZH" },
            };

            this.RsaKeyConvert = new FunctionItem(this, Guid.Parse("E1760524FC2F4434AF2EFAA5184C046E"), nameof(RsaKeyConvert))
            {
                Title = "RSA 密钥格式转换",
                Icon = Icons.KeyChange,
                Description = "RSA 私钥格式转换工具，支持 PKCS#1 、PKCS#8 私钥格式相互转换。",
                View = Views.ViewNames.Channels.Converters.RsaKeyConvert,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "rsaMiYaoGeShiZhuanHuan", "RsaMYGSZH", "MYZH", "MiYaoZhuanHuan" },
            };

            this.IndentJson = new FunctionItem(this, Guid.Parse("3D3FA103F8D84067BA0FDAE3DD356C6D"), nameof(IndentJson))
            {
                Title = "JSON 格式化",
                Icon = Icons.LanguageJavascript,
                Description = "支持对 JSON 字符串美化、压缩、转义等功能。",
                View = Views.ViewNames.Channels.Converters.IndentJson,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "JsonGeShiHua", "JsonGSH" },
            };

            this.IndentXml = new FunctionItem(this, Guid.Parse("B8665B3060FC4BEF982F2D3C39764A69"), nameof(IndentXml))
            {
                Title = "XML 格式化",
                Icon = Icons.Xml,
                Description = "支持对 XML 字符串美化、压缩等功能。",
                View = Views.ViewNames.Channels.Converters.IndentXml,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "XmlGeShiHua", "XmlGSH" },
            };

            this.HexConvert = new FunctionItem(this, Guid.Parse("BCA1AA0C8E5545B5A51D37116EF07BF7"), nameof(HexConvert))
            {
                Title = "进制转换",
                Icon = Icons.Numeric,
                Description = "支持在2~36进制之间进行任意转换。",
                View = Views.ViewNames.Channels.Converters.HexConvert,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "JinZhiZhuanHuan", "JZZH" },
            };

            this.TranslateXmlByXsl = new FunctionItem(this, Guid.Parse("ECBBC27B95CE48CFB27E2E91FCE3C82F"), nameof(TranslateXmlByXsl))
            {
                Title = "XSLT 转换",
                Icon = Icons.Xml,
                Description = "可以将 XML 数据档转换为另外的 XML 或其它格式，如 HTML 网页，纯文字。",
                View = Views.ViewNames.Channels.Converters.TranslateXmlByXsl,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "XSLTZhuanHuan", "XSLTZH" },
            };

            this.TranslateJsonByJs = new FunctionItem(this, Guid.Parse("F732A4D50D3C4A87AD681DD2D75B6C26"), nameof(TranslateJsonByJs))
            {
                Title = "JSON 转换",
                Icon = Icons.LanguageJavascript,
                Description = "可以将 JSON 数据档通过 JavaScript 转换为另外的 JSON 或其它格式。",
                View = Views.ViewNames.Channels.Converters.TranslateJsonByJs,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "JSONZhuanHuan", "JSONZH" },
            };

            this.TranslateJsonByLiquid = new FunctionItem(this, Guid.Parse("F18B557AEE614BAABE2942F98570C42F"), nameof(TranslateJsonByLiquid))
            {
                Title = "Liquid 转换",
                Icon = Icons.LiquidSpot,
                Description = "可以将 JSON 数据档通过 Liquid 转换为其它格式。",
                View = Views.ViewNames.Channels.Converters.TranslateJsonByLiquid,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "LiquidZhuanHuan", "LiquidZH" },
            };

            this.ColorConverter = new FunctionItem(this, Guid.Parse("B528E5B0C66548BAB5CBB8D210312E8C"), nameof(ColorConverter))
            {
                Title = "RGB 颜色转换",
                Icon = Icons.ColorHelper,
                Description = "RGB颜色和16进制色互转。",
                View = Views.ViewNames.Channels.Converters.ColorConverter,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "RGBYanSeZhuanHuan", "RGBYSZH" },
            };

            this.JsonToCSharp = new FunctionItem(this, Guid.Parse("6CBE22DCF367401CAFE331CA5B3843DA"), nameof(JsonToCSharp))
            {
                Title = "JSON 转 C# 实体类",
                Icon = Icons.AccountConvert,
                Description = "将 JSON 对象转换为相对应的 C# 实体类",
                View = Views.ViewNames.Channels.Converters.JsonToCSharp,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "JSONZhuanC#ShiTiLei", "JSONZCSTL" },
            };

            this.JsonToCsv = new FunctionItem(this, Guid.Parse("0A62665513684D9DB560A4C5E1BD3EFE"), nameof(JsonToCsv))
            {
                Title = "JSON 转 CSV",
                Icon = Icons.MicrosoftExcel,
                Description = "将 JSON 转换为 CSV 数据",
                View = Views.ViewNames.Channels.Converters.JsonToCsv,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "JSONZhuanCSV", "JSONZCSV" },
            };

            this.PostmanConverter = new FunctionItem(this, Guid.Parse("AC92D3CB1CEA46D2A263401B2D8E2577"), nameof(PostmanConverter))
            {
                Title = "Postman 数据转换",
                Icon = Icons.Web,
                Description = "Postman 参数格式互转",
                View = Views.ViewNames.Channels.Converters.PostmanConverter,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "PostmanShuJuZhuanHuan", "PostmanSJZH" },
            };

            this.YamlToJson = new FunctionItem(this, Guid.Parse("78C9651CF6E5470E801CAE043ADBC0CB"), nameof(YamlToJson))
            {
                Title = "Yaml 转 Json",
                Icon = Icons.CodeJson,
                Description = "Yaml 和 Json 格式转换",
                View = Views.ViewNames.Channels.Converters.YamlToJson,
                IsNew = true,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "YamlZhuanJson", "Yaml", "Json" },
            };

            this.Items = new[]
            {
                UnixTimestamp,
                RsaKeyConvert,
                IndentJson,
                IndentXml,
                HexConvert,
                TranslateXmlByXsl,
                TranslateJsonByJs,
                TranslateJsonByLiquid,
                ColorConverter,
                JsonToCSharp,
                JsonToCsv,
                PostmanConverter,
                YamlToJson,
            };
        }
    }
}
