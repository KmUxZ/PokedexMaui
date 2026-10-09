using System.Text.Json;
using PokedexMaui.Models;
using PokedexMaui.Services;

namespace PokedexMaui;

public partial class VehiclesPage : ContentPage
{
    private readonly VehicleApiService _vehicleApiService;
    private List<VehicleMake> _allMakes = [];
    private CancellationTokenSource? _loadCts;

    public VehiclesPage(VehicleApiService vehicleApiService)
    {
        InitializeComponent();
        _vehicleApiService = vehicleApiService;
    }

    // Descarga la lista la primera vez que se abre la pantalla.
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_allMakes.Count == 0)
            await LoadMakesAsync();
    }

    private async void OnReloadClicked(object? sender, EventArgs e)
    {
        await LoadMakesAsync();
    }

    private async Task LoadMakesAsync()
    {
        _loadCts?.Cancel();
        _loadCts?.Dispose();
        _loadCts = new CancellationTokenSource();

        SetLoading(true);
        MessageLabel.IsVisible = false;

        try
        {
            _allMakes = await _vehicleApiService.GetAllMakesAsync(_loadCts.Token);
            ApplyFilter(FilterBar.Text);
        }
        catch (TaskCanceledException)
        {
            ShowMessage("La consulta fue cancelada o tardó demasiado.");
        }
        catch (HttpRequestException)
        {
            ShowMessage("No fue posible conectarse con el servicio de vehículos.");
        }
        catch (JsonException)
        {
            ShowMessage("El servicio devolvió una respuesta inválida.");
        }
        catch (InvalidDataException)
        {
            ShowMessage("El servicio devolvió una respuesta vacía.");
        }
        catch (Exception)
        {
            ShowMessage("Ocurrió un error inesperado.");
        }
        finally
        {
            SetLoading(false);
        }
    }

    // Filtro local: no vuelve a llamar a la API, filtra la lista ya descargada.
    private void OnFilterChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFilter(e.NewTextValue);
    }

    private void ApplyFilter(string? text)
    {
        string filter = text?.Trim() ?? string.Empty;

        List<VehicleMake> result = string.IsNullOrEmpty(filter)
            ? _allMakes
            : _allMakes
                .Where(make => make.MakeName.Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToList();

        MakesCollection.ItemsSource = result;
        Title = $"Fabricantes ({result.Count})";
    }

    private void ShowMessage(string message)
    {
        MessageLabel.Text = message;
        MessageLabel.IsVisible = true;
    }

    private void SetLoading(bool isLoading)
    {
        LoadingIndicator.IsVisible = isLoading;
        LoadingIndicator.IsRunning = isLoading;
        ReloadButton.IsEnabled = !isLoading;
    }
}
