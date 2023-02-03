using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LuYao.Toolkit.Events;

public class ViewFileContentAsStringEventPayload
{
    public ViewFileContentAsStringEventPayload(string path, Encoding encoding)
    {
        this.Path = path;
        this.Encoding = encoding;
    }
    public string Path { get; }
    public Encoding Encoding { get; }
}
