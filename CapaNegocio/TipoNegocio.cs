using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class TipoNegocio
    {
        public static List<TipoEntidad> ObtenerTiposUsuarios()
        {
            return TipoDatos.ObtenerTiposUsuarios();
        }
    }
}
