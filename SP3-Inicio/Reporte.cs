using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP3
{

    internal class Reporte
    {
        // propiedades
        private String Ruta = "";
        private SaveFileDialog _dlg;

        // constructor
        public Reporte(SaveFileDialog dlg)
        {
            // configuración del diálogo
            dlg.Filter = "Archivos de texto (*.txt)|*.txt";
            dlg.FilterIndex = 0;
            dlg.Title = "Archivo del Reporte";
            dlg.RestoreDirectory = true;
            _dlg = dlg; // el objeto SaveDialog recibido por parámetro se guarda en la variable privada
            Ruta = "";
        }

        // métodos
        public String SeleccionarArchivo()
        {
            Ruta = "";
            // acá se usa el SaveDialog recibido por parámetro
            if(_dlg.ShowDialog() ==  DialogResult.OK )
            {   // si el usuario cerró el dialogo con el botón Ok
                Ruta = _dlg.FileName;
            }
            return Ruta;
        }

        public bool GenerarReporte(int proveedorId)
        {
            bool resultado = false;
            if (Ruta != "") // si se configuró el archivo
            {
                try
                {
                    // crear el archivo del reporte: se usa la ruta obtenida del SaveDialog
                    StreamWriter writer = new StreamWriter(Ruta);
                    // grabar el título del reporte
                    writer.WriteLine("REPORTE de ARTICULOS por PROVEEDOR");
                    writer.WriteLine("----------------------------------");
                    writer.WriteLine("");

                    using var db = new ModeloAdministracion();

                    // consultar los datos del proveedor seleccionado
                    var queryP = from p in db.Proveedores
                                    where p.ProveedorId == proveedorId
                                    select p.Nombre; // acá se obtiene solamente el campo "Nombre"
                    // grabar los datos del proveedor (Id y Nombre)
                    writer.WriteLine("PROVEEDOR: " + proveedorId.ToString() + ", " + queryP.First().ToString());
                    writer.WriteLine("");

                    // consultar los articulos del proveedor seleccionado
                    var queryA = from a in db.Articulos
                                    where a.ProveedorId == proveedorId
                                    orderby a.ArticuloId
                                    select a;

                    // recorrer los artículos obtenidos y grabarlos en el archivo
                    writer.WriteLine("ARTICULOS:");
                    writer.WriteLine("Identificador Nombre                        Precio");
                    foreach (var a in queryA)
                    {
                        writer.Write(a.ArticuloId.ToString().PadRight(14));
                        writer.Write(a.Nombre.PadRight(30));
                        writer.WriteLine(a.Precio.ToString().PadRight(12));
                    }
                    // cerrar el archivo
                    writer.Close();
                    writer.Dispose();
                    resultado = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return resultado;
        }
    }
}
