using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Texts;

public partial class GoogleTranslateViewModel : ViewModelBase
{
    public record LangItem(String Id, string Name, string[] Keywords);
    public static List<LangItem> AllFromLanguages { get; }
    public static List<LangItem> AllToLanguages { get; }
    private static LangItem Create(string id, string name, params string[] keywords) => new LangItem(id, name, keywords);
    static GoogleTranslateViewModel()
    {
        AllFromLanguages = new List<LangItem>
        {
            Create("auto","自动检测","auto","自动检测","zidongjiance","zdjc"),
            Create("en","英语","en","英语","yingyu","yy"),
            Create("fr","法语","fr","法语","fayu","fy"),
            Create("de","德语","de","德语","deyu","dy"),
            Create("it","意大利语","it","意大利语","yidaliyu","ydly"),
            Create("es","西班牙语","es","西班牙语","xibanyayu","xbyy"),
            Create("pt","葡萄牙语","pt","葡萄牙语","putaoyayu","ptyy"),
            Create("nl","荷兰语","nl","荷兰语","helanyu","hly"),
            Create("pl","波兰语","pl","波兰语","bolanyu","bly"),
            Create("ja","日语","ja","日语","riyu","ry"),
            Create("ko","韩语","ko","韩语","hanyu","hy"),
            Create("ar","阿拉伯语","ar","阿拉伯语","alaboyu","alby"),
            Create("tr","土耳其语","tr","土耳其语","tuerqiyu","teqy"),
            Create("th","泰语","th","泰语","taiyu","ty"),
            Create("ms","马来语","ms","马来语","malaiyu","mly"),
            Create("vi","越南语","vi","越南语","yuenanyu","yny"),
            Create("sv","瑞典语","sv","瑞典语","ruidianyu","rdy"),
            Create("id","印度尼西亚语","id","印度尼西亚语","yindunixiyayu","ydnxyy"),
            Create("zh-cn","中文（简体）","zh-cn","中文（简体）","zhongwen（jianti）","zw（jt）"),
            Create("zh-tw","中文（繁体）","zh-tw","中文（繁体）","zhongwen（fanti）","zw（ft）"),
            Create("sq","阿尔巴尼亚语","sq","阿尔巴尼亚语","aerbaniyayu","aebnyy"),
            Create("am","阿姆哈拉语","am","阿姆哈拉语","amuhalayu","amhly"),
            Create("az","阿塞拜疆语","az","阿塞拜疆语","asaibaijiangyu","asbjy"),
            Create("ga","爱尔兰语","ga","爱尔兰语","aierlanyu","aely"),
            Create("et","爱沙尼亚语","et","爱沙尼亚语","aishaniyayu","asnyy"),
            Create("eu","巴斯克语","eu","巴斯克语","basikeyu","bsky"),
            Create("be","白俄罗斯语","be","白俄罗斯语","baieluosiyu","belsy"),
            Create("bg","保加利亚语","bg","保加利亚语","baojialiyayu","bjlyy"),
            Create("is","冰岛语","is","冰岛语","bingdaoyu","bdy"),
            Create("bs","波斯尼亚语","bs","波斯尼亚语","bosiniyayu","bsnyy"),
            Create("fa","波斯语","fa","波斯语","bosiyu","bsy"),
            Create("da","丹麦语","da","丹麦语","danmaiyu","dmy"),
            Create("ru","俄语","ru","俄语","eyu","ey"),
            Create("fi","芬兰语","fi","芬兰语","fenlanyu","fly"),
            Create("km","高棉语","km","高棉语","gaomianyu","gmy"),
            Create("ka","格鲁吉亚语","ka","格鲁吉亚语","gelujiyayu","gljyy"),
            Create("gu","古吉拉特语","gu","古吉拉特语","gujilateyu","gjlty"),
            Create("kk","哈萨克语","kk","哈萨克语","hasakeyu","hsky"),
            Create("ht","海地克里奥尔语","ht","海地克里奥尔语","haidikeliaoeryu","hdklaey"),
            Create("ha","豪萨语","ha","豪萨语","haosayu","hsy"),
            Create("gl","加利西亚语","gl","加利西亚语","jialixiyayu","jlxyy"),
            Create("ca","加泰罗尼亚语","ca","加泰罗尼亚语","jiatailuoniyayu","jtlnyy"),
            Create("cs","捷克语","cs","捷克语","jiekeyu","jky"),
            Create("kn","卡纳达语","kn","卡纳达语","kanadayu","kndy"),
            Create("ky","柯尔克孜语","ky","柯尔克孜语","keerkeziyu","kekzy"),
            Create("xh","科萨语","xh","科萨语","kesayu","ksy"),
            Create("co","科西嘉语","co","科西嘉语","kexijiayu","kxjy"),
            Create("hr","克罗地亚语","hr","克罗地亚语","keluodiyayu","kldyy"),
            Create("ku","库尔德语","ku","库尔德语","kuerdeyu","kedy"),
            Create("la","拉丁语","la","拉丁语","ladingyu","ldy"),
            Create("lv","拉脱维亚语","lv","拉脱维亚语","latuoweiyayu","ltwyy"),
            Create("lo","老挝语","lo","老挝语","laowoyu","lwy"),
            Create("lt","立陶宛语","lt","立陶宛语","litaowanyu","ltwy"),
            Create("lb","卢森堡语","lb","卢森堡语","lusenbaoyu","lsby"),
            Create("ro","罗马尼亚语","ro","罗马尼亚语","luomaniyayu","lmnyy"),
            Create("mt","马耳他语","mt","马耳他语","maertayu","mety"),
            Create("mr","马拉地语","mr","马拉地语","maladiyu","mldy"),
            Create("mg","马拉加斯语","mg","马拉加斯语","malajiasiyu","mljsy"),
            Create("ml","马拉雅拉姆语","ml","马拉雅拉姆语","malayalamuyu","mlylmy"),
            Create("mk","马其顿语","mk","马其顿语","maqidunyu","mqdy"),
            Create("mi","毛利语","mi","毛利语","maoliyu","mly"),
            Create("mn","蒙古语","mn","蒙古语","mengguyu","mgy"),
            Create("bn","孟加拉语","bn","孟加拉语","mengjialayu","mjly"),
            Create("my","缅甸语","my","缅甸语","miandianyu","mdy"),
            Create("hmn","苗语","hmn","苗语","miaoyu","my"),
            Create("af","南非荷兰语","af","南非荷兰语","nanfeihelanyu","nfhly"),
            Create("st","南索托语","st","南索托语","nansuotuoyu","nsty"),
            Create("ne","尼泊尔语","ne","尼泊尔语","niboeryu","nbey"),
            Create("no","挪威语","no","挪威语","nuoweiyu","nwy"),
            Create("ps","普什图语","ps","普什图语","pushentuyu","psty"),
            Create("ny","齐切瓦语","ny","齐切瓦语","qiqiewayu","qqwy"),
            Create("sm","萨摩亚语","sm","萨摩亚语","samoyayu","smyy"),
            Create("sr","塞尔维亚语","sr","塞尔维亚语","saierweiyayu","sewyy"),
            Create("si","僧伽罗语","si","僧伽罗语","sengjialuoyu","sjly"),
            Create("sn","绍纳语","sn","绍纳语","shaonayu","sny"),
            Create("eo","世界语","eo","世界语","shijieyu","sjy"),
            Create("sk","斯洛伐克语","sk","斯洛伐克语","siluofakeyu","slfky"),
            Create("sl","斯洛文尼亚语","sl","斯洛文尼亚语","siluowenniyayu","slwnyy"),
            Create("sw","斯瓦希里语","sw","斯瓦希里语","siwaxiliyu","swxly"),
            Create("gd","苏格兰盖尔语","gd","苏格兰盖尔语","sugelangaieryu","sglgey"),
            Create("ceb","宿务语","ceb","宿务语","suwuyu","swy"),
            Create("so","索马里语","so","索马里语","suomaliyu","smly"),
            Create("tl","他加禄语","tl","他加禄语","tajialuyu","tjly"),
            Create("tg","塔吉克语","tg","塔吉克语","tajikeyu","tjky"),
            Create("te","泰卢固语","te","泰卢固语","tailuguyu","tlgy"),
            Create("ta","泰米尔语","ta","泰米尔语","taimieryu","tmey"),
            Create("cy","威尔士语","cy","威尔士语","weiershiyu","wesy"),
            Create("ur","乌尔都语","ur","乌尔都语","wuerdouyu","wedy"),
            Create("uk","乌克兰语","uk","乌克兰语","wukelanyu","wkly"),
            Create("uz","乌兹别克语","uz","乌兹别克语","wuzibiekeyu","wzbky"),
            Create("fy","西弗里西亚语","fy","西弗里西亚语","xifulixiyayu","xflxyy"),
            Create("iw","希伯来语","iw","希伯来语","xibolaiyu","xbly"),
            Create("el","希腊语","el","希腊语","xilayu","xly"),
            Create("haw","夏威夷语","haw","夏威夷语","xiaweiyiyu","xwyy"),
            Create("sd","信德语","sd","信德语","xindeyu","xdy"),
            Create("hu","匈牙利语","hu","匈牙利语","xiongyaliyu","xyly"),
            Create("su","巽他语","su","巽他语","xuntayu","xty"),
            Create("hy","亚美尼亚语","hy","亚美尼亚语","yameiniyayu","ymnyy"),
            Create("ig","伊博语","ig","伊博语","yiboyu","yby"),
            Create("yi","意第绪语","yi","意第绪语","yidixuyu","ydxy"),
            Create("hi","印地语","hi","印地语","yindiyu","ydy"),
            Create("yo","约鲁巴语","yo","约鲁巴语","yuelubayu","ylby"),
            Create("jw","爪哇语","jw","爪哇语","zhuawayu","zwy"),
            Create("zu","祖鲁语","zu","祖鲁语","zuluyu","zly"),
        };
        AllToLanguages = AllFromLanguages.Skip(1).ToList();
    }
    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(From))]
    private string _from = "auto";
    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(To))]
    private string _to = "zh-cn";
    public GoogleTranslateViewModel()
    {
        this.Filter(AllFromLanguages, FromLanguages, String.Empty);
        this.Filter(AllToLanguages, ToLanguages, String.Empty);
    }

    [RelayCommand]
    private void SetTo(string lang)
    {
        if (string.IsNullOrWhiteSpace(lang)) throw new ArgumentNullException(nameof(lang));
        To = lang;
    }

    [ObservableProperty]
    private string _input;
    [ObservableProperty]
    private string _output;
    [ObservableProperty]
    private string _sourceLanguage;

    [RelayCommand]
    private async Task Translate()
    {
        using (this.Busy())
        {
            if (string.IsNullOrWhiteSpace(this.Input))
            {
                this.Output = string.Empty;
                this.SourceLanguage = string.Empty;
            }
            else
            {
                var lines = this.Input.Split('\n');
                var num = this.dataVersion;
                TongjiService.Tongji(Views.ViewNames.Channels.Texts.GoogleTranslate, new { Action = nameof(Translate) });
                var ret = await GoogleService.Translate(this.From, this.To, lines);
                if (this.dataVersion == num)
                {
                    this.Output = string.Join("\n", ret.Lines);
                    this.SourceLanguage = ret.SourceLanguage;
                }
            }
        }
    }

    partial void OnInputChanged(string value)
    {
        var v = DateTime.Now.Ticks;
        dataVersion = v;
        AutoStart(v);
    }
    private long dataVersion = 0;
    private async void AutoStart(long n)
    {
        await Task.Delay(500);
        if (this.dataVersion == n) await Translate();
    }

    [RelayCommand]
    private void Clear() => this.Input = this.Output = this.SourceLanguage = string.Empty;

    [ObservableProperty]
    private string fromText;

    [ObservableProperty]
    private string toText;

    partial void OnFromTextChanged(string value)
    {
        Filter(AllFromLanguages, this.FromLanguages, value);
    }

    partial void OnToTextChanged(string value)
    {
        Filter(AllToLanguages, this.ToLanguages, value);
    }

    public ObservableCollection<LangItem> FromLanguages { get; set; } = new();

    public ObservableCollection<LangItem> ToLanguages { get; set; } = new();


    private void Filter(List<LangItem> all, ObservableCollection<LangItem> result, string text)
    {
        result.Clear();
        if (string.IsNullOrWhiteSpace(text))
        {
            foreach (var item in all)
            {
                result.Add(item);
            }
        }
        else
        {
            var key = text.ToLowerInvariant();
            foreach (var item in all)
            {
                var find = false;
                foreach (var k in item.Keywords)
                {
                    if (k.Contains(key))
                    {
                        find = true;
                        break;
                    }
                }
                if (find) result.Add(item);
            }
        }
    }
}
