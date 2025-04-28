using Wsinvmovil.Entidad;
using Microsoft.Maui.Controls;
using System.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq.Expressions;
using Wsinvmovil.Servicios;

namespace Wsinvmovil.Views;
public partial class BuscadorCLienteNombre : ContentPage
{
    private string ultimoTexto = "";
    private bool isAlertDisplayed = false;
    private CancellationTokenSource? _cts;

    public ObservableCollection<BuscadorNom> Clientes { get; set; } = new();

    public event EventHandler<(string nombrecli, string idcli)>? ClienteSeleccionado;

    public BuscadorCLienteNombre(string nombreInicial)
    {
        InitializeComponent();
        BindingContext = this;
       LlenarLista(nombreInicial);
    }

    private async Task LlenarLista(string nombre)
    {
        try
        {
            Clientes.Clear();

            if (string.IsNullOrWhiteSpace(nombre))
                return;

            var servicio = new PagosService();
            var respuesta = await servicio.ObtenerClientePorNom("1", nombre.Trim());

            if (respuesta.Exito && respuesta.Resultado != null && respuesta.Resultado.Any())
            {
                foreach (var cliente in respuesta.Resultado)
                {
                    Clientes.Add(new BuscadorNom
                    {
                        Nombre = cliente.cliente?.Trim() ?? "",
                        Id = cliente.id.ToString().Trim()
                    });
                }
            }
            else
            {
                if (!isAlertDisplayed)
                {
                    isAlertDisplayed = true;
                    await DialogHelper.ShowAlert("Información", respuesta.MensajeError ?? "Cliente no existe, favor validar");
                    isAlertDisplayed = false;
                    if (Navigation.ModalStack.Count > 0)
                        await Navigation.PopModalAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowAlert("Error", $"Error en lista de Clientes: {ex.Message}");
        }
    }

    private async void OnTextoCambiado(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue == ultimoTexto)
            return;
        ultimoTexto = e.NewTextValue;
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        try
        {
             await Task.Delay(300, token); // Espera 300ms sin teclear
             if (!token.IsCancellationRequested)
             {
                await LlenarLista(e.NewTextValue); // Llama solo si no fue cancelado
             }
         }
    catch (TaskCanceledException) { }
    }
  
    private async void OnClienteSeleccionado(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (e.CurrentSelection?.FirstOrDefault() is BuscadorNom cliente)
            {
                Console.WriteLine($"Cliente seleccionado: {cliente.Nombre} - {cliente.Id}");
                ClienteSeleccionado?.Invoke(this, (cliente.Nombre, cliente.Id));
                await Task.Delay(100);
                await Navigation.PopModalAsync();
            }

            ((CollectionView)sender).SelectedItem = null;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al seleccionar cliente: {ex.Message}", "OK");
        }
    }
}
