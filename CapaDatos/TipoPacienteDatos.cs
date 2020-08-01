using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;

namespace CapaDatos
{
   public class TipoPacienteDatos
    {

        public static List<TipoPaciente> ObtenerTiposUsuarios()
        {
            try
            {
                List<TipoPaciente> lstTitulos = new List<TipoPaciente>();
                SqlConnection conexion = new SqlConnection(Configuracion.Default.ConexionBD);
                conexion.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"SELECT * FROM TipoUsuarios";

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        TipoPaciente tipo = new TipoPaciente
                        {
                            Id = dr["id"].ToString(),
                            TipoUsuario = dr["tipousuario"].ToString()
                        };
                        lstTitulos.Add(tipo);
                    }
                }

                return lstTitulos;

            }
            catch (Exception ex)
            {
                List<TipoPaciente> listaTipos = new List<TipoPaciente>();
                listaTipos.Add(new TipoPaciente { Mensaje = ex.Message });
                return listaTipos;
            }
        }


        internal static object ObtenerIdTipo(string TipoUsuario)
        {
            try
            {

                string id = String.Empty;

                SqlConnection conexion = new SqlConnection(Configuracion.Default.ConexionBD);
                conexion.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"SELECT * FROM TipoUsuarios WHERE tipousuario=@nombre";

                cmd.Parameters.AddWithValue("@nombre", TipoUsuario);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        id = dr["id"].ToString();
                    }
                }

                return id;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


    }
}
}
