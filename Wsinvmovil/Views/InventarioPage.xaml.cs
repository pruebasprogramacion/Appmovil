using System.Data;
using Wsinvmovil.Entidad;
using Wsinvmovil.Servicios;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Wsinvmovil.Views;

public partial class InventarioPage : ContentPage
{
    public InventarioPage()
	{
		InitializeComponent();
	}

    private async void BuscarxCod_Clicked(object sender, EventArgs e)
    {
        string codarticulo = TxtArticulos.Text?.Trim() ?? "";

        if (codarticulo == "" || codarticulo == null)
        {
            await DialogHelper.ShowAlert("Error!!", "Debe Ingresar un valor para realizar la busqueda");
        }
        else
        {
            var servicio = new InventarioService();
            var listaArticulos = await servicio.ObtenerArticuloCod(codarticulo);
            if (listaArticulos != null && listaArticulos.Any())
            {
                LlenarInventario(listaArticulos); // lo convertimos a lista
            }
            else
            {
                await DialogHelper.ShowAlert("Sin resultados", "No se encontraron artículos con el código ingresado");
            }
        }
    }

    private async void BuscarxNom_Clicked(object sender, EventArgs e)
    {
        string nomarticulo = TxtArticulos.Text?.Trim() ?? "";

        if (nomarticulo == "" || nomarticulo == null)
        {
            await DialogHelper.ShowAlert("Error!!", "Debe Ingresar un valor para realizar la busqueda");
        }
        else
        {
            var servicio = new InventarioService();
            var listaArticulos = await servicio.ObtenerArticuloNom(nomarticulo);

            if (listaArticulos != null && listaArticulos.Any())
            {
                LlenarInventario(listaArticulos);
            }
            else
            {
                await DialogHelper.ShowAlert("Sin resultados", "No se encontraron artículos con ese nombre");
            }
        }

    }

    private void LlenarInventario(List<E_Articulo> listaCompleta)
    {
        var articulos = listaCompleta.Select(a => new Articulos
        {
            Codigo = a.Codigo,
            Nombre = a.Descripcion,
            Stock = a.Exis.ToString(),
            Precio = a.Precio.ToString()
        }).ToList();

        ListaArticulos.ItemsSource = articulos;
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