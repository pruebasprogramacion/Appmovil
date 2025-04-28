using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsinvmovil.Servicios
{
    public class ApiResponse<T>
    {
        public bool Exito { get; set; }
        public string MensajeError { get; set; } = string.Empty;
        public T Resultado { get; set; } = default!;
    }

}
