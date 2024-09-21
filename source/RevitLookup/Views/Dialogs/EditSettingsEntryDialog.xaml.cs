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

using RevitLookup.ViewModels.ObservableObjects;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.Views.Dialogs;

public sealed partial class EditSettingsEntryDialog
{
    public EditSettingsEntryDialog(IContentDialogService dialogService) : base(dialogService.GetDialogHost())
    {
        InitializeComponent();
    }

    public ObservableRevitSettingsEntry Entry { get; private set; }

    public async Task<ContentDialogResult> ShowCreateDialogAsync(ObservableRevitSettingsEntry selectedEntry)
    {
        Title = "Create the entry";
        PrimaryButtonText = "Create";

        Entry = new ObservableRevitSettingsEntry
        {
            IsActive = true
        };

        if (selectedEntry is not null)
        {
            Entry.Category = selectedEntry.Category;
        }

        DataContext = Entry;
        return await ShowAsync();
    }

    public async Task<ContentDialogResult> ShowUpdateDialogAsync(ObservableRevitSettingsEntry entry)
    {
        Title = "Update the entry";
        PrimaryButtonText = "Update";

        Entry = entry;
        DataContext = entry;
        return await ShowAsync();
    }

    protected override void OnButtonClick(ContentDialogButton button)
    {
        if (button == ContentDialogButton.Primary)
        {
            Entry.Validate();
            if (Entry.HasErrors)
            {
                return;
            }
        }
        
        base.OnButtonClick(button);
    }
}