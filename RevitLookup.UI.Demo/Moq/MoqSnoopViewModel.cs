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

using Autodesk.Revit.DB;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Enums;
using RevitLookup.UI.Common;
using RevitLookup.UI.Contracts;
using RevitLookup.UI.Controls;
using RevitLookup.ViewModels.Contracts;

namespace RevitLookup.UI.Demo.Moq;

public sealed partial class MoqSnoopViewModel : ObservableObject, ISnoopViewModel
{
    private readonly ISnackbarService _snackbarService;
    [ObservableProperty] private string _searchText;
    [ObservableProperty] private IReadOnlyList<Descriptor> _snoopableData = Array.Empty<Descriptor>();
    [ObservableProperty] private IReadOnlyList<SnoopableObject> _snoopableObjects = Array.Empty<SnoopableObject>();

    public MoqSnoopViewModel(ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
    }

    public void Snoop(SnoopableObject snoopableObject)
    {
        if (snoopableObject.Descriptor is IDescriptorEnumerator {IsEmpty: false} descriptorEnumerator)
        {
            var objects = new List<SnoopableObject>();
            var enumerator = descriptorEnumerator.GetEnumerator();
            while (enumerator.MoveNext())
                objects.Add(new SnoopableObject(snoopableObject.Context, enumerator.Current));

            SnoopableObjects = objects;
        }
        else
        {
            SnoopableObjects = new[] {snoopableObject};
        }
    }

    public async Task Snoop(SnoopableType snoopableType)
    {
        SnoopableData = Array.Empty<Descriptor>();

        int generationCount;
        switch (snoopableType)
        {
            case SnoopableType.Selection:
                generationCount = 10_000;
                break;
            case SnoopableType.View:
                generationCount = 1_000;
                break;
            case SnoopableType.Document:
                generationCount = 100;
                break;
            case SnoopableType.Application:
                generationCount = 50;
                break;
            case SnoopableType.Database:
                generationCount = 10;
                break;
            case SnoopableType.LinkedElement:
                generationCount = 5;
                break;
            case SnoopableType.Face:
                generationCount = 2;
                break;
            case SnoopableType.Edge:
                generationCount = 1;
                break;
            case SnoopableType.DependentElements:
                SnoopableObjects = Array.Empty<SnoopableObject>();
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(snoopableType), snoopableType, null);
        }

        SnoopableObjects = await Task.Run(() =>
        {
            return new Faker<SnoopableObject>()
                .CustomInstantiator(faker =>
                {
                    if (faker.IndexFaker == 0)
                        return new SnoopableObject(null, faker.Lorem.Word());
                    if (faker.IndexFaker % 1000 == 0)
                        return new SnoopableObject(null, new {Collection = faker.Make(20, () => faker.Internet.UserName())});
                    if (faker.IndexFaker % 500 == 0)
                        return new SnoopableObject(null, null);
                    if (faker.IndexFaker % 200 == 0)
                        return new SnoopableObject(null, string.Empty);
                    if (faker.IndexFaker % 100 == 0)
                        return new SnoopableObject(null, new Color(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte()));
                    if (faker.IndexFaker % 5 == 0)
                        return new SnoopableObject(null, faker.Random.Int(0));
                    if (faker.IndexFaker % 3 == 0)
                        return new SnoopableObject(null, faker.Random.Bool());

                    return new SnoopableObject(null, faker.Lorem.Word());
                })
                .Generate(generationCount);
        });
    }

    [RelayCommand]
    private async Task CollectMembersAsync(SnoopableObject snoopableObject)
    {
        try
        {
            // ReSharper disable once MethodHasAsyncOverload
            SnoopableData = snoopableObject.GetMembers();
        }
        catch (Exception exception)
        {
            await _snackbarService.ShowAsync("Snoop engine error", exception.Message, SymbolRegular.ErrorCircle24, ControlAppearance.Danger);
        }
    }
}