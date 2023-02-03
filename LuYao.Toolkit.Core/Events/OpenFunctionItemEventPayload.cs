using LuYao.Toolkit.Channels;

namespace LuYao.Toolkit.Events;

public class OpenFunctionItemEventPayload
{
    public FunctionItem Item { get; }

    public OpenFunctionItemEventPayload(FunctionItem item)
    {
        Item = item;
    }
    public bool IsNewSession { get; set; }
    public bool IsMultiboxing { get; set; }
}
