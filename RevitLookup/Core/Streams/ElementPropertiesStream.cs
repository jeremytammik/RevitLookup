using System.Collections;
using System.Reflection;
using Autodesk.Revit.DB;
using RevitLookup.Core.CollectorExtensions;
using RevitLookup.Core.RevitTypes;

namespace RevitLookup.Core.Streams;

public class ElementPropertiesStream : IElementStream
{
    private readonly ArrayList _data;
    private readonly Document _document;
    private readonly object _elem;
    private readonly List<string> _seenProperties = new();

    public ElementPropertiesStream(Document document, ArrayList data, object elem)
    {
        _document = document;
        _data = data;
        _elem = elem;
    }

    public void Stream(Type type)
    {
        var properties = GetElementProperties(type);
        var currentTypeProperties = new List<string>();
        if (properties.Length > 0) _data.Add(new MemberSeparatorWithOffsetData("Properties"));

        foreach (var pi in properties)
        {
            if (_seenProperties.Contains(pi.Name)) continue;
            currentTypeProperties.Add(pi.Name);
            AddPropertyToData(pi);
        }

        _seenProperties.AddRange(currentTypeProperties);
    }

    private PropertyInfo[] GetElementProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(x => x.GetMethod is not null)
            .OrderBy(x => x.Name)
            .ToArray();
    }

    private void AddPropertyToData(PropertyInfo pi)
    {
        var propertyInfo = pi.PropertyType.ContainsGenericParameters ? _elem.GetType().GetProperty(pi.Name) : pi;
        if (propertyInfo is null) return;
        var propertyType = propertyInfo.PropertyType;

        try
        {
            object propertyValue;
            switch (propertyInfo.Name)
            {
                case "Geometry":
                    propertyValue = propertyInfo.GetValue(_elem, new object[] {new Options()});
                    break;
                case "BoundingBox":
                    propertyValue = propertyInfo.GetValue(_elem, new object[] {_document.ActiveView});
                    break;
                case "Item":
                    propertyValue = propertyInfo.GetValue(_elem, new object[] {0});
                    break;
                case "Parameter":
                case "PlanTopology":
                case "PlanTopologies" when propertyInfo.GetMethod.GetParameters().Length != 0:
                    return;
                default:
                {
                    propertyValue = propertyType.ContainsGenericParameters
                        ? _elem.GetType().GetProperty(propertyInfo.Name)?.GetValue(_elem)
                        : propertyInfo.GetValue(_elem);
                    break;
                }
            }

            DataTypeInfoHelper.AddDataFromTypeInfo(_document, propertyInfo, propertyType, propertyValue, _elem, _data);

            if (_elem is Category category && propertyInfo.Name == "Id" && category.Id.IntegerValue < 0)
            {
                var bic = (BuiltInCategory) category.Id.IntegerValue;
                _data.Add(new StringData("BuiltInCategory", bic.ToString()));
            }
        }
        catch (Exception ex)
        {
            _data.Add(new ExceptionData(propertyInfo.Name, ex));
        }
    }
}