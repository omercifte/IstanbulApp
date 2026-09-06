using CityGuide.Maui.Models;
using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

public partial class LoginPage : ContentPage
{
    private readonly AppDatabase _db = new AppDatabase();
    public LoginPage()
	{
		InitializeComponent();
	}

    private void OnTogglePasswordVisibility(object sender, TappedEventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        // Göz ikonunu duruma göre deðiþtir
        if (PasswordEntry.IsPassword)
            PasswordToggleIcon.Text = "\ue8f4";   // visibility (göz açýk)
        else
            PasswordToggleIcon.Text = "\ue8f5";   // visibility_off (göz çizgili)
    }

    private async void OnForgotPasswordTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Þifremi Unuttum", "Þifre sýfýrlama yakýnda eklenecek.", "Tamam");
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // 1) Girilen veriyi oku
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        // 2) Boþ alan kontrolü
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Eksik Bilgi", "Lütfen e-posta ve þifrenizi girin.", "Tamam");
            return;
        }

        // 3) Veritabanýndan bu e-postaya sahip kullanýcýyý bul
        User? user = await _db.GetUserByEmailAsync(email);

        // 4) Kullanýcý yok mu, ya da þifre eþleþmiyor mu?
        if (user is null || user.Password != password)
        {
            await DisplayAlert("Giriþ Baþarýsýz", "E-posta veya þifre hatalý.", "Tamam");
            return;
        }

        // 5) Baþarýlý giriþ
        CurrentSession.UserId = user.Id;
        CurrentSession.FullName = user.FullName;
        CurrentSession.Email = user.Email;

        await Shell.Current.GoToAsync("//home");
    }

    private async void OnGoogleTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Google", "Google ile giriþ yakýnda eklenecek.", "Tamam");
    }

    private async void OnAppleTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Apple", "Apple ile giriþ yakýnda eklenecek.", "Tamam");
    }

    private async void OnRequestAccessTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//register");
    }


}