namespace LuYao.Toolkit.Views;

public static class ViewNames
{
    public static class Tabs
    {
        public static class Session
        {
            public const string Index = $"{nameof(Tabs)}.{nameof(Session)}.{nameof(Index)}";
        }
        public static class Explorer
        {
            public const string Index = $"{nameof(Tabs)}.{nameof(Explorer)}.{nameof(Index)}";
        }
        public static class Rdp
        {
            public const string Index = $"{nameof(Tabs)}.{nameof(Rdp)}.{nameof(Index)}";
        }
        public static class Docs
        {
            public const string Index = $"{nameof(Tabs)}.{nameof(Docs)}.{nameof(Index)}";
        }
        public static class Navs
        {
            public const string Index = $"{nameof(Tabs)}.{nameof(Navs)}.{nameof(Index)}";
        }
    }
    public static class Channels
    {
        public static class Gens
        {
            public const string GenGuid = $"{nameof(Channels)}.{nameof(Gens)}.{nameof(GenGuid)}";
            public const string GenPassword = $"{nameof(Channels)}.{nameof(Gens)}.{nameof(GenPassword)}";
            public const string GenAesKey = $"{nameof(Channels)}.{nameof(Gens)}.{nameof(GenAesKey)}";
            public const string GenRsaKey = $"{nameof(Channels)}.{nameof(Gens)}.{nameof(GenRsaKey)}";
            public const string GenXCodeEntity = $"{nameof(Channels)}.{nameof(Gens)}.{nameof(GenXCodeEntity)}";
            public const string GenLinesByRange = $"{nameof(Channels)}.{nameof(Gens)}.{nameof(GenLinesByRange)}";
        }
        public static class Networks
        {
            public const string IPLookup = $"{nameof(Channels)}.{nameof(Networks)}.{nameof(IPLookup)}";
            public const string Ping = $"{nameof(Channels)}.{nameof(Networks)}.{nameof(Ping)}";
            public const string Whois = $"{nameof(Channels)}.{nameof(Networks)}.{nameof(Whois)}";
            public const string UserAgentParser = $"{nameof(Channels)}.{nameof(Networks)}.{nameof(UserAgentParser)}";
            public const string UrlAnalyzer = $"{nameof(Channels)}.{nameof(Networks)}.{nameof(UrlAnalyzer)}";
            public const string RemoteDesktopManager = $"{nameof(Channels)}.{nameof(Networks)}.{nameof(RemoteDesktopManager)}";
            public const string TrafficMonitor = $"{nameof(Channels)}.{nameof(Networks)}.{nameof(TrafficMonitor)}";
            public const string HttpProxyChecker = $"{nameof(Channels)}.{nameof(Networks)}.{nameof(HttpProxyChecker)}";
            public static class PortProxy
            {
                public const string Index = $"{nameof(Channels)}.{nameof(Networks)}.{nameof(PortProxy)}.{nameof(Index)}";
                public const string Detail = $"{nameof(Channels)}.{nameof(Networks)}.{nameof(PortProxy)}.{nameof(Detail)}";
            }
        }
        public static class Converters
        {
            public const string UnixTimestamp = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(UnixTimestamp)}";
            public const string RsaKeyConvert = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(RsaKeyConvert)}";
            public const string IndentJson = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(IndentJson)}";
            public const string IndentXml = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(IndentXml)}";
            public const string HexConvert = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(HexConvert)}";
            public const string TranslateXmlByXsl = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(TranslateXmlByXsl)}";
            public const string TranslateJsonByJs = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(TranslateJsonByJs)}";
            public const string TranslateJsonByLiquid = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(TranslateJsonByLiquid)}";
            public const string ColorConverter = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(ColorConverter)}";
            public const string JsonToCSharp = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(JsonToCSharp)}";
            public const string JsonToCsv = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(JsonToCsv)}";
            public const string PostmanConverter = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(PostmanConverter)}";
            public const string YamlToJson = $"{nameof(Channels)}.{nameof(Converters)}.{nameof(YamlToJson)}";
        }
        public static class Texts
        {
            public const string GoogleTranslate = $"{nameof(Channels)}.{nameof(Texts)}.{nameof(GoogleTranslate)}";
            public const string TextJoin = $"{nameof(Channels)}.{nameof(Texts)}.{nameof(TextJoin)}";
            public const string LogReader = $"{nameof(Channels)}.{nameof(Texts)}.{nameof(LogReader)}";
            public const string FullHalfConvert = $"{nameof(Channels)}.{nameof(Texts)}.{nameof(FullHalfConvert)}";
            public const string CsvReader = $"{nameof(Channels)}.{nameof(Texts)}.{nameof(CsvReader)}";
            public const string RegexEvaluator = $"{nameof(Channels)}.{nameof(Texts)}.{nameof(RegexEvaluator)}";
            public const string YoudaoDictionary = $"{nameof(Channels)}.{nameof(Texts)}.{nameof(YoudaoDictionary)}";
            public const string HashCalculator = $"{nameof(Channels)}.{nameof(Texts)}.{nameof(HashCalculator)}";
        }
        public static class Encodings
        {
            public const string StringZipper = $"{nameof(Channels)}.{nameof(Encodings)}.{nameof(StringZipper)}";
            public const string UrlEncode = $"{nameof(Channels)}.{nameof(Encodings)}.{nameof(UrlEncode)}";
            public const string HtmlEncode = $"{nameof(Channels)}.{nameof(Encodings)}.{nameof(HtmlEncode)}";
            public const string Ascii85Encode = $"{nameof(Channels)}.{nameof(Encodings)}.{nameof(Ascii85Encode)}";
            public const string Base64Encode = $"{nameof(Channels)}.{nameof(Encodings)}.{nameof(Base64Encode)}";
            public const string Base62Encode = $"{nameof(Channels)}.{nameof(Encodings)}.{nameof(Base62Encode)}";
            public const string Base16Encode = $"{nameof(Channels)}.{nameof(Encodings)}.{nameof(Base16Encode)}";
        }
        public static class Files
        {
            public const string DetectFileEncodeing = $"{nameof(Channels)}.{nameof(Files)}.{nameof(DetectFileEncodeing)}";
            public const string HashFile = $"{nameof(Channels)}.{nameof(Files)}.{nameof(HashFile)}";
        }
        public static class Images
        {
            public const string ImageToIcon = $"{nameof(Channels)}.{nameof(Images)}.{nameof(ImageToIcon)}";
            public const string GifSplitter = $"{nameof(Channels)}.{nameof(Images)}.{nameof(GifSplitter)}";
            public const string ImageToBase64 = $"{nameof(Channels)}.{nameof(Images)}.{nameof(ImageToBase64)}";
            public const string Base64ToImage = $"{nameof(Channels)}.{nameof(Images)}.{nameof(Base64ToImage)}";
        }
        public static class CrossBorder
        {
            public const string MercadoToWorldFirst = $"{nameof(Channels)}.{nameof(CrossBorder)}.{nameof(MercadoToWorldFirst)}";
        }
        public static class Other
        {
            public const string SystemToolkit = $"{nameof(Channels)}.{nameof(Other)}.{nameof(SystemToolkit)}";
        }
    }
}
