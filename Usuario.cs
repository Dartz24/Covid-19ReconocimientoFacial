using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19ReconocimientoFacial
{
   public class Usuario
    {

//clases
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Image Imagen { get; set; }
        public int IdTipo { get; set; }

        public Usuario()
        {
        }

        public Usuario(string cedula, string nombre, string apellido, string telefono, string email, DateTime fechaNacimiento, Image imagen, int idTipo)
        {
            Cedula = cedula;
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
            Email = email;
            FechaNacimiento = fechaNacimiento;
            Imagen = imagen;
            IdTipo = idTipo;
        }
    }
}
