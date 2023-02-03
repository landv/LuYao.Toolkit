using NewLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LuYao.Toolkit.IO;

public static class FileTypeExtensions
{
    private static FileTypeExtensionsAttribute GetFileTypeExtensions(this FileType value)
    {

        FieldInfo field = value.GetType().GetField(value.ToString(), BindingFlags.Static | BindingFlags.Public);
        if (field == null)
        {
            return null;
        }
        return field.GetCustomAttribute<FileTypeExtensionsAttribute>();
    }

    public static bool TryGetExtensions(this FileType type, string custome, out List<string> extensions)
    {
        extensions = new List<string>();
        if (type == FileType.All) return false;
        if (type == FileType.Customer)
        {
            if (!string.IsNullOrWhiteSpace(custome))
            {
                foreach (var ext in custome.Split(';')) if (!extensions.Contains(ext)) extensions.Add(ext);
            }
        }
        else
        {
            var values = Enum<FileType>.GetValues();
            foreach (var item in values)
            {
                if (type.HasFlag(item))
                {
                    var exts = item.GetFileTypeExtensions();
                    if (exts != null)
                    {
                        foreach (var ext in exts.Extensions)
                        {
                            if (!extensions.Contains(ext)) extensions.Add(ext);
                        }
                    }
                }
            }
        }
        return true;
    }

    public static string BuildFilter(this FileType type, string custome)
    {
        if (type == FileType.All) return "全部文件|*.*";
        var sb = new StringBuilder();
        if (type == FileType.Customer)
        {
            sb.Append("文件|");
            if (string.IsNullOrWhiteSpace(custome))
            {
                sb.Append("*.*");
            }
            else
            {
                foreach (var ext in custome.Split(';')) sb.AppendFormat("*{0};", ext);
                sb.Length--;
            }
        }
        else
        {
            var dic = new SortedDictionary<string, FileTypeExtensionsAttribute>();
            var values = Enum<FileType>.GetValues();
            foreach (var item in values)
            {
                if (type.HasFlag(item))
                {
                    var exts = item.GetFileTypeExtensions();
                    if (exts != null)
                    {
                        dic.Add(item.GetDescription(), exts);
                    }
                }
            }
            if (dic.Count <= 0) return "全部文件|*.*";
            if (dic.Count == 1)
            {
                var first = dic.First();
                return $"{first.Key}|{first.Value.FilterValue}";
            }
            var set = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in dic)
            {
                foreach (var value in item.Value.Extensions)
                {
                    set.Add("*" + value);
                }
            }
            sb.Append("全部文件|");
            sb.Append(string.Join(";", set));
            sb.Append('|');
            foreach (var item in dic)
            {
                sb.Append(item.Key);
                sb.Append('|');
                sb.Append(item.Value.FilterValue);
                sb.Append('|');
            }
            sb.Length--;
        }
        return sb.ToString();
    }
}
