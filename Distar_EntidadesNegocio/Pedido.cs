using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class Pedido
    {
        public string descripcion { get; set; }
        public string estado { get; set; }
        public DateTime fecha_creacion { get; set; }
        public int id_cliente { get; set; }
        public int id_pedido { get; set; }
        public int nro_pedido { get; set; }
        public float total { get; set; }
        public List<DetallePedido> detalle_items { get; set; }
        public Distar_EntidadesNegocio.Usuario cliente { get; set; }
    }
}
