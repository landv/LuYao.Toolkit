using Prism.Ioc;

namespace LuYao.Toolkit;
static class AppRegisterTypes
{
    public static void RegisterViews(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.ColorConverter), "Channels.Converters.ColorConverter");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.HexConvert), "Channels.Converters.HexConvert");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.IndentJson), "Channels.Converters.IndentJson");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.IndentXml), "Channels.Converters.IndentXml");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.JsonToCSharp), "Channels.Converters.JsonToCSharp");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.JsonToCsv), "Channels.Converters.JsonToCsv");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.PostmanConverter), "Channels.Converters.PostmanConverter");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.RsaKeyConvert), "Channels.Converters.RsaKeyConvert");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.TranslateJsonByJs), "Channels.Converters.TranslateJsonByJs");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.TranslateJsonByLiquid), "Channels.Converters.TranslateJsonByLiquid");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.TranslateXmlByXsl), "Channels.Converters.TranslateXmlByXsl");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.UnixTimestamp), "Channels.Converters.UnixTimestamp");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Converts.YamlToJson), "Channels.Converters.YamlToJson");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.CrossBorder.MercadoToWorldFirst), "Channels.CrossBorder.MercadoToWorldFirst");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Encodings.Ascii85Encode), "Channels.Encodings.Ascii85Encode");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Encodings.Base16Encode), "Channels.Encodings.Base16Encode");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Encodings.Base62Encode), "Channels.Encodings.Base62Encode");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Encodings.Base64Encode), "Channels.Encodings.Base64Encode");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Encodings.HtmlEncode), "Channels.Encodings.HtmlEncode");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Encodings.StringZipper), "Channels.Encodings.StringZipper");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Encodings.UrlEncode), "Channels.Encodings.UrlEncode");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Files.DetectFileEncodeing), "Channels.Files.DetectFileEncodeing");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Files.HashFile), "Channels.Files.HashFile");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Gens.GenAesKey), "Channels.Gens.GenAesKey");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Gens.GenGuid), "Channels.Gens.GenGuid");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Gens.GenLinesByRange), "Channels.Gens.GenLinesByRange");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Gens.GenPassword), "Channels.Gens.GenPassword");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Gens.GenRsaKey), "Channels.Gens.GenRsaKey");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Gens.GenXCodeEntity), "Channels.Gens.GenXCodeEntity");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Images.Base64ToImage), "Channels.Images.Base64ToImage");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Images.GifSplitter), "Channels.Images.GifSplitter");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Images.ImageToBase64), "Channels.Images.ImageToBase64");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Images.ImageToIcon), "Channels.Images.ImageToIcon");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Networks.HttpProxyChecker), "Channels.Networks.HttpProxyChecker");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Networks.IPLookup), "Channels.Networks.IPLookup");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Networks.Ping), "Channels.Networks.Ping");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Networks.PortProxy.Detail), "Channels.Networks.PortProxy.Detail");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Networks.PortProxy.Index), "Channels.Networks.PortProxy.Index");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Networks.RemoteDesktopManager), "Channels.Networks.RemoteDesktopManager");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Networks.TrafficMonitor), "Channels.Networks.TrafficMonitor");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Networks.UrlAnalyzer), "Channels.Networks.UrlAnalyzer");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Networks.UserAgentParser), "Channels.Networks.UserAgentParser");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Networks.Whois), "Channels.Networks.Whois");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Other.SystemToolkit), "Channels.Other.SystemToolkit");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Texts.CsvReader), "Channels.Texts.CsvReader");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Texts.FullHalfConvert), "Channels.Texts.FullHalfConvert");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Texts.GoogleTranslate), "Channels.Texts.GoogleTranslate");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Texts.HashCalculator), "Channels.Texts.HashCalculator");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Texts.LogReader), "Channels.Texts.LogReader");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Texts.RegexEvaluator), "Channels.Texts.RegexEvaluator");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Texts.TextJoin), "Channels.Texts.TextJoin");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Channels.Texts.YoudaoDictionary), "Channels.Texts.YoudaoDictionary");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Tabs.Explorer.Index), "Tabs.Explorer.Index");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Tabs.Navs.Index), "Tabs.Navs.Index");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Tabs.Rdp.Index), "Tabs.Rdp.Index");
        containerRegistry.RegisterForNavigation(typeof(LuYao.Toolkit.Tabs.Session.Index), "Tabs.Session.Index");
    }
}