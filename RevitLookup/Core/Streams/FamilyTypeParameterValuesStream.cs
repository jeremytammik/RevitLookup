using System.Collections;
using Autodesk.Revit.DB;
using RevitLookup.Core.RevitTypes;

namespace RevitLookup.Core.Streams;

public class FamilyTypeParameterValuesStream : IElementStream
{
    private readonly ArrayList _data;
    private readonly object _element;

    public FamilyTypeParameterValuesStream(ArrayList data, object element)
    {
        _data = data;
        _element = element;
    }

    public void Stream(Type type)
    {
        if (type != typeof(Parameter)) return;

        var parameter = (Parameter) _element;
        if (!Category.IsBuiltInCategory(parameter.Definition.GetDataType())) return;
        var family = (parameter.Element as FamilyInstance)?.Symbol.Family ?? (parameter.Element as FamilySymbol)?.Family;
        if (family is null) return;
        var familyTypeParameterValues = family
            .GetFamilyTypeParameterValues(parameter.Id)
            .Select(family.Document.GetElement)
            .ToList();

        _data.Add(new EnumerableData($"{nameof(Family)}.{nameof(Family.GetFamilyTypeParameterValues)}()", familyTypeParameterValues));
    }
}