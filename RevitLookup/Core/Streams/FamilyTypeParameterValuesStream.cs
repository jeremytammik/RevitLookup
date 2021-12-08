using System;
using System.Collections;
using System.Linq;
using Autodesk.Revit.DB;
using Enumerable = RevitLookup.Core.RevitTypes.Enumerable;

namespace RevitLookup.Core.Streams;

public class FamilyTypeParameterValuesStream : IElementStream
{
    private readonly ArrayList _data;
    private readonly object _elem;

    public FamilyTypeParameterValuesStream(ArrayList data, object elem)
    {
        _data = data;
        _elem = elem;
    }

    public void Stream(Type type)
    {
        if (type != typeof(Parameter)) return;

        var parameter = (Parameter) _elem;
        if (!Category.IsBuiltInCategory(parameter.Definition.GetDataType())) return;
        var family = (parameter.Element as FamilyInstance)?.Symbol.Family ?? (parameter.Element as FamilySymbol)?.Family;
        if (family is null) return;
        var familyTypeParameterValues = family
            .GetFamilyTypeParameterValues(parameter.Id)
            .Select(family.Document.GetElement)
            .ToList();

        _data.Add(new Enumerable($"{nameof(Family)}.{nameof(Family.GetFamilyTypeParameterValues)}()", familyTypeParameterValues));
    }
}