using System;
using System.ComponentModel;

namespace LuYao.Toolkit.IO;

[Flags]
public enum FileType
{
    [Description("全部文件")]
    All = -1,

    [Description("文件")]
    Customer = 0,

    [Description("位图文件")]
    [FileTypeExtensions(".jpg", ".png", ".bmp", ".gif", ".webp")]
    Image = 1,

    [Description("文本文件")]
    [FileTypeExtensions(".txt")]
    Text = 2,

    [Description("Word文件")]
    [FileTypeExtensions(".doc", ".docx")]
    Word = 4,

    [Description("Excel文件")]
    [FileTypeExtensions(".xls", ".xlsx")]
    Excel = 8,

    [Description("压缩文件")]
    [FileTypeExtensions(".zip", ".rar", ".gz", ".7z")]
    Zip = 16,

    [Description("XML文件")]
    [FileTypeExtensions(".xml")]
    Xml = 32,

    [Description("可执行文件")]
    [FileTypeExtensions(".exe")]
    Exe = 64,

    [Description("XSL文件")]
    [FileTypeExtensions(".xsl", ".xslt")]
    Xsl = 128,

    [FileTypeExtensions(".json", ".js", ".txt")]
    [Description("JSON")]
    Json = 256,

    [FileTypeExtensions(".js")]
    [Description("JavaScript")]
    JavaScript = 512,

    [Description("日志")]
    [FileTypeExtensions(".log", ".txt")]
    Log = 1024,

    [Description("Liquid")]
    [FileTypeExtensions(".liquid")] 
    Liquid = 2048,

    [Description("矢量图")]
    [FileTypeExtensions(".svg")] 
    Svg = 4096,
}
