using CityGuide.Maui.Models;
using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

public partial class RegisterPage : ContentPage
{
    private readonly AppDatabase _db=new AppDatabase();
	public RegisterPage()
	{
		InitializeComponent();
	}

    private void OnTogglePasswordVisibility(object sender, TappedEventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        if (PasswordEntry.IsPassword)
            PasswordToggleIcon.Text = "\ue8f4";   // visibility (göz açık)
        else
            PasswordToggleIcon.Text = "\ue8f5";   // visibility_off (göz çizgili)
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
        // 1) Girilen veriyi oku
        string fullName = FullNameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        // 2) Boş alan kontrolü
        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Eksik Bilgi", "Lütfen tüm alanları doldurun.", "Tamam");
            return;
        }

        // 3) Şifreler eşleşiyor mu?
        if (password != confirmPassword)
        {
            await DisplayAlert("Hata", "Şifreler eşleşmiyor.", "Tamam");
            return;
        }

        // 4) Şartlar kabul edildi mi?
        if (!TermsCheckBox.IsChecked)
        {
            await DisplayAlert("Hata", "Şartları kabul etmelisiniz.", "Tamam");
            return;
        }

        // 5) Veritabanına kaydet
        try
        {
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Password = password
            };

            await _db.AddUserAsync(newUser);

            await DisplayAlert("Başarılı", $"Hoş geldiniz, {fullName}! Kaydınız oluşturuldu.", "Tamam");
        }
        catch (Exception)
        {
            // [Unique] ihlali: e-posta zaten kayıtlı
            await DisplayAlert("Hata", "Bu e-posta adresi zaten kayıtlı.", "Tamam");
        }
    }

 

    private async void OnSupportTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Destek", "Destek için: destek@milanorehberi.com", "Tamam");
    }


}