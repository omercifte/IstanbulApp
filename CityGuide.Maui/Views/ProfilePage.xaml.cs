using System.Xml;
using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

public partial class ProfilePage : ContentPage
{
	private readonly AppDatabase _db = new AppDatabase();
	public ProfilePage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        NameLabel.Text = CurrentSession.FullName;
        EmailLabel.Text = CurrentSession.Email;

        var favoritePlaces = await _db.GetFavoritePlacesAsync(CurrentSession.UserId);
        FavoriteCountLabel.Text = favoritePlaces.Count.ToString();

        var allRoutes = await _db.GetRoutesAsync();
        RouteCountLabel.Text = allRoutes.Count.ToString();
    }

    private async void OnMenuItemTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Yakýnda", "Bu özellik yakýnda eklenecek.", "Tamam");
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Çýkýþ Yap", "Çýkýþ yapmak istediðinizden emin misiniz?", "Evet", "Hayýr");
        if (confirm)
        {
            CurrentSession.Clear();

            Preferences.Remove("RememberMe");
            Preferences.Remove("RememberedUserId");

            await Shell.Current.GoToAsync("//login");
        }
    }

    private async void OnHomeTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }

    private async void OnDiscoverTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("routes");
    }

    private async void OnEventsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//events");
    }

    private async void OnFavoritesTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("myfavorites");
    }
}

