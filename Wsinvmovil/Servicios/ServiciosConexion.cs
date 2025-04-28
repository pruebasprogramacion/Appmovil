using System.Text.Json;
using Wsinvmovil.Entidad;

public class ServiciosConexion
{
    public async Task<ConexionInfo?> VerificarEmpresa(string empresa)
    {
        var url = $"https://script.google.com/macros/s/AKfycbzHowgP7rjlTfc_favplzLXQotr9qCygloIrcJkdrIn9dUhUgzbcCyKPWVEFRBonKyq/exec?empresa={Uri.EscapeDataString(empresa)}";
        var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(url);

        var resultado = JsonSerializer.Deserialize<ConexionInfo>(response);

        if (resultado == null || !string.IsNullOrEmpty(resultado.error))
        {
            return null;
        }
        return resultado;
    }

    public async Task<bool> VerificarConexionServidor(string ipServidor)
    {
        using var httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(5) // Evita esperas largas
        };

        try
        {
            var response = await httpClient.GetAsync($"http://{ipServidor}", HttpCompletionOption.ResponseHeadersRead);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return true;
        }
    }
}


