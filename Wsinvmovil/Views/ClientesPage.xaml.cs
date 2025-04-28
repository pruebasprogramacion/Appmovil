
using System.Data;
using System.Threading.Tasks;
using Wsinvmovil.Entidad;
using System.Security.Cryptography;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using Wsinvmovil.Servicios;

namespace Wsinvmovil.Views;

public partial class ClientesPage : ContentPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (GlobalSettings.nomPantalla == "RealizarPagoPage")
        {

                TxtSaldo.Text = string.Empty;
                TxtCedula.Text = string.Empty;
                ListaCreditos.ItemsSource = null;
                TxtCredito.Text= string.Empty;
                TxtDias.Text= string.Empty;
                TxtNombre.Text= string.Empty;
                TxtId.Text= string.Empty;
                Rpago.IsEnabled = false;
            GlobalSettings.nomPantalla = string.Empty;
        }
    }

    public ClientesPage()
	{
		InitializeComponent();       
	}
    private async void Buscar_Clicked(object sender, EventArgs e)
    {
        try
        {
            string cedula = TxtCedula.Text.Trim();
            if (cedula == "" || cedula == null)
            {
                await DisplayAlert("Error!", "Debe Ingresar un valor para realizar la busqueda ", "OK");
            }
            else
            {
                var servicio = new ClienteService();
                var respuesta = await servicio.ObtenerClientePorCedula(TxtCedula.Text.Trim());
                if (respuesta.Exito && respuesta.Resultado != null)
                {
                    string nombre = respuesta.Resultado.nombre;
                    TxtNombre.Text = nombre;
                    string credito = respuesta.Resultado.credito.ToString();
                    TxtCredito.Text = credito;
                    string id = respuesta.Resultado.id;
                    TxtId.Text = id;
                    var servicio1 = new ClienteService();
                    var respuesta1 = await servicio1.ObtenerSaldoCliente(Convert.ToInt32(id));
                    if (respuesta1.Exito && respuesta1.Resultado != null)
                    {
                        string miValor = respuesta1.Resultado.saldo.ToString();
                        string dias = "0";
                        if (miValor != "0")
                        {
                            dias = dias = respuesta1.Resultado.dias.ToString();
                        }
                        TxtSaldo.Text = miValor;
                        TxtDias.Text = dias;
                        await ConsultarCreditos(Convert.ToInt32(id));
                    }
                }
                else
                {
                    await DialogHelper.ShowAlert("Advertencia", respuesta.MensajeError ?? "No se encontró creditos  del cliente.");
                }
            }
        }
        catch (Exception ex) { 
            await DisplayAlert("Error",$"Error al bucar por cedula Page Clientes {ex.Message}", "OK");
        }
    }

    private async void BuscarPornom_Clicked(object sender, EventArgs e)
    {
        try
        {
            var buscador = new BuscadorCLienteNombre(TxtCedula.Text);
            if (TxtCedula.Text == "" || TxtCedula.Text == null)
            {
                await DisplayAlert("Error!", "Debe Ingresar un valor para realizar la busqueda", "OK");
            }
            else
                 if (TxtCedula.Text.Trim().Length < 4)
            {
                await DisplayAlert("Error!", "Ingrese más carácteres para hacer una consulta mas precisa", "OK");
            }
            else
            {
                // Suscribirse al evento
                buscador.ClienteSeleccionado += async (s, datos) =>
                {
                    TxtCedula.Text = datos.nombrecli; // Asigna el nombre al TextBox
                    TxtNombre.Text = datos.nombrecli;
                    TxtId.Text = datos.idcli;
                    int id = Convert.ToInt32(datos.idcli);
                    var servicio1 = new ClienteService();
                    var respuesta1 = await servicio1.ObtenerSaldoCliente(id);
                    if (respuesta1.Exito && respuesta1.Resultado != null)
                    {
                        string miValor = respuesta1.Resultado.saldo.ToString();
                        string dias = "0";
                        if (miValor != "0")
                        {
                            dias = dias = respuesta1.Resultado.dias.ToString();
                        }
                        TxtSaldo.Text = miValor;
                        TxtDias.Text = dias;
                    }
                    await ConsultarCreditos(id);
                };
                await Navigation.PushModalAsync(buscador);
            }
        }
        catch (Exception ex) {
            await DialogHelper.ShowAlert("Error", $"Error al mostrar los creditos del cliente: {ex.Message}");
        }
    }
    private async Task ConsultarCreditos(int idcliente)
    {
        try
        {
            List<E_Credito> creditos = new List<E_Credito>();
            creditos.Clear();
            var servicio = new PagosService();
            var respuesta = await servicio.ObtenerCreditosCliente(idcliente);
            if (respuesta.Exito && respuesta.Resultado != null)
            {
                foreach (var row in respuesta.Resultado)
                {
                    if (row.saldo > 0)
                    {
                        creditos.Add(new E_Credito
                        {
                            NroCredito =  row.credito.ToString(),
                            Fecha = row.fecha.ToString(),
                            Factura = row.factura.ToString(),
                            Plazo = row.plazo.ToString(),
                            Valor = row.valor.ToString(),
                            Saldo = row.saldo.ToString(),
                            cabid = Convert.ToInt32(row.cabId),
                        });
                    }
                }
                ListaCreditos.ItemsSource = creditos
                    .OrderBy(c => Convert.ToDateTime(c.Fecha)) // Ordena por fecha de más antigua a más reciente
                    .ToList();
                Rpago.IsEnabled = true;
            }
            else
            {
                await DialogHelper.ShowAlert("Advertencia", respuesta.MensajeError ?? "No se encontró creditos  del cliente.");
            }
        }
        catch (Exception ex) {
            await DialogHelper.ShowAlert("Error", $"Error al mostrar los creditos del cliente: {ex.Message}");
        }
    }
    private async void Rpago_Clicked(object sender, EventArgs e)
        {
        // Obtener los valores de los campos
        try
        {

            string saldo = TxtSaldo.Text;
            string id = TxtId.Text;
            string nombre = TxtNombre.Text;
            string cedula = TxtCedula.Text;
            List<E_Credito> LisCreditos = ListaCreditos.ItemsSource?.Cast<E_Credito>().ToList() ?? new List<E_Credito>();
            // Navegar a la página de pago enviando los datos
            if (nombre.Trim().Equals(null) | nombre.Trim() == "")
            {
                await DisplayAlert("Error!", "Ingresar datos del Cliente para poder realizar el pago", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new RealizarPagoPage(saldo, id, nombre, cedula, LisCreditos));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al realizar el pago Page Clientes {ex.Message}", "OK");
        }
    }

    private void ConvertirMayuscula_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.Text = e.NewTextValue?.ToUpper(); // Convierte todo el texto a mayúsculas
        }
    }

    protected override bool OnBackButtonPressed()
    {
        return true; // Retorna "true" para evitar que el usuario regrese
    }

    private async void Rback_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}