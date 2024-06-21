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


using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using RevitLookup.Core.ComponentModel.Descriptors;
using RevitLookup.Views.Dialogs;

namespace RevitLookup.ViewModels.Dialogs;

public sealed partial class FamilySizeTableSelectDialogViewModel : ObservableObject
{
    private readonly FamilySizeTableManager _manager;
    [ObservableProperty] private string _selectedTable;
    public List<string> Tables { get; }
    public FamilySizeTableSelectDialogViewModel(FamilySizeTableManager manager)
    {
        _manager = manager;
        Tables = manager.GetAllSizeTableNames().ToList();
        SelectedTable = Tables.First(); // User can not run this command if manager has no tables
    }
    
    public void Export()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "CSV files (*.csv)|*.csv",
            FileName = SelectedTable,
            RestoreDirectory = true,
            Title = "Save family size table",
        };
        if (saveFileDialog.ShowDialog() == false) return;
        
        _manager.ExportSizeTable(SelectedTable, saveFileDialog.FileName);
    }
    
    public async Task ShowEditDialogAsync(IServiceProvider serviceProvider)
    {
        try
        {
            var dialog = new FamilySizeTableShowDialog(serviceProvider, _manager, SelectedTable);
            await dialog.ShowAsync();
        }
        catch (Exception exception)
        {
            var logger = serviceProvider.GetService<ILogger<FamilySizeTableManagerDescriptor>>();
            logger.LogError(exception, "FamilySizeTableDialog error");
        }
    }
}