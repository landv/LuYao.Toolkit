using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtfUnknown;

namespace LuYao.Toolkit.Services;

public static class FileService
{
    public static Encoding GetEncoding(string filePath)
    {
        var result = CharsetDetector.DetectFromFile(filePath);
        var encoding = Encoding.UTF8;
        if (result.Detected != null && result.Detected.Encoding != null) { encoding = result.Detected.Encoding; }
        return encoding;
    }
    public static async Task<string> ReadAllTextAsync(string filePath)
    {
        var encoding = GetEncoding(filePath);
        using (var fs = File.OpenRead(filePath))
        {
            using (var sr = new StreamReader(fs, encoding))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }

    public static string ReadAllText(string filePath)
    {
        var encoding = GetEncoding(filePath);
        using (var fs = File.OpenRead(filePath))
        {
            using (var sr = new StreamReader(fs, encoding))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
