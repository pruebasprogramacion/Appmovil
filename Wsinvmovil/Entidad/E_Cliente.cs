using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsinvmovil.Entidad
{
    public class E_Cliente
    {
        public string id { get; set; } = "";
        public string cedula { get; set; } = "";
        public string nombre { get; set; } = "";
        public string direccion { get; set; } = "";
        public string ciudad { get; set; } = "";
        public string telefono { get; set; } = "";
        public string celular { get; set; } = "";
        public string correo { get; set; } = "";
        public string observacion { get; set; } = "";
        public DateTime? freal { get; set; } 
        public DateTime? fnacimiento { get; set; }
        public int grupoP { get; set; }
        public decimal credito { get; set; }
        public int tiempo { get; set; }
    }
    public class E_SaldoCliente
    {
        public decimal saldo { get; set; }
        public int dias { get; set; }
    }
}
