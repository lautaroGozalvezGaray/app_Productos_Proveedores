using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP3
{
    public class Articulo
    {
        public int ArticuloId { get; set; }
        public string Nombre { get; set; }
        public Single Precio { get; set; }
        public int ProveedorId { get; set; }
    }

}
