using System.Text.Json;
using Wsinvmovil.Entidad;

public class GoogleSheetsService
{
    public async Task<ConexionInfo?> VerificarEmpresa(string empresa)
    {
        var url = $"https://script.googleusercontent.com/macros/echo?user_content_key=AehSKLjjmvytEl9R2rJxSCqGWE-1t2N8VuEd0vBpKZw7x3QOtAjufe3W-s76hhFnbb0ETosX2oN5IvKW18tlptNYlhp0CPvbnBixOtblwD25MX40C9-ZFVL8bbOfUHzZ0ZTlH1xLyWhMc9rPJfznP4UplW1KQaHC9yDjSQIJgdO6Z1MPnYm63Az2EFnH7NLoGIQEC8iijyZ1aSmP9TQhFJ-8b1hsR3NXQtiYbhBeHMr3niIJ9iNxkczpAYX2EbH5ITiqvfWTJCP-KO-qi7WBnVWqGJmEekfLVkE3rEvegqQcp_Fb4qpeRIWn3KiXwgn2ZQ&lib=MnKCiT266OA4hOZeBdked70JrcxNO-sv5&empresa={Uri.EscapeDataString(empresa)}";
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


