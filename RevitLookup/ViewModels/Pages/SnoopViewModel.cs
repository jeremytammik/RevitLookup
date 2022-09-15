// Copyright 2003-2022 by Autodesk, Inc.
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

using CommunityToolkit.Mvvm.ComponentModel;
using RevitLookup.Core;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.ViewModels.Pages;

public sealed class SnoopViewModel : ObservableObject, ISnoopService
{
    private IReadOnlyList<SnoopableObject> _snoopableObjects;

    public IReadOnlyList<SnoopableObject> SnoopableObjects
    {
        get => _snoopableObjects;
        private set
        {
            if (Equals(value, _snoopableObjects)) return;
            _snoopableObjects = value;
            OnPropertyChanged();
        }
    }

    public void Snoop(SnoopableType type)
    {
        SnoopableObjects = type switch
        {
            SnoopableType.Selection => Snooper.SnoopSelection(),
            SnoopableType.Application => Snooper.SnoopApplication(),
            SnoopableType.Document => Snooper.SnoopDocument(),
            SnoopableType.View => Snooper.SnoopView(),
            SnoopableType.Database => Snooper.SnoopDatabase(),
            SnoopableType.Face => Snooper.SnoopFace(),
            SnoopableType.Edge => Snooper.SnoopEdge(),
            SnoopableType.LinkedElement => Snooper.SnoopLinkedElement(),
            SnoopableType.DependentElements => Snooper.SnoopDependentElements(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}