using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SP3
{
    public class ModeloAdministracion : DbContext
    {
        // propiedades
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Articulo> Articulos { get; set; }

        // definición de la ruta para la ubicación de la base de datos
        public string DbPath { get; }
        // constructor
        public ModeloAdministracion()
        {
            // define la ubicación y el nombre del archivo con la base SQLite
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Application.StartupPath;
            DbPath = System.IO.Path.Join(path, "Administracion.db");
        }
        // configurar EF para crear la base SQLite en la carpeta "local" del 
         protected override void OnConfiguring(DbContextOptionsBuilder options)
         => options.UseSqlite($"Data Source={DbPath}");
    }

}
