using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19ReconocimientoFacial
{
   public class PacientesEntidades
    {




        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaNacimiento { get; set; }
        // public bit Imagen { get; set; }

        public Byte Imagen { get; set; }

        public PacientesEntidades()
        {
        }

        public PacientesEntidades(string cedula, string nombre, string apellido, string telefono, string email, DateTime fechaNacimiento, Byte imagen)
        {
            Cedula = cedula;
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
            Email = email;
            FechaNacimiento = fechaNacimiento;
            Imagen = imagen;
        }
    }
}
