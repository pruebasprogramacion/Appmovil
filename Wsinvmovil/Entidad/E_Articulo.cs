using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsinvmovil.Entidad
{
    public class E_Articulo
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = "";
        public string Codigo1 { get; set; } = "";
        public string Codigo2 { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public string Marca { get; set; } = "";
        public decimal Costo { get; set; } = decimal.Zero;
        public string Ubicacion { get; set; } = "";
        public decimal Exis { get; set; } = decimal.Zero;
        public decimal Rese { get; set; } = decimal.Zero;
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; } = decimal.Zero;
        public decimal Precio1 { get; set; } = decimal.Zero;
        public decimal Precio2 { get; set; } = decimal.Zero;
        public string Observacion { get; set; } = "";
        public string TarifaIva { get; set; } = "";
        public string Serie { get; set; } = "";
        public bool Combo { get; set; }
        public bool Servicio { get; set; }
        public string Tipo { get; set; } = "";
        public string Imagen { get; set; } = "";
        public string Link { get; set; } = "";
        public string Descorta { get; set; } = "";
    }
}
