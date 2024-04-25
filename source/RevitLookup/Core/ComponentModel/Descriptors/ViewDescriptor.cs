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

public class ViewDescriptor(View view) : Descriptor, IDescriptorResolver
{
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(View.CanCategoryBeHidden) => ResolveCanCategoryBeHidden(),
            nameof(View.CanCategoryBeHiddenTemporary) => ResolveCanCategoryBeHiddenTemporary(),
            nameof(View.CanViewBeDuplicated) => ResolveCanViewBeDuplicated(),
            nameof(View.GetCategoryHidden) => ResolveCategoryHidden(),
            nameof(View.GetCategoryOverrides) => ResolveCategoryOverrides(),
            nameof(View.GetIsFilterEnabled) => ResolveFilterEnabled(),
            nameof(View.GetFilterOverrides) => ResolveFilterOverrides(),
            nameof(View.GetFilterVisibility) => ResolveFilterVisibility(),
            nameof(View.GetWorksetVisibility) => ResolveWorksetVisibility(),
            nameof(View.IsCategoryOverridable) => ResolveIsCategoryOverridable(),
            nameof(View.IsFilterApplied) => ResolveIsFilterApplied(),
            nameof(View.IsInTemporaryViewMode) => ResolveIsInTemporaryViewMode(),
            nameof(View.IsValidViewTemplate) => ResolveIsValidViewTemplate(),
            nameof(View.IsWorksetVisible) => ResolveIsWorksetVisible(),
            nameof(View.SupportsWorksharingDisplayMode) => ResolveSupportsWorksharingDisplayMode(),
#if REVIT2022_OR_GREATER
            nameof(View.GetColorFillSchemeId) => ResolveColorFillSchemeId(),
#endif
            _ => null
        };
        
        ResolveSet ResolveCanCategoryBeHidden()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.CanCategoryBeHidden(category.Id);
                resolveSummary.AppendVariant(result, $"{category.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCanCategoryBeHiddenTemporary()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.CanCategoryBeHiddenTemporary(category.Id);
                resolveSummary.AppendVariant(result, $"{category.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCanViewBeDuplicated()
        {
            var values = Enum.GetValues(typeof(ViewDuplicateOption));
            var resolveSummary = new ResolveSet(values.Length);
            
            foreach (ViewDuplicateOption option in values)
            {
                resolveSummary.AppendVariant(view.CanViewBeDuplicated(option), option.ToString());
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCategoryHidden()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.GetCategoryHidden(category.Id);
                resolveSummary.AppendVariant(result, $"{category.Name}: {result}");
            }

            return resolveSummary;
        }
        
        ResolveSet ResolveCategoryOverrides()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.GetCategoryOverrides(category.Id);
                resolveSummary.AppendVariant(result, category.Name);
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsCategoryOverridable()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.IsCategoryOverridable(category.Id);
                resolveSummary.AppendVariant(result, category.Name);
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveFilterOverrides()
        {
            var filters = view.GetFilters();
            var resolveSummary = new ResolveSet(filters.Count);
            foreach (var filterId in filters)
            {
                var filter = filterId.ToElement(context)!;
                var result = view.GetFilterOverrides(filterId);
                resolveSummary.AppendVariant(result, filter.Name);
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveFilterVisibility()
        {
            var filters = view.GetFilters();
            var resolveSummary = new ResolveSet(filters.Count);
            foreach (var filterId in filters)
            {
                var filter = filterId.ToElement(context)!;
                var result = view.GetFilterVisibility(filterId);
                resolveSummary.AppendVariant(result, $"{filter.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveFilterEnabled()
        {
            var filters = view.GetFilters();
            var resolveSummary = new ResolveSet(filters.Count);
            foreach (var filterId in filters)
            {
                var filter = filterId.ToElement(context)!;
                var result = view.GetIsFilterEnabled(filterId);
                resolveSummary.AppendVariant(result, $"{filter.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsFilterApplied()
        {
            var filters = view.GetFilters();
            var resolveSummary = new ResolveSet(filters.Count);
            foreach (var filterId in filters)
            {
                var filter = filterId.ToElement(context)!;
                var result = view.IsFilterApplied(filterId);
                resolveSummary.AppendVariant(result, $"{filter.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsInTemporaryViewMode()
        {
            var values = Enum.GetValues(typeof(TemporaryViewMode));
            var resolveSummary = new ResolveSet(values.Length);
            
            foreach (TemporaryViewMode mode in values)
            {
                resolveSummary.AppendVariant(view.IsInTemporaryViewMode(mode), mode.ToString());
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsValidViewTemplate()
        {
            var templates = context.EnumerateInstances<View>().Where(x => x.IsTemplate).ToArray();
            var resolveSummary = new ResolveSet(templates.Length);
            foreach (var template in templates)
            {
                var result = view.IsValidViewTemplate(template.Id);
                resolveSummary.AppendVariant(result, $"{template.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsWorksetVisible()
        {
            var workSets = new FilteredWorksetCollector(context).OfKind(WorksetKind.UserWorkset).ToWorksets();
            var resolveSummary = new ResolveSet(workSets.Count);
            foreach (var workSet in workSets)
            {
                var result = view.IsWorksetVisible(workSet.Id);
                resolveSummary.AppendVariant(result, $"{workSet.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveWorksetVisibility()
        {
            var workSets = new FilteredWorksetCollector(context).OfKind(WorksetKind.UserWorkset).ToWorksets();
            var resolveSummary = new ResolveSet(workSets.Count);
            foreach (var workSet in workSets)
            {
                var result = view.GetWorksetVisibility(workSet.Id);
                resolveSummary.AppendVariant(result, $"{workSet.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveSupportsWorksharingDisplayMode()
        {
            var values = Enum.GetValues(typeof(WorksharingDisplayMode));
            var resolveSummary = new ResolveSet(values.Length);
            
            foreach (WorksharingDisplayMode mode in values)
            {
                resolveSummary.AppendVariant(view.SupportsWorksharingDisplayMode(mode), mode.ToString());
            }
            
            return resolveSummary;
        }
#if REVIT2022_OR_GREATER      

        ResolveSet ResolveColorFillSchemeId()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                var result = view.GetColorFillSchemeId(category.Id);
                resolveSummary.AppendVariant(result, category.Name);
            }
            
            return resolveSummary;
        }
#endif
    }
}