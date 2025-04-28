using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsinvmovil.Entidad
{
    public  class E_Credito
    {
        public string NroCredito { get; set; } = string.Empty;
        public string Fecha { get; set; } = string.Empty;
        public string Factura { get; set; } = string.Empty;
        public string Plazo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public string Saldo { get; set; } = string.Empty;
        public int cabid { get; set; } = 0;

    }

    public class Pago
    {
        public string Fecha { get; set; } = string.Empty;
        public string Recibo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }

    public class DetPago
    {
        public string NroCredito { get; set; } = string.Empty;
        public string Factura { get; set; } = string.Empty;
        public string Fec_Credito { get; set; } = string.Empty;
        public string Pago { get; set; } = string.Empty;
        public string Saldo { get; set; } = string.Empty;
    }

    public class Banco
    {
        public int ID { get; set; } = 0;
        public string nomBanco { get; set; } = string.Empty;
        public override string ToString()
        {
            return nomBanco; // Muestra solo el nombre en el Picker
        }
    }
    public class Articulos
    {
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Stock { get; set; } = "";
        public string Precio { get; set; } = "";

    }

    public class ConexionInfo
    {
        public string ipServidor { get; set; } = "";
        public string nombreBDD { get; set; } = "";
        public string ambiente { get; set; } = "";
        public string error { get; set; } = ""; // para manejar errores también
    }

    public class UsuarioLogin
    {
        public string Usuario { get; set; } = "";
        public string Clave { get; set; } = "";
        public string IpServidor { get; set; } = "";
        public string NombreBDD { get; set; } = "";
    }

    public class BuscadorNom
    {
        public string Nombre { get; set; } = "";
        public string Id { get; set; } = "";
        public string Cedual { get; set; } = "";
    }

    public class RealizarPagoViewModel
    {
        // Estas son las propiedades que estás usando en tu XAML
        public bool MostrarNro { get; set; } = false;
        public bool MostrarTarjeta { get; set; } = false;
        public bool MostrarVoucher { get; set; } = false;
        public bool MostrarBanco { get; set; } = false;

        // Puedes agregar más propiedades si luego necesitas expandir tu lógica
    }
}
