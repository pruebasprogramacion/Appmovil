using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsinvmovil.Entidad
{
    public static class GlobalSettings
    {
        public static string IpServidor => Preferences.Get("IPServidor", string.Empty);
        public static string NombreBDD => Preferences.Get("NombreBDD", string.Empty);

        public static string ambiente = Preferences.Get("AmbienteNombre", string.Empty);

        public static string empresa = Preferences.Get("EmpresaNombre", string.Empty);

        public static string razonsocial = Preferences.Get("razonsocial", string.Empty);

        public static string ruc = Preferences.Get("ruc", string.Empty);

        public static string nomempresa = Preferences.Get("nomempresa", string.Empty);
        public static string nomPantalla { get; set; } = string.Empty;

        public static string usuario = Preferences.Get("Usuario", string.Empty);

    }
}

