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

using System.Collections.ObjectModel;
using System.Windows;
using RevitLookup.Core;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Core.Utils;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.ViewModels.Contracts;

namespace RevitLookup.Services;

public sealed class SnoopVisualService(NotificationService notificationService, ISnoopViewModel viewModel, IWindow window) : ISnoopVisualService
{
    public void Snoop(SnoopableObject snoopableObject)
    {
        try
        {
            IReadOnlyCollection<SnoopableObject> snoopableObjects;
            if (snoopableObject.Descriptor is IDescriptorEnumerator {IsEmpty: false} descriptor)
            {
                snoopableObjects = descriptor.ParseEnumerable(snoopableObject);
            }
            else
            {
                snoopableObjects = [snoopableObject];
            }

            Snoop(snoopableObjects);
        }
        catch (Exception exception)
        {
            notificationService.ShowError("Invalid object", exception);
        }
    }

    public async Task SnoopAsync(SnoopableType snoopableType)
    {
        try
        {
            switch (snoopableType)
            {
                case SnoopableType.Face:
                case SnoopableType.Edge:
                case SnoopableType.LinkedElement:
                case SnoopableType.Point:
                case SnoopableType.SubElement:
                    UpdateWindowVisibility(Visibility.Hidden);
                    break;
            }

            var snoopableObjects = await Application.ExternalElementHandler.RaiseAsync(_ => Selector.Snoop(snoopableType));
            Snoop(snoopableObjects);
        }
        catch (OperationCanceledException)
        {
            notificationService.ShowWarning("Operation cancelled", "Operation cancelled by user");
        }
        catch (Exception exception)
        {
            notificationService.ShowError("Operation cancelled", exception);
        }
        finally
        {
            UpdateWindowVisibility(Visibility.Visible);
        }
    }
    
    public void Snoop(IReadOnlyCollection<SnoopableObject> snoopableObjects)
    {
        viewModel.SnoopableObjects = new ObservableCollection<SnoopableObject>(snoopableObjects);
        viewModel.SnoopableData = Array.Empty<Descriptor>();
    }
    
    private void UpdateWindowVisibility(Visibility visibility)
    {
        if (!window.IsLoaded) return;

        window.Visibility = visibility;
    }
}