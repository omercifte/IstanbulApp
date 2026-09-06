using CityGuide.Maui.Models;
using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

[QueryProperty(nameof(PlaceId), "id")]
public partial class PlaceDetailPage : ContentPage
{
    private readonly AppDatabase _db = new AppDatabase();
    private Place? _currentPlace;

    public string PlaceId { get; set; } = string.Empty;

    public PlaceDetailPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (int.TryParse(PlaceId, out int placeId))
        {
            await LoadPlaceDetailAsync(placeId);
        }
    }

    private async Task LoadPlaceDetailAsync(int placeId)
    {
        var allPlaces = await _db.GetPlacesAsync();
        _currentPlace = allPlaces.FirstOrDefault(p => p.Id == placeId);

        if (_currentPlace is null) return;

        HeroImage.Source = _currentPlace.ImageUrl;
        CategoryBadgeLabel.Text = _currentPlace.CategoryName;
        TitleLabel.Text = _currentPlace.Title;

        DescriptionLabel.Text = string.IsNullOrWhiteSpace(_currentPlace.Description)
            ? "Bu mekan için henüz açýklama eklenmedi."
            : _currentPlace.Description;

        DurationLabel.Text = string.IsNullOrWhiteSpace(_currentPlace.Duration) ? "—" : _currentPlace.Duration;
        PriceQuickLabel.Text = string.IsNullOrWhiteSpace(_currentPlace.PriceInfo) ? "—" : _currentPlace.PriceInfo;
        RatingQuickLabel.Text = $"{_currentPlace.Score} ({_currentPlace.ReviewCount})";

        AddressLabel.Text = string.IsNullOrWhiteSpace(_currentPlace.Address)
            ? _currentPlace.Location
            : _currentPlace.Address;

        MapImage.Source = string.IsNullOrWhiteSpace(_currentPlace.MapImageUrl)
            ? "milano_000.jpg"
            : _currentPlace.MapImageUrl;

        PriceInfoLabel.Text = string.IsNullOrWhiteSpace(_currentPlace.PriceInfo)
            ? "Bilgi yok"
            : _currentPlace.PriceInfo;

        var images = await _db.GetPlaceImagesAsync(placeId);
        GalleryCollection.ItemsSource = images;
    }

    private async void OnOpenMapsClicked(object sender, EventArgs e)
    {
        if (_currentPlace is null) return;

        string address = Uri.EscapeDataString(_currentPlace.Address ?? _currentPlace.Location);
        string mapsUrl = $"https://www.google.com/maps/search/?api=1&query={address}";
        await Launcher.OpenAsync(mapsUrl);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_currentPlace is null) return;

        if (_currentPlace.IsFavorite)
            await _db.RemoveFavoriteAsync(CurrentSession.UserId, _currentPlace.Id);
        else
            await _db.AddFavoriteAsync(CurrentSession.UserId, _currentPlace.Id);

        await DisplayAlert("Kaydedildi", "Mekan favorilerinize eklendi/çýkarýldý.", "Tamam");
    }

    private async void OnShareClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Paylaþ", "Paylaþým özelliði yakýnda eklenecek.", "Tamam");
    }


    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnBookTicketsClicked(object sender, EventArgs e)
    {
        if (_currentPlace is null || string.IsNullOrWhiteSpace(_currentPlace.TicketUrl))
        {
            await DisplayAlert("Bilet", "Bu mekan için bilet bilgisi bulunmuyor.", "Tamam");
            return;
        }

        await Launcher.OpenAsync(_currentPlace.TicketUrl);
    }
}
