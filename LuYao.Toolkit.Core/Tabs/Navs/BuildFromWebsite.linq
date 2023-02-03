<Query Kind="Statements">
  <Reference Relative="..\..\bin\Debug\netstandard2.0\LuYao.Common.dll">D:\src\coderbusy.com\luyao\src\toolkit\LuYao.Toolkit.Core\bin\Debug\netstandard2.0\LuYao.Common.dll</Reference>
  <Reference Relative="..\..\bin\Debug\netstandard2.0\LuYao.Toolkit.Core.dll">D:\src\coderbusy.com\luyao\src\toolkit\LuYao.Toolkit.Core\bin\Debug\netstandard2.0\LuYao.Toolkit.Core.dll</Reference>
  <NuGetReference>YamlDotNet</NuGetReference>
  <Namespace>LuYao.Toolkit.Tabs.Navs</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>LuYao</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>HtmlAgilityPack</Namespace>
</Query>

var http = new HttpClient();
var html = await http.GetStringAsync("https://www.coderbusy.com/navs");
var doc = new HtmlDocument();
doc.LoadHtml(html);
var items = doc.DocumentNode.SelectNodes("//div[@class='item']");
var d = new YamlDotNet.Serialization.Deserializer();
var navs = new List<NavItem>();
var all = new List<NavGroup>();
foreach (var item in items)
{
	navs.Clear();
	var g = new NavGroup { };
	all.Add(g);
	var title = item.SelectSingleNode("h3").InnerText;
	g.Title = title;
	var links = item.SelectNodes("ul/li/a");
	foreach (var link in links)
	{
		var notes = link.Attributes["notes"].Value;
		if (!string.IsNullOrWhiteSpace(notes))
		{
			var bytes = Convert.FromBase64String(notes);
			notes = Encoding.UTF8.GetString(bytes);
		}
		var nav = d.Deserialize<NavItem>(notes) ?? new NavItem { };
		nav.Url = link.GetAttributeValue("href", string.Empty);
		var strong = link.SelectSingleNode("strong");
		if (strong != null) nav.Title = strong.InnerText;
		var p = link.SelectSingleNode("p");
		if (p != null) nav.Description = p.InnerText;
		var img = link.SelectSingleNode("img");
		if (img != null) nav.Favicon = img.GetAttributeValue("src", string.Empty);
		navs.Add(nav);
	}
	g.Items = navs.ToArray();
}
all.Dump();
var json = JsonConvert.SerializeObject(all);
var zip = StringZipper.Zip(json, StringZipper.Deflate, StringZipper.Ascii85);
zip.Dump();
var target = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "NavGroup.g.cs");
var sb = new StringBuilder();
sb.AppendLine("namespace LuYao.Toolkit.Tabs.Navs;");
sb.AppendLine();
sb.AppendLine("public partial class NavGroup");
sb.AppendLine("{");
sb.AppendFormat("    private const string Data = {0};", JsonConvert.SerializeObject(zip));
sb.AppendLine();
sb.AppendLine("}");
File.WriteAllText(target, sb.ToString(), Encoding.UTF8);