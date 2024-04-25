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

public class ScheduleDefinitionDescriptor(ScheduleDefinition scheduleDefinition) : Descriptor, IDescriptorResolver
{
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
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
        
        ResolveSet ResolveFilterByGlobalParameters()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var resolveSummary = new ResolveSet(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByGlobalParameters(field);
                var name = scheduleDefinition.GetField(field).GetName();
                resolveSummary.AppendVariant(result, $"{name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveFilterByParameterExistence()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var resolveSummary = new ResolveSet(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByParameterExistence(field);
                var name = scheduleDefinition.GetField(field).GetName();
                resolveSummary.AppendVariant(result, $"{name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveFilterBySubstring()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var resolveSummary = new ResolveSet(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterBySubstring(field);
                var name = scheduleDefinition.GetField(field).GetName();
                resolveSummary.AppendVariant(result, $"{name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveFilterByValue()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var resolveSummary = new ResolveSet(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByValue(field);
                var name = scheduleDefinition.GetField(field).GetName();
                resolveSummary.AppendVariant(result, $"{name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveFilterByValuePresence()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var resolveSummary = new ResolveSet(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByValuePresence(field);
                var name = scheduleDefinition.GetField(field).GetName();
                resolveSummary.AppendVariant(result, $"{name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveSortByField()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var resolveSummary = new ResolveSet(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanSortByField(field);
                var name = scheduleDefinition.GetField(field).GetName();
                resolveSummary.AppendVariant(result, $"{name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveGetField()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var resolveSummary = new ResolveSet(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.GetField(field);
                resolveSummary.AppendVariant(result, $"{result.GetName()}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveGetFieldId()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var resolveSummary = new ResolveSet(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.GetFieldId(field.IntegerValue);
                var name = scheduleDefinition.GetField(field).GetName();
                resolveSummary.AppendVariant(result, $"{name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveGetFieldIndex()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var resolveSummary = new ResolveSet(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.GetFieldIndex(field);
                var name = scheduleDefinition.GetField(field).GetName();
                resolveSummary.AppendVariant(result, $"{name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveGetFilter()
        {
            var count = scheduleDefinition.GetFilterCount();
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                resolveSummary.AppendVariant(scheduleDefinition.GetFilter(i));
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveGetSortGroupField()
        {
            var count = scheduleDefinition.GetSortGroupFieldCount();
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                resolveSummary.AppendVariant(scheduleDefinition.GetSortGroupField(i));
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveValidCategoryForEmbeddedSchedule()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                if (scheduleDefinition.IsValidCategoryForEmbeddedSchedule(category.Id))
                    resolveSummary.AppendVariant(true, category.Name);
            }
            
            return resolveSummary;
        }
    }
}