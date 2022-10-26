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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // cargar el comboBox con los proveedores
            CargarProveedores(cmbProveedores);
        }

        private void CargarProveedores(ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                using (var db = new ModeloAdministracion())
                {
                    // consultar los proveedores
                    var query = from p in db.Proveedores
                                orderby p.ProveedorId
                                select p;
                    // crear una tabla temporal para enlazar con el comboBox
                    DataTable table = new DataTable();
                    table.Columns.Add("ProveedorId", typeof(int));
                    table.Columns.Add("Nombre", typeof(string));
                    foreach (var p in query)
                    { // agregar los registros a la tabla temporal
                        DataRow dr = table.NewRow();
                        dr["ProveedorId"] = p.ProveedorId;
                        dr["Nombre"] = p.Nombre;
                        table.Rows.Add(dr);
                    }
                    // enlazar la tabla con el ComoBox
                    cmb.DisplayMember = "Nombre";
                    cmb.ValueMember = "ProveedorId";
                    cmb.DataSource = table;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            int proveedorId = (int)cmbProveedores.SelectedValue;
            CargarArticulos(dgvArticulos, proveedorId);
        }

        private void CargarArticulos(DataGridView grArt, int proveedorId)
        {
            try
            {
                using (var db = new ModeloAdministracion())
                {
                    // consultar los articulos del proveedor seleccionado
                    var query = from a in db.Articulos
                                where a.ProveedorId == proveedorId
                                orderby a.ArticuloId
                                select a;
                    grArt.Rows.Clear();
                    foreach (var a in query)
                    {
                        grArt.Rows.Add(a.ArticuloId.ToString(), a.Nombre,
                        a.Precio.ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void cmbProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvArticulos.Rows.Clear(); // limpiar la grilla
        }

        private void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            Reporte rpt = new Reporte(saveDlg);
            //
            if (rpt.SeleccionarArchivo() != "")
            {
                if (rpt.GenerarReporte((int)cmbProveedores.SelectedValue))
                {
                    MessageBox.Show("Reporte generado correctamente", "Reporte",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Error generando el Reporte", "Reporte",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
