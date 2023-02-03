using System;
using System.Collections.Generic;
using System.Linq;

namespace LuYao.Toolkit.IO;

public class FileTypeExtensionsAttribute : Attribute
{
    public FileTypeExtensionsAttribute(params string[] extensions)
    {
        this.Extensions = extensions ?? throw new ArgumentNullException(nameof(extensions));
        if (this.Extensions.Count <= 0) throw new ArgumentOutOfRangeException(nameof(extensions));
        this.FilterValue = string.Join(
            ";",
            extensions.Order(StringComparer.InvariantCultureIgnoreCase)
            .Distinct()
            .Select(str => $"*{str}")
        );
    }
    public IReadOnlyCollection<string> Extensions { get; }
    public string FilterValue { get; }
}
