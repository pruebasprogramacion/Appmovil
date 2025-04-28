using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsinvmovil.Entidad
{
    public class E_ConsultaPago
    {
        public string recibo { get; set; } = string.Empty;
        public DateTime fecha { get; set; }
        public string tipo { get; set; } = string.Empty;
        public decimal valor { get; set; }
        public string Ref { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
        public string usuario { get; set; } = string.Empty;
        public DateTime fecRegistro { get; set; }
    }

    public class E_DetallePago
    {
        public string nroCredito { get; set; } = string.Empty;
        public string factura { get; set; } = string.Empty;
        public DateTime fec_Credito { get; set; }
        public decimal pago { get; set; }
        public decimal saldo { get; set; }
        public int idFactura { get; set; }
    }

    public class E_ClienteBasico
    {
        public int id { get; set; }
        public string cliente { get; set; } = string.Empty;
        public string cedula { get; set; } = string.Empty;
    }

    public class E_ClienteNom
    {
        public int id { get; set; }
        public string cliente { get; set; } = string.Empty;
    }

    public class E_CreditoCliente
    {
        public string credito { get; set; } = "";
        public string cliente { get; set; } = "";
        public DateTime fecha { get; set; }
        public string factura { get; set; } = "";
        public int plazo { get; set; }
        public decimal valor { get; set; }
        public decimal saldo { get; set; } = 0;
        public int cliId { get; set; }
        public string estado { get; set; } = "";
        public int cabId { get; set; }
    }

    public class EPago
    {
        public int pagc_id { get; set; }                  // ID del pago (puede ser 0 para nuevo)
        public int pag_idcredito { get; set; }            // ID del crédito asociado
        public double pagc_pago { get; set; }             // Monto del pago
        public int pagc_banco { get; set; }               // ID del banco (si aplica)
        public string pagc_fecha { get; set; } = "";      // Fecha del pago en string, ideal formato yyyy-MM-dd
        public string pagc_tipo { get; set; } = "";       // Tipo de pago: EFECTIVO, CHEQUE, etc.
        public string pagc_usuario { get; set; } = "";    // Usuario que realiza el pago
        public double pagc_saldo { get; set; } = 0;       // Saldo restante (opcional, si no se usa lo puedes quitar)
        public int pagc_idcli { get; set; }               // ID del cliente
        public string pagc_nombre { get; set; } = "";     // Nombre del cliente
        public string pagc_nro { get; set; } = "";        // Nro de cheque, transferencia, etc.
    }
}
