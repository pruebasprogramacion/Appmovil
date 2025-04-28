using System.Security.Cryptography;
using System.Text.Json;
using Wsinvmovil.Entidad;

namespace Wsinvmovil.Views;

public partial class LoginPage : ContentPage
{
    public string empresa = "";

    public LoginPage()
    {
        InitializeComponent();

    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
     
        lblambiente.Text = $"Ambiente: {GlobalSettings.ambiente}";
        lblempresa.Text = $"Empresa: {GlobalSettings.empresa}";
    }

    private async void Ingresar_Clicked(object sender, EventArgs e)
    {
        try
        {
            string username = TxtUsuario.Text;
            string password = TxtPassword.Text;
            if (username == "" || password == "" || username == null || password == null)
            {
                await DisplayAlert("Error!!", "Usuario/Password  no pueden ser blancos", "OK");
            }
            else
            {
                // Valido conexion para ejecucion de servicios web
                var serviciosConexion = new ServiciosConexion();
                bool servidorDisponible = await serviciosConexion.VerificarConexionServidor(GlobalSettings.IpServidor);
                if (!servidorDisponible)
                {
                    await DialogHelper.ShowAlert("Sin conexión", "No se pudo conectar con el servidor. Verifica la IP o tu conexión VPN.");
                    return; // salir sin continuar
                } else {
                    var loginService = new LoginService();
                    var resultado = await loginService.ValidarLoginAsync(username, password, GlobalSettings.IpServidor, GlobalSettings.NombreBDD);
                    if (resultado.Exito && resultado.Resultado)
                    {
                        Preferences.Set("Usuario", username.Trim());
                        datosempresa(GlobalSettings.IpServidor, GlobalSettings.NombreBDD);
                        await Navigation.PushModalAsync(new MenuPage());
                        // Login válido
                    }
                    else
                    {
                        await DialogHelper.ShowAlert("Login fallido", resultado.MensajeError ?? "Error al validar credenciales.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowAlert("Error!!", $"En la validacion del usuario {ex.Message}");
        }
    }
    private void UsuarioEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.Text = e.NewTextValue?.ToUpper(); // Convierte todo el texto a mayúsculas
        }
    }
    private void TogglePasswordVisibility(object sender, EventArgs e)
    {
        if (TxtPassword.IsPassword)
        {
            TxtPassword.IsPassword = false;
            TogglePasswordButton.Source = ImageSource.FromFile("eye_open.png"); // Ojo abierto
        }
        else
        {
            TxtPassword.IsPassword = true;
            TogglePasswordButton.Source = ImageSource.FromFile("eye_closed.png"); // Ojo cerrado
        }
    }
    private async void datosempresa(string ipservidor, string nombdd)
    {
        var servicio = new EmpresaService();
        var datos = await servicio.ObtenerDatosEmpresa(ipservidor, nombdd);

        if (datos.Exito && datos.Resultado != null)
        {
            // Puedes navegar a otra página o mostrar mensaje de éxito

        }
        else
        {
            // Ya se muestra el error desde el servicio, este es opcional si quieres reforzar
            await DialogHelper.ShowAlert("Error", datos.MensajeError ?? "No se encontró la empresa.");
        }
    }

}
