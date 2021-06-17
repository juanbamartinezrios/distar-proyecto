using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class Ticket
    {
        public int DVH { get; set; }
        public string estado { get; set; }
        public int id_ticket { get; set; }
        public int id_cliente { get; set; }
        public int nro_factura { get; set; }
        public int nro_ticket { get; set; }
        public DateTime fecha_emision { get; set; }
        public List<DetalleFactura> detalle_items { get; set; }
        public Distar_EntidadesNegocio.Usuario cliente { get; set; }
        public float total { get; set; }
    }
}
