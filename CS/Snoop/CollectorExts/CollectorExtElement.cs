#region Header
//
// Copyright 2003-2017 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
#endregion // Header

using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Autodesk.Revit.DB;

using RevitLookup.Snoop.Collectors;


namespace RevitLookup.Snoop.CollectorExts
{
    /// <summary>
    /// Provide Snoop.Data for any classes related to an Element.
    /// </summary>

    public class CollectorExtElement : CollectorExt
    {
        Type[] types = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                        where domainAssembly.GetName().Name.ToLower().Contains("revit")
                        from assemblyType in domainAssembly.GetTypes()
                        select assemblyType).ToArray();

        protected override void CollectEvent(object sender, CollectorEventArgs e)
        {
            Collector snoopCollector = sender as Collector;
            if (snoopCollector == null)
            {
                Debug.Assert(false); // why did someone else send us the message?
                return;
            }

            if (e.ObjToSnoop is IEnumerable)
                snoopCollector.Data().Add(new Snoop.Data.Enumerable(e.ObjToSnoop.GetType().Name, e.ObjToSnoop as IEnumerable));
            else
                Stream(snoopCollector.Data(), e.ObjToSnoop);
        }

        private void Stream(ArrayList data, object elem)
        {
            var thisElementTypes = types.Where(x => elem.GetType().IsSubclassOf(x) || elem.GetType() == x || x.IsAssignableFrom(elem.GetType())).ToList();

            SpatialElementBoundaryOptions ops = new SpatialElementBoundaryOptions();
            ops.StoreFreeBoundaryFaces = true;
            ops.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Center;

            List<string> seenProperties = new List<string>();
            List<string> seenMethods = new List<string>();

            foreach (Type t in thisElementTypes)
            {
                var properties = GetElementProperties(t);
                var methods = GetElementMethods(t);

                data.Add(new Snoop.Data.ClassSeparator(t));

                List<string> currentTypeProperties = new List<string>();
                List<string> currentTypeMethods = new List<string>();

                if (properties.Length > 0) data.Add(new Snoop.Data.MemberSeparator("PROPERTIES"));

                foreach (PropertyInfo pi in properties)
                {
                    if (seenProperties.Contains(pi.Name)) continue;

                    currentTypeProperties.Add(pi.Name);
                    AddPropertyToData(pi, data, elem);
                }

                seenProperties.AddRange(currentTypeProperties);

                if (methods.Length > 0) data.Add(new Snoop.Data.MemberSeparator("METHODS"));

                foreach (MethodInfo mi in methods)
                {
                    if (seenMethods.Contains(mi.Name)) continue;

                    currentTypeMethods.Add(mi.Name);
                    AddMethodToData(mi, data, elem);
                }

                seenMethods.AddRange(currentTypeMethods);

                if (t.Name == "Space" || t.Name == "SpatialElement" || t.Name == "Room")
                    data.Add(new Snoop.Data.Object("GetBoundarySegments", (elem as SpatialElement).GetBoundarySegments(ops)));
            }
        }

        private void AddPropertyToData(PropertyInfo pi, ArrayList data, object elem)
        {
            Type propertyType = pi.PropertyType;

            try
            {
                object propertyValue;
                if (pi.Name == "Geometry")
                {
                    propertyValue = pi.GetValue(elem, new object[1] { new Options() });
                }
                else if (pi.Name == "BoundingBox")
                {
                    propertyValue = pi.GetValue(elem, new object[1] { m_app.ActiveUIDocument.ActiveView });
                }
                else if (pi.Name == "Parameter")
                {
                    return;
                }
                else
                {
                    propertyValue = pi.GetValue(elem); 
                }

                AddDataFromTypeInfo(pi, propertyType, propertyValue, elem, data);
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

        private void AddMethodToData(MethodInfo mi, ArrayList data, object elem)
        {
            Type methodType = mi.ReturnType;

            try
            {
                var returnValue = mi.Invoke(elem, new object[0]);
                AddDataFromTypeInfo(mi, methodType, returnValue, elem, data);
            }
            catch (TargetException ex)
            {
                data.Add(new Snoop.Data.Exception(mi.Name, ex));
            }
            catch (TargetInvocationException ex)
            {
                data.Add(new Snoop.Data.Exception(mi.Name, ex));
            }
            catch (TargetParameterCountException ex)
            {
                data.Add(new Snoop.Data.Exception(mi.Name, ex));
            }
        }

        private void AddDataFromTypeInfo(MemberInfo info, Type expectedType, object returnValue, object elem, ArrayList data)
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
                else if (expectedType == typeof(GeometryObject) || expectedType == typeof(GeometryElement))
                {
                    data.Add(new Snoop.Data.ElementGeometry(info.Name, elem as Element, m_app.Application));
                }
                else if (expectedType == typeof(ElementId))
                {
                    if (info.Name == "Id")
                        data.Add(new Snoop.Data.String(info.Name, (returnValue as ElementId).IntegerValue.ToString()));
                    else
                        data.Add(new Snoop.Data.ElementId(info.Name, returnValue as ElementId, m_app.ActiveUIDocument.Document));
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

        private PropertyInfo[] GetElementProperties(Type type)
        {
            return type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(x => x.GetMethod != null)
                .ToArray();
        }

        private MethodInfo[] GetElementMethods(Type type)
        {
            MethodInfo[] mInfo = type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(x => !x.GetParameters().Any()
                       && x.ReturnType != typeof(void)
                       && !x.IsSpecialName)
            .ToArray();

            return mInfo;
        }
    }
}
