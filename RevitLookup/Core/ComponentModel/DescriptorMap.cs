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

using System.Collections;
using Autodesk.Revit.DB;
using RevitLookup.Core.ComponentModel.Descriptors;
using RevitLookup.Core.Objects;
using RevitApplication = Autodesk.Revit.ApplicationServices.Application;

namespace RevitLookup.Core.ComponentModel;

public static class DescriptorMap
{
    /// <summary>
    ///     Finding the first match of a descriptor type in the inheritance hierarchy
    /// </summary>
    public static Descriptor FindDescriptor(object obj, Type type)
    {
        return obj switch
        {
            bool value when type is null || type == typeof(bool) => new BoolDescriptor(value),

            Element value when type is null || type == typeof(Element) => new ElementDescriptor(value),
            Parameter value when type is null || type == typeof(Parameter) => new ParameterDescriptor(value),
            Color value when type is null || type == typeof(Color) => new ColorDescriptor(value),
            Category value when type is null || type == typeof(Category) => new CategoryDescriptor(value),
            Document value when type is null || type == typeof(Document) => new DocumentDescriptor(value),
            ForgeTypeId value when type is null || type == typeof(ForgeTypeId) => new ForgeTypeIdDescriptor(value),
            City value when type is null || type == typeof(City) => new CityDescriptor(value),
            PrintManager value when type is null || type == typeof(PrintManager) => new PrintManagerDescriptor(value),
            WorksetTable value when type is null || type == typeof(WorksetTable) => new WorksetTableDescriptor(value),
            Units value when type is null || type == typeof(Units) => new UnitsDescriptor(value),
            GuidEnum value when type is null || type == typeof(GuidEnum) => new GuidEnumDescriptor(value),
            Definition value when type is null || type == typeof(Definition) => new DefinitionDescriptor(value),
            DocumentPreviewSettings value when type is null || type == typeof(DocumentPreviewSettings) => new DocumentPreviewSettingsDescriptor(value),
            RevitApplication value when type is null || type == typeof(RevitApplication) => new ApplicationDescriptor(value),

            HashSet<ElementId> value when type is null || type == typeof(HashSet<ElementId>) => new IEnumerableDescriptor(value),
            CurveLoop value when type is null || type == typeof(CurveLoop) => new IEnumerableDescriptor(value),
            ICollection value => new IEnumerableDescriptor(value),
            IEnumerable value and APIObject => new IEnumerableDescriptor(value),

            APIObject when type is null || type == typeof(APIObject) => new APIObjectDescriptor(),
            Exception value when type is null || type == typeof(Exception) => new ExceptionDescriptor(value),

            null => new ObjectDescriptor(),
            _ when type is null => new ObjectDescriptor(obj),
            _ => new ObjectDescriptor()
        };
    }
}