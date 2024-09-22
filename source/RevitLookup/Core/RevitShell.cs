// Copyright 2003-2024 by Autodesk, Inc.
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
using System.Text;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitLookup.Models;

namespace RevitLookup.Core;

public static class RevitShell
{
    public static ActionEventHandler ActionEventHandler { get; private set; }
    public static AsyncEventHandler AsyncEventHandler { get; private set; }
    public static AsyncEventHandler<IList<SnoopableObject>> ExternalElementHandler { get; private set; }
    public static AsyncEventHandler<IList<Descriptor>> ExternalDescriptorHandler { get; private set; }

    public static void RegisterHandlers()
    {
        ActionEventHandler = new ActionEventHandler();
        AsyncEventHandler = new AsyncEventHandler();
        ExternalElementHandler = new AsyncEventHandler<IList<SnoopableObject>>();
        ExternalDescriptorHandler = new AsyncEventHandler<IList<Descriptor>>();
    }

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
            return GetBuiltinParametersInfo();
        }

        if (unitType == typeof(BuiltInCategory))
        {
            return GetBuiltinCategoriesInfo();
        }

        if (unitType == typeof(ForgeTypeId))
        {
            return GetForgeInfo();
        }

        throw new NotSupportedException();
    }

    private static List<UnitInfo> GetBuiltinParametersInfo()
    {
        var parameters = Enum.GetValues(typeof(BuiltInParameter)).Cast<BuiltInParameter>();
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

    private static List<UnitInfo> GetBuiltinCategoriesInfo()
    {
        var categories = Enum.GetValues(typeof(BuiltInCategory)).Cast<BuiltInCategory>();
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

    private static List<UnitInfo> GetForgeInfo()
    {
        const BindingFlags searchFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly;

        var dataTypes = new List<PropertyInfo>();
#if REVIT2022_OR_GREATER
        dataTypes.AddRange(typeof(UnitTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId.Boolean).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId.String).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId.Int).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId.Reference).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(ParameterTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(GroupTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(DisciplineTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SymbolTypeId).GetProperties(searchFlags));
#else
        dataTypes.AddRange(typeof(UnitTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SymbolTypeId).GetProperties(searchFlags));
#endif
        return dataTypes.Select(info =>
            {
                var typeId = (ForgeTypeId) info.GetValue(null)!;
                var label = GetLabel(typeId, info);
                var className = GetClassName(info);
                return new UnitInfo(typeId, typeId.TypeId, label, className);
            })
            .ToList();
    }

    private static string GetClassName(PropertyInfo property)
    {
        var type = property.DeclaringType!;
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(type.Name);
        stringBuilder.Append('.');
        stringBuilder.Append(property.Name);

        while (type.IsNested)
        {
            type = type.DeclaringType!;
            stringBuilder.Insert(0, '.');
            stringBuilder.Insert(0, type.Name);
        }

        return stringBuilder.ToString();
    }

    private static string GetLabel(ForgeTypeId typeId, PropertyInfo property)
    {
        if (typeId.Empty()) return string.Empty;
        if (property.Name == nameof(SpecTypeId.Custom)) return string.Empty;

        var type = property.DeclaringType;
        while (type!.IsNested)
        {
            type = type.DeclaringType;
        }

        try
        {
            return type.Name switch
            {
                nameof(UnitTypeId) => typeId.ToUnitLabel(),
                nameof(SpecTypeId) => typeId.ToSpecLabel(),
                nameof(SymbolTypeId) => typeId.ToSymbolLabel(),
#if REVIT2022_OR_GREATER
                nameof(ParameterTypeId) => typeId.ToParameterLabel(),
                nameof(GroupTypeId) => typeId.ToGroupLabel(),
                nameof(DisciplineTypeId) => typeId.ToDisciplineLabel(),
#endif
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        catch
        {
            //Some parameter label thrown an exception
            return string.Empty;
        }
    }

    public static Parameter GetBuiltinParameter(BuiltInParameter builtInParameter)
    {
        const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var documentType = typeof(Document);
        var parameterType = typeof(Parameter);
        var assembly = Assembly.GetAssembly(parameterType)!;
        var aDocumentType = assembly.GetType("ADocument")!;
        var elementIdType = assembly.GetType("ElementId")!;
        var elementIdIdType = elementIdType.GetField("<alignment member>", bindingFlags)!;
        var getADocumentType = documentType.GetMethod("getADocument", bindingFlags)!;
        var parameterCtorType = parameterType.GetConstructor(bindingFlags, null, [aDocumentType.MakePointerType(), elementIdType.MakePointerType()], null)!;

        var elementId = Activator.CreateInstance(elementIdType)!;
        elementIdIdType.SetValue(elementId, builtInParameter);

        var handle = GCHandle.Alloc(elementId);
        var elementIdPointer = GCHandle.ToIntPtr(handle);
        Marshal.StructureToPtr(elementId, elementIdPointer, true);

        var parameter = (Parameter) parameterCtorType.Invoke([getADocumentType.Invoke(Context.ActiveDocument, null), elementIdPointer]);
        handle.Free();

        return parameter;
    }

    public static Category GetBuiltinCategory(BuiltInCategory builtInCategory)
    {
        const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var documentType = typeof(Document);
        var categoryType = typeof(Category);
        var assembly = Assembly.GetAssembly(categoryType)!;
        var aDocumentType = assembly.GetType("ADocument")!;
        var elementIdType = assembly.GetType("ElementId")!;
        var elementIdIdType = elementIdType.GetField("<alignment member>", bindingFlags)!;
        var getADocumentType = documentType.GetMethod("getADocument", bindingFlags)!;
        var categoryCtorType = categoryType.GetConstructor(bindingFlags, null, [aDocumentType.MakePointerType(), elementIdType.MakePointerType()], null)!;

        var elementId = Activator.CreateInstance(elementIdType)!;
        elementIdIdType.SetValue(elementId, builtInCategory);

        var handle = GCHandle.Alloc(elementId);
        var elementIdPointer = GCHandle.ToIntPtr(handle);
        Marshal.StructureToPtr(elementId, elementIdPointer, true);

        var category = (Category) categoryCtorType.Invoke([getADocumentType.Invoke(Context.ActiveDocument, null), elementIdPointer]);
        handle.Free();

        return category;
    }
}