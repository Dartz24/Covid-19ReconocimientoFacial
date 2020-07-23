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
            RegistroPaciente paciente = new RegistroPaciente();
            AbrirFormEnPanel(paciente);
        }

        private void reconocimientoFacialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReconocimientoFacial facial = new ReconocimientoFacial();
            AbrirFormEnPanel(facial);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Mapa map = new Mapa();
            AbrirFormEnPanel(map);
        }
    
    }
}
