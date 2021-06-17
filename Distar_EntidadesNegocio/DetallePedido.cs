using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class DetallePedido
    {
        public string descripcion_prod { get; set; }
        public float p_unitario_prod { get; set; }
        public int cantidad { get; set; }
        public int id_detalle_pedido { get; set; }
        public int id_pedido { get; set; }
        public int id_producto { get; set; }
        public float importe { get; set; }
        public int nro_detalle { get; set; }
        public Distar_EntidadesNegocio.Producto producto { get; set; }
    }
}
