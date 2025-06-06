using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using System;
using Wsinvmovil.Entidad;
using Wsinvmovil.Views;


namespace Wsinvmovil.Views
{
    public partial class SetupPage : ContentPage
    {
        public SetupPage()
        {
            InitializeComponent();
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(entryEmpresa.Text))
            {
                string empresaMayusculas = entryEmpresa.Text.ToUpper();
                var googleService = new ServiciosConexion();
                ConexionInfo? conexion = await googleService.VerificarEmpresa(empresaMayusculas);
                if ( conexion is not null)
                {
                    Preferences.Set("EmpresaNombre", empresaMayusculas);
                    Preferences.Set("AmbienteNombre", conexion.ambiente);
                    Preferences.Set("IPServidor", conexion.ipServidor);
                    Preferences.Set("NombreBDD", conexion.nombreBDD);

                    if (Application.Current?.Windows.Count > 0)
                    {
                        Application.Current.Windows[0].Page = new NavigationPage(new LoginPage());
                    }
                }
                else
                {
                    await DisplayAlert("Error", "La empresa no está registrada en el sistema/o No tiene Autorización para instalarla.", "OK");
                    salir();
                }
            }
            else
            {
                await DisplayAlert("Error", "Debe ingresar un nombre válido.", "OK");
            }
        }

        private void entryEmpresa_TextChanged(object sender, TextChangedEventArgs e)
        {
            entryEmpresa.Text = e.NewTextValue?.ToUpper();
        }

     private void  salir()
     {
#if ANDROID
    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif WINDOWS
    Application.Current?.Quit();
#elif IOS
            if (Application.Current?.Windows.Count > 0 &&
                Application.Current.Windows[0].Page is Page currentPage)
            {
                 currentPage.DisplayAlert("Información", "Por políticas de Apple, debe cerrar la app manualmente.", "OK");
            }
#endif
        }
    }
}
