using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Services;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Pages;

namespace RevitLookup.UI.Demo.Mock;

public sealed class MoqEventsViewModel : SnoopViewModelBase, IEventsViewModel
{
    private readonly Stack<SnoopableObject> _events = new();

    public MoqEventsViewModel(NotificationService notificationService, IServiceProvider provider) : base(notificationService, provider)
    {
    }

    public void OnNavigatedTo()
    {
        PushEvent("Subscribe", new EventArgs());
    }

    public void OnNavigatedFrom()
    {
        PushEvent("Unsubscribe", new EventArgs());
    }

    private void PushEvent(string name, EventArgs _event)
    {
        var snoopableObject = new SnoopableObject(_event)
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

