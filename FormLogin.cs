using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidades;
using CapaNegocio;


namespace Covid_19ReconocimientoFacial
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void IniciarSesion(string text1, string text2)
        {
            //Negocio.IniciarSesion_Negocio(text1, text2);

            if (!String.IsNullOrEmpty(txtUsuario.Text) && !String.IsNullOrEmpty(txtContraseña.Text))
            {
                try
                {
                    UsuarioEntidad usuario = new UsuarioEntidad();
                    usuario = UsuarioNegocio.IniciarSesionNegocio(text1, text2);

                    if (!String.IsNullOrEmpty(usuario.Nombre))
                    {
                        FormMenuPrincipal p = new FormMenuPrincipal();
                       p.UsuarioIngresado = usuario;
                       p.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Datos Incorrectos." + usuario.Nombre, "Mensaje de Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtUsuario.Focus();
                    }
                }
                catch
                {
                    MessageBox.Show("Error", "Mensaje de Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsuario.Focus();
                }
            }
            else
            {
                MessageBox.Show("Ingrese todos los datos", "Mensaje de Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuario.Focus();
            }

        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            IniciarSesion(txtUsuario.Text, txtContraseña.Text);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text=="USUARIO")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.LightGray;
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (txtUsuario.Text=="")
            {
                txtUsuario.Text = "USUARIO";
                txtUsuario.ForeColor = Color.DimGray;

            }
        }

        private void txtContraseña_Enter(object sender, EventArgs e)
        {
            if (txtContraseña.Text == "CONTRASEÑA")
            {
                txtContraseña.Text = "";
                txtContraseña.ForeColor = Color.LightGray;
                txtContraseña.UseSystemPasswordChar = true;
            }
        }

        private void txtContraseña_Leave(object sender, EventArgs e)
        {

            if (txtContraseña.Text == "")
            {
                txtContraseña.Text = "CONTRASEÑA";
                txtContraseña.ForeColor = Color.DimGray;
                txtContraseña.UseSystemPasswordChar = false;
            }

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
    }
}
