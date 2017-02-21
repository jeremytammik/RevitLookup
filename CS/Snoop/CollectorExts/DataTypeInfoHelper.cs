using System;
using System.Collections;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitLookup.Snoop.CollectorExts
{
    public static class DataTypeInfoHelper
    {
        public static void AddDataFromTypeInfo(UIApplication application, MemberInfo info, Type expectedType, object returnValue, object elem, ArrayList data)
        {
            try
            {
                if (expectedType == typeof(bool))
                {
                    bool? val = returnValue as bool?;
                    data.Add(new Snoop.Data.Bool(info.Name, val.Value));
                }
                else if (expectedType == typeof(CategoryNameMap))
                {
                    data.Add(new Snoop.Data.CategoryNameMap(info.Name, returnValue as CategoryNameMap));
                }
                else if (expectedType == typeof(Double))
                {
                    double? val = returnValue as double?;
                    data.Add(new Snoop.Data.Double(info.Name, val.Value));
                }
                else if ((expectedType == typeof(GeometryObject) || expectedType == typeof(GeometryElement)) && elem is Element)
                {
                    data.Add(new Snoop.Data.ElementGeometry(info.Name, elem as Element, application.Application));
                }
                else if (expectedType == typeof(ElementId))
                {
                    if (info.Name == "Id")
                        data.Add(new Snoop.Data.String(info.Name, (returnValue as ElementId).IntegerValue.ToString()));
                    else
                        data.Add(new Snoop.Data.ElementId(info.Name, returnValue as ElementId, application.ActiveUIDocument.Document));
                }
                else if (expectedType == typeof(ElementSet))
                {
                    data.Add(new Snoop.Data.ElementSet(info.Name, returnValue as ElementSet));
                }
                else if (expectedType == typeof(IEnumerable))
                {
                    data.Add(new Snoop.Data.Enumerable(info.Name, returnValue as IEnumerable));
                }
                else if (expectedType == typeof(int))
                {
                    int? val = returnValue as int?;
                    data.Add(new Snoop.Data.Int(info.Name, val.Value));
                }
                else if (expectedType == typeof(int))
                {
                    int? val = returnValue as int?;
                    data.Add(new Snoop.Data.Int(info.Name, val.Value));
                }
                else if (expectedType == typeof(ParameterSet))
                {
                    data.Add(new Snoop.Data.ParameterSet(info.Name, elem as Element, returnValue as ParameterSet));
                }
                else if (expectedType == typeof(string))
                {
                    data.Add(new Snoop.Data.String(info.Name, returnValue as string));
                }
                else if (expectedType == typeof(UV))
                {
                    data.Add(new Snoop.Data.Uv(info.Name, returnValue as UV));
                }
                else if (expectedType == typeof(XYZ))
                {
                    data.Add(new Snoop.Data.Xyz(info.Name, returnValue as XYZ));
                }
                else if (expectedType.IsEnum)
                {
                    data.Add(new Snoop.Data.String(info.Name, returnValue.ToString()));
                } else if (expectedType == typeof (Guid))
                {
                    var guidValue = (Guid) returnValue;
                    data.Add(new Snoop.Data.String(info.Name, guidValue.ToString()));
                }
                else
                {
                    data.Add(new Snoop.Data.Object(info.Name, returnValue as object));
                }
            }
            catch (Exception ex)
            {
                data.Add(new Snoop.Data.Exception(info.Name, ex));
            }
        }
    }
}