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
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class CategoryDescriptor : Descriptor, IDescriptorExtension, IDescriptorResolver
{
    private readonly Category _category;

    public CategoryDescriptor(Category category)
    {
        _category = category;
        Name = category.Name;
    }

    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(_category, extension =>
        {
            extension.Name = "GetElements";
            extension.Result = extension.Context
#if REVIT2023_OR_GREATER
                .GetInstances(_category.BuiltInCategory);
#else
                .GetInstances((BuiltInCategory) _category.Id.IntegerValue);
#endif
        });
#if !REVIT2023_OR_GREATER
        manager.Register(_category, extension =>
        {
            extension.Name = "BuiltInCategory";
            extension.Result = (BuiltInCategory) extension.Value.Id.IntegerValue;
        });
#endif
    }

    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            "AllowsVisibilityControl" => ResolveSet.Append(_category.get_AllowsVisibilityControl(Context.ActiveView), "Active view"),
            "Visible" => ResolveSet.Append(_category.get_Visible(Context.ActiveView), "Active view"),
            nameof(Category.GetGraphicsStyle) => ResolveSet
                .Append(_category.GetGraphicsStyle(GraphicsStyleType.Cut), "Cut")
                .AppendVariant(_category.GetGraphicsStyle(GraphicsStyleType.Projection), "Projection"),
            nameof(Category.GetLinePatternId) => ResolveSet
                .Append(_category.GetLinePatternId(GraphicsStyleType.Cut), "Cut")
                .AppendVariant(_category.GetLinePatternId(GraphicsStyleType.Projection), "Projection"),
            nameof(Category.GetLineWeight) => ResolveSet
                .Append(_category.GetLineWeight(GraphicsStyleType.Cut), "Cut")
                .AppendVariant(_category.GetLineWeight(GraphicsStyleType.Projection), "Projection"),
            _ => null
        };
    }
}