using Newtonsoft.Json;
using System;

namespace LuYao.Toolkit.ViewStates;

public class ViewStateItem
{
    public string Type { get; set; }
    public string Json { get; set; }
    public object Read(Type type)
    {
        return JsonConvert.DeserializeObject(Json, type);
    }
    public void Write(object value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        Type = value.GetType().FullName;
        Json = JsonConvert.SerializeObject(value);
    }
}
