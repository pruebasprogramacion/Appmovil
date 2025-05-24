using Wsinvmovil.Entidad;
using Wsinvmovil.Servicios;
using System.Data;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Wsinvmovil.Views;

public partial class ConsultaPagosPage : ContentPage
{
    private ObservableCollection<DetPago> detpagos = new ObservableCollection<DetPago>();
    private ObservableCollection<Pago> pagos = new ObservableCollection<Pago>();
    private Pago _pagoActual = new();
    private bool _popupActivo = false;

    public ConsultaPagosPage()
    {
        InitializeComponent();
        inicializarfecha();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (PopPagos != null)
        {
            PopPagos.PopupClosed += OnPopupCloseClicked;
        }
    }

    private void inicializarfecha()
    {
        TxtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    private async void Pornom_Clicked(object sender, EventArgs e)
    {
        try
        {
            detpagos.Clear();
            pagos.Clear();

            if (string.IsNullOrWhiteSpace(TxtCliente?.Text))
            {
                await DialogHelper.ShowAlert("⚠ Advertencia", "Debe ingresar un valor para realizar la búsqueda");
                return;
            }

            if (TxtCliente.Text.Trim().Length < 4)
            {
                await DialogHelper.ShowAlert("⚠ Advertencia", "Ingrese más caracteres para hacer una consulta más precisa");
                return;
            }

            var buscador = new BuscadorCLienteNombre(TxtCliente.Text.Trim());

            buscador.ClienteSeleccionado += async (s, datos) =>
            {
                TxtCliente.Text = datos.nombrecli;
                TxtNombreCli.Text = datos.nombrecli;
                TxtIdCli.Text = datos.idcli;

                if (!string.IsNullOrEmpty(datos.idcli))
                    await ConsultarPagos(datos.idcli);
            };

            await Navigation.PushModalAsync(buscador);
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowAlert("Error", $"Error en consulta por nombre: {ex.Message}");
        }
    }

    private async void Porci_Clicked(object sender, EventArgs e)
    {
        try
        {
            pagos.Clear();

            if (string.IsNullOrWhiteSpace(TxtCliente.Text))
            {
                await DialogHelper.ShowAlert("Error", "Debe ingresar un valor para realizar la búsqueda");
                return;
            }

            var servicio = new PagosService();
            var respuesta = await servicio.ObtenerClientePorCedula(TxtCliente.Text.Trim());

            if (respuesta.Exito && respuesta.Resultado != null)
            {
                var cliente = respuesta.Resultado;

                TxtNombreCli.Text = cliente.cliente;
                TxtIdCli.Text = cliente.id.ToString();
                await ConsultarPagos(TxtIdCli.Text);
            }
            else
            {
                await DialogHelper.ShowAlert("Sin resultados", respuesta.MensajeError ?? "No se encontraron datos para el cliente indicado");
            }
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowAlert("Error", $"Error en consulta por CI: {ex.Message}");
        }
    }

    private async Task ConsultarPagos(string idcliente)
    {
        try
        {
            detpagos.Clear();
            pagos.Clear();

            var servicio = new PagosService();
            var respuesta = await servicio.ObtenerPagosPorCliente(idcliente);

            if (respuesta.Exito && respuesta.Resultado != null && respuesta.Resultado.Any())
            {
                foreach (var row in respuesta.Resultado)
                {
                    pagos.Add(new Pago
                    {
                        Fecha = row.fecha.ToString("yyyy-MM-dd"),
                        Recibo = row.recibo?.ToString() ?? string.Empty,
                        Tipo = row.tipo ?? string.Empty,
                        Valor = row.valor.ToString("F2"),
                        Estado = row.estado ?? string.Empty
                    });
                }
            }
            else
            {
                await DialogHelper.ShowAlert("Sin resultados", respuesta.MensajeError ?? "No existen pagos para este cliente.");
            }

            ListaPagos.ItemsSource = pagos;
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowAlert("Error", $"Error al llenar pagos: {ex.Message}");
        }
    }

    private async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (_popupActivo) return;

        var checkBox = (CheckBox)sender;
        var pagoSeleccionado = (Pago)checkBox.BindingContext;

        if (pagoSeleccionado == null) return;

        if (checkBox.IsChecked)
        {
            _pagoActual = pagoSeleccionado;
            _popupActivo = true;
            await ShowPaymentDetailsPopup(pagoSeleccionado);
        }
        else
        {
            HidePaymentDetailsPopup();
        }
    }

    private async Task ShowPaymentDetailsPopup(Pago pago)
    {
        try
        {
            detpagos.Clear();
            await LlenarDetallePagos(pago.Recibo);

            var detalles = string.Join("\n\n", detpagos.Select(j =>
                $"Nro Crédito: {j.NroCredito}\nFactura: {j.Factura}\nFecha Crédito: {j.Fec_Credito}\nPago: {j.Pago}\nSaldo: {j.Saldo}"));

            PopPagos?.SetDetails(detalles);

            if (PopPagos != null)
            {
                PopPagos.IsVisible = true;
                ListaPagos.IsEnabled = false;
                ListaPagos.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowAlert("Error", $"Error al mostrar detalle:: {ex.Message}");
        }
    }

    private void HidePaymentDetailsPopup()
    {
        if (PopPagos == null) return;

        PopPagos.IsVisible = false;
        ListaPagos.IsEnabled = true;
        ListaPagos.IsVisible = true;
    }

    private async Task LlenarDetallePagos(string idpago)
    {
        try
        {
            var servicio = new PagosService();
            var respuesta = await servicio.ObtenerDetallePago(idpago);

            detpagos.Clear();

            if (respuesta.Exito && respuesta.Resultado != null && respuesta.Resultado.Any())
            {
                foreach (var row in respuesta.Resultado)
                {
                    detpagos.Add(new DetPago
                    {
                        NroCredito = row.nroCredito?.ToString() ?? string.Empty,
                        Factura = row.factura?.ToString() ?? string.Empty,
                        Fec_Credito = row.fec_Credito.ToString("yyyy-MM-dd"),
                        Pago = row.pago.ToString("F2"),
                        Saldo = row.saldo.ToString("F2")
                    });
                }
            }
            else
            {
                await DialogHelper.ShowAlert("Advertencia", respuesta.MensajeError ?? "No se encontró detalle del pago.");
            }
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowAlert("Error", $"Error al mostrar detalle de pago: {ex.Message}");
        }
    }

    private void OnPopupCloseClicked(object? sender, EventArgs e)
    {
        if (PopPagos == null) return;

        PopPagos.IsVisible = false;
        ListaPagos.IsEnabled = true;
        ListaPagos.IsVisible = true;

        if (_pagoActual != null)
        {
            _pagoActual.IsSelected = false;
            _pagoActual = new();
            ListaPagos.ItemsSource = pagos;
        }

        _popupActivo = false;
    }

    private async void Rback_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void ConvertirMayuscula_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.Text = e.NewTextValue?.ToUpper();
        }
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}