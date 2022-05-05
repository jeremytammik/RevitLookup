// Copyright 2003-2022 by Autodesk, Inc. 
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

using System.Reflection;
using Autodesk.Revit.DB;
using RevitLookup.Core.RevitTypes.PlaceHolders;
using Exception = System.Exception;

namespace RevitLookup.Core;

public static class Utils
{
    private static string GetNamedObjectLabel(object obj)
    {
        var nameProperty = obj
            .GetType()
            .GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (nameProperty is null) return null;
        var propertyValue = nameProperty.GetValue(obj) as string;
        return $"{obj.GetType().Name}  {(string.IsNullOrEmpty(propertyValue) ? Labels.Undefined : propertyValue)}";
    }

    public static string GetParameterObjectLabel(object obj)
    {
        var parameter = obj as Parameter;
        return parameter?.Definition.Name;
    }

    private static string GetFamilyParameterObjectLabel(object obj)
    {
        var familyParameter = obj as FamilyParameter;
        return familyParameter?.Definition.Name;
    }

    public static string GetLabel(object obj)
    {
        switch (obj)
        {
            case null:
                return Labels.Null;
            case ISnoopPlaceholder placeholder:
                return placeholder.GetName();
            case Element elem:
                // TBD: Exceptions are thrown in certain cases when accessing the Name property. 
                // Eg. for the RoomTag object. So we will catch the exception and display the exception message
                // arj - 1/23/07
                try
                {
                    var nameStr = elem.Name == string.Empty ? Labels.Undefined : elem.Name;
                    return $"{nameStr}  {elem.Id.IntegerValue.ToString()}";
                }
                catch (Exception ex)
                {
                    return $"{null}  {ex.Message}";
                }
            default:
                return GetNamedObjectLabel(obj) ?? GetParameterObjectLabel(obj) ?? GetFamilyParameterObjectLabel(obj) ?? $"{obj.GetType().Name}";
        }
    }
}