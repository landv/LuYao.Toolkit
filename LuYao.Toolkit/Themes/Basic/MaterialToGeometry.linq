<Query Kind="Statements">
  <NuGetReference>MahApps.Metro.IconPacks.Material</NuGetReference>
  <Namespace>MahApps.Metro.IconPacks.Converter</Namespace>
  <Namespace>System.Windows.Data</Namespace>
  <Namespace>MahApps.Metro.IconPacks</Namespace>
  <Namespace>System.Windows.Media</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

var idx = PackIconMaterialDataFactory.DataIndex.Value!;
var names = Enum.GetNames(typeof(PackIconMaterialKind));
foreach (var n in names)
{
	var kind = (PackIconMaterialKind)Enum.Parse(typeof(PackIconMaterialKind), n);
	if (idx.TryGetValue(kind, out var path))
	{
		Console.WriteLine("<Geometry o:Freeze=\"True\" x:Key=\"Material{0}Geometry\">{1}</Geometry>", n, path);
	}
}