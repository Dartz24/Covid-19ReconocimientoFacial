using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidades;

namespace CapaNegocio
{
   public class TipoPacienNegocio
    {
        public static List<TipoPaciente> ObtenerTiposUsuarios()
        {
            return TipoPacienteDatos.ObtenerTiposUsuarios();
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
