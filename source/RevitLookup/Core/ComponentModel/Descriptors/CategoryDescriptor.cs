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
        manager.Register("GetElements", context =>
        {
            return context
#if REVIT2023_OR_GREATER
                .GetInstances(_category.BuiltInCategory);
#else
                .GetInstances((BuiltInCategory) _category.Id.IntegerValue);
#endif
        });
#if !REVIT2023_OR_GREATER
        manager.Register("BuiltInCategory", _ => (BuiltInCategory) _category.Id.IntegerValue);
#endif
    }
    
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            "AllowsVisibilityControl" => ResolveAllowsVisibilityControl,
            "Visible" => ResolveVisible,
            nameof(Category.GetGraphicsStyle) => ResolveGetGraphicsStyle,
            nameof(Category.GetLinePatternId) => ResolveGetLinePatternId,
            nameof(Category.GetLineWeight) => ResolveGetLineWeight,
            _ => null
        };
        
        IVariants ResolveGetLineWeight()
        {
            return new Variants<int?>(2)
                .Add(_category.GetLineWeight(GraphicsStyleType.Cut), "Cut")
                .Add(_category.GetLineWeight(GraphicsStyleType.Projection), "Projection");
        }
        
        IVariants ResolveGetLinePatternId()
        {
            return new Variants<ElementId>(2)
                .Add(_category.GetLinePatternId(GraphicsStyleType.Cut), "Cut")
                .Add(_category.GetLinePatternId(GraphicsStyleType.Projection), "Projection");
        }
        
        IVariants ResolveGetGraphicsStyle()
        {
            return new Variants<GraphicsStyle>(2)
                .Add(_category.GetGraphicsStyle(GraphicsStyleType.Cut), "Cut")
                .Add(_category.GetGraphicsStyle(GraphicsStyleType.Projection), "Projection");
        }
        
        IVariants ResolveAllowsVisibilityControl()
        {
            return Variants.Single(_category.get_AllowsVisibilityControl(Context.ActiveView), "Active view");
        }
        
        IVariants ResolveVisible()
        {
            return Variants.Single(_category.get_Visible(Context.ActiveView), "Active view");
        }
    }
}