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

namespace RevitLookup.ViewModels.Dialogs;

public sealed class FamilySizeTableDialogViewModel : DataTable
{
    public FamilySizeTableDialogViewModel(FamilySizeTable table)
    {
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
            var typeId = table.GetColumnHeader(i).GetUnitTypeId();
            var headerName = table.GetColumnHeader(i).Name;
            
            var columnName = typeId.Empty() ? headerName : $"{headerName}##{typeId.ToUnitLabel()}";
            Columns.Add(new DataColumn(columnName, typeof(string)));
        }
    }
}