// Copyright 2003-2023 by Autodesk, Inc.
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

using System.Diagnostics;
using Autodesk.Revit.DB;
using Bogus;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Core.Utils;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.ViewModels.Contracts;
using Visibility = System.Windows.Visibility;

namespace RevitLookup.UI.Demo.Services;

public sealed class MoqSnoopVisualService(NotificationService notificationService, ISnoopViewModel viewModel, IWindow window) : ISnoopVisualService
{
    public void Snoop(SnoopableObject snoopableObject)
    {
        try
        {
            if (snoopableObject.Descriptor is IDescriptorEnumerator {IsEmpty: false} descriptor)
            {
                viewModel.SnoopableObjects = descriptor.ParseEnumerable(snoopableObject);
            }
            else
            {
                viewModel.SnoopableObjects = new[] {snoopableObject};
            }
        }
        catch (Exception exception)
        {
            notificationService.ShowError("Invalid object", exception);
        }
    }

    public async Task SnoopAsync(SnoopableType snoopableType)
    {
        switch (snoopableType)
        {
            case SnoopableType.Face:
            case SnoopableType.Edge:
            case SnoopableType.LinkedElement:
            case SnoopableType.Point:
            case SnoopableType.SubElement:
                UpdateWindowVisibility(Visibility.Hidden);
                await Task.Delay(TimeSpan.FromSeconds(2));
                break;
        }

        var generationCount = snoopableType switch
        {
            SnoopableType.View => 50_000,
            SnoopableType.Document => 25_000,
            SnoopableType.Application => 10_000,
            SnoopableType.UiApplication => 5_000,
            SnoopableType.Database => 2_000,
            SnoopableType.DependentElements => 1_000,
            SnoopableType.Selection => 500,
            SnoopableType.LinkedElement => 250,
            SnoopableType.Face => 125,
            SnoopableType.Edge => 60,
            SnoopableType.Point => 30,
            SnoopableType.SubElement => 15,
            SnoopableType.ComponentManager => 8,
            SnoopableType.PerformanceAdviser => 4,
            SnoopableType.UpdaterRegistry => 2,
            SnoopableType.Services => 1,
            SnoopableType.Schemas => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(snoopableType), snoopableType, null)
        };

        viewModel.SnoopableObjects = await GenerateObjectsAsync(generationCount);
        viewModel.SnoopableData = Array.Empty<Descriptor>();
        UpdateWindowVisibility(Visibility.Visible);
    }

    private void UpdateWindowVisibility(Visibility visibility)
    {
        if (!window.IsLoaded) return;

        window.Visibility = visibility;
    }

    private static async Task<IReadOnlyCollection<SnoopableObject>> GenerateObjectsAsync(int generationCount)
    {
        if (generationCount == 0) return Array.Empty<SnoopableObject>();

        return await Task.Run(() => new Faker<SnoopableObject>()
            .CustomInstantiator(faker =>
            {
                if (faker.IndexFaker % 2000 == 0) return new SnoopableObject(null);
                if (faker.IndexFaker % 1000 == 0) return new SnoopableObject(string.Empty);
                if (faker.IndexFaker % 700 == 0) return new SnoopableObject(faker.Make(150, () => faker.Internet.UserName()));
                if (faker.IndexFaker % 500 == 0) return new SnoopableObject(typeof(Debug));
                if (faker.IndexFaker % 200 == 0) return new SnoopableObject(faker.Lorem.Sentence());
                if (faker.IndexFaker % 100 == 0) return new SnoopableObject(new Color(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte()));
                if (faker.IndexFaker % 5 == 0) return new SnoopableObject(faker.Random.Int(0));
                if (faker.IndexFaker % 3 == 0) return new SnoopableObject(faker.Random.Bool());

                return new SnoopableObject(faker.Lorem.Word());
            })
            .Generate(generationCount));
    }
}