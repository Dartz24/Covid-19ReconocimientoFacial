using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class UsuarioEntidad
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string IdTipoUsuario { get; set; }

        public string TipoUsuario { get; set; }

        public string Contrasena { get; set; }

        public string Mensaje { get; set; }
    }
}
