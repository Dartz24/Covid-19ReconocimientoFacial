using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Covid_19ReconocimientoFacial
{
    public partial class inputDatos : Form
    {

        public Usuario usuario = new Usuario();

        public inputDatos()
        {
            InitializeComponent();
        }


        //internal Usuario Usuario { get => usuario; set => usuario = value; }

        private void button1_Click(object sender, EventArgs e)
        {
            usuario.Cedula = textBox_cedula.Text;
            usuario.Nombre = textBox_nombre.Text;
            usuario.Apellido = textBox_apellido.Text;
            usuario.Email = textBox_emil.Text;
            usuario.Telefono = textBox_telefono.Text;
            usuario.FechaNacimiento = dateTimePicker_fechaNacimiento.Value;

            this.Close();
        }

        internal void setDatos(Usuario usuario)
        {
            this.usuario.Id = usuario.Id;
            textBox_cedula.Text = usuario.Cedula;
            textBox_nombre.Text = usuario.Nombre;
            textBox_apellido.Text = usuario.Apellido;
            textBox_telefono.Text = usuario.Telefono;
            textBox_emil.Text = usuario.Email;
            dateTimePicker_fechaNacimiento.Value = usuario.FechaNacimiento;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            usuario.Cedula = textBox_cedula.Text;
            usuario.Nombre = textBox_nombre.Text;
            usuario.Apellido = textBox_apellido.Text;
            usuario.Email = textBox_emil.Text;
            usuario.Telefono = textBox_telefono.Text;
            usuario.FechaNacimiento = dateTimePicker_fechaNacimiento.Value;

            this.Close();
        }

        private void cerrar_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
            return;
        }

        private void restaurar_Click(object sender, EventArgs e)
        {
            //Restaurar
            WindowState = FormWindowState.Normal;
            restaurar.Visible = false;
            pictureBox4.Visible = true;
        }

        private void minimizar_Click(object sender, EventArgs e)
        {
            //minimizar
            WindowState = FormWindowState.Minimized;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //maximizar
            WindowState = FormWindowState.Maximized;
            pictureBox4.Visible = false;
            restaurar.Visible = true;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
