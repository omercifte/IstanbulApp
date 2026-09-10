using System.Net.Mail;
using System.Security.Cryptography;
using CityGuide.Maui.Models;
using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

public partial class RegisterPage : ContentPage
{
    private readonly AppDatabase _db = new AppDatabase();

    public RegisterPage()
    {
        InitializeComponent();
    }

    private void ClearFields()
    {
        FullNameEntry.Text = string.Empty;
        EmailEntry.Text = string.Empty;
        PasswordEntry.Text = string.Empty;
        ConfirmPasswordEntry.Text = string.Empty;
        TermsCheckBox.IsChecked = false;

        PasswordEntry.IsPassword = true;
        PasswordToggleIcon.Text = "\ue8f4";
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

    private async void OnSignInTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//login");
    }

    private async void OnTermsTapped(object sender, TappedEventArgs e)
    {
        string termsText =
            "MİLANO ŞEHİR REHBERİ — ŞARTLAR VE KOŞULLAR\n\n" +
            "1. Üyelik\n" +
            "Bu uygulamayı kullanarak, sağladığınız bilgilerin doğru ve güncel olduğunu kabul edersiniz.\n\n" +
            "2. Gizlilik\n" +
            "Kişisel verileriniz yalnızca uygulama hizmetlerini sunmak amacıyla kullanılır ve üçüncü taraflarla paylaşılmaz.\n\n" +
            "3. Kullanım\n" +
            "Uygulama içeriği yalnızca kişisel ve ticari olmayan amaçlarla kullanılabilir.\n\n" +
            "4. Sorumluluk\n" +
            "Etkinlik bilgileri değişebilir; güncel bilgileri ilgili mekândan teyit etmeniz önerilir.\n\n" +
            "Bu şartları kabul ederek devam etmiş olursunuz.";

        await DisplayAlert("Şartlar ve Koşullar", termsText, "Kapat");
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string fullName = FullNameEntry.Text?.Trim() ?? string.Empty;
        string email = EmailEntry.Text?.Trim().ToLowerInvariant() ?? string.Empty;
        string password = PasswordEntry.Text ?? string.Empty;
        string confirmPassword = ConfirmPasswordEntry.Text ?? string.Empty;

        // 1) Boş alan kontrolü
        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert(
                "Eksik Bilgi",
                "Lütfen tüm alanları doldurun.",
                "Tamam");

            return;
        }

        // 2) E-posta formatı kontrolü
        if (!IsValidEmail(email))
        {
            await DisplayAlert(
                "Geçersiz E-posta",
                "Lütfen geçerli bir e-posta adresi girin.\n\nÖrnek: ornek@gmail.com",
                "Tamam");

            return;
        }

        // 3) Aynı e-posta daha önce kayıtlı mı?
        var existingUser = await _db.GetUserByEmailAsync(email);

        if (existingUser is not null)
        {
            await DisplayAlert(
                "E-posta Zaten Kayıtlı",
                "Bu e-posta adresiyle daha önce bir hesap oluşturulmuş. Lütfen farklı bir e-posta adresi kullanın.",
                "Tamam");

            return;
        }

        // 4) Şifre güvenlik kontrolü
        if (!IsStrongPassword(password))
        {
            await DisplayAlert(
                "Zayıf Şifre",
                "Şifreniz şu şartları sağlamalıdır:\n\n" +
                "• En az 8 karakter\n" +
                "• En az 1 büyük harf (A-Z)\n" +
                "• En az 1 küçük harf (a-z)\n" +
                "• En az 1 rakam (0-9)\n" +
                "• En az 1 özel karakter (!, @, #, $, %, vb.)",
                "Tamam");

            return;
        }

        // 5) Şifreler eşleşiyor mu?
        if (password != confirmPassword)
        {
            await DisplayAlert(
                "Şifreler Eşleşmiyor",
                "Girdiğiniz şifreler aynı değil.",
                "Tamam");

            return;
        }

        // 6) Şartlar kabul edildi mi?
        if (!TermsCheckBox.IsChecked)
        {
            await DisplayAlert(
                "Hata",
                "Şartlar ve Koşulları kabul etmelisiniz.",
                "Tamam");

            return;
        }

        // 7) Kullanıcıyı oluştur
        try
        {
            var newUser = new User
            {
                FullName = fullName,
                Email = email,

                // Şifre artık düz metin olarak tutulmuyor.
                Password = HashPassword(password)
            };

            await _db.AddUserAsync(newUser);

            await DisplayAlert(
                "Başarılı",
                $"Hoş geldiniz, {fullName}!\n\nKaydınız başarıyla oluşturuldu.",
                "Tamam");

            // Kayıttan sonra giriş ekranına dön
            await Shell.Current.GoToAsync("//login");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[REGISTER ERROR] {ex}");

            await DisplayAlert(
                "Hata",
                "Kayıt sırasında bir sorun oluştu. Lütfen tekrar deneyin.",
                "Tamam");
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);

            return mailAddress.Address.Equals(
                email,
                StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private static bool IsStrongPassword(string password)
    {
        if (password.Length < 8)
            return false;

        bool hasUppercase = password.Any(char.IsUpper);
        bool hasLowercase = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

        return hasUppercase &&
               hasLowercase &&
               hasDigit &&
               hasSpecial;
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

    private static bool VerifyPassword(string password, string storedPassword)
    {
        try
        {
            if (!storedPassword.StartsWith("PBKDF2$"))
                return false;

            string[] parts = storedPassword.Split('$');

            if (parts.Length != 4)
                return false;

            int iterations = int.Parse(parts[1]);

            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] storedHash = Convert.FromBase64String(parts[3]);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256);

            byte[] calculatedHash = pbkdf2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(
                calculatedHash,
                storedHash);
        }
        catch
        {
            return false;
        }
    }

    private async void OnSupportTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert(
            "Destek",
            "Destek için: destek@istanbulsehirreheri.com",
            "Tamam");
    }
}