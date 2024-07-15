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
using Autodesk.Revit.DB.Analysis;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class ViewDescriptor(View view) : ElementDescriptor(view)
{
    public override Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(View.CanCategoryBeHidden) => ResolveCanCategoryBeHidden,
            nameof(View.CanCategoryBeHiddenTemporary) => ResolveCanCategoryBeHiddenTemporary,
            nameof(View.CanViewBeDuplicated) => ResolveCanViewBeDuplicated,
            nameof(View.GetCategoryHidden) => ResolveCategoryHidden,
            nameof(View.GetCategoryOverrides) => ResolveCategoryOverrides,
            nameof(View.GetIsFilterEnabled) => ResolveFilterEnabled,
            nameof(View.GetFilterOverrides) => ResolveFilterOverrides,
            nameof(View.GetFilterVisibility) => ResolveFilterVisibility,
            nameof(View.GetWorksetVisibility) => ResolveWorksetVisibility,
            nameof(View.IsCategoryOverridable) => ResolveIsCategoryOverridable,
            nameof(View.IsFilterApplied) => ResolveIsFilterApplied,
            nameof(View.IsInTemporaryViewMode) => ResolveIsInTemporaryViewMode,
            nameof(View.IsValidViewTemplate) => ResolveIsValidViewTemplate,
            nameof(View.IsWorksetVisible) => ResolveIsWorksetVisible,
            nameof(View.SupportsWorksharingDisplayMode) => ResolveSupportsWorksharingDisplayMode,
#if REVIT2022_OR_GREATER
            nameof(View.GetColorFillSchemeId) => ResolveColorFillSchemeId,
#endif
            _ => null
        };
        
        IVariants ResolveCanCategoryBeHidden()
        {
            var categories = context.Settings.Categories;
            var variants = new Variants<bool>(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.CanCategoryBeHidden(category.Id);
                variants.Add(result, $"{category.Name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCanCategoryBeHiddenTemporary()
        {
            var categories = context.Settings.Categories;
            var variants = new Variants<bool>(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.CanCategoryBeHiddenTemporary(category.Id);
                variants.Add(result, $"{category.Name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCanViewBeDuplicated()
        {
            var values = Enum.GetValues(typeof(ViewDuplicateOption));
            var variants = new Variants<bool>(values.Length);
            
            foreach (ViewDuplicateOption option in values)
            {
                var result = view.CanViewBeDuplicated(option);
                variants.Add(result, $"{option.ToString()}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCategoryHidden()
        {
            var categories = context.Settings.Categories;
            var variants = new Variants<bool>(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.GetCategoryHidden(category.Id);
                variants.Add(result, $"{category.Name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCategoryOverrides()
        {
            var categories = context.Settings.Categories;
            var variants = new Variants<OverrideGraphicSettings>(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.GetCategoryOverrides(category.Id);
                variants.Add(result, category.Name);
            }
            
            return variants;
        }
        
        IVariants ResolveIsCategoryOverridable()
        {
            var categories = context.Settings.Categories;
            var variants = new Variants<bool>(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.IsCategoryOverridable(category.Id);
                variants.Add(result, $"{category.Name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveFilterOverrides()
        {
            var filters = view.GetFilters();
            var variants = new Variants<OverrideGraphicSettings>(filters.Count);
            foreach (var filterId in filters)
            {
                var filter = filterId.ToElement(context)!;
                var result = view.GetFilterOverrides(filterId);
                variants.Add(result, filter.Name);
            }
            
            return variants;
        }
        
        IVariants ResolveFilterVisibility()
        {
            var filters = view.GetFilters();
            var variants = new Variants<bool>(filters.Count);
            foreach (var filterId in filters)
            {
                var filter = filterId.ToElement(context)!;
                var result = view.GetFilterVisibility(filterId);
                variants.Add(result, $"{filter.Name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveFilterEnabled()
        {
            var filters = view.GetFilters();
            var variants = new Variants<bool>(filters.Count);
            foreach (var filterId in filters)
            {
                var filter = filterId.ToElement(context)!;
                var result = view.GetIsFilterEnabled(filterId);
                variants.Add(result, $"{filter.Name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveIsFilterApplied()
        {
            var filters = view.GetFilters();
            var variants = new Variants<bool>(filters.Count);
            foreach (var filterId in filters)
            {
                var filter = filterId.ToElement(context)!;
                var result = view.IsFilterApplied(filterId);
                variants.Add(result, $"{filter.Name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveIsInTemporaryViewMode()
        {
            var values = Enum.GetValues(typeof(TemporaryViewMode));
            var variants = new Variants<bool>(values.Length);
            
            foreach (TemporaryViewMode mode in values)
            {
                var result = view.IsInTemporaryViewMode(mode);
                variants.Add(result, $"{mode.ToString()}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveIsValidViewTemplate()
        {
            var templates = context.EnumerateInstances<View>().Where(x => x.IsTemplate).ToArray();
            var variants = new Variants<bool>(templates.Length);
            foreach (var template in templates)
            {
                var result = view.IsValidViewTemplate(template.Id);
                variants.Add(result, $"{template.Name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveIsWorksetVisible()
        {
            var workSets = new FilteredWorksetCollector(context).OfKind(WorksetKind.UserWorkset).ToWorksets();
            var variants = new Variants<bool>(workSets.Count);
            foreach (var workSet in workSets)
            {
                var result = view.IsWorksetVisible(workSet.Id);
                variants.Add(result, $"{workSet.Name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveWorksetVisibility()
        {
            var workSets = new FilteredWorksetCollector(context).OfKind(WorksetKind.UserWorkset).ToWorksets();
            var variants = new Variants<WorksetVisibility>(workSets.Count);
            foreach (var workSet in workSets)
            {
                var result = view.GetWorksetVisibility(workSet.Id);
                variants.Add(result, $"{workSet.Name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveSupportsWorksharingDisplayMode()
        {
            var values = Enum.GetValues(typeof(WorksharingDisplayMode));
            var variants = new Variants<bool>(values.Length);
            
            foreach (WorksharingDisplayMode mode in values)
            {
                var result = view.SupportsWorksharingDisplayMode(mode);
                variants.Add(result, $"{mode.ToString()}: {result}");
            }
            
            return variants;
        }
#if REVIT2022_OR_GREATER
        
        IVariants ResolveColorFillSchemeId()
        {
            var categories = context.Settings.Categories;
            var variants = new Variants<ElementId>(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.GetColorFillSchemeId(category.Id);
                variants.Add(result, category.Name);
            }
            
            return variants;
        }
#endif
    }
    
    public override void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(SpatialFieldManager.GetSpatialFieldManager), _ => SpatialFieldManager.GetSpatialFieldManager(view));
    }
}