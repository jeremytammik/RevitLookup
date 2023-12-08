﻿// Copyright 2003-2023 by Autodesk, Inc.
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

using CommunityToolkit.Mvvm.Input;
using RevitLookup.Core.Objects;

namespace RevitLookup.ViewModels.Contracts;

public interface ISnoopViewModel
{
    IReadOnlyCollection<SnoopableObject> SnoopableObjects { get; set; }
    IReadOnlyCollection<Descriptor> SnoopableData { get; set; }
    IReadOnlyCollection<SnoopableObject> FilteredSnoopableObjects { get; }
    IReadOnlyCollection<Descriptor> FilteredSnoopableData { get; }
    IAsyncRelayCommand FetchMembersCommand { get; }
    IAsyncRelayCommand RefreshMembersCommand { get; }
    public string SearchText { get; set; }
    public SnoopableObject SelectedObject { get; set; }
    void Navigate(SnoopableObject selectedItem);
    void Navigate(IReadOnlyCollection<SnoopableObject> selectedItems);
}