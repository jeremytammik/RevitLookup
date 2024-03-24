// Copyright 2003-2023 by Autodesk, Inc.
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
using System.Runtime.InteropServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit;
using RevitLookup.Models;

namespace RevitLookup.Core;

public static class RevitShell
{
    public static UIControlledApplication CreateUiControlledApplication()
    {
        return (UIControlledApplication) Activator.CreateInstance(
            typeof(UIControlledApplication),
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            [Context.UiApplication],
            null);
    }

    public static List<UnitInfo> GetUnitInfos<T>()
    {
        var unitType = typeof(T);
        if (unitType == typeof(BuiltInParameter))
        {
            var parameters = Enum.GetValues(unitType).Cast<BuiltInParameter>();
            var list = new List<UnitInfo>();
            foreach (var parameter in parameters)
                try
                {
                    list.Add(new UnitInfo(parameter, parameter.ToString(), parameter.ToLabel()));
                }
                catch
                {
                    // ignored
                    // Some parameters don't have a label
                }

            return list;
        }

        if (unitType == typeof(BuiltInCategory))
        {
            var categories = Enum.GetValues(unitType).Cast<BuiltInCategory>();
            var list = new List<UnitInfo>();
            foreach (var category in categories)
                try
                {
                    list.Add(new UnitInfo(category, category.ToString(), category.ToLabel()));
                }
                catch
                {
                    // ignored
                    // Some categories don't have a label
                }

            return list;
        }

        if (unitType == typeof(ForgeTypeId))
#if R22_OR_GREATER
            return UnitUtils.GetAllUnits()
                .Concat(UnitUtils.GetAllDisciplines())
                .Concat(UnitUtils.GetAllMeasurableSpecs())
                .Concat(SpecUtils.GetAllSpecs())
                .Concat(ParameterUtils.GetAllBuiltInGroups())
                .Concat(ParameterUtils.GetAllBuiltInParameters())
                .Select(typeId => new UnitInfo(typeId, typeId.TypeId, typeId.ToLabel()))
                .ToList();
#else
            return UnitUtils.GetAllUnits().Select(typeId => new UnitInfo(typeId, typeId.TypeId, typeId.ToUnitLabel()))
                .Concat(UnitUtils.GetAllSpecs().Select(typeId => new UnitInfo(typeId, typeId.TypeId, typeId.ToSpecLabel())))
                .ToList();
#endif

        throw new NotSupportedException();
    }

    public static Parameter GetBuiltinParameter(BuiltInParameter builtInParameter)
    {
        const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var documentType = typeof(Document);
        var parameterType = typeof(Parameter);
        var assembly = Assembly.GetAssembly(parameterType);
        var aDocumentType = assembly.GetType("ADocument");
        var elementIdType = assembly.GetType("ElementId");
        var elementIdIdType = elementIdType.GetField("<alignment member>", bindingFlags)!;
        var getADocumentType = documentType.GetMethod("getADocument", bindingFlags)!;
        var parameterCtorType = parameterType.GetConstructor(bindingFlags, null, [aDocumentType.MakePointerType(), elementIdType.MakePointerType()], null)!;

        var elementId = Activator.CreateInstance(elementIdType);
        elementIdIdType.SetValue(elementId, builtInParameter);

        var handle = GCHandle.Alloc(elementId);
        var elementIdPointer = GCHandle.ToIntPtr(handle);
        Marshal.StructureToPtr(elementId, elementIdPointer, true);

        var parameter = (Parameter) parameterCtorType.Invoke([getADocumentType.Invoke(Context.Document, null), elementIdPointer]);
        handle.Free();

        return parameter;
    }

    public static Category GetBuiltinCategory(BuiltInCategory builtInCategory)
    {
        const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var documentType = typeof(Document);
        var categoryType = typeof(Category);
        var assembly = Assembly.GetAssembly(categoryType);
        var aDocumentType = assembly.GetType("ADocument");
        var elementIdType = assembly.GetType("ElementId");
        var elementIdIdType = elementIdType.GetField("<alignment member>", bindingFlags)!;
        var getADocumentType = documentType.GetMethod("getADocument", bindingFlags)!;
        var categoryCtorType = categoryType.GetConstructor(bindingFlags, null, [aDocumentType.MakePointerType(), elementIdType.MakePointerType()], null)!;

        var elementId = Activator.CreateInstance(elementIdType);
        elementIdIdType.SetValue(elementId, builtInCategory);

        var handle = GCHandle.Alloc(elementId);
        var elementIdPointer = GCHandle.ToIntPtr(handle);
        Marshal.StructureToPtr(elementId, elementIdPointer, true);

        var category = (Category) categoryCtorType.Invoke([getADocumentType.Invoke(Context.Document, null), elementIdPointer]);
        handle.Free();

        return category;
    }
}