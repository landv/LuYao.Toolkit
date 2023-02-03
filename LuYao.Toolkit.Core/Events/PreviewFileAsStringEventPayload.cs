using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.Toolkit.Events;

public class PreviewFileAsStringEventPayload
{
    public PreviewFileAsStringEventPayload(string fullName, Encoding encoding)
    {
        FullName = fullName;
        Encoding = encoding;
    }

    public string FullName { get; }
    public Encoding Encoding { get; }
}
