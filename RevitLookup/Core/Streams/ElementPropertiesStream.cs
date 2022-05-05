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
    private readonly object _element;
    private readonly List<string> _seenProperties = new();

    public ElementPropertiesStream(Document document, ArrayList data, object element)
    {
        _document = document;
        _data = data;
        _element = element;
    }

    public void Stream(Type type)
    {
        var properties = GetElementProperties(type);
        var currentTypeProperties = new List<string>();

        foreach (var property in properties)
        {
            if (_seenProperties.Contains(property.Name)) continue;
            currentTypeProperties.Add(property.Name);
            AddPropertyToData(property);
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

    private void AddPropertyToData(PropertyInfo propertyInfo)
    {
        var property = propertyInfo.PropertyType.ContainsGenericParameters ? _element.GetType().GetProperty(propertyInfo.Name) : propertyInfo;
        if (property is null) return;
        var propertyType = property.PropertyType;

        try
        {
            object propertyValue;
            switch (property.Name)
            {
                case "Geometry":
                    propertyValue = property.GetValue(_element, new object[] {new Options()});
                    break;
                case "BoundingBox":
                    propertyValue = property.GetValue(_element, new object[] {_document.ActiveView});
                    break;
                case "Item":
                    propertyValue = property.GetValue(_element, new object[] {0});
                    break;
                case "Parameter":
                case "PlanTopology":
                case "PlanTopologies" when property.GetMethod.GetParameters().Length != 0:
                    return;
                default:
                {
                    propertyValue = propertyType.ContainsGenericParameters
                        ? _element.GetType().GetProperty(property.Name)?.GetValue(_element)
                        : property.GetValue(_element);
                    break;
                }
            }

            DataTypeInfoHelper.AddDataFromTypeInfo(_document, property, propertyType, propertyValue, _element, _data);

            if (_element is Category category && property.Name == "Id" && category.Id.IntegerValue < 0)
            {
                var bic = (BuiltInCategory) category.Id.IntegerValue;
                _data.Add(new StringData("BuiltInCategory", bic.ToString()));
            }
        }
        catch (Exception ex)
        {
            _data.Add(new ExceptionData(property.Name, ex));
        }
    }
}