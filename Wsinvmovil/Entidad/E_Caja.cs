using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsinvmovil.Entidad
{
    public class E_Caja
    {
        public int caj_id { get; set; }
        public int caj_idfactura { get; set; }
        public string caj_medio { get; set; } = string.Empty;
        public string caj_nro { get; set; } = string.Empty;
        public string caj_banco { get; set; } = string.Empty;
        public double caj_valor { get; set; }
        public string caj_fecha { get; set; } = string.Empty;
        public string caj_tarjeta { get; set; } = string.Empty;
        public string caj_boucher { get; set; } = string.Empty;
        public string caj_usuario { get; set; } = string.Empty;
        public string caj_tipo { get; set; } = string.Empty;
    }

}
