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
    public IVariants Resolve(Document context, string target, ParameterInfo[] parameters)
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
            nameof(TableSectionData.RefreshData) => Variants.Single(false, "Method execution disabled"),
            _ => null
        };
        
        IVariants ResolveOverrideCellStyle()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<bool>(rowsNumber * columnsNumber);
            for (var i = 0; i < rowsNumber; i++)
            {
                for (var j = 0; j < columnsNumber; j++)
                {
                    var result = tableSectionData.AllowOverrideCellStyle(i, j);
                    variants.Add(result, $"Row {i}, Column {j}: {result}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveCanInsertColumn()
        {
            var count = tableSectionData.NumberOfColumns;
            var variants = new Variants<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanInsertColumn(i);
                variants.Add(result, $"{i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCanInsertRow()
        {
            var count = tableSectionData.NumberOfRows;
            var variants = new Variants<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanInsertRow(i);
                variants.Add(result, $"{i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCanRemoveColumn()
        {
            var count = tableSectionData.NumberOfColumns;
            var variants = new Variants<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanRemoveColumn(i);
                variants.Add(result, $"{i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCanRemoveRow()
        {
            var count = tableSectionData.NumberOfRows;
            var variants = new Variants<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanRemoveRow(i);
                variants.Add(result, $"{i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCellCalculatedValueForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<TableCellCalculatedValueData>(columnsNumber);
            for (var j = 0; j < columnsNumber; j++)
            {
                var result = tableSectionData.GetCellCalculatedValue(j);
                variants.Add(result, $"Column {j}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCellCalculatedValueForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<TableCellCalculatedValueData>(rowsNumber * columnsNumber);
            for (var i = 0; i < rowsNumber; i++)
            {
                for (var j = 0; j < columnsNumber; j++)
                {
                    var result = tableSectionData.GetCellCalculatedValue(i, j);
                    variants.Add(result, $"Row {i}, Column {j}: {result}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveCellCategoryIdForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<ElementId>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellCategoryId(i);
                if (result == ElementId.InvalidElementId) continue;
                
                var category = Category.GetCategory(context, result);
                if (category is null) continue;
                
                variants.Add(result, $"Column {i}: {category.Name}");
            }
            
            return variants;
        }
        
        IVariants ResolveCellCategoryIdForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<ElementId>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellCategoryId(j, i);
                    if (result == ElementId.InvalidElementId) continue;
                    
                    var category = Category.GetCategory(context, result);
                    if (category is null) continue;
                    
                    variants.Add(result, $"Row {j}, Column {i}: {category.Name}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveCellCombinedParametersForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<IList<TableCellCombinedParameterData>>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellCombinedParameters(i);
                variants.Add(result, $"Column {i}");
            }
            
            return variants;
        }
        
        IVariants ResolveCellCombinedParametersForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<IList<TableCellCombinedParameterData>>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellCombinedParameters(j, i);
                    variants.Add(result, $"Row {j}, Column {i}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveCellFormatOptionsForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<FormatOptions>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellFormatOptions(i, context);
                variants.Add(result, $"Column {i}");
            }
            
            return variants;
        }
        
        IVariants ResolveCellFormatOptionsForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<FormatOptions>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellFormatOptions(j, i, context);
                    variants.Add(result, $"Row {j}, Column {i}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveCellParamIdForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<ElementId>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellParamId(i);
                if (result != ElementId.InvalidElementId)
                {
                    var parameter = result.ToElement(context);
                    variants.Add(result, $"Column {i}: {parameter!.Name}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveCellParamIdForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<ElementId>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellParamId(j, i);
                    if (result == ElementId.InvalidElementId) continue;
                    
                    var parameter = result.ToElement(context);
                    if (parameter is null) continue;
                    
                    variants.Add(result, $"Row {j}, Column {i}: {parameter.Name}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveCellSpec()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<ForgeTypeId>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellSpec(j, i);
                    if (result.Empty()) continue;
                    
                    variants.Add(result, $"Row {j}, Column {i}: {result.ToSpecLabel()}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveCellText()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<string>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellText(j, i);
                    variants.Add(result, $"Row {j}, Column {i}: {result}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveCellTypeForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<CellType>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellType(i);
                variants.Add(result, $"Column {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCellTypeForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<CellType>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellType(j, i);
                    variants.Add(result, $"Row {j}, Column {i}: {result}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveColumnWidth()
        {
            var count = tableSectionData.NumberOfColumns;
            var variants = new Variants<double>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetColumnWidth(i);
                variants.Add(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }
            
            return variants;
        }
        
        
        IVariants ResolveColumnWidthInPixels()
        {
            var count = tableSectionData.NumberOfColumns;
            var variants = new Variants<int>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetColumnWidthInPixels(i);
                variants.Add(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }
            
            return variants;
        }
        
        IVariants ResolveMergedCell()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<TableMergedCell>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetMergedCell(j, i);
                    variants.Add(result, $"Row {j}, Column {i}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveRowHeight()
        {
            var count = tableSectionData.NumberOfRows;
            var variants = new Variants<double>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetRowHeight(i);
                variants.Add(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }
            
            return variants;
        }
        
        IVariants ResolveRowHeightInPixels()
        {
            var count = tableSectionData.NumberOfRows;
            var variants = new Variants<int>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetRowHeightInPixels(i);
                variants.Add(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }
            
            return variants;
        }
        
        IVariants ResolveTableCellStyle()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<TableCellStyle>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetTableCellStyle(j, i);
                    variants.Add(result, $"Row {j}, Column {i}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveIsCellFormattable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<bool>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.IsCellFormattable(j, i);
                    variants.Add(result, $"Row {j}, Column {i}: {result}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveIsCellOverriddenForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<bool>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.IsCellOverridden(i);
                variants.Add(result, $"Column {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveIsCellOverriddenForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = new Variants<bool>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.IsCellOverridden(j, i);
                    variants.Add(result, $"Row {j}, Column {i}: {result}");
                }
            }
            
            return variants;
        }
        
        IVariants ResolveIsValidColumnNumber()
        {
            var count = tableSectionData.NumberOfColumns;
            var variants = new Variants<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.IsValidColumnNumber(i);
                variants.Add(result, $"{i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveIsValidRowNumber()
        {
            var count = tableSectionData.NumberOfRows;
            var variants = new Variants<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.IsValidRowNumber(i);
                variants.Add(result, $"{i}: {result}");
            }
            
            return variants;
        }
    }
}