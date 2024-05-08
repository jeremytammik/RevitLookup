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

using System.Globalization;
using System.Reflection;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class TableSectionDataDescriptor(TableSectionData tableSectionData) : Descriptor, IDescriptorResolver
{
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(TableSectionData.AllowOverrideCellStyle) => ResolveOverrideCellStyle(),
            nameof(TableSectionData.CanInsertColumn) => ResolveCanInsertColumn(),
            nameof(TableSectionData.CanInsertRow) => ResolveCanInsertRow(),
            nameof(TableSectionData.CanRemoveColumn) => ResolveCanRemoveColumn(),
            nameof(TableSectionData.CanRemoveRow) => ResolveCanRemoveRow(),
            nameof(TableSectionData.GetCellCalculatedValue) when parameters.Length == 1 => ResolveCellCalculatedValueForColumns(),
            nameof(TableSectionData.GetCellCalculatedValue) when parameters.Length == 2 => ResolveCellCalculatedValueForTable(),
            nameof(TableSectionData.GetCellCategoryId) when parameters.Length == 1 => ResolveCellCategoryIdForColumns(),
            nameof(TableSectionData.GetCellCategoryId) when parameters.Length == 2 => ResolveCellCategoryIdForTable(),
            nameof(TableSectionData.GetCellCombinedParameters) when parameters.Length == 1 => ResolveCellCombinedParametersForColumns(),
            nameof(TableSectionData.GetCellCombinedParameters) when parameters.Length == 2 => ResolveCellCombinedParametersForTable(),
            nameof(TableSectionData.GetCellFormatOptions) when parameters.Length == 2 => ResolveCellFormatOptionsForColumns(),
            nameof(TableSectionData.GetCellFormatOptions) when parameters.Length == 3 => ResolveCellFormatOptionsForTable(),
            nameof(TableSectionData.GetCellParamId) when parameters.Length == 1 => ResolveCellParamIdForColumns(),
            nameof(TableSectionData.GetCellParamId) when parameters.Length == 2 => ResolveCellParamIdForTable(),
            nameof(TableSectionData.GetCellSpec) => ResolveCellSpec(),
            nameof(TableSectionData.GetCellText) => ResolveCellText(),
            nameof(TableSectionData.GetCellType) when parameters.Length == 1 => ResolveCellTypeForColumns(),
            nameof(TableSectionData.GetCellType) when parameters.Length == 2 => ResolveCellTypeForTable(),
            nameof(TableSectionData.GetColumnWidth) => ResolveColumnWidth(),
            nameof(TableSectionData.GetColumnWidthInPixels) => ResolveColumnWidthInPixels(),
            nameof(TableSectionData.GetMergedCell) => ResolveMergedCell(),
            nameof(TableSectionData.GetRowHeight) => ResolveRowHeight(),
            nameof(TableSectionData.GetRowHeightInPixels) => ResolveRowHeightInPixels(),
            nameof(TableSectionData.GetTableCellStyle) => ResolveTableCellStyle(),
            nameof(TableSectionData.IsCellFormattable) => ResolveIsCellFormattable(),
            nameof(TableSectionData.IsCellOverridden) when parameters.Length == 1 => ResolveIsCellOverriddenForColumns(),
            nameof(TableSectionData.IsCellOverridden) when parameters.Length == 2 => ResolveIsCellOverriddenForTable(),
            nameof(TableSectionData.IsValidColumnNumber) => ResolveIsValidColumnNumber(),
            nameof(TableSectionData.IsValidRowNumber) => ResolveIsValidRowNumber(),
            nameof(TableSectionData.RefreshData) => ResolveSet.Append(false, "Method execution disabled"),
            _ => null
        };
        
        ResolveSet ResolveOverrideCellStyle()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < rowsNumber; i++)
            {
                for (var j = 0; j < columnsNumber; j++)
                {
                    var result = tableSectionData.AllowOverrideCellStyle(i, j);
                    resolveSummary.AppendVariant(result, $"Row {i}, Column {j}: {result}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCanInsertColumn()
        {
            var count = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanInsertColumn(i);
                resolveSummary.AppendVariant(result, $"{i}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCanInsertRow()
        {
            var count = tableSectionData.NumberOfRows;
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanInsertRow(i);
                resolveSummary.AppendVariant(result, $"{i}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCanRemoveColumn()
        {
            var count = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanRemoveColumn(i);
                resolveSummary.AppendVariant(result, $"{i}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCanRemoveRow()
        {
            var count = tableSectionData.NumberOfRows;
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanRemoveRow(i);
                resolveSummary.AppendVariant(result, $"{i}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellCalculatedValueForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(columnsNumber);
            for (var j = 0; j < columnsNumber; j++)
            {
                var result = tableSectionData.GetCellCalculatedValue(j);
                resolveSummary.AppendVariant(result, $"Column {j}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellCalculatedValueForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < rowsNumber; i++)
            {
                for (var j = 0; j < columnsNumber; j++)
                {
                    var result = tableSectionData.GetCellCalculatedValue(i, j);
                    resolveSummary.AppendVariant(result, $"Row {i}, Column {j}: {result}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellCategoryIdForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellCategoryId(i);
                if (result == ElementId.InvalidElementId) continue;
                
                var category = Category.GetCategory(context, result);
                if (category is null) continue;
                
                resolveSummary.AppendVariant(result, $"Column {i}: {category.Name}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellCategoryIdForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellCategoryId(j, i);
                    if (result == ElementId.InvalidElementId) continue;
                    
                    var category = Category.GetCategory(context, result);
                    if (category is null) continue;
                    
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}: {category.Name}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellCombinedParametersForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellCombinedParameters(i);
                resolveSummary.AppendVariant(result, $"Column {i}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellCombinedParametersForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellCombinedParameters(j, i);
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellFormatOptionsForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellFormatOptions(i, context);
                resolveSummary.AppendVariant(result, $"Column {i}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellFormatOptionsForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellFormatOptions(j, i, context);
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellParamIdForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellParamId(i);
                if (result != ElementId.InvalidElementId)
                {
                    var parameter = result.ToElement(context);
                    resolveSummary.AppendVariant(result, $"Column {i}: {parameter!.Name}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellParamIdForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellParamId(j, i);
                    if (result == ElementId.InvalidElementId) continue;
                    
                    var parameter = result.ToElement(context);
                    if (parameter is null) continue;
                    
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}: {parameter.Name}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellSpec()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellSpec(j, i);
                    if (result.Empty()) continue;
                    
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}: {result.ToSpecLabel()}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellText()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellText(j, i);
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}: {result}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellTypeForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellType(i);
                resolveSummary.AppendVariant(result, $"Column {i}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellTypeForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellType(j, i);
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}: {result}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveColumnWidth()
        {
            var count = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetColumnWidth(i);
                resolveSummary.AppendVariant(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }
            
            return resolveSummary;
        }
        
        
        ResolveSet ResolveColumnWidthInPixels()
        {
            var count = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetColumnWidthInPixels(i);
                resolveSummary.AppendVariant(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveMergedCell()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetMergedCell(j, i);
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveRowHeight()
        {
            var count = tableSectionData.NumberOfRows;
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetRowHeight(i);
                resolveSummary.AppendVariant(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveRowHeightInPixels()
        {
            var count = tableSectionData.NumberOfRows;
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetRowHeightInPixels(i);
                resolveSummary.AppendVariant(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveTableCellStyle()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetTableCellStyle(j, i);
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsCellFormattable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.IsCellFormattable(j, i);
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}: {result}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsCellOverriddenForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.IsCellOverridden(i);
                resolveSummary.AppendVariant(result, $"Column {i}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsCellOverriddenForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.IsCellOverridden(j, i);
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}: {result}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsValidColumnNumber()
        {
            var count = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.IsValidColumnNumber(i);
                resolveSummary.AppendVariant(result, $"{i}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsValidRowNumber()
        {
            var count = tableSectionData.NumberOfRows;
            var resolveSummary = new ResolveSet(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.IsValidRowNumber(i);
                resolveSummary.AppendVariant(result, $"{i}: {result}");
            }
            
            return resolveSummary;
        }
    }
}