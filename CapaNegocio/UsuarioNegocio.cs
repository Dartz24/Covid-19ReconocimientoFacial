using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using CapaDatos;

namespace CapaNegocio
{
    public static class UsuarioNegocio
    {
        public static UsuarioEntidad IniciarSesionNegocio(string text1, string text2)
        {
            return UsuarioDatos.IniciarSesionDatos(text1, text2);
        }
    }
}
