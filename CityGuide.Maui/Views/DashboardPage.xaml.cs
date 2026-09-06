using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    private readonly CurrencyApiService _currencyApi = new CurrencyApiService();
    private readonly WeatherApiService _weatherApi = new WeatherApiService();


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadExchangeRatesAsync();
        await LoadWeatherAsync();
        //LoadTimeInfo();
    }

    //private void LoadTimeInfo()
    //{
    //    try
    //    {
    //        var milanoZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome");
    //        var turkeyZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");

    //        var milanoTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, milanoZone);
    //        var turkeyTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyZone);

    //        MilanoTimeLabel.Text = milanoTime.ToString("HH:mm");
    //        TurkeyTimeLabel.Text = turkeyTime.ToString("HH:mm");

    //        int hourDifference = (int)(turkeyTime - milanoTime).TotalHours;
    //        TimeDifferenceLabel.Text = hourDifference > 0
    //            ? $"Türkiye, Milano'dan {hourDifference} saat ileride"
    //            : "Milano ve Türkiye aynı saat diliminde";
    //    }
    //    catch (Exception)
    //    {
    //        MilanoTimeLabel.Text = "—";
    //        TurkeyTimeLabel.Text = "—";
    //        TimeDifferenceLabel.Text = "Saat bilgisi alınamadı.";
    //    }
    //}

    private async Task LoadWeatherAsync()
    {
        var weather = await _weatherApi.GetMilanoWeatherAsync();

        if (weather is null)
        {
            TemperatureLabel.Text = "—";
            WeatherDescriptionLabel.Text = "Hava durumu alınamadı.";
            return;
        }

        var condition = weather.CurrentObservation.Condition;
        TemperatureLabel.Text = $"{condition.Temperature}°C";
        WeatherDescriptionLabel.Text = $"{condition.Text} • Nem %{weather.CurrentObservation.Atmosphere.Humidity}";
    }
    private async Task LoadExchangeRatesAsync()
    {
        // EUR/TRY
        var eurResult = await _currencyApi.GetExchangeRateAsync("EUR", "TRY");
        if (eurResult is not null)
        {
            EurRateLabel.Text = $"₺ {eurResult.Result:F2}";
        }
        else
        {
            EurRateLabel.Text = "—";
        }

        // USD/TRY
        var usdResult = await _currencyApi.GetExchangeRateAsync("USD", "TRY");
        if (usdResult is not null)
        {
            UsdRateLabel.Text = $"₺ {usdResult.Result:F2}";
        }
        else
        {
            UsdRateLabel.Text = "—";
        }
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

    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("profile");
    }
}