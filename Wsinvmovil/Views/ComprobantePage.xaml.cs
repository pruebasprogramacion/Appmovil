using System;
using System.IO;
using System.Net.Http;
using Microsoft.Maui.Controls;
using Wsinvmovil.Servicios;
using Microsoft.Maui.ApplicationModel; // necesario para FileSystem y Share

namespace Wsinvmovil.Views
{
    public partial class ComprobantePage : ContentPage
    {
        private readonly string _htmlContent;

        public ComprobantePage(string htmlContent)
        {
            InitializeComponent();

            _htmlContent = htmlContent;

            // Cargar el HTML en el WebView
            ComprobanteWebView.Source = new HtmlWebViewSource
            {
                Html = htmlContent
            };

            // Quitar barra de navegación
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void Cerrar_Clicked(object sender, EventArgs e)
        {
#pragma warning disable CA1416
            await Navigation.PopModalAsync();
#pragma warning restore CA1416
        }

        protected override bool OnBackButtonPressed()
        {
            return true; // Desactiva el botón de retroceso físico
        }

        private async void Compartir_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Title = "Compartir Comprobante",
                    Text = _htmlContent,
                    Subject = "Comprobante de Pago"
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo compartir: {ex.Message}", "OK");
            }
        }

        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                string fileName = "comprobante.html";
                string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                // Guardar el contenido en un archivo
                File.WriteAllText(filePath, _htmlContent);

                await DisplayAlert("Éxito", "Comprobante guardado correctamente.", "OK");

                // Compartir archivo guardado
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Compartir Comprobante",
                    File = new ShareFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo guardar: {ex.Message}", "OK");
            }
        }
    }
}

