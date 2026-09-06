using CityGuide.Maui.Models;
using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

public class PlaceFilterItem
{
    public string Name { get; set; } = string.Empty;
    public bool IsSelected { get; set; }

    public Color BackgroundColor => IsSelected ? Color.FromArgb("#0D47A1") : Color.FromArgb("#FFFFFF");
    public Color BorderColor => IsSelected ? Color.FromArgb("#0D47A1") : Color.FromArgb("#C3C6D4");
    public Color TextColor => IsSelected ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#191C1D");

    public override string ToString() => Name;
}

public partial class FavoritesPage : ContentPage
{
    private readonly AppDatabase _db = new AppDatabase();
    private int CurrentUserId => CurrentSession.UserId;

    private List<Place> _allPlaces = new List<Place>();
    private List<PlaceFilterItem> _filterItems = new List<PlaceFilterItem>();
    private string _selectedCategory = "Tümü";

    public FavoritesPage()
    {
        InitializeComponent();

        _filterItems = new List<PlaceFilterItem>
        {
            new PlaceFilterItem { Name = "Tümü", IsSelected = true },
            new PlaceFilterItem { Name = "Restoran" },
            new PlaceFilterItem { Name = "Müze" },
            new PlaceFilterItem { Name = "Rota" },
            new PlaceFilterItem { Name = "Kafe" }
        };

        FilterCollection.ItemsSource = _filterItems;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPlacesAsync();
        FilterCollection.SelectedItem = _filterItems.First(i => i.Name == "Tümü");
    }

    private async Task LoadPlacesAsync()
    {
        var places = await _db.GetPlacesAsync();

        foreach (var place in places)
        {
            place.IsFavorite = await _db.IsFavoriteAsync(CurrentUserId, place.Id);
        }

        _allPlaces = places;
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        if (_selectedCategory == "Tümü")
        {
            PlacesCollection.ItemsSource = _allPlaces;
        }
        else
        {
            PlacesCollection.ItemsSource = _allPlaces
                .Where(p => p.CategoryName == _selectedCategory)
                .ToList();
        }
    }

    private void OnFilterSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not PlaceFilterItem selected)
            return;

        foreach (var item in _filterItems)
        {
            item.IsSelected = (item == selected);
        }

        FilterCollection.ItemsSource = null;
        FilterCollection.ItemsSource = _filterItems;
        FilterCollection.SelectedItem = selected;

        _selectedCategory = selected.Name;
        ApplyFilter();
    }

    private async void OnFavoriteTapped(object sender, TappedEventArgs e)
    {
        if (sender is not Label label) return;
        if (label.BindingContext is not Place place) return;

        if (place.IsFavorite)
            await _db.RemoveFavoriteAsync(CurrentUserId, place.Id);
        else
            await _db.AddFavoriteAsync(CurrentUserId, place.Id);

        await LoadPlacesAsync();
    }

    private async void OnDetailsClicked(object sender, EventArgs e)
    {
        if (sender is not Button button) return;
        if (button.BindingContext is not Place place) return;

        await Shell.Current.GoToAsync($"placedetail?id={place.Id}");
    }

    private async void OnHomeTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }

    private async void OnEventsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//events");
    }

    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("profile");
    }
}
