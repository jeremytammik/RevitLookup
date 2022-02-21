using System.Collections;
using System.Reflection;
using Autodesk.Revit.DB;
using RevitLookup.Core.CollectorExtensions;
using RevitLookup.Core.RevitTypes;

namespace RevitLookup.Core.Streams;

public class ElementMethodsStream : IElementStream
{
    private readonly ArrayList _data;
    private readonly DataFactory _methodDataFactory;
    private readonly List<string> _seenMethods = new();

    public ElementMethodsStream(Document document, ArrayList data, object element)
    {
        _data = data;
        _methodDataFactory = new DataFactory(document, element);
    }

    public void Stream(Type type)
    {
        var methods = type
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(x => x.Name)
            .ToList();

        if (methods.Count > 0) _data.Add(new MemberSeparatorWithOffsetData("Methods"));
        var currentTypeMethods = new List<string>();

        foreach (var methodInfo in methods)
        {
            //if(IsMethodExcept(methodInfo)) continue;
            if (_seenMethods.Contains(methodInfo.Name)) continue;

            currentTypeMethods.Add(methodInfo.Name);
            var methodData = GetMethodData(methodInfo);
            if (methodData is not null) _data.Add(methodData);
        }

        _seenMethods.AddRange(currentTypeMethods);
    }

    private Data GetMethodData(MethodInfo methodInfo)
    {
        try
        {
            return _methodDataFactory.Create(methodInfo);
        }
        catch (Exception ex)
        {
            return new ExceptionData(methodInfo.Name, ex);
        }
    }

    bool IsMethodExcept(MethodInfo methodInfo)
    {
        if (methodInfo.Name.Equals("SubmitPrint", StringComparison.OrdinalIgnoreCase)) return true;
        return false;
    }
}