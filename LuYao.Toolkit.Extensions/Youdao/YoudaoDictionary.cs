using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Youdao;

public static class YoudaoDictionary
{
    public static async Task<IReadOnlyList<YouDaoSuggestItem>> SuggestAsync(HttpClient http, string input)
    {
        if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));
        var url = $"https://dict.youdao.com/suggest?type=DESKDICT&num=4&ver=2.0&le=eng&q={HttpUtility.UrlEncode(input)}";
        using (var response = await http.GetAsync(url))
        {
            response.EnsureSuccessStatusCode();
            var xml = await response.ReadAsHtmlAsync();
            var ret = new List<YouDaoSuggestItem>();
            var doc = XDocument.Parse(xml);
            foreach (var node in doc.XPathSelectElements("//item"))
            {
                var title = node.Element("title");
                var explain = node.Element("explain");
                var result_num = node.Element("result_num");
                if (title == null || explain == null) continue;
                var item = new YouDaoSuggestItem
                {
                    Title = title.Value,
                    Explain = explain.Value,
                };
                if (result_num != null && Int32.TryParse(result_num.Value, out var num))
                {
                    item.ResultNum = num;
                }
                ret.Add(item);
            }
            return ret;
        }
    }
    public static async Task<YoudaoWord> QueryAsync(HttpClient http, string input)
    {
        if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));
        var url = $"https://m.youdao.com/dict?le=eng&q={HttpUtility.UrlEncode(input.Trim())}";
        using (var response = await http.GetAsync(url))
        {
            response.EnsureSuccessStatusCode();
            var html = await response.ReadAsHtmlAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var empty = doc.DocumentNode.SelectSingleNode("//p[@class=\"empty-content\"]");
            if (empty != null) throw new Exception(HtmlEntity.DeEntitize(empty.InnerText).Trim());
            var ret = new YoudaoWord(input.Trim());

            var phonetics = doc.DocumentNode.SelectNodes("//*[@class=\"phonetic\"]");
            if (phonetics != null)
            {
                foreach (var text in phonetics)
                {
                    var o = text.ParentNode;
                    var type = (o.SelectSingleNode("text()")?.InnerText ?? string.Empty).Trim();
                    var source = text.ParentNode.SelectSingleNode("a");
                    if (!string.IsNullOrWhiteSpace(type) && source != null)
                    {
                        ret.Phonetic.Add(new YoudaoPhonetic
                        {
                            Type = type,
                            Text = text.InnerText.Trim(),
                            Source = source.GetAttributeValue("data-rel", string.Empty)
                        });
                    }
                }
            }
            var items = doc.DocumentNode.SelectNodes("//*[@id=\"ec\"]/ul/li");
            if (items != null)
            {
                foreach (var item in items)
                {
                    ret.Paraphrase.Add(HtmlEntity.DeEntitize(item.InnerText).Trim());
                }
            }
            var subs = doc.DocumentNode.SelectNodes("//div[@class=\"sub\"]/p");
            if (subs != null)
            {
                foreach (var sub in subs)
                {
                    ret.Variant.Add(HtmlEntity.DeEntitize(sub.InnerText).Trim());
                }
            }
            if (ret.IsEmpty) throw new Exception("查询结果为空");
            return ret;
        }
    }
}
