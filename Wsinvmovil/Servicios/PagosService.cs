using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wsinvmovil.Entidad;

namespace Wsinvmovil.Servicios
{
        public class PagosService
        {
            private readonly HttpClient _httpClient;

            public PagosService()
            {
                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri($"http://{GlobalSettings.IpServidor}:5299/")
                };
            }

            public async Task<ApiResponse<List<E_ConsultaPago>>> ObtenerPagosPorCliente(string idcliente)
            {
                var respuesta = new ApiResponse<List<E_ConsultaPago>>();
                try
                {
                    var url = $"Pagos/ConsultarPagos?ipServidor={GlobalSettings.IpServidor}&nombreBDD={GlobalSettings.NombreBDD}&idprov={Uri.EscapeDataString(idcliente)}";
                    var response = await _httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var pagos = await response.Content.ReadFromJsonAsync<List<E_ConsultaPago>>() ?? new List<E_ConsultaPago>();
                        respuesta.Exito = true;
                        respuesta.Resultado = pagos;
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

            public async Task<ApiResponse<List<E_DetallePago>>> ObtenerDetallePago(string idPago)
            {
                var respuesta = new ApiResponse<List<E_DetallePago>>();
                try
                {
                    var url = $"Pagos/ObtenerDetallePago?idpago={idPago}&ipServidor={GlobalSettings.IpServidor}&nombreBDD={GlobalSettings.NombreBDD}";
                    var response = await _httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var detalle = await response.Content.ReadFromJsonAsync<List<E_DetallePago>>() ?? new List<E_DetallePago>();
                        respuesta.Exito = true;
                        respuesta.Resultado = detalle;
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

            public async Task<ApiResponse<E_ClienteBasico?>> ObtenerClientePorCedula(string cedula)
            {
                var respuesta = new ApiResponse<E_ClienteBasico?>();
                try
                {
                    var url = $"Pagos/ObtenerClientePorCedula?ipServidor={GlobalSettings.IpServidor}&nombreBDD={GlobalSettings.NombreBDD}&cedula={Uri.EscapeDataString(cedula)}";
                    var response = await _httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var cliente = await response.Content.ReadFromJsonAsync<E_ClienteBasico>();
                        respuesta.Exito = true;
                        respuesta.Resultado = cliente;
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

            public async Task<ApiResponse<List<E_ClienteNom>>> ObtenerClientePorNom(string id, string nomcli)
            {
                var respuesta = new ApiResponse<List<E_ClienteNom>>();
                try
                {
                    var url = $"Pagos/BuscarClientePorNombre?ipServidor={GlobalSettings.IpServidor}&nombreBDD={GlobalSettings.NombreBDD}&id={id}&proveedor={Uri.EscapeDataString(nomcli)}";
                    var response = await _httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var listaclientes = await response.Content.ReadFromJsonAsync<List<E_ClienteNom>>() ?? new List<E_ClienteNom>();
                        respuesta.Exito = true;
                        respuesta.Resultado = listaclientes;
                    }
                    else
                    {
                        respuesta.Exito = false;
                        respuesta.MensajeError = $"Error al consultar por nombre del cliente: {response.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.Exito = false;
                    respuesta.MensajeError = $"Excepción: {ex.Message}";
                }

                return respuesta;
            }

            public async Task<ApiResponse<List<E_CreditoCliente>>> ObtenerCreditosCliente(int idcliente)
            {
                var respuesta = new ApiResponse<List<E_CreditoCliente>>();
                try
                {
                    var url = $"Pagos/ObtenerCreditosCliente?ipServidor={GlobalSettings.IpServidor}&nombreBDD={GlobalSettings.NombreBDD}&cliente={idcliente}";
                    var response = await _httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var lista = await response.Content.ReadFromJsonAsync<List<E_CreditoCliente>>() ?? new List<E_CreditoCliente>();
                        respuesta.Exito = true;
                        respuesta.Resultado = lista;
                    }
                    else
                    {
                        respuesta.Exito = false;
                        respuesta.MensajeError = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    respuesta.Exito = false;
                    respuesta.MensajeError = $"Error al consultar créditos: {ex.Message}";
                }

                return respuesta;
            }

        public async Task<ApiResponse<int>> GuardarPagoVenta(EPago pago)
        {
            var respuesta = new ApiResponse<int>();

            try
            {
                var ip = GlobalSettings.IpServidor;
                var bdd = GlobalSettings.NombreBDD;

                string url = $"Pagos/GuardarPagoVenta?ipServidor={ip}&nombreBDD={bdd}";

                var httpResponse = await _httpClient.PostAsJsonAsync(url, pago);



                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();

                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("pagc_id", out var idElement) && idElement.TryGetInt32(out int pagc_id))
                    {
                        respuesta.Exito = true;
                        respuesta.Resultado = pagc_id;
                    }
                    else
                    {
                        respuesta.Exito = false;
                        respuesta.MensajeError = "No se pudo obtener el ID generado del pago.";
                    }
                }
                else
                {
                    respuesta.Exito = false;

                    var errorContent = await httpResponse.Content.ReadAsStringAsync();
                    respuesta.MensajeError = $"Error HTTP {httpResponse.StatusCode}: {errorContent}";
                    //respuesta.MensajeError = $"Error HTTP: {httpResponse.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                respuesta.Exito = false;
                respuesta.MensajeError = $"Excepción: {ex.Message}";


            }

            return respuesta;
        }


        public async Task<ApiResponse<bool>> ActualizarNumeroCaja(string id, string caja)
        {
            var respuesta = new ApiResponse<bool>();

            try
            {
                var ip = GlobalSettings.IpServidor;
                var bdd = GlobalSettings.NombreBDD;

                string url = $"Pagos/ActualizaPagosNumCaja?ipServidor={ip}&nombreBDD={bdd}&id={Uri.EscapeDataString(id)}&caja={Uri.EscapeDataString(caja)}";

                var httpResponse = await _httpClient.PostAsync(url, null); // POST sin cuerpo

                if (httpResponse.IsSuccessStatusCode)
                {
                    respuesta.Exito = true;
                    respuesta.Resultado = true;
                }
                else
                {
                    var mensaje = await httpResponse.Content.ReadAsStringAsync();
                    respuesta.Exito = false;
                    respuesta.MensajeError = $"Error HTTP: {httpResponse.StatusCode} - {mensaje}";
                }
            }
            catch (Exception ex)
            {
                respuesta.Exito = false;
                respuesta.MensajeError = $"Excepción: {ex.Message}";
            }

            return respuesta;
        }

        public async Task<ApiResponse<bool>> InsertarDetallePagos(EPago objPagos)
        {
            var respuesta = new ApiResponse<bool>();

            try
            {
                var ip = GlobalSettings.IpServidor;
                var bdd = GlobalSettings.NombreBDD;

                string url = $"Pagos/InsertarDetallePagos?ipServidor={ip}&nombreBDD={bdd}";

                var httpResponse = await _httpClient.PostAsJsonAsync(url, objPagos);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var resultado = await httpResponse.Content.ReadAsStringAsync();
                    respuesta.Exito = resultado.Contains("ok", StringComparison.OrdinalIgnoreCase);
                    respuesta.Resultado = respuesta.Exito;
                }
                else
                {
                    var error = await httpResponse.Content.ReadAsStringAsync();
                    respuesta.Exito = false;
                    respuesta.MensajeError = $"Error HTTP: {httpResponse.StatusCode} - {error}";
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
