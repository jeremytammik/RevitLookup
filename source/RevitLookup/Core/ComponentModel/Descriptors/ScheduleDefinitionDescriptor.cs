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

public sealed class ScheduleDefinitionDescriptor(ScheduleDefinition scheduleDefinition) : Descriptor, IDescriptorResolver
{
    public IVariants Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(ScheduleDefinition.CanFilterByGlobalParameters) => ResolveFilterByGlobalParameters(),
            nameof(ScheduleDefinition.CanFilterByParameterExistence) => ResolveFilterByParameterExistence(),
            nameof(ScheduleDefinition.CanFilterBySubstring) => ResolveFilterBySubstring(),
            nameof(ScheduleDefinition.CanFilterByValue) => ResolveFilterByValue(),
            nameof(ScheduleDefinition.CanFilterByValuePresence) => ResolveFilterByValuePresence(),
            nameof(ScheduleDefinition.CanSortByField) => ResolveSortByField(),
            nameof(ScheduleDefinition.GetField) => ResolveGetField(),
            nameof(ScheduleDefinition.GetFieldId) => ResolveGetFieldId(),
            nameof(ScheduleDefinition.GetFieldIndex) => ResolveGetFieldIndex(),
            nameof(ScheduleDefinition.GetFilter) => ResolveGetFilter(),
            nameof(ScheduleDefinition.GetSortGroupField) => ResolveGetSortGroupField(),
            nameof(ScheduleDefinition.IsValidCategoryForEmbeddedSchedule) => ResolveValidCategoryForEmbeddedSchedule(),
            _ => null
        };
        
        IVariants ResolveFilterByGlobalParameters()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = new Variants<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByGlobalParameters(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveFilterByParameterExistence()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = new Variants<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByParameterExistence(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveFilterBySubstring()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = new Variants<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterBySubstring(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveFilterByValue()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = new Variants<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByValue(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveFilterByValuePresence()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = new Variants<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByValuePresence(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveSortByField()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = new Variants<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanSortByField(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetField()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = new Variants<ScheduleField>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.GetField(field);
                variants.Add(result, $"{result.GetName()}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetFieldId()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = new Variants<ScheduleFieldId>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.GetFieldId(field.IntegerValue);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetFieldIndex()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = new Variants<int>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.GetFieldIndex(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetFilter()
        {
            var count = scheduleDefinition.GetFilterCount();
            var variants = new Variants<ScheduleFilter>(count);
            for (var i = 0; i < count; i++)
            {
                variants.Add(scheduleDefinition.GetFilter(i));
            }
            
            return variants;
        }
        
        IVariants ResolveGetSortGroupField()
        {
            var count = scheduleDefinition.GetSortGroupFieldCount();
            var variants = new Variants<ScheduleSortGroupField>(count);
            for (var i = 0; i < count; i++)
            {
                variants.Add(scheduleDefinition.GetSortGroupField(i));
            }
            
            return variants;
        }
        
        IVariants ResolveValidCategoryForEmbeddedSchedule()
        {
            var categories = context.Settings.Categories;
            var variants = new Variants<bool>(categories.Size);
            foreach (Category category in categories)
            {
                if (scheduleDefinition.IsValidCategoryForEmbeddedSchedule(category.Id))
                {
                    variants.Add(true, category.Name);
                }
            }
            
            return variants;
        }
    }
}