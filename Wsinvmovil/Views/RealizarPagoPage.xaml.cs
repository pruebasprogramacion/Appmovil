using Wsinvmovil.Entidad;
using Wsinvmovil.Servicios;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Wsinvmovil.Views;

public partial class RealizarPagoPage : ContentPage
{
    private RealizarPagoViewModel viewModel = new();
    public RealizarPagoPage(string saldo, string id, string nombre, string cedula, List<E_Credito> creditospago)
    {
     
        InitializeComponent();
        viewModel = new RealizarPagoViewModel();
        BindingContext = viewModel;
        // Asignar los valores a los campos de la pantalla de pago
        TxtSaldoPago.Text = saldo;
        TxtNombrePago.Text = nombre;
        TxtIdPago.Text = id;
        creditosgp = creditospago ?? new List<E_Credito>();
    }
    E_Caja objEcaja = new E_Caja();
    EPago objEntPagos = new EPago();
    public int idfac;
    public string tipo = "" ;
    public string vvalor1 = "";
    private bool _mostrarBanco, _mostrarNro, _mostrarTarjeta, _mostrarVoucher;
    public bool MostrarBanco { get => _mostrarBanco; set { _mostrarBanco = value; OnPropertyChanged(); } }
    public bool MostrarNro { get => _mostrarNro; set { _mostrarNro = value; OnPropertyChanged(); } }
    public bool MostrarTarjeta { get => _mostrarTarjeta; set { _mostrarTarjeta = value; OnPropertyChanged(); } }
    public bool MostrarVoucher { get => _mostrarVoucher; set { _mostrarVoucher = value; OnPropertyChanged(); } }
  
    public  List<E_Credito> creditosgp;
    protected override void OnAppearing()
    {
        base.OnAppearing();
        btnBack.IsEnabled = true;
        btnImprimir.IsEnabled = false;
        CargarBancos();
        MostrarBanco = false;
        MostrarNro = false;
        MostrarTarjeta = false;
        MostrarVoucher = false;

    }

    private void ConvertirMayuscula_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.Text = e.NewTextValue?.ToUpper(); // Convierte todo el texto a mayúsculas
        }
    }

    private async void ImprimirButton_Clicked(object sender, EventArgs e)
    {
        await MostrarComprobanteWebViewPrincipal.MostrarComprobanteWebView(Navigation, objEntPagos,TxtMontoPago.Text);
    }

    private void GrabarButton_Clicked(object sender, EventArgs e)
    {
        GrabarButton_Clicked(sender, e, TxtRecibo);
    }

    private async void GrabarButton_Clicked(object sender, EventArgs e, Label? txtRecibo)
    {
        try
        {
            if (TxtMontoPago.Text.Trim().Equals(null))
            {
                await   DisplayAlert("Error!", "Debe Ingresar el valor apagar, no puede ser 0 o blanco", "OK");
                vvalor1 = "";
                return;
            }


            if (TxtMontoPago.Text.Trim().Equals(""))
            {
                await DisplayAlert("Error!", "Debe Ingresar el valor apagar, no puede ser 0 o blanco", "OK");
                vvalor1 = "";
                return;
            }
            double valorpago = Convert.ToDouble(TxtMontoPago.Text);
            string usuario = GlobalSettings.usuario;

            if (Convert.ToDouble(TxtMontoPago.Text.Trim()) > Convert.ToDouble(TxtSaldoPago.Text.Trim()))
            {
                await DisplayAlert("Error", "El monto no puede ser mayor al monto de la deuda", "OK");
                vvalor1 = "";
                return;
            }
            if (PckFormaPago.SelectedItem == null)
            {
                await DisplayAlert("Error", "Debe seleccionar una forma de pago", "OK");
                vvalor1 = "";
                return;
            }

            if (PckFormaPago.SelectedItem.ToString() == "TRANSFERENCIA")
            {
                if (TxtNroP.Text.Trim().Equals(""))
                {
                    await DisplayAlert("Error", "Debe Ingresar el Numero de transferencia", "OK");
                    vvalor1 = "";
                    return;
                }
                if (PckBanco.SelectedItem == null)
                {
                    await DisplayAlert("Error", "Debe seleccionar banco", "OK");
                    vvalor1 = "";
                    return;
                }
                if (PckBanco.SelectedItem is Banco bancoSeleccionado)
                {
                    objEcaja.caj_banco = bancoSeleccionado.ID.ToString(); // Asignar el ID
                }
                objEcaja.caj_medio = PckFormaPago.SelectedItem?.ToString() ?? "";
                objEcaja.caj_valor = Convert.ToDouble(TxtMontoPago.Text);
                objEcaja.caj_nro = TxtNroP.Text;
                objEcaja.caj_idfactura = idfac;
                objEcaja.caj_tipo = "C";
                TxtNro.Text = "Nro Trf:";
                vvalor1 = "G";
            }
            if ((PckFormaPago.SelectedItem?.ToString() ?? "") == "EFECTIVO")
            {
                objEcaja.caj_banco = "0";
                objEcaja.caj_medio = PckFormaPago.SelectedItem?.ToString() ?? "";
                objEcaja.caj_valor = Convert.ToDouble(TxtMontoPago.Text);
                objEcaja.caj_nro = "";
                objEcaja.caj_idfactura = idfac;
                objEcaja.caj_tipo = "C";
                vvalor1 = "G";
            }

            if (PckFormaPago.SelectedItem?.ToString() == "CHEQUE")
            {
                if (TxtNroP.Text.Trim().Equals(""))
                {
                    await DisplayAlert("Error", "Debe Ingresar el Numero de Cheque", "OK");
                    vvalor1 = "";
                    return;
                }
                if (PckBanco.SelectedItem == null)
                {
                    await DisplayAlert("Error", "Debe seleccionar banco", "OK");
                    vvalor1 = "";
                    return;
                }
                if (PckBanco.SelectedItem is Banco bancoSeleccionado)
                {
                    objEcaja.caj_banco = bancoSeleccionado.ID.ToString(); // Asignar el ID
                }
                objEcaja.caj_medio = PckFormaPago.SelectedItem?.ToString() ?? ""; 
                objEcaja.caj_valor = Convert.ToDouble(TxtMontoPago.Text);
                objEcaja.caj_nro = TxtNroP.Text;
                objEcaja.caj_idfactura = idfac;
                objEcaja.caj_tipo = "C";
                TxtNro.Text = "Nro.Cheque";
                vvalor1 = "G";
            }
            if (PckFormaPago.SelectedItem?.ToString() == "TARJETA")
            {
                if (TxtVoucher.Text.Trim().Equals(""))
                {
                    await DisplayAlert("Error", "Debe Ingresar el Numero de Voucher", "OK");
                    vvalor1 = "";
                    return;
                }
                if (PckBanco.SelectedItem == null)
                {
                    await DisplayAlert("Error", "Debe seleccionar Banco", "OK");
                    vvalor1 = "";
                    return;
                }
                if (PckBanco.SelectedItem is Banco bancoSeleccionado)
                {
                    objEcaja.caj_banco = bancoSeleccionado.ID.ToString(); // Asignar el ID
                }
                objEcaja.caj_medio = PckFormaPago.SelectedItem?.ToString() ?? "";
                objEcaja.caj_valor = Convert.ToDouble(TxtMontoPago.Text);
                objEcaja.caj_nro = TxtNroP.Text;
                objEcaja.caj_idfactura = idfac;
                objEcaja.caj_tipo = "C";
                objEcaja.caj_tarjeta = TxtTarjeta.Text;
                objEcaja.caj_boucher = TxtVoucher.Text;
                vvalor1 = "G";
            }
            if (PckFormaPago.SelectedItem?.ToString() == "TRANSFERENCIA" | PckFormaPago.SelectedItem?.ToString() == "CHEQUE")
            {
                var servicioCaja = new CajaService();
                var respuesta4 = await servicioCaja.ConsultarTransferencia(objEcaja.caj_nro, objEcaja.caj_banco);
                if (respuesta4.Exito && respuesta4.Resultado != null && respuesta4.Resultado.Any())
                {
                    await DisplayAlert("Error", $"La {PckFormaPago.SelectedItem?.ToString()} ya esta asignada a otro pago, favor revisar", "OK");
                    vvalor1 = "";
                    return;
                }
            }
            if (objEntPagos == null)
            {
                objEntPagos = new EPago();
            }
            objEntPagos.pagc_id = (TxtRecibo?.Text == null || TxtRecibo.Text.Trim() == "") ? 0 : Convert.ToInt32(TxtRecibo.Text);
            //objEntPagoCompra.pag_idcredito = Convert.ToInt32(txtCompra.Text);
            objEntPagos.pagc_idcli = Convert.ToInt32(TxtIdPago.Text);
            objEntPagos.pagc_nombre = TxtNombrePago.Text.Trim();
            objEntPagos.pagc_fecha = TxtFechaPago.Date.ToString("yyyy-dd-MM");
            objEntPagos.pagc_pago = Convert.ToDouble(TxtMontoPago.Text);
            objEntPagos.pagc_tipo = PckFormaPago.SelectedItem?.ToString() ?? "";
            objEntPagos.pagc_usuario = $"{usuario}-Web";
            if (vvalor1 == "G")
            {
                objEntPagos.pagc_saldo = 0;
                objEntPagos.pagc_tipo = objEcaja.caj_medio ?? "";
                var servicio = new PagosService();
                var resultado = await servicio.GuardarPagoVenta(objEntPagos);
                if (resultado.Exito)
                {
                    string idRecibo = resultado.Resultado.ToString();
                    TxtRecibo!.Text = idRecibo;
                    objEntPagos.pagc_id = Convert.ToInt32(TxtRecibo.Text ?? "0");
                    string idCaja = resultado.Resultado.ToString();
                    objEcaja.caj_idfactura = Convert.ToInt32(idCaja);
                    objEcaja.caj_fecha = TxtFechaPago.Date.ToString("yyyy-dd-MM");
                    objEcaja.caj_usuario = $"{usuario}-Web";
                    var servicioCaja = new CajaService();
                    var resultado1 = await servicioCaja.InsertarCajaCredAbo(objEcaja);
                    if (resultado1.Exito && resultado1.Resultado > 0)
                    {
                        string caja = resultado1.Resultado.ToString();
                        var servicioPago = new PagosService();
                        var resultado2 = await servicioPago.ActualizarNumeroCaja(TxtRecibo.Text ?? "0", caja);
                        if (resultado2.Exito)
                        {


                        } // Actualiza Numero Caja
                    } // inserta Caja Credito  Abo 
                    double saldoTotal = Convert.ToDouble(TxtMontoPago.Text.Trim());
                    double pago = 0;
                    for (int i = 0; i < creditosgp.Count && saldoTotal > 0; i++)
                    {
                       var credito = creditosgp[i];
                       objEntPagos.pag_idcredito = Convert.ToInt32(credito.NroCredito);
                       objEntPagos.pagc_nro = credito.cabid.ToString();
                       pago = Convert.ToDouble(credito.Saldo);
                       if (saldoTotal >= pago) 
                       {
                           objEntPagos.pagc_pago = pago;
                           objEntPagos.pagc_saldo = 0;
                           credito.Saldo = "0.00";
                        }
                        else
                        {
                            objEntPagos.pagc_pago = saldoTotal;
                            objEntPagos.pagc_saldo = pago - saldoTotal;
                            credito.Saldo = (pago - saldoTotal).ToString("N2");
                         }
                        var servicioPago1 = new PagosService();
                        var resultado3 = await servicioPago1.InsertarDetallePagos(objEntPagos);
                        if (resultado3.Exito)
                        {
                            saldoTotal = saldoTotal - pago;
                        }
                     } // fin del for
                     await DisplayAlert("Informacion", "Pago grabado satisfactoriamente, Su Recibo es:  " + TxtRecibo.Text, "OK");
                } // Guarda pago Venta
                else
                {
                    await DialogHelper.ShowAlert("Error", resultado.MensajeError ?? "No se pudo guardar en caja");
                } // fin Pago venta
                //btEliminar.Enabled = true;
                //btImprimir.Enabled = true;
            } // Valor G
            else
            {
                await DisplayAlert("Error", "Pago no se guardo, se cancelo", "OK");
            } // Cierra el if de Valorg G
            btnGrabar.IsEnabled = false;
            btnImprimir.IsEnabled = true;
            btnBack.IsEnabled = false;
            vvalor1 = "";
        } // fin try
        catch (Exception ex) {

            await DisplayAlert("Error",$"Error al grabar el pago page RealizarPago {ex.Message}","OK");

        }
    }

    private void PckBanco_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PckBanco.SelectedItem is Banco bancoSeleccionado)
        {
            int bancoID = bancoSeleccionado.ID;
            string bancoNombre = bancoSeleccionado.nomBanco;
            // Asignar a tu objeto
            objEcaja.caj_banco = bancoID.ToString();
        }
    }

    private void PckFormaPago_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PckFormaPago.SelectedIndex != -1) // Verifica que haya una selección válida
        {
            string formaPagoSeleccionada = PckFormaPago.SelectedItem?.ToString() ?? "";
            DisplayAlert("Forma de Pago", $"Seleccionaste: {formaPagoSeleccionada}", "OK");

            string formaPago = PckFormaPago.SelectedItem?.ToString() ?? "";

            MostrarBanco = formaPago == "TRANSFERENCIA" || formaPago == "CHEQUE" || formaPago == "TARJETA";
            MostrarNro = formaPago == "TRANSFERENCIA" || formaPago == "CHEQUE";
            MostrarTarjeta = formaPago == "TARJETA";
            MostrarVoucher = formaPago == "TARJETA";

            if  (formaPago == "TRANSFERENCIA" || formaPago == "CHEQUE" || formaPago == "TARJETA")
            {
                if (formaPago == "TRANSFERENCIA")
                {
                    TxtNro.Text = "Nro Tr:";
                }
                if (formaPago == "CHEQUE")
                {
                    TxtNro.Text = "Nro Cheq.:";
                }
                TxtNroP.Text = "";
                PckBanco.SelectedItem = null;
            }
            if (formaPago == "TARJETA")
            {
                TxtNroP.Text = "";
                PckBanco.SelectedItem = null;
                TxtTarjeta.Text = "";
                TxtVoucher.Text = "";

            }
            if (formaPago == "EFECTIVO")
            {
                TxtNroP.Text = "";
                PckBanco.SelectedItem = null;
                TxtTarjeta.Text = "";
                TxtVoucher.Text = "";

            }

            // Puedes hacer lo que necesPites con el valor seleccionado
            // Por ejemplo, guardarlo en una variable o habilitar un botón
        }
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        GlobalSettings.nomPantalla = "RealizarPagoPage";
        await Navigation.PopModalAsync();
    }

    private async void CargarBancos()
    {
        try
        {
            var servicio = new CajaService();
            var respuesta = await servicio.ObtenerBancos();

            if (respuesta.Exito && respuesta.Resultado != null)
            {
                PckBanco.ItemsSource = respuesta.Resultado;
                PckBanco.ItemDisplayBinding = new Binding("nomBanco");
            }
            else
            {
                await DialogHelper.ShowAlert("Error", respuesta.MensajeError ?? "No se pudieron cargar los bancos.");
            }
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowAlert("Excepción", ex.Message);
        }
    }
   
    protected override bool OnBackButtonPressed()
    {
        return true; // Retorna "true" para evitar que el usuario regrese
    }
}