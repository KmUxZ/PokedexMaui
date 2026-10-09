using Microsoft.Extensions.Logging;
using PokedexMaui.Services;

namespace PokedexMaui;

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
            });

        builder.Services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri("https://pokeapi.co/api/v2/"),
            Timeout = TimeSpan.FromSeconds(15)
        });

        builder.Services.AddSingleton<PokeApiService>();

        // vPIC: su propio HttpClient con otra dirección base. Timeout mayor porque la lista es grande.
        builder.Services.AddSingleton(_ => new VehicleApiService(new HttpClient
        {
            BaseAddress = new Uri("https://vpic.nhtsa.dot.gov/api/"),
            Timeout = TimeSpan.FromSeconds(30)
        }));

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<VehiclesPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
