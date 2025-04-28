using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Wsinvmovil.Entidad;

namespace Wsinvmovil.Views
{
    public class MostrarComprobanteWebViewPrincipal
    {
        public static async Task MostrarComprobanteWebView(INavigation navigation, EPago objEntPagos, string montoPago)
        {
            // Obtener datos globales
            string razonSocial = GlobalSettings.razonsocial;
            string ruc = GlobalSettings.ruc;

            // Construir contenido HTML
            string htmlContent = $@"
            <html>
                <head>
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Comprobante de Pago</title>
                    <style>
                        html, body {{
                            height: 100%;
                            margin: 0;
                            padding: 0;
                            overflow: auto;
                            -webkit-overflow-scrolling: touch;
                        }}

                        body {{
                            font-family: Arial, sans-serif;
                            text-align: center;
                        }}
                        .container {{
                            width: 80%;
                            margin: auto;
                            padding: 20px;
                            border: 1px solid black;
                            border-radius: 10px;
                        }}

                        .header {{
                            font-size: 18px;
                            font-weight: bold;
                            margin-bottom: 10px;
                        }}

                        .ruc {{
                            font-size: 16px;
                            font-weight: bold;
                            margin-bottom: 20px;
                        }}

                        button {{
                            padding: 10px 20px;
                            font-size: 16px;
                            margin-top: 20px;
                            cursor: pointer;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>{razonSocial}</div>
                        <div class='ruc'>RUC: {ruc}</div>
                        <h2>Comprobante de Pago</h2>
                        <p><b>Nro. Comprobante:</b> {objEntPagos.pagc_id}</p>
                        <p><b>Cliente:</b> {objEntPagos.pagc_nombre}</p>
                        <p><b>Monto:</b> {montoPago}</p>
                        <p><b>Fecha:</b> {objEntPagos.pagc_fecha}</p>
                        <p><b>Forma de Pago:</b> {objEntPagos.pagc_tipo}</p>
                        <button onclick='window.print()'>🖨️ Imprimir Comprobante</button>
                    </div>
                </body>
            </html>";

            // Abrir la página con WebView para mostrar el HTML
            await navigation.PushModalAsync(new ComprobantePage(htmlContent));
        }
    }
}

