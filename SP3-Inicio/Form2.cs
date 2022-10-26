using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        // eventos Click de los items del menú
        private void administrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1(); // formulario de Proveedores
            frm.ShowDialog();
        }

        private void administrarArtículosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3(); // formulario de Articulos
            frm.ShowDialog();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void generarReporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 frm = new Form4(); // formulario del Reporte
            frm.ShowDialog();
        }
    }
}
