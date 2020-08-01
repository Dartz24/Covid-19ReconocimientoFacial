using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public static class UsuarioDatos
    {
       
            public static UsuarioEntidad IniciarSesionDatos(string text1, string text2)
            {
                UsuarioEntidad usuario = new UsuarioEntidad();
                try
                {
                    //TODO CAMBIAR CADENA DE CONEXIOJ
                    SqlConnection conexion = new SqlConnection(Configuracion.Default.ConexionBD);

                    //SqlConnection conexion = new SqlConnection(Settings1.Default.CadenaConexion);
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conexion;
                    cmd.CommandText = @"SELECT  nombre , apellido
                                        FROM [Usuarios] 
                                        WHERE cedula='" + text1 + "'  AND contrasena='" + text2 + "'";


                    cmd.CommandType = CommandType.Text;
                    using (var dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr.HasRows)
                        {
                            usuario.Nombre = dr["nombre"].ToString();
                            usuario.Apellido = dr["apellido"].ToString();
                        }
                    }

                    conexion.Close();
                    return usuario;
                }
                catch (Exception)
                {

                    throw;
                }

            }

        public static UsuarioEntidad GuardarNuevoUsuario(UsuarioEntidad usuario)
        {
            try
            {
                SqlConnection conexion = new SqlConnection(Configuracion.Default.ConexionBD);
                conexion.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"INSERT INTO [dbo].[Usuarios]
                                                           ([cedula]
                                                           ,[nombre]
                                                           ,[apellido]
                                                           ,[contrasena]
                                                           ,[idtiposuario])
                                                     VALUES
                                                           (@cedula,@nombre,@apellido,@contrasena,@idusuario);
                                    SELECT SCOPE_IDENTITY()";

                cmd.Parameters.AddWithValue("@cedula", usuario.Cedula);
                cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
                cmd.Parameters.AddWithValue("@idusuario", TipoDatos.ObtenerIdTipo(usuario.TipoUsuario));
                cmd.Parameters.AddWithValue("@contrasena", usuario.Contrasena);

                usuario.Id = Convert.ToInt32(cmd.ExecuteScalar());

             

                return usuario;


            }
            catch (Exception ex)
            {
                return new UsuarioEntidad { Mensaje = ex.Message };
            }
        }
    }
    }

