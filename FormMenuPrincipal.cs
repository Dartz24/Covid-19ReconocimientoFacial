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
    public partial class FormMenuPrincipal : Form
    {
        public UsuarioEntidad UsuarioIngresado { get; set; }
        public FormMenuPrincipal()
        {
            InitializeComponent();
        }

        private void AbrirFormEnPanel(object formulario)
        {
            if (this.panelContenedor.Controls.Count > 0)
                this.panelContenedor.Controls.RemoveAt(0);
            Form fh = formulario as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.None;
            this.panelContenedor.Controls.Add(fh);
            this.panelContenedor.Tag = fh;

            fh.Show();

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void registroPacienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //RegistroPaciente paciente = new RegistroPaciente();
            //AbrirFormEnPanel(paciente);
        }

        private void reconocimientoFacialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ReconocimientoFacial facial = new ReconocimientoFacial();
            //AbrirFormEnPanel(facial);
        }

        private void Minimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }

        private void Maximizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            Maximizar.Visible = false;
            Restaurar.Visible = true;

        }

        private void Restaurar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Restaurar.Visible = false;
            Maximizar.Visible = true;
        }

        private void Cerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();


        }

        private void menuArriba_Click(object sender, EventArgs e)
        {
            if (SideBar.Width == 270)
            {
                SideBar.Width = 68;
                menuIzquierda.Width = 90;
                lineaSeparator.Width = 52;


            }
            else
            {
                SideBar.Width = 270;
                menuIzquierda.Width = 300;
                lineaSeparator.Width = 252;
            }
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            RegistroPaciente paciente = new RegistroPaciente();
            AbrirFormEnPanel(paciente);
           
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            ReconocimientoFacial facial = new ReconocimientoFacial();
            AbrirFormEnPanel(facial);
      
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            Mapa mapa = new Mapa();
            AbrirFormEnPanel(mapa);
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            FormUsuarios usuarios = new FormUsuarios();
            AbrirFormEnPanel(usuarios);
        }
    }
}
