using System.Net.Http;
using System.Threading.Tasks;
using System;
using Wsinvmovil.Servicios;
public class LoginService
{
    private readonly HttpClient _httpClient;

    public LoginService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<ApiResponse<bool>> ValidarLoginAsync(string usuario, string clave, string ipServidor, string nombreBDD)
    {
        var respuesta = new ApiResponse<bool>();

        try
        {
            string url = $"http://{ipServidor}:5299/ValidarLogin?usuario={usuario}&clave={clave}&ipServidor={ipServidor}&nombreBDD={nombreBDD}";

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (bool.TryParse(content, out bool resultado))
                {
                    respuesta.Exito = resultado;
                    respuesta.Resultado = resultado;

                    if (!resultado)
                        respuesta.MensajeError = "Usuario o clave incorrectos, o no autorizado.";
                }
                else
                {
                    respuesta.Exito = false;
                    respuesta.MensajeError = "Formato de respuesta no válido del servidor.";
                }
            }
            else
            {
                respuesta.Exito = false;
                respuesta.MensajeError = $"Error de servidor: {response.StatusCode}";
            }
        }
        catch (HttpRequestException ex)
        {
            respuesta.Exito = false;
            respuesta.MensajeError = $"Error de conexión: {ex.Message}";
        }
        catch (Exception ex)
        {
            respuesta.Exito = false;
            respuesta.MensajeError = $"Error inesperado: {ex.Message}";
        }

        return respuesta;
    }

}

