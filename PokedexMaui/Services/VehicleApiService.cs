using System.Net.Http.Json;
using PokedexMaui.Models;

namespace PokedexMaui.Services;

// Consume la API vPIC de la NHTSA. Usa su propio HttpClient porque la dirección base es distinta a PokeAPI.
public class VehicleApiService
{
    private readonly HttpClient _httpClient;

    public VehicleApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<VehicleMake>> GetAllMakesAsync(
        CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(
            "vehicles/GetAllMakes?format=json",
            cancellationToken);

        response.EnsureSuccessStatusCode();

        // Si el contenido no es JSON válido, ReadFromJsonAsync lanza JsonException.
        VehicleMakeResponse? data = await response.Content.ReadFromJsonAsync<VehicleMakeResponse>(
            cancellationToken: cancellationToken);

        if (data is null)
            throw new InvalidDataException("La respuesta llegó vacía.");

        return data.Results
            .OrderBy(make => make.MakeName)
            .ToList();
    }
}
