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
            var values = context.Settings.Categories;
            var resolveSummary = new ResolveSet(values.Size);
            foreach (Category value in values)
            {
                if (value is null || !value.IsVisibleInUI) continue;
                var id = value.Id;
                var name = value.Name;
                var result = view.GetCategoryHidden(id);
                resolveSummary.AppendVariant(result, $"{name}, {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCategoryOverrides()
        {
            var values = context.Settings.Categories;
            var resolveSummary = new ResolveSet(values.Size);
            foreach (Category value in values)
            {
                if (value is null || !value.IsVisibleInUI) continue;
                var id = value.Id;
                var name = value.Name;
                var result = view.GetCategoryOverrides(id);
                resolveSummary.AppendVariant(result, $"Settings: {name}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveFilterOverrides()
        {
            var values = view.GetFilters();
            var resolveSummary = new ResolveSet(values.Count);
            foreach (var id in values)
            {
                var name = id.ToElement(context)!.Name;
                var result = view.GetFilterOverrides(id);
                resolveSummary.AppendVariant(result, $"Filter: {name}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveFilterVisibility()
        {
            var values = view.GetFilters();
            var resolveSummary = new ResolveSet(values.Count);
            foreach (var id in values)
            {
                var name = id.ToElement(context)!.Name;
                var result = view.GetFilterVisibility(id);
                resolveSummary.AppendVariant(result, $"Filter: {name}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveFilterEnabled()
        {
            var values = view.GetFilters();
            var resolveSummary = new ResolveSet(values.Count);
            foreach (var id in values)
            {
                var name = id.ToElement(context)!.Name;
                var result = view.GetIsFilterEnabled(id);
                resolveSummary.AppendVariant(result, $"Filter: {name}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveWorksetVisibility()
        {
            if (!context.IsWorkshared)
                return new ResolveSet().AppendVariant("The document is not workshared");
            var workSets = new FilteredWorksetCollector(Context.Document).OfKind(WorksetKind.UserWorkset).ToWorksets();
            var resolveSummary = new ResolveSet(workSets.Count);
            foreach (var workSet in workSets)
            {
                var name = workSet.Name;
                var result = view.GetWorksetVisibility(workSet.Id);
                resolveSummary.AppendVariant(result, $"Workset: {name}");
            }
            
            return resolveSummary;
        }
#if REVIT2022_OR_GREATER        
        ResolveSet ResolveColorFillSchemeId()
        {
            var values = context.Settings.Categories;
            var resolveSummary = new ResolveSet(values.Size);
            foreach (Category value in values)
            {
                if (value is null || !value.IsVisibleInUI) continue;
                var id = value.Id;
                var name = value.Name;
                var result = view.GetColorFillSchemeId(id);
                resolveSummary.AppendVariant(result, $"{name}, ColorFillScheme: {result}");
            }
            
            return resolveSummary;
        }
#endif
    }
}