using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

public partial class SpecialEventsPage : ContentPage
{
    private readonly EventApiService _api = new EventApiService();
    public SpecialEventsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var events = await _api.GetEventsAsync();
            EventsCollection.ItemsSource = events;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Etkinlikler yüklenemedi: {ex.Message}", "Tamam");
        }
    }
}