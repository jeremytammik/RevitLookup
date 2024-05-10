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

public class FamilySizeTableDialogViewModel : DataTable
{
    private readonly FamilySizeTable _table;
    public FamilySizeTableDialogViewModel(FamilySizeTable table, string name)
    {
        _table = table;
        Name = name;
        Initialize();
    }
    
    public string Name { get; set; } 
    
    private void Initialize()
    {
        var columnsCount = _table.NumberOfColumns;
        var rowsCount = _table.NumberOfRows;
        
        for (var i = 0; i < columnsCount; i++)
        {
            Columns.Add(new DataColumn(_table.GetColumnHeader(i).Name));
        }
        
        var unitsArray = new object[columnsCount];
        for (var i = 0; i < columnsCount; i++)
        {
            unitsArray[i] = _table.GetColumnHeader(i).GetUnitTypeId().ToUnitLabel();
        }
        
        Rows.Add(unitsArray);
        
        for (var i = 0; i < rowsCount; i++)
        {
            var rowArray = new object[columnsCount];
            for (var j = 0; j < columnsCount; j++)
            {
                rowArray[j] = _table.AsValueString(i, j);
            }
            
            Rows.Add(rowArray);
        }
    }
}