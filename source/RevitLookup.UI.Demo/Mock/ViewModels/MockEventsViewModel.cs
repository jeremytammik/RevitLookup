using System.Windows.Media;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.Core.Objects;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Enums;
using RevitLookup.ViewModels.Utils;
using RevitLookup.Views.Pages;

namespace RevitLookup.UI.Demo.Mock.ViewModels;

public sealed partial class MockEventsViewModel(
    NotificationService notificationService,
    IServiceProvider provider)
    : ObservableObject, IEventsViewModel
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly Stack<SnoopableObject> _events = new();

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private IList<SnoopableObject> _snoopableObjects = [];
    [ObservableProperty] private IList<SnoopableObject> _filteredSnoopableObjects = [];
    [ObservableProperty] private IList<Descriptor> _filteredSnoopableData;
    [ObservableProperty] private IList<Descriptor> _snoopableData;

    public SnoopableObject SelectedObject { get; set; }
    public IServiceProvider ServiceProvider { get; } = provider;

    public void Navigate(SnoopableObject selectedItem)
    {
        Host.GetService<ILookupService>()
            .Snoop(selectedItem)
            .DependsOn(ServiceProvider)
            .Show<SnoopView>();
    }

    public void Navigate(IList<SnoopableObject> selectedItems)
    {
        Host.GetService<ILookupService>()
            .Snoop(selectedItems)
            .DependsOn(ServiceProvider)
            .Show<SnoopView>();
    }

    public void RemoveObject(object obj)
    {
        var snoopableObject = obj switch
        {
            SnoopableObject snoopable => snoopable,
            Descriptor descriptor => descriptor.Value.Descriptor.Value,
            _ => throw new NotSupportedException($"Type {obj.GetType().Name} removing not supported")
        };

        SnoopableObjects.Remove(snoopableObject);
        FilteredSnoopableObjects.Remove(snoopableObject);
    }

    partial void OnSearchTextChanged(string value)
    {
        UpdateSearchResults(SearchOption.Objects);
    }

    partial void OnSnoopableObjectsChanged(IList<SnoopableObject> value)
    {
        SelectedObject = null;
        UpdateSearchResults(SearchOption.Objects);
    }

    partial void OnSnoopableDataChanged(IList<Descriptor> value)
    {
        UpdateSearchResults(SearchOption.Selection);
    }

    private void UpdateSearchResults(SearchOption option)
    {
        Task.Run(() =>
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                FilteredSnoopableObjects = SnoopableObjects;
                FilteredSnoopableData = SnoopableData;
                return;
            }

            var results = SearchEngine.Search(this, option);
            if (results.Data is not null) FilteredSnoopableData = results.Data;
            if (results.Objects is not null) FilteredSnoopableObjects = results.Objects;
        });
    }

    [RelayCommand]
    private Task FetchMembersAsync()
    {
        CollectMembers(true);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task RefreshMembersAsync()
    {
        CollectMembers(false);
        return Task.CompletedTask;
    }

    private void CollectMembers(bool useCached)
    {
        if (SelectedObject is null)
        {
            SnoopableData = Array.Empty<Descriptor>();
            return;
        }

        try
        {
            // ReSharper disable once MethodHasAsyncOverload
            SnoopableData = SelectedObject.GetMembers();
        }
        catch (Exception exception)
        {
            notificationService.ShowError("Snoop engine error", exception);
        }
    }

    public void OnNavigatedTo()
    {
        PushEvents(_cancellationTokenSource.Token);
    }

    public void OnNavigatedFrom()
    {
        _cancellationTokenSource.Cancel();
    }

    private void PushEvents(CancellationToken cancellationToken)
    {
        Task.Run((Func<Task>)(async () =>
        {
            var iteration = 0;
            var faker = new Faker();
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

                var snoopableObject = GenerateEvent(faker, iteration);
                _events.Push(snoopableObject);
                SnoopableObjects = new List<SnoopableObject>(_events);
                iteration++;
            }
        }), cancellationToken);
    }

    private static SnoopableObject GenerateEvent(Faker faker, int iteration)
    {
        if (iteration % 5 == 0) return new SnoopableObject(typeof(DateTime));
        if (iteration % 4 == 0) return new SnoopableObject(Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte()));
        if (iteration % 3 == 0) return new SnoopableObject(faker.Random.Int(0));
        if (iteration % 2 == 0) return new SnoopableObject(faker.Random.Bool());
        return new SnoopableObject(faker.Lorem.Word());
    }
}