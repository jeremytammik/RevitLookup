// Copyright 2003-2021 by Autodesk, Inc. 
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

using System.Collections;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using RevitLookup.Core.RevitTypes;
using RevitLookup.Core.RevitTypes.PlaceHolders;
using Color = Autodesk.Revit.DB.Color;

namespace RevitLookup.Core.CollectorExtensions;

public static class DataTypeInfoHelper
{
    public static Data CreateFrom(Document document, MethodInfo info, object returnValue, object element)
    {
        return CreateFrom(document, info, info.ReturnType, returnValue, element);
    }

    private static Data CreateFrom(Document document, MemberInfo info, Type expectedType, object returnValue, object element)
    {
        try
        {
            if (expectedType == typeof(double))
                return new DoubleData(info.Name, (double) returnValue);
            
            if (expectedType == typeof(int))
            {
                var val = returnValue as int?;
                return new IntData(info.Name, val.GetValueOrDefault());
            }
            
            if (expectedType == typeof(bool))
            {
                var val = returnValue as bool?;
                return new BoolData(info.Name, val.GetValueOrDefault());
            }
            
            if (expectedType == typeof(string))
                return new StringData(info.Name, returnValue as string);
            
            if (expectedType == typeof(double?))
            {
                var value = (double?) returnValue;
                if (value.HasValue) return new DoubleData(info.Name, value.Value);
                return new StringData(info.Name, null);
            }

            if (expectedType == typeof(byte))
                return new IntData(info.Name, (byte) returnValue);

            if ((expectedType == typeof(GeometryObject) || expectedType == typeof(GeometryElement)) && element is Element geometryElement)
                return new ElementGeometryData(info.Name, geometryElement);

            if (expectedType == typeof(ElementId))
            {
                if (info.Name == nameof(Element.Id))
                    return new StringData(info.Name, (returnValue as ElementId)?.IntegerValue.ToString());

                return new ElementIdData(info.Name, returnValue as ElementId, document);
            }

            if (expectedType == typeof(ElementSet))
                return new ElementSetData(info.Name, returnValue as ElementSet);

            if (expectedType == typeof(CategoryNameMap))
                return new CategoryNameMapData(info.Name, returnValue as CategoryNameMap);
            
            if (expectedType == typeof(AssetProperty))
                return new AssetPropertyData(info.Name, element as AssetProperties);

            if (expectedType == typeof(Color))
                return new ColorData(info.Name, returnValue as Color);

            if (expectedType == typeof(DoubleArray))
                return new DoubleArrayData(info.Name, returnValue as DoubleArray);

            if (expectedType == typeof(ParameterSet))
                return new ParameterSetData(info.Name, element as Element, returnValue as ParameterSet);

            if (expectedType == typeof(UV))
                return new UvData(info.Name, returnValue as UV);

            if (expectedType == typeof(XYZ))
                return new XyzData(info.Name, returnValue as XYZ);

            if (expectedType == typeof(BindingMap))
                return new BindingMapData(info.Name, returnValue as BindingMap);
            
            if (expectedType == typeof(Guid))
            {
                var guidValue = (Guid) returnValue;
                return new StringData(info.Name, guidValue.ToString());
            }
            
            if (expectedType == typeof(PlanTopologySet))
            {
                var set = (PlanTopologySet) returnValue;
                var placeholders = set
                    .Cast<PlanTopology>()
                    .Select(x => new PlanTopologyPlaceholder(x))
                    .ToList();
                return new EnumerableData(info.Name, placeholders, document);
            }

            if (typeof(IEnumerable).IsAssignableFrom(expectedType) &&
                expectedType.IsGenericType &&
                (expectedType.GenericTypeArguments[0] == typeof(double) ||
                 expectedType.GenericTypeArguments[0] == typeof(int)) ||
                expectedType == typeof(DoubleArray))
                return new EnumerableAsString(info.Name, returnValue as IEnumerable);

            if (typeof(IEnumerable).IsAssignableFrom(expectedType))
                return new EnumerableData(info.Name, returnValue as IEnumerable, document);

            if (expectedType.IsEnum)
                return new StringData(info.Name, returnValue.ToString());

            return new ObjectData(info.Name, returnValue);
        }
        catch (Exception exception)
        {
            return new ExceptionData(info.Name, exception);
        }
    }

    public static void AddDataFromTypeInfo(Document document, MemberInfo info, Type expectedType, object returnValue, object elem, ArrayList data)
    {
        data.Add(CreateFrom(document, info, expectedType, returnValue, elem));
    }
}