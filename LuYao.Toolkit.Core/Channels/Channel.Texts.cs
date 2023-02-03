using System;

namespace LuYao.Toolkit.Channels;

public partial class Channel
{
    public static TextsChannel Texts { get; } = new TextsChannel();
    public class TextsChannel : Channel
    {
        public FunctionItem GoogleTranslate { get; }
        public FunctionItem TextJoin { get; }
        public FunctionItem LogReader { get; }
        public FunctionItem FullHalfConvert { get; }
        public FunctionItem CsvReader { get; }
        public FunctionItem RegexEvaluator { get; }
        public FunctionItem YoudaoDictionary { get; }
        public FunctionItem HashCalculator { get; }

        public TextsChannel() : base(nameof(Texts), "文字工具", Icons.FormatText)
        {
            this.GoogleTranslate = new FunctionItem(this, Guid.Parse("FDFA4822F6C94B6DAC9A2C7AF37DC407"), nameof(GoogleTranslate))
            {
                Title = "谷歌翻译",
                Icon = Icons.Translate,
                Description = "Google 的免费翻译服务可提供简体中文和另外 100 多种语言之间的互译功能。",
                View = Views.ViewNames.Channels.Texts.GoogleTranslate,
                IsNew = false,
                UseNetwork = true,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "GuGeFanYi", "GGFY" },
            };

            this.TextJoin = new FunctionItem(this, Guid.Parse("E6D432D4FA63469F983B3DC2D7A08A1B"), nameof(TextJoin))
            {
                Title = "多行拼接",
                Icon = Icons.FormatLineStyle,
                Description = "多用于拼接 SQL 中的 IN 语句参数",
                View = Views.ViewNames.Channels.Texts.TextJoin,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "DuoHangPinJie", "DHPJ" },
            };

            this.LogReader = new FunctionItem(this, Guid.Parse("6CF8B81E3D7541C7B4D50069AA6DC29D"), nameof(LogReader))
            {
                Title = "日志查看器",
                Icon = Icons.SearchWeb,
                Description = "用于查看不断输出的日志",
                View = Views.ViewNames.Channels.Texts.LogReader,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "RiZhiChaKanQi", "RZCKQ" },
            };

            this.FullHalfConvert = new FunctionItem(this, Guid.Parse("7571EB70555C41428343CC086DCB5A33"), nameof(FullHalfConvert))
            {
                Title = "全角半角转换",
                Icon = Icons.CircleHalfFull,
                Description = "全角占两个字节，全角是一种电脑字符，且每个全角字符占用两个标准字符（或半角字符）位置。半角占一个字节，全角半角转换工具，很方便的将全角字符和半角字符相互切换。",
                View = Views.ViewNames.Channels.Texts.FullHalfConvert,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "QuanJiaoBanJiaoZhuanHuan", "QJBJZH", "BanJiaoZhuanHuan", "QuanJiaoZhuanHuan", "QJZH", "BJZH" },
            };

            this.CsvReader = new FunctionItem(this, Guid.Parse("7571EB70555C41428343CC086DCB5A33"), nameof(CsvReader))
            {
                Title = "CSV 查看器",
                Icon = Icons.MicrosoftExcel,
                Description = "用于预览 CSV 文件",
                View = Views.ViewNames.Channels.Texts.CsvReader,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "CSVChaKanQi", "CSVCKQ" },
            };

            this.RegexEvaluator = new FunctionItem(this, Guid.Parse("7349B31A354440B4AD76C677D90D8D04"), nameof(RegexEvaluator))
            {
                Title = "正则测试",
                Icon = Icons.Regex,
                Description = "正则表达式测试工具",
                View = Views.ViewNames.Channels.Texts.RegexEvaluator,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "ZhengZeCeShi", "ZZCS", "regex" },
            };

            this.YoudaoDictionary = new FunctionItem(this, Guid.Parse("6D80E543C4274281BF7BB46ACDF54A87"), nameof(YoudaoDictionary))
            {
                Title = "有道词典",
                Icon = Icons.Bookshelf,
                Description = "英汉词典",
                View = Views.ViewNames.Channels.Texts.YoudaoDictionary,
                IsNew = false,
                UseNetwork = true,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "YouDaoCiDian", "YDCD" },
            };

            this.HashCalculator = new FunctionItem(this, Guid.Parse("4ABD65D372644199B78B6F2F60C829E8"), nameof(HashCalculator))
            {
                Title = "哈希计算器",
                Icon = Icons.Calculator,
                Description = "计算文本的哈希值，支持 HMAC 算法。",
                View = Views.ViewNames.Channels.Texts.HashCalculator,
                IsNew = true,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "HaXiJiSuanQi", "HXJSQ", "MD5", "SHA1", "HMAC" },
            };

            this.Items = new[]
            {
                GoogleTranslate,
                TextJoin,
                LogReader,
                FullHalfConvert,
                CsvReader,
                RegexEvaluator,
                YoudaoDictionary,
                HashCalculator,
            };
        }
    }
}
