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
using RevitLookup.Core;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Enums;
using RevitLookup.Views.Pages;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.Navigation;

namespace RevitLookup.ViewModels.Pages;

public sealed class EventsViewModel : SnoopViewModelBase, INavigationAware
{
    private readonly EventMonitor _eventMonitor;
    private readonly Stack<SnoopableObject> _events = new();
    private readonly INavigationService _navigationService;

    public EventsViewModel(INavigationService navigationService, ISnackbarService snackbarService) : base(navigationService, snackbarService)
    {
        _navigationService = navigationService;
        SnoopableObjects = new ObservableCollection<SnoopableObject>();
        _eventMonitor = new EventMonitor(OnHandlingEvent);
    }

    public void OnNavigatedTo()
    {
        _eventMonitor.Subscribe();
    }

    public void OnNavigatedFrom()
    {
        _eventMonitor.Unsubscribe();
    }

    public override async Task Snoop(SnoopableType snoopableType)
    {
        await Task.CompletedTask;
        if (snoopableType != SnoopableType.Events) throw new NotSupportedException();

        _navigationService.Navigate(typeof(EventsView));
    }

    private void OnHandlingEvent(string name, EventArgs args)
    {
        var snoopableObject = new SnoopableObject(RevitApi.Document, args)
        {
            Descriptor =
            {
                Name = $"{name} {DateTime.Now:HH:mm:ss}"
            }
        };

        _events.Push(snoopableObject);
        SnoopableObjects = new List<SnoopableObject>(_events);

        //Object lifecycle expires. We should force a data retrieval before the object is cleared from memory
        var descriptors = snoopableObject.GetMembers();
        foreach (var descriptor in descriptors)
            if (descriptor.Value.Descriptor is IDescriptorCollector)
                descriptor.Value.GetMembers();
    }
}