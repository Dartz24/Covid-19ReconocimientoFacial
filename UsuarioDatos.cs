using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19ReconocimientoFacial
{
   public class UsuarioDatos
    {
        public static List<Usuario> CargarListaUsuario(List<Usuario> listaUsuarios)
        {
            using (var conexion = new SqlConnection(ConfiguracionDB.Default.conexion))
            {
                conexion.Open();

                SqlCommand sql = new SqlCommand();
                sql.Connection = conexion;
                sql.CommandType = CommandType.Text;

                sql.CommandText = @"SELECT [id]
                                          ,[nombre]
                                          ,[apellido]
                                          ,[cedula]
                                          ,[telefono]
                                          ,[email]
                                          ,[fechaNacimiento]
                                          ,[imagen]
                                      FROM [dbo].[Empleado]";

                using (SqlDataReader reader = sql.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] data = (byte[])(reader["imagen"]);

                        MemoryStream mem = new MemoryStream();
                        mem.Write(data, 0, data.Length);
                        Bitmap bit = new Bitmap(mem);


                        Usuario usuario = new Usuario();

                        usuario.Id = Convert.ToInt32(reader["id"].ToString());
                        usuario.Nombre = reader["nombre"].ToString();
                        usuario.Apellido = reader["apellido"].ToString();
                        usuario.Cedula = reader["cedula"].ToString();
                        usuario.Telefono = reader["telefono"].ToString();
                        usuario.Email = reader["email"].ToString();
                        usuario.FechaNacimiento = Convert.ToDateTime(reader["fechaNacimiento"]);
                        usuario.Imagen = bit;

                        listaUsuarios.Add(usuario);

                    }
                }
            }
            return listaUsuarios;
        }

        public static Usuario insertUsuario(Usuario usuario)
        {

            using (var conexion = new SqlConnection(ConfiguracionDB.Default.conexion))
            {
                conexion.Open();
                SqlCommand sql = new SqlCommand();
                sql.Connection = conexion;
                sql.CommandType = CommandType.Text;

                sql.CommandText = @"INSERT INTO [dbo].[Empleado]
                                                   ([nombre]
                                                   ,[apellido]
                                                   ,[cedula]
                                                   ,[telefono]
                                                   ,[email]
                                                   ,[fechaNacimiento]
                                                   ,[imagen])
                                             VALUES
                                                   (@nombre
                                                   ,@apellido
                                                   ,@cedula
                                                   ,@telefono
                                                   ,@email
                                                   ,@fechaNacimiento
                                                   ,@imagen);
                                            SELECT SCOPE_IDENTITY()";

                sql.Parameters.AddWithValue("@nombre", usuario.Nombre);
                sql.Parameters.AddWithValue("@apellido", usuario.Apellido);
                sql.Parameters.AddWithValue("@cedula", usuario.Cedula);
                sql.Parameters.AddWithValue("@telefono", usuario.Telefono);
                sql.Parameters.AddWithValue("@fechaNacimiento", usuario.FechaNacimiento);
                sql.Parameters.AddWithValue("@email", usuario.Email);

                MemoryStream ms = new MemoryStream();
                usuario.Imagen.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                sql.Parameters.AddWithValue("@imagen", ms.ToArray());

                var identificador = Convert.ToInt32(sql.ExecuteScalar());

                usuario.Id = identificador;

                //if (identificador != 0)
                //{
                //    MessageBox.Show(nombre + " Guardado correctamente!\nId: " + identificador);
                //}
                //MessageBox.Show("Id: " + identificador);
            }

            return usuario;
        }

        public static Usuario updateUsuario(Usuario usuario)
        {
            using (var conexion = new SqlConnection(ConfiguracionDB.Default.conexion))
            {
                conexion.Open();
                SqlCommand sql = new SqlCommand();
                sql.Connection = conexion;
                sql.CommandType = CommandType.Text;

                //Usuario usuario = new Usuario(Convert.ToInt32(textBox_update_id.Text),
                //    textBox_update_nombre.Text,
                //    textBox_update_apellido.Text,
                //    textBox_update_telefono.Text,
                //    textBox_update_email.Text,
                //    result);

                //if (actualizaFoto)
                //{
                sql.CommandText = @"UPDATE [dbo].[Empleado]
                                    SET [nombre] = @nombre
                                          ,[apellido] = @apellido
                                          ,[cedula] = @cedula
                                          ,[telefono] = @telefono
                                          ,[email] = @email
                                          ,[fechaNacimiento] = @fechaNacimiento
                                          ,[imagen] = @imagen
                                    WHERE [id]=@id";

                MemoryStream ms = new MemoryStream();
                usuario.Imagen.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                sql.Parameters.AddWithValue("@imagen", ms.ToArray());
                //}
                //else
                //{
                //    sql.CommandText = @"UPDATE [dbo].[Usuario]
                //                    SET [nombre] = @nombre
                //                          ,[apellido] = @apellido
                //                          ,[telefono] = @telefono
                //                          ,[email] = @email
                //                    WHERE [id]=@id";
                //}

                sql.Parameters.AddWithValue("@nombre", usuario.Nombre);
                sql.Parameters.AddWithValue("@apellido", usuario.Apellido);
                sql.Parameters.AddWithValue("@cedula", usuario.Cedula);
                sql.Parameters.AddWithValue("@telefono", usuario.Telefono);
                sql.Parameters.AddWithValue("@email", usuario.Email);
                sql.Parameters.AddWithValue("@fechaNacimiento", usuario.FechaNacimiento);
                sql.Parameters.AddWithValue("@id", usuario.Id);

                var identificador = Convert.ToInt32(sql.ExecuteNonQuery());

                return usuario;
                //if (identificador >= 0)
                //{
                //}

                //CargarDesdeDB();
            }
        }
    }
}
