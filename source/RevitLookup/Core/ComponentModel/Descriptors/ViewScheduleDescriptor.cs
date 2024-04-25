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

public class ViewScheduleDescriptor(ViewSchedule viewSchedule) : Descriptor, IDescriptorResolver
{
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(ViewSchedule.GetStripedRowsColor) => ResolveStripedRowsColor(),
            nameof(ViewSchedule.IsValidTextTypeId) => ResolveValidTextTypeId(),
            nameof(ViewSchedule.GetDefaultNameForKeySchedule) => ResolveDefaultNameForKeySchedule(),
            nameof(ViewSchedule.GetDefaultNameForMaterialTakeoff) => ResolveDefaultNameForMaterialTakeoff(),
            nameof(ViewSchedule.GetDefaultNameForSchedule) => ResolveDefaultNameForSchedule(),
            nameof(ViewSchedule.GetDefaultParameterNameForKeySchedule) => ResolveDefaultParameterNameForKeySchedule(),
            nameof(ViewSchedule.IsValidCategoryForKeySchedule) => ResolveIsValidCategoryForKeySchedule(),
            nameof(ViewSchedule.IsValidCategoryForMaterialTakeoff) => ResolveIsValidCategoryForMaterialTakeoff(),
            nameof(ViewSchedule.IsValidCategoryForSchedule) => ResolveIsValidCategoryForSchedule(),
            nameof(ViewSchedule.GetDefaultNameForKeynoteLegend) => ResolveSet.Append(ViewSchedule.GetDefaultNameForKeynoteLegend(context)),
            nameof(ViewSchedule.GetDefaultNameForNoteBlock) => ResolveSet.Append(ViewSchedule.GetDefaultNameForNoteBlock(context)),
            nameof(ViewSchedule.GetDefaultNameForRevisionSchedule) => ResolveSet.Append(ViewSchedule.GetDefaultNameForRevisionSchedule(context)),
            nameof(ViewSchedule.GetDefaultNameForSheetList) => ResolveSet.Append(ViewSchedule.GetDefaultNameForSheetList(context)),
            nameof(ViewSchedule.GetDefaultNameForViewList) => ResolveSet.Append(ViewSchedule.GetDefaultNameForViewList(context)),
            nameof(ViewSchedule.GetValidFamiliesForNoteBlock) => ResolveSet.Append(ViewSchedule.GetValidFamiliesForNoteBlock(context)),
#if REVIT2022_OR_GREATER
            nameof(ViewSchedule.GetScheduleInstances) => ResolveScheduleInstances(),
            nameof(ViewSchedule.GetSegmentHeight) => ResolveSegmentHeight(),
#endif
            _ => null
        };
        
        ResolveSet ResolveStripedRowsColor()
        {
            var values = Enum.GetValues(typeof(StripedRowPattern));
            var resolveSummary = new ResolveSet(values.Length);
            
            foreach (StripedRowPattern pattern in values)
            {
                resolveSummary.AppendVariant(viewSchedule.GetStripedRowsColor(pattern), pattern.ToString());
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveValidTextTypeId()
        {
            var types = context.EnumerateTypes<TextNoteType>().ToArray();
            var resolveSummary = new ResolveSet(types.Length);
            
            foreach (var type in types)
            {
                var result = viewSchedule.IsValidTextTypeId(type.Id);
                resolveSummary.AppendVariant(result, $"{type.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveDefaultNameForKeySchedule()
        {
            var categories = ViewSchedule.GetValidCategoriesForKeySchedule();
            var resolveSummary = new ResolveSet(categories.Count);
            foreach (var id in categories)
            {
                resolveSummary.AppendVariant(ViewSchedule.GetDefaultNameForKeySchedule(context, id));
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveDefaultNameForMaterialTakeoff()
        {
            var categories = ViewSchedule.GetValidCategoriesForMaterialTakeoff();
            var resolveSummary = new ResolveSet(categories.Count);
            foreach (var id in categories)
            {
                resolveSummary.AppendVariant(ViewSchedule.GetDefaultNameForMaterialTakeoff(context, id));
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveDefaultNameForSchedule()
        {
            var categories = ViewSchedule.GetValidCategoriesForSchedule();
            var resolveSummary = new ResolveSet(categories.Count);
            foreach (var id in categories)
            {
                if (id == new ElementId(BuiltInCategory.OST_Areas)) continue;
                resolveSummary.AppendVariant(ViewSchedule.GetDefaultNameForSchedule(context, id));
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveDefaultParameterNameForKeySchedule()
        {
            var categories = ViewSchedule.GetValidCategoriesForKeySchedule();
            var resolveSummary = new ResolveSet(categories.Count);
            foreach (var id in categories)
            {
                if (id == new ElementId(BuiltInCategory.OST_Areas)) continue;
                resolveSummary.AppendVariant(ViewSchedule.GetDefaultParameterNameForKeySchedule(context, id));
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsValidCategoryForKeySchedule()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                var result = ViewSchedule.IsValidCategoryForKeySchedule(category.Id);
                resolveSummary.AppendVariant(result, $"{category.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsValidCategoryForMaterialTakeoff()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                var result = ViewSchedule.IsValidCategoryForMaterialTakeoff(category.Id);
                resolveSummary.AppendVariant(result, $"{category.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsValidCategoryForSchedule()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                var result = ViewSchedule.IsValidCategoryForSchedule(category.Id);
                resolveSummary.AppendVariant(result, $"{category.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
#if REVIT2022_OR_GREATER
        ResolveSet ResolveScheduleInstances()
        {
            var count = viewSchedule.GetSegmentCount();
            var resolveSummary = new ResolveSet(count);
            
            for (var i = 0; i<count; i++)
            {
                resolveSummary.AppendVariant(viewSchedule.GetScheduleInstances(i));
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveSegmentHeight()
        {
            var count = viewSchedule.GetSegmentCount();
            var resolveSummary = new ResolveSet(count);
            
            for (var i = 0; i<count; i++)
            {
                resolveSummary.AppendVariant(viewSchedule.GetSegmentHeight(i));
            }
            
            return resolveSummary;
        }
#endif  
    }
}