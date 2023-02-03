using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace LuYao.Toolkit.Resources;

public static partial class AppResources
{
    private static readonly IReadOnlyList<string> _values;
    public static long Version { get; }
    static AppResources()
    {

        var ass = typeof(AppResources).Assembly;
        using (var ms = ass.GetManifestResourceStream($"{typeof(AppResources).Namespace}.LuYao.Toolkit.dat"))
        using (var r = new BinaryReader(ms))
        {
            Version = r.ReadInt64();
            var total = r.ReadInt32();
            var lengths = new int[total];
            var values = new string[total];
            for (int i = 0; i < total; i++)
            {
                var len = r.ReadInt32();
                lengths[i] = len;
            }
            for (int i = 0; i < total; i++)
            {
                var len = lengths[i];
                var bytes = r.ReadBytes(len);
                var str = Decompress(bytes);
                values[i] = str;
            }
            _values = values;
        }
    }
    static string Decompress(byte[] data)
    {
        using (var input = new MemoryStream(data))
        using (var output = new MemoryStream())
        using (DeflateStream decompressor = new DeflateStream(input, CompressionMode.Decompress))
        {
            decompressor.CopyTo(output);
            var bytes = output.ToArray();
            return Encoding.UTF8.GetString(bytes);
        }
    }
    public static string Get(int id)
    {
        if (id >= 0 && id < _values.Count) return _values[id];
        throw new Exception($"ID为:{id} 得资源未找到");
    }
    public static bool TryGet(int id, out string value)
    {
        if (id >= 0 && id < _values.Count)
        {
            value = _values[id];
            return true;
        }
        value = default;
        return false;
    }
    public static int Count => _values.Count;
}
