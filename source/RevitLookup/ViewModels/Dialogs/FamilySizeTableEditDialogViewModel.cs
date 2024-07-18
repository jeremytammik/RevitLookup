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

using System.Data;
using System.IO;
using System.Text;
using RevitLookup.Core;

namespace RevitLookup.ViewModels.Dialogs;

public sealed class FamilySizeTableEditDialogViewModel : DataTable
{
    private readonly FamilySizeTableManager _manager;
    private readonly Document _document;
    private readonly string _tableName;
    
    public FamilySizeTableEditDialogViewModel(Document document, FamilySizeTableManager manager, string tableName)
    {
        _manager = manager;
        _tableName = tableName;
        _document = document;
        
        var table = manager.GetSizeTable(tableName);
        CreateColumns(table);
        WriteRows(table);
    }
    
    public FamilySizeTableEditDialogViewModel(Document document, FamilySizeTable table)
    {
        _document = document;
        CreateColumns(table);
        WriteRows(table);
    }
    
    private void WriteRows(FamilySizeTable table)
    {
        for (var i = 0; i < table.NumberOfRows; i++)
        {
            var rowArray = new object[table.NumberOfColumns];
            for (var j = 0; j < table.NumberOfColumns; j++)
            {
                rowArray[j] = table.AsValueString(i, j);
            }
            
            Rows.Add(rowArray);
        }
    }
    
    private void CreateColumns(FamilySizeTable table)
    {
        for (var i = 0; i < table.NumberOfColumns; i++)
        {
            var header = table.GetColumnHeader(i);
            var headerName = table.GetColumnHeader(i).Name;
            var columnName = headerName;
            
            if (i == 0)
            {
                columnName = "Description";
            }
            else
            {
                var specId = header.GetSpecTypeId();
                var typeId = table.GetColumnHeader(i).GetUnitTypeId();
                
                if (!specId.Empty())
                {
                    columnName = $"{columnName}##{specId.ToSpecLabel().ToLowerInvariant()}";
                }
                
                columnName = !typeId.Empty() ? $"{columnName}##{typeId.ToUnitLabel().ToLowerInvariant()}" : $"{columnName}##other##";
            }
            
            Columns.Add(new DataColumn(columnName, typeof(string)));
        }
    }
    
    public void SaveData()
    {
        var tableFolder = Path.GetTempPath();
        var tablePath = Path.Combine(tableFolder, $"{_tableName}.csv");
        
        var headerBuilder = new StringBuilder();
        using var writer = new StreamWriter(tablePath, false, Encoding.Unicode);
        for (var i = 1; i < Columns.Count; i++)
        {
            var column = Columns[i];
            var name = column.ColumnName.Replace(' ', '_');
            headerBuilder.Append(',');
            headerBuilder.Append(name);
        }
        
        writer.WriteLine(headerBuilder);
        foreach (DataRow row in Rows)
        {
            var result = new StringBuilder();
            foreach (var value in row.ItemArray)
            {
                if (value is null) continue;
                
                var recordValue = value.ToString();
                if (recordValue!.Contains(','))
                {
                    recordValue = $"\"{recordValue}\"";
                }
                
                result.Append(recordValue);
                result.Append(',');
            }
            
            result.Remove(result.Length - 1, 1);
            writer.WriteLine(result);
        }
        
        writer.Close();
        
        RevitShell.ActionEventHandler.Raise(_ =>
        {
            try
            {
                using var transaction = new Transaction(_document, "Import size table");
                transaction.Start();
                var errorInfo = new FamilySizeTableErrorInfo();
                _manager.ImportSizeTable(_document, tablePath, errorInfo);
                transaction.Commit();
            }
            
            catch
            {
                // ignored
            }
            finally
            {
                File.Delete(tablePath);
            }
        });
    }
    
    public void DeleteRow(DataRow row)
    {
        Rows.Remove(row);
    }
    
    public void DuplicateRow(DataRow row)
    {
        var index = Rows.IndexOf(row);
        var newRow = NewRow();
        newRow.ItemArray = (object[]) row.ItemArray.Clone();
        Rows.InsertAt(newRow, index + 1);
    }
}