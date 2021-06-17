using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class Factura
    {
        public List<DetalleFactura> detalle_items { get; set; }
        public Distar_EntidadesNegocio.Usuario cliente { get; set; }
        public DateTime fecha_emision { get; set; }
        public int id_cliente { get; set; }
        public int id_factura { get; set; }
        public int iva { get; set; }
        public float total { get; set; }
        public int nro_factura { get; set; }
        public string tipo_factura { get; set; }
    }
}
