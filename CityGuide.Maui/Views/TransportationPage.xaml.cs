using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

public partial class TransportationPage : ContentPage
{
	private readonly AppDatabase _db=new AppDatabase();
	public TransportationPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var allLines = await _db.GetTransportLinesAsync();

        // Metro hatlarý
        MetroCollection.ItemsSource = allLines
            .Where(l => l.Type == "Metro")
            .ToList();

        // Tramvay + Otobüs (ikon eþleþtirmesiyle)
        var surfaceLines = allLines
            .Where(l => l.Type == "Tramvay" || l.Type == "Otobüs")
            .Select(l => new
            {
                l.LineName,
                l.Route,
                l.ColorHex,
                IconGlyph = l.Type == "Tramvay" ? "\ue571" : "\ue530"
            })
            .ToList();

        SurfaceCollection.ItemsSource = surfaceLines;
    }

    private async void OnHomeTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }

    private async void OnDiscoverTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//discover");
    }

    private async void OnEventsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//events");
    }

    private async void OnBuyTicketClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://www.malpensaexpress.it/en/travel-documents/tickets/");
    }
}