using System.Security.Cryptography;
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

    private void ClearFields()
    {
        EmailEntry.Text = string.Empty;
        PasswordEntry.Text = string.Empty;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ClearFields();
    }

    private void OnTogglePasswordVisibility(object sender, TappedEventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        if (PasswordEntry.IsPassword)
            PasswordToggleIcon.Text = "\ue8f4";
        else
            PasswordToggleIcon.Text = "\ue8f5";
    }

    private async void OnForgotPasswordTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert(
            "Þifremi Unuttum",
            "Þifre sýfýrlama yakýnda eklenecek.",
            "Tamam");
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text?.Trim().ToLowerInvariant() ?? string.Empty;
        string password = PasswordEntry.Text ?? string.Empty;

        // 1) Boþ alan kontrolü
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert(
                "Eksik Bilgi",
                "Lütfen e-posta ve þifrenizi girin.",
                "Tamam");

            return;
        }

        // 2) Kullanýcýyý bul
        User? user = await _db.GetUserByEmailAsync(email);

        if (user is null)
        {
            await DisplayAlert(
                "Giriþ Baþarýsýz",
                "E-posta veya þifre hatalý.",
                "Tamam");

            return;
        }

        // 3) Yeni sistemde hash'lenmiþ þifreyi kontrol et
        bool passwordValid = VerifyPassword(
            password,
            user.Password);

        // 4) Eski kullanýcýlar için geriye dönük uyumluluk
        // Eski kayýtlarýn þifreleri düz metin olabilir.
        if (!passwordValid &&
            !user.Password.StartsWith("PBKDF2$") &&
            user.Password == password)
        {
            passwordValid = true;

            // Eski düz metin þifreyi yeni güvenli sisteme geçir
            user.Password = HashPassword(password);

            await _db.UpdateUserAsync(user);
        }

        // 5) Þifre yanlýþsa
        if (!passwordValid)
        {
            await DisplayAlert(
                "Giriþ Baþarýsýz",
                "E-posta veya þifre hatalý.",
                "Tamam");

            return;
        }

        // 6) Baþarýlý giriþ
        CurrentSession.UserId = user.Id;
        CurrentSession.FullName = user.FullName;
        CurrentSession.Email = user.Email;

        // Beni hatýrla seçiliyse oturumu cihazda sakla
        if (RememberCheckBox.IsChecked)
        {
            Preferences.Set("RememberMe", true);
            Preferences.Set("RememberedUserId", user.Id);
        }
        else
        {
            Preferences.Remove("RememberMe");
            Preferences.Remove("RememberedUserId");
        }

        await Shell.Current.GoToAsync("//home");
    }

    private async void OnGoogleTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert(
            "Google",
            "Google ile giriþ yakýnda eklenecek.",
            "Tamam");
    }

    private async void OnAppleTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert(
            "Apple",
            "Apple ile giriþ yakýnda eklenecek.",
            "Tamam");
    }

    private async void OnRequestAccessTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//register");
    }

    private static string HashPassword(string password)
    {
        const int iterations = 100_000;

        byte[] salt = RandomNumberGenerator.GetBytes(16);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256);

        byte[] hash = pbkdf2.GetBytes(32);

        return $"PBKDF2${iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    private static bool VerifyPassword(
        string password,
        string storedPassword)
    {
        try
        {
            if (!storedPassword.StartsWith("PBKDF2$"))
                return false;

            string[] parts = storedPassword.Split('$');

            if (parts.Length != 4)
                return false;

            int iterations = int.Parse(parts[1]);

            byte[] salt =
                Convert.FromBase64String(parts[2]);

            byte[] storedHash =
                Convert.FromBase64String(parts[3]);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256);

            byte[] calculatedHash =
                pbkdf2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(
                calculatedHash,
                storedHash);
        }
        catch
        {
            return false;
        }
    }
}