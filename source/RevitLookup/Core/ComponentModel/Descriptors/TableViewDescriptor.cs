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
using Autodesk.Revit.DB.Electrical;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class TableViewDescriptor(TableView tableView) : ElementDescriptor(tableView)
{
    public override Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            // nameof(TableView.GetAvailableParameterCategories) => ResolveAvailableParameterCategories, disabled, long computation time
            nameof(TableView.GetAvailableParameters) => ResolveAvailableParameters,
            nameof(TableView.GetCalculatedValueName) => ResolveCalculatedValueName,
            nameof(TableView.GetCalculatedValueText) => ResolveCalculatedValueText,
            nameof(TableView.IsValidSectionType) => ResolveIsValidSectionType,
            nameof(TableView.GetCellText) => ResolveCellText,
            _ => null
        };
        
        IVariants ResolveAvailableParameters()
        {
            var categories = context.Settings.Categories;
            var variants = new Variants<IList<ElementId>>(categories.Size);
            foreach (Category category in categories)
            {
                var result = TableView.GetAvailableParameters(context, category.Id);
                variants.Add(result, $"{category.Name}");
            }
            
            return variants;
        }
        
        IVariants ResolveCalculatedValueName()
        {
            var tableData = tableView switch
            {
                ViewSchedule viewSchedule => viewSchedule.GetTableData(),
                PanelScheduleView panelScheduleView => panelScheduleView.GetTableData(),
                _ => throw new NotSupportedException($"{tableView.GetType().FullName} is not supported in the current API version")
            };
            
            var sectionTypes = Enum.GetValues(typeof(SectionType));
            var variants = new Variants<string>(sectionTypes.Length);
            foreach (SectionType sectionType in sectionTypes)
            {
                var tableSectionData = tableData!.GetSectionData(sectionType);
                if (tableSectionData is null) continue;
                
                for (var i = tableSectionData.FirstRowNumber; i < tableSectionData.LastRowNumber; i++)
                for (var j = tableSectionData.FirstColumnNumber; j < tableSectionData.LastColumnNumber; j++)
                {
                    var result = tableView.GetCalculatedValueName(sectionType, i, j);
                    variants.Add(result, $"{sectionType}, row {i}, column {j}: {result}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveCalculatedValueText()
        {
            var tableData = tableView switch
            {
                ViewSchedule viewSchedule => viewSchedule.GetTableData(),
                PanelScheduleView panelScheduleView => panelScheduleView.GetTableData(),
                _ => throw new NotSupportedException($"{tableView.GetType().FullName} is not supported in the current API version")
            };
            
            var sectionTypes = Enum.GetValues(typeof(SectionType));
            var variants = new Variants<string>(sectionTypes.Length);
            foreach (SectionType sectionType in sectionTypes)
            {
                var tableSectionData = tableData!.GetSectionData(sectionType);
                if (tableSectionData is null) continue;
                
                for (var i = tableSectionData.FirstRowNumber; i < tableSectionData.LastRowNumber; i++)
                for (var j = tableSectionData.FirstColumnNumber; j < tableSectionData.LastColumnNumber; j++)
                {
                    var result = tableView.GetCalculatedValueText(sectionType, i, j);
                    variants.Add(result, $"{sectionType}, row {i}, column {j}: {result}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveCellText()
        {
            var tableData = tableView switch
            {
                ViewSchedule viewSchedule => viewSchedule.GetTableData(),
                PanelScheduleView panelScheduleView => panelScheduleView.GetTableData(),
                _ => throw new NotSupportedException($"{tableView.GetType().FullName} is not supported in the current API version")
            };
            
            var sectionTypes = Enum.GetValues(typeof(SectionType));
            var variants = new Variants<string>(sectionTypes.Length);
            foreach (SectionType sectionType in sectionTypes)
            {
                var tableSectionData = tableData!.GetSectionData(sectionType);
                if (tableSectionData is null) continue;
                for (var i = tableSectionData.FirstRowNumber; i < tableSectionData.LastRowNumber; i++)
                for (var j = tableSectionData.FirstColumnNumber; j < tableSectionData.LastColumnNumber; j++)
                {
                    var result = tableView.GetCellText(sectionType, i, j);
                    variants.Add(result, $"{sectionType}, row {i}, column {j}: {result}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveIsValidSectionType()
        {
            var sectionTypes = Enum.GetValues(typeof(SectionType));
            var variants = new Variants<bool>(sectionTypes.Length);
            foreach (SectionType sectionType in sectionTypes)
            {
                var result = tableView.IsValidSectionType(sectionType);
                variants.Add(result, $"{sectionType}: {result}");
            }
            
            return variants;
        }
    }
}