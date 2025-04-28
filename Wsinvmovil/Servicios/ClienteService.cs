using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Wsinvmovil.Entidad;

namespace Wsinvmovil.Servicios
{
    public class ClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService()
        {
            // Usamos la IP global con protocolo HTTP
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://{GlobalSettings.IpServidor}:5299/") // ← Usando tu variable global
            };
        }

        public async Task<ApiResponse<E_Cliente>> ObtenerClientePorCedula(string cedula)
        {
            var response = new ApiResponse<E_Cliente>();

            try
            {
                var ip = GlobalSettings.IpServidor;
                var bdd = GlobalSettings.NombreBDD;

                string url = $"Cliente/ObtenerPorCedula?ipServidor={ip}&nombreBDD={bdd}&cedula={cedula}";

                var httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var cliente = await httpResponse.Content.ReadFromJsonAsync<E_Cliente>() ?? new E_Cliente();
                    response.Exito = true;
                    response.Resultado = cliente;
                }
                else
                {
                    response.Exito = false;
                    response.MensajeError = await httpResponse.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                response.Exito = false;
                response.MensajeError = $"Error al consultar cliente: {ex.Message}";
            }

            return response;
        }
        public async Task<ApiResponse<E_SaldoCliente>> ObtenerSaldoCliente(int id)
        {
            var response = new ApiResponse<E_SaldoCliente>();

            try
            {

                var ip = GlobalSettings.IpServidor;
                var bdd = GlobalSettings.NombreBDD;
                string url = $"Cliente/ObtenerSaldoCliente?ipServidor={ip}&nombreBDD={bdd}&id={id}";

                var httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var saldo = await httpResponse.Content.ReadFromJsonAsync<E_SaldoCliente>() ?? new E_SaldoCliente();
                    response.Exito = true;
                    response.Resultado = saldo;
                }
                else
                {
                    response.Exito = false;
                    response.MensajeError = await httpResponse.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                response.Exito = false;
                response.MensajeError = $"Error al consultar saldo del cliente: {ex.Message}";
            }

            return response;
        }

    }

}
