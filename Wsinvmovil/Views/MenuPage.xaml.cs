using Wsinvmovil.Servicios;
using Wsinvmovil.Views;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Wsinvmovil.Entidad;
namespace Wsinvmovil.Views;

public partial class MenuPage : ContentPage
{
	public MenuPage()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void ClientesPage_Clicked(object sender, EventArgs e)
    {
       await Navigation.PushModalAsync(new ClientesPage());
    }

    private async void PagosPage_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ConsultaPagosPage());
    }

    private async void InventarioPage_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new InventarioPage());

    }
    private async void Salir_Clicked(object sender, EventArgs e)
    {
        var salirsino = await DisplayAlert("Confirmacion:", "Seguro que desea Salir ?", "Si", "No");
        if (salirsino)
        {
            //await Navigation.PopToRootAsync();
#if ANDROID
            // Para Android
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif WINDOWS
            // Para Windows
            if (Application.Current != null)
            {
                Application.Current.Quit();
            }
#elif IOS
        // iOS no permite cerrar la app programáticamente
        await DisplayAlert("Información", "Por políticas de Apple, debe cerrar la app manualmente.", "OK");
#endif
        }
    }

    protected override bool OnBackButtonPressed()
    {
        return true; // Retorna "true" para evitar que el usuario regrese.
    }
}