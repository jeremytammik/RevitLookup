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
            nameof(View.GetCategoryHidden) => ResolveCategoryHidden(),
            nameof(View.GetCategoryOverrides) => ResolveCategoryOverrides(),
            nameof(View.GetIsFilterEnabled) => ResolveFilterEnabled(),
            nameof(View.GetFilterOverrides) => ResolveFilterOverrides(),
            nameof(View.GetFilterVisibility) => ResolveFilterVisibility(),
            nameof(View.GetWorksetVisibility) => ResolveWorksetVisibility(),
#if REVIT2022_OR_GREATER
            nameof(View.GetColorFillSchemeId) => ResolveColorFillSchemeId(),
#endif
            _ => null
        };
        
        ResolveSet ResolveCategoryHidden()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                if (!category.IsVisibleInUI) continue;

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
                if (!category.IsVisibleInUI) continue;

                var result = view.GetCategoryOverrides(category.Id);
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
        
        ResolveSet ResolveWorksetVisibility()
        {
            var workSets = new FilteredWorksetCollector(Context.Document).OfKind(WorksetKind.UserWorkset).ToWorksets();
            var resolveSummary = new ResolveSet(workSets.Count);
            foreach (var workSet in workSets)
            {
                var result = view.GetWorksetVisibility(workSet.Id);
                resolveSummary.AppendVariant(result, $"{workSet.Name}: {result}");
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
                if (!category.IsVisibleInUI) continue;

                var result = view.GetColorFillSchemeId(category.Id);
                resolveSummary.AppendVariant(result, category.Name);
            }
            
            return resolveSummary;
        }
#endif
    }
}