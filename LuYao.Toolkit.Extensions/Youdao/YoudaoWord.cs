using System;
using System.Collections.Generic;
using System.Text;

namespace Youdao;
public class YoudaoWord
{
    public YoudaoWord(string word)
    {
        Word = string.IsNullOrWhiteSpace(word) ? throw new ArgumentNullException(nameof(word)) : word;
    }
    public bool IsEmpty => Paraphrase.Count == 0;
    public string Word { get; }
    public List<string> Paraphrase { get; } = new List<string>();
    public List<string> Variant { get; } = new List<string>();
    public List<YoudaoPhonetic> Phonetic { get; } = new List<YoudaoPhonetic>();
}