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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            txtNombre.Text = "";
            txtPrecio.Text = "";
            txtArticuloId.Text = "";
            // cargar el comboBox con los proveedores
            CargarProveedores(cmbProveedores);
            // consultar los artículos
            ConsultarArticulos(dgvArticulos);
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

        private void ConsultarArticulos(DataGridView grArt)
        {
            try
            {
                using (var db = new ModeloAdministracion())
                {
                    // consultar los articulos
                    var query = from a in db.Articulos
                                orderby a.ArticuloId
                                select a;
                    grArt.Rows.Clear();
                    foreach (var a in query)
                    {
                        grArt.Rows.Add(a.ArticuloId.ToString(),
                        a.Nombre,
                        a.Precio.ToString(),
                       a.ProveedorId.ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNombre.Text != "" && txtPrecio.Text != "" && cmbProveedores.SelectedIndex != -1)
                {
                    Single precio = 0;
                    Single.TryParse(txtPrecio.Text, out precio);
                    if (precio > 0)
                    {
                        using (var db = new ModeloAdministracion())
                        {
                            // obtener el identificador del proveedor seleccionado
                            int proveedorId = (int)cmbProveedores.SelectedValue;
                            // Agregar un nuevo Artículo
                            db.Add(new Articulo
                            {
                                Nombre = txtNombre.Text,
                                Precio = precio,
                                ProveedorId = proveedorId
                            });
                            db.SaveChanges();
                            txtNombre.Text = "";
                            txtPrecio.Text = "";
                            // actualizar la grilla
                            ConsultarArticulos(dgvArticulos);
                        }
                    }
                    else
                    {
                        MessageBox.Show("El precio debe ser un valor numérico mayor a cero.",
                       "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Debe completar los datos requeridos", "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0;
                if (txtArticuloId.Text != "")
                {
                    int.TryParse(txtArticuloId.Text, out id);
                    if (id != 0)
                    {
                        using (var db = new ModeloAdministracion())
                        {
                            // consultar si existe un articulo con ese identificador
                            var query = from a in db.Articulos
                                        where a.ArticuloId == id
                                        select a;
                            if (query.Any()) // si el resultado de la consulta existe
                            {
                                // primer articulo del resultado de la consulta
                                var art = query.First();
                                // pedir confirmación al usuario
                                if (MessageBox.Show("¿Confirma borrar el artículo?",
                                "Confirmación", MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    db.Remove(art); // borrar el artículo
                                    db.SaveChanges();
                                    ConsultarArticulos(dgvArticulos); // actualizar la grilla
                                }
                                txtArticuloId.Text = "";
                            }
                            if (txtArticuloId.Text != "")
                            {
                                MessageBox.Show("No existe el identificador", "Error",
                                MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe ingresar un identidicador numérico y mayor a cero",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Debe ingresar el identidicador", "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
