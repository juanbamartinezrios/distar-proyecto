using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class Producto
    {
        public string descripcion { get; set; }
        public int DVH { get; set; }
        public string estado { get; set; }
        public int id_producto { get; set; }
        public float p_unitario { get; set; }
        public int stock { get; set; }
    }
}
