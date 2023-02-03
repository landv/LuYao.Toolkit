using Newtonsoft.Json;
using System.Collections.Generic;

namespace LuYao.Toolkit.Tabs.Navs;

public partial class NavGroup
{
    public string Title { get; set; }
    public NavItem[] Items { get; set; }
    public static IReadOnlyCollection<NavGroup> GetAll() => JsonConvert.DeserializeObject<List<NavGroup>>(StringZipper.Unzip(Data));
}
