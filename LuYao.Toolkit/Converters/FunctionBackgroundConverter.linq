<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var colors = new string[] {
"#006266","#007B43","#009432","#0094C8","#0095D9","#00A3AF","#00A497","#00a8ff","#00b5cc","#00b894","#00cec9","#019875","#01a3a4","#03c9a9","#0652DD","#0984e3","#0abde3","#10ac84","#1289A7","#22a6b3","#273c75","#2792C3","#2CA9E1","#2e86de","#2ed573","#341f97","#349e69","#38A1DB","#38B48B","#47585C","#4834d4","#487eb0","#4cd137","#4d05e8","#4ecdc4","#595857","#6ab04c","#6c5ce7","#6F1E51","#71808E","#7f8fa6","#833471","#8395a7","#913d88","#9980FA","#9c88ff","#A3CB38","#a537fd","#B53471","#be2edd","#D3381C","#d63031","#D9333F","#D980FA","#e17055","#E17B34","#E60033","#e84118","#e84393","#E95295","#EA2027","#EA5506","#eb4d4b","#EB6238","#ED4C67","#ee5253","#EE5A24","#f0932b","#f368e0","#f52443","#f5ab35","#f62459","#F79F1F","#f7ca18","#fbc531","#fd79a8","#fdcb6e","#ff47d1","#ff6b81","#ff9f43","blue"
};
var converter = new ColorConverter();
var list = new SortedSet<int>();
foreach (var color in colors)
{
	var item =(Color) converter.ConvertFrom(color);
	list.Add(item.ToArgb());
}
foreach (var i in list)
{
	var item = Color.FromArgb(i);
	Console.WriteLine("new SolidColorBrush(Color.FromArgb({0},{1},{2},{3})),",item.A,item.R,item.G,item.B);
}