using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;


[QueryProperty(nameof(RouteId), "id")]
public partial class RouteDetailPage : ContentPage
{
    private readonly AppDatabase _db = new AppDatabase();
    public string RouteId { get; set; } = string.Empty;
    public RouteDetailPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (int.TryParse(RouteId, out int routeId))
        {
            await LoadRouteDetailAsync(routeId);
        }
    }

    private async Task LoadRouteDetailAsync(int routeId)
    {
        var allRoutes = await _db.GetRoutesAsync();
        var route = allRoutes.FirstOrDefault(r => r.Id == routeId);

        if (route is null) return;

        RouteImage.Source = route.ImageUrl;
        CategoryLabel.Text = route.Category;
        DurationLabel.Text = route.Duration;
        TitleLabel.Text = route.Title;
        DescriptionLabel.Text = route.Description;

        var stops = await _db.GetRouteStopsAsync(routeId);
        StopsCollection.ItemsSource = stops;
    }
}