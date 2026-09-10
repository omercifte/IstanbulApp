using CityGuide.Maui.Services;
using CityGuide.Maui.Views;

namespace CityGuide.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            window.Width = 393;
            window.Height = 652;

            // Daha önce "Beni hatırla" seçilmişse
            // kayıtlı kullanıcıyı tekrar oturum açtırmadan Home'a gönder.
            if (Preferences.Get("RememberMe", false))
            {
                int userId = Preferences.Get("RememberedUserId", 0);

                if (userId > 0)
                {
                    _ = RestoreSessionAsync(userId);
                }
            }

            return window;
        }

        private async Task RestoreSessionAsync(int userId)
        {
            try
            {
                var db = new Services.AppDatabase();

                var user = await db.GetUserByIdAsync(userId);

                if (user is not null)
                {
                    CurrentSession.UserId = user.Id;
                    CurrentSession.FullName = user.FullName;
                    CurrentSession.Email = user.Email;

                    await Shell.Current.GoToAsync("//home");
                }
                else
                {
                    ClearRememberedSession();
                }
            }
            catch
            {
                ClearRememberedSession();
            }
        }

        private void ClearRememberedSession()
        {
            Preferences.Remove("RememberMe");
            Preferences.Remove("RememberedUserId");
        }
    }
}