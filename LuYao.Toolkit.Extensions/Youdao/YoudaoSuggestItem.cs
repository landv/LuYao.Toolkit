using System;
using System.Collections.Generic;
using System.Text;

namespace Youdao;

public class YouDaoSuggestItem
{
    public string Title { get; set; }
    public string Explain { get; set; }
    public int ResultNum { get; set; }
    public override string ToString() => this.Title;
}