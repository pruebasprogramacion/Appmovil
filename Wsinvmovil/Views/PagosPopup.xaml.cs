using Microsoft.Maui.Controls;
namespace Wsinvmovil.Views;

public partial class PagosPopup : ContentView
{
    public event EventHandler? PopupClosed;
    public PagosPopup()
	{
		InitializeComponent();
	}
    public void OnCloseClicked(object sender, EventArgs e)
    {
        this.IsVisible = false;  // Oculta el popup
        PopupClosed?.Invoke(this, EventArgs.Empty);
    }

    // Método para actualizar el texto del popup
    public void SetDetails(string details)
    {
        lblDetails.Text = details;
        this.IsVisible = true;
    }
}
