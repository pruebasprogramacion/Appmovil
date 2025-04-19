using System.Text.Json;
using Wsinvmovil.Entidad;

public class GoogleSheetsService
{
    public async Task<ConexionInfo?> VerificarEmpresa(string empresa)
    {
        var url = $"https://script.google.com/macros/s/AKfycbw6FN6Lfb79QRdTyYYdEoZEB1hNj5BOZbYg4wh013DeInnJ8MNZGPEp3SBnz-_LGWsr/exec?empresa={Uri.EscapeDataString(empresa)}";
        var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(url);

        var resultado = JsonSerializer.Deserialize<ConexionInfo>(response);

        if (resultado == null || !string.IsNullOrEmpty(resultado.error))
        {
            return null;
        }
        return resultado;
    }
}


