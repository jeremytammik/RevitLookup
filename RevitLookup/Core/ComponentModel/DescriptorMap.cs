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
using Autodesk.Revit.DB.ExtensibleStorage;
using RevitLookup.Core.ComponentModel.Descriptors;
using RevitLookup.Core.Objects;
using RevitApplication = Autodesk.Revit.ApplicationServices.Application;

namespace RevitLookup.Core.ComponentModel;

public static class DescriptorMap
{
    /// <summary>
    ///     Search for a descriptor by approximate or exact match
    /// </summary>
    /// <remarks>
    ///     Exact search is necessary for the reflection engine, to add extensions and resolve conflicts when calling methods and properties. Type is not null <p />
    ///     An approximate search is needed to describe the object, which is displayed to the user. Type is null
    /// </remarks>
    public static Descriptor FindDescriptor(object obj, Type type)
    {
        return obj switch
        {
            //System
            bool value when type is null || type == typeof(bool) => new BoolDescriptor(value),

            //Hierarchy
            Element value when type is null || type == typeof(Element) => new ElementDescriptor(value),
            Parameter value when type is null || type == typeof(Parameter) => new ParameterDescriptor(value),
            Color value when type is null || type == typeof(Color) => new ColorDescriptor(value),
            Category value when type is null || type == typeof(Category) => new CategoryDescriptor(value),
            Document value when type is null || type == typeof(Document) => new DocumentDescriptor(value),
            ForgeTypeId value when type is null || type == typeof(ForgeTypeId) => new ForgeTypeIdDescriptor(value),
            City value when type is null || type == typeof(City) => new CityDescriptor(value),
            PrintManager value when type is null || type == typeof(PrintManager) => new PrintManagerDescriptor(value),
            GuidEnum value when type is null || type == typeof(GuidEnum) => new GuidEnumDescriptor(value),
            Definition value when type is null || type == typeof(Definition) => new DefinitionDescriptor(value),
            FailureMessage value when type is null || type == typeof(FailureMessage) => new FailureMessageDescriptor(value),
            PlanViewRange value when type is null || type == typeof(PlanViewRange) => new PlanViewRangeDescriptor(value),
            RevitApplication value when type is null || type == typeof(RevitApplication) => new ApplicationDescriptor(value),
            PaperSize value when type is null || type == typeof(PaperSize) => new PaperSizeDescriptor(value),
            Curve value when type is null || type == typeof(Curve) => new CurveDescriptor(value),
            Edge value when type is null || type == typeof(Edge) => new EdgeDescriptor(value),
            Solid value when type is null || type == typeof(Solid) => new SolidDescriptor(value),
            Face value when type is null || type == typeof(Face) => new FaceDescriptor(value),

            //Root
            Entity value => new EntityDescriptor(value),
            Field value => new FieldDescriptor(value),
            Schema value => new SchemaDescriptor(value),
            Exception value => new ExceptionDescriptor(value),
            IEnumerable value and not string => new EnumerableDescriptor(value),
            APIObject when type is null || type == typeof(APIObject) => new ApiObjectDescriptor(),
            IDisposable => new ApiObjectDescriptor(), //Faster then obj.GetType().Namespace == "Autodesk.Revit.DB"

            //Other
            null => new ObjectDescriptor(),
            _ when type is null => new ObjectDescriptor(obj),
            _ => new ObjectDescriptor()
        };
    }
}