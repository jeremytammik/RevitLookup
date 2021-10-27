using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitLookup.Snoop.CollectorExts
{
  public class ElementPropertiesStream : IElementStream
  {
    private readonly Document _document;
    private readonly ArrayList _data;
    private readonly object _elem;
    private readonly List<string> _seenProperties = new List<string>();

    public ElementPropertiesStream(Document document, ArrayList data, object elem )
    {
      _document = document;
      _data = data;
      _elem = elem;
    }

    public void Stream( Type type )
    {
      var properties = GetElementProperties( type );

      var currentTypeProperties = new List<string>();

      if( properties.Length > 0 )
        _data.Add( new Data.MemberSeparatorWithOffset( "Properties" ) );

      foreach( var pi in properties )
      {
        if( _seenProperties.Contains( pi.Name ) )
          continue;

        currentTypeProperties.Add( pi.Name );
        AddPropertyToData( pi );
      }
      _seenProperties.AddRange( currentTypeProperties );
    }

    private PropertyInfo[] GetElementProperties( Type type )
    {
      return type
        .GetProperties( BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly )
        .Where( x => x.GetMethod != null )
        .OrderBy( x => x.Name )
        .ToArray();
    }

    private void AddPropertyToData(PropertyInfo pi)
    {
        var propertyInfo = pi.PropertyType.ContainsGenericParameters ? _elem.GetType().GetProperty(pi.Name) : pi;
        
        if (propertyInfo == null)
            return;
        
        var propertyType = propertyInfo.PropertyType;

        try
        {
            object propertyValue;
            switch (propertyInfo.Name)
            {
                case "Geometry":
                    propertyValue = propertyInfo.GetValue(_elem, new object[1] {new Options()});
                    break;
                case "BoundingBox":
                    propertyValue = propertyInfo.GetValue(_elem, new object[1] {_document.ActiveView});
                    break;
                case "Item":
                    propertyValue = propertyInfo.GetValue(_elem, new object[1] {0});
                    break;
                case "Parameter":
                case "PlanTopology":
                case "PlanTopologies" when propertyInfo.GetMethod.GetParameters().Length != 0:
                    return;
                default:
                {
                    if (propertyType.ContainsGenericParameters)
                        propertyValue = _elem.GetType().GetProperty(propertyInfo.Name)?.GetValue(_elem);
                    else
                        propertyValue = propertyInfo.GetValue(_elem);
                    break;
                }
            }

            DataTypeInfoHelper.AddDataFromTypeInfo(_document, propertyInfo, propertyType, propertyValue, _elem, _data);

            var category = _elem as Category;
            if (category != null && propertyInfo.Name == "Id" && category.Id.IntegerValue < 0)
            {
                var bic = (BuiltInCategory) category.Id.IntegerValue;

                _data.Add(new Snoop.Data.String("BuiltInCategory", bic.ToString()));
            }

        }
        catch (ArgumentException ex)
        {
            _data.Add(new Snoop.Data.Exception(propertyInfo.Name, ex));
        }
        catch (TargetException ex)
        {
            _data.Add(new Snoop.Data.Exception(propertyInfo.Name, ex));
        }
        catch (TargetInvocationException ex)
        {
            _data.Add(new Snoop.Data.Exception(propertyInfo.Name, ex));
        }
        catch (TargetParameterCountException ex)
        {
            _data.Add(new Snoop.Data.Exception(propertyInfo.Name, ex));
        }
    }
  }
}
