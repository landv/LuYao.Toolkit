<Query Kind="Program">
  <Reference Relative="..\..\LuYao.Toolkit\bin\Debug\net7.0-windows\LuYao.Common.dll">D:\src\coderbusy\luyao\src\toolkit\LuYao.Toolkit\bin\Debug\net7.0-windows\LuYao.Common.dll</Reference>
  <Reference Relative="..\..\LuYao.Toolkit\bin\Debug\net7.0-windows\LuYao.Toolkit.Core.dll">D:\src\coderbusy\luyao\src\toolkit\LuYao.Toolkit\bin\Debug\net7.0-windows\LuYao.Toolkit.Core.dll</Reference>
  <Namespace>LuYao.Toolkit.Services</Namespace>
  <Namespace>System.IO.Compression</Namespace>
</Query>

void Main()
{
	var dir = Path.GetDirectoryName(Util.CurrentQueryPath);
	var root = string.Empty;
	while (!string.IsNullOrWhiteSpace(dir))
	{
		var sln = Directory.GetFiles(dir, "*.sln");
		if (sln.Length == 0)
		{
			dir = Path.GetDirectoryName(dir);
		}
		else
		{
			root = dir;
			break;
		}
	}
	root.Dump("ROOT");
	var regex = new Regex("\\.res\\.(?<ext>\\w+)$", RegexOptions.Compiled);
	var files = Directory.GetFiles(root, "*.res.*", SearchOption.AllDirectories);
	var items = new List<Item>();
	foreach (var file in files)
	{
		//获取项目目录
		var proj = GetProjectRoot(file);
		if (string.IsNullOrWhiteSpace(proj)) continue;
		var re = Path.GetRelativePath(proj, file);
		re = regex.Replace(re, "") + Path.GetExtension(file).ToUpperInvariant();
		var name = ToConstName(re);
		var str = FileService.ReadAllText(file);
		var bytes = Encoding.UTF8.GetBytes(str);
		bytes = Compress(bytes);
		items.Add(new Item(name, bytes.Length, bytes));
	}
	var version = DateTimeOffset.Now.ToUnixTimeSeconds();
	//写分布类
	var sb = new StringBuilder();
	sb.AppendLine("namespace LuYao.Toolkit.Resources;");
	sb.AppendLine();
	sb.AppendFormat("// Build Time : {0:O}", DateTime.Now);
	sb.AppendLine();
	sb.AppendFormat("// Version : {0}", version);
	sb.AppendLine();
	sb.AppendLine("partial class AppResources");
	sb.AppendLine("{");
	for (int i = 0; i < items.Count; i++)
	{
		var item = items[i];
		sb.AppendFormat("    public static string {0} => Get({1});", item.Name, i);
		sb.AppendLine();
	}
	sb.AppendLine("}");
	File.WriteAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "AppResources.g.cs"), sb.ToString(), Encoding.UTF8);
	//写数据格式
	var target = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "LuYao.Toolkit.dat");
	if (File.Exists(target)) File.Delete(target);
	using (var fs = File.OpenWrite(target))
	{
		using (var w = new BinaryWriter(fs))
		{
			w.Write(version);
			w.Write(items.Count);
			foreach (var item in items) w.Write(item.Length);
			foreach (var item in items) w.Write(item.Data);
		}
	}
	items.Count.Dump("Total");
}
string GetProjectRoot(string file)
{
	var dir = Path.GetDirectoryName(file);
	string root = string.Empty;
	while (!string.IsNullOrWhiteSpace(dir))
	{
		var files = Directory.GetFiles(dir, "*.csproj");
		if (files.Length > 0)
		{
			root = dir;
			break;
		}
		else
		{
			dir = Path.GetDirectoryName(dir);
		}
	}
	return root;
}
string ToConstName(string str)
{
	return str.Replace("\\", "_").Replace(".", "_").Replace("-","_");
}
// You can define other methods, fields, classes and namespaces here
byte[] Compress(byte[] bytes)
{
	using (var input = new MemoryStream(bytes))
	using (var output = new MemoryStream())
	using (DeflateStream compressor = new DeflateStream(output, CompressionLevel.SmallestSize))
	{
		input.CopyTo(compressor);
		compressor.Flush();
		return output.ToArray();
	}
}

public record Item(string Name, int Length, byte[] Data);