using CapaNegocio;
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
    public partial class FormUsuarios : Form
    {
        public FormUsuarios()
        {
            InitializeComponent();
        }

        private void btnCerrrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void BloquearCampos()
        {
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtCedula.Enabled = false;
            txtContraseña.Enabled = false;
            comboBoxCargo.Enabled = false;
            btnNuevo.Enabled = true;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
           // btnSalir.Enabled = true;
        }

        public void limpiarCampos()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtCedula.Text = "";
            txtContraseña.Text = "";
            comboBoxCargo.Text = "";
            btnNuevo.Enabled = true;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = false;
        }
       

        public void NuevoRegistro()
        {
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtCedula.Enabled = true;
            txtContraseña.Enabled = true;
            comboBoxCargo.Enabled = true;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
        }

        private void CargarComboTipoUsuario()
        {
            List<TipoEntidad> listaTipos = TipoNegocio.ObtenerTiposUsuarios();
            if (String.IsNullOrEmpty(listaTipos[0].Mensaje))
            {
                comboBoxCargo.Items.Clear();
                comboBoxCargo.Items.Add("---Seleccionar---");
                foreach (TipoEntidad item in listaTipos)
                {
                    comboBoxCargo.Items.Add(item.TipoUsuario);
                }

                comboBoxCargo.SelectedIndex = 0;

            }
        }


        private void GuardarNuevoUsuario()
        {
            UsuarioEntidad usuario = new UsuarioEntidad
            {
                Id = 0,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Cedula = txtCedula.Text,
                TipoUsuario = comboBoxCargo.SelectedItem.ToString(),
                Contrasena = txtContraseña.Text

            };

            usuario = UsuarioNegocio.GuardarNuevoUsuario(usuario);
            if (String.IsNullOrEmpty(usuario.Mensaje))
            {
                MessageBox.Show("Usuario Creado", "Insertar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(usuario.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormUsuarios_Load(object sender, EventArgs e)
        {
            BloquearCampos();
            CargarComboTipoUsuario();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            NuevoRegistro();
            limpiarCampos();
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarCampos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarNuevoUsuario();
        }
    }
}
