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
        private readonly UIApplication application;
        private readonly ArrayList data;
        private readonly object elem;
        private readonly List<string> seenProperties = new List<string>();

        public ElementPropertiesStream(UIApplication application, ArrayList data, object elem)
        {
            this.application = application;
            this.data = data;
            this.elem = elem;
        }

        public void Stream(Type type)
        {
            var properties = GetElementProperties(type);

            var currentTypeProperties = new List<string>();

            if (properties.Length > 0) data.Add(new Snoop.Data.MemberSeparatorWithOffset("Properties"));

            foreach (PropertyInfo pi in properties)
            {
                if (seenProperties.Contains(pi.Name)) continue;

                currentTypeProperties.Add(pi.Name);
                AddPropertyToData(pi);
            }

            seenProperties.AddRange(currentTypeProperties);
        }

        private PropertyInfo[] GetElementProperties(Type type)
        {
            return type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(x => x.GetMethod != null)
                .OrderBy(x => x.Name)
                .ToArray();
        }

        private void AddPropertyToData(PropertyInfo pi)
        {
            Type propertyType = pi.PropertyType;

            try
            {
                object propertyValue;
                if (pi.Name == "Geometry")
                    propertyValue = pi.GetValue(elem, new object[1] {new Options()});
                else if (pi.Name == "BoundingBox")
                    propertyValue = pi.GetValue(elem, new object[1] {application.ActiveUIDocument.ActiveView});
                else if (pi.Name == "Item")
                    propertyValue = pi.GetValue(elem, new object[1] { 0 });
                else if (pi.Name == "Parameter")
                    return;
                else
                    propertyValue = pi.GetValue(elem);

                DataTypeInfoHelper.AddDataFromTypeInfo(application, pi, propertyType, propertyValue, elem, data);

                var category = elem as Category;
                if (category != null && pi.Name == "Id" && category.Id.IntegerValue < 0)
                {
                    var bic = (BuiltInCategory) category.Id.IntegerValue;

                    data.Add(new Snoop.Data.String("BuiltInCategory", bic.ToString()));
                }
                    
            }
            catch (ArgumentException ex)
            {
                data.Add(new Snoop.Data.Exception(pi.Name, ex));
            }
            catch (TargetException ex)
            {
                data.Add(new Snoop.Data.Exception(pi.Name, ex));
            }
            catch (TargetInvocationException ex)
            {
                data.Add(new Snoop.Data.Exception(pi.Name, ex));
            }
            catch (TargetParameterCountException ex)
            {
                data.Add(new Snoop.Data.Exception(pi.Name, ex));
            }
        }
    }
}