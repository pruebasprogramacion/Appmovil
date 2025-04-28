using System.Text.Json;
using Wsinvmovil.Entidad;
using Wsinvmovil.Servicios;
public class EmpresaService
{
    public async Task<ApiResponse<E_Empresa?>> ObtenerDatosEmpresa(string ipServidor, string nombreBDD)
    {
        var resultado = new ApiResponse<E_Empresa?>();

        try
        {
            var url = $"http://{ipServidor}:5299/Empresa?ipServidor={ipServidor}&nombreBDD={nombreBDD}";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var empresa = JsonSerializer.Deserialize<E_Empresa>(contenido);

                if (empresa != null)
                {
                    // Guardar valores en Preferences
                    Preferences.Set("ruc", empresa.ruc);
                    Preferences.Set("razonsocial", empresa.razonSocial);
                    Preferences.Set("nomempresa", empresa.nombre);
                    resultado.Exito = true;
                    resultado.Resultado = empresa;
                }
                else
                {
                    resultado.Exito = false;
                    resultado.MensajeError = "No se encontró la información de la empresa.";
                }
            }
            else
            {
                resultado.Exito = false;
                resultado.MensajeError = $"Error al consultar empresa: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            resultado.Exito = false;
            resultado.MensajeError = $"Excepción: {ex.Message}";
        }

        return resultado;
    }

}

