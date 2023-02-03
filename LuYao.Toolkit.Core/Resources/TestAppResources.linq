<Query Kind="Statements">
  <Reference Relative="..\..\LuYao.Toolkit\bin\Debug\net7.0-windows\LuYao.Common.dll">D:\src\coderbusy\luyao\src\toolkit\LuYao.Toolkit\bin\Debug\net7.0-windows\LuYao.Common.dll</Reference>
  <Reference Relative="..\..\LuYao.Toolkit\bin\Debug\net7.0-windows\LuYao.Toolkit.Core.dll">D:\src\coderbusy\luyao\src\toolkit\LuYao.Toolkit\bin\Debug\net7.0-windows\LuYao.Toolkit.Core.dll</Reference>
  <Namespace>LuYao.Toolkit.Services</Namespace>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>LuYao.Toolkit.Resources</Namespace>
</Query>

for (int i = 0; i < AppResources.Count; i++)
{
	AppResources.Get(i).Length.Dump();
}
AppResources.Version.Dump();