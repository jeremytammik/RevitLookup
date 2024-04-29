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

public class TableSectionDataDescriptor(TableSectionData tableSectionData) : Descriptor, IDescriptorResolver
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
            nameof(TableSectionData.GetCellCalculatedValue) => ResolveCellCalculatedValue(),
            nameof(TableSectionData.GetCellCategoryId) => ResolveCellCategoryId(),
            nameof(TableSectionData.GetCellCombinedParameters) => ResolveCellCombinedParameters(),
            nameof(TableSectionData.GetCellFormatOptions) => ResolveCellFormatOptions(),
            nameof(TableSectionData.GetCellParamId) => ResolveCellParamId(),
            nameof(TableSectionData.GetCellSpec) => ResolveCellSpec(),
            nameof(TableSectionData.GetCellText) => ResolveCellText(),
            nameof(TableSectionData.GetCellType) => ResolveCellType(),
            nameof(TableSectionData.GetRowHeight) => ResolveRowHeight(),
            nameof(TableSectionData.RefreshData) => ResolveSet.Append(false, "Overridden"),
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
            var resolveSummary = new ResolveSet(count + 2);
            for (var i = 0; i < count + 2; i++)
            {
                var result = tableSectionData.CanInsertColumn(i);
                resolveSummary.AppendVariant(result, $"{i}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCanInsertRow()
        {
            var count = tableSectionData.NumberOfRows;
            var resolveSummary = new ResolveSet(count + 2);
            for (var i = 0; i < count + 2; i++)
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
        
        ResolveSet ResolveCellCalculatedValue()
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
        
        ResolveSet ResolveCellCategoryId()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var columnResult = tableSectionData.GetCellCategoryId(i);
                if (columnResult != ElementId.InvalidElementId)
                {
                    var category = Category.GetCategory(context, columnResult);
                    if (category is not null)
                    {
                        resolveSummary.AppendVariant(columnResult, $"Column {i}: {category.Name}");
                    }
                }
                
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellCategoryId(j, i);
                    if (result != ElementId.InvalidElementId)
                    {
                        var category = Category.GetCategory(context, result);
                        if (category is not null)
                        {
                            resolveSummary.AppendVariant(result, $"Row {j}, Column {i}: {category.Name}");
                        }
                    }
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellCombinedParameters()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var columnResult = tableSectionData.GetCellCombinedParameters(i);
                resolveSummary.AppendVariant(columnResult, $"Column {i}");
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellCombinedParameters(j, i);
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellFormatOptions()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var columnResult = tableSectionData.GetCellFormatOptions(i, context);
                resolveSummary.AppendVariant(columnResult, $"Column {i}");
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellFormatOptions(j, i, context);
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellParamId()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var columnResult = tableSectionData.GetCellParamId(i);
                if (columnResult != ElementId.InvalidElementId)
                {
                    var parameter = columnResult.ToElement(context);
                    resolveSummary.AppendVariant(columnResult, $"Column {i}: {parameter!.Name}");
                }
                
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellParamId(j, i);
                    if (result != ElementId.InvalidElementId)
                    {
                        var parameter = result.ToElement(context);
                        resolveSummary.AppendVariant(result, $"Row {j}, Column {i}: {parameter!.Name}");
                    }
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
                    if (!result.Empty())
                        resolveSummary.AppendVariant(result, $"Row {j}, Column {i}: {result.ToLabel()}");
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
        
        ResolveSet ResolveCellType()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var resolveSummary = new ResolveSet(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var columnResult = tableSectionData.GetCellType(i);
                resolveSummary.AppendVariant(columnResult, $"Column {i}: {columnResult}");
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellType(j, i);
                    resolveSummary.AppendVariant(result, $"Row {j}, Column {i}: {result}");
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
                resolveSummary.AppendVariant(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)} ft");
            }
            
            return resolveSummary;
        }
    }
}