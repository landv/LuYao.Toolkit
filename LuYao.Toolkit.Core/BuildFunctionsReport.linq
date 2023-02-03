<Query Kind="Statements">
  <Reference Relative="bin\Debug\netstandard2.0\LuYao.Common.dll">D:\src\coderbusy\luyao\src\toolkit\LuYao.Toolkit.Core\bin\Debug\netstandard2.0\LuYao.Common.dll</Reference>
  <Reference Relative="bin\Debug\netstandard2.0\LuYao.Toolkit.Core.dll">D:\src\coderbusy\luyao\src\toolkit\LuYao.Toolkit.Core\bin\Debug\netstandard2.0\LuYao.Toolkit.Core.dll</Reference>
  <Namespace>LuYao.Toolkit.Channels</Namespace>
  <Namespace>HtmlAgilityPack</Namespace>
</Query>

var channels = Channel.Channels;
var html = new HtmlAgilityPack.HtmlDocument();
var body = html.CreateElement("body");
html.DocumentNode.AppendChild(body);
body.AppendChild(html.CreateTextNode(Environment.NewLine));
foreach (var item in channels)
{
	body.AppendChild(html.CreateComment("<!-- wp:heading {\"level\":3} -->"));
	body.AppendChild(html.CreateTextNode(Environment.NewLine));
	var h3 = html.CreateElement("h3");
	h3.AppendChild(html.CreateTextNode(item.Title));
	body.AppendChild(h3);
	body.AppendChild(html.CreateTextNode(Environment.NewLine));
	body.AppendChild(html.CreateComment("<!-- /wp:heading -->"));
	body.AppendChild(html.CreateTextNode(Environment.NewLine));


	body.AppendChild(html.CreateTextNode(Environment.NewLine));
	body.AppendChild(html.CreateComment("<!-- wp:list -->"));
	body.AppendChild(html.CreateTextNode(Environment.NewLine));
	var ul = html.CreateElement("ul");
	body.AppendChild(ul);
	foreach (var func in item.Items)
	{
		ul.AppendChild(html.CreateTextNode(Environment.NewLine));
		var li = html.CreateElement("li");
		li.AppendChild(html.CreateTextNode(func.Title));
		ul.AppendChild(li);
	}
	body.AppendChild(html.CreateTextNode(Environment.NewLine));
	body.AppendChild(html.CreateComment("<!-- /wp:list -->"));
	body.AppendChild(html.CreateTextNode(Environment.NewLine));
	body.AppendChild(html.CreateTextNode(Environment.NewLine));
}

html.DocumentNode.InnerHtml.Dump();