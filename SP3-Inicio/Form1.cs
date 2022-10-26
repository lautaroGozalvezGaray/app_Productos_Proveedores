namespace SP3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtNombre.Text = "";
            txtProveedorId.Text = "";
            ConsultarProveedores();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNombre.Text != "")
                {
                    using (var db = new ModeloAdministracion())
                    {
                        Proveedor p = new Proveedor(); // se crea un nuevo objeto Proveedor
                        p.Nombre = txtNombre.Text; // se asigna le nombre
                        db.Add(p); // se agrega a la tabla 
                        db.SaveChanges(); // se guardan los cambios sobre la base de datos
                        txtNombre.Text = "";
                    }
                    // actualizar la grilla
                    ConsultarProveedores();
                }
                else
                {
                    MessageBox.Show("Debe ingresar el nombre", "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ConsultarProveedores()
        {
            try
            {
                grProveedores.Rows.Clear(); // limpiar la grilla
                using (var db = new ModeloAdministracion())
                {
                    // consultar todos los proveedores de la base de datos
                    var consulta = from a in db.Proveedores // LINQ
                                   orderby a.ProveedorId
                                   select a;
                    // cargar la grilla
                    foreach (var p in consulta)
                    {
                        grProveedores.Rows.Add(p.ProveedorId.ToString(), p.Nombre.ToString());
                    }
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
                int id = int.Parse(txtProveedorId.Text);
                using (var db = new ModeloAdministracion())
                {
                    // consultar si existe un proveedor con ese identificador
                    var query = from a in db.Proveedores
                                where a.ProveedorId == id
                                select a;
                    if (query.Count() > 0) // si el resultado de la consulta existe
                    {
                        // obtener el primer proveedor del resultado de la consulta (sería el único)
                        var prov = query.First();
                        // pedir confirmación al usuario para borrar
                        if (MessageBox.Show("¿Confirma borrar el proveedor?",
                            "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                                DialogResult.Yes)
                        {
                            db.Remove(prov); // borrar el proveedor
                            db.SaveChanges(); // guardar los cambios en la base de datos
                            ConsultarProveedores(); // actualizar el contenido de la grilla
                        }
                        txtProveedorId.Text = "";
                    }
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