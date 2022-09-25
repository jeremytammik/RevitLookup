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

using System.Collections.ObjectModel;
using System.Windows.Input;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.UI.Tests.Moq;

public sealed class MoqSnoopViewModel : ObservableObject, ISnoopViewModel
{
    private ObservableCollection<SnoopableObject> _snoopableObjects;

    public MoqSnoopViewModel()
    {
        SnoopSelectionCommand = new RelayCommand(SnoopSelection);
    }

    public event EventHandler SelectionChanged;

    public string SnoopedData
    {
        get => _snoopedData;
        private set
        {
            if (value == _snoopedData) return;
            _snoopedData = value;
            OnPropertyChanged();
        }
    }


    public ObservableCollection<SnoopableObject> SnoopableObjects
    {
        get => _snoopableObjects;
        private set
        {
            if (Equals(value, _snoopableObjects)) return;
            _snoopableObjects = value;
            OnPropertyChanged();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public ICommand SnoopSelectionCommand { get; }

    public void SnoopSelection()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100));
    }

    public void SnoopApplication()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100));
    }

    public void SnoopDocument()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100));
    }

    public void SnoopView()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100));
    }

    public void SnoopDatabase()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100));
    }

    public void SnoopEdge()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100));
    }

    public void SnoopFace()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100));
    }

    public void SnoopLinkedElement()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100));
    }

    public void SnoopDependentElements()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100));
    }

    private ICommand _snoopObjectCommand;
    private string _snoopedData;

    [UsedImplicitly]
    public ICommand SnoopObjectCommand => _snoopObjectCommand ??= new RelayCommand<object>(SnoopObject);

    private void SnoopObject(object o)
    {
        if (o is not SnoopableObject snoopableObject) return;
        SnoopedData = snoopableObject.Descriptor.Label;
    }
}