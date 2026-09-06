using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        Routing.RegisterRoute("specialevents", typeof(Views.SpecialEventsPage));
	}

    private readonly AppDatabase _db= new AppDatabase();

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Popüler yerleri yükle (ilk birkaç mekan)
        var places = await _db.GetPlacesAsync();
        PopularPlacesCollection.ItemsSource = places.Take(10).ToList();
    }

    private async void OnExploreClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Keþfet", "Detay sayfasý yakýnda eklenecek.", "Tamam");
    }

    private async void OnFavoritesClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//discover");
    }

    private async void OnDiscoverTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("routes");
    }

    private async void OnEventsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//events");
    }

    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("profile");
    }

    private async void OnFoodDrinksClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("fooddrinks");
    }
    private async void OnCultureClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("cultures");
    }
    private async void OnDashboardClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("dashboard");
    }

}