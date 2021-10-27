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
    private readonly Document document;
    private readonly ArrayList data;
    private readonly object elem;
    private readonly List<string> seenProperties = new List<string>();

    public ElementPropertiesStream(Document document, ArrayList data, object elem )
    {
      this.document = document;
      this.data = data;
      this.elem = elem;
    }

    public void Stream( Type type )
    {
      var properties = GetElementProperties( type );

      var currentTypeProperties = new List<string>();

      if( properties.Length > 0 )
        data.Add( new Snoop.Data.MemberSeparatorWithOffset( "Properties" ) );

      foreach( PropertyInfo pi in properties )
      {
        if( seenProperties.Contains( pi.Name ) )
          continue;

        currentTypeProperties.Add( pi.Name );
        AddPropertyToData( pi );
      }
      seenProperties.AddRange( currentTypeProperties );
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
        var propertyInfo = pi.PropertyType.ContainsGenericParameters ? elem.GetType().GetProperty(pi.Name) : pi;
        
        if (propertyInfo == null)
            return;
        
        var propertyType = propertyInfo.PropertyType;

        try
        {
            object propertyValue;
            if (propertyInfo.Name == "Geometry")
                propertyValue = propertyInfo.GetValue(elem, new object[1] {new Options()});
            else if (propertyInfo.Name == "BoundingBox")
                propertyValue = propertyInfo.GetValue(elem, new object[1] {document.ActiveView});
            else if (propertyInfo.Name == "Item")
                propertyValue = propertyInfo.GetValue(elem, new object[1] {0});
            else if (propertyInfo.Name == "Parameter")
                return;
            else if (propertyInfo.Name == "PlanTopology")
                return;
            else if (propertyInfo.Name == "PlanTopologies" && propertyInfo.GetMethod.GetParameters().Length != 0)
                return;
            else if (propertyType.ContainsGenericParameters)
                propertyValue = elem.GetType().GetProperty(propertyInfo.Name)?.GetValue(elem);
            else
                propertyValue = propertyInfo.GetValue(elem);

            DataTypeInfoHelper.AddDataFromTypeInfo(document, propertyInfo, propertyType, propertyValue, elem, data);

            var category = elem as Category;
            if (category != null && propertyInfo.Name == "Id" && category.Id.IntegerValue < 0)
            {
                var bic = (BuiltInCategory) category.Id.IntegerValue;

                data.Add(new Snoop.Data.String("BuiltInCategory", bic.ToString()));
            }

        }
        catch (ArgumentException ex)
        {
            data.Add(new Snoop.Data.Exception(propertyInfo.Name, ex));
        }
        catch (TargetException ex)
        {
            data.Add(new Snoop.Data.Exception(propertyInfo.Name, ex));
        }
        catch (TargetInvocationException ex)
        {
            data.Add(new Snoop.Data.Exception(propertyInfo.Name, ex));
        }
        catch (TargetParameterCountException ex)
        {
            data.Add(new Snoop.Data.Exception(propertyInfo.Name, ex));
        }
    }
  }
}
