using CityGuide.Maui.Models;
using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

public partial class MyFavoritesPage : ContentPage
{
    private readonly AppDatabase _db = new AppDatabase();

    public MyFavoritesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadFavoritesAsync();
    }

    private async Task LoadFavoritesAsync()
    {
        var favorites = await _db.GetFavoritePlacesAsync(CurrentSession.UserId);

        FavoritesCollection.ItemsSource = favorites;
    }

    private async void OnFavoriteTapped(object sender, TappedEventArgs e)
    {
        if (sender is not Label label)
            return;

        if (label.BindingContext is not Place place)
            return;

        // Favoriden çýkar
        await _db.RemoveFavoriteAsync(CurrentSession.UserId, place.Id);

        // Listeyi güncelle
        await LoadFavoritesAsync();
    }

    private async void OnDetailsClicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (button.BindingContext is not Place place)
            return;

        await Shell.Current.GoToAsync($"placedetail?id={place.Id}");
    }

    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}