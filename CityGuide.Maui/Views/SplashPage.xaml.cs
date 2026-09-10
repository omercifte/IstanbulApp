namespace CityGuide.Maui.Views;

public partial class SplashPage : ContentPage
{
    // Arka plan görselleri (Resources/Images'taki dosyalar)
    private readonly string[] _images = new string[]
    {
        "galatakulesi.png", 
        "sultanahmet.jpg",
        "balat.jpg"

        
    };

    private int _currentIndex = 0;
    private bool _isRunning = true;

    public SplashPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BackgroundImage.Source = _images[0];

        StartZoomAnimation();
        StartSlideshow();
        StartLoadingBar();

        // 10 saniye sonra Login'e geç
        NavigateAfterDelay();
    }

    private async void NavigateAfterDelay()
    {
        await Task.Delay(10000);  // 10 saniye

        if (!_isRunning) return;  // Sayfa zaten kapandıysa geçme

        await Shell.Current.GoToAsync("//login");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _isRunning = false;  // Sayfa kapanınca döngüleri durdur
    }

    // Görselleri sırayla değiştir (fade geçişiyle)
    private async void StartSlideshow()
    {
        while (_isRunning)
        {
            // 3 saniye bekle
            await Task.Delay(3000);

            if (!_isRunning) break;

            // Sonraki görsele geç
            _currentIndex = (_currentIndex + 1) % _images.Length;

            // Yumuşak geçiş: kaybol → değiştir → belir
            await BackgroundImage.FadeTo(0, 600, Easing.CubicInOut);
            BackgroundImage.Source = _images[_currentIndex];
            await BackgroundImage.FadeTo(1, 600, Easing.CubicInOut);
        }
    }

    // Arka plana sürekli yavaş zoom
    private async void StartZoomAnimation()
    {
        while (_isRunning)
        {
            await BackgroundImage.ScaleTo(1.1, 10000, Easing.SinInOut);
            await BackgroundImage.ScaleTo(1.0, 10000, Easing.SinInOut);
        }
    }

    // Yükleniyor çubuğunu doldur
    private async void StartLoadingBar()
    {
        LoadingBarFill.WidthRequest = 0;
        await LoadingBarFill.LayoutTo(new Rect(0, 0, 140, 3), 3000, Easing.CubicInOut);
    }
}
