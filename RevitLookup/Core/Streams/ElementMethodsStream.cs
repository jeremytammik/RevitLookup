using System.Collections;
using System.Reflection;
using Autodesk.Revit.DB;
using RevitLookup.Core.CollectorExtensions;
using RevitLookup.Core.RevitTypes;

namespace RevitLookup.Core.Streams;

public class ElementMethodsStream : IElementStream
{
    private readonly List<Data> _data;
    private readonly DataFactory _methodDataFactory;
    private readonly List<string> _seenMethods = new();

    public ElementMethodsStream(Document document, List<Data> data, object element)
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

        var currentTypeMethods = new List<string>();

        foreach (var methodInfo in methods)
        {
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
}