using RevitLookup.Core.RevitTypes.PlaceHolders;

namespace RevitLookup.Core;

public class SnoopableWrapper
{
    public SnoopableWrapper(string title, object obj)
    {
        Title = title;
        Object = obj;
    }

    public string Title { get; }
    public object Object { get; }

    public static SnoopableWrapper Create(object obj)
    {
        return new SnoopableWrapper(Utils.GetLabel(obj), obj);
    }

    public Type GetUnderlyingType()
    {
        if (Object is ISnoopPlaceholder placeholder) return placeholder.GetUnderlyingType();
        return Object.GetType();
    }
}