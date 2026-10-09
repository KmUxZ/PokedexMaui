using System.Text.Json.Serialization;

namespace PokedexMaui.Models;

// Respuesta de vPIC: GetAllMakes?format=json
// {"Count": 11000, "Message": "...", "SearchCriteria": null,
//  "Results": [ { "Make_ID": 440, "Make_Name": "ASTON MARTIN" }, ... ] }
// A diferencia de PokeAPI (un objeto por recurso), vPIC envuelve una LISTA dentro de "Results".
public class VehicleMakeResponse
{
    [JsonPropertyName("Count")]
    public int Count { get; set; }

    [JsonPropertyName("Message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("Results")]
    public List<VehicleMake> Results { get; set; } = [];
}

public class VehicleMake
{
    [JsonPropertyName("Make_ID")]
    public int MakeId { get; set; }

    [JsonPropertyName("Make_Name")]
    public string MakeName { get; set; } = string.Empty;

    // Texto listo para mostrar en la lista.
    public string DisplayId => $"ID {MakeId}";
}
