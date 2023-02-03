<Query Kind="Statements">
  <Reference Relative="bin\Debug\net7.0-windows\LuYao.Toolkit.Core.dll">D:\src\coderbusy\luyao\src\toolkit\LuYao.Toolkit\bin\Debug\net7.0-windows\LuYao.Toolkit.Core.dll</Reference>
  <Reference Relative="bin\Debug\net7.0-windows\LuYao.Toolkit.dll">D:\src\coderbusy\luyao\src\toolkit\LuYao.Toolkit\bin\Debug\net7.0-windows\LuYao.Toolkit.dll</Reference>
  <NuGetReference>Fluid.Core</NuGetReference>
  <Namespace>LuYao.Toolkit</Namespace>
  <Namespace>LuYao.Toolkit.Views</Namespace>
  <Namespace>Fluid</Namespace>
</Query>

var assembly = typeof(MainWindow).Assembly;
var types = assembly.GetTypes();
var dic = new SortedDictionary<String, String>();
foreach (var item in types)
{
	if (item.IsAbstract) continue;
	if (item.IsGenericType) continue;
	var attr = item.GetCustomAttribute<ViewNameAttribute>();
	if (attr == null) continue;
	var key = item.FullName;
	var view = attr.Name;
	dic[key] = view;
}
var dir = Path.GetDirectoryName(Util.CurrentQueryPath);
var parser = new FluidParser();
var model = new { Types = dic.Select(i => new { Type = i.Key, View = i.Value }).ToList() };
var source = File.ReadAllText(Path.Combine(dir, "AppRegisterTypes.liquid"));
if (parser.TryParse(source, out var template, out var error))
{
	var context = new TemplateContext(model);
	context.Options.MemberAccessStrategy = new UnsafeMemberAccessStrategy();
	var output = template.Render(context);
	Console.WriteLine(output);
	File.WriteAllText(Path.Combine(dir, "AppRegisterTypes.cs"), output, Encoding.UTF8);
}
else
{
	Console.WriteLine($"Error: {error}");
}