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

using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.UI.Tests.Moq;

public sealed class MoqSnoopViewModel : ObservableObject, ISnoopViewModel
{
    public MoqSnoopViewModel()
    {
        SnoopSelectionCommand = new RelayCommand(SnoopSelection);
    }

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

    public RelayCommand SnoopSelectionCommand { get; }

    public void SnoopSelection()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Text()))
            .Generate(15);
    }

    public void SnoopApplication()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Text()))
            .Generate(15);
    }

    public void SnoopDocument()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Text()))
            .Generate(15);
    }

    public void SnoopView()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Text()))
            .Generate(15);
    }

    public void SnoopDatabase()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Text()))
            .Generate(15);
    }

    public void SnoopEdge()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Text()))
            .Generate(15);
    }

    public void SnoopFace()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Text()))
            .Generate(15);
    }

    public void SnoopLinkedElement()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Text()))
            .Generate(15);
    }

    public void SnoopDependentElements()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Text()))
            .Generate(15);
    }
}