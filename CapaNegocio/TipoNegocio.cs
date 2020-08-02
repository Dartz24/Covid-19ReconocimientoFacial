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

     


        public static List<TipoPaciente> BuscarTipo(string texto)
        {
            List<TipoPaciente> tipos = new List<TipoPaciente>();

            tipos = TipoDatos.BuscarTipo("%" + texto + "%");
           
            for (int i = 0; i < tipos.Count; i++)
            {
                //Clientes[i].VentaCabezeraEntidad = Facturar.BuscarServiciosAdquiridos(Clientes[i].Id);
            }
            return tipos;
        }
    }
}
