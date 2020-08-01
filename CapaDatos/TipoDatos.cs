using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidades;

namespace CapaDatos
{
    public class TipoDatos
    {
        public static List<TipoEntidad> ObtenerTiposUsuarios()
        {
            try
            {
                List<TipoEntidad> lstTitulos = new List<TipoEntidad>();
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
                        TipoEntidad tipo = new TipoEntidad
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
                List<TipoEntidad> listaTipos = new List<TipoEntidad>();
                listaTipos.Add(new TipoEntidad { Mensaje = ex.Message });
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
