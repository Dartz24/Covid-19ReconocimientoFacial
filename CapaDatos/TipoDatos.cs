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
                            Id =Convert.ToInt32( dr["id"].ToString()),
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


       public static object ObtenerIdTipo(string TipoUsuario)
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



        public static object ObtenerTipo(int TipoUsuario)
        {
            try
            {

                string id = String.Empty;

                SqlConnection conexion = new SqlConnection(Configuracion.Default.ConexionBD);
                conexion.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"SELECT * FROM EstadoPaciente WHERE id=@nombre";

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






        public static List<TipoPaciente> BuscarTipo(string texto)
        {
            List<TipoPaciente> lista = new List<TipoPaciente>();
            using (SqlConnection cn = new SqlConnection(Configuracion.Default.ConexionBD))
            {
                cn.Open();
                string sql = @"SELECT [id]
                                      ,[estado]
                                  FROM [dbo].[EstadoPaciente]
                    where id like @texto";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@texto", texto);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(CargarTipo(reader));
                }
            }
            return lista;
        }

        private static TipoPaciente CargarTipo(SqlDataReader reader)
        {
            TipoPaciente cliente = new TipoPaciente();
            cliente.Id = Convert.ToInt32(reader["id"]);
            cliente.TipoUsuario = Convert.ToString(reader["estado"]);

            return cliente;
        }

    }
}
