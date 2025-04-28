using System.Text.Json;
using Wsinvmovil.Entidad;
public class InventarioService
{
    public async Task<List<E_Articulo>?> ObtenerArticuloCod(string codigo)
    {
        try
        {
            var ip = Preferences.Get("IPServidor", "");
            var bdd = Preferences.Get("NombreBDD", "");
            var url = $"http://{ip}:5299/Inventario/ObtenerArticulo?ipServidor={ip}&nombreBDD={bdd}&codigo={Uri.EscapeDataString(codigo)}";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            Console.WriteLine($"StatusCode: {response.StatusCode}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var lista = JsonSerializer.Deserialize<List<E_Articulo>>(json, options);
                return lista;
            }
            return null;
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowAlert("Error", $"No se pudo obtener el articulo: {ex.Message}");
            return null;
        }
    }
    public async Task<List<E_Articulo>?> ObtenerArticuloNom(string nomarticulo)
    {
       try
       {
           var ip = Preferences.Get("IPServidor", "");
           var bdd = Preferences.Get("NombreBDD", "");
           var url = $"http://{ip}:5299/Inventario/ObtenerPorNombre?ipServidor={ip}&nombreBDD={bdd}&nombre={Uri.EscapeDataString(nomarticulo)}";
           var httpClient = new HttpClient();
           var response = await httpClient.GetAsync(url);
           if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var lista = JsonSerializer.Deserialize<List<E_Articulo>>(json, options);
                return lista;
            }
            return null;
            }
            catch (Exception ex)
            {
              await DialogHelper.ShowAlert("Error", $"No se pudo obtener los artículos: {ex.Message}");
              return null;
        }
    }
}
