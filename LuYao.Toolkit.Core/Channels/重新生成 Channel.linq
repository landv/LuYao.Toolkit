<Query Kind="Program">
  <Reference Relative="..\bin\Debug\net7.0\LuYao.Common.dll">D:\src\coderbusy.com\luyao\src\toolkit\LuYao.Toolkit.Core\bin\Debug\net7.0\LuYao.Common.dll</Reference>
  <Reference Relative="..\bin\Debug\net7.0\LuYao.Common.pdb">D:\src\coderbusy.com\luyao\src\toolkit\LuYao.Toolkit.Core\bin\Debug\net7.0\LuYao.Common.pdb</Reference>
  <Reference Relative="..\bin\Debug\net7.0\LuYao.Common.xml">D:\src\coderbusy.com\luyao\src\toolkit\LuYao.Toolkit.Core\bin\Debug\net7.0\LuYao.Common.xml</Reference>
  <Reference Relative="..\bin\Debug\net7.0\LuYao.Toolkit.Core.deps.json">D:\src\coderbusy.com\luyao\src\toolkit\LuYao.Toolkit.Core\bin\Debug\net7.0\LuYao.Toolkit.Core.deps.json</Reference>
  <Reference Relative="..\bin\Debug\net7.0\LuYao.Toolkit.Core.dll">D:\src\coderbusy.com\luyao\src\toolkit\LuYao.Toolkit.Core\bin\Debug\net7.0\LuYao.Toolkit.Core.dll</Reference>
  <Reference Relative="..\bin\Debug\net7.0\LuYao.Toolkit.Core.pdb">D:\src\coderbusy.com\luyao\src\toolkit\LuYao.Toolkit.Core\bin\Debug\net7.0\LuYao.Toolkit.Core.pdb</Reference>
  <Namespace>LuYao.Toolkit.Channels</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

void Main()
{
	//Channel.Channels.Dump();
	var dir = Path.GetDirectoryName(Util.CurrentQueryPath);
	foreach (var chl in Channel.Channels)
	{
		var fn = Path.Combine(dir, $"Channel.{chl.Name}.cs");
		Console.WriteLine(fn);
		var sb = new StringBuilder();
		sb.AppendLine("using System;");
		sb.AppendLine();
		sb.AppendLine("namespace LuYao.Toolkit.Channels;");
		sb.AppendLine();
		sb.AppendLine("public partial class Channel");
		sb.AppendLine("{");
		sb.AppendFormat("    public static {0}Channel {0} {{ get; }} = new {0}Channel();", chl.Name);
		sb.AppendLine();

		sb.AppendLine($"    public class {chl.Name}Channel : Channel");
		sb.AppendLine("    {");

		foreach (var item in chl.Items)
		{
			sb.AppendLine($"        public FunctionItem {(item.Name)} {{ get; }}");
		}
		sb.AppendLine();

		sb.AppendLine($"        public {chl.Name}Channel() : base(nameof({chl.Name}), {JsonConvert.SerializeObject(chl.Title)}, Icons.{chl.Icon})");
		sb.AppendLine("        {");

		foreach (var item in chl.Items)
		{
			sb.AppendLine($"            this.{(item.Name)} = new FunctionItem(this, Guid.Parse(\"{item.Id.ToString("N").ToUpper()}\"), nameof({item.Name}))");
			sb.AppendLine("            {");
			sb.AppendLine($"                Title = {JsonConvert.SerializeObject(item.Title)},");
			sb.AppendLine($"                Icon = Icons.{(item.Icon)},");
			sb.AppendLine($"                Description = {JsonConvert.SerializeObject(item.Description)},");
			sb.AppendLine($"                View = Views.ViewNames.{(item.View)},");
			sb.AppendLine($"                IsNew = {(item.IsNew ? "true" : "false")},");
			sb.AppendLine($"                UseNetwork = {(item.UseNetwork ? "true" : "false")},");
			sb.AppendLine($"                IsDocument = {(item.IsDocument ? "true" : "false")},");
			sb.AppendLine($"                Multiboxing = {(item.Multiboxing ? "true" : "false")},");
			sb.AppendLine($"                Keywords = new string[] {{ {(GetKeywords(item))} }},");
			sb.AppendLine("            };");
			sb.AppendLine();
		}

		sb.AppendLine("            this.Items = new[]");
		sb.AppendLine("            {");
		foreach (var item in chl.Items)
		{
			sb.AppendLine($"                {(item.Name)},");
		}
		sb.AppendLine("            };");

		sb.AppendLine("        }");

		sb.AppendLine("    }");

		sb.AppendLine("}");
		var str = sb.ToString();
		File.WriteAllText(fn, str, Encoding.UTF8);
	}
}

// You can define other methods, fields, classes and namespaces here
private static string GetName(string view)
{
	return view.Split('.').Last();
}
private static string GetKeywords(FunctionItem item)
{
	return Newtonsoft.Json.JsonConvert.SerializeObject(item.Keywords).Trim('[', ']').Replace(",",", ");
}