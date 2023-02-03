using NewLife.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LuYao.Toolkit.IO;

public static class TempHelper
{
    static TempHelper()
    {
        Root = Path.Combine(Path.GetTempPath(), "LuYao.Toolkit", "Cache");
        if (!Directory.Exists(Root)) Directory.CreateDirectory(Root);
    }

    public static string Root { get; }

    public static string GetTempFileName()
    {
        var dir = Root;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        var fn = Path.Combine(dir, Path.GetRandomFileName());
        XTrace.WriteLine("分配临时文件：{0}", fn);
        return fn;
    }

    public static string GetTempFileName(string name)
    {
        var dir = Root;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        return Path.Combine(dir, name);
    }


    public static void ClearTemp()
    {
        XTrace.WriteLine("清理临时目录：{0}", Root);
        var dirs = Directory.GetDirectories(Root);
        foreach (var dir in dirs)
        {
            try
            {
                XTrace.WriteLine("删除临时目录：{0}", dir);
                Directory.Delete(dir, true);
            }
            catch (Exception e)
            {
                XTrace.WriteLine("目录删除失败");
                XTrace.WriteException(e);
            }
        }

        var files = Directory.GetFiles(Root);
        foreach (var file in files)
        {
            try
            {
                XTrace.WriteLine("删除临时文件：{0}", file);
                File.Delete(file);
            }
            catch (Exception e)
            {
                XTrace.WriteLine("文件删除失败");
                XTrace.WriteException(e);
            }
        }
    }
}