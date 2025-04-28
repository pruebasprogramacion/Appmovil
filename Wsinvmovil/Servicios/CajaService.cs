using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Wsinvmovil.Entidad;

namespace Wsinvmovil.Servicios
{
    public class CajaService
    {
        private readonly HttpClient _httpClient;

        public CajaService()
        {
            // Usa la IP configurada globalmente con protocolo HTTP
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://{GlobalSettings.IpServidor}:5299/")
            };
        }

        public async Task<ApiResponse<int>> InsertarCajaCredAbo(E_Caja caja)
        {
            var respuesta = new ApiResponse<int>();

            try
            {
                var ip = GlobalSettings.IpServidor;
                var bdd = GlobalSettings.NombreBDD;
                string url = $"Caja/InsertarCajaCredAbo?ipServidor={ip}&nombreBDD={bdd}";

                var httpResponse = await _httpClient.PostAsJsonAsync(url, caja);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();

                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("caj_id", out var idElement) && idElement.TryGetInt32(out int caj_id))
                    {
                        respuesta.Exito = true;
                        respuesta.Resultado = caj_id;
                    }
                    else
                    {
                        respuesta.Exito = false;
                        respuesta.MensajeError = "No se pudo obtener el ID generado de caja.";
                    }
                }
                else
                {
                    respuesta.Exito = false;
                    respuesta.MensajeError = $"Error HTTP: {httpResponse.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                respuesta.Exito = false;
                respuesta.MensajeError = $"Excepción: {ex.Message}";
            }

            return respuesta;
        }
        public async Task<ApiResponse<List<Dictionary<string, object>>>> ConsultarTransferencia(string nro, string banco)
        {
            var response = new ApiResponse<List<Dictionary<string, object>>>();

            try
            {
                var ip = GlobalSettings.IpServidor;
                var bdd = GlobalSettings.NombreBDD;

                string url = $"Caja/ConsultarTransferencia?nro={Uri.EscapeDataString(nro)}&banco={Uri.EscapeDataString(banco)}&ipServidor={ip}&nombreBDD={bdd}";

                var httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<Dictionary<string, object>>();

                    response.Exito = true;
                    response.Resultado = resultado;
                }
                else
                {
                    response.Exito = false;
                    response.MensajeError = $"Error HTTP: {httpResponse.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                response.Exito = false;
                response.MensajeError = $"Excepción: {ex.Message}";
            }

            return response;
        }

        public async Task<ApiResponse<List<Banco>>> ObtenerBancos()
        {
            var respuesta = new ApiResponse<List<Banco>>();
            try
            {
                var ip = GlobalSettings.IpServidor;
                var bdd = GlobalSettings.NombreBDD;
                var url = $"Caja/ObtenerBancos?ipServidor={ip}&nombreBDD={bdd}";

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var bancos = JsonSerializer.Deserialize<List<Banco>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                    respuesta.Exito = true;
                    respuesta.Resultado = bancos;
                }
                else
                {
                    respuesta.Exito = false;
                    respuesta.MensajeError = $"Error HTTP: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                respuesta.Exito = false;
                respuesta.MensajeError = $"Excepción: {ex.Message}";
            }
            return respuesta;
        }

    }
}

