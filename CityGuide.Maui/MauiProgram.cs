using Microsoft.Extensions.Logging;

namespace CityGuide.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    // Hanken Grotesk (başlık + gövde)
                    fonts.AddFont("HankenGrotesk-Regular.ttf", "HankenRegular");
                    fonts.AddFont("HankenGrotesk-Medium.ttf", "HankenMedium");
                    fonts.AddFont("HankenGrotesk-SemiBold.ttf", "HankenSemiBold");
                    fonts.AddFont("HankenGrotesk-Bold.ttf", "HankenBold");

                    // Inter (etiketler)
                    fonts.AddFont("Inter_18pt-Regular.ttf", "Inter");

                    // Material Symbols (ikonlar)
                    fonts.AddFont("material-symbols-outlined-latin-400-normal.ttf", "MaterialSymbols");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
